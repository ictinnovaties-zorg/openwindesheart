﻿using System;
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
        public DateTime temptime = new DateTime(2019, 10, 10, 10, 1, 1);
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
            PageBuilder.BuildAndAddHeaderImages(Layout);
            PageBuilder.BuildAndAddLabel(Layout, "TEST", 0.05, 0.7);
            PageBuilder.BuildAndAddReturnButton(Layout, this);
            PageBuilder.BuildAndAddReturnButton(Layout, this);

            AbsoluteLayout.SetLayoutBounds(scanButton, new Rectangle(0.05, 0.2, 0.7, 0.07));
            AbsoluteLayout.SetLayoutFlags(scanButton, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(disconnButton, new Rectangle(0.05, 0.27, 0.7, 0.07));
            AbsoluteLayout.SetLayoutFlags(disconnButton, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(getHeartrateButton, new Rectangle(0.05, 0.34, 0.7, 0.07));
            AbsoluteLayout.SetLayoutFlags(getHeartrateButton, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(setcurrenttimeButton, new Rectangle(0.05, 0.41, 0.7, 0.07));
            AbsoluteLayout.SetLayoutFlags(setcurrenttimeButton, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(settimeButton, new Rectangle(0.05, 0.48, 0.7, 0.07));
            AbsoluteLayout.SetLayoutFlags(settimeButton, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(readBattContin, new Rectangle(0.05, 0.55, 0.7, 0.07));
            AbsoluteLayout.SetLayoutFlags(readBattContin, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(readBattCurrent, new Rectangle(0.05, 0.62, 0.7, 0.07));
            AbsoluteLayout.SetLayoutFlags(readBattCurrent, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(stepsButton, new Rectangle(0.05, 0.69, 0.7, 0.07));
            AbsoluteLayout.SetLayoutFlags(stepsButton, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(realtimestepsButton, new Rectangle(0.05, 0.76, 0.7, 0.07));
            AbsoluteLayout.SetLayoutFlags(realtimestepsButton, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(disablerealtimestepsButton, new Rectangle(0.05, 0.83, 0.7, 0.07));
            AbsoluteLayout.SetLayoutFlags(disablerealtimestepsButton, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(fetchButton, new Rectangle(0.05, 0.9, 0.7, 0.07));
            AbsoluteLayout.SetLayoutFlags(fetchButton, AbsoluteLayoutFlags.All);
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
            temptime = temptime.AddHours(2);
            bool timeset = await Globals.device.SetTime(temptime);
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
        }

        public void GetHeartrate(Heartrate heartrate)
        {
            Console.WriteLine(heartrate.HeartrateValue);
            Globals.heartRate = heartrate.HeartrateValue;
        }


        public void GetHeartRate_Clicked(object sender, EventArgs e)
        {
            Globals.device.SetHeartrateMeasurementInterval(1);
            Globals.device.EnableRealTimeHeartrate(GetHeartrate);
        }

        public async void GetSteps(object sender, EventArgs e)
        {
            StepInfo steps = await Globals.device.GetSteps();
            Console.WriteLine("Steps: " + steps.GetStepCount());
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
            Console.WriteLine("Steps updated: " + steps.GetStepCount());
        }

        public void FetchData(object sender, EventArgs e)
        {
            Globals.device.FetchData();
        }

        private void GoBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
