using System;
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
        public Grid grid;
        public HomePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            BuildPage();
        }

        private void BuildPage()
        {
            grid = new Grid();
            grid.BackgroundColor = Globals.primaryColor;

            layout.Children.Add(grid);


            RowDefinition row0 = new RowDefinition { Height = Globals.screenHeight / 10 };
            RowDefinition row1 = new RowDefinition { Height = Globals.screenHeight / 10 * 2 };
            RowDefinition row2 = new RowDefinition { Height = Globals.screenHeight / 10 * 2 };
            RowDefinition row3 = new RowDefinition { Height = Globals.screenHeight / 10 * 2 };
            RowDefinition row4 = new RowDefinition { Height = Globals.screenHeight / 10 * 2 };
            RowDefinition row5 = new RowDefinition { Height = Globals.screenHeight / 10 };
            ColumnDefinition colum0 = new ColumnDefinition { Width = Globals.screenWidth * 0.5 };
            ColumnDefinition colum1 = new ColumnDefinition { Width = Globals.screenWidth * 0.5 };
            ColumnDefinition colum2 = new ColumnDefinition { Width = Globals.screenWidth * 0.5 };
            ColumnDefinition colum3 = new ColumnDefinition { Width = Globals.screenWidth * 0.5 };
            ColumnDefinition colum4 = new ColumnDefinition { Width = Globals.screenWidth * 0.5 };
            ColumnDefinition colum5 = new ColumnDefinition { Width = Globals.screenWidth * 0.5 };
            ColumnDefinition colum6 = new ColumnDefinition { Width = Globals.screenWidth * 0.5 };
            ColumnDefinition colum7 = new ColumnDefinition { Width = Globals.screenWidth * 0.5 };
            ColumnDefinition colum8 = new ColumnDefinition { Width = Globals.screenWidth * 0.5 };
            ColumnDefinition colum9 = new ColumnDefinition { Width = Globals.screenWidth * 0.5 };
            ColumnDefinition colum10 = new ColumnDefinition { Width = Globals.screenWidth * 0.5 };
            ColumnDefinition colum11 = new ColumnDefinition { Width = Globals.screenWidth * 0.5 };

            grid.RowDefinitions.Add(row0);
            grid.RowDefinitions.Add(row1);
            grid.RowDefinitions.Add(row2);
            grid.RowDefinitions.Add(row3);
            grid.RowDefinitions.Add(row4);
            grid.RowDefinitions.Add(row5);
            grid.ColumnDefinitions.Add(colum0);
            grid.ColumnDefinitions.Add(colum1);
            grid.ColumnDefinitions.Add(colum2);
            grid.ColumnDefinitions.Add(colum3);
            grid.ColumnDefinitions.Add(colum4);
            grid.ColumnDefinitions.Add(colum5);
            grid.ColumnDefinitions.Add(colum6);
            grid.ColumnDefinitions.Add(colum7);
            grid.ColumnDefinitions.Add(colum8);
            grid.ColumnDefinitions.Add(colum9);
            grid.ColumnDefinitions.Add(colum10);
            grid.ColumnDefinitions.Add(colum11);

            Image heartonlyImage = new Image();
            heartonlyImage.Source = "HeartOnlyTransparent.png";
            heartonlyImage.HorizontalOptions = LayoutOptions.StartAndExpand;
            heartonlyImage.Margin = new Thickness(10, 0, 0, 0);
            heartonlyImage.BackgroundColor = Globals.primaryColor;

            Image textonlyImage = new Image();
            textonlyImage.Source = "TextOnlyTransparent.png";
            textonlyImage.HorizontalOptions = LayoutOptions.End;
            textonlyImage.Margin = new Thickness(0, 0, 7, 0);
            textonlyImage.BackgroundColor = Globals.primaryColor;

            grid.Children.Add(heartonlyImage, 0, 0);
            grid.Children.Add(textonlyImage, 1, 0);



            Button aboutButton = new Button();
            aboutButton.Text = "About";
            aboutButton.BackgroundColor = Globals.primaryColor;
            aboutButton.BorderColor = Color.White;
            aboutButton.BorderWidth = 1;
            aboutButton.HorizontalOptions = LayoutOptions.End;
            aboutButton.VerticalOptions = LayoutOptions.Center;
            aboutButton.CornerRadius = (int)((Globals.screenHeight / 10 * 1) - Globals.buttonSize);
            aboutButton.HeightRequest = aboutButton.CornerRadius * 2 - 20;
            aboutButton.WidthRequest = aboutButton.CornerRadius * 2 - 20;
            aboutButton.Clicked += aboutbutton_Clicked;
            aboutButton.Margin = new Thickness(10, 10, 10, 10);

            grid.Children.Add(aboutButton, 0, 2);

            Button deviceButton = new Button();
            deviceButton.Text = "Device";
            deviceButton.BackgroundColor = Globals.primaryColor;
            deviceButton.BorderColor = Color.White;
            deviceButton.BorderWidth = 1;
            deviceButton.HorizontalOptions = LayoutOptions.Start;
            deviceButton.VerticalOptions = LayoutOptions.Center;
            deviceButton.CornerRadius = (int)((Globals.screenHeight / 10 * 1) - Globals.buttonSize);
            deviceButton.HeightRequest = deviceButton.CornerRadius * 2 - 20;
            deviceButton.WidthRequest = deviceButton.CornerRadius * 2 - 20;
            deviceButton.Clicked += deviceButton_Clicked;
            deviceButton.Margin = new Thickness(10, 10, 10, 10);

            grid.Children.Add(deviceButton, 1, 2);

            Button heartrateButton = new Button();
            heartrateButton.Text = "Heartrate";
            heartrateButton.BackgroundColor = Globals.primaryColor;
            heartrateButton.BorderColor = Color.White;
            heartrateButton.BorderWidth = 1;
            heartrateButton.HorizontalOptions = LayoutOptions.End;
            heartrateButton.VerticalOptions = LayoutOptions.Center;
            heartrateButton.CornerRadius = (int)((Globals.screenHeight / 10 * 1) - Globals.buttonSize);
            heartrateButton.HeightRequest = heartrateButton.CornerRadius * 2 - 20;
            heartrateButton.WidthRequest = heartrateButton.CornerRadius * 2 - 20;
            heartrateButton.Clicked += heartrateButton_Clicked;
            heartrateButton.Margin = new Thickness(10, 10, 10, 10);

            grid.Children.Add(heartrateButton, 0, 3);

            Button stepsButton = new Button();
            stepsButton.Text = "Steps";
            stepsButton.BackgroundColor = Globals.primaryColor;
            stepsButton.BorderColor = Color.White;
            stepsButton.BorderWidth = 1;
            stepsButton.HorizontalOptions = LayoutOptions.Start;
            stepsButton.VerticalOptions = LayoutOptions.Center;
            stepsButton.CornerRadius = (int)((Globals.screenHeight / 10 * 1) - Globals.buttonSize);
            stepsButton.HeightRequest = stepsButton.CornerRadius * 2 - 20;
            stepsButton.WidthRequest = stepsButton.CornerRadius * 2 - 20;
            stepsButton.Clicked += stepsButton_Clicked;
            stepsButton.Margin = new Thickness(10, 10, 10, 10);

            grid.Children.Add(stepsButton, 1, 3);


            Button settingsButton = new Button();
            settingsButton.Text = "Settings";
            settingsButton.BackgroundColor = Globals.primaryColor;
            settingsButton.BorderColor = Color.White;
            settingsButton.BorderWidth = 1;
            settingsButton.HorizontalOptions = LayoutOptions.End;
            settingsButton.VerticalOptions = LayoutOptions.Center;
            settingsButton.CornerRadius = (int)((Globals.screenHeight / 10 * 1) - Globals.buttonSize);
            settingsButton.HeightRequest = settingsButton.CornerRadius * 2 - 20;
            settingsButton.WidthRequest = settingsButton.CornerRadius * 2 - 20;
            settingsButton.Clicked += settingsButton_Clicked;
            settingsButton.Margin = new Thickness(10, 10, 10, 10);

            grid.Children.Add(settingsButton, 0, 4);

            Button testButton = new Button();
            testButton.Text = "Test";
            testButton.BackgroundColor = Globals.primaryColor;
            testButton.BorderColor = Color.White;
            testButton.BorderWidth = 1;
            testButton.HorizontalOptions = LayoutOptions.Start;
            testButton.VerticalOptions = LayoutOptions.Center;
            testButton.CornerRadius = (int)((Globals.screenHeight / 10 * 1) - Globals.buttonSize);
            testButton.HeightRequest = testButton.CornerRadius * 2 - 20;
            testButton.WidthRequest = testButton.CornerRadius * 2 - 20;
            testButton.Clicked += testButton_Clicked;
            testButton.Margin = new Thickness(10, 10, 10, 10);

            grid.Children.Add(testButton, 1, 4);


        }

        private void testButton_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("CLICKED");
        }

        private void settingsButton_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("CLICKED");
        }

        private void stepsButton_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("CLICKED");
        }

        private void heartrateButton_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("CLICKED");
        }

        private void deviceButton_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("device Button Clicked");
        }

        private void aboutbutton_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("started from the bottom");
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await BluetoothService.ScanForUniqueDevicesAsync();
            if (BluetoothService.ScanResults.Count > 0 && BluetoothService.ScanResults[0] != null)
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
                }
                catch (BatteryException exception)
                {
                    Console.WriteLine(exception);
                }
            }
            else
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