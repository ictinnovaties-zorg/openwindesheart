/* Copyright 2020 Research group ICT innovations

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License. */

using System;
using System.Security.Cryptography;
using OpenWindesheart.Helpers;

namespace OpenWindesheart.Devices.MiBand3Device.Helpers
{
    public static class MiBand3ConversionHelper
    {
        /// <summary>
        /// This generates a new secret key for authenticating a new device
        /// </summary>
        /// <returns></returns>
        public static byte[] GenerateAuthKey()
        {
            Random random = new Random();
            byte[] SecretKey = new byte[16];
            for (var i = 0; i < 16; i++)
            {
                int keyNumber = random.Next(1, 255);
                SecretKey[i] = Convert.ToByte(keyNumber);
            }
            return SecretKey;
        }

        public static byte[] CreateKey(byte[] value, byte[] key)
        {
            byte[] bytes = { 0x03, 0x00 };
            byte[] secretKey = key;

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
