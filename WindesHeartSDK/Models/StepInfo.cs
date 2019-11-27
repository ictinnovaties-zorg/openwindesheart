using WindesHeartSDK.Helpers;

namespace WindesHeartSDK.Models
{
    public class StepInfo
    {
        public readonly byte[] RawData;
        public readonly int StepCount;

        public StepInfo(byte[] data)
        {
            RawData = data;
            byte[] stepsValue = new byte[] { RawData[1], RawData[2] };
            StepCount = ConversionHelper.ToUint16(stepsValue);
        }
    }
}
