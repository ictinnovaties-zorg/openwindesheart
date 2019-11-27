using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Collections.ObjectModel;
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
        public DateTime temptime = new DateTime(2019, 10, 10, 10, 1, 1);
        public string key = "LastConnectedDeviceGuid";
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
            PageBuilder.AddLabel(Layout, "TEST", 0.05, 0.10, Globals.lighttextColor);
            PageBuilder.AddReturnButton(Layout, this);
            PageBuilder.AddReturnButton(Layout, this);

            AbsoluteLayout.SetLayoutBounds(scanButton, new Rectangle(0.05, 0.2, 0.5, 0.07));
            AbsoluteLayout.SetLayoutFlags(scanButton, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(disconnButton, new Rectangle(0.05, 0.25, 0.5, 0.07));
            AbsoluteLayout.SetLayoutFlags(disconnButton, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(getHeartrateButton, new Rectangle(0.05, 0.30, 0.5, 0.07));
            AbsoluteLayout.SetLayoutFlags(getHeartrateButton, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(setcurrenttimeButton, new Rectangle(0.05, 0.35, 0.5, 0.07));
            AbsoluteLayout.SetLayoutFlags(setcurrenttimeButton, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(settimeButton, new Rectangle(0.05, 0.40, 0.50, 0.07));
            AbsoluteLayout.SetLayoutFlags(settimeButton, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(readBattContin, new Rectangle(0.05, 0.45, 0.50, 0.07));
            AbsoluteLayout.SetLayoutFlags(readBattContin, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(readBattCurrent, new Rectangle(0.05, 0.50, 0.50, 0.07));
            AbsoluteLayout.SetLayoutFlags(readBattCurrent, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(stepsButton, new Rectangle(0.05, 0.55, 0.50, 0.07));
            AbsoluteLayout.SetLayoutFlags(stepsButton, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(realtimestepsButton, new Rectangle(0.05, 0.60, 0.5, 0.07));
            AbsoluteLayout.SetLayoutFlags(realtimestepsButton, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(disablerealtimestepsButton, new Rectangle(0.05, 0.65, 0.5, 0.07));
            AbsoluteLayout.SetLayoutFlags(disablerealtimestepsButton, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(Setln, new Rectangle(0.05, 0.70, 0.5, 0.07));
            AbsoluteLayout.SetLayoutFlags(Setln, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(fetchButton, new Rectangle(0.05, 0.9, 0.7, 0.07));
            AbsoluteLayout.SetLayoutFlags(fetchButton, AbsoluteLayoutFlags.All);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (App.Current.Properties.ContainsKey(key))
                {
                    App.Current.Properties.TryGetValue(key, out object result);
                    var device = await Windesheart.GetKnownDevice((Guid)result);
                    device?.Connect();
                }
                else
                {
                    ObservableCollection<BLEDevice> bleDevices = await Windesheart.ScanForDevices();
                    if (bleDevices.Count > 0)
                    {
                        bleDevices[0].Connect();
                        SaveDeviceInAppProperties(bleDevices[0].Device.Uuid);
                    }
                }
            }
            catch (Exception r)
            {
                Console.WriteLine(r.Message);
            }
        }

        private void Disconnect(object sender, EventArgs e)
        {
            Windesheart.ConnectedDevice.Disconnect();
        }

        private async void ReadCurrentBattery(object sender, EventArgs e)
        {
            var battery = await Windesheart.ConnectedDevice.GetBattery();
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

        private void SetTime(object sender, EventArgs e)
        {
            bool timeset = Windesheart.ConnectedDevice.SetTime(new DateTime(2000, 1, 1, 1, 1, 1));
            Console.WriteLine("Time set " + timeset);
        }

        private void SetCurrentTime(object sender, EventArgs e)
        {
            bool timeset = Windesheart.ConnectedDevice.SetTime(DateTime.Now);
            Console.WriteLine("Time set " + timeset);
        }

        private void ReadBatteryContinuous(object sender, EventArgs e)
        {
            Windesheart.ConnectedDevice.EnableRealTimeBattery(CallbackHandler.ChangeBattery);
        }

        public void GetHeartRate_Clicked(object sender, EventArgs e)
        {
            Windesheart.ConnectedDevice.SetHeartrateMeasurementInterval(1);
            Windesheart.ConnectedDevice.EnableRealTimeHeartrate(CallbackHandler.ChangeHeartRate);
        }

        public async void GetSteps(object sender, EventArgs e)
        {
            StepInfo steps = await Windesheart.ConnectedDevice.GetSteps();
            Console.WriteLine("Steps: " + steps.StepCount);
        }

        public void EnableRealTimeSteps(object sender, EventArgs e)
        {
            Windesheart.ConnectedDevice.EnableRealTimeSteps(OnStepsChanged);
            Console.WriteLine("Enabled realtime steps");
        }

        public void DisableRealTimeSteps(object sender, EventArgs e)
        {
            Windesheart.ConnectedDevice.DisableRealTimeSteps();
            Console.WriteLine("Disabled realtime steps");
        }

        public void OnStepsChanged(StepInfo steps)
        {
            Console.WriteLine("Steps updated: " + steps.StepCount);
        }

        public void FetchData(object sender, EventArgs e)
        {
            Windesheart.ConnectedDevice.FetchData();
        }

        private void GoBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void Setln_Clicked(object sender, EventArgs e)
        {
            Windesheart.ConnectedDevice.EnableSleepTracking(true);
            Windesheart.ConnectedDevice.SetDateDisplayFormat(is24hour);
            Windesheart.ConnectedDevice.SetTimeDisplayUnit(is24hour);
            Windesheart.ConnectedDevice.SetActivateOnLiftWrist(is24hour);
            if (is24hour)
            {
                Windesheart.ConnectedDevice.SetLanguage("nl-NL");
            }
            else
            {
                Windesheart.ConnectedDevice.SetLanguage("en-EN");
            }
            is24hour = !is24hour;
        }
        private void SaveDeviceInAppProperties(Guid guid)
        {
            if (guid != Guid.Empty)
            {
                if (App.Current.Properties.ContainsKey(key))
                {
                    App.Current.Properties.Remove(key);
                }

                App.Current.Properties.Add(key, guid);
            }
        }
    }
}
