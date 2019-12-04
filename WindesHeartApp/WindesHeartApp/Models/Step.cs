using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace WindesHeartApp.Models
{
    public class Step
    {
        public Step() { }
        public Step(DateTime datetime, int stepCount)
        {
            this.DateTime = datetime;
            this.StepCount = stepCount;
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        public int StepCount;
    }
}
