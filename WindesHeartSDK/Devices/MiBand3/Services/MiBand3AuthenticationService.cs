using Plugin.BluetoothLE;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using WindesHeartSDK.Devices.MiBand3Device.Helpers;
using WindesHeartSDK.Devices.MiBand3Device.Models;
using WindesHeartSDK.Devices.MiBand3Device.Resources;
using WindesHeartSDK.Exceptions;

namespace WindesHeartSDK.Devices.MiBand3Device.Services
{
    public class MiBand3AuthenticationService
    {
        private static IGattCharacteristic authCharacteristic;
        private readonly BLEDevice BLEDevice;
        private IDisposable AuthenticationDisposable;
        public MiBand3AuthenticationService(BLEDevice device)
        {
            BLEDevice = device;
        }

        /// <summary>
        /// Authenticates Mi Band 3 devices
        /// </summary>
        /// <exception cref="NullReferenceException">Throws exception if AuthCharacteristic could not be found.</exception>
        /// <exception cref="ConnectionException">Throws exception if authentication went wrong.</exception>
        public async Task Authenticate()
        {
            authCharacteristic = BLEDevice.GetCharacteristic(MiBand3Resource.GuidCharacteristicAuth);
            if (authCharacteristic != null)
            {
                //Fired when Mi Band 3 is tapped
                AuthenticationDisposable?.Dispose();
                AuthenticationDisposable = authCharacteristic.RegisterAndNotify().Subscribe(async result =>
                {
                    var data = result.Data;
                    if (data == null)
                    {
                        throw new NullReferenceException("No data found in authentication-result.");
                    }

                    //Check if response is valid
                    if (data[0] == MiBand3Resource.AuthResponse && data[2] == MiBand3Resource.AuthSuccess)
                    {
                        if (data[1] == MiBand3Resource.AuthSendKey)
                        {
                            await RequestAuthorizationNumber();
                        }
                        else if (data[1] == MiBand3Resource.AuthRequestRandomAuthNumber)
                        {
                            await RequestRandomEncryptionKey(data);
                        }
                        else if (data[1] == MiBand3Resource.AuthSendEncryptedAuthNumber)
                        {
                            Console.WriteLine("Authenticated & Connected!");
                            AuthenticationDisposable.Dispose();
                            return;
                        }
                    }
                    else
                    {
                        throw new ConnectionException("Authentication failed!");
                    }
                },
                exception =>
                {
                    throw new ConnectionException(exception.Message);
                });

                if (BLEDevice.NeedsAuthentication)
                {
                    //Triggers vibration on device
                    await TriggerAuthentication();
                }
                else
                {
                    //Continues session with authorization-number
                    await RequestAuthorizationNumber();
                }
            }
            else
            {
                throw new NullReferenceException("AuthCharacteristic is null!");
            }
        }

        private async Task TriggerAuthentication()
        {
            Console.WriteLine("Authenticating...");
            Console.WriteLine("Writing authentication-key..");
            await authCharacteristic.WriteWithoutResponse(MiBand3Resource.AuthKey);
        }

        private async Task RequestAuthorizationNumber()
        {
            Console.WriteLine("1.Requesting Authorization-number");
            await authCharacteristic.WriteWithoutResponse(MiBand3Resource.RequestNumber);
        }

        private async Task RequestRandomEncryptionKey(byte[] data)
        {
            Console.WriteLine("2.Requesting random encryption key");
            await authCharacteristic.WriteWithoutResponse(MiBand3ConversionHelper.CreateKey(data));
        }
    }
}
