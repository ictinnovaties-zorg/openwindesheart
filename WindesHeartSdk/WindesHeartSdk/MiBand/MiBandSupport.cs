using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Plugin.BluetoothLE;
using WindesHeartSdk.Helpers;
using WindesHeartSdk.Model;

namespace WindesHeartSdk.MiBand
{
    public class MiBandSupport
    {
        /// <summary>
        /// Set all Mi Band settings to default
        /// </summary>
        public static async Task RestoreSettings()
        {
            await SetActivateDisplayOnLiftWrist(false);
            await SetHeartRateMeasurementInterval(1);
        }

        /// <summary>
        /// Set activate display when lift wrist
        /// </summary>
        public static async Task SetActivateDisplayOnLiftWrist(bool enable)
        {
            if (enable)
            {
                await BleTransactionHelper.TryWriteWithoutResponse(MiBandResources.GuidCharacteristic3Configuration, 
                    MiBandResources.CommandEnableDisplayOnLiftWrist);
            }
            else
            {
                await BleTransactionHelper.TryWriteWithoutResponse(MiBandResources.GuidCharacteristic3Configuration,
                    MiBandResources.CommandDisableDisplayOnLiftWrist);
            }
        }

        /// <summary>
        /// Set heart rate measure interval
        /// </summary>
        public static async Task SetHeartRateMeasurementInterval(int interval)
        {
            await BleTransactionHelper.TryWriteWithoutResponse(MiBandResources.GuidCharacteristicHeartRateControlPoint,
                new byte[] {MiBandResources.CommandSetPeriodicHrMeasurementInterval, 0x01});
        }

        /// <summary>
        /// Set Mi Band date and time to device time and time
        /// </summary>
        public static async Task SetCurrentTime()
        {
            var bytes = BleTypeConversions.GetTimeBytes(DateTime.Now, TimeUnit.Seconds);

            await BleTransactionHelper.TryWrite(MiBandResources.GuidCharacteristicCurrentTime, bytes);

            Console.WriteLine("Time set to device");
        }

        /// <summary>
        /// Set listener for battery changes. 
        /// </summary>
        public static async Task<IDisposable> OnBatteryStatusChange(Action<BatteryInfo> callback)
        {
            var charBatterySub = BleTransactionHelper.GetCharacteristic(MiBandResources.GuidCharacteristic6BatteryInfo).RegisterAndNotify().Subscribe(
                x => callback(new BatteryInfo(x.Characteristic.Value)),
                Console.Write
            );

            var batteryData = await BleTransactionHelper.GetCharacteristic(MiBandResources.GuidCharacteristic6BatteryInfo).Read();
            callback(new BatteryInfo(batteryData.Characteristic.Value));

            return charBatterySub;
        }


    }
}
