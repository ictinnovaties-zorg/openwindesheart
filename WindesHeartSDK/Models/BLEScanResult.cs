using Plugin.BluetoothLE;

namespace WindesHeartSDK.Models
{
    public class BLEScanResult
    {
        public BLEDevice Device { get; }
        public int Rssi { get; }
        public IAdvertisementData AdvertisementData { get; }

        public BLEScanResult(BLEDevice device, int rssi, IAdvertisementData advertisementData)
        {
            Device = device;
            Rssi = rssi;
            AdvertisementData = advertisementData;
        }
    }
}
