using System;

namespace WindesHeartSDK.Exceptions
{
    class ReadException : Exception
    {
        public ReadException()
        {

        }

        public ReadException(string message) : base(message)
        {

        }
    }
}
