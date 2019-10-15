using Plugin.BluetoothLE;
using System;

namespace WindesHeartSDK
{
    public static class BluetoothService
    {
        /// <summary>
        /// Tries to pair to the devices. If the pairing is succesfull returns true.
        /// </summary>
        /// <param name="device"></param>
        /// <returns>bool</returns>
        public static bool PairDevice(IDevice device)
        {
            bool succes = false;
            // Checks if devices supports pairing
            if (device.IsPairingAvailable())
            {
                // If device isn't paired yet pair the device
                if (device.PairingStatus != PairingStatus.Paired)
                {
                    device.PairingRequest().Subscribe(isSuccessful =>
                    {
                        Console.WriteLine("Pairing Succesfull: " + isSuccessful);
                        if (isSuccessful)
                        {
                            succes = true;
                        }
                    });
                    return succes;
                }
                else
                {
                    succes = true;
                    return succes;
                }
            }
            return succes;
        }
    }
}
