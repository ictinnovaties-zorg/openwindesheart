using Plugin.BluetoothLE;
using System;
using System.Reactive.Linq;
using WindesHeartSDK.Devices.MiBand3.Resources;
using WindesHeartSDK.Models;

namespace WindesHeartSDK.Devices.MiBand3.Services
{
    public class MiBand3HeartrateService
    {
        private readonly BLEDevice BLEDevice;
        public IDisposable heartrateDisposable;

        public MiBand3HeartrateService(BLEDevice device)
        {
            BLEDevice = device;
        }

        /// <summary>
        /// Add a callback to run everytime the user manually measures their heartrate
        /// </summary>
        /// <param name="callback"></param>
        public void EnableHeartrateUpdates(Action<Heartrate> callback)
        {
            heartrateDisposable?.Dispose();
            heartrateDisposable = BLEDevice.GetCharacteristic(MiBand3Resource.GuidCharacteristicHeartrate).RegisterAndNotify().Subscribe(
                x => callback(new Heartrate())
            );
        }

        /// <summary>
        /// Set the interval for automatic heartrate measurements
        /// </summary>
        /// <param name="minutes"></param>
        public void SetMeasurementInterval(int minutes)
        {
            var Char = BLEDevice.GetCharacteristic(MiBand3Resource.GuidCharacteristicHeartrateControl);
            Char.Write(new byte[] { 0x14, (byte)minutes });
        }
    }
}
