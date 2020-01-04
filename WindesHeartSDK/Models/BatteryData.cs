using System;
namespace WindesHeartSDK.Models
{
    public enum BatteryStatus
    {
        Charging,
        NotCharging
    }

    public class BatteryData
    {
        public BatteryStatus Status
        {
            get;
            set;
        }

        public int Percentage
        {
            get;
            set;
        }

        public byte[] RawData
        {
            get;
            set;
        }
    }
}
