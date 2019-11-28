using Plugin.BluetoothLE;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using WindesHeartSDK.Devices.MiBand3.Resources;
using WindesHeartSDK.Models;

namespace WindesHeartSDK.Devices.MiBand3.Services
{
    public class MiBand3StepsService
    {
        private readonly BLEDevice BLEDevice;
        private IDisposable realtimeDisposable;

        public MiBand3StepsService(BLEDevice device)
        {
            BLEDevice = device;
        }

        /// <summary>
        /// Add a callback to run everytime the Mi Band updates its step count
        /// </summary>
        /// <param name="callback"></param>
        public void EnableRealTimeSteps(Action<StepInfo> callback)
        {
            realtimeDisposable?.Dispose();
            realtimeDisposable = BLEDevice.GetCharacteristic(MiBand3Resource.GuidCharacteristic7RealtimeSteps).RegisterAndNotify().Subscribe(
                x => callback(new StepInfo(DateTime.Now, x.Characteristic.Value)));
        }

        /// <summary>
        /// Disables real time step count updates
        /// </summary>
        public void DisableRealTimeSteps()
        {
            realtimeDisposable?.Dispose();
        }

        public async Task<StepInfo> GetSteps()
        {
            var steps = await BLEDevice.GetCharacteristic(MiBand3Resource.GuidCharacteristic7RealtimeSteps).Read();
            return new StepInfo(DateTime.Now, steps.Characteristic.Value);
        }
    }
}
