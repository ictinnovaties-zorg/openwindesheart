using System;
using System.Threading.Tasks;
using WindesHeartApp.Resources;
using WindesHeartSDK;
using WindesHeartSDK.Devices.MiBand3.Services;
using WindesHeartSDK.Exceptions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WindesHeartApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }
        
        protected override void OnAppearing()
        {
            AbsoluteLayout.BackgroundColor = Globals.primaryColor;
            //AbsoluteLayout.WidthRequest = Globals.screenWidth;
            //AbsoluteLayout.HeightRequest = Globals.screenHeight;

            var screenRatioFactor = Globals.screenHeight / Globals.screenWidth;

            HeartImage.BackgroundColor = Globals.primaryColor;
            TextImage.BackgroundColor = Globals.primaryColor;

            //AbsoluteLayout.SetLayoutBounds(HeartImage, new Rectangle(0.5, 0.5, Globals.buttonSize, Globals.buttonSize));
            //AbsoluteLayout.SetLayoutFlags(HeartImage, AbsoluteLayoutFlags.All);
            //AbsoluteLayout.SetLayoutBounds(TextImage, new Rectangle(0.8, 0.8, Globals.buttonSize*Globals.screenratioFactor, Globals.buttonSize));
            //AbsoluteLayout.SetLayoutFlags(TextImage, AbsoluteLayoutFlags.All);

            AbsoluteLayout.SetLayoutBounds(TestButton, new Rectangle(0.5, 0.5, Globals.buttonSize * Globals.screenratioFactor, Globals.buttonSize));
            AbsoluteLayout.SetLayoutFlags(TestButton, AbsoluteLayoutFlags.All);
            TestButton.CornerRadius = (int)(Globals.screenHeight / 8 / 2);

        }

        /*<Image Margin = "10,10,500, 500" Source="WindesHeartTransparent.png" BackgroundColor="{StaticResource Primary}">
</Image>


<Button HeightRequest = "50" WidthRequest="50" Text="About" AbsoluteLayout.LayoutBounds="0.15,0.45" AbsoluteLayout.LayoutFlags="PositionProportional"/>
*/
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
            Navigation.PushModalAsync(new HomePage());
        }
    }
}