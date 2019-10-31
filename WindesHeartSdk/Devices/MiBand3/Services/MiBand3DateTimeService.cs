using System;
using System.Threading.Tasks;
using WindesHeartSDK.Devices.MiBand3.Resources;
using WindesHeartSDK.Helpers;
using ConversionHelper = WindesHeartSDK.Helpers.ConversionHelper;

namespace WindesHeartSDK.Devices.MiBand3.Services
{
    public static class TimeService
    {
        public static async Task<bool> SetTime(DateTime time)
        {
            //Convert time to bytes
            byte[] timeToSet = ConversionHelper.GetTimeBytes(time, ConversionHelper.TimeUnit.Seconds);

            //Send to MiBand
            CharacteristicHelper.GetCharacteristic(MiBand3Resource.GuidCharacteristicCurrentTime).Write(timeToSet).Subscribe(result =>
            {
                Console.WriteLine("Time set to " + time.ToString());
            });
            return true;
        }
    }
}
