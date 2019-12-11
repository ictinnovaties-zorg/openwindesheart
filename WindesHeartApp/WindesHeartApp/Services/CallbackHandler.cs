using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WindesHeartApp.Resources;
using WindesHeartSDK;
using WindesHeartSDK.Models;

namespace WindesHeartApp.Services
{
    public static class CallbackHandler
    {
        private static readonly string _key = "LastConnectedDeviceGuid";

        //OnHeartrateChange/Measurement
        public static async void ChangeHeartRate(Heartrate heartrate)
        {
            if (heartrate.HeartrateValue == 0)
                return;
            Globals.HeartratePageViewModel.Heartrate = heartrate.HeartrateValue;
            Globals.HomePageViewModel.Heartrate = heartrate.HeartrateValue;
        }

        //OnHeartrateChange/Measurement
        public static void ChangeBattery(Battery battery)
        {
            Globals.HomePageViewModel.UpdateBattery(battery);
        }

        public static void OnStepsUpdated(StepInfo stepsInfo)
        {
            var count = stepsInfo.StepCount;
            Debug.WriteLine($"Stepcount updated: {count}");
            Globals.StepsViewModel.OnStepsUpdated(stepsInfo);

        }

        public static void OnConnetionCallBack(ConnectionResult result)
        {
            if (result == ConnectionResult.Succeeded)
            {
                Globals.SamplesService.EmptyDatabase();
                Windesheart.ConnectedDevice.SetHeartrateMeasurementInterval(1);
                Windesheart.ConnectedDevice.EnableRealTimeHeartrate(ChangeHeartRate);
                Windesheart.ConnectedDevice.EnableRealTimeBattery(ChangeBattery);
                Windesheart.ConnectedDevice.EnableRealTimeSteps(OnStepsUpdated);
                Windesheart.ConnectedDevice.EnableSleepTracking(true);
                Windesheart.ConnectedDevice.SetActivateOnLiftWrist(true);
                Globals.DevicePageViewModel.DeviceList = new ObservableCollection<BLEDevice>();
                Globals.DevicePageViewModel.StatusText = "Connected";
                Globals.DevicePageViewModel.IsLoading = false;
                Windesheart.ConnectedDevice.SetTime(DateTime.Now);
                Windesheart.ConnectedDevice.SubscribeToDisconnect(OnDisconnectCallBack);
                Globals.SamplesService.StartFetching();
                if (Windesheart.ConnectedDevice.Device.Uuid != Guid.Empty)
                {
                    if (App.Current.Properties.ContainsKey(_key))
                    {
                        App.Current.Properties.Remove(_key);
                    }

                    App.Current.Properties.Add(_key, Windesheart.ConnectedDevice.Device.Uuid);
                }
            }
            else if (result == ConnectionResult.Failed)
            {
                Debug.WriteLine("FAIL");
                return;
            }
        }

        public static void OnDisconnectCallBack(Object obj)
        {
            Globals.DevicePageViewModel.StatusText = "Disconnected";
        }
    }
}