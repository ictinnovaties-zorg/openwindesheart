using System;
using System.Collections.Generic;
using System.Text;

namespace WindesHeartSdk.Model
{
    class DataPointRequest
    {
        public User User { get; set; }
        public List<MiBandActivitySample> Measurements { get; set; }
    }
}
