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
            if (battery.Status == StatusEnum.Charging)
            {
                Globals.batteryImage = "BatteryCharging.png";
                Globals.homepageviewModel.BatteryImage = "BatteryCharging.png";
                return;
            }

            else if (battery.BatteryPercentage >= 0 && battery.BatteryPercentage < 26)
            {
                Globals.batteryImage = "BatteryQuart.png";

                Globals.homepageviewModel.BatteryImage = "BatteryQuart.png";
            }
            else if (battery.BatteryPercentage >= 26 && battery.BatteryPercentage < 51)
            {
                Globals.batteryImage = "BatteryHalf.png";

                Globals.homepageviewModel.BatteryImage = "BatteryHalf.png";
            }
            else if (battery.BatteryPercentage >= 51 && battery.BatteryPercentage < 76)
            {
                Globals.batteryImage = "BatteryThreeQuarts.png";

                Globals.homepageviewModel.BatteryImage = "BatteryThreeQuarts.png";
            }
            else if (battery.BatteryPercentage >= 76)
            {
                Globals.batteryImage = "BatteryFull.png";

                Globals.homepageviewModel.BatteryImage = "BatteryFull.png";
            }

        }
    }
}