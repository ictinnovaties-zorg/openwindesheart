using System;
using System.Collections.Generic;
using WindesHeartSDK;
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

        private async void ReadBattery(object sender, EventArgs e)
        {
            var battery = await Device.GetBattery();
            Console.WriteLine("Battery: " + battery.BatteryPercentage + "%");
        }

        private async void SetTime(object sender, EventArgs e)
        {
            bool timeset = await Device.SetTime(new DateTime(2000, 1, 1, 1, 1, 1));
            Console.WriteLine("Time set " + timeset);
        }
    }
}