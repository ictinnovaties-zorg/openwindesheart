using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.BluetoothLE;

namespace WindesHeartSDK
{
    public static class BluetoothService
    {
        /// <summary>
        /// Scan for devices with a certain name that are not yet connected.
        /// param: scanTimeInSeconds determines the time the scanning should take (default 10 seconds).
        /// param: deviceName is the name that should be searched for while scanning (default Mi Band 3).
        /// </summary>
        /// <returns>List of IDevice</returns>
        public static List<IDevice> ScanForUniqueDevices(int scanTimeInSeconds = 10, string deviceName = "Mi Band 3")
        {
            var deviceList = new List<IDevice>();
            var uniqueGuids = new List<Guid>();

            //Check when adapter is ready to scan
            CrossBleAdapter.Current.WhenReady().Subscribe(async adapter =>
            {
                //Start scanning when adapter is powered on.
                if (adapter.Status == AdapterStatus.PoweredOn)
                {
                    //Trigger event and add to devices list
                    Console.WriteLine("Started scanning");
                    var scanner = adapter.Scan().Subscribe(scanResult =>
                    {
                        if (scanResult.Device != null && !string.IsNullOrEmpty(scanResult.Device.Name) && scanResult.Device.Name.Equals(deviceName) && !uniqueGuids.Contains(scanResult.Device.Uuid))
                        {
                            Console.WriteLine(deviceName + " found with signalstrength: " + scanResult.Rssi);
                            deviceList.Add(scanResult.Device);
                            uniqueGuids.Add(scanResult.Device.Uuid);
                        }
                    });

                    //Stop scanning after delayed time.
                    await Task.Delay(scanTimeInSeconds * 1000);
                    Console.WriteLine("Stopped scanning for devices... Amount of unique devices found: " + deviceList.Count);
                    scanner.Dispose();
                }
                else
                {
                    Console.WriteLine("Bluetooth adapter is not powered on, try again!");
                }
            });
            
            return deviceList;
        }
    }
}
