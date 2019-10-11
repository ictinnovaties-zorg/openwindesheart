using System;
using System.ComponentModel;
using System.Reflection;
using System.Resources;
using WindesHeartSdk.MiBand;
using WindesHeartSdk.Model;
using WindesHeartSdk.Services;
using Plugin.BluetoothLE;
using WindesHeart.Helpers;
using WindesHeart.Model;
using WindesHeart.Pages;
using Xamarin.Forms;

namespace WindesHeart.MiBand
{
    public class MiBandDevice: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public DateTime? LastSyncTime;
        private readonly ResourceManager _rm = new ResourceManager("WindesHeart.Resources.AppResources", typeof(PairDevicePage).GetTypeInfo().Assembly);
        
        public string DeviceName { get; set; }

        private bool _isPaired;
        private bool _isSyncing;
        private bool _isConnected;
        private bool _isFetching;
        private string _deviceStatus;
        private string _batteryStatus;

        private IDisposable _pairDeviceSub;
        private IDisposable _batteryStatusSub;
        private IDisposable _connectionStatusSub;
        private IDisposable _fetchingStatusSub;

        private readonly FetchOperation _fetchOperation;
        
        public MiBandDevice()
        {
            _deviceStatus = _rm.GetString("MiBandDevice_DeviceStatus_Disconnected");
            _batteryStatus = "";
            DeviceName = "Mi Band 3";

            RestService.ApiUri = "http://insulinepredictionplatform.com:3000";
            RestService.DsuUri = "http://insulinepredictionplatform.com:8082/oauth/token";

            if (Application.Current.Properties.ContainsKey("device_id"))
            {
                IsPaired = true;
            }
            _fetchOperation = new FetchOperation();
            UpdateLastSyncDate();
            ListenForChanges();
        }

        public bool IsPaired
        {
            get => _isPaired;
            set
            {
                _isPaired = value;
                OnPropertyChanged("IsPaired");
            }
        }

        public bool IsConnected
        {
            get => _isConnected;
            set
            {
                _isConnected = value;
                OnPropertyChanged("IsConnected");
            }
        }

        public bool IsFetching
        {
            get => _isFetching;
            set
            {
                _isFetching = value;
                OnPropertyChanged("IsFetching");
            }
        }

        public bool IsSyncing
        {
            get => _isSyncing;
            set
            {
                _isSyncing = value;
                OnPropertyChanged("IsSyncing");
            }
        }

        public void Connect()
        {
            BleService.ConnectKnownDevice();
        }

        public void StartFetching()
        {
            // Reset counter for background fetching
            MessagingCenter.Send(new LongRunningTaskMessage(), "ResetCounter");

            if (!IsConnected || _isFetching)
            {
                return;
            }

            // Start fetching activity data
            IsFetching = true;
            _fetchingStatusSub?.Dispose();
            _fetchingStatusSub = _fetchOperation.InitiateFetching().Subscribe(async isFetching => {
                IsFetching = isFetching;

                // Send datapoints to server
                if (!isFetching)
                {
                    if (IsSyncing)
                    {
                        return;
                    }
                    IsSyncing = true;
                    OnPropertyChanged("LastSyncTimeString");
                    var sendDataPoints = await RestService.PushDataPoints();
                    IsSyncing = false;
                    UpdateLastSyncDate();
                    Console.WriteLine("Send {0} data points to api.", sendDataPoints);
                }
            });
        }

        public async void RemoveDevice()
        {
            // Remove stored device id
            Application.Current.Properties.Remove("device_id");
            await Application.Current.SavePropertiesAsync();
            IsPaired = false;

            // Disconnect device
            BleService.DisconnectDevice();

            // Remove data from local database
            await MiBandDb.Database.DeleteAllAsync();
        }

        //Update battery status
        private void BatteryChange(BatteryInfo info)
        {
            if (info.GetState() == BatteryState.BATTERY_CHARGING)
            {
                _batteryStatus = string.Format(_rm.GetString("MiBandDevice_BatteryStatus_Charging"), info.GetLevelInPercent());
            }
            else
            {
                _batteryStatus = string.Format(_rm.GetString("MiBandDevice_BatteryStatus_Normal"), info.GetLevelInPercent());
            }

            if (IsConnected)
            {
                MessagingCenter.Send(new ServiceMessage(), "DeviceStatus", true);
                OnPropertyChanged("DeviceStatus");
            }
        }

        private void ListenForChanges()
        {
            // Update connection status
            _connectionStatusSub?.Dispose();
            _connectionStatusSub = BleService.ConnectionStatusSubject.Subscribe(status =>
                {
                    switch (status)
                    {
                        case ConnectionStatus.Connecting:
                            IsConnected = false;
                            _deviceStatus = _rm.GetString("MiBandDevice_DeviceStatus_Connecting");
                            _batteryStatus = "";
                            break;
                        case ConnectionStatus.Disconnected:
                            IsConnected = false;
                            _deviceStatus = _rm.GetString("MiBandDevice_DeviceStatus_Disconnected");
                            _batteryStatus = "";
                            MessagingCenter.Send(new ServiceMessage(), "DeviceStatus", true);
                            break;
                    }
                    
                    OnPropertyChanged("DeviceStatus");
                });

            // Update pairing status
            _pairDeviceSub?.Dispose();
            _pairDeviceSub = BleService.PairResultSubject.Subscribe(async result =>
            {
                switch (result)
                {
                    case BleService.PairResult.Success:
                        Application.Current.Properties["device_id"] = BleService.KnownDeviceId;
                        await Application.Current.SavePropertiesAsync();
                        _deviceStatus = _rm.GetString("MiBandDevice_DeviceStatus_Connected");

                        _batteryStatusSub?.Dispose();
                        _batteryStatusSub = await MiBandSupport.OnBatteryStatusChange(BatteryChange);

                        IsConnected = true;
                        StartFetching();
                        break;
                    case BleService.PairResult.Configuring:
                        _deviceStatus = _rm.GetString("MiBandDevice_DeviceStatus_Configuring");
                        break;
                    case BleService.PairResult.Failed:
                        _deviceStatus = _rm.GetString("MiBandDevice_DeviceStatus_Disconnected");
                        break;
                }

                MessagingCenter.Send(new ServiceMessage(), "DeviceStatus", true);
                OnPropertyChanged("DeviceStatus");
            });
        }

        public string DeviceStatus => $"{_deviceStatus}{_batteryStatus}";

        public string LastSyncTimeString
        {
            //Get date/time from last locally stored datapoint
            get
            {
                if (_isSyncing)
                {
                    return string.Format(_rm.GetString("MiBandDevice_DeviceStatus_LastSyncTime"), _rm.GetString("MiBandDevice_DeviceStatus_Syncing"), "");
                }
                if (LastSyncTime != null)
                {
                    var lastSyncTime = (DateTime) LastSyncTime;
                    return string.Format(_rm.GetString("MiBandDevice_DeviceStatus_LastSyncTime"), lastSyncTime.ToShortDateString(), lastSyncTime.ToString("HH:mm"));
                }

                return _rm.GetString("MiBandDevice_DeviceStatus_LastSyncTimeUnknown");
            }
        }

        private async void UpdateLastSyncDate()
        {
            var lastSample = await MiBandDb.Database.GetLastSample();
            LastSyncTime = lastSample?.Timestamp.ToLocalTime();

            OnPropertyChanged("LastSyncTimeString");
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
