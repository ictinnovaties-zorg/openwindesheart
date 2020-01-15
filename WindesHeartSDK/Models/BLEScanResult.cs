// Copyright 2020 Research group ICT innovations 
// in Health Care, Windesheim University of Applied Sciences

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
