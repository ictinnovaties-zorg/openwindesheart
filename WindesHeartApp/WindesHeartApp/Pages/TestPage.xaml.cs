using System;
using System.Collections.Generic;
using System.Diagnostics;
using WindesHeartApp.Resources;
using WindesHeartSDK;
using WindesHeartSDK.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WindesHeartApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestPage : ContentPage
    {
        public TestPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            Layout.BackgroundColor = Globals.primaryColor;
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                List<BLEDevice> devices = await Windesheart.ScanForDevices();
                if (devices.Count > 0)
                {
                    Globals.device = devices[0];
                    Globals.device.Connect();
                }
            }
            catch (Exception r)
            {
                Console.WriteLine(r.Message);
                Debug.WriteLine(" RAMONS - DEBUG.CW FEESTJE");
            }
        }

        private async void Disconnect(object sender, EventArgs e)
        {
            Globals.device.Disconnect();
        }

        private async void ReadCurrentBattery(object sender, EventArgs e)
        {
            var battery = await Globals.device.GetBattery();
            Console.WriteLine("Battery: " + battery.BatteryPercentage + "%");
            Globals.batteryPercentage = battery.BatteryPercentage;
            HomePage.batteryLabel.Text = $"Battery level: {battery.BatteryPercentage}";
        }

        private async void SetTime(object sender, EventArgs e)
        {
            bool timeset = await Globals.device.SetTime(new DateTime(2000, 1, 1, 1, 1, 1));
            Console.WriteLine("Time set " + timeset);
        }

        private async void SetCurrentTime(object sender, EventArgs e)
        {
            bool timeset = await Globals.device.SetTime(DateTime.Now);
            Console.WriteLine("Time set " + timeset);
        }

        private async void ReadBatteryContinuous(object sender, EventArgs e)
        {
            Globals.device.EnableRealTimeBattery(GetBatteryStatus);
        }

        private void GetBatteryStatus(Battery battery)
        {
            Console.WriteLine("Batterypercentage is now: " + battery.BatteryPercentage + "% || Batterystatus is: " + battery.Status);
            Globals.batteryPercentage = battery.BatteryPercentage;
            HomePage.batteryLabel.Text = $"Battery level: {battery.BatteryPercentage}";
        }

        public void GetHeartrate(Heartrate heartrate)
        {
            Console.WriteLine(heartrate.HeartrateValue);
            Device.BeginInvokeOnMainThread(delegate { HomePage.updateHeartrateLabel(heartrate.HeartrateValue); });


        }


        public void GetHeartRate_Clicked(object sender, EventArgs e)
        {
            Globals.device.SetHeartrateMeasurementInterval(1);
            Globals.device.EnableRealTimeHeartrate(GetHeartrate);
        }

        private void GoBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
