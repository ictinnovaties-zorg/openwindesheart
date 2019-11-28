using System;
using WindesHeartSDK.Exceptions;

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
            if (rawdata != null && rawdata[0] != 0)
            {
                throw new ReadException("Error while reading raw heartrate data");
            }

            DateTime = dateTime;
            Rawdata = rawdata;
            HeartrateValue = rawdata[1];
        }
    }
}
