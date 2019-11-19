using System;
using WindesHeartApp.Resources;
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
            BindingContext = Globals.homepageviewModel;
            BuildPage();
            App.RequestLocationPermission();
        }


        private void BuildPage()
        {
            absoluteLayout = new AbsoluteLayout();
            PageBuilder.BuildPageBasics(absoluteLayout, this);
            PageBuilder.BuildAndAddHeaderImages(absoluteLayout);

            #region define battery and hr Label
            Image batteryImage = new Image();
            batteryImage.SetBinding(Image.SourceProperty, new Binding("BatteryImage"));
            batteryImage.HeightRequest = Globals.screenHeight / 100 * 2.5;

            AbsoluteLayout.SetLayoutBounds(batteryImage, new Rectangle(0.85, 0.18, -1, -1));
            AbsoluteLayout.SetLayoutFlags(batteryImage, AbsoluteLayoutFlags.PositionProportional);

            Label batteryLabel = new Label
            { FontSize = Globals.screenHeight / 100 * 2.5, FontAttributes = FontAttributes.Bold };
            batteryLabel.SetBinding(Label.TextProperty, new Binding("DisplayBattery"));
            AbsoluteLayout.SetLayoutBounds(batteryLabel, new Rectangle(0.95, 0.18, -1, -1));
            AbsoluteLayout.SetLayoutFlags(batteryLabel, AbsoluteLayoutFlags.PositionProportional);

            absoluteLayout.Children.Add(batteryImage);
            absoluteLayout.Children.Add(batteryLabel);

            Image heartrateImage = new Image();
            heartrateImage.SetBinding(Image.SourceProperty, new Binding("HeartImage"));
            heartrateImage.HeightRequest = Globals.screenHeight / 100 * 2.5;

            AbsoluteLayout.SetLayoutBounds(heartrateImage, new Rectangle(0.85, 0.18, -1, -1));
            AbsoluteLayout.SetLayoutFlags(heartrateImage, AbsoluteLayoutFlags.PositionProportional);
            Label HRLabel = new Label
            { FontSize = Globals.screenHeight / 100 * 2.5, FontAttributes = FontAttributes.Bold };
            HRLabel.SetBinding(Label.TextProperty, new Binding("DisplayHeartRate"));
            AbsoluteLayout.SetLayoutBounds(HRLabel, new Rectangle(0.05, 0.18, -1, -1));
            AbsoluteLayout.SetLayoutFlags(HRLabel, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(HRLabel);
            absoluteLayout.Children.Add(heartrateImage);
            #endregion

            #region define and add Buttons
            var buttonStyle = new Style(typeof(Button))
            {
                Setters =
                {
                    new Setter
                    {
                        Property = Button.FontSizeProperty,
                        Value = Globals.cornerRadius / 4
                    },
                    new Setter
                    {
                        Property = Button.CornerRadiusProperty,
                        Value = (int)Globals.cornerRadius
                    },
                    new Setter
                    {
                        Property = WidthRequestProperty,
                        Value = ((int)Globals.cornerRadius) *2
                    },
                    new Setter
                    {
                        Property = HeightRequestProperty,
                        Value = ((int)Globals.cornerRadius) *2
                    },
                    new Setter
                    {
                        Property = Button.BackgroundColorProperty,
                        Value = Globals.secondaryColor
                    },
                    new Setter
                    {
                        Property = Button.VerticalOptionsProperty,
                        Value = LayoutOptions.Center
                    }
                }
            };
            Button aboutButton = new Button();
            aboutButton.Text = "About";
            aboutButton.Clicked += aboutbutton_Clicked;
            aboutButton.Style = buttonStyle;
            AbsoluteLayout.SetLayoutFlags(aboutButton, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(aboutButton, new Rectangle(0.80, 0.90, -1, -1));
            absoluteLayout.Children.Add(aboutButton);

            Button deviceButton = new Button();
            deviceButton.Text = "Device";
            deviceButton.Clicked += deviceButton_Clicked;
            deviceButton.Style = buttonStyle;
            AbsoluteLayout.SetLayoutFlags(deviceButton, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(deviceButton, new Rectangle(0.80, 0.40, -1, -1));
            absoluteLayout.Children.Add(deviceButton);

            Button heartrateButton = new Button();
            heartrateButton.Text = "Heartrate";
            heartrateButton.Clicked += heartrateButton_Clicked;
            heartrateButton.Style = buttonStyle;
            AbsoluteLayout.SetLayoutFlags(heartrateButton, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(heartrateButton, new Rectangle(0.10, 0.65, -1, -1));
            absoluteLayout.Children.Add(heartrateButton);

            Button stepsButton = new Button();
            stepsButton.Text = "Steps";
            stepsButton.Clicked += stepsButton_Clicked;
            stepsButton.Style = buttonStyle;
            AbsoluteLayout.SetLayoutFlags(stepsButton, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(stepsButton, new Rectangle(0.90, 0.65, -1, -1));
            absoluteLayout.Children.Add(stepsButton);

            Button settingsButton = new Button();
            settingsButton.Text = "Settings";
            settingsButton.Clicked += settingsButton_Clicked;
            settingsButton.Style = buttonStyle;
            AbsoluteLayout.SetLayoutFlags(settingsButton, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(settingsButton, new Rectangle(0.20, 0.90, -1, -1));
            absoluteLayout.Children.Add(settingsButton);

            Button testButton = new Button();
            testButton.Text = "Test";
            testButton.Clicked += testButton_Clicked;
            testButton.Style = buttonStyle;
            AbsoluteLayout.SetLayoutFlags(testButton, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(testButton, new Rectangle(0.20, 0.40, -1, -1));
            absoluteLayout.Children.Add(testButton);
            #endregion
        }

        #region button eventhandlers
        private void testButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TestPage());
        }

        private void settingsButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsPage());
        }

        private void stepsButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new StepsPage());
        }

        private void heartrateButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new HeartratePage());
        }

        private void deviceButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DevicePage());

        }

        private void aboutbutton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AboutPage());
        }
        #endregion
    }
}