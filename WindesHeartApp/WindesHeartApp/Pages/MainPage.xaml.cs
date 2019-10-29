using System;
using System.Threading.Tasks;
using WindesHeartSDK;
using WindesHeartSDK.Services;
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
            BluetoothService.FindAllCharacteristics(BluetoothService.ScanResults[0].Device);
        }

        private async void Connect(object sender, EventArgs e)
        {
            BluetoothService.ConnectDevice(BluetoothService.ScanResults[0].Device);
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
                var rawBattery = await MiBandService.GetRawBatteryData();
                var battery = await MiBandService.GetCurrentBatteryData();
                Console.WriteLine();
            }
        }
    }
}