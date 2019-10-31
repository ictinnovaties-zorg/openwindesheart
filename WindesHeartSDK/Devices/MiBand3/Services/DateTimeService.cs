using System;
using WindesHeartSDK.Devices.MiBand3.Helpers;
using WindesHeartSDK.Devices.MiBand3.Resources;
using WindesHeartSDK.Helpers;

namespace WindesHeartSDK.Devices.MiBand3.Services
{
    public static class DateTimeService
    {
        public static void SetTime(DateTime time)
        {
            if (BluetoothService.ConnectedDevice != null)
            {
                //Convert time to bytes
                byte[] timeToSet = ConversionHelper.GetTimeBytes(time, ConversionHelper.TimeUnit.Seconds);

                //Send to MiBand
                CharacteristicHelper.GetCharacteristic(MiBand3Resource.GuidCharacteristicCurrentTime).Write(timeToSet).Subscribe(result =>
                {
                    Console.WriteLine("Time set to " + time.ToString());
                });
            }
            else
            {
                throw new NullReferenceException("ConnectedDevice is null");
            }
        }

    }
}
