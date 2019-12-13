using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace WindesHeartApp.Models
{
    public enum SleepType
    {
        Awake,
        Light,
        Deep
    }

    [Table("Sleep")]
    public class Sleep
    {
        public Sleep() { }
        public Sleep(DateTime datetime, SleepType sleepType)
        {
            this.DateTime = datetime;
            this.SleepType = sleepType;
        }

        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }

        [Column("DateTime")]
        public DateTime DateTime { get; set; }

        [Column("SleepType")]
        public SleepType SleepType { get; set; }
    }
}
