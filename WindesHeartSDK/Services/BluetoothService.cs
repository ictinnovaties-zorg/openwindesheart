using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using WindesHeartSDK.Devices.MiBand3Device.Models;
using WindesHeartSDK.Exceptions;

namespace WindesHeartSDK
{
    public class BluetoothService
    {
        //Globals
        private readonly BLEDevice BLEDevice;
        private IDevice IDevice => BLEDevice.Device;
        
        private static AdapterStatus AdapterStatus;

        private static IDisposable AdapterDisposable;

        public BluetoothService(BLEDevice device)
        {
            BLEDevice = device;
        }

        /// <summary>
        /// Scan when Bluetooth-adapter is ready to scan.
        /// </summary>
        /// <param name="scanTimeInSeconds"></param>
        /// <returns>List<BLEDevice></returns>
        public static async Task<ObservableCollection<BLEDevice>> ScanWhenAdapterReady(int scanTimeInSeconds = 10) 
        {
            var scanResults = new ObservableCollection<BLEDevice>();
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
        public static async Task<ObservableCollection<BLEDevice>> ScanForUniqueDevicesAsync(int scanTimeInSeconds = 10)
        {
            var scanResults = new ObservableCollection<BLEDevice>();
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
                        BLEDevice device = GetDevice(scanResult.Device, scanResult.Rssi);

                        if (device != null)
                        {
                            device.NeedsAuthentication = true;
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
                scanResults = new ObservableCollection<BLEDevice>(scanResults.OrderByDescending(x => x.Rssi).ToList());
            }

            return scanResults;
        }

        public void Connect()
        {            
            Console.WriteLine("Connecting started...");

            //Connect
            IDevice.Connect(new ConnectionConfig
            {
                AutoConnect = true,
                AndroidConnectionPriority = ConnectionPriority.High
            });
        }

        public static async Task<BLEDevice> GetKnownDevice(Guid uuid)
        {
            if (uuid != Guid.Empty)
            {
                var knownDevice = await CrossBleAdapter.Current.GetKnownDevice(uuid);
                
                if (knownDevice != null) { 
                    var bleDevice = GetDevice(knownDevice);
                    bleDevice.NeedsAuthentication = false;
                    return bleDevice;
                }
            }
            return null;
        }

        /// <summary>
        /// Disconnect current device.
        /// </summary>
        public void Disconnect()
        {
            //Cancel the connection
            Console.WriteLine("Disconnecting device..");
            IDevice.CancelConnection();
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
                    AdapterStatus = status;
                    if (status == AdapterStatus.PoweredOff && Windesheart.ConnectedDevice != null && startListening)
                    {
                        Windesheart.ConnectedDevice?.DisposeDisposables();
                        Windesheart.ConnectedDevice?.Disconnect();
                    }

                    if (status == AdapterStatus.PoweredOn && Windesheart.ConnectedDevice != null && startListening)
                    {
                        var device = await GetKnownDevice(Windesheart.ConnectedDevice.Device.Uuid);
                        device?.Connect();
                    }
                    startListening = true;
                }
            });
        }


        /// <summary>
        /// Returns the right WDevice based on the ScanResult
        /// </summary>
        private static BLEDevice GetDevice(IDevice device, int rssi = 0)
        {
            var name = device.Name;
            
            if (name.Equals("Mi Band 3") || name.Equals("Xiaomi Mi Band 3"))
            {
                return new MiBand3(rssi, device);
            }

            //Create additional if-statements for devices other than Mi Band 3/Xiami Band 3.
            return null;
        }
    }
}


