using System;

namespace WindesHeartSDK.Models
{
    public class Heartrate
    {
        public int Id { get; set; }
        public byte[] Rawdata { get; set; }
        public DateTime DateTime { get; set; }

        public int HeartrateValue { get; set; }

        public Heartrate(DateTime dateTime, byte[] rawdata)
        {
            DateTime = dateTime;
            Rawdata = rawdata;
            HeartrateValue = rawdata[1];
        }
    }
}
