using System;
using System.Collections.ObjectModel;
using WindesHeartApp.Resources;
using WindesHeartSDK;
using WindesHeartSDK.Models;

namespace WindesHeartApp.Services
{
    public static class CallbackHandler
    {
        //private static readonly string _key = "LastConnectedDeviceGuid";
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
            Console.WriteLine($"Stepcount updated: {count}");

        }
        public static void OnConnetionCallBack(ConnectionResult result)
        {
            if (result == ConnectionResult.Succeeded)
            {
                Windesheart.ConnectedDevice.EnableRealTimeBattery(CallbackHandler.ChangeBattery);
                Windesheart.ConnectedDevice.EnableRealTimeHeartrate(CallbackHandler.ChangeHeartRate);
                Windesheart.ConnectedDevice.SetHeartrateMeasurementInterval(1);
                Windesheart.ConnectedDevice.EnableRealTimeBattery(CallbackHandler.ChangeBattery);
                Windesheart.ConnectedDevice.EnableRealTimeSteps(CallbackHandler.OnStepsUpdated);
                Globals.DevicePageViewModel.DeviceList = new ObservableCollection<BLEDevice>();
                Globals.DevicePageViewModel.StatusText = "Connected";
                Globals.DevicePageViewModel.IsLoading = false;
                Windesheart.ConnectedDevice.SetTime(DateTime.Now);
            }
            else if (result == ConnectionResult.Failed)
            {
                Console.WriteLine("FAIL");
                return;
            }
        }
        //GUID SHOULD STILL BE SAVED, IN PROPERTIES
        //private void SaveDeviceInAppProperties(Guid guid)
        //{
        //    if (guid != Guid.Empty)
        //    {
        //        if (App.Current.Properties.ContainsKey(_key))
        //        {
        //            App.Current.Properties.Remove(_key);
        //        }

        //        App.Current.Properties.Add(_key, guid);
        //    }
    }
}
