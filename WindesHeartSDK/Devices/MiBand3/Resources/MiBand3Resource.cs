using System;
namespace WindesHeartSDK.Devices.MiBand3Device.Resources
{
    public static class MiBand3Resource
    {
        //Authentication
        public static Guid GuidCharacteristicAuth = new Guid("00000009-0000-3512-2118-0009af100700");
        public static readonly byte[] AuthKey = { 0x01, 0x00, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x40, 0x41, 0x42, 0x43, 0x44, 0x45 };

        public static byte AuthResponse = 0x10;

        public static byte AuthSendKey = 0x01;
        public static readonly byte[] RequestNumber = { 0x02, 0x00 };
        public static byte AuthRequestRandomAuthNumber = 0x02;

        public static byte AuthSendEncryptedAuthNumber = 0x03;

        public static byte AuthSuccess = 0x01;

        //Battery Guid
        public static Guid GuidCharacteristic6BatteryInfo = new Guid("00000006-0000-3512-2118-0009af100700");

        //Current Time Guid
        public static Guid GuidCharacteristicCurrentTime = new Guid("00002A2B-0000-1000-8000-00805f9b34fb");

        //Heartrate Control Point Guid
        public static Guid GuidCharacteristicHeartrateControl = new Guid("00002A39-0000-1000-8000-00805f9b34fb");

        //Heartrate Realtime Guid
        public static Guid GuidCharacteristicHeartrate = new Guid("00002A37-0000-1000-8000-00805f9b34fb");
        
        //Samples Bytes and Guids
        public static Guid GuidUnknownCharacteristic4 = new Guid("00000004-0000-3512-2118-0009af100700");

        public static Guid GuidCharacteristic5ActivityData = new Guid("00000005-0000-3512-2118-0009af100700");

        public static byte Response = 0x10;
        public static byte CommandActivityDataStartDate = 0x01;
        public static byte Success = 0x01;

        public static byte[] ResponseActivityDataStartDateSuccess = { Response, CommandActivityDataStartDate, Success };

        //Steps Realtime Guid
        public static Guid GuidCharacteristic7RealtimeSteps = new Guid("00000007-0000-3512-2118-0009af100700");
    }

}
