using System;
using WindesHeartSDK.Exceptions;

namespace WindesHeartSDK.Models
{
    public class HeartrateData
    {
        public byte[] Rawdata { get; }

        public int Heartrate { get; }

        public HeartrateData(byte[] rawdata)
        {
            {
                if (rawdata == null && rawdata[0] == 0)
                {
                    throw new ReadException("Error while reading raw heartrate data");
                }

                Rawdata = rawdata;
                Heartrate = rawdata[1];
            }
        }
    }
}

