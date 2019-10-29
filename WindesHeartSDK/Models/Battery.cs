using System;
namespace WindesHeartSDK.Models
{
    public enum StatusEnum
    {
        Charging,
        NotCharging
    }

    public class Battery
    {
        public StatusEnum Status
        {
            get;
            set;
        }

        public int BatteryPercentage
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
