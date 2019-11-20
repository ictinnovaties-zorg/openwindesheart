using System;
using System.Collections.Generic;
using System.Diagnostics;
using WindesHeartApp.Resources;
using WindesHeartApp.Services;
using WindesHeartSDK;
using WindesHeartSDK.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WindesHeartApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestPage : ContentPage
    {
        public bool is24hour = true;

        public TestPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            BuildPage();
        }

        private void BuildPage()
        {
            PageBuilder.BuildPageBasics(Layout, this);
            PageBuilder.AddHeaderImages(Layout);
            PageBuilder.AddLabel(Layout, "TEST", 0.05, 0.10);
            PageBuilder.AddReturnButton(Layout, this);
            PageBuilder.AddReturnButton(Layout, this);

            AbsoluteLayout.SetLayoutBounds(scanButton, new Rectangle(0.05, 0.2, 0.5, 0.05));
            AbsoluteLayout.SetLayoutFlags(scanButton, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(disconnButton, new Rectangle(0.05, 0.25, 0.5, 0.05));
            AbsoluteLayout.SetLayoutFlags(disconnButton, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(getHeartrateButton, new Rectangle(0.05, 0.30, 0.5, 0.05));
            AbsoluteLayout.SetLayoutFlags(getHeartrateButton, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(setcurrenttimeButton, new Rectangle(0.05, 0.35, 0.5, 0.05));
            AbsoluteLayout.SetLayoutFlags(setcurrenttimeButton, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(settimeButton, new Rectangle(0.05, 0.40, 0.50, 0.05));
            AbsoluteLayout.SetLayoutFlags(settimeButton, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(readBattContin, new Rectangle(0.05, 0.45, 0.50, 0.05));
            AbsoluteLayout.SetLayoutFlags(readBattContin, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(readBattCurrent, new Rectangle(0.05, 0.50, 0.50, 0.05));
            AbsoluteLayout.SetLayoutFlags(readBattCurrent, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(stepsButton, new Rectangle(0.05, 0.55, 0.50, 0.05));
            AbsoluteLayout.SetLayoutFlags(stepsButton, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(realtimestepsButton, new Rectangle(0.05, 0.60, 0.5, 0.05));
            AbsoluteLayout.SetLayoutFlags(realtimestepsButton, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(disablerealtimestepsButton, new Rectangle(0.05, 0.65, 0.5, 0.05));
            AbsoluteLayout.SetLayoutFlags(disablerealtimestepsButton, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(Setln, new Rectangle(0.05, 0.70, 0.5, 0.05));
            AbsoluteLayout.SetLayoutFlags(Setln, AbsoluteLayoutFlags.All);
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
            Globals.device.EnableRealTimeBattery(CallbackHandler.ChangeBattery);
        }
        private void GetBatteryStatus(Battery battery)
        {

        }

        public void GetHeartRate_Clicked(object sender, EventArgs e)
        {
            Globals.device.SetHeartrateMeasurementInterval(1);
            Globals.device.EnableRealTimeHeartrate(CallbackHandler.ChangeHeartRate);
        }

        public async void GetSteps(object sender, EventArgs e)
        {
            StepInfo steps = await Globals.device.GetSteps();
            Console.WriteLine("Steps: " + steps.StepCount);
        }

        public void EnableRealTimeSteps(object sender, EventArgs e)
        {
            Globals.device.EnableRealTimeSteps(OnStepsChanged);
            Console.WriteLine("Enabled realtime steps");
        }

        public void DisableRealTimeSteps(object sender, EventArgs e)
        {
            Globals.device.DisableRealTimeSteps();
            Console.WriteLine("Disabled realtime steps");
        }

        public void OnStepsChanged(StepInfo steps)
        {
            Console.WriteLine("Steps updated: " + steps.StepCount);
        }

        private void GoBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void Setln_Clicked(object sender, EventArgs e)
        {
            Globals.device.SetDateDisplayFormat(is24hour);
            Globals.device.SetTimeDisplayUnit(is24hour);
            Globals.device.SetActivateOnLiftWrist(is24hour);
            if (is24hour)
            {
                Globals.device.SetLanguage("nl-NL");
            }
            else
            {
                Globals.device.SetLanguage("en-EN");
            }
            is24hour = !is24hour;
        }
    }
}
