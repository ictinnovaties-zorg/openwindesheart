using SQLite;
using System;

namespace WindesHeartApp.Models
{
    [Table("Steps")]
    public class Step
    {
        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }

        [Column("DateTime")]
        public DateTime DateTime { get; set; }

        [Column("StepCount")]
        public int StepCount { get; set; }

        public Step() { }
        public Step(DateTime datetime, int stepCount)
        {
            DateTime = datetime;
            StepCount = stepCount;
        }
    }
}
