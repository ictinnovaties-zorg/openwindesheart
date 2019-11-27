using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WindesHeartApp.Pages;
using WindesHeartApp.Resources;
using WindesHeartApp.Services;
using WindesHeartSDK;
using WindesHeartSDK.Models;
using Xamarin.Forms;

namespace WindesHeartApp.ViewModels
{
    public class DevicePageViewModel : INotifyPropertyChanged
    {
        private BLEDevice _selectedDevice;
        private int _heartrateInterval;
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<BLEDevice> deviceList;
        private bool _isLoading;
        private string _statusText;
        public Command scanButtonCommand { get; }
        public Command disconnectButtonCommand { get; }

        public DevicePageViewModel()
        {
            scanButtonCommand = new Command(scanButtonClicked);
            disconnectButtonCommand = new Command(disconnectButtonClicked);
            if (DeviceList == null)
                DeviceList = new ObservableCollection<BLEDevice>();
            _heartrateInterval = Globals.heartrateInterval;
            if (Globals.device == null)
                StatusText = "Disconnected";
        }

        private async void disconnectButtonClicked()
        {
            IsLoading = true;
            Globals.device.Disconnect();
            await Task.Delay(500);
            IsLoading = false;
            StatusText = "Disconnected";
        }

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ObservableCollection<BLEDevice> DeviceList
        {
            get { return deviceList; }
            set
            {
                deviceList = value;
                OnPropertyChanged();
            }
        }

        public string StatusText
        {
            get { return _statusText; }
            set
            {
                _statusText = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public BLEDevice SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                _selectedDevice = value;

                if (_selectedDevice == null)
                    return;
                OnPropertyChanged();
                deviceSelected(_selectedDevice);
                DevicePage.devicelist.SelectedItem = null;
                _selectedDevice = null;

            }
        }
        private async void scanButtonClicked()
        {
            try
            {
                StatusText = "Scanning for devices";
                IsLoading = true;
                var devices = await Windesheart.ScanForDevices();
                if (devices != null)
                {
                    foreach (var device in devices)
                    {
                        DeviceList.Add(device);
                    }
                }

                StatusText = $"Results found: {devices.Count}";
                IsLoading = false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private async void deviceSelected(BLEDevice device)
        {

            try
            {
                StatusText = $"Connecting to {device.Name}";
                IsLoading = true;
                device.Connect();
                Globals.device = device;

                // NEED TO FIX THIS
                await Task.Delay(5000);

                await ReadCurrentBattery();
                await Globals.device.SetTime(DateTime.Now);

                Globals.device.EnableRealTimeBattery(CallbackHandler.ChangeBattery);
                Globals.device.SetHeartrateMeasurementInterval(_heartrateInterval);
                Globals.device.EnableRealTimeHeartrate(CallbackHandler.ChangeHeartRate);
                Globals.device.EnableRealTimeBattery(CallbackHandler.ChangeBattery);
                Globals.device.EnableRealTimeSteps(CallbackHandler.OnStepsUpdated);
                DeviceList = new ObservableCollection<BLEDevice>();
                StatusText = "Connected";
                IsLoading = false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }



        private async Task ReadCurrentBattery()
        {
            var battery = await Globals.device.GetBattery();
            Console.WriteLine("Battery: " + battery.BatteryPercentage + "%");
            Globals.homepageviewModel.Battery = battery.BatteryPercentage;
            if (battery.Status == StatusEnum.Charging)
            {
                Globals.homepageviewModel.BatteryImage = "BatteryCharging.png";
                return;
            }
            if (battery.BatteryPercentage >= 0 && battery.BatteryPercentage < 26)
            {
                Globals.homepageviewModel.BatteryImage = "BatteryQuart.png";
            }
            else if (battery.BatteryPercentage >= 26 && battery.BatteryPercentage < 51)
            {
                Globals.homepageviewModel.BatteryImage = "BatteryHalf.png";
            }
            else if (battery.BatteryPercentage >= 51 && battery.BatteryPercentage < 76)
            {
                Globals.homepageviewModel.BatteryImage = "BatteryThreeQuarts.png";
            }
            else if (battery.BatteryPercentage >= 76)
            {
                Globals.homepageviewModel.BatteryImage = "BatteryFull.png";
            }
        }

    }
}
