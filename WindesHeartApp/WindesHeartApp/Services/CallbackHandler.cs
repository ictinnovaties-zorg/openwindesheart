using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WindesHeartApp.Models;
using WindesHeartApp.Resources;
using WindesHeartSDK;
using WindesHeartSDK.Models;
using Xamarin.Forms;

namespace WindesHeartApp.Services
{
    public static class CallbackHandler
    {
        private static readonly string _key = "LastConnectedDeviceGuid";

        public static void OnHeartrateUpdated(WindesHeartSDK.Models.Heartrate heartrate)
        {
            if (heartrate.HeartrateValue == 0)
                return;
            Globals.HomePageViewModel.Heartrate = heartrate.HeartrateValue;
        }

        public static void OnBatteryUpdated(Battery battery)
        {
            Globals.HomePageViewModel.UpdateBattery(battery);
        }

        public static void OnStepsUpdated(StepInfo stepsInfo)
        {
            var count = stepsInfo.StepCount;
            Debug.WriteLine($"Stepcount updated: {count}");
            Globals.StepsPageViewModel.OnStepsUpdated(count);

        }

        public static void OnConnect(ConnectionResult result)
        {
            if (result == ConnectionResult.Succeeded)
            {
                try
                {
                    //Sync settings
                    Windesheart.ConnectedDevice.SetTime(DateTime.Now);
                    Windesheart.ConnectedDevice.SetDateDisplayFormat(DeviceSettings.DateFormatDMY);
                    Windesheart.ConnectedDevice.SetLanguage(DeviceSettings.DeviceLanguage);
                    Windesheart.ConnectedDevice.SetTimeDisplayFormat(DeviceSettings.TimeFormat24Hour);
                    Windesheart.ConnectedDevice.SetActivateOnLiftWrist(DeviceSettings.WristRaiseDisplay);
                    Windesheart.ConnectedDevice.SetFitnessGoal(DeviceSettings.DailyStepsGoal);
                    Windesheart.ConnectedDevice.EnableFitnessGoalNotification(true);
                    Windesheart.ConnectedDevice.EnableSleepTracking(true);
                    Windesheart.ConnectedDevice.SetHeartrateMeasurementInterval(1);

                    //Callbacks
                    Windesheart.ConnectedDevice.EnableRealTimeHeartrate(OnHeartrateUpdated);
                    Windesheart.ConnectedDevice.EnableRealTimeBattery(OnBatteryUpdated);
                    Windesheart.ConnectedDevice.EnableRealTimeSteps(OnStepsUpdated);
                    Windesheart.ConnectedDevice.SubscribeToDisconnect(OnDisconnect);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    Debug.WriteLine("Something went wrong while connecting to device, disconnecting...");
                    Windesheart.ConnectedDevice.Disconnect();
                    Globals.DevicePageViewModel.IsLoading = false;
                }

                Globals.DevicePageViewModel.StatusText = "Connected";
                Globals.DevicePageViewModel.DeviceList = new ObservableCollection<BLEScanResult>();
                Globals.DevicePageViewModel.IsLoading = false;
                Device.BeginInvokeOnMainThread(delegate { Application.Current.MainPage.Navigation.PopAsync(); });
                Globals.SamplesService.StartFetching();


                if (Windesheart.ConnectedDevice.Uuid != Guid.Empty)
                {
                    if (App.Current.Properties.ContainsKey(_key))
                    {
                        App.Current.Properties.Remove(_key);
                    }

                    App.Current.Properties.Add(_key, Windesheart.ConnectedDevice.Uuid);
                }
            }
            else if (result == ConnectionResult.Failed)
            {
                Debug.WriteLine("FAIL");
                return;
            }
        }

        public static void OnDisconnect(Object obj)
        {
            Globals.DevicePageViewModel.StatusText = "Disconnected";
        }
    }
}