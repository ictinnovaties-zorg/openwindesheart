using Plugin.BluetoothLE;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using WindesHeartSDK.Devices.MiBand3Device.Models;
using WindesHeartSDK.Devices.MiBand3Device.Resources;
using WindesHeartSDK.Models;

namespace WindesHeartSDK.Devices.MiBand3Device.Services
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
