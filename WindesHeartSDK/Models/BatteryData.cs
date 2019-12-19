using System;
namespace WindesHeartSDK.Models
{
    public enum StatusEnum
    {
        Charging,
        NotCharging
    }

    public class BatteryData
    {
        public StatusEnum Status
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
