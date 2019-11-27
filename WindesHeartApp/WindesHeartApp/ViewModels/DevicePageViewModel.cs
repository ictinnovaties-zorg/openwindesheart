using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WindesHeartApp.Resources;
using WindesHeartSDK;
using WindesHeartSDK.Devices.MiBand3.Models;
using WindesHeartSDK.Models;
using Xamarin.Forms;

namespace WindesHeartApp.ViewModels
{
    public class DevicePageViewModel
    {
        private BLEDevice _selectedDevice;
        private int _heartrateInterval;
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<BLEDevice> deviceList;
        public Command scanButtonCommand { get; }
        public Command disconnectButtonCommand { get; }

        public DevicePageViewModel()
        {
            scanButtonCommand = new Command(scanButtonClicked);
            disconnectButtonCommand = new Command(disconnectButtonClicked);
            if (DeviceList == null)
                DeviceList = new ObservableCollection<BLEDevice>();
            _heartrateInterval = Globals.heartrateInterval;
        }

        private void disconnectButtonClicked()
        {
            Globals.device.Disconnect();
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

        public BLEDevice SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                _selectedDevice = value;

                if (_selectedDevice == null)
                    return;

                deviceSelected(_selectedDevice);
                _selectedDevice = null;
            }
        }

        private void deviceSelected(BLEDevice device)
        {
            Console.WriteLine(device.Name);
            //try
            //{
            //    device.Connect();
            // Globals.device.EnableRealTimeBattery(CallbackHandler.ChangeBattery);
            // Globals.device = device;
            //ReadCurrentBattery();
            //SetCurrentTime();
            //Globals.device.SetHeartrateMeasurementInterval(_heartrateInterval);
            //Globals.device.EnableRealTimeHeartrate(CallbackHandler.ChangeHeartRate);
            //Globals.device.EnableRealTimeBattery(CallbackHandler.ChangeBattery);
            //Globals.device.EnableRealTimeSteps(CallbackHandler.OnStepsUpdated);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}
        }

        private /*async*/ void scanButtonClicked()
        {
            var band = new MiBand3();
            band.Name = "DANIEL";
            band.Rssi = 420;
            DeviceList.Add(band);

            //try
            //{
            //    var devices = await Windesheart.ScanForDevices();
            //    if (devices != null)
            //        Globals.device = devices[0];
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}
        }

        private void SetCurrentTime()
        {
            bool timeset = Windesheart.ConnectedDevice.SetTime(DateTime.Now);
        }
        private async void ReadCurrentBattery()
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
