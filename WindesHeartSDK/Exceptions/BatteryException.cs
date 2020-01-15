// Copyright 2020 Research group ICT innovations 
// in Health Care, Windesheim University of Applied Sciences

using System;

namespace WindesHeartSDK.Exceptions
{
    public class BatteryException : Exception
    {
        public BatteryException() { }

        public BatteryException(string message) : base(message) { }
    }
}
