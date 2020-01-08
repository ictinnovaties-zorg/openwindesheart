using System;
using System.Security.Cryptography;
using WindesHeartSDK.Helpers;

namespace WindesHeartSDK.Devices.MiBand3Device.Helpers
{
    public static class MiBand3ConversionHelper
    {

        public static byte[] CreateKey(byte[] value)
        {
            byte[] bytes = { 0x03, 0x00 };
            byte[] secretKey = { 0xa3, 0x04, 0x6e, 0x3c, 0x39, 0x8c, 0x47, 0xe9, 0x8c, 0x89, 0xda, 0xfe, 0x35, 0xb7, 0x8c, 0xb1 };

            value = ConversionHelper.CopyOfRange(value, 3, 19);
            byte[] buffer = EncryptBuff(secretKey, value);
            byte[] endBytes = new byte[18];
            Buffer.BlockCopy(bytes, 0, endBytes, 0, 2);
            Buffer.BlockCopy(buffer, 0, endBytes, 2, 16);
            return endBytes;
        }

        public static byte[] EncryptBuff(byte[] sessionKey, byte[] buffer)
        {
            AesManaged myAes = new AesManaged();

            myAes.Mode = CipherMode.ECB;
            myAes.Key = sessionKey;
            myAes.Padding = PaddingMode.None;

            ICryptoTransform encryptor = myAes.CreateEncryptor();
            return encryptor.TransformFinalBlock(buffer, 0, buffer.Length);
        }


    }
}
