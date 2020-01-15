// Copyright 2020 Research group ICT innovations 
// in Health Care, Windesheim University of Applied Sciences

using System;

namespace WindesHeartSDK.Exceptions
{
    class ReadException : Exception
    {
        public ReadException() { }

        public ReadException(string message) : base(message) { }
    }
}
