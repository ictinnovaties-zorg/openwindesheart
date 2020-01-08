using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Text;
using WindesHeartSDK.Devices.MiBand3Device.Models;
using WindesHeartSDK.Devices.MiBand3Device.Resources;
using WindesHeartSDK.Devices.MiBand4Device.Services;

namespace WindesHeartSDK.Devices.MiBand4Device.Models
{

    // Due to the Mi band 4 working the same as the Mi band 3, this class extends the MiBand3 class and only overrides the authentication
    public class MiBand4 : MiBand3
    {
        private readonly MiBand4AuthenticationService _authenticationService;


        public MiBand4(IDevice device) : base(device)
        {
            _authenticationService = new MiBand4AuthenticationService(this);

        }

        public MiBand4() : base()
        {

        }

        public override void OnConnect()
        {
            Console.WriteLine("Device Connected!");

            Windesheart.PairedDevice = this;

            //Check if bluetooth-state changes to off and then on, to enable reconnection management
            BluetoothService.StartListeningForAdapterChanges();

            Characteristics?.Clear();

            CharacteristicDisposable?.Dispose();
            //Find unique characteristics
            CharacteristicDisposable = IDevice.WhenAnyCharacteristicDiscovered().Subscribe(async characteristic =>
            {
                if (characteristic != null && !Characteristics.Contains(characteristic))
                {
                    Characteristics.Add(characteristic);

                    //Check if authCharacteristic has been found, then authenticate
                    if (characteristic.Uuid == MiBand3Resource.GuidCharacteristicAuth)
                    {
                        //Check if this is a new connection that needs authentication
                        await _authenticationService.Authenticate();
                    }
                }
            });
        }
    }
}
