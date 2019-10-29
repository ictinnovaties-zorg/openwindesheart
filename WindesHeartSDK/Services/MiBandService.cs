using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Plugin.BluetoothLE;
using WindesHeartSDK.Models;

namespace WindesHeartSDK.Services
{
    public static class MiBandService
    {
        /// <summary>
        /// Get Raw Battery data
        /// </summary>
        /// <returns>byte[]</returns>
        public static async Task<byte[]> GetRawBatteryData()
        {
            var batteryCharacteristic = GetBatteryCharacteristic();
            var gattResult = await batteryCharacteristic.Read();

            if (gattResult.Characteristic.Value != null)
            {
                var rawData = gattResult.Characteristic.Value;
                return rawData;
            }

            return null;
        }


        /// <summary>
        /// Get Battery-object from raw data.
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns>Battery</returns>
        public static async Task<Battery> GetCurrentBatteryData()
        {
            var rawData = await GetRawBatteryData();
            return CreateBatteryObject(rawData);
        }

        /// <summary>
        /// Creates Battery-object from rawData.
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns>Battery</returns>
        private static Battery CreateBatteryObject(byte[] rawData)
        {
            if(rawData != null)
            {
                var batteryPercentage = rawData[1];
                StatusEnum status = StatusEnum.NotCharging;

                if (rawData[2] == 1)
                {
                    status = StatusEnum.Charging;
                }

                var battery = new Battery
                {
                    RawData = rawData,
                    BatteryPercentage = batteryPercentage,
                    Status = status
                };

                return battery;
            }

            return null;
        }

        /// <summary>
        /// Set listener for battery changes. 
        /// </summary>
        /// <returns>IDisposable</returns>
        public static IDisposable GetBatteryStatusContinuously(Action<Battery> callback)
        {
            var charBatterySub = GetBatteryCharacteristic().RegisterAndNotify().Subscribe(
                 x => callback(CreateBatteryObject(x.Characteristic.Value))
             );            

            return charBatterySub;
        }

        /// <summary>
        /// Get Battery Characteristic
        /// </summary>
        /// <returns>IGattCharacteristic</returns>
        private static IGattCharacteristic GetBatteryCharacteristic()
        {
            var batteryCharacteristic = BluetoothService.GetCharacteristic(MiBand.MiBandResource.GuidCharacteristic6BatteryInfo);
            return batteryCharacteristic;
        }
    }
}
