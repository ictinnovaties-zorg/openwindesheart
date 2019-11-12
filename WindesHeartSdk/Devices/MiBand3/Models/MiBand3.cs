using Plugin.BluetoothLE;
using System;
using System.Threading.Tasks;
using WindesHeartSDK.Devices.MiBand3Device.Resources;
using WindesHeartSDK.Devices.MiBand3Device.Services;
using WindesHeartSDK.Models;

namespace WindesHeartSDK.Devices.MiBand3Device.Models
{
    public class MiBand3 : BLEDevice
    {
        //MiBand 3 Services
        private readonly MiBand3BatteryService BatteryService;
        private readonly MiBand3HeartrateService HeartrateService;
        private readonly MiBand3DateTimeService DateTimeService;
        private readonly MiBand3StepsService StepsService;
        private readonly MiBand3AuthenticationService AuthenticationService;
        private readonly MiBand3FetchService FetchService;

        public MiBand3(int rssi, IDevice device) : base(rssi, device)
        {
            BatteryService = new MiBand3BatteryService(this);
            HeartrateService = new MiBand3HeartrateService(this);
            DateTimeService = new MiBand3DateTimeService(this);
            AuthenticationService = new MiBand3AuthenticationService(this);
            FetchService = new MiBand3FetchService(this);
            StepsService = new MiBand3StepsService(this);
        }

        public override void Connect()
        {
            BluetoothService.Connect();
        }

        public override void Disconnect()
        {
            BluetoothService.Disconnect();
        }

        public override void EnableRealTimeBattery(Action<Battery> getBatteryStatus)
        {
            BatteryService.EnableRealTimeBattery(getBatteryStatus);
        }

        public override Task<Battery> GetBattery()
        {
            return BatteryService.GetCurrentBatteryData();
        }

        public override void EnableRealTimeHeartrate(Action<Heartrate> getHeartrate)
        {
            HeartrateService.EnableRealTimeHeartrate(getHeartrate);
        }

        public override void SetHeartrateMeasurementInterval(int minutes)
        {
            HeartrateService.SetMeasurementInterval(minutes);
        }

        public override Task<StepInfo> GetSteps()
        {
            return StepsService.GetSteps();
        }

        public override void DisableRealTimeSteps()
        {
            StepsService.DisableRealTimeSteps();
        }

        public override void EnableRealTimeSteps(Action<StepInfo> onStepsChanged)
        {
            StepsService.EnableRealTimeSteps(onStepsChanged);
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
                        //Check if this is a new connection that needs authentication
                        if (!Authenticated)
                        {
                            await AuthenticationService.Authenticate();
                            Authenticated = true;
                        }
                    }
                }
            });
        }

        public override void DisableRealTimeBattery()
        {
            BatteryService.DisableRealTimeBattery();
        }

        public override void DisableRealTimeHeartrate()
        {
            HeartrateService.DisableRealTimeHeartrate();
        }
    }
}
