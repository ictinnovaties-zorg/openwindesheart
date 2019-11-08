using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using WindesHeartSdk.Model;
using WindesHeartSDK.Devices.MiBand3.Resources;
using static WindesHeartSDK.Helpers.ConversionHelper;

namespace WindesHeartSDK.Devices.MiBand3.Services
{
    class MiBand3FetchService
    {
        private readonly List<ActivitySample> Samples = new List<ActivitySample>();
        private sbyte LastPacketCounter;
        private DateTime StartTimestamp;
        private int ExpectedDataLength;

        private IDisposable CharUnknownSub;
        private IDisposable CharActivitySub;
        private int FetchCount;
        public BLEDevice Device;

        public MiBand3FetchService(BLEDevice bledevice)
        {
            Device = bledevice;
        }

        public void InitiateFetching()
        {
            StartFetching();
        }
        private async void StartFetching()
        {
            FetchCount++;
            LastPacketCounter = -128;
            Samples.Clear();

            //Start date to get activitydata from
            byte[] timestamp = GetTimeBytes(DateTime.Today.AddDays(-2), TimeUnit.Minutes);
            byte[] fetchBytes = new byte[10] { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 };

            //Copy timestamp to last 8 bytes of fetchBytes
            Buffer.BlockCopy(timestamp, 0, fetchBytes, 2, 8);

            PrintBytes(fetchBytes, "fetchbytes");

            //When unknown characteristic changed, call OnUnknownCharactericticChange()
            CharUnknownSub?.Dispose();
            CharUnknownSub = Device.GetCharacteristic(MiBand3Resource.GuidUnknownCharacteristic4).RegisterAndNotify().Subscribe(OnUnknownCharacteristicChange);

            //Write fetchbytes to the unknown characteristic
            await Device.GetCharacteristic(MiBand3Resource.GuidUnknownCharacteristic4).WriteWithoutResponse(fetchBytes);
        }

        private void PrintBytes(byte[] bytes, string name)
        {
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("VALUES OF BYTE ARRAY " + name + ":");
            foreach (byte b in bytes)
            {
                Console.Write(b + ",");
            }
            Console.WriteLine("");
            Console.WriteLine("-------------------------------------------------");
        }

        private async void OnUnknownCharacteristicChange(CharacteristicGattResult result)
        {
            byte[] response = result.Data;

            PrintBytes(response, "Unknown Characteristic");

            //Confirmation is first 3 bytes of response
            byte[] confirmation = response.Take(3).ToArray();

            // first two bytes are whether our request was accepted
            if (confirmation.SequenceEqual(MiBand3Resource.ResponseActivityDataStartDateSuccess))
            {
                HandleActivityMetadata(response);

                CharActivitySub?.Dispose();
                //When activity characteristic changed, call OnActivityCharacteristicChanged()
                CharActivitySub = Device.GetCharacteristic(MiBand3Resource.GuidCharacteristic5ActivityData).RegisterAndNotify().Subscribe(OnActivityCharacteristicChanged);

                //write 2 to the UnknownCharacteristic (why?)
                await Device.GetCharacteristic(MiBand3Resource.GuidUnknownCharacteristic4).WriteWithoutResponse(new byte[] { 0x02 });
            }
            else
            {
                HandleActivityMetadata(response);
            }
        }

        ///<summary>Creates samples from the given 17-length array</summary>
        ///<param name="result"></param>
        protected void BufferActivityData(byte[] value)
        {
            int len = value.Length;

            if (len % 4 != 1)
            {
                throw new ArgumentException("Unexpected activity array size: " + len);
            }

            for (int i = 1; i < len; i += 4)
            {
                //create the sample
                ActivitySample sample = CreateSample(value[i], value[i + 1], value[i + 2], value[i + 3]);
                Samples.Add(sample);
                Console.WriteLine("Samples:  " + Samples.Count);
            }
        }

        private ActivitySample CreateSample(byte category, byte intensity, byte steps, byte heartrate)
        {
            ActivitySample sample = new ActivitySample
            {
                RawKind = category & 0xff,
                RawIntensity = intensity & 0xff,
                Steps = steps & 0xff,
                HeartRate = heartrate & 0xff
            };

            return sample;
        }


        ///<summary>
        ///Method to handle the incoming activity data. There are two kind of messages we currently know:
        ///the first one is 11 bytes long and contains metadata (how many bytes to expect, when the data starts, etc.)
        ///the second one is 20 bytes long and contains the actual activity data. The first message type is parsed by this method, for every other length of the value param, bufferActivityData is called
        ///</summary>
        ///<param name="result"></param>
        private void OnActivityCharacteristicChanged(CharacteristicGattResult result)
        {
            byte[] response = result.Data;

            PrintBytes(response, "Activity Characteristic");

            if (response?.Length % 4 == 1)
            {
                if (LastPacketCounter + 128 == response[0])
                {
                    LastPacketCounter++;
                    BufferActivityData(response);
                }
                else
                {
                    OnActivityFetchFinish();
                }
            }
            else
            {
                OnActivityFetchFinish();
            }
        }

        private void HandleActivityMetadata(byte[] value)
        {
            if (value.Length == 15)
            {
                var confirmation = new byte[3];
                Array.Copy(value, 0, confirmation, 0, 3);
                // first two bytes are whether our request was accepted
                if (confirmation.SequenceEqual(MiBand3Resource.ResponseActivityDataStartDateSuccess))
                {
                    // the third byte (0x01 on success) = ?
                    // the 4th - 7th bytes represent the number of bytes/packets to expect, excluding the counter bytes

                    // last 8 bytes are the start date
                    var timeStampBytes = new byte[8];
                    Array.Copy(value, 7, timeStampBytes, 0, 8);

                    StartTimestamp = RawBytesToCalendar(timeStampBytes, false);
                    Console.WriteLine("Expected data length: {0}", ExpectedDataLength);
                }
                else
                {
                    Console.WriteLine("Unexpected activity metadata: {0}", value);
                    OnActivityFetchFinish();
                }
            }
            else
            {
                OnActivityFetchFinish();
            }
        }

        private void OnActivityFetchFinish()
        {
            CharActivitySub?.Dispose();
            CharUnknownSub?.Dispose();
        }

        private bool NeedAnotherFetch(DateTime? lastSyncTimestamp)
        {
            if (FetchCount > 5)
            {
                return false;
            }
            if (lastSyncTimestamp != null && lastSyncTimestamp.Value.Date == DateTime.Today)
            {
                return false;
            }

            if (lastSyncTimestamp != null && lastSyncTimestamp.Value > DateTime.Now)
            {
                return false;

            }
            return true;
        }
    }
}


