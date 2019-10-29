using System;
using Plugin.BluetoothLE;

namespace WindesHeartSDK.Helpers
{
    public static class CharacteristicHelper
    {
        /// <summary>
        /// Get a certain characteristic with its UUID.
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns>IGattCharacteristic</returns>
        public static IGattCharacteristic GetCharacteristic(Guid uuid)
        {
            return BluetoothService.Characteristics.Find(x => x.Uuid == uuid);
        }
    }
}
