using SQLite;
using System;

namespace WindesHeartApp.Models
{
    public class Step
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int StepCount { get; set; }

        public Step() { }

        public Step(DateTime datetime, int stepCount)
        {
            DateTime = datetime;
            StepCount = stepCount;
        }
    }
}
