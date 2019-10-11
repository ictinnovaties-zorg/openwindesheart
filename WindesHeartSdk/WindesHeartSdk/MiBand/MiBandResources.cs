using System;

namespace WindesHeartSdk.MiBand
{
    public class MiBandResources
    {
        public static Guid GuidServiceMiBandService = new Guid("0000FEE0-0000-1000-8000-00805f9b34fb");
        public static Guid GuidServiceMiBand2Service = new Guid("0000FEE1-0000-1000-8000-00805f9b34fb");

        public static Guid GuidServiceHeartRate = new Guid("0000180d-0000-1000-8000-00805f9b34fb");

        public static Guid GuidCharacteristic3Configuration = new Guid("00000003-0000-3512-2118-0009af100700");
        public static Guid GuidUnknownCharacteristic4 = new Guid("00000004-0000-3512-2118-0009af100700");
        public static Guid GuidCharacteristic5ActivityData = new Guid("00000005-0000-3512-2118-0009af100700");
        public static Guid GuidCharacteristic6BatteryInfo = new Guid("00000006-0000-3512-2118-0009af100700");
        public static Guid GuidCharacteristic7RealtimeSteps = new Guid("00000007-0000-3512-2118-0009af100700");
        public static Guid GuidCharacteristic8UserSettings = new Guid("00000008-0000-3512-2118-0009af100700");
        public static Guid GuidCharacteristicAuth = new Guid("00000009-0000-3512-2118-0009af100700");
        public static Guid GuidCharacteristicDeviceEvent = new Guid("00000010-0000-3512-2118-0009af100700");

        public static Guid GuidCharacteristicTest = new Guid("0000FEC1-0000-3512-2118-0009AF100700");
        public static Guid GuidCharacteristicHeartRateControlPoint = new Guid("00002A39-0000-1000-8000-00805f9b34fb");

        public static Guid GuidCharacteristicCurrentTime = new Guid("00002A2B-0000-1000-8000-00805f9b34fb");

        public static byte AuthResponse = 0x10;
        public static byte AuthSendKey = 0x01;
        public static byte AuthSuccess = 0x01;

        public static byte AuthRequestRandomAuthNumber = 0x02;
        public static byte AuthSendEncryptedAuthNumber = 0x03;

        public static byte Response = 0x10;
        public static byte CommandActivityDataStartDate = 0x01;
        public static byte Success = 0x01;

        public static byte[] ResponseActivityDataStartDateSuccess = { Response, CommandActivityDataStartDate, Success };

        public static byte[] CommandEnableDisplayOnLiftWrist = {0x06, 0x05, 0x00, 0x01 };
        public static byte[] CommandDisableDisplayOnLiftWrist = { 0x06, 0x05, 0x00, 0x00 };

        public static byte CommandSetPeriodicHrMeasurementInterval = 0x14;
    }
}
