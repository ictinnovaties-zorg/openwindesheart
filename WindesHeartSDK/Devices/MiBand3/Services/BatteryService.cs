using System;
using System.Threading.Tasks;
using Plugin.BluetoothLE;
using WindesHeartSDK.Exceptions;
using WindesHeartSDK.Models;
using WindesHeartSDK.Devices.MiBand3.Resources;
using System.Reactive.Linq;
using WindesHeartSDK.Helpers;

namespace WindesHeartSDK.Devices.MiBand3.Services
{
    public static class BatteryService
    {
        /// <summary>
        /// Get Raw Battery data.
        /// </summary>
        /// <exception cref="BatteryException">Throws BatteryException if no bytes where found or Battery-characteristic was not found</exception>
        /// <returns>byte[]</returns>
        public static async Task<byte[]> GetRawBatteryData()
        {
            var batteryCharacteristic = GetBatteryCharacteristic();
            if (batteryCharacteristic != null)
            {
                var gattResult = await batteryCharacteristic.Read();

                if (gattResult.Characteristic.Value != null)
                {
                    var rawData = gattResult.Characteristic.Value;
                    return rawData;
                }

                throw new BatteryException("Raw bytes have not been found in the battery-characteristic");
            }

            throw new BatteryException("Batterycharacteristic could not be found.");
        }


        /// <summary>
        /// Get Battery-object from raw data. Throws BatteryException when something went wrong.
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns>Battery</returns>
        public static async Task<Battery> GetCurrentBatteryData()
        {
            var rawData = await GetRawBatteryData();
            if (rawData != null)
            {
                return CreateBatteryObject(rawData);
            }

            throw new BatteryException("Rawdata has not been found.");
        }

        /// <summary>
        /// Creates Battery-object from rawData.
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns>Battery</returns>
        private static Battery CreateBatteryObject(byte[] rawData)
        {
            if (rawData != null)
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

            throw new BatteryException("Rawdata is structured incorrectly, try again!");
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
            var batteryCharacteristic = CharacteristicHelper.GetCharacteristic(MiBand3Resource.GuidCharacteristic6BatteryInfo);
            return batteryCharacteristic;
        }
    }
}
