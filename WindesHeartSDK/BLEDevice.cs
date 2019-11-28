using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public void DisposeDisposables()
        {
            ConnectionDisposable?.Dispose();
            CharacteristicDisposable?.Dispose();
        }

        public abstract void OnConnect();
        public abstract void Connect();
        public abstract void Disconnect();
        public abstract void SetTimeDisplayUnit(bool is24hours);
        public abstract void SetDateDisplayFormat(bool isddMMYYYY);
        public abstract void SetLanguage(string localeString);
        public abstract bool SetTime(DateTime dateTime);
        public abstract Task<StepInfo> GetSteps();
        public abstract void SetActivateOnLiftWrist(bool activate);
        public abstract void SetActivateOnLiftWrist(DateTime from, DateTime to);
        public abstract void EnableRealTimeSteps(Action<StepInfo> OnStepsChanged);
        public abstract void DisableRealTimeSteps();
        public abstract Task<Battery> GetBattery();

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
        public abstract void EnableRealTimeHeartrate(Action<Heartrate> getHeartrate);

        public abstract void SetHeartrateMeasurementInterval(int minutes);
    }
}
