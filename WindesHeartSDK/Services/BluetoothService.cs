using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using WindesHeartSDK.Devices.MiBand3Device.Models;

namespace WindesHeartSDK
{
    public class BluetoothService
    {
        //Globals
        private readonly BLEDevice BLEDevice;
        private IDevice IDevice => BLEDevice.Device;

        private ConnectionStatus ConnectionStatus;


        public BluetoothService(BLEDevice device)
        {
            BLEDevice = device;
        }

        /// <summary>
        /// Scan for devices, Mi Band 3 or Xiaomi Band 3, that are not yet connected.
        /// </summary>
        /// <exception cref="System.Exception">Throws exception when trying to start scan when a scan is already running.</exception>
        /// <param name="scanTimeInSeconds"></param>
        /// <returns>List of IScanResult</returns>
        public static async Task<List<BLEDevice>> ScanForDevices(int scanTimeInSeconds = 10)
        {
            var scanResults = new List<BLEDevice>();
            var uniqueGuids = new List<Guid>();

            //Start scanning when adapter is powered on.
            if (CrossBleAdapter.Current.Status == AdapterStatus.PoweredOn)
            {
                //Trigger event and add to devices list
                Console.WriteLine("Started scanning");
                var scanner = CrossBleAdapter.Current.Scan().Subscribe(scanResult =>
                {
                    if (scanResult.Device != null && !string.IsNullOrEmpty(scanResult.Device.Name) && !uniqueGuids.Contains(scanResult.Device.Uuid))
                    {
                        //Set device
                        BLEDevice device = GetDevice(scanResult);
                        if (device != null)
                        {
                            scanResults.Add(device);
                        }
                        uniqueGuids.Add(scanResult.Device.Uuid);
                    }
                });

                //Stop scanning after delayed time.
                await Task.Delay(scanTimeInSeconds * 1000);
                Console.WriteLine("Stopped scanning for devices... Amount of unique devices found: " + scanResults.Count);
                scanner.Dispose();
            }
            else
            {
                Console.WriteLine("Bluetooth-Adapter state is: " + CrossBleAdapter.Current.Status + ". Trying again!");
                await Task.Delay(2000);
                return await ScanForDevices(scanTimeInSeconds);
            }

            //Order scanresults by descending signal strength
            if (scanResults.Count > 1)
            {
                scanResults = scanResults.OrderByDescending(x => x.Rssi).ToList();
            }

            return scanResults;
        }

        public void Connect()
        {
            Console.WriteLine("Connecting started...");

            //Check for status changes
            StartListeningForConnectionChanges();

            //Connect
            IDevice.Connect(new ConnectionConfig
            {
                AutoConnect = false,
                AndroidConnectionPriority = ConnectionPriority.High
            });
        }


        /// <summary>
        /// Disconnect current device.
        /// </summary>
        public void Disconnect()
        {
            //Cancel the connection
            Console.WriteLine("Trying to disconnect device...");
            BLEDevice.Authenticated = false;
            IDevice.CancelConnection();
        }

        /// <summary>
        /// Enables logging of device status on change.
        /// </summary>
        private void StartListeningForConnectionChanges()
        {
            IDevice.WhenStatusChanged().Subscribe(status =>
            {
                if (ConnectionStatus != status)
                {
                    Console.WriteLine("Connectionstatus changed from: " + ConnectionStatus + " to: " + status);
                    ConnectionStatus = status;
                }
            });
        }

        /// <summary>
        /// Returns the right WDevice based on the ScanResult
        /// </summary>
        private static BLEDevice GetDevice(IScanResult result)
        {
            Console.WriteLine(result.Device.Name);
            var name = result.Device.Name;

            if (name.Equals("Mi Band 3") || name.Equals("Xiaomi Mi Band 3"))
            {
                return new MiBand3(result.Rssi, result.Device);
            }
            return null;
        }
    }
}


