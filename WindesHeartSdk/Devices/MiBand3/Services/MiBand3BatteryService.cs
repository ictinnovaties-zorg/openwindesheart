using Plugin.BluetoothLE;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using WindesHeartSDK.Devices.MiBand3.Resources;
using WindesHeartSDK.Exceptions;
using WindesHeartSDK.Helpers;
using WindesHeartSDK.Models;

namespace WindesHeartSDK.Devices.MiBand3.Services
{
    public static class BatteryService
    {
        public static IDisposable batteryDisposable;

        /// <summary>
        /// Get Raw Battery data.
        /// </summary>
        /// <exception cref="NullReferenceException">Throws exception if BatteryCharacteristic or its value is null.</exception>
        /// <returns>byte[]</returns>
        public static async Task<byte[]> GetRawBatteryDataAsync()
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

                throw new NullReferenceException("BatteryCharacteristic value is null!");
            }

            throw new NullReferenceException("BatteryCharacteristic could not be found!");
        }


        /// <summary>
        /// Get Battery-object from raw data.
        /// </summary>
        /// <exception cref="NullReferenceException">Throws exception if rawData is null.</exception>
        /// <returns>Battery</returns>
        public static async Task<Battery> GetCurrentBatteryDataAsync()
        {
            var rawData = await GetRawBatteryDataAsync();
            if (rawData != null)
            {
                return CreateBatteryObject(rawData);
            }

            throw new NullReferenceException("Rawdata is null!");
        }

        /// <summary>
        /// Creates Battery-object from rawData.
        /// </summary>
        /// <param name="rawData"></param>
        /// <exception cref="NullReferenceException">Throws exception if rawData is null.</exception>
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

            throw new NullReferenceException("Rawdata of battery is null!");
        }

        /// <summary>
        /// Receive BatteryStatus-updates continuously.
        /// </summary>
        public static void EnableBatteryStatusUpdates(Action<Battery> callback)
        {
            batteryDisposable?.Dispose();
            batteryDisposable = CharacteristicHelper.GetCharacteristic(MiBand3Resource.GuidCharacteristic6BatteryInfo).RegisterAndNotify().Subscribe(
                x => callback(CreateBatteryObject(x.Characteristic.Value))
            );
        }

        /// <summary>
        /// Get Battery Characteristic
        /// </summary>
        /// <returns>IGattCharacteristic</returns>
        private static IGattCharacteristic GetBatteryCharacteristic()
        {
            return CharacteristicHelper.GetCharacteristic(MiBand3Resource.GuidCharacteristic6BatteryInfo);
        }
    }
}
