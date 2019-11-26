using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using WindesHeartSDK.Devices.MiBand3.Models;
using WindesHeartSDK.Exceptions;

namespace WindesHeartSDK
{
    public class BluetoothService
    {
        //Globals
        private readonly BLEDevice BLEDevice;
        private IDevice IDevice => BLEDevice.Device;

        private static ConnectionStatus ConnectionStatus;
        private static AdapterStatus AdapterStatus;

        private static IDisposable AdapterDisposable;
        private static bool BluetoothOff = false;

        public BluetoothService(BLEDevice device)
        {
            BLEDevice = device;
        }

        /// <summary>
        /// Scan when Bluetooth-adapter is ready to scan.
        /// </summary>
        /// <param name="scanTimeInSeconds"></param>
        /// <returns>List<BLEDevice></returns>
        public static async Task<List<BLEDevice>> ScanWhenAdapterReady(int scanTimeInSeconds = 10) 
        {
            var scanResults = new List<BLEDevice>();
            AdapterDisposable?.Dispose();
            AdapterDisposable = CrossBleAdapter.Current.WhenReady().Subscribe(async adapter =>
            {
                scanResults = await ScanForUniqueDevicesAsync(scanTimeInSeconds);
            });
            await Task.Delay(scanTimeInSeconds * 1000);
            return scanResults;
        }

        /// <summary>
        /// Scan for devices, Mi Band 3 or Xiaomi Band 3, that are not yet connected.
        /// </summary>
        /// <exception cref="System.Exception">Throws exception when trying to start scan when a scan is already running.</exception>
        /// <param name="scanTimeInSeconds"></param>
        /// <returns>List of IScanResult</returns>
        public static async Task<List<BLEDevice>> ScanForUniqueDevicesAsync(int scanTimeInSeconds = 10)
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
                return await ScanForUniqueDevicesAsync(scanTimeInSeconds);
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
            //StartListeningForConnectionChanges();

            //Connect
            IDevice.Connect(new ConnectionConfig
            {
                AutoConnect = true,
                AndroidConnectionPriority = ConnectionPriority.High
            });
        }

        public static async Task ConnectKnownDevice(Guid uuid)
        {
            if (uuid != Guid.Empty)
            {
                var knownDevice = await CrossBleAdapter.Current.GetKnownDevice(uuid);
                //var rssi = await knownDevice.ReadRssi();
                if (knownDevice != null) { 
                    var bleDevice = new MiBand3(0, knownDevice);
                    //bleDevice.NeedsAuthentication = false;
                    bleDevice.Connect();
                }
                else
                {
                    throw new ConnectionException();
                }
            }
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
        /// Enables logging of device status on change.
        /// </summary>
        public static void StartListeningForAdapterChanges()
        {
            bool startListening = false;
            AdapterDisposable?.Dispose();
            AdapterDisposable = CrossBleAdapter.Current.WhenStatusChanged().Subscribe(async status =>
            {
                if (status != AdapterStatus)
                {
                    if (status == AdapterStatus.PoweredOff && Windesheart.ConnectedDevice != null && startListening)
                    {
                        Windesheart.ConnectedDevice?.DisposeDisposables();
                        Windesheart.ConnectedDevice?.Device.CancelConnection();
                    }

                    if (status == AdapterStatus.PoweredOn && Windesheart.ConnectedDevice != null && startListening)
                    {
                        await ConnectKnownDevice(Windesheart.ConnectedDevice.Device.Uuid);
                    }
                    startListening = true;
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


