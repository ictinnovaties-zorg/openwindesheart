/* Copyright 2020 Research group ICT innovations in Health Care

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
using System.Reactive.Linq;
using OpenWindesheart.Devices.MiBand3Device.Models;
using OpenWindesheart.Devices.MiBand3Device.Resources;
using OpenWindesheart.Helpers;

namespace OpenWindesheart.Devices.MiBand3Device.Services
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
