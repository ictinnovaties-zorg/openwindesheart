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
    public class Sleep
    {
        public Sleep() { }
        public Sleep(DateTime datetime, SleepType sleepType)
        {
            this.DateTime = datetime;
            this.SleepType = sleepType;
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime DateTime { get; set; }
        public SleepType SleepType { get; set; }
    }
}
