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

namespace WindesHeartSDK.Devices.MiBand3Device.Services
{
    class MiBand3FetchService
    {
        private readonly MiBand3.Models.MiBand3 _miBand3;
        private readonly List<ActivitySample> _samples = new List<ActivitySample>();

        private DateTime _firstTimestamp;
        private DateTime _lastTimestamp;
        private int _samplenumber = 0;

        private IDisposable _charUnknownSub;
        private IDisposable _charActivitySub;

        private Action<List<ActivitySample>> _callback;
        private Action<float> _progressCallback;

        private int _expectedSamples;


        public MiBand3FetchService(MiBand3.Models.MiBand3 device)
        {
            _miBand3 = device;
        }

        /// <summary>
        /// Clear the list of samples and start fetching
        /// </summary>
        public async void StartFetching(DateTime date, Action<List<ActivitySample>> callback, Action<float> progressCallback)
        {
            _samples.Clear();
            _expectedSamples = 0;
            await InitiateFetching(date);
            _callback = callback;
            _progressCallback = progressCallback;
        }

        /// <summary>
        /// Setup the disposables for the fetch operation
        /// </summary>
        /// <param name="date"></param>
        public async Task InitiateFetching(DateTime date)
        {
            _samplenumber = 0;
            //Dispose all DIsposables to prevent double data
            _charActivitySub?.Dispose();
            _charUnknownSub?.Dispose();

            // Subscribe to the unknown and activity characteristics
            _charUnknownSub = _miBand3.GetCharacteristic(MiBand3Resource.GuidUnknownCharacteristic4).RegisterAndNotify().Subscribe(HandleUnknownChar);
            _charActivitySub = _miBand3.GetCharacteristic(MiBand3Resource.GuidCharacteristic5ActivityData).RegisterAndNotify().Subscribe(HandleActivityChar);


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
            Trace.WriteLine(ConversionHelper.RawBytesToCalendar(Timebytes));
            byte[] Fetchbytes = new byte[10] { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 };

            // Copy the date in the byte template to send to the device
            Buffer.BlockCopy(Timebytes, 0, Fetchbytes, 2, 8);

            // Send the bytes to the device
            await _miBand3.GetCharacteristic(MiBand3Resource.GuidUnknownCharacteristic4).WriteWithoutResponse(Fetchbytes);
        }

        /// <summary>
        /// Called when recieving MetaData
        /// </summary>
        /// <param name="result"></param>
        public async void HandleUnknownChar(CharacteristicGattResult result)
        {
            if (result.Data.Length >= 3)
            {
                // Create an empty byte array and copy the response type to it
                byte[] responseByte = new byte[3];
                Buffer.BlockCopy(result.Data, 0, responseByte, 0, 3);

                // Check if our request was accepted
                if (responseByte.SequenceEqual(new byte[3] { 0x10, 0x01, 0x01 }))
                {
                    if (result.Data.Length > 3)
                    {
                        _expectedSamples = result.Data[5] << 16 | result.Data[4] << 8 | result.Data[3];
                        if (_expectedSamples == 0)
                        {
                            _callback(_samples);
                            return;
                        }
                        Trace.WriteLine("Expected Samples: " + _expectedSamples);

                        if (result.Data.Length > 6) {
                            // Get the timestamp of the first sample
                            byte[] DateTimeBytes = new byte[8];
                            Buffer.BlockCopy(result.Data, 7, DateTimeBytes, 0, 8);
                            _firstTimestamp = RawBytesToCalendar(DateTimeBytes);

                            Trace.WriteLine("Fetching data from: " + _firstTimestamp.ToString());

                            // Write 0x02 to tell the band to start the fetching process
                            await _miBand3.GetCharacteristic(MiBand3Resource.GuidUnknownCharacteristic4).WriteWithoutResponse(new byte[] { 0x02 });
                            Trace.WriteLine("Done writing 0x02");
                        }
                    }
                }

                // Check if done fetching
                else if (responseByte.SequenceEqual(new byte[3] { 0x10, 0x02, 0x01 }))
                {
                    Trace.WriteLine("Done Fetching: " + _samples.Count + " Samples");

                    Trace.WriteLine(_lastTimestamp >= DateTime.Now.AddMinutes(-1));
                    if (_lastTimestamp >= DateTime.Now.AddMinutes(-1))
                    {
                        _callback(_samples);
                        _charActivitySub?.Dispose();
                        _charUnknownSub?.Dispose();
                    }
                    else
                    {
                        Trace.WriteLine("else-statement");
                        _progressCallback((float)(_expectedSamples));
                        await InitiateFetching(_lastTimestamp.AddMinutes(1));
                    }
                }
                else
                {
                    Trace.WriteLine("Error while Fetching");
                    _callback(_samples);
                    // Error while fetching
                    _charActivitySub?.Dispose();
                    _charUnknownSub?.Dispose();
                }
            }
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
                if (_lastTimestamp > DateTime.Now.AddMinutes(-1))
                {
                    Trace.WriteLine("Done Fetching: " + _samples.Count + " Samples");
                }
                Trace.WriteLine("Need More fetching");
                InitiateFetching(_lastTimestamp.AddMinutes(1));
            }
            else
            {
                var samplecount = _samplenumber;
                _samplenumber++;
                var i = 1;
                while (i < result.Data.Length)
                {
                    int timeIndex = (samplecount) * 4 + (i - 1) / 4;
                    var timeStamp = _firstTimestamp.AddMinutes(timeIndex);
                    _lastTimestamp = timeStamp;

                    // Create a sample from the recieved bytes
                    byte[] rawdata = new byte[] { result.Data[i], result.Data[i + 1], result.Data[i + 2], result.Data[i + 3]};
                    var category = result.Data[i] & 0xff;
                    var intensity = result.Data[i + 1] & 0xff;
                    var steps = result.Data[i + 2] & 0xff;
                    var heartrate = result.Data[i + 3];

                    // Add the sample to the sample list
                    _samples.Add(new ActivitySample(timeStamp, category, intensity, steps, heartrate, rawdata));

                    i += 4;

                    var d = DateTime.Now.AddMinutes(-1);
                    d.AddSeconds(-d.Second);
                    d.AddMilliseconds(-d.Millisecond);

                    // Make sure we aren't getting samples from the future
                    if (timeStamp == d)
                    {
                        Trace.WriteLine("Done Fetching");
                        break;
                    }
                }
            }
        }
    }
}
