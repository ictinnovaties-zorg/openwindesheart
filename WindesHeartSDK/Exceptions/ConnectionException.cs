// Copyright 2020 Research group ICT innovations 
// in Health Care, Windesheim University of Applied Sciences

using System;

namespace WindesHeartSDK.Exceptions
{
    public class ConnectionException : Exception
    {
        public ConnectionException() { }        

        public ConnectionException(string message) : base(message) { }
    }
}
