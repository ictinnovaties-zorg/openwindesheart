using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WindesHeartApp.Models;
using WindesHeartApp.Resources;
using WindesHeartSDK;
using WindesHeartSDK.Models;
using Xamarin.Forms;

namespace WindesHeartApp.Services
{
    public static class CallbackHandler
    {
        private static readonly string _key = "LastConnectedDeviceGuid";

        public static void OnHeartrateUpdated(WindesHeartSDK.Models.HeartrateData heartrate)
        {
            if (heartrate.Heartrate == 0)
                return;
            Globals.HomePageViewModel.Heartrate = heartrate.Heartrate;
        }

        public static void OnBatteryUpdated(BatteryData battery)
        {
            Globals.HomePageViewModel.UpdateBattery(battery);
        }

        public static void OnStepsUpdated(StepData stepsInfo)
        {
            var count = stepsInfo.StepCount;
            Debug.WriteLine($"Stepcount updated: {count}");
            Globals.StepsPageViewModel.OnStepsUpdated(count);

        }

        public static void OnConnect(ConnectionResult result)
        {
            if (result == ConnectionResult.Succeeded)
            {
                try
                {
                    //Sync settings
                    Windesheart.PairedDevice.SetTime(DateTime.Now);
                    Windesheart.PairedDevice.SetDateDisplayFormat(DeviceSettings.DateFormatDMY);
                    Windesheart.PairedDevice.SetLanguage(DeviceSettings.DeviceLanguage);
                    Windesheart.PairedDevice.SetTimeDisplayFormat(DeviceSettings.TimeFormat24Hour);
                    Windesheart.PairedDevice.SetActivateOnLiftWrist(DeviceSettings.WristRaiseDisplay);
                    Windesheart.PairedDevice.SetStepGoal(DeviceSettings.DailyStepsGoal);
                    Windesheart.PairedDevice.EnableFitnessGoalNotification(true);
                    Windesheart.PairedDevice.EnableSleepTracking(true);
                    Windesheart.PairedDevice.SetHeartrateMeasurementInterval(1);

                    //Callbacks
                    Windesheart.PairedDevice.EnableRealTimeHeartrate(OnHeartrateUpdated);
                    Windesheart.PairedDevice.EnableRealTimeBattery(OnBatteryUpdated);
                    Windesheart.PairedDevice.EnableRealTimeSteps(OnStepsUpdated);
                    Windesheart.PairedDevice.SubscribeToDisconnect(OnDisconnect);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    Debug.WriteLine("Something went wrong while connecting to device, disconnecting...");
                    Windesheart.PairedDevice.Disconnect();
                    Globals.DevicePageViewModel.IsLoading = false;
                }

                Globals.DevicePageViewModel.StatusText = "Connected";
                Globals.DevicePageViewModel.DeviceList = new ObservableCollection<BLEScanResult>();
                Globals.DevicePageViewModel.IsLoading = false;

                Globals.HomePageViewModel.ReadCurrentBattery();
                Globals.HomePageViewModel.BandNameLabel = Windesheart.PairedDevice.Name;

                Device.BeginInvokeOnMainThread(delegate { Application.Current.MainPage.Navigation.PopAsync(); });
                Globals.SamplesService.StartFetching();
            }
            else if (result == ConnectionResult.Failed)
            {
                Debug.WriteLine("FAIL");
                return;
            }
        }

        public static void OnDisconnect(Object obj)
        {
            Globals.DevicePageViewModel.StatusText = "Disconnected"; 
            Globals.HomePageViewModel.BandNameLabel = "";
            Globals.HomePageViewModel.BatteryImage = "";
        }
    }
}