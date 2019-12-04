using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using WindesHeartSDK.Helpers;

namespace WindesHeartApp.Data.Models
{
    public class StepsModel
    {
        [PrimaryKey]
        public int Id { get; set; }
        public byte[] RawData { get; set; }
        public int StepCount { get; set; }
        public DateTime DateTime { get; set; }

        public StepsModel()
        {

        }

        public StepsModel(DateTime dateTime, byte[] rawData)
        {
            DateTime = dateTime;
            RawData = rawData;
            byte[] stepsValue = new byte[] { RawData[1], RawData[2] };
            StepCount = ConversionHelper.ToUint16(stepsValue);
        }
    }
}
