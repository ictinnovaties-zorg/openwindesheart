using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using WindesHeartSDK.Devices.MiBand3.Models;
using WindesHeartSDK.Devices.MiBand3.Services;
using WindesHeartSDK.Exceptions;

namespace WindesHeartSDK
{
    public static class BluetoothService
    {
        //Globals
        public static IDevice ConnectedDevice;
        public static List<IGattCharacteristic> Characteristics = new List<IGattCharacteristic>();

        private static ConnectionStatus ConnectionStatus;

        //Disposables
        public static IDisposable characteristicsDisposable;
        public static IDisposable statusDisposable;


        /// <summary>
        /// Scan for devices, Mi Band 3 or Xiaomi Band 3, that are not yet connected.
        /// </summary>
        /// <exception cref="System.Exception">Throws exception when trying to start scan when a scan is already running.</exception>
        /// <param name="scanTimeInSeconds"></param>
        /// <returns>List of IScanResult</returns>
        public static async Task<List<Device>> ScanForUniqueDevicesAsync(int scanTimeInSeconds = 10)
        {
            var scanResults = new List<Device>();
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
                        Device device = GetDevice(scanResult);
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
        /// <param name="device"></param>
        public static async void FindAllCharacteristics(IDevice device)
        {
            characteristicsDisposable = device.WhenAnyCharacteristicDiscovered().Subscribe(characteristic =>
            {
                if (!Characteristics.Contains(characteristic))
                {
                    Characteristics.Add(characteristic);
                }
            });
        }

        /// <summary>
        /// Connect a device
        /// </summary>
        /// <param name="device"></param>
        /// <exception cref="ConnectionException">Throws exception if device is null</exception>
        public static async Task<bool> ConnectDevice(IDevice device)
        {
            Console.WriteLine("Connecting started...");
            if (device != null)
            {
                //Set current ConnectionStatus
                ConnectionStatus = device.Status;

                //Connect with device
                device.Connect(new ConnectionConfig
                {
                    AutoConnect = false,
                    AndroidConnectionPriority = ConnectionPriority.High
                });

                if (statusDisposable == null)
                {
                    ListenForConnectionChanges(device);
                }

                ////Find characteristics of device
                if (characteristicsDisposable == null)
                {
                    FindAllCharacteristics(device);
                }

                if (Characteristics.Count > 0)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("No Characteristics found yet, trying again..");
                    await Task.Delay(5000);
                    return await ConnectDevice(device);
                }
            }
            else
            {
                throw new NullReferenceException("Device is null!");
            }
        }


        /// <summary>
        /// Disconnect current device.
        /// </summary>
        /// <param name="device"></param>
        /// <exception cref="ConnectionException">Throws exception if device is null</exception>
        public static async Task<bool> DisconnectDevice(IDevice device)
        {
            if (device != null)
            {
                //Cancel the connection
                Console.WriteLine("Trying to disconnect device...");
                device.CancelConnection();

                //Clear the global variables
                ClearGlobals();
                return true;
            }
            throw new NullReferenceException("Device is null!");
        }

        private static async void ListenForConnectionChanges(IDevice device)
        {
            statusDisposable = device.WhenStatusChanged().Subscribe(status =>
            {
                if (ConnectionStatus != status)
                {
                    Console.WriteLine("Connectionstatus changed from: " + ConnectionStatus + " to: " + status);
                    ConnectionStatus = status;
                }
            });
        }

        /// <summary>
        /// Clears the global variables for disconnecting
        /// </summary>
        private static void ClearGlobals()
        {
            AuthenticationService.authDisposable.Dispose();
            characteristicsDisposable.Dispose();
            ConnectedDevice = null;
            Characteristics.Clear();
        }

        private static Device GetDevice(IScanResult result)
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


