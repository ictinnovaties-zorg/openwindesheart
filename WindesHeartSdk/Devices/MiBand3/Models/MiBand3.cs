using Plugin.BluetoothLE;
using System;
using System.Threading.Tasks;
using WindesHeartSDK.Devices.MiBand3.Resources;
using WindesHeartSDK.Devices.MiBand3.Services;
using WindesHeartSDK.Models;

namespace WindesHeartSDK.Devices.MiBand3.Models
{
    public class MiBand3 : BLEDevice
    {
        //MiBand 3 Services
        private readonly MiBand3BatteryService BatteryService;
        private readonly MiBand3HeartrateService HeartrateService;
        private readonly MiBand3DateTimeService DateTimeService;
        private readonly MiBand3AuthenticationService AuthenticationService;
        private readonly MiBand3FetchService FetchService;

        public MiBand3(int rssi, IDevice device) : base(rssi, device)
        {
            BatteryService = new MiBand3BatteryService(this);
            HeartrateService = new MiBand3HeartrateService(this);
            DateTimeService = new MiBand3DateTimeService(this);
            AuthenticationService = new MiBand3AuthenticationService(this);
            FetchService = new MiBand3FetchService(this);
        }

        public override void Connect()
        {
            BluetoothService.Connect();
        }

        public async override void Disconnect()
        {
            BluetoothService.Disconnect();
        }

        public override void EnableRealTimeBattery(Action<Battery> getBatteryStatus)
        {
            BatteryService.EnableBatteryStatusUpdates(getBatteryStatus);
        }

        public override Task<Battery> GetBattery()
        {
            return BatteryService.GetCurrentBatteryData();
        }

        public override void EnableRealTimeHeartrate(Action<Heartrate> getHeartrate)
        {
            HeartrateService.EnableHeartrateUpdates(getHeartrate);
        }

        public override void SetHeartrateMeasurementInterval(int minutes)
        {
            HeartrateService.SetMeasurementInterval(minutes);
        }

        public override Task<bool> GetSteps()
        {
            throw new NotImplementedException();
        }

        public async override Task<bool> SetTime(DateTime dateTime)
        {
            return await DateTimeService.SetTime(dateTime);
        }

        public override void FetchData()
        {
            FetchService.InitiateFetching();
        }

        public override void OnConnect()
        {
            Console.WriteLine("Device Connected!");

            //Find unique characteristics
            Device.WhenAnyCharacteristicDiscovered().Subscribe(async characteristic =>
            {
                if (!Characteristics.Contains(characteristic))
                {
                    Characteristics.Add(characteristic);

                    //Check if authCharacteristic has been found, then authenticate
                    if (characteristic.Uuid == MiBand3Resource.GuidCharacteristicAuth)
                    {
                        await AuthenticationService.Authenticate();
                    }
                }
            });
        }
    }
}
