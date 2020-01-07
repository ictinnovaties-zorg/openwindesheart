using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using WindesHeartSDK.Models;

namespace WindesHeartSDK
{
    public abstract class BLEDevice
    {
        public string Name { get => IDevice.Name; }
        public ConnectionStatus Status { get => IDevice.Status; }
        public Guid Uuid { get => IDevice.Uuid; }
        public List<IGattCharacteristic> Characteristics = new List<IGattCharacteristic>();
        public readonly IDevice IDevice;
        public bool Authenticated = false;
        protected readonly BluetoothService BluetoothService;
        public bool NeedsAuthentication = false;

        internal Action<ConnectionResult> ConnectionCallback;
        internal Action<object> DisconnectCallback;
        internal IDisposable ConnectionDisposable;
        internal IDisposable CharacteristicDisposable;

        public BLEDevice()
        {
        }

        public BLEDevice(IDevice device)
        {
            IDevice = device;
            BluetoothService = new BluetoothService(this);
            ConnectionDisposable = IDevice.WhenConnected().Subscribe(x => OnConnect());
        }

        public bool IsConnected()
        {
            return IDevice.IsConnected();
        }

        public bool IsDisconnected()
        {
            return IDevice.IsDisconnected();
        }

        public bool IsAuthenticated()
        {
            return (IsConnected() && Authenticated);
        }

        public bool IsPairingAvailable()
        {
            return IDevice.IsPairingAvailable();
        }

        public abstract void DisposeDisposables();
        public abstract void OnConnect();
        public abstract void SetStepGoal(int steps);
        public abstract void SubscribeToDisconnect(Action<object> disconnectCallback);
        public abstract void Connect(Action<ConnectionResult> connectCallback);
        public abstract void Disconnect(bool rememberDevice = true);
        public abstract void EnableFitnessGoalNotification(bool enable);
        public abstract void SetTimeDisplayFormat(bool is24hours);
        public abstract void SetDateDisplayFormat(bool isddMMYYYY);
        public abstract void SetLanguage(string localeString);
        public abstract void SetTime(DateTime dateTime);

        public abstract void SetActivateOnLiftWrist(bool activate);
        public abstract void SetActivateOnLiftWrist(DateTime from, DateTime to);
        public abstract void EnableRealTimeSteps(Action<StepData> OnStepsChanged);
        public abstract void DisableRealTimeSteps();
        public abstract Task<StepData> GetSteps();
        public abstract Task<BatteryData> GetBattery();
        public abstract void EnableSleepTracking(bool enable);
        public abstract void GetSamples(DateTime startDate, Action<List<ActivitySample>> finishedCallback, Action<int> remainingSamplesCallback);

        /// <summary>
        /// Get a certain characteristic with its UUID.
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns>IGattCharacteristic</returns>
        public IGattCharacteristic GetCharacteristic(Guid uuid)
        {
            return Characteristics.Find(x => x.Uuid == uuid);
        }

        public abstract void EnableRealTimeBattery(Action<BatteryData> getBatteryStatus);
        public abstract void DisableRealTimeBattery();
        public abstract void EnableRealTimeHeartrate(Action<HeartrateData> getHeartrate);
        public abstract void DisableRealTimeHeartrate();
        public abstract void SetHeartrateMeasurementInterval(int minutes);
    }

    public enum ConnectionResult { Failed, Succeeded }
}
