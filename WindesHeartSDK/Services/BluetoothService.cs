using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using WindesHeartSDK.Models;

namespace WindesHeartSDK
{
    public static class BluetoothService
    {
        public static List<IScanResult> ScanResults = new List<IScanResult>();
        public static IDevice ConnectedDevice;
        public static List<IGattCharacteristic> Characteristics = new List<IGattCharacteristic>();

        public static async void Start()
        {
            var scanResults = await ScanForUniqueDevicesAsync();
            if(scanResults.Count > 0 && scanResults[0].Device != null)
            {
                FindAllCharacteristics(scanResults[0].Device);
                ConnectDevice(scanResults[0].Device);
                DisconnectDevice(ConnectedDevice);
            } else
            {
                Console.WriteLine("No devices found");
            }
        }

        /// <summary>
        /// Scan for devices with a certain name that are not yet connected.
        /// </summary>
        /// <param name="scanTimeInSeconds"></param>
        /// <param name="deviceName"></param>
        /// <returns>List of IScanResult</returns>
        public static async Task<List<IScanResult>> ScanForUniqueDevicesAsync(int scanTimeInSeconds = 10, string deviceName = "Mi Band 3")
        {
            var scanResults = new List<IScanResult>();
            var uniqueGuids = new List<Guid>();

            //Start scanning when adapter is powered on.
            if (CrossBleAdapter.Current.Status == AdapterStatus.PoweredOn)
            {
                //Trigger event and add to devices list
                try
                {
                    Console.WriteLine("Started scanning");
                    var scanner = CrossBleAdapter.Current.Scan().Subscribe(scanResult =>
                    {
                        if (scanResult.Device != null && !string.IsNullOrEmpty(scanResult.Device.Name) && scanResult.Device.Name.Equals(deviceName) && !uniqueGuids.Contains(scanResult.Device.Uuid))
                        {
                            Console.WriteLine(deviceName + " found with signalstrength: " + scanResult.Rssi);
                            scanResults.Add(scanResult);
                            uniqueGuids.Add(scanResult.Device.Uuid);
                        }
                    });

                    //Stop scanning after delayed time.
                    await Task.Delay(scanTimeInSeconds * 1000);
                    Console.WriteLine("Stopped scanning for devices... Amount of unique devices found: " + scanResults.Count);
                    scanner.Dispose();
                }
                catch (Exception exp)
                {
                    Console.WriteLine("Exception thrown: " + exp);
                }
            } else
            {
                Console.WriteLine("Bluetooth-Adapter state is: " + CrossBleAdapter.Current.Status + ". Trying again!");
                await Task.Delay(2000);
                return await ScanForUniqueDevicesAsync(scanTimeInSeconds, deviceName);
            }

            //Order scanresults by descending signal strength
            scanResults = scanResults.OrderByDescending(x => x.Rssi).ToList();

            //Set ScanResults global
            ScanResults = scanResults;
            return scanResults;
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

        /// <summary>
        /// Find all characteristics for a device and store it in the Characteristics property
        /// </summary>
        /// <param name="device"></param>
        public static void FindAllCharacteristics(IDevice device)
        {
            device.Connect(new ConnectionConfig
            {
                AutoConnect = true,
                AndroidConnectionPriority = ConnectionPriority.High
            });

            Characteristics.Clear();
            device.WhenAnyCharacteristicDiscovered().Subscribe(characteristic =>
            {
                if (!Characteristics.Contains(characteristic))
                {
                    Characteristics.Add(characteristic);
                }
            });
        }

        /// <summary>
        /// Get a certain characteristic with its UUID.
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns>IGattCharacteristic</returns>
        public static IGattCharacteristic GetCharacteristic(Guid uuid)
        {
            return Characteristics.Find(x => x.Uuid == uuid);
        }

        public static async void ConnectDevice(IDevice device)
        {
            if(device != null)
            {
                if (Characteristics.Count > 0)
                {
                    var authCharacteristic = GetCharacteristic(MiBand.MiBandResource.GuidCharacteristicAuth);
                    if (authCharacteristic != null)
                    {
                        if (ConnectedDevice != null)
                        {
                            Console.WriteLine("Connected device found, disconnecting..");
                            DisconnectDevice(ConnectedDevice);

                        }

                        Console.WriteLine("Connecting...");
                        await authCharacteristic.WriteWithoutResponse(MiBand.MiBandResource.AuthKey);
                        authCharacteristic.RegisterAndNotify().Timeout(TimeSpan.FromSeconds(20)).Subscribe(async result =>
                        {
                            var data = result.Data;
                            if (data == null)
                            {
                                Console.WriteLine("No data found whilst authenticating");
                                return;
                            }

                            if (data[0] == MiBand.MiBandResource.AuthResponse && data[2] == MiBand.MiBandResource.AuthSuccess)
                            {
                                if (data[1] == MiBand.MiBandResource.AuthSendKey)
                                {
                                    Console.WriteLine("Authenticating.. Requesting Authorization-number");
                                    await authCharacteristic.WriteWithoutResponse(MiBand.MiBandResource.RequestNumber);
                                    Console.WriteLine("Authoriztion-number written..");
                                }
                                else if (data[1] == MiBand.MiBandResource.AuthRequestRandomAuthNumber)
                                {
                                    Console.WriteLine("Authenticating.. Requesting random encryption key");
                                    await authCharacteristic.WriteWithoutResponse(Helpers.ConversionHelper.CreateKey(data));
                                    Console.WriteLine("Encryption key created and written..");
                                }
                                else if (data[1] == MiBand.MiBandResource.AuthSendEncryptedAuthNumber)
                                {
                                    Console.WriteLine("Authenticated & Connected!");

                                    //Set ConnectedDevice
                                    ConnectedDevice = device;
                                    return;
                                }
                            }
                            else
                            {
                                Console.WriteLine("AuthResponse or AuthSuccess not correct");
                                return;
                            }
                        },
                        exception =>
                        {
                            Console.WriteLine("Connection exception: " + exception.Message);
                            return;
                        });
                    }
                    else
                    {
                        Console.WriteLine("AuthCharacteristic not yet found, trying again..");
                        await Task.Delay(2000);
                        ConnectDevice(device);
                    }
                }
                else
                {
                    Console.WriteLine("No Characteristics found yet, trying again..");
                    await Task.Delay(5000);
                    ConnectDevice(device);
                }
            } else
            {
                Console.WriteLine("No device has been given to connect with, be sure your device is not null!");
                return;
            }
        }


        /// <summary>
        /// Disconnect current device.
        /// </summary>
        /// <param name="device"></param>
        /// <returns>bool</returns>
        public static bool DisconnectDevice(IDevice device)
        {
            if(device != null)
            {
                //Cancel the connection
                Console.WriteLine("Disconnecting Device...");
                device.CancelConnection();

                if (device.Status == ConnectionStatus.Disconnected)
                {
                    Console.WriteLine("Disconnected successfully");
                    return true;
                }

                Console.WriteLine("Disconnecting failed!");
                return false;
            }
            Console.WriteLine("No device has been given to disconnect, make sure device is not null!");
            return false;
        }
    }
}


