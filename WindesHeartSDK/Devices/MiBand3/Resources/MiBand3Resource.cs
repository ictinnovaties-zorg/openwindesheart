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

        //General Guid for device settings
        public static Guid GuidDeviceConfiguration = new Guid("00000003-0000-3512-2118-0009af100700");

        public static byte[] Byte_EnableActivateOnLiftWrist = new byte[] { 0x06, 0x05, 0x00, 0x01 };
        public static byte[] Byte_DisableActivateOnLiftWrist = new byte[] { 0x06, 0x05, 0x00, 0x00 };
        public static byte[] Byte_ScheduleActivateOnLiftWrist_Template = new byte[] { 0x06, 0x05, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00 };

        public static byte[] Byte_SetLanguage_Template = new byte[] { 0x06, 0x17, 0x00, 0, 0, 0, 0, 0 };

        public static byte[] Byte_TimeFomat_24hours = new byte[] { 0x06, 0x02, 0x0, 0x1 };
        public static byte[] Byte_TimeFomat_12hours = new byte[] { 0x06, 0x02, 0x0, 0x0 };

        public static byte[] Byte_EnableGoalNotification = new byte[] { 0x06, 0x06, 0x00, 0x01};
        public static byte[] Byte_DisableGoalNotification = new byte[] { 0x06, 0x06, 0x00, 0x00};

        public static byte[] Byte_EnableSleepMeasurement = new byte[] { 0x15, 0x00, 0x01 };
        public static byte[] Byte_DisableSleepMeasurement = new byte[] { 0x15, 0x00, 0x00 };

        public static byte[] Byte_DateFormat_dd_MM_YYYY = new byte[] { 0x06, 30, 0x00, Convert.ToByte('d'), Convert.ToByte('d'), Convert.ToByte('/'), Convert.ToByte('M'), Convert.ToByte('M'), Convert.ToByte('/'), Convert.ToByte('y'), Convert.ToByte('y'), Convert.ToByte('y'), Convert.ToByte('y') };
        public static byte[] Byte_DateFormat_MM_dd_YYYY = new byte[] { 0x06, 30, 0x00, Convert.ToByte('M'), Convert.ToByte('M'), Convert.ToByte('/'), Convert.ToByte('d'), Convert.ToByte('d'), Convert.ToByte('/'), Convert.ToByte('y'), Convert.ToByte('y'), Convert.ToByte('y'), Convert.ToByte('y') };

        //User settings
        public static Guid GuidUserInfo = new Guid("00000008-0000-3512-2118-0009af100700");

        //Battery Guid
        public static Guid GuidBatteryInfo = new Guid("00000006-0000-3512-2118-0009af100700");

        //Current Time Guid
        public static Guid GuidCurrentTime = new Guid("00002A2B-0000-1000-8000-00805f9b34fb");

        //Heartrate Control Point Guid
        public static Guid GuidHeartRateControl = new Guid("00002A39-0000-1000-8000-00805f9b34fb");

        //Heartrate Realtime Guid
        public static Guid GuidHeartrate = new Guid("00002A37-0000-1000-8000-00805f9b34fb");
        
        //Samples Bytes and Guids
        public static Guid GuidSamplesRequest = new Guid("00000004-0000-3512-2118-0009af100700");

        public static Guid GuidActivityData = new Guid("00000005-0000-3512-2118-0009af100700");

        public static byte Response = 0x10;
        public static byte CommandActivityDataStartDate = 0x01;
        public static byte Success = 0x01;

        public static byte[] ResponseActivityDataStartDateSuccess = { Response, CommandActivityDataStartDate, Success };

        //Steps Realtime Guid
        public static Guid GuidStepsInfo = new Guid("00000007-0000-3512-2118-0009af100700");

    }

}
