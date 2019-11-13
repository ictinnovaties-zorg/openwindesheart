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

        /// <summary>
        /// Set the Mi Bands time unit to either 24 hours when true or 12 hours when false
        /// </summary>
        /// <param name="is24format"></param>
        public async void SetTimeDisplayUnit(bool is24format)
        {
            if (is24format)
            {
                await BLEDevice.GetCharacteristic(MiBand3Resource.GuidDeviceConfiguration).WriteWithoutResponse(new byte[] { 0x06, 0x02, 0x0, 0x1 });
            }
            else
            {
                await BLEDevice.GetCharacteristic(MiBand3Resource.GuidDeviceConfiguration).WriteWithoutResponse(new byte[] { 0x06, 0x02, 0x0, 0x0 });
            }
        }
    }
}
