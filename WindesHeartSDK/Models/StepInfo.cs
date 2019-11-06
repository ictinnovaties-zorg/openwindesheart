using System;
using System.Collections.Generic;
using System.Text;
using WindesHeartSDK.Helpers;

namespace WindesHeartSDK.Models
{
    public class StepInfo
    {
        private readonly byte[] Data;

        public StepInfo(byte[] data)
        {
            Data = data;
        }

        public int GetStepCount()
        {
            byte[] stepsValue = new byte[] { Data[1], Data[2] };
            return ConversionHelper.ToUint16(stepsValue);
        }
    }
}
