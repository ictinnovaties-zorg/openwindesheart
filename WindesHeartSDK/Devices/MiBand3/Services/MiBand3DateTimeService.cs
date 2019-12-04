using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using WindesHeartSDK.Devices.MiBand3Device.Resources;
using WindesHeartSDK.Helpers;

namespace WindesHeartSDK.Devices.MiBand3Device.Services
{
    public class MiBand3DateTimeService
    {
        private readonly MiBand3.Models.MiBand3 MiBand3;

        public MiBand3DateTimeService(MiBand3.Models.MiBand3 device)
        {
            MiBand3 = device;
        }

        public bool SetTime(DateTime time)
        {
            //Convert time to bytes
            byte[] timeToSet = ConversionHelper.GetTimeBytes(time, ConversionHelper.TimeUnit.Seconds);

            //Send to MiBand
            MiBand3.GetCharacteristic(MiBand3Resource.GuidCharacteristicCurrentTime).Write(timeToSet).Subscribe(result =>
            {
                Console.WriteLine("Time set to " + time.ToString());
            });
            return true;
        }
    }
}
