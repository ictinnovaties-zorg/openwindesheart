using System;
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
        /// <param name="callback">Called when a device is found</param>
        /// <returns>List of IScanResult</returns>
        public static bool StartScanning(Action<BLEDevice> callback)
        {
            return BluetoothService.StartScanning(callback);
        }

        public static void StopScanning()
        {
            BluetoothService.StopScanning();
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


        public static void WhenAdapterReady(Action callback)
        {
            BluetoothService.WhenAdapterReady(callback);
        }
    }
}
