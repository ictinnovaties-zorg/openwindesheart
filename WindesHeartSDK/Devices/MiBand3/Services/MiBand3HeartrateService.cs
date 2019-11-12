using Plugin.BluetoothLE;
using System;
using System.Reactive.Linq;
using WindesHeartSDK.Devices.MiBand3Device.Models;
using WindesHeartSDK.Devices.MiBand3Device.Resources;
using WindesHeartSDK.Models;

namespace WindesHeartSDK.Devices.MiBand3Device.Services
{
    public class MiBand3HeartrateService
    {
        private readonly MiBand3 MiBand;
        public IDisposable heartrateDisposable;

        public MiBand3HeartrateService(MiBand3 device)
        {
            MiBand = device;
        }

        /// <summary>
        /// Add a callback to run everytime the user manually measures their heartrate
        /// </summary>
        /// <param name="callback"></param>
        public void EnableHeartrateUpdates(Action<Heartrate> callback)
        {
            heartrateDisposable?.Dispose();
            heartrateDisposable = MiBand.GetCharacteristic(MiBand3Resource.GuidCharacteristicHeartrate).RegisterAndNotify().Subscribe(
                x => callback(new Heartrate(x.Characteristic.Value))
            );
        }

        /// <summary>
        /// Set the interval for automatic heartrate measurements
        /// </summary>
        /// <param name="minutes"></param>
        public void SetMeasurementInterval(int minutes)
        {
            var Char = MiBand.GetCharacteristic(MiBand3Resource.GuidCharacteristicHeartrateControl);
            Char.Write(new byte[] { 0x14, (byte)minutes });
        }
    }
}
