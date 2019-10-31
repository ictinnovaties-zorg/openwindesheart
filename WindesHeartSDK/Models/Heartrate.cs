namespace WindesHeartSDK.Models
{
    public class Heartrate
    {
        public byte[] rawdata { get; set; }

        public int heartrate { get; set; }

        public Heartrate(byte[] rawdata)
        {
            this.rawdata = rawdata;
            heartrate = rawdata[0];
        }
    }
}
