using System;
using System.Collections.Generic;
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
        WindesHeartSDK.Device Device = null;
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            List<WindesHeartSDK.Device> devices = await Windesheart.ScanForDevices();
            if (devices.Count > 0)
            {
                Device = devices[0];
                //BluetoothService.ConnectDevice(BluetoothService.ScanResults[0].Device);
            }
        }

        private async void Connect(object sender, EventArgs e)
        {
            bool connected = await Device.Connect();
            Console.WriteLine("Connected:" + connected);
        }

        private async void Disconnect(object sender, EventArgs e)
        {
            bool disconnected = await Device.Disconnect();
            Console.WriteLine("Disconnected:" + disconnected);
        }

        private async void ReadCurrentBattery(object sender, EventArgs e)
        {
            var battery = await Device.GetBattery();
            Console.WriteLine("Battery: " + battery.BatteryPercentage + "%");
        }

        private async void SetTime(object sender, EventArgs e)
        {
            bool timeset = await Device.SetTime(new DateTime(2000, 1, 1, 1, 1, 1));
            Console.WriteLine("Time set " + timeset);

        }

        private async void GetBattery()
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

        private void GetBatteryStatus(Battery battery)
        {
            Console.WriteLine("Batterypercentage is now: " + battery.BatteryPercentage + "% || Batterystatus is: " + battery.Status);
        }
    }
}