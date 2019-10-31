using Plugin.BluetoothLE;
using System;
using System.Threading.Tasks;
using WindesHeartSDK.Devices.MiBand3.Services;
using WindesHeartSDK.Models;

namespace WindesHeartSDK.Devices.MiBand3.Models
{
    public class MiBand3 : WDevice
    {
        //MiBand 3 Services
        private readonly MiBand3BatteryService BatteryService;
        private readonly MiBand3DateTimeService DateTimeService;
        private readonly MiBand3AuthenticationService AuthenticationService;

        public MiBand3(int rssi, IDevice device) : base(rssi, device)
        {
            BatteryService = new MiBand3BatteryService(this);
            DateTimeService = new MiBand3DateTimeService(this);
            AuthenticationService = new MiBand3AuthenticationService(this);
        }

        public async override Task<bool> Connect()
        {
            bool connected = await BluetoothService.Connect();
            Console.WriteLine("Connected: " + connected);
            if (connected)
            {
                //Authentication
                await AuthenticationService.Authenticate();
                return true;
            }
            return false;
        }

        public async override Task<bool> Disconnect()
        {
            return await BluetoothService.Disconnect();
        }

        public override void EnableRealTimeBattery(Action<Battery> getBatteryStatus)
        {
            BatteryService.EnableBatteryStatusUpdates(getBatteryStatus);
        }

        public override Task<Battery> GetBattery()
        {
            return BatteryService.GetCurrentBatteryData();
        }

        public override Task<bool> GetSteps()
        {
            throw new NotImplementedException();
        }

        public async override Task<bool> SetTime(DateTime dateTime)
        {
            return await DateTimeService.SetTime(dateTime);
        }
    }
}
