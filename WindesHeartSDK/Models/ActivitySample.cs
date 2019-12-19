using System;

namespace WindesHeartSDK.Models
{
    public class ActivitySample
    {
        public byte[] RawData { get; set; }
        public DateTime Timestamp { get; set; }
        public int UnixEpochTimestamp { get; set; }
        public int Category { get; set; }
        public int RawIntensity { get; set; }
        public int Steps { get; set; }
        public int HeartRate { get; set; }

        public ActivitySample(DateTime timestamp, int category, int intensity, int steps, int heartrate, byte[] rawdata = null)
        {
            this.RawData = rawdata;
            this.Timestamp = timestamp;
            this.UnixEpochTimestamp = (Int32)(timestamp.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            this.Category = category;
            this.RawIntensity = intensity;
            this.Steps = steps;
            this.HeartRate = heartrate;
        }

        public override string ToString()
        {
            return "Date = " + Timestamp + " Category = " + Category + " Intensity = " + RawIntensity + " Steps = " + Steps + " Heartrate = " + HeartRate;
        }
    }
}
