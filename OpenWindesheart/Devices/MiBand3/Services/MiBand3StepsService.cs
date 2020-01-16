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
using System.Threading.Tasks;
using OpenWindesheart.Devices.MiBand3Device.Models;
using OpenWindesheart.Devices.MiBand3Device.Resources;
using OpenWindesheart.Models;

namespace OpenWindesheart.Devices.MiBand3Device.Services
{
    public class MiBand3StepsService
    {
        private readonly MiBand3 _miBand3;
        public IDisposable realtimeDisposable;

        public MiBand3StepsService(MiBand3 device)
        {
            _miBand3 = device;
        }

        /// <summary>
        /// Add a callback to run everytime the Mi Band updates its step count
        /// </summary>
        /// <param name="callback"></param>
        public void EnableRealTimeSteps(Action<StepData> callback)
        {
            realtimeDisposable?.Dispose();
            realtimeDisposable = _miBand3.GetCharacteristic(MiBand3Resource.GuidStepsInfo).RegisterAndNotify().Subscribe(
                x => callback(new StepData(x.Characteristic.Value)));
        }

        /// <summary>
        /// Disables real time step count updates
        /// </summary>
        public void DisableRealTimeSteps()
        {
            realtimeDisposable?.Dispose();
        }

        public async Task<StepData> GetSteps()
        {
            if (_miBand3.IsAuthenticated())
            {
                var steps = await _miBand3.GetCharacteristic(MiBand3Resource.GuidStepsInfo).Read();
                return new StepData(steps.Characteristic.Value);
            }
            else
            {
                return new StepData(new byte[] { 0, 0, 0 });
            }
        }
    }
}
