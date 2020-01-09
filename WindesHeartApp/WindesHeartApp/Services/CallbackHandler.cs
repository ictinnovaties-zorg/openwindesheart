using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WindesHeartApp.Models;
using WindesHeartApp.Resources;
using WindesHeartApp.Views;
using WindesHeartSDK;
using WindesHeartSDK.Models;
using Xamarin.Forms;

namespace WindesHeartApp.Services
{
    public static class CallbackHandler
    {
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

                    Globals.DevicePageViewModel.StatusText = "Connected";
                    Globals.DevicePageViewModel.DeviceList = new ObservableCollection<BLEScanResult>();
                    Globals.DevicePageViewModel.IsLoading = false;
                    Globals.HomePageViewModel.ReadCurrentBattery();
                    Globals.HomePageViewModel.BandNameLabel = Windesheart.PairedDevice.Name;
                    Globals.SamplesService.StartFetching();
                    SaveGuid();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    Debug.WriteLine("Something went wrong while connecting to device, disconnecting...");
                    Windesheart.PairedDevice.Disconnect();
                    Globals.DevicePageViewModel.IsLoading = false;
                }


            }
            else if (result == ConnectionResult.Failed)
            {
                Debug.WriteLine("Connection failed");
                Globals.DevicePageViewModel.StatusText = "Disconnected";
                Device.BeginInvokeOnMainThread(delegate
                {
                    DevicePage.DisconnectButton.IsEnabled = true;
                    Globals.HomePageViewModel.IsLoading = false;
                    Globals.HomePageViewModel.EnableDisableButtons(true);
                    Globals.DevicePageViewModel.ScanButtonText = "Scan for devices";
                    DevicePage.ScanButton.IsEnabled = true;
                    DevicePage.ReturnButton.IsVisible = true;
                });
                Globals.DevicePageViewModel.IsLoading = false;
                return;
            }
        }


        public static void SaveGuid()
        {
            if (!Application.Current.Properties.ContainsKey(Windesheart.PairedDevice.IDevice.Uuid.ToString()))
            {
                Application.Current.Properties.Add(Windesheart.PairedDevice.IDevice.Uuid.ToString(), Windesheart.PairedDevice.SecretKey);
                Application.Current.SavePropertiesAsync();
            }
        }
        public static void OnDisconnect(Object obj)
        {
            Globals.DevicePageViewModel.StatusText = "Disconnected";
            Globals.HomePageViewModel.BandNameLabel = "";
            Globals.HomePageViewModel.Battery = 0;
            Globals.HomePageViewModel.BatteryImage = "";
        }
    }
}