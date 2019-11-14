using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using WindesHeartSDK.Devices.MiBand3.Resources;
using WindesHeartSDK.Helpers;

namespace WindesHeartSDK.Devices.MiBand3.Services
{
    public class MiBand3DateTimeService
    {
        private readonly BLEDevice BLEDevice;

        public MiBand3DateTimeService(BLEDevice device)
        {
            BLEDevice = device;
        }

        public async Task<bool> SetTime(DateTime time)
        {
            //Convert time to bytes
            byte[] timeToSet = ConversionHelper.GetTimeBytes(time, ConversionHelper.TimeUnit.Seconds);

            //Send to MiBand
            BLEDevice.GetCharacteristic(MiBand3Resource.GuidCharacteristicCurrentTime).Write(timeToSet).Subscribe(result =>
            {
                Console.WriteLine("Time set to " + time.ToString());
            });
            return true;
        }
    }
}
