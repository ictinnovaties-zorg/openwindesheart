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
                await BLEDevice.GetCharacteristic(MiBand3Resource.GuidDeviceConfiguration).WriteWithoutResponse(MiBand3Resource.Byte_TimeFomat_24hours);
            }
            else
            {
                await BLEDevice.GetCharacteristic(MiBand3Resource.GuidDeviceConfiguration).WriteWithoutResponse(MiBand3Resource.Byte_TimeFomat_12hours);
            }
        }

        /// <summary>
        /// Set the Mi Bands Date format to either dd/MM/YYYY if true or MM/dd/YYYY if false
        /// </summary>
        /// <param name="isdMY"></param>
        public async void SetDateDisplayUnit(bool isdMY)
        {
            if (isdMY)
            {

                await BLEDevice.GetCharacteristic(MiBand3Resource.GuidDeviceConfiguration).WriteWithoutResponse(MiBand3Resource.Byte_DateFormat_dd_MM_YYYY);
            }
            else
            {
                await BLEDevice.GetCharacteristic(MiBand3Resource.GuidDeviceConfiguration).WriteWithoutResponse(MiBand3Resource.Byte_DateFormat_MM_dd_YYYY);
            }
        }
    }
}
