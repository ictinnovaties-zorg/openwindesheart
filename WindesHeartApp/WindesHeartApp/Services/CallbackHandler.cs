using System;
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

        public static async void OnConnect(ConnectionResult result, byte[] secretKey)
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

                    Globals.HomePageViewModel.ReadCurrentBattery();
                    Globals.HomePageViewModel.BandNameLabel = Windesheart.PairedDevice.Name;

                    //Callbacks
                    Windesheart.PairedDevice.EnableRealTimeHeartrate(OnHeartrateUpdated);
                    Windesheart.PairedDevice.EnableRealTimeBattery(OnBatteryUpdated);
                    Windesheart.PairedDevice.EnableRealTimeSteps(OnStepsUpdated);
                    Windesheart.PairedDevice.SubscribeToDisconnect(OnDisconnect);

                    Globals.DevicePageViewModel.StatusText = "Connected";
                    Globals.DevicePageViewModel.DeviceList = new ObservableCollection<BLEScanResult>();
                    Device.BeginInvokeOnMainThread(delegate 
                    {
                        DevicePage.DisconnectButton.IsEnabled = true;
                    });
                    Globals.DevicePageViewModel.IsLoading = false;
                    Globals.SamplesService.StartFetching();
                    SaveGuid(secretKey);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    Debug.WriteLine("Something went wrong while connecting to device, disconnecting...");
                    Windesheart.PairedDevice?.Disconnect();
                    Globals.DevicePageViewModel.IsLoading = false;
                }
            }
            else if (result == ConnectionResult.Failed)
            {
                Debug.WriteLine("Connection failed");
                if (Windesheart.PairedDevice.SecretKey != null && Application.Current.Properties.ContainsKey(Windesheart.PairedDevice.IDevice.Uuid.ToString()))
                {
                    Application.Current.Properties.Remove(Windesheart.PairedDevice.IDevice.Uuid.ToString());
                    Device.BeginInvokeOnMainThread(delegate
                    {
                        Application.Current.MainPage.DisplayAlert("Connecting failed, Please try again", "The secret key of this device has changed since the last time it was connected to this phone. Please try connecting to this device again or try to factory reset the device!", "OK");
                    });
                }
                Windesheart.PairedDevice?.Disconnect();
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


        public static void SaveGuid(byte[] secretKey)
        {
            if (!Application.Current.Properties.ContainsKey(Windesheart.PairedDevice.IDevice.Uuid.ToString()))
            {
                Application.Current.Properties.Add(Windesheart.PairedDevice.IDevice.Uuid.ToString(), secretKey);
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