using Plugin.BluetoothLE;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using WindesHeartSDK.Devices.MiBand3Device.Models;
using WindesHeartSDK.Devices.MiBand3Device.Resources;
using WindesHeartSDK.Models;

namespace WindesHeartSDK.Devices.MiBand3Device.Services
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
