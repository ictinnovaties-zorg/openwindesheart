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
            Globals.heartrateviewModel.Heartrate = heartrate.HeartrateValue;
            Globals.homepageviewModel.Heartrate = heartrate.HeartrateValue;
            var heartRate = new Models.Heartrate(DateTime.Now, heartrate.HeartrateValue);
            await Globals.HeartrateRepository.AddAsync(heartRate);
        }

        //OnHeartrateChange/Measurement
        public static void ChangeBattery(Battery battery)
        {
            Globals.homepageviewModel.UpdateBattery(battery);
        }

        public static void OnStepsUpdated(StepInfo stepsInfo)
        {
            var count = stepsInfo.StepCount;
            Debug.WriteLine($"Stepcount updated: {count}");

        }

        public static void OnConnetionCallBack(ConnectionResult result)
        {
            if (result == ConnectionResult.Succeeded)
            {
                Windesheart.ConnectedDevice.EnableRealTimeBattery(CallbackHandler.ChangeBattery);
                Windesheart.ConnectedDevice.SetHeartrateMeasurementInterval(5);
                Windesheart.ConnectedDevice.EnableRealTimeHeartrate(CallbackHandler.ChangeHeartRate);
<<<<<<< HEAD
                Windesheart.ConnectedDevice.EnableRealTimeBattery(CallbackHandler.ChangeBattery);

=======
>>>>>>> 838a090fd24967b42d99178c3943d5e196458b0c
                Windesheart.ConnectedDevice.EnableRealTimeSteps(CallbackHandler.OnStepsUpdated);
                Windesheart.ConnectedDevice.EnableSleepTracking(true);
                Windesheart.ConnectedDevice.SetActivateOnLiftWrist(true);
                Globals.DevicePageViewModel.DeviceList = new ObservableCollection<BLEDevice>();
                Windesheart.ConnectedDevice.SetActivateOnLiftWrist(true);
                Globals.DevicePageViewModel.StatusText = "Connected";
                Globals.DevicePageViewModel.IsLoading = false;
                Windesheart.ConnectedDevice.SetTime(DateTime.Now);
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

        private static void SaveDeviceInAppProperties(Guid guid)
        {
            if (guid != Guid.Empty)
            {
                if (App.Current.Properties.ContainsKey(_key))
                {
                    App.Current.Properties.Remove(_key);
                }

                App.Current.Properties.Add(_key, guid);
            }
        }
    }
}