using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using WindesHeartSdk.Model;
using WindesHeartSDK.Devices.MiBand3.Resources;
using WindesHeartSDK.Helpers;
using static WindesHeartSDK.Helpers.ConversionHelper;

namespace WindesHeartSDK.Devices.MiBand3.Services
{
    class MiBand3FetchService
    {
        public Subject<bool> FetchingStatusSubject = new Subject<bool>();

        private readonly List<ActivitySample> _samples = new List<ActivitySample>();
        private sbyte _lastPacketCounter;
        private int _fetchCount;
        private DateTime _startTimestamp;
        private int _expectedDataLength;

        private IDisposable _charUnknownSub;
        private IDisposable _charActivitySub;

        public BLEDevice Device;

        public MiBand3FetchService(BLEDevice bledevice)
        {
            Device = bledevice;
        }

        public Subject<bool> InitiateFetching()
        {
            StartFetching();
            return FetchingStatusSubject;
        }
        private async void StartFetching()
        {
            FetchingStatusSubject.OnNext(true);

            _lastPacketCounter = -128;
            _samples.Clear();
            _fetchCount++;

            var timestamp = ConversionHelper.GetTimeBytes(await GetNextSyncTime(), TimeUnit.Minutes);
            var fetchBytes = new byte[10];
            Buffer.BlockCopy(new byte[] { 1, 1 }, 0, fetchBytes, 0, 2);
            Buffer.BlockCopy(timestamp, 0, fetchBytes, 2, 8);

            _charUnknownSub?.Dispose();
            _charUnknownSub = Device.GetCharacteristic(MiBand3Resource.GuidUnknownCharacteristic4)
                .RegisterAndNotify().Subscribe(
                    HandleCharacteristicUnknownChange,
                    Console.Write
            );


            await Device.GetCharacteristic(MiBand3Resource.GuidUnknownCharacteristic4).WriteWithoutResponse(fetchBytes);

        }

        private async void HandleCharacteristicUnknownChange(CharacteristicGattResult result)
        {
            var value = result.Characteristic.Value;
            var confirmation = new byte[3];

            if (value.Length >= 2)
            {
                Array.Copy(value, 0, confirmation, 0, 3);

            }

            if (confirmation.SequenceEqual(MiBand3Resource.ResponseActivityDataStartDateSuccess))
            {
                HandleActivityMetadata(value);

                _charActivitySub?.Dispose();
                _charActivitySub = Device.GetCharacteristic(MiBand3Resource.GuidCharacteristic5ActivityData)
                    .RegisterAndNotify().Subscribe(
                        HandleActivityNotify,
                        Console.Write
                );

                await Device.GetCharacteristic(MiBand3Resource.GuidUnknownCharacteristic4).WriteWithoutResponse(new byte[] { 0x02 });
            }
            else
            {
                HandleActivityMetadata(value);
            }
        }

        private async Task EnableNotifications(bool enable)
        {
            if (enable)
            {
                await Device.GetCharacteristic(MiBand3Resource.GuidUnknownCharacteristic4).EnableNotifications();
                await Device.GetCharacteristic(MiBand3Resource.GuidCharacteristic5ActivityData).EnableNotifications();
            }
            else
            {
                _charActivitySub?.Dispose();
                _charUnknownSub?.Dispose();
                await Device.GetCharacteristic(MiBand3Resource.GuidUnknownCharacteristic4).DisableNotifications();
                await Device.GetCharacteristic(MiBand3Resource.GuidCharacteristic5ActivityData).DisableNotifications();
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
                    var expectedLengthBytes = new byte[4];
                    Array.Copy(value, 3, expectedLengthBytes, 0, 4);
                    _expectedDataLength = BitConverter.ToInt32(expectedLengthBytes, 0);

                    // last 8 bytes are the start date
                    var timeStampBytes = new byte[8];
                    Array.Copy(value, 7, timeStampBytes, 0, 8);

                    _startTimestamp = ConversionHelper.RawBytesToCalendar(timeStampBytes, false);
                    //Calendar startTimestamp = getSupport().fromTimeBytes(Arrays.copyOfRange(value, 7, value.length));
                    //setStartTimestamp(startTimestamp);

                    //GB.updateTransferNotification(getContext().getString(R.string.busy_task_fetch_activity_data),
                    //getContext().getString(R.string.FetchActivityOperation_about_to_transfer_since,
                    //DateFormat.getDateTimeInstance().format(startTimestamp.getTime())), true, 0, getContext()); ;

                    Console.WriteLine("Expected data length: {0}", _expectedDataLength);
                }
                else
                {
                    Console.WriteLine("Unexpected activity metadata: {0}", value);
                    //LOG.warn("Unexpected activity metadata: " + Logging.formatBytes(value));
                    HandleActivityFetchFinish();
                }
            }
            //else if (value.Length == 3)
            //{
            //    if (Arrays.equals(HuamiService.RESPONSE_FINISH_SUCCESS, value))
            //    {
            //        handleActivityFetchFinish(true);
            //    }
            //    else
            //    {
            //        LOG.warn("Unexpected activity metadata: " + Logging.formatBytes(value));
            //        handleActivityFetchFinish(false);
            //    }
            //}
            else
            {
                //    LOG.warn("Unexpected activity metadata: " + Logging.formatBytes(value));
                HandleActivityFetchFinish();
            }
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

        private void HandleActivityNotify(CharacteristicGattResult result)
        {
            var value = result.Characteristic.Value;

            if (value?.Length % 4 == 1)
            {
                if (_lastPacketCounter + 128 == value[0])
                {
                    _lastPacketCounter++;
                    BufferActivityData(value);
                }
                else
                {
                    HandleActivityFetchFinish();
                }
            }
            else
            {
                HandleActivityFetchFinish();
            }
        }

        private async void HandleActivityFetchFinish()
        {

            if (true /* lastSyncTimestamp != null && NeedAnotherFetch(lastSyncTimestamp) */)
            {
                try
                {
                    StartFetching();
                    return;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            await EnableNotifications(false);

            FetchingStatusSubject.OnNext(false);
        }

        private bool NeedAnotherFetch(DateTime? lastSyncTimestamp)
        {
            if (_fetchCount > 5)
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

        private async Task<DateTime> GetNextSyncTime()
        {
            var timestamp = DateTime.Today;
            return timestamp.AddDays(-100);
        }
    }
}


