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
    }
}
