using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Plugin.BluetoothLE;

namespace WindesHeartSdk.Helpers
{
    static class BleTransactionHelper
    {
        public static IDevice Device;
        public static List<IGattCharacteristic> Characteristics = new List<IGattCharacteristic>();
        public static async Task TryWrite(Guid characteristic, byte[] bytes)
        {
            try
            {
                await GetCharacteristic(characteristic).Write(bytes);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
        
        public static async Task TryWriteWithoutResponse(Guid characteristic, byte[] bytes)
        {
            try
            {
                await GetCharacteristic(characteristic).WriteWithoutResponse(bytes);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public static IGattCharacteristic GetCharacteristic(Guid uuid)
        {
            return Characteristics.Find(x => x.Uuid == uuid);
        }
    }
}
