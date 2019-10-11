using Plugin.BluetoothLE;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace WindesHeartSDK
{
    public static class BluetoothService
    {

        private static readonly ObservableCollection<IDevice> DeviceList = new ObservableCollection<IDevice>();

        public async static void ScanBands()
        {
            var scanner = CrossBleAdapter.Current.Scan().Subscribe(scanResult =>
            {
                if (!string.IsNullOrWhiteSpace(scanResult.Device.Name) && scanResult.Device.Name.Equals("Mi Band 3"))
                {
                    DeviceList.Add(scanResult.Device);
                }

            });

            await Task.Delay(15000);
            scanner.Dispose();
            PairDevice(DeviceList[0]);
            ConnectDevice(DeviceList[0]);

        }

        public static bool PairDevice(IDevice device)
        {
            bool succes = false;
            if (device.IsPairingAvailable())
            {
                // If Device isn't paired yet pair the device, else 
                if (device.PairingStatus != PairingStatus.Paired)
                {
                    device.PairingRequest().Subscribe(isSuccessful =>
                    {
                        Console.WriteLine("Pairing Succesfull: " + isSuccessful);
                        succes = true;
                    });
                    return succes;
                }
                else
                {
                    Console.WriteLine("Already Paired");
                    return succes;
                }
            }
            return succes;
        }

        public static bool ConnectDevice(IDevice device)
        {
            device.Connect(new ConnectionConfig
            {
                AutoConnect = true,
                AndroidConnectionPriority = ConnectionPriority.High
            });
            Console.WriteLine("Connecting...");

            return true;

        }


    }
}
