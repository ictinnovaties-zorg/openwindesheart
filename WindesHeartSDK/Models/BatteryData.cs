// Copyright 2020 Research group ICT innovations 
// in Health Care, Windesheim University of Applied Sciences

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
