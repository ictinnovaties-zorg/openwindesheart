using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using WindesHeartSDK.Models;
using WindesHeartSDK.Devices.MiBand3Device.Resources;
using WindesHeartSDK.Helpers;
using static WindesHeartSDK.Helpers.ConversionHelper;
using WindesHeartSDK.Devices.MiBand3Device.Models;

namespace WindesHeartSDK.Devices.MiBand3Device.Services
{
    public class MiBand3SampleService
    {
        private readonly MiBand3 _miBand3;
        private readonly List<ActivitySample> _samples = new List<ActivitySample>();

        private DateTime _firstTimestamp;
        private DateTime _lastTimestamp;
        private int _samplenumber = 0;

        private IDisposable _charUnknownSub;
        private IDisposable _charActivitySub;

        private Action<List<ActivitySample>> _finishedCallback;
        private Action<int> _remainingSamplesCallback;

        private int _expectedSamples;
        private int _totalSamples;

        public MiBand3SampleService(MiBand3 device)
        {
            _miBand3 = device;
        }

        /// <summary>
        /// Clear the list of samples and start fetching
        /// </summary>
        public async void StartFetching(DateTime date, Action<List<ActivitySample>> finishedCallback, Action<int> remainingSamplesCallback)
        {
            _samples.Clear();
            _expectedSamples = 0;
            _totalSamples = 0;

            _finishedCallback = finishedCallback;
            _remainingSamplesCallback = remainingSamplesCallback;
            await InitiateFetching(date);
        }

        private void CalculateExpectedSamples(DateTime startDate)
        {
            TimeSpan timespan = DateTime.Now - startDate;
            _totalSamples = (int)timespan.TotalMinutes;
        }


        /// <summary>
        /// Setup the disposables for the fetch operation
        /// </summary>
        /// <param name="date"></param>
        private async Task InitiateFetching(DateTime date)
        {
            _samplenumber = 0;
            //Dispose all DIsposables to prevent double data
            _charActivitySub?.Dispose();
            _charUnknownSub?.Dispose();

            // Subscribe to the unknown and activity characteristics
            _charUnknownSub = _miBand3.GetCharacteristic(MiBand3Resource.GuidSamplesRequest).RegisterAndNotify().Subscribe(HandleUnknownChar);
            _charActivitySub = _miBand3.GetCharacteristic(MiBand3Resource.GuidActivityData).RegisterAndNotify().Subscribe(HandleActivityChar);

            // Write the date and time from which to receive samples to the Mi Band
            await WriteDateBytes(date);
        }

        /// <summary>
        /// Write the date from wich to recieve data to the mi band
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private async Task WriteDateBytes(DateTime date)
        {
            // Convert date to bytes
            byte[] Timebytes = GetTimeBytes(date, TimeUnit.Minutes);
            byte[] Fetchbytes = new byte[10] { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 };

            // Copy the date in the byte template to send to the device
            Buffer.BlockCopy(Timebytes, 0, Fetchbytes, 2, 8);

            // Send the bytes to the device
            await _miBand3.GetCharacteristic(MiBand3Resource.GuidSamplesRequest).WriteWithoutResponse(Fetchbytes);
        }

        /// <summary>
        /// Called when recieving MetaData
        /// </summary>
        /// <param name="result"></param>
        private async void HandleUnknownChar(CharacteristicGattResult result)
        {
            //Correct response
            if (result.Data.Length >= 3)
            {
                // Create an empty byte array and copy the response type to it
                byte[] responseByte = new byte[3];
                Buffer.BlockCopy(result.Data, 0, responseByte, 0, 3);

                // Start or Continue fetching process
                if (responseByte.SequenceEqual(new byte[3] { 0x10, 0x01, 0x01 }))
                {
                    await HandleResponse(result.Data);
                    return;
                }

                // Finished fetching
                if (responseByte.SequenceEqual(new byte[3] { 0x10, 0x02, 0x01 }))
                {
                    if (_lastTimestamp >= DateTime.Now.AddMinutes(-1))
                    {
                        _finishedCallback(_samples);
                        _charActivitySub?.Dispose();
                        _charUnknownSub?.Dispose();
                        return;
                    }

                    await InitiateFetching(_lastTimestamp.AddMinutes(1));
                    return;
                }
            }

            Trace.WriteLine("No more samples could be fetched, samples returned: "+_samples.Count);
            _finishedCallback(_samples);
            _charActivitySub?.Dispose();
            _charUnknownSub?.Dispose();
            return;
        }

        /// <summary>
        /// Called when recieving samples
        /// </summary>
        /// <param name="result"></param>
        private void HandleActivityChar(CharacteristicGattResult result)
        {
            // Each sample is made up from 4 bytes. The first byte represents the status.
            if (result.Data.Length % 4 != 1)
            {
                InitiateFetching(_lastTimestamp.AddMinutes(1));
            }
            else
            {
                CreateSamplesFromResponse(result.Data);
            }
        }

        private async Task HandleResponse(byte[] data)
        {
            if(data.Length > 6)
            {
                //Start fetching
                try
                {
                    // Get the timestamp of the first sample
                    byte[] DateTimeBytes = new byte[8];
                    Buffer.BlockCopy(data, 7, DateTimeBytes, 0, 8);
                    _firstTimestamp = RawBytesToCalendar(DateTimeBytes);

                    // Write 0x02 to tell the band to start the fetching process
                    await _miBand3.GetCharacteristic(MiBand3Resource.GuidSamplesRequest).WriteWithoutResponse(new byte[] { 0x02 });
                }
                catch(Exception e)
                {
                    Trace.WriteLine("Could not start fetching: " + e);
                }
            }
            else
            {
                //Continue fetching if more expected
                try
                {
                    _expectedSamples = data[5] << 16 | data[4] << 8 | data[3];
                    if(_expectedSamples == 0)
                    {
                        _finishedCallback(_samples);
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine("Could not calculate expected samples: " + e);
                }
            }
        }
        
        private void CreateSamplesFromResponse(byte[] data)
        {
            var samplecount = _samplenumber;
            _samplenumber++;
            var i = 1;
            while (i < data.Length)
            {
                int timeIndex = (samplecount) * 4 + (i - 1) / 4;
                var timeStamp = _firstTimestamp.AddMinutes(timeIndex);
                _lastTimestamp = timeStamp;

                // Create a sample from the received bytes
                byte[] rawdata = new byte[] { data[i], data[i + 1], data[i + 2], data[i + 3] };
                var category = data[i] & 0xff;
                var intensity = data[i + 1] & 0xff;
                var steps = data[i + 2] & 0xff;
                var heartrate = data[i + 3];

                // Add the sample to the sample list
                _samples.Add(new ActivitySample(timeStamp, category, intensity, steps, heartrate, rawdata));

                //Callback for progress
                if (_samples.Count % 250 == 0)
                {
                    CalculateExpectedSamples(timeStamp);
                    _remainingSamplesCallback(_totalSamples);
                }

                i += 4;

               
                var d = DateTime.Now.AddMinutes(-1);
                d.AddSeconds(-d.Second);
                d.AddMilliseconds(-d.Millisecond);

                // Make sure we aren't getting samples from the future
                if (timeStamp == d)
                {
                    break;
                }
            }
        }
    }
}
