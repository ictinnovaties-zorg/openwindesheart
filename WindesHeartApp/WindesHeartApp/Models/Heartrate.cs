using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace WindesHeartApp.Models
{
    [Table("Heartrates")]
    public class Heartrate
    {
        public Heartrate() { }

        public Heartrate(DateTime datetime, int heartRateValue)
        {
            this.DateTime = datetime;
            this.HeartrateValue = heartRateValue;
        }

        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }

        [Column("DateTime")]
        public DateTime DateTime { get; set; }

        [Column("HeartrateValue")]
        public int HeartrateValue { get; set; }
    }
}
