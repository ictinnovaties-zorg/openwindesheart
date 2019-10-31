using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Plugin.BluetoothLE;
using WindesHeartSDK.Devices.MiBand3.Resources;
using WindesHeartSDK.Exceptions;
using WindesHeartSDK.Helpers;

namespace WindesHeartSDK.Devices.MiBand3.Services
{
    public static class AuthenticationService
    {
        private static IGattCharacteristic authCharacteristic;
        public static IDisposable authDisposable;

        /// <summary>
        /// Authenticates Mi Band 3 devices
        /// </summary>
        /// <param name="device"></param>
        /// <exception cref="NullReferenceException">Throws exception if AuthCharacteristic could not be found.</exception>
        /// <exception cref="ConnectionException">Throws exception if authentication went wrong.</exception>
        public static async Task AuthenticateDeviceAsync(IDevice device)
        {
            authCharacteristic = CharacteristicHelper.GetCharacteristic(MiBand3Resource.GuidCharacteristicAuth);
            if (authCharacteristic != null)
            {
                if (BluetoothService.ConnectedDevice != null)
                {
                    Console.WriteLine("Connected device already");
                    return;
                }

                //Triggers vibration on Mi Band 3
                await TriggerAuthenticationAsync();

                //Fired when Mi Band 3 is tapped
                authDisposable = authCharacteristic.RegisterAndNotify().Subscribe(async result =>
                {
                    var data = result.Data;
                    if (data == null)
                    {
                        Console.WriteLine("No usable data found in device-response.");
                        return;
                    }

                    //Check if response is valid
                    if (data[0] == MiBand3Resource.AuthResponse && data[2] == MiBand3Resource.AuthSuccess)
                    {
                        if (data[1] == MiBand3Resource.AuthSendKey)
                        {
                            await RequestAuthorizationNumberAsync();
                        }
                        else if (data[1] == MiBand3Resource.AuthRequestRandomAuthNumber)
                        {
                            await RequestRandomEncryptionKeyAsync(data);
                        }
                        else if (data[1] == MiBand3Resource.AuthSendEncryptedAuthNumber)
                        {
                            Console.WriteLine("Authenticated & Connected!");

                            //Set ConnectedDevice
                            BluetoothService.ConnectedDevice = device;
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
                    throw new ConnectionException(exception.Message);
                });
            } else
            {
                throw new NullReferenceException("AuthCharacteristic is null!");
            }
        }

        private static async Task TriggerAuthenticationAsync()
        {
            Console.WriteLine("Authenticating...");
            Console.WriteLine("Writing authentication-key..");
            await authCharacteristic.WriteWithoutResponse(MiBand3Resource.AuthKey);
        }

        private static async Task RequestAuthorizationNumberAsync()
        {
            Console.WriteLine("1.Requesting Authorization-number");
            await authCharacteristic.WriteWithoutResponse(MiBand3Resource.RequestNumber);
        }

        private static async Task RequestRandomEncryptionKeyAsync(byte[] data)
        {
            Console.WriteLine("2.Requesting random encryption key");
            await authCharacteristic.WriteWithoutResponse(Helpers.ConversionHelper.CreateKey(data));
        }
    }
}
