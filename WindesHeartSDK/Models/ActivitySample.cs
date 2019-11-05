using System;

namespace WindesHeartSdk.Model
{
    public class ActivitySample
    {
        public DateTime Timestamp { get; set; }
        public Guid DeviceId { get; set; }
        public long UserId { get; set; }
        public int RawIntensity { get; set; }
        public int Steps { get; set; }
        public int RawKind { get; set; }
        public int HeartRate { get; set; }
    }
}
