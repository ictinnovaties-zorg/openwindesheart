using System;
namespace WindesHeartSDK.Models
{
    public class Battery
    {
        private readonly byte[] _data;

        public Battery(byte[] data)
        {
            _data = data;
        }

        public int GetLevelInPercent()
        {
            if (_data.Length >= 2)
            {
                return _data[1];
            }
            return 0; //Unknown
        }
    }
}
