using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WindesHeartApp.Models;
using WindesHeartApp.Resources;
using WindesHeartSDK;
using WindesHeartSDK.Models;

namespace WindesHeartApp.Services
{
    public static class CallbackHandler
    {
        private static readonly string _key = "LastConnectedDeviceGuid";

        //OnHeartrateChange/Measurement
        public static async void ChangeHeartRate(WindesHeartSDK.Models.Heartrate heartrate)
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

        }

        public static void OnConnetionCallBack(ConnectionResult result)
        {
            if (result == ConnectionResult.Succeeded)
            {
                Globals.SamplesService.EmptyDatabase();
                Windesheart.ConnectedDevice.SetHeartrateMeasurementInterval(1);
                Windesheart.ConnectedDevice.EnableRealTimeHeartrate(CallbackHandler.ChangeHeartRate);
                Windesheart.ConnectedDevice.EnableRealTimeBattery(CallbackHandler.ChangeBattery);
                Windesheart.ConnectedDevice.EnableRealTimeSteps(CallbackHandler.OnStepsUpdated);
                Windesheart.ConnectedDevice.EnableSleepTracking(true);
                Windesheart.ConnectedDevice.SetActivateOnLiftWrist(true);
                Globals.DevicePageViewModel.DeviceList = new ObservableCollection<BLEDevice>();
                Globals.DevicePageViewModel.StatusText = "Connected";
                Globals.DevicePageViewModel.IsLoading = false;
                Windesheart.ConnectedDevice.SetTime(DateTime.Now);
                Windesheart.ConnectedDevice.SubscribeToDisconnect(OnDisconnectCallBack);
                Globals.SamplesService.StartFetching();

                App.Current.Properties.TryGetValue(_key, out object storedKey);

                //If a previous key is stored
                if(storedKey != null && storedKey is Guid)
                {
                    //If connected device key is not the same as stored one
                    if (!storedKey.Equals(Windesheart.ConnectedDevice.Device.Uuid))
                    {
                        //Its the first time
                        OnNewDeviceConnected();
                    }
                }
                else
                {
                    //If no previous key stored, its the first time
                    OnNewDeviceConnected();
                }

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

        public static void OnNewDeviceConnected()
        {
            Debug.WriteLine("NEW DEVICE!");

            //Reset device settings to default
            bool DMY = true; // Day-Month-Year date format
            string language = "en-EN"; //Language English
            bool is24Hour = true; //24 hour clock
            bool wristRaise = false; //Display doesn't turn on on wrist raise

            try
            {
                Windesheart.ConnectedDevice?.SetDateDisplayFormat(DMY);
                DeviceSettings.DateFormatDMY = DMY;

                Windesheart.ConnectedDevice?.SetLanguage(language);
                DeviceSettings.DeviceLanguage = language;

                Windesheart.ConnectedDevice?.SetTimeDisplayFormat(is24Hour);
                DeviceSettings.TimeFormat24Hour = is24Hour;

                Windesheart.ConnectedDevice?.SetActivateOnLiftWrist(wristRaise);
                DeviceSettings.WristRaiseDisplay = wristRaise;
            }catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public static void OnDisconnectCallBack(Object obj)
        {
            Globals.DevicePageViewModel.StatusText = "Disconnected";
        }
    }
}