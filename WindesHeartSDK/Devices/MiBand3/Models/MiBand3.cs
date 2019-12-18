using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WindesHeartSdk.Model;
using WindesHeartSDK.Devices.MiBand3Device.Resources;
using WindesHeartSDK.Devices.MiBand3Device.Services;
using WindesHeartSDK.Models;

namespace WindesHeartSDK.Devices.MiBand3.Models
{
    public class MiBand3 : BLEDevice
    {
        //MiBand 3 Services
        private readonly MiBand3BatteryService _batteryService;
        private readonly MiBand3HeartrateService _heartrateService;
        private readonly MiBand3DateTimeService _dateTimeService;
        private readonly MiBand3StepsService _stepsService;
        private readonly MiBand3AuthenticationService _authenticationService;
        private readonly MiBand3FetchService _fetchService;
        private readonly MiBand3ConfigurationService _configurationService;

        public MiBand3(int rssi, IDevice device) : base(rssi, device)
        {
            _batteryService = new MiBand3BatteryService(this);
            _heartrateService = new MiBand3HeartrateService(this);
            _dateTimeService = new MiBand3DateTimeService(this);
            _authenticationService = new MiBand3AuthenticationService(this);
            _fetchService = new MiBand3FetchService(this);
            _stepsService = new MiBand3StepsService(this);
            _configurationService = new MiBand3ConfigurationService(this);
        }

        public MiBand3() : base()
        {

        }

        public override void Connect(Action<ConnectionResult> connectCallback)
        {
            ConnectionCallback = connectCallback;
            BluetoothService.Connect();
        }

        public override void SubscribeToDisconnect(Action<Object> disconnectCallback)
        {
            DisconnectCallback = disconnectCallback;
            Device.WhenDisconnected().Subscribe(observer => DisconnectCallback(observer));
        }

        public override void Disconnect(bool rememberDevice = true)
        {
            BluetoothService.Disconnect(rememberDevice);
        }

        public override void SetTimeDisplayFormat(bool is24Hours)
        {
            _configurationService.SetTimeDisplayUnit(is24Hours);
        }

        public override void SetFitnessGoal(int goal)
        {
            _configurationService.SetFitnessGoal(goal);
        }

        public override void EnableFitnessGoalNotification(bool enable)
        {
            _configurationService.EnableStepGoalNotification(enable);
        }

        public override void SetDateDisplayFormat(bool isddMMYYYY)
        {
            _configurationService.SetDateDisplayUnit(isddMMYYYY);
        }

        public override void DisposeDisposables()
        {
            _authenticationService.AuthenticationDisposable?.Dispose();
            _stepsService.realtimeDisposable?.Dispose();
            _heartrateService.RealtimeDisposible?.Dispose();
            _batteryService.RealTimeDisposible?.Dispose();
            ConnectionDisposable?.Dispose();
            CharacteristicDisposable?.Dispose();
        }

        public override void SetLanguage(string localeString)
        {
            _configurationService.SetLanguage(localeString);
        }

        public override void SetActivateOnLiftWrist(bool activate)
        {
            _configurationService.SetActivateOnWristLift(activate);
        }

        public override void SetActivateOnLiftWrist(DateTime from, DateTime to)
        {
            _configurationService.SetActivateOnWristLift(from, to);
        }

        public override void EnableRealTimeBattery(Action<Battery> getBatteryStatus)
        {
            _batteryService.EnableRealTimeBattery(getBatteryStatus);
        }

        public override Task<Battery> GetBattery()
        {
            return _batteryService.GetCurrentBatteryData();
        }

        public override void EnableRealTimeHeartrate(Action<Heartrate> getHeartrate)
        {
            _heartrateService.EnableRealTimeHeartrate(getHeartrate);
        }

        public override void SetHeartrateMeasurementInterval(int minutes)
        {
            _heartrateService.SetMeasurementInterval(minutes);
        }

        public override Task<StepInfo> GetSteps()
        {
            return _stepsService.GetSteps();
        }

        public override void DisableRealTimeSteps()
        {
            _stepsService.DisableRealTimeSteps();
        }

        public override void EnableRealTimeSteps(Action<StepInfo> onStepsChanged)
        {
            _stepsService.EnableRealTimeSteps(onStepsChanged);
        }

        public override bool SetTime(DateTime dateTime)
        {
            return _dateTimeService.SetTime(dateTime);
        }

        public override void FetchData(DateTime startDate, Action<List<ActivitySample>> finishedCallback, Action<int> remainingSamplesCallback)
        {
            _fetchService.StartFetching(startDate, finishedCallback, remainingSamplesCallback);
        }

        public override void EnableSleepTracking(bool enable)
        {
            _configurationService.EnableSleepTracking(enable);
        }

        public override void OnConnect()
        {
            Console.WriteLine("Device Connected!");

            Windesheart.ConnectedDevice = this;

            //Check if bluetooth-state changes to off and then on, to enable reconnection management
            BluetoothService.StartListeningForAdapterChanges();

            Characteristics?.Clear();

            //Find unique characteristics
            CharacteristicDisposable = Device.WhenAnyCharacteristicDiscovered().Subscribe(async characteristic =>
            {
                if (characteristic != null && !Characteristics.Contains(characteristic))
                {
                    Characteristics.Add(characteristic);

                    //Check if authCharacteristic has been found, then authenticate
                    if (characteristic.Uuid == MiBand3Resource.GuidCharacteristicAuth)
                    {
                        //Check if this is a new connection that needs authentication
                        await _authenticationService.Authenticate();
                    }
                }
            });
        }

        public override void DisableRealTimeBattery()
        {
            _batteryService.DisableRealTimeBattery();
        }

        public override void DisableRealTimeHeartrate()
        {
            _heartrateService.DisableRealTimeHeartrate();
        }
    }
}
