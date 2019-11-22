using System;

namespace WindesHeartSdk.Model
{
    public class ActivitySample
    {
        public DateTime Timestamp { get; set; }
        public Guid DeviceId { get; set; }
        public long UserId { get; set; }
        public int Category { get; set; }
        public int RawIntensity { get; set; }
        public int Steps { get; set; }
        public int RawKind { get; set; }
        public int HeartRate { get; set; }

        public ActivitySample(DateTime timestamp, int category, int intensity, int steps, int heartrate)
        {
            this.Timestamp = timestamp;
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
