using System;
namespace WindesHeartSDK.MiBand
{
    public class MiBandResource
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


    }
}
