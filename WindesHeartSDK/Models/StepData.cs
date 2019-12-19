using System;
using WindesHeartSDK.Helpers;

namespace WindesHeartSDK.Models
{
    public class StepData
    {
        public byte[] RawData { get; }
        public int StepCount { get; }

        public StepData()
        {

        }

        public StepData(byte[] rawData)
        {
            RawData = rawData;
            byte[] stepsValue = new byte[] { RawData[1], RawData[2] };
            StepCount = ConversionHelper.ToUint16(stepsValue);
        }
    }
}
