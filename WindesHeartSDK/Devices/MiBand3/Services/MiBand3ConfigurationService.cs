using System;
using System.Reactive.Linq;
using System.Text;
using WindesHeartSDK.Devices.MiBand3.Resources;

namespace WindesHeartSDK.Devices.MiBand3.Services
{
    public class MiBand3ConfigurationService
    {
        private readonly BLEDevice BLEDevice;

        public MiBand3ConfigurationService(BLEDevice device)
        {
            BLEDevice = device;
        }

        /// <summary>
        /// Sets the language of the Mi Band to the given language. String has to be in format en-EN. If language is not supported nothing will happen
        /// </summary>
        /// <param name="localeString"></param>
        public async void SetLanguage(string localeString)
        {
            byte[] LanguageBytes = MiBand3Resource.Byte_SetLanguage_Template;

            Buffer.BlockCopy(Encoding.ASCII.GetBytes(localeString), 0, LanguageBytes, 3, Encoding.ASCII.GetBytes(localeString).Length);

            await BLEDevice.GetCharacteristic(MiBand3Resource.GuidDeviceConfiguration).WriteWithoutResponse(LanguageBytes);
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
