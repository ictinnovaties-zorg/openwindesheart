using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WindesHeartSDK.Models;

namespace WindesHeartSDK
{
    public abstract class BLEDevice
    {
        public int Rssi;
        public readonly IDevice Device;
        public List<IGattCharacteristic> Characteristics = new List<IGattCharacteristic>();

        //Services
        public readonly BluetoothService BluetoothService;

        public BLEDevice(int rssi, IDevice device)
        {
            this.Rssi = rssi;
            this.Device = device;
            BluetoothService = new BluetoothService(this);
            Device.WhenConnected().Subscribe(x => OnConnect());
        }

        public abstract void OnConnect();
        public abstract void Connect();
        public abstract void Disconnect();
        public abstract Task<bool> SetTime(System.DateTime dateTime);
        public abstract Task<bool> GetSteps();
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
