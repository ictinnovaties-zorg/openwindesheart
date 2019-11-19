using System;

namespace WindesHeartSDK.Exceptions
{
    public class ConnectionException : Exception
    {
        public ConnectionException()
        {
        }

        public ConnectionException(string message) : base(message)
        {
        }

    }
}
