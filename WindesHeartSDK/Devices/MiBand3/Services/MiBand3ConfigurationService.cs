using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Text;
using WindesHeartSDK.Devices.MiBand3Device.Models;
using WindesHeartSDK.Devices.MiBand3Device.Resources;

namespace WindesHeartSDK.Devices.MiBand3Device.Services
{
    public class MiBand3ConfigurationService
    {
        private readonly BLEDevice _miBand3;

        public MiBand3ConfigurationService(MiBand3 device)
        {
            _miBand3 = device;
        }

        /// <summary>
        /// Sets the language of the Mi Band to the given language. String has to be in format en-EN. If language is not supported nothing will happen
        /// </summary>
        /// <param name="localeString"></param>
        public async void SetLanguage(string localeString)
        {
            byte[] LanguageBytes = MiBand3Resource.Byte_SetLanguage_Template;

            Buffer.BlockCopy(Encoding.ASCII.GetBytes(localeString), 0, LanguageBytes, 3, Encoding.ASCII.GetBytes(localeString).Length);

            await _miBand3.GetCharacteristic(MiBand3Resource.GuidDeviceConfiguration).WriteWithoutResponse(LanguageBytes);
        }

        /// <summary>
        /// Set the Mi Bands time unit to either 24 hours when true or 12 hours when false
        /// </summary>
        /// <param name="is24format"></param>
        public async void SetTimeDisplayUnit(bool is24format)
        {
            if (is24format)
            {
                await _miBand3.GetCharacteristic(MiBand3Resource.GuidDeviceConfiguration).WriteWithoutResponse(MiBand3Resource.Byte_TimeFomat_24hours);
            }
            else
            {
                await _miBand3.GetCharacteristic(MiBand3Resource.GuidDeviceConfiguration).WriteWithoutResponse(MiBand3Resource.Byte_TimeFomat_12hours);
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
                await _miBand3.GetCharacteristic(MiBand3Resource.GuidDeviceConfiguration).WriteWithoutResponse(MiBand3Resource.Byte_DateFormat_dd_MM_YYYY);
            }
            else
            {
                await _miBand3.GetCharacteristic(MiBand3Resource.GuidDeviceConfiguration).WriteWithoutResponse(MiBand3Resource.Byte_DateFormat_MM_dd_YYYY);
            }
        }

        /// <summary>
        /// Set the step target on the Mi Band. Max value of 2 bytes (around 65.000)
        /// </summary>
        /// <param name="goal"></param>
        public async void SetStepGoal(int goal)
        {
            var beginCommand = new byte[] { 0x10, 0x0, 0x0 };
            var endCommand = new byte[] { 0, 0 };
            var goalBytes = BitConverter.GetBytes((ushort) goal);

            byte[] CommandBytes = new byte[beginCommand.Length + endCommand.Length + goalBytes.Length];

            Buffer.BlockCopy(beginCommand, 0, CommandBytes, 0, beginCommand.Length);
            Buffer.BlockCopy(goalBytes, 0, CommandBytes, beginCommand.Length, goalBytes.Length);
            Buffer.BlockCopy(endCommand, 0, CommandBytes, beginCommand.Length + goalBytes.Length, endCommand.Length);

            await _miBand3.GetCharacteristic(MiBand3Resource.GuidUserInfo).Write(CommandBytes);
        }

        public async void EnableStepGoalNotification(bool enable)
        {
            if (enable)
            {
                await _miBand3.GetCharacteristic(MiBand3Resource.GuidDeviceConfiguration).WriteWithoutResponse(MiBand3Resource.Byte_EnableGoalNotification);
            }
            else
            {
                await _miBand3.GetCharacteristic(MiBand3Resource.GuidDeviceConfiguration).WriteWithoutResponse(MiBand3Resource.Byte_DisableGoalNotification);
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
                await _miBand3.GetCharacteristic(MiBand3Resource.GuidDeviceConfiguration).WriteWithoutResponse(MiBand3Resource.Byte_EnableActivateOnLiftWrist);
            }
            else
            {
                await _miBand3.GetCharacteristic(MiBand3Resource.GuidDeviceConfiguration).WriteWithoutResponse(MiBand3Resource.Byte_DisableActivateOnLiftWrist);
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

            await _miBand3.GetCharacteristic(MiBand3Resource.GuidDeviceConfiguration).WriteWithoutResponse(CommandByte);
        }

        /// <summary>
        /// Enable or Disable the sleep measurement of the devices
        /// </summary>
        /// <param name="enable"></param>
        public async void EnableSleepTracking(bool enable)
        {
            if (enable)
            {
                await _miBand3.GetCharacteristic(MiBand3Resource.GuidDeviceConfiguration).WriteWithoutResponse(MiBand3Resource.Byte_EnableSleepMeasurement);
            }
            else
            {
                await _miBand3.GetCharacteristic(MiBand3Resource.GuidDeviceConfiguration).WriteWithoutResponse(MiBand3Resource.Byte_DisableSleepMeasurement);

            }
        }
    }
}
