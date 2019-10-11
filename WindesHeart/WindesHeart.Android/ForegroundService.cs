using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using WindesHeart.Helpers;
using WindesHeart.MiBand;
using WindesHeart.Model;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Device = Xamarin.Forms.Device;

namespace WindesHeart.Droid
{
    [Service]
	public class ForegroundService : Service
	{
		static readonly string Tag = typeof(ForegroundService).FullName;

		private bool _isStarted;

        private Notification.Builder _foregroundNotification;
        private NotificationManager _notificationManager;
        private System.Timers.Timer _fetchingTimer;

        private const string ChannelId = "channel_01";

        public override void OnCreate()
		{
			base.OnCreate();
			Log.Info(Tag, "OnCreate: the service is initializing.");

            _notificationManager = (NotificationManager) GetSystemService(NotificationService);

            if (Build.VERSION.SdkInt >= Build.VERSION_CODES.O)
            {
                var name = GetString(Resource.String.service_name);
                var mChannel = new NotificationChannel(ChannelId, name, NotificationImportance.Low);
                _notificationManager.CreateNotificationChannel(mChannel);
            }
		}

		public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
		{
			if (intent.Action.Equals(Constants.ACTION_START_SERVICE))
			{
				if (_isStarted)
				{
					Log.Info(Tag, "OnStartCommand: The service is already running.");
				}
				else 
				{
					Log.Info(Tag, "OnStartCommand: The service is starting.");
					RegisterForegroundService();
                    ListenForUpdates();
                    _isStarted = true;
				}
			}
			else if (intent.Action.Equals(Constants.ACTION_STOP_SERVICE))
			{
				Log.Info(Tag, "OnStartCommand: The service is stopping.");
				StopForeground(true);
				StopSelf();
				_isStarted = false;
			}

			// This tells Android not to restart the service if it is killed to reclaim resources.
			return StartCommandResult.NotSticky;
		}


		public override IBinder OnBind(Intent intent)
		{
			// Return null because this is a pure started service. A hybrid service would return a binder that would
			// allow access to the GetFormattedStamp() method.
			return null;
		}


		public override void OnDestroy()
		{
			// We need to shut things down.
			//Log.Debug(TAG, GetFormattedTimestamp() ?? "The TimeStamper has been disposed.");
			Log.Info(Tag, "OnDestroy: The started service is shutting down.");

			// Stop the handler.
			//handler.RemoveCallbacks(runnable);

			// Remove the notification from the status bar.
			var notificationManager = (NotificationManager)GetSystemService(NotificationService);
			notificationManager.Cancel(Constants.SERVICE_RUNNING_NOTIFICATION_ID);

			_isStarted = false;
			base.OnDestroy();
		}

		void RegisterForegroundService()
        {
            _foregroundNotification = new Notification.Builder(this, ChannelId)
                .SetContentTitle(Resources.GetString(Resource.String.service_name))
                .SetColor(Color.Red.ToAndroid())
                .SetContentText(App.MiBandDevice?.DeviceStatus)
                .SetSmallIcon(Resource.Drawable.ic_no_heartbeat)
                .SetContentIntent(BuildIntentToShowMainActivity())
                .SetOngoing(true)
                .AddAction(BuildStopServiceAction());
				

			// Enlist this instance of the service as a foreground service
			StartForeground(Constants.SERVICE_RUNNING_NOTIFICATION_ID, _foregroundNotification.Build());
		}

		/// <summary>
		/// Builds a PendingIntent that will display the main activity of the app. This is used when the 
		/// user taps on the notification; it will take them to the main activity of the app.
		/// </summary>
		/// <returns>The content intent.</returns>
		PendingIntent BuildIntentToShowMainActivity()
		{
			var notificationIntent = new Intent(this, typeof(MainActivity));
			notificationIntent.SetAction(Constants.ACTION_MAIN_ACTIVITY);
			notificationIntent.SetFlags(ActivityFlags.SingleTop | ActivityFlags.FromBackground);
			notificationIntent.PutExtra(Constants.SERVICE_STARTED_KEY, true);

			var pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent, PendingIntentFlags.UpdateCurrent);
			return pendingIntent;
		}

		/// <summary>
		/// Builds the Notification.Action that will allow the user to stop the service via the
		/// notification in the status bar
		/// </summary>
		/// <returns>The stop service action.</returns>
		Notification.Action BuildStopServiceAction()
		{
			var stopServiceIntent = new Intent(this, GetType());
			stopServiceIntent.SetAction(Constants.ACTION_STOP_SERVICE);
			var stopServicePendingIntent = PendingIntent.GetService(this, 0, stopServiceIntent, 0);

			var builder = new Notification.Action.Builder(Android.Resource.Drawable.IcMediaPause,
														  GetText(Resource.String.stop_service),
														  stopServicePendingIntent);
			return builder.Build();

		}

        void ListenForUpdates()
        {
            MessagingCenter.Subscribe(this, "DeviceStatus", (ServiceMessage message, bool isConnected) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    switch (isConnected)
                    {
                        case true:
                            _foregroundNotification.SetSmallIcon(Resource.Drawable.ic_heartbeat);
                            break;
                        case false:
                            _foregroundNotification.SetSmallIcon(Resource.Drawable.ic_no_heartbeat);
                            break;
                    }

                    _foregroundNotification.SetContentText(App.MiBandDevice.DeviceStatus);
                    _notificationManager.Notify(Constants.SERVICE_RUNNING_NOTIFICATION_ID,
                        _foregroundNotification.Build());

                });
            });

            MessagingCenter.Subscribe(this, "ResetCounter",
                (LongRunningTaskMessage message) => { Device.BeginInvokeOnMainThread(() =>
                {
                    _fetchingTimer?.Dispose();
                    _fetchingTimer = new System.Timers.Timer(1800000);
                    _fetchingTimer.Elapsed += (o, e) =>
                    {
                        Task.Run(() =>
                        {
                            App.MiBandDevice.StartFetching();
                        });
                    };
                    _fetchingTimer.Enabled = true;
                });
            });

        }
    }
}