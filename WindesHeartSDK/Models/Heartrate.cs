using WindesHeartSDK.Exceptions;

namespace WindesHeartSDK.Models
{
    public class Heartrate
    {
        public byte[] Rawdata { get; set; }

        public int HeartrateValue { get; set; }

        public Heartrate(byte[] rawdata)
        {
            if (rawdata != null && rawdata[0] != 0)
            {
                throw new ReadException("Error while reading raw heartrate data");
            }

            Rawdata = rawdata;
            HeartrateValue = rawdata[1];
        }
    }
}
