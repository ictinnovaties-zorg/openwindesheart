using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace WindesHeartSDK
{
    public static class Windesheart
    {
        public static BLEDevice ConnectedDevice;

        /// <summary>
        /// Scan for BLEDevices that are not yet connected.
        /// </summary>
        /// <exception cref="System.Exception">Throws exception when trying to start scan when a scan is already running.</exception>
        /// <param name="scanTimeInSeconds"></param>
        /// <returns>List of IScanResult</returns>
        public static async Task<ObservableCollection<BLEDevice>> ScanForDevices(int scanTimeInSeconds = 10)
        {
            return await BluetoothService.ScanForUniqueDevicesAsync(scanTimeInSeconds);
        }

        /// <summary>
        /// Get a BLEDevice based on the UUID
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public static async Task<BLEDevice> GetKnownDevice(Guid uuid)
        {
            return await BluetoothService.GetKnownDevice(uuid);
        }
    }
}
