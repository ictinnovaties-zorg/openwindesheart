using FormsControls.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using WindesHeartApp.Resources;
using WindesHeartApp.Services;
using WindesHeartSdk.Model;
using WindesHeartSDK;
using WindesHeartSDK.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WindesHeartApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestPage : ContentPage, IAnimationPage
    {
        public DateTime temptime = new DateTime(2019, 10, 10, 10, 1, 1);
        public string key = "LastConnectedDeviceGuid";
        public bool is24hour = true;

        public TestPage()
        {
            InitializeComponent();
            BuildPage();
        }

        private void BuildPage()
        {
            PageBuilder.BuildPageBasics(Layout, this);
            PageBuilder.AddHeaderImages(Layout);
            PageBuilder.AddLabel(Layout, "TEST", 0.05, 0.10, Globals.LightTextColor, "", 0);
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
                    device?.Connect(CallbackHandler.OnConnetionCallBack);
                }
                else
                {
                    bool isScanning = Windesheart.StartScanning(WhenDeviceFound);
                    if (!isScanning)
                    {
                        Trace.WriteLine("Can't start scanning... Bluetooth adapter not ready?");
                    }
                }
            }
            catch (Exception r)
            {
                Trace.WriteLine(r.Message);
            }
        }


        private void WhenDeviceFound(BLEDevice device)
        {
            Trace.WriteLine("Device found! Connecting...");
            Windesheart.StopScanning();
            device.Connect(CallbackHandler.OnConnetionCallBack);
        }

        private void Disconnect(object sender, EventArgs e)
        {
            if (Windesheart.ConnectedDevice != null)
                Windesheart.ConnectedDevice.Disconnect(false);
        }

        private async void ReadCurrentBattery(object sender, EventArgs e)
        {
            var battery = await Windesheart.ConnectedDevice.GetBattery();
            Trace.WriteLine("Battery: " + battery.BatteryPercentage + "%");
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
            Trace.WriteLine("Time set " + timeset);
        }

        private void SetCurrentTime(object sender, EventArgs e)
        {
            bool timeset = Windesheart.ConnectedDevice.SetTime(DateTime.Now);
            Trace.WriteLine("Time set " + timeset);
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
            Trace.WriteLine("Steps: " + steps.StepCount);
        }

        public void EnableRealTimeSteps(object sender, EventArgs e)
        {
            Trace.WriteLine("Enabled realtime steps");
        }

        public void DisableRealTimeSteps(object sender, EventArgs e)
        {
            Windesheart.ConnectedDevice.DisableRealTimeSteps();
            Trace.WriteLine("Disabled realtime steps");
        }

        public void OnStepsChanged(StepInfo steps)
        {
            Trace.WriteLine("Steps updated: " + steps.StepCount);
        }

        public void FetchData(object sender, EventArgs e)
        {
            Windesheart.ConnectedDevice.FetchData(DateTime.Now.AddDays(-2), HandleActivityData);
        }

        private void HandleActivityData(List<ActivitySample> samples)
        {
            Trace.WriteLine("Samples found! Here they come:");

            foreach (ActivitySample sample in samples)
            {
                Debug.WriteLine(sample.ToString());
            }
        }

        private void GoBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void Setln_Clicked(object sender, EventArgs e)
        {
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
        public IPageAnimation PageAnimation { get; } = new SlidePageAnimation { Duration = AnimationDuration.Short, Subtype = AnimationSubtype.FromTop };

        public void OnAnimationStarted(bool isPopAnimation)
        {
            // Put your code here but leaving empty works just fine
        }

        public void OnAnimationFinished(bool isPopAnimation)
        {
            // Put your code here but leaving empty works just fine
        }
    }
}
