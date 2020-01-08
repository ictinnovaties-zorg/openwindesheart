using Plugin.BluetoothLE;
using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading.Tasks;
using WindesHeartSDK.Devices.MiBand3Device.Resources;
using WindesHeartSDK.Devices.MiBand4Device.Helpers;
using WindesHeartSDK.Devices.MiBand4Device.Models;
using WindesHeartSDK.Exceptions;

namespace WindesHeartSDK.Devices.MiBand4Device.Services
{
    public class MiBand4AuthenticationService
    {
        private static IGattCharacteristic _authCharacteristic;
        private readonly MiBand4 MiBand4;
        public IDisposable AuthenticationDisposable;

        public MiBand4AuthenticationService(MiBand4 device)
        {
            MiBand4 = device;
        }

        /// <summary>
        /// Authenticates Mi Band 4 devices
        /// </summary>
        /// <exception cref="NullReferenceException">Throws exception if AuthCharacteristic could not be found.</exception>
        /// <exception cref="ConnectionException">Throws exception if authentication went wrong.</exception>
        public async Task Authenticate()
        {
            _authCharacteristic = MiBand4.GetCharacteristic(MiBand3Resource.GuidCharacteristicAuth);
            if (_authCharacteristic != null)
            {
                //Fired when Mi Band 4 is tapped
                AuthenticationDisposable?.Dispose();
                AuthenticationDisposable = _authCharacteristic.RegisterAndNotify().Subscribe(async result =>
                {
                    var data = result.Data;
                    if (data == null)
                    {
                        MiBand4.ConnectionCallback(ConnectionResult.Failed);
                        throw new NullReferenceException("No data found in authentication-result.");
                    }


                    //Check if response is valid
                    if (data[0] == MiBand3Resource.AuthResponse && data[2] == MiBand3Resource.AuthSuccess)
                    {
                        if (data[1] == MiBand3Resource.AuthRequestRandomAuthNumber)
                        {
                            await RequestRandomEncryptionKey(data);
                        }
                        else if (data[1] == MiBand3Resource.AuthSendEncryptedAuthNumber)
                        {
                            Console.WriteLine("Authenticated & Connected!");
                            MiBand4.Authenticated = true;
                            MiBand4.ConnectionCallback(ConnectionResult.Succeeded);
                            AuthenticationDisposable.Dispose();
                            return;
                        }
                    }
                    else
                    {
                        MiBand4.Authenticated = false;
                        MiBand4.ConnectionCallback(ConnectionResult.Failed);
                        throw new ConnectionException("Authentication failed!");
                    }
                },
                exception =>
                {
                    MiBand4.ConnectionCallback(ConnectionResult.Failed);
                    throw new ConnectionException(exception.Message);
                });


                await RequestAuthorizationNumber();
                
            }
            else
            {
                MiBand4.ConnectionCallback(ConnectionResult.Failed);
                throw new NullReferenceException("AuthCharacteristic is null!");
            }
        }

        private async Task RequestAuthorizationNumber()
        {
            Console.WriteLine("1.Requesting Authorization-number");
            await _authCharacteristic.WriteWithoutResponse(MiBand3Resource.RequestNumber);
        }

        private async Task RequestRandomEncryptionKey(byte[] data)
        {
            Console.WriteLine("2.Requesting random encryption key");
            await _authCharacteristic.WriteWithoutResponse(MiBand4ConversionHelper.CreateKey(data));
        }
    }
}
