using FormsControls.Base;
using System;
using System.Threading.Tasks;
using WindesHeartApp.Resources;
using WindesHeartApp.Services;
using WindesHeartApp.Views;
using WindesHeartSDK;
using WindesHeartSDK.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Image = Xamarin.Forms.Image;
using Label = Xamarin.Forms.Label;

namespace WindesHeartApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage, IAnimationPage
    {
        private readonly string _key = "LastConnectedDeviceGuid";
        public HomePage()
        {
            InitializeComponent();
            BindingContext = Globals.homepageviewModel;
            BuildPage();
        }

        protected override void OnAppearing()
        {

            App.RequestLocationPermission();
            if (Windesheart.ConnectedDevice != null)
                ReadCurrentBattery();

        }


        private void BuildPage()
        {
            absoluteLayout = new AbsoluteLayout();
            PageBuilder.BuildPageBasics(absoluteLayout, this);
            PageBuilder.AddLabel(absoluteLayout, "Home", 0.05, 0.10, Globals.LightTextColor, "", 30);
            PageBuilder.AddHeaderImages(absoluteLayout);

            #region define battery and hr Label
            Image batteryImage = new Image();
            batteryImage.SetBinding(Image.SourceProperty, new Binding("BatteryImage"));
            batteryImage.HeightRequest = Globals.ScreenHeight / 100 * 2.5;

            AbsoluteLayout.SetLayoutBounds(batteryImage, new Rectangle(0.85, 0.18, -1, -1));
            AbsoluteLayout.SetLayoutFlags(batteryImage, AbsoluteLayoutFlags.PositionProportional);

            Label batteryLabel = new Label
            { FontSize = Globals.ScreenHeight / 100 * 2.5, FontAttributes = FontAttributes.Bold };
            batteryLabel.SetBinding(Label.TextProperty, new Binding("DisplayBattery"));
            AbsoluteLayout.SetLayoutBounds(batteryLabel, new Rectangle(0.95, 0.18, -1, -1));
            AbsoluteLayout.SetLayoutFlags(batteryLabel, AbsoluteLayoutFlags.PositionProportional);

            absoluteLayout.Children.Add(batteryImage);
            absoluteLayout.Children.Add(batteryLabel);

            Image heartrateImage = new Image();
            heartrateImage.SetBinding(Image.SourceProperty, new Binding("HeartImage"));
            heartrateImage.HeightRequest = Globals.ScreenHeight / 100 * 2.5;

            AbsoluteLayout.SetLayoutBounds(heartrateImage, new Rectangle(0.85, 0.18, -1, -1));
            AbsoluteLayout.SetLayoutFlags(heartrateImage, AbsoluteLayoutFlags.PositionProportional);
            Label HRLabel = new Label
            { FontSize = Globals.ScreenHeight / 100 * 2.5, FontAttributes = FontAttributes.Bold };
            HRLabel.SetBinding(Label.TextProperty, new Binding("DisplayHeartRate"));
            AbsoluteLayout.SetLayoutBounds(HRLabel, new Rectangle(0.05, 0.18, -1, -1));
            AbsoluteLayout.SetLayoutFlags(HRLabel, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(HRLabel);
            absoluteLayout.Children.Add(heartrateImage);
            #endregion 

            if (App.Current.Properties.ContainsKey(_key))
            {
                App.Current.Properties.TryGetValue(_key, out object result);
                ConnectKnowDevice(result);
            }

            #region define and add Buttons
            var buttonStyle = new Style(typeof(Button))
            {
                Setters =
                {
                    new Setter
                    {
                        Property = Button.FontSizeProperty,
                        Value = Globals.CornerRadius / 4
                    },
                    new Setter
                    {
                        Property = Button.CornerRadiusProperty,
                        Value = (int)Globals.CornerRadius
                    },
                    new Setter
                    {
                        Property = WidthRequestProperty,
                        Value = ((int)Globals.CornerRadius) *2
                    },
                    new Setter
                    {
                        Property = HeightRequestProperty,
                        Value = ((int)Globals.CornerRadius) *2
                    },
                    new Setter
                    {
                        Property = Button.BackgroundColorProperty,
                        Value = Globals.SecondaryColor
                    },
                    new Setter
                    {
                        Property = Button.VerticalOptionsProperty,
                        Value = LayoutOptions.Center
                    }
                }
            };
            Button aboutButton = new Button { Text = "About", Style = buttonStyle };
            aboutButton.Clicked += AboutButtonClicked;
            AbsoluteLayout.SetLayoutFlags(aboutButton, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(aboutButton, new Rectangle(0.80, 0.90, -1, -1));
            absoluteLayout.Children.Add(aboutButton);

            Button deviceButton = new Button { Text = "Device", Style = buttonStyle };
            deviceButton.Clicked += DeviceButtonClicked;
            AbsoluteLayout.SetLayoutFlags(deviceButton, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(deviceButton, new Rectangle(0.80, 0.40, -1, -1));
            absoluteLayout.Children.Add(deviceButton);

            Button heartrateButton = new Button { Text = "Heartrate", Style = buttonStyle };
            heartrateButton.Clicked += HeartrateButtonClicked;
            AbsoluteLayout.SetLayoutFlags(heartrateButton, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(heartrateButton, new Rectangle(0.10, 0.65, -1, -1));
            absoluteLayout.Children.Add(heartrateButton);

            Button stepsButton = new Button { Text = "Steps", Style = buttonStyle };
            stepsButton.Clicked += StepsButtonClicked;
            AbsoluteLayout.SetLayoutFlags(stepsButton, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(stepsButton, new Rectangle(0.90, 0.65, -1, -1));
            absoluteLayout.Children.Add(stepsButton);

            Button settingsButton = new Button { Text = "Settings", Style = buttonStyle };
            settingsButton.Clicked += SettingsButtonClicked;
            AbsoluteLayout.SetLayoutFlags(settingsButton, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(settingsButton, new Rectangle(0.20, 0.90, -1, -1));
            absoluteLayout.Children.Add(settingsButton);

            settingsButton.Style = buttonStyle;
            Button sleepButton = new Button { Text = "Sleep", Style = buttonStyle };
            sleepButton.Clicked += SleepButtonClicked;
            AbsoluteLayout.SetLayoutFlags(sleepButton, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(sleepButton, new Rectangle(0.20, 0.40, -1, -1));
            absoluteLayout.Children.Add(sleepButton);
            #endregion

            var button =
                PageBuilder.AddButton(absoluteLayout, "TEST", "", 0.5, 0.5, 0.4, 0.05, AbsoluteLayoutFlags.All);
            button.Clicked += TestButtonClicked;
        }

        private async void ConnectKnowDevice(object result)
        {
            var device = await Windesheart.GetKnownDevice((Guid)result);
            device?.Connect(CallbackHandler.OnConnetionCallBack);
        }

        #region button eventhandlers
        private void TestButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TestPage());
        }

        private void SettingsButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsPage()
            {
                BindingContext = Globals.settingspageviewModel
            });
        }

        private void StepsButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new StepsPage()
            {
                BindingContext = Globals.StepsViewModel
            });
        }

        private void HeartrateButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new HeartratePage()
            {
                BindingContext = Globals.heartrateviewModel
            });
        }

        private void SleepButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SleepPage()
            {
                BindingContext = Globals.heartrateviewModel
            });
        }

        private void DeviceButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DevicePage());

        }

        private void AboutButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AboutPage());
        }
        #endregion

        private async Task ReadCurrentBattery()
        {
            var battery = await Windesheart.ConnectedDevice.GetBattery();
            Console.WriteLine("Battery: " + battery.BatteryPercentage + "%");
            Globals.homepageviewModel.Battery = battery.BatteryPercentage;
            if (battery.Status == StatusEnum.Charging)
            {
                Globals.homepageviewModel.BatteryImage = "BatteryCharging.png";
                return;
            }
            if (battery.BatteryPercentage >= 0 && battery.BatteryPercentage < 26)
            {
                Globals.homepageviewModel.BatteryImage = "BatteryQuart.png";
            }
            else if (battery.BatteryPercentage >= 26 && battery.BatteryPercentage < 51)
            {
                Globals.homepageviewModel.BatteryImage = "BatteryHalf.png";
            }
            else if (battery.BatteryPercentage >= 51 && battery.BatteryPercentage < 76)
            {
                Globals.homepageviewModel.BatteryImage = "BatteryThreeQuarts.png";
            }
            else if (battery.BatteryPercentage >= 76)
            {
                Globals.homepageviewModel.BatteryImage = "BatteryFull.png";
            }
        }
        public IPageAnimation PageAnimation { get; } = new SlidePageAnimation { Duration = AnimationDuration.Short, Subtype = AnimationSubtype.FromTop };

        public void OnAnimationStarted(bool isPopAnimation)
        {
            // Put your code here but leaving empty works just fine
        }

        public void OnAnimationFinished(bool isPopAnimation)
        {
            // Put your code here but leaving empty works just fine
        }
    }
}