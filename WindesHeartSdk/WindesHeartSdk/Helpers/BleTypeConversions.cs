using System;
using System.Security.Cryptography;

namespace WindesHeartSdk.Helpers
{
    public enum TimeUnit { Seconds, Days, Hours, Minutes, Unknown = -1 }

    class BleTypeConversions
    {
        public static byte[] ShortDateTimeToRawBytes(DateTime dateTime, bool honorDeviceTimeOffset)
        {
            byte[] year = FromUint16(dateTime.Year);
            return new[] {
                year[0],
                year[1],
                FromUint8(dateTime.Month),
                FromUint8(dateTime.Day),
                FromUint8(dateTime.Hour),
                FromUint8(dateTime.Minute)
            };
        }

        public static byte[] DateTimeToRawBytes(DateTime dateTime, bool honorDeviceTimeOffset)
        {
            byte[] year = FromUint16(dateTime.Year);
            return new[] {
                year[0],
                year[1],
                FromUint8(dateTime.Month),
                FromUint8(dateTime.Day),
                FromUint8(dateTime.Hour),
                FromUint8(dateTime.Minute),
                FromUint8(dateTime.Second),
                DayOfWeekToRawBytes(dateTime),
                (byte) 0
            };
        }

        public static byte[] GetTimeBytes(DateTime dateTime, TimeUnit precision)
        {
            byte[] bytes;
            if (precision == TimeUnit.Minutes)
            {
                bytes = ShortDateTimeToRawBytes(dateTime, false);
            } 
            else if (precision == TimeUnit.Seconds)
            {
                bytes = DateTimeToRawBytes(dateTime, false);
            }
            else
            {
                throw new ArgumentException();
            }

            byte[] all = new byte[bytes.Length + 2];
            Buffer.BlockCopy(bytes, 0, all, 0, bytes.Length);
            Buffer.BlockCopy(new byte[] { 0, 4 }, 0, all, bytes.Length, 2);

            return all;
        }

        private static byte DayOfWeekToRawBytes(DateTime dateTime)
        {
            int dayValue = (int) dateTime.DayOfWeek;
            if (dateTime.DayOfWeek == DayOfWeek.Saturday)
            {
                return 7;
            }
            return (byte) dayValue;
        }

        public static byte FromUint8(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return bytes[0];
        }

        public static byte[] FromUint16(int value)
        {
            return BitConverter.GetBytes((short) value);
        }

        public static int ToUint16(byte[] bytes)
        {
            return (bytes[1] << 8 | bytes[0]);
        }

        public static DateTime RawBytesToCalendar(byte[] value, bool honorDeviceTimeOffset)
        {
            if (value.Length >= 7)
            {
                int year = ToUint16(new byte[] {value[0], value[1], 0, 0});
                DateTime timestamp = new DateTime(
                        year,
                        (value[2] & 0xff),
                        value[3] & 0xff,
                        value[4] & 0xff,
                        value[5] & 0xff,
                        value[6] & 0xff
                );
                /*if (value.Length > 7)
                {
                    TimeZoneInfo timeZone = TimeZoneInfo.Local;
                    timeZone.setRawOffset(value[7] * 15 * 60 * 1000);
                    timestamp.;
                }*/

                /*if (honorDeviceTimeOffset)
                {
                    int offsetInHours = MiBandCoordinator.getDeviceTimeOffsetHours();
                    if (offsetInHours != 0)
                    {
                        timestamp.add(Calendar.HOUR_OF_DAY, -offsetInHours);
                    }
                }*/

                return timestamp;
            }

            return new DateTime();
        }

        public static byte[] CopyOfRange(byte[] src, int start, int end)
        {
            int len = end - start;
            byte[] dest = new byte[len];
            // note i is always from 0
            for (int i = 0; i < len; i++)
            {
                dest[i] = src[start + i]; // so 0..n = 0+x..n+x
            }
            return dest;
        }

        public static byte[] CreateKey(byte[] value)
        {
            byte[] bytes = { 0x03, 0x08 };
            byte[] secretKey = { 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x40, 0x41, 0x42, 0x43, 0x44, 0x45 };

            value = CopyOfRange(value, 3, 19);
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
