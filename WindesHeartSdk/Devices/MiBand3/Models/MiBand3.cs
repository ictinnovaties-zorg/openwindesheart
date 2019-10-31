using Plugin.BluetoothLE;
using System;
using System.Threading.Tasks;
using WindesHeartSDK.Devices.MiBand3.Services;
using WindesHeartSDK.Models;

namespace WindesHeartSDK.Devices.MiBand3.Models
{
    public class MiBand3 : Device
    {
        public MiBand3(int rssi, IDevice device) : base(rssi, device)
        {
        }

        public async override Task<bool> Connect()
        {
            bool connected = await BluetoothService.ConnectDevice(base.device);
            Console.WriteLine("Connected: " + connected);
            if (connected)
            {
                //Authentication
                AuthenticationService.AuthenticateDevice(device);
                return true;
            }
            return false;
        }

        public async override Task<bool> Disconnect()
        {
            return await BluetoothService.DisconnectDevice(base.device);
        }

        public override Task<Battery> GetBattery()
        {
            return BatteryService.GetCurrentBatteryData();
        }

        public override Task<bool> GetSteps()
        {
            throw new System.NotImplementedException();
        }

        public async override Task<bool> SetTime(DateTime dateTime)
        {
            return await TimeService.SetTime(dateTime);
        }
    }
}
