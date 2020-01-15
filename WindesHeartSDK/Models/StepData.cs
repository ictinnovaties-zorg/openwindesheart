// Copyright 2020 Research group ICT innovations 
// in Health Care, Windesheim University of Applied Sciences

using WindesHeartSDK.Helpers;

namespace WindesHeartSDK.Models
{
    public class StepData
    {
        public StepData()
        {

        }

        public StepData(byte[] rawData)
        {
            RawData = rawData;
            byte[] stepsValue = new byte[] { RawData[1], RawData[2] };
            StepCount = ConversionHelper.ToUint16(stepsValue);
        }

        public byte[] RawData { get; }
        public int StepCount { get; }        
    }
}
