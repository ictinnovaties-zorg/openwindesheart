using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WindesHeartSDK
{
    public static class BluetoothService
    {
        public static Guid GuidCharacteristicAuth = new Guid("00000009-0000-3512-2118-0009af100700");
        public static readonly byte[] AuthKey = { 0x01, 0x08, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x40, 0x41, 0x42, 0x43, 0x44, 0x45 };

        public static async void Start()
        {
            var devices = await ScanForUniqueDevicesAsync();
            await Task.Delay(11000);
            if (devices[0] != null)
            {
                var connected = ConnectDevice(devices[0]);
                if (connected)
                {
                    Console.WriteLine("Connected");
                }
            }       
        }

        /// <summary>
        /// Scan for devices with a certain name that are not yet connected.
        /// </summary>
        /// <param name="scanTimeInSeconds"></param>
        /// <param name="deviceName"></param>
        /// <returns>List of IDevice</returns>
        public static async Task<List<IDevice>> ScanForUniqueDevicesAsync(int scanTimeInSeconds = 10, string deviceName = "Mi Band 3")
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
                    try
                    {
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
                    catch (Exception exp)
                    {
                        Console.WriteLine("Exception thrown: " + exp);
                    }
                }
                else
                {
                    Console.WriteLine("Bluetooth adapter is not powered on, try again!");
                }
            });
            return deviceList;
        }


        /// <summary>
        /// Tries to pair to the devices. If the pairing is successfull returns true.
        /// </summary>
        /// <param name="device"></param>
        /// <returns>bool</returns>
        public static bool PairDevice(IDevice device)
        {
            bool success = false;
            // Checks if devices supports pairing
            if (device.IsPairingAvailable())
            {
                // If device isn't paired yet pair the device
                if (device.PairingStatus != PairingStatus.Paired)
                {
                    device.PairingRequest().Subscribe(isSuccessful =>
                    {
                        Console.WriteLine("Pairing Succesfull: " + isSuccessful);
                        success = isSuccessful;
                    });
                    return success;
                }
                return true;
            }
            Console.WriteLine("Pairing unavailable!");
            return false;
        }

        public static bool ConnectDevice(IDevice device)
        {
            device.Connect(new ConnectionConfig
            {
                AutoConnect = true,
                AndroidConnectionPriority = ConnectionPriority.High
            });
            Console.WriteLine("Connecting...");
            device.WhenAnyCharacteristicDiscovered().Subscribe(characteristic =>
            {
                if (characteristic.Uuid == GuidCharacteristicAuth)
                {
                    characteristic.WriteWithoutResponse(AuthKey).Subscribe(result =>
                    {
                        Console.WriteLine("Connected");
                    });
                }
            });

            return false;
        }


        /// <summary>
        /// Disconnect current device.
        /// </summary>
        /// <param name="device"></param>
        /// <returns>bool</returns>
        public static bool DisconnectDevice(IDevice device)
        {
            //Cancel the connection
            device.CancelConnection();

            if(device.Status == ConnectionStatus.Disconnected)
            {
                return true;
            }

            return false;
        }        
    }
}


