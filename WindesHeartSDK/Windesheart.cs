using System;
using System.Threading.Tasks;
using WindesHeartSDK.Models;

namespace WindesHeartSDK
{
    public static class Windesheart
    {
        public static BLEDevice PairedDevice;

        /// <summary>
        /// Scan for BLEDevices that are not yet connected.
        /// </summary>
        /// <exception cref="System.Exception">Throws exception when trying to start scan when a scan is already running.</exception>
        /// <param name="callback">Called when a device is found</param>
        /// <returns>List of IScanResult</returns>
        public static bool StartScanning(Action<BLEScanResult> callback)
        {
            return BluetoothService.StartScanning(callback);
        }

        /// <summary>
        /// Stops scanning for devices
        /// </summary>
        public static void StopScanning()
        {
            BluetoothService.StopScanning();
        }

        /// <summary>
        /// Get a BLEDevice based on the UUID
        /// </summary>
        /// <param name="uuid">Uuid of the BLEDevice</param>
        /// <returns></returns>
        public static async Task<BLEDevice> GetKnownDevice(Guid uuid)
        {
            return await BluetoothService.GetKnownDevice(uuid);
        }

        /// <summary>
        /// Calls the callback method when Bluetooth adapter state changes to ready
        /// </summary>
        /// <param name="callback">Called when adapter is ready</param>
        public static void WhenAdapterReady(Action callback)
        {
            BluetoothService.WhenAdapterReady(callback);
        }

        /// <summary>
        /// Calls the callback method when Bluetooth adapter status changes
        /// </summary>
        /// <param name="callback">Called when status changed</param>
        public static void OnAdapterChanged(Action callback)
        {
            BluetoothService.OnAdapterChanged(callback);
        }

        /// <summary>
        /// Return whether device is currently scanning for devices.
        /// </summary>
        public static bool IsScanning()
        {
            return BluetoothService.IsScanning();
        }
    }
}
