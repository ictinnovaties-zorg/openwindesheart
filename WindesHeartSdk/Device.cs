using Plugin.BluetoothLE;
using System.Threading.Tasks;
using WindesHeartSDK.Models;

namespace WindesHeartSDK
{
    public abstract class Device
    {
        public int Rssi;
        public IDevice device;

        public Device(int rssi, IDevice device)
        {
            this.Rssi = rssi;
            this.device = device;
            BluetoothService.FindAllCharacteristics(device);
        }
        public abstract Task<bool> Connect();
        public abstract Task<bool> Disconnect();
        public abstract Task<bool> SetTime();
        public abstract Task<bool> GetSteps();
        public abstract Task<Battery> GetBattery();
    }
}
