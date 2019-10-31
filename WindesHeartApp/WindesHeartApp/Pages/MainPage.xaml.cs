using System;
using WindesHeartSDK;
using WindesHeartSDK.Devices.MiBand3.Services;
using WindesHeartSDK.Exceptions;
using WindesHeartSDK.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WindesHeartApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await BluetoothService.ScanForUniqueDevicesAsync();
            if (BluetoothService.ScanResults.Count > 0 && BluetoothService.ScanResults[0] != null)
            {
                BluetoothService.ConnectDevice(BluetoothService.ScanResults[0].Device);
            }
        }

        private async void Connect(object sender, EventArgs e)
        {
            if (BluetoothService.ScanResults.Count > 0 && BluetoothService.ScanResults[0] != null)
            {
                BluetoothService.ConnectDevice(BluetoothService.ScanResults[0].Device);
            }
        }

        private async void Disconnect(object sender, EventArgs e)
        {
            BluetoothService.DisconnectDevice(BluetoothService.ConnectedDevice);
        }

        private async void ReadCurrentBattery(object sender, EventArgs e)
        {
            var connectedDevice = BluetoothService.ConnectedDevice;
            if (connectedDevice != null)
            {
                try
                {
                    var rawBattery = await BatteryService.GetRawBatteryDataAsync();
                    var battery = await BatteryService.GetCurrentBatteryDataAsync();
                    Console.WriteLine("Battery: " + battery.BatteryPercentage + "%");
                    Console.WriteLine("Batterystatus: " + battery.Status);
                }
                catch (BatteryException exception)
                {
                    Console.WriteLine(exception);
                }
            }
            else
            {
                Console.WriteLine("There is no connected device.");
            }
        }

        private async void ReadBatteryContinuous(object sender, EventArgs e)
        {
            var connectedDevice = BluetoothService.ConnectedDevice;
            if (connectedDevice != null)
            {
                try
                {
                    BatteryService.EnableBatteryStatusUpdates(GetBatteryStatus);
                }
                catch (BatteryException exception)
                {
                    Console.WriteLine(exception);
                }
            }
            else
            {
                Console.WriteLine("There is no connected device.");
            }
        }

        private void SetTime(object sender, EventArgs e)
        {
            DateTimeService.SetTime(DateTime.Now);
            MiBand3HeartrateService.SetMeasurementInterval(1);
            MiBand3HeartrateService.EnableHeartrateUpdates(HeartrateCallback);
        }

        private void GetBatteryStatus(Battery battery)
        {
            Console.WriteLine("Batterypercentage is now: " + battery.BatteryPercentage + "% || Batterystatus is: " + battery.Status);

        }

        private void HeartrateCallback(Heartrate heartrate)
        {
            Console.WriteLine(heartrate.rawdata);
            Console.WriteLine("Hi");
        }

        public void GetHeartRate(object sender, EventArgs e)
        {
            MiBand3HeartrateService.GetHeartrate();
        }
    }
}