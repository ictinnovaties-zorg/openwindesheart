using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using WindesHeartSDK.Devices.MiBand3Device.Models;
using WindesHeartSDK.Devices.MiBand4Device.Models;
using WindesHeartSDK.Models;

namespace WindesHeartSDK
{
    public class BluetoothService
    {
        public static AdapterStatus AdapterStatus;

        private static IDisposable _adapterReadyDisposable;
        private static IDisposable _currentScan;
        private static IDisposable _adapterChangedDisposable;

        private readonly BLEDevice _bleDevice;


        public BluetoothService(BLEDevice device)
        {
            _bleDevice = device;
        }

        /// <summary>
        /// Stops scanning for devices
        /// </summary>
        public static void StopScanning()
        {
            _currentScan?.Dispose();
        }

        /// <summary>
        /// Scan for devices that are not yet connected.
        /// </summary>
        /// <exception cref="System.Exception">Throws exception when trying to start scan when a scan is already running.</exception>
        /// <param name="callback"></param>
        /// <returns>Bool wheter scanning has started</returns>
        public static bool StartScanning(Action<BLEScanResult> callback)
        {
            var uniqueGuids = new List<Guid>();

            //Start scanning when adapter is powered on.
            if (CrossBleAdapter.Current.Status == AdapterStatus.PoweredOn)
            {
                //Trigger event and add to devices list
                Console.WriteLine("Started scanning");
                _currentScan = CrossBleAdapter.Current.Scan().Subscribe(result =>
                {
                    if (result.Device != null && !string.IsNullOrEmpty(result.Device.Name) && !uniqueGuids.Contains(result.Device.Uuid))
                    {
                        //Set device
                        BLEDevice device = GetDevice(result.Device);

                        BLEScanResult scanResult = new BLEScanResult(device, result.Rssi, result.AdvertisementData);
                        if (device != null)
                        {
                            device.NeedsAuthentication = true;
                            callback(scanResult);
                        }
                        uniqueGuids.Add(result.Device.Uuid);
                    }
                });
                return true;
            }
            return false;
        }

        /// <summary>
        /// Calls the callback method when Bluetooth adapter state changes to ready
        /// </summary>
        /// <param name="callback">Called when adapter is ready</param>
        public static void WhenAdapterReady(Action callback)
        {
            _adapterReadyDisposable?.Dispose();
            _adapterReadyDisposable = CrossBleAdapter.Current.WhenReady().Subscribe(adapter => callback());
        }

        /// <summary>
        /// Return whether device is currently scanning for devices.
        /// </summary>
        public static bool IsScanning()
        {
            return CrossBleAdapter.Current.IsScanning;
        }

        /// <summary>
        /// Calls the callback method when Bluetooth adapter status changes
        /// </summary>
        /// <param name="callback">Called when status changed</param>
        public static void OnAdapterChanged(Action callback)
        {
            _adapterChangedDisposable?.Dispose();
            _adapterChangedDisposable = CrossBleAdapter.Current.WhenStatusChanged().Subscribe(adapter => callback());
        }

        /// <summary>
        /// Connect current device
        /// </summary>
        public void Connect()
        {
            Console.WriteLine("Connecting started...");

            //Connect
            _bleDevice.IDevice.Connect(new ConnectionConfig
            {
                AutoConnect = true,
                AndroidConnectionPriority = ConnectionPriority.High
            });
        }

        /// <summary>
        /// Gets a device based on its uuid
        /// </summary>
        /// <param name="uuid">Uuid of device to find</param>
        /// <returns>The device of the uuid</returns>
        public static async Task<BLEDevice> GetKnownDevice(Guid uuid)
        {
            if (uuid != Guid.Empty)
            {
                var knownDevice = await CrossBleAdapter.Current.GetKnownDevice(uuid);

                if (knownDevice != null)
                {
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
        public void Disconnect(bool rememberDevice = true)
        {
            //Cancel the connection
            Console.WriteLine("Disconnecting device..");
            if (Windesheart.PairedDevice != null)
            {
                Windesheart.PairedDevice.Authenticated = false;
                Windesheart.PairedDevice.DisposeDisposables();
                _bleDevice.IDevice.CancelConnection();
                if (!rememberDevice)
                {
                    Windesheart.PairedDevice = null;
                }
            }
        }

        /// <summary>
        /// Enables logging of device status on change.
        /// </summary>
        public static void StartListeningForAdapterChanges()
        {
            bool startListening = false;
            _adapterReadyDisposable?.Dispose();
            _adapterReadyDisposable = CrossBleAdapter.Current.WhenStatusChanged().Subscribe(async status =>
            {
                if (status != AdapterStatus)
                {
                    AdapterStatus = status;
                    if (status == AdapterStatus.PoweredOff && Windesheart.PairedDevice != null)
                    {
                        Windesheart.PairedDevice?.Disconnect();
                    }

                    if (status == AdapterStatus.PoweredOn && Windesheart.PairedDevice != null && startListening)
                    {
                        var tempConnectCallback = Windesheart.PairedDevice.ConnectionCallback;
                        var DisconnectCallback = Windesheart.PairedDevice.DisconnectCallback;
                        var device = await GetKnownDevice(Windesheart.PairedDevice.IDevice.Uuid);

                        if (DisconnectCallback != null)
                        {
                            device?.SubscribeToDisconnect(DisconnectCallback);
                        }
                        device?.Connect(tempConnectCallback);
                    }
                    startListening = true;
                }
            });
        }


        /// <summary>
        /// Returns the right BLEDevice based on the ScanResult
        /// </summary>
        private static BLEDevice GetDevice(IDevice device)
        {
            var name = device.Name;

            switch (name)
            {
                case "Mi Band 3":
                case "Xiaomi Mi Band 3":
                    return new MiBand3(device);
                case "Mi Smart Band 4":
                    return new MiBand4(device);

                    //Create additional cases for other devices.
            }
            return null;
        }
    }
}


