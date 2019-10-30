using System;
using System.Threading.Tasks;
using WindesHeartSDK;
using WindesHeartSDK.Devices.MiBand3.Services;
using WindesHeartSDK.Exceptions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WindesHeartApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await BluetoothService.ScanForUniqueDevicesAsync();
            if(BluetoothService.ScanResults.Count > 0 && BluetoothService.ScanResults[0] != null)
            {
                BluetoothService.FindAllCharacteristics(BluetoothService.ScanResults[0].Device);
            }
        }

        private async void Connect(object sender, EventArgs e)
        {
            if (BluetoothService.ScanResults.Count > 0 && BluetoothService.ScanResults[0] != null)
            {
                BluetoothService.ConnectDevice(BluetoothService.ScanResults[0].Device);
            }
        }

        private async void Disconnect(object sender, EventArgs e)
        {
            BluetoothService.DisconnectDevice(BluetoothService.ConnectedDevice);
        }

        private async void ReadBattery(object sender, EventArgs e)
        {
            var connectedDevice = BluetoothService.ConnectedDevice;
            if (connectedDevice != null)
            {
                try
                {
                    var rawBattery = await BatteryService.GetRawBatteryData();
                    var battery = await BatteryService.GetCurrentBatteryData();
                    Console.WriteLine("Battery: " + battery.BatteryPercentage + "%");
                    Console.WriteLine("Batterystatus: " + battery.Status);
                } catch(BatteryException exception)
                {
                    Console.WriteLine(exception);
                }
            } else
            {
                Console.WriteLine("There is no connected device.");
            }
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new MainPage());
        }
    }
}