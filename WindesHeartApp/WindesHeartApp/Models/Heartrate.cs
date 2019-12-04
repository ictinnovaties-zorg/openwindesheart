using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace WindesHeartApp.Models
{
    public class Heartrate
    {
        public Heartrate() { }
        public Heartrate(DateTime datetime, int heartRateValue)
        {
            this.DateTime = datetime;
            this.HeartrateValue = heartRateValue;
        }


        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        public int HeartrateValue { get; set; }
    }
}
