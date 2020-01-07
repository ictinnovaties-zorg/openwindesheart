using System;
using System.Reactive.Linq;
using WindesHeartSDK.Devices.MiBand3Device.Models;
using WindesHeartSDK.Devices.MiBand3Device.Resources;
using WindesHeartSDK.Helpers;

namespace WindesHeartSDK.Devices.MiBand3Device.Services
{
    public class MiBand3DateTimeService
    {
        private readonly MiBand3 _miBand3;

        public MiBand3DateTimeService(MiBand3 device)
        {
            _miBand3 = device;
        }

        public void SetTime(DateTime time)
        {
            //Convert time to bytes
            byte[] timeToSet = ConversionHelper.GetTimeBytes(time, ConversionHelper.TimeUnit.Seconds);

            //Send to MiBand
            _miBand3.GetCharacteristic(MiBand3Resource.GuidCurrentTime).Write(timeToSet).Subscribe(result =>
            {
                Console.WriteLine("Time set to " + time.ToString());
            });
        }
    }
}
