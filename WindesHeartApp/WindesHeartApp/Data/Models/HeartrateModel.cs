using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace WindesHeartApp.Data.Models
{
    public class HeartrateModel
    {
        [PrimaryKey]
        public int Id { get; set; }
        public byte[] Rawdata { get; set; }
        public DateTime DateTime { get; set; }

        public int HeartrateValue { get; set; }

        public HeartrateModel()
        {

        }

        public HeartrateModel(DateTime datetime, byte[] rawdata)
        {
            DateTime = datetime;
            Rawdata = rawdata;
            HeartrateValue = rawdata[1];
        }
    }
}
