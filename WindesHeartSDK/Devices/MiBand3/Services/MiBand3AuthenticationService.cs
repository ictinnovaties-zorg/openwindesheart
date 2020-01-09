using Plugin.BluetoothLE;
using System;
using System.Diagnostics;
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
        private static IGattCharacteristic _authCharacteristic;
        private readonly MiBand3 _miBand3;
        public IDisposable AuthenticationDisposable;
        public MiBand3AuthenticationService(MiBand3 device)
        {
            _miBand3 = device;
        }

        /// <summary>
        /// Authenticates Mi Band 3 devices
        /// </summary>
        /// <exception cref="NullReferenceException">Throws exception if AuthCharacteristic could not be found.</exception>
        /// <exception cref="ConnectionException">Throws exception if authentication went wrong.</exception>
        public async Task Authenticate()
        {
            _authCharacteristic = _miBand3.GetCharacteristic(MiBand3Resource.GuidCharacteristicAuth);
            if (_authCharacteristic != null)
            {
                //Fired when Mi Band 3 is tapped
                AuthenticationDisposable?.Dispose();
                AuthenticationDisposable = _authCharacteristic.RegisterAndNotify().Subscribe(async result =>
                {
                    var data = result.Data;
                    if (data == null)
                    {
                        _miBand3.ConnectionCallback(ConnectionResult.Failed);
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
                            Trace.WriteLine("Authenticated & Connected!");
                            _miBand3.Authenticated = true;
                            _miBand3.ConnectionCallback(ConnectionResult.Succeeded);
                            AuthenticationDisposable.Dispose();
                            return;
                        }
                    }
                    else
                    {
                        _miBand3.Authenticated = false;
                        _miBand3.ConnectionCallback(ConnectionResult.Failed);
                        _miBand3.Disconnect();
                        //throw new ConnectionException("Authentication failed!");
                    }
                },
                exception =>
                {
                    _miBand3.ConnectionCallback(ConnectionResult.Failed);
                    throw new ConnectionException(exception.Message);
                });

                if (_miBand3.NeedsAuthentication)
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
                _miBand3.ConnectionCallback(ConnectionResult.Failed);
                throw new NullReferenceException("AuthCharacteristic is null!");
            }
        }

        private async Task TriggerAuthentication()
        {
            Trace.WriteLine("Authenticating...");
            Trace.WriteLine("Writing authentication-key..");
            await _authCharacteristic.WriteWithoutResponse(MiBand3Resource.AuthKey);
        }

        private async Task RequestAuthorizationNumber()
        {
            Trace.WriteLine("1.Requesting Authorization-number");
            await _authCharacteristic.WriteWithoutResponse(MiBand3Resource.RequestNumber);
        }

        private async Task RequestRandomEncryptionKey(byte[] data)
        {
            Trace.WriteLine("2.Requesting random encryption key");
            await _authCharacteristic.WriteWithoutResponse(MiBand3ConversionHelper.CreateKey(data));
        }
    }
}
