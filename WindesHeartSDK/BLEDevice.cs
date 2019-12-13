using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WindesHeartSdk.Model;
using WindesHeartSDK.Models;

namespace WindesHeartSDK
{
    public abstract class BLEDevice
    {
        public int Rssi { get; set; }
        public readonly IDevice Device;
        public bool NeedsAuthentication = false;
        public string Name { get; set; }
        public List<IGattCharacteristic> Characteristics = new List<IGattCharacteristic>();
        public IDisposable ConnectionDisposable;
        public IDisposable CharacteristicDisposable;

        public Action<ConnectionResult> ConnectionCallback;
        public Action<Object> DisconnectCallback;

        //Services
        public readonly BluetoothService BluetoothService;

        public BLEDevice()
        {
        }

        public BLEDevice(int rssi, IDevice device)
        {
            this.Rssi = rssi;
            this.Device = device;
            this.Name = device.Name;
            BluetoothService = new BluetoothService(this);
            ConnectionDisposable = Device.WhenConnected().Subscribe(x => OnConnect());
        }

        public abstract void DisposeDisposables();
        
        public abstract void OnConnect();

        public abstract void SetFitnessGoal(int goal);
        public abstract void SubscribeToDisconnect(Action<Object> disconnectCallback);
        public abstract void Connect(Action<ConnectionResult> connectCallback);
        public abstract void Disconnect(bool rememberDevice = true);

        public abstract void EnableFitnessGoalNotification(bool enable);

        public bool isConnected()
        {
            if(Device.Status == ConnectionStatus.Connected)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
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
        public abstract void FetchData(DateTime startDate, Action<List<ActivitySample>> callback);

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
