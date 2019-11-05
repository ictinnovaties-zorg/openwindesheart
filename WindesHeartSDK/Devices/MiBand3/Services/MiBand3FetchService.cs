using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using WindesHeartSdk.Model;
using WindesHeartSDK.Devices.MiBand3.Resources;
using WindesHeartSDK.Helpers;
using static WindesHeartSDK.Helpers.ConversionHelper;

namespace WindesHeartSDK.Devices.MiBand3.Services
{
    class MiBand3FetchService
    {
        private readonly BLEDevice BLEDevice;
        private List<ActivitySample> _samples = new List<ActivitySample>();

        public MiBand3FetchService(BLEDevice device)
        {
            BLEDevice = device;
        }
        public async void HandleFetch(CharacteristicGattResult characteristic)
        {
            var value = characteristic.Characteristic.Value;

            var Samples = BLEDevice.GetCharacteristic(MiBand3Resource.GuidActivity).RegisterAndNotify().Subscribe(
                HandleSamples
            );
            var confirmation = new byte[3];

            if (value.Length >= 2)
            {
                Array.Copy(value, 0, confirmation, 0, 3);

            }

            if (confirmation.SequenceEqual(MiBand3Resource.ResponseActivityDataStartDateSuccess))
            {
                await BLEDevice.GetCharacteristic(MiBand3Resource.GuidUnknown).WriteWithoutResponse(new byte[] { 0x02 });
                Console.WriteLine(value);
            }
            else
            {
                //HandleActivityMetadata(value);
            }
        }


        public async void Fetch()
        {
            await BLEDevice.GetCharacteristic(MiBand3Resource.GuidUnknown).EnableNotifications();
            await BLEDevice.GetCharacteristic(MiBand3Resource.GuidActivity).EnableNotifications();

            var Samples = BLEDevice.GetCharacteristic(MiBand3Resource.GuidUnknown).RegisterAndNotify().Subscribe(
                HandleFetch
            );

            DateTime time = DateTime.Today;
            time = time.AddDays(-100);

            var timestamp = ConversionHelper.GetTimeBytes(time, TimeUnit.Minutes);
            var fetchBytes = new byte[10];
            Buffer.BlockCopy(new byte[] { 1, 1 }, 0, fetchBytes, 0, 2);
            Buffer.BlockCopy(timestamp, 0, fetchBytes, 2, 8);

            await BLEDevice.GetCharacteristic(MiBand3Resource.GuidUnknown).WriteWithoutResponse(fetchBytes);
        }

        private void HandleSamples(CharacteristicGattResult result)
        {
            var res = result;
            BufferActivityData(result.Characteristic.Value);
            Console.WriteLine(res);
        }

        protected void BufferActivityData(byte[] value)
        {
            int len = value.Length;

            if (len % 4 != 1)
            {
                throw new ArgumentException("Unexpected activity array size: " + len);
            }

            for (int i = 1; i < len; i += 4)
            {
                ActivitySample sample = CreateSample(value[i], value[i + 1], value[i + 2], value[i + 3]); // lgtm [java/index-out-of-bounds]
                _samples.Add(sample);
                Console.WriteLine("Samples:  " + _samples.Count);
            }
        }

        private static ActivitySample CreateSample(byte category, byte intensity, byte steps, byte heartrate)
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
    }
}

