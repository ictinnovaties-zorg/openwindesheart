using System;
using System.Reactive.Linq;
using System.Text;
using WindesHeartSDK.Devices.MiBand3Device.Resources;

namespace WindesHeartSDK.Devices.MiBand3Device.Services
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

        /// <summary>
        /// Set permanent activate on wrist lift. true for enable. false for disable
        /// </summary>
        /// <param name="activate"></param>
        public async void SetActivateOnWristLift(bool activate)
        {
            if (activate)
            {
                await BLEDevice.GetCharacteristic(MiBand3Resource.GuidDeviceConfiguration).WriteWithoutResponse(MiBand3Resource.Byte_EnableActivateOnLiftWrist);
            }
            else
            {
                await BLEDevice.GetCharacteristic(MiBand3Resource.GuidDeviceConfiguration).WriteWithoutResponse(MiBand3Resource.Byte_DisableActivateOnLiftWrist);
            }
        }

        /// <summary>
        /// Set the activate display on lift wrist only to be active between the two given times. Date's dont matter
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public async void SetActivateOnWristLift(DateTime from, DateTime to)
        {
            byte[] CommandByte = MiBand3Resource.Byte_ScheduleActivateOnLiftWrist_Template;

            CommandByte[4] = Convert.ToByte(from.Hour);
            CommandByte[5] = Convert.ToByte(from.Minute);

            CommandByte[6] = Convert.ToByte(to.Hour);
            CommandByte[7] = Convert.ToByte(to.Minute);

            await BLEDevice.GetCharacteristic(MiBand3Resource.GuidDeviceConfiguration).WriteWithoutResponse(CommandByte);
        }
    }
}
