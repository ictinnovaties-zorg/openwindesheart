using System;
using WindesHeartApp.Resources;
using WindesHeartSDK.Models;

namespace WindesHeartApp.Services
{
    public static class CallbackHandler
    {
        //OnHeartrateChange/Measurement
        public static void ChangeHeartRate(Heartrate heartrate)
        {
            if (heartrate.HeartrateValue == 0)
                return;

            Console.WriteLine($"HEARTRATE VALUE = {heartrate.HeartrateValue}");
            Globals.heartRate = heartrate.HeartrateValue;
            Globals.hrviewModel.HeartRate = heartrate.HeartrateValue;
            Globals.homepageviewModel.HeartRate = heartrate.HeartrateValue;
        }

        //OnHeartrateChange/Measurement
        public static void ChangeBattery(Battery battery)
        {
            Console.WriteLine($"Batterypercentage is now: {battery.Status}");
            Globals.batteryPercentage = battery.BatteryPercentage;
            Globals.homepageviewModel.Battery = battery.BatteryPercentage;
        }
    }
}