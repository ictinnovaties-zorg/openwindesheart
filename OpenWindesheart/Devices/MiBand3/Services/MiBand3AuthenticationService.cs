/* Copyright 2020 Research group ICT innovations

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License. */

using Plugin.BluetoothLE;
using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading.Tasks;
using OpenWindesheart.Devices.MiBand3Device.Helpers;
using OpenWindesheart.Devices.MiBand3Device.Models;
using OpenWindesheart.Devices.MiBand3Device.Resources;
using OpenWindesheart.Exceptions;

namespace OpenWindesheart.Devices.MiBand3Device.Services
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
                        _miBand3.ConnectionCallback(ConnectionResult.Failed, null);
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
                            _miBand3.ConnectionCallback(ConnectionResult.Succeeded, _miBand3.SecretKey);
                            AuthenticationDisposable.Dispose();
                            return;
                        }
                    }
                    else
                    {
                        _miBand3.Authenticated = false;
                        _miBand3.ConnectionCallback(ConnectionResult.Failed, null);
                        _miBand3.Disconnect();
                    }
                },
                exception =>
                {
                    _miBand3.ConnectionCallback(ConnectionResult.Failed, null);
                    throw new ConnectionException(exception.Message);
                });

                if (_miBand3.SecretKey == null)
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
                _miBand3.ConnectionCallback(ConnectionResult.Failed, null);
                throw new NullReferenceException("AuthCharacteristic is null!");
            }
        }

        private async Task TriggerAuthentication()
        {
            Trace.WriteLine("Writing authentication-key..");
            byte[] KeyBytes = new byte[18];
            byte[] AuthKey = MiBand3ConversionHelper.GenerateAuthKey(); // Key needs to be saved somewhere (BLEDevice or Something)
            _miBand3.SecretKey = AuthKey;
            KeyBytes[0] = 0x01;
            KeyBytes[1] = 0x00;
            Buffer.BlockCopy(AuthKey, 0, KeyBytes, 2, 16);

            await _authCharacteristic.WriteWithoutResponse(KeyBytes);
        }

        private async Task RequestAuthorizationNumber()
        {
            Trace.WriteLine("1.Requesting Authorization-number");
            await _authCharacteristic.WriteWithoutResponse(MiBand3Resource.RequestNumber);
        }

        private async Task RequestRandomEncryptionKey(byte[] data)
        {
            Trace.WriteLine("2.Requesting random encryption key");
            await _authCharacteristic.WriteWithoutResponse(MiBand3ConversionHelper.CreateKey(data, _miBand3.SecretKey));
        }
    }
}
