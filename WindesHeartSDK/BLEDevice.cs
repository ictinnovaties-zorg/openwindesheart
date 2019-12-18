using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using WindesHeartSdk.Model;
using WindesHeartSDK.Models;

namespace WindesHeartSDK
{
    public abstract class BLEDevice
    {

        public bool NeedsAuthentication = false;
        public string Name { get => Device.Name; }
        public ConnectionStatus Status { get => Device.Status; }
        public Guid Uuid { get => Device.Uuid; }

        public List<IGattCharacteristic> Characteristics = new List<IGattCharacteristic>();

        protected readonly BluetoothService BluetoothService;

        internal Action<ConnectionResult> ConnectionCallback;
        internal Action<object> DisconnectCallback;
        internal IDisposable ConnectionDisposable;
        internal IDisposable CharacteristicDisposable;
        internal IDisposable RssiDisposable;
        internal readonly IDevice Device;

        public BLEDevice()
        {
        }

        public BLEDevice(IDevice device)
        {
            Device = device;
            BluetoothService = new BluetoothService(this);
            ConnectionDisposable = Device.WhenConnected().Subscribe(x => OnConnect());
        }

        public async Task<int> ReadRssi()
        {
            return await Device.ReadRssi();
        }

        public void OnRssiUpdated(Action callback, TimeSpan? readInterval = null)
        {
            RssiDisposable?.Dispose();
            RssiDisposable = Device.ReadRssiContinuously(readInterval).Subscribe(x => callback());
        }

        public bool IsConnected()
        {
            return Device.IsConnected();
        }

        public bool IsDisconnected()
        {
            return Device.IsDisconnected();
        }

        public bool IsPairingAvailable()
        {
            return Device.IsPairingAvailable();
        }

        public abstract void DisposeDisposables();

        public abstract void OnConnect();

        public abstract void SetFitnessGoal(int goal);
        public abstract void SubscribeToDisconnect(Action<object> disconnectCallback);
        public abstract void Connect(Action<ConnectionResult> connectCallback);
        public abstract void Disconnect(bool rememberDevice = true);
        public abstract void EnableFitnessGoalNotification(bool enable);
        public abstract void SetTimeDisplayFormat(bool is24hours);
        public abstract void SetDateDisplayFormat(bool isddMMYYYY);
        public abstract void SetLanguage(string localeString);
        public abstract bool SetTime(DateTime dateTime);
        public abstract Task<StepInfo> GetSteps();
        public abstract void SetActivateOnLiftWrist(bool activate);
        public abstract void SetActivateOnLiftWrist(DateTime from, DateTime to);
        public abstract void EnableRealTimeSteps(Action<StepInfo> OnStepsChanged);
        public abstract void DisableRealTimeSteps();
        public abstract Task<Battery> GetBattery();
        public abstract void EnableSleepTracking(bool enable);
        public abstract void FetchData(DateTime startDate, Action<List<ActivitySample>> callback, Action<float> progressCallback);

        /// <summary>
        /// Get a certain characteristic with its UUID.
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns>IGattCharacteristic</returns>
        public IGattCharacteristic GetCharacteristic(Guid uuid)
        {
            return Characteristics.Find(x => x.Uuid == uuid);
        }

        public abstract void EnableRealTimeBattery(Action<Battery> getBatteryStatus);
        public abstract void DisableRealTimeBattery();
        public abstract void EnableRealTimeHeartrate(Action<Heartrate> getHeartrate);
        public abstract void DisableRealTimeHeartrate();
        public abstract void SetHeartrateMeasurementInterval(int minutes);
    }

    public enum ConnectionResult { Failed, Succeeded }
}
