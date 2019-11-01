using Plugin.BluetoothLE;
using System;
using System.Reactive.Linq;
using WindesHeartSDK.Devices.MiBand3.Resources;
using WindesHeartSDK.Helpers;
using WindesHeartSDK.Models;

namespace WindesHeartSDK.Devices.MiBand3.Services
{
    public class MiBand3HeartrateService
    {
        public static IDisposable heartrateDisposable;

        public static void EnableHeartrateUpdates(Action<Heartrate> callback)
        {
            heartrateDisposable?.Dispose();
            heartrateDisposable = CharacteristicHelper.GetCharacteristic(MiBand3Resource.GuidCharacteristicHeartrate).RegisterAndNotify().Subscribe(
                x => callback(new Heartrate(x.Characteristic.Value))
            );
        }

        public static void SetMeasurementInterval(int minutes)
        {
            var Char = CharacteristicHelper.GetCharacteristic(MiBand3Resource.GuidCharacteristicHeartrateControl);
            Char.Write(new byte[] { 0x14, (byte)minutes });
        }
    }
}
