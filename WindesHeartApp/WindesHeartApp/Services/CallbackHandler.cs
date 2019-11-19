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

        public static void OnStepsUpdated(StepInfo stepsInfo)
        {
            var count = stepsInfo.StepCount;
            Console.WriteLine($"Stepcount updated: {count}");
            Globals.StepsViewModel.Steps = count;
        }
    }
}