using Plugin.BluetoothLE;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WindesHeartSDK
{
    public class Windesheart
    {
        public static List<IScanResult> ScanResults = new List<IScanResult>();

        /// <summary>
        /// Scan for devices that are not yet connected.
        /// </summary>
        /// <exception cref="System.Exception">Throws exception when trying to start scan when a scan is already running.</exception>
        /// <param name="scanTimeInSeconds"></param>
        /// <returns>List of IScanResult</returns>
        public static async Task<List<Device>> ScanForDevices(int scanTimeInSeconds = 10)
        {
            return await BluetoothService.ScanForUniqueDevicesAsync(scanTimeInSeconds);
        }
    }
}
