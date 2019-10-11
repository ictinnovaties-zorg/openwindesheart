using System;
using WindesHeartSdk.Helpers;

namespace WindesHeartSdk.Model
{
    public enum BatteryState
    {
        UNKNOWN,
        BATTERY_NORMAL,
        BATTERY_LOW,
        BATTERY_CHARGING,
        BATTERY_CHARGING_FULL,
        BATTERY_NOT_CHARGING_FULL,
        NO_BATTERY
    }

    public class BatteryInfo
    {
        private readonly byte[] _data;

        public BatteryInfo(byte[] data)
        {
            _data = data;
        }

        public int GetLevelInPercent()
        {
            if (_data.Length >= 2)
            {
                return _data[1];
            }
            return 50; // actually unknown
        }

        public BatteryState GetState()
        {
            if (_data.Length >= 3)
            {
                int value = _data[2];
                switch (value)
                {
                    case 0:
                        return BatteryState.BATTERY_NORMAL;
                    case 1:
                        return BatteryState.BATTERY_CHARGING;
                }
            }
            return BatteryState.UNKNOWN;
        }

        public DateTime GetLastChargeTime()
        {
            DateTime lastCharge = new DateTime();

            if (_data.Length >= 18)
            {
                lastCharge = BleTypeConversions.RawBytesToCalendar(new byte[]{
                    _data[10], _data[11], _data[12], _data[13], _data[14], _data[15], _data[16], _data[17]
                }, true);
            }

            return lastCharge;
        }
    }
}
