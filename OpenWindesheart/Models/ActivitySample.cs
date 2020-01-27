/* Copyright 2020 Research group ICT innovations in Health Care, Windesheim University of Applied Sciences

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License. */

using System;

namespace OpenWindesheart.Models
{
    public class ActivitySample
    {
        public byte[] RawData { get; }
        public DateTime Timestamp { get; }
        public int UnixEpochTimestamp { get; }
        public int Category { get; }
        public int RawIntensity { get; }
        public int Steps { get; }
        public int HeartRate { get; }

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
    }
}
