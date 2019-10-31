using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Plugin.BluetoothLE;
using WindesHeartSDK.Devices.MiBand3.Resources;
using WindesHeartSDK.Devices.MiBand3.Services;

namespace WindesHeartSDK
{
    public static class BluetoothService
    {
        //Globals
        public static List<IScanResult> ScanResults = new List<IScanResult>();
        public static IDevice ConnectedDevice;
        public static List<IGattCharacteristic> Characteristics = new List<IGattCharacteristic>();

        private static ConnectionStatus ConnectionStatus;

        //Disposables
        public static IDisposable characteristicsDisposable;
        public static IDisposable statusDisposable;
        public static IDisposable connectedDeviceDisposable;
        public static IDisposable disconnectionDisposable;
 

        /// <summary>
        /// Scan for devices, Mi Band 3 or Xiaomi Band 3, that are not yet connected.
        /// </summary>
        /// <exception cref="System.Exception">Throws exception when trying to start scan when a scan is already running.</exception>
        /// <param name="scanTimeInSeconds"></param>
        /// <returns>List of IScanResult</returns>
        public static async Task<List<IScanResult>> ScanForUniqueDevicesAsync(int scanTimeInSeconds = 10)
        {
            var scanResults = new List<IScanResult>();
            var uniqueGuids = new List<Guid>();

            //Start scanning when adapter is powered on.
            if (CrossBleAdapter.Current.Status == AdapterStatus.PoweredOn)
            {
                //Trigger event and add to devices list
                Console.WriteLine("Started scanning");
                var scanner = CrossBleAdapter.Current.Scan().Subscribe(scanResult =>
                {
                    if (scanResult.Device != null && !string.IsNullOrEmpty(scanResult.Device.Name) && (scanResult.Device.Name.Equals("Mi Band 3") || scanResult.Device.Name.Equals("Xiaomi Band 3")) && !uniqueGuids.Contains(scanResult.Device.Uuid))
                    {
                        Console.WriteLine("Mi Band 3 found with signalstrength: " + scanResult.Rssi);
                        scanResults.Add(scanResult);
                        uniqueGuids.Add(scanResult.Device.Uuid);
                    }
                });

                //Stop scanning after delayed time.
                await Task.Delay(scanTimeInSeconds * 1000);
                Console.WriteLine("Stopped scanning for devices... Amount of unique devices found: " + scanResults.Count);
                scanner.Dispose();
            } else
            {
                Console.WriteLine("Bluetooth-Adapter state is: " + CrossBleAdapter.Current.Status + ". Trying again!");
                await Task.Delay(2000);
                return await ScanForUniqueDevicesAsync(scanTimeInSeconds);
            }

            //Order scanresults by descending signal strength
            scanResults = scanResults.OrderByDescending(x => x.Rssi).ToList();

            //Set ScanResults global
            ScanResults = scanResults;
            return scanResults;
        }        

        /// <summary>
        /// Connect a device
        /// </summary>
        /// <param name="device"></param>
        /// <exception cref="NullReferenceException">Throws exception if device is null.</exception>
        public static void ConnectDevice(IDevice device)
        {
            if (device != null)
            {
                //Check if there is not already a connected device
                if(ConnectedDevice != null)
                {
                    Console.WriteLine("A device is already connected, please disconnect first.");
                    return;
                }

                //Check for status changes
                StartListeningForConnectionChanges(device);

                //Connect
                device.Connect(new ConnectionConfig
                {
                    AutoConnect = false,
                    AndroidConnectionPriority = ConnectionPriority.High
                });

                //Check when connected to device
                connectedDeviceDisposable = device.WhenConnected().Subscribe(connectedDevice =>
                {
                    Characteristics.Clear();

                    //Find unique characteristics
                    characteristicsDisposable = device.WhenAnyCharacteristicDiscovered().Subscribe(async characteristic =>
                    {
                        if (!Characteristics.Contains(characteristic))
                        {
                            Characteristics.Add(characteristic);

                            //Check if authCharacteristic has been found, then authenticate
                            if(characteristic.Uuid == MiBand3Resource.GuidCharacteristicAuth)
                            {
                                await AuthenticationService.AuthenticateDeviceAsync(device);
                            }
                        }
                    });
                });                
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
        /// <exception cref="NullReferenceException">Throws exception if device is null</exception>
        public static void DisconnectDevice(IDevice device)
        {
            if(device != null)
            {
                //Cancel the connection
                Console.WriteLine("Trying to disconnect device...");
                device.CancelConnection();

                //Clear the global variables and disposables
                disconnectionDisposable = device.WhenDisconnected().Subscribe(disconnectedDevice =>
                {
                    ClearGlobals();
                });
                return;
            }
            throw new NullReferenceException("Device is null!");
        }

        /// <summary>
        /// Enables logging of device status on change.
        /// </summary>
        /// <param name="device"></param>
        private static void StartListeningForConnectionChanges(IDevice device)
        {
            statusDisposable?.Dispose();
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
            //Global variables
            ScanResults.Clear();
            ConnectedDevice = null;
            Characteristics.Clear();

            //Disposables
            AuthenticationService.authDisposable?.Dispose();
            characteristicsDisposable?.Dispose();
            connectedDeviceDisposable?.Dispose();
            statusDisposable?.Dispose();

            disconnectionDisposable?.Dispose();

        }
    }
}


