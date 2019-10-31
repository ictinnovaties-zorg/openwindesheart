using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using WindesHeartSDK.Devices.MiBand3.Models;
using WindesHeartSDK.Devices.MiBand3.Services;

namespace WindesHeartSDK
{
    public class BluetoothService
    {
        //Globals
        public WDevice WDevice;
        private IDevice Device => WDevice.Device;

        private static ConnectionStatus ConnectionStatus;

        //Disposables
        public static IDisposable characteristicsDisposable;
        public static IDisposable statusDisposable;
        public static IDisposable connectedDeviceDisposable;
        public static IDisposable disconnectionDisposable;


        public BluetoothService(WDevice wDevice)
        {
            WDevice = wDevice;
        }

        /// <summary>
        /// Scan for devices, Mi Band 3 or Xiaomi Band 3, that are not yet connected.
        /// </summary>
        /// <exception cref="System.Exception">Throws exception when trying to start scan when a scan is already running.</exception>
        /// <param name="scanTimeInSeconds"></param>
        /// <returns>List of IScanResult</returns>
        public static async Task<List<WDevice>> ScanForUniqueDevicesAsync(int scanTimeInSeconds = 10)
        {
            var scanResults = new List<WDevice>();
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
                        WDevice device = GetDevice(scanResult);
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

        /// <summary>
        /// Find all characteristics for a device and store it in the Characteristics property
        /// </summary>
        public async void FindAllCharacteristics()
        {
            characteristicsDisposable = Device.WhenAnyCharacteristicDiscovered().Subscribe(characteristic =>
            {
                if (!WDevice.Characteristics.Contains(characteristic))
                {
                    WDevice.Characteristics.Add(characteristic);
                }
            });
        }

        /// <summary>
        /// Connects to the device
        /// </summary>
        public async Task<bool> Connect()
        {
            Console.WriteLine("Connecting started...");

            //Check for status changes
            StartListeningForConnectionChanges();

            //Connect
            Device.Connect(new ConnectionConfig
            {
                AutoConnect = false,
                AndroidConnectionPriority = ConnectionPriority.High
            });

            //If any characteristic found, it's connected!
            if (WDevice.Characteristics.Count > 0)
            {
                return true;
            }
            else
            {
                Console.WriteLine("No Characteristics found yet, trying again..");
                await Task.Delay(5000);
                return await Connect();
            }
        }



        /// <summary>
        /// Disconnect current device.
        /// </summary>
        public async Task<bool> Disconnect()
        {
            //Cancel the connection
            Console.WriteLine("Trying to disconnect device...");
            Device.CancelConnection();

            //Clear the global variables and disposables
            disconnectionDisposable = Device.WhenDisconnected().Subscribe(disconnectedDevice =>
            {
                ClearGlobals();
            });
            return true;
        }

        /// <summary>
        /// Enables logging of device status on change.
        /// </summary>
        private void StartListeningForConnectionChanges()
        {
            statusDisposable?.Dispose();
            statusDisposable = Device.WhenStatusChanged().Subscribe(status =>
            {
                if (ConnectionStatus != status)
                {
                    Console.WriteLine("Connectionstatus changed from: " + ConnectionStatus + " to: " + status);
                    ConnectionStatus = status;
                }
            });
        }

        /// <summary>
        /// Disables device status logs.
        /// </summary>
        public static void StopListeningForConnectionChanges()
        {
            statusDisposable?.Dispose();
        }

        /// <summary>
        /// Clears the global variables and disposables.
        /// </summary>
        private static void ClearGlobals()
        {

            //Disposables
            MiBand3AuthenticationService.authDisposable?.Dispose();
            characteristicsDisposable?.Dispose();
            connectedDeviceDisposable?.Dispose();
            statusDisposable?.Dispose();

            disconnectionDisposable?.Dispose();

        }

        /// <summary>
        /// Returns the right WDevice based on the ScanResult
        /// </summary>
        private static WDevice GetDevice(IScanResult result)
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


