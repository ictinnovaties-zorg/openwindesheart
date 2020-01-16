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
using System.Reactive.Linq;
using OpenWindesheart.Devices.MiBand3Device.Models;
using OpenWindesheart.Devices.MiBand3Device.Resources;
using OpenWindesheart.Models;

namespace OpenWindesheart.Devices.MiBand3Device.Services
{
    public class MiBand3HeartrateService
    {
        private readonly MiBand3 _miBand3;
        public IDisposable RealtimeDisposible;

        public MiBand3HeartrateService(MiBand3 device)
        {
            _miBand3 = device;
        }

        /// <summary>
        /// Add a callback to run everytime the user manually measures their heartrate
        /// </summary>
        /// <param name="callback"></param>
        public void EnableRealTimeHeartrate(Action<HeartrateData> callback)
        {
            RealtimeDisposible?.Dispose();
            RealtimeDisposible = _miBand3.GetCharacteristic(MiBand3Resource.GuidHeartrate).RegisterAndNotify().Subscribe(
                x => callback(new HeartrateData(x.Characteristic.Value))
            );
        }

        public void DisableRealTimeHeartrate()
        {
            RealtimeDisposible?.Dispose();
        }

        /// <summary>
        /// Set the interval for automatic heartrate measurements
        /// </summary>
        /// <param name="minutes"></param>
        public async void SetMeasurementInterval(int minutes)
        {
            var Char = _miBand3.GetCharacteristic(MiBand3Resource.GuidHeartRateControl);
            await Char.Write(new byte[] { 0x14, (byte)minutes });
        }
    }
}
