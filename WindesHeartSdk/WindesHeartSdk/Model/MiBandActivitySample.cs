using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace WindesHeartSdk.Model
{
    public class MiBandActivitySample
    {
        [PrimaryKey, Indexed]
        public DateTime Timestamp { get; set; }
        public Guid DeviceId { get; set; }
        public long UserId { get; set; }
        public int RawIntensity { get; set; }
        public int Steps { get; set; }
        public int RawKind { get; set; }
        public int HeartRate { get; set; }
    }
}
