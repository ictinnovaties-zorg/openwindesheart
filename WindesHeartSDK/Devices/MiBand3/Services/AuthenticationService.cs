using Plugin.BluetoothLE;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using WindesHeartSDK.Devices.MiBand3.Resources;
using WindesHeartSDK.Helpers;

namespace WindesHeartSDK.Devices.MiBand3.Services
{
    public static class AuthenticationService
    {
        private static IGattCharacteristic authCharacteristic;
        public static IDisposable authDisposable;

        public static async void AuthenticateDevice(IDevice device)
        {
            authCharacteristic = CharacteristicHelper.GetCharacteristic(MiBand3Resource.GuidCharacteristicAuth);
            if (authCharacteristic != null)
            {
                if (BluetoothService.ConnectedDevice != null)
                {
                    Console.WriteLine("Connected device already");
                    return;
                }

                TriggerAuthentication();

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
                            RequestAuthorizationNumber();
                        }
                        else if (data[1] == MiBand3Resource.AuthRequestRandomAuthNumber)
                        {
                            RequestRandomEncryptionKey(data);
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
                    Console.WriteLine("Connection exception: " + exception.Message);
                    return;
                });
            }
            else
            {
                Console.WriteLine("AuthCharacteristic not yet found, trying again..");
                await Task.Delay(2000);
                AuthenticateDevice(device);
            }
        }

        private static async void TriggerAuthentication()
        {
            Console.WriteLine("Authenticating...");
            Console.WriteLine("Writing authentication-key..");
            await authCharacteristic.WriteWithoutResponse(MiBand3Resource.AuthKey);
        }

        private static async void RequestAuthorizationNumber()
        {
            Console.WriteLine("1.Requesting Authorization-number");
            await authCharacteristic.WriteWithoutResponse(MiBand3Resource.RequestNumber);
        }

        private static async void RequestRandomEncryptionKey(byte[] data)
        {
            Console.WriteLine("2.Requesting random encryption key");
            await authCharacteristic.WriteWithoutResponse(Helpers.ConversionHelper.CreateKey(data));
        }
    }
}
