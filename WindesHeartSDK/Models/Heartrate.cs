using System;

namespace WindesHeartSDK.Models
{
    public class Heartrate
    {
        public int Id { get; set; }

        public byte[] Rawdata { get; set; }

        public int HeartrateValue { get; set; }

        public Heartrate(byte[] rawdata)
        {
            Rawdata = rawdata;
            HeartrateValue = rawdata[1];
        }
    }
}
