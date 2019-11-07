using System;
using WindesHeartApp.Resources;
using WindesHeartSDK.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WindesHeartApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public static Label batteryLabel;
        public static Label HRLabel;
        public static AbsoluteLayout absoluteLayout;
        public HomePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            Globals.BuildGlobals();
            BuildPage();
            App.RequestLocationPermission();
        }

        private void BuildPage()
        {
            absoluteLayout = new AbsoluteLayout();

            PageBuilder.BuildPageBasics(absoluteLayout, this);
            PageBuilder.BuildAndAddHeaderImages(absoluteLayout);

            #region define battery Label and ProgressBar TEST TEST
            batteryLabel = new Label { FontSize = Globals.screenHeight / 100 * 2.5, FontAttributes = FontAttributes.Bold, Text = $"Battery level: {Globals.batteryPercentage.ToString()}" };
            AbsoluteLayout.SetLayoutBounds(batteryLabel, new Rectangle(0.95, 0.18, -1, -1));
            AbsoluteLayout.SetLayoutFlags(batteryLabel, AbsoluteLayoutFlags.PositionProportional);

            ProgressBar batteryBar = new ProgressBar { ProgressColor = Globals.secondaryColor, Progress = Globals.batteryPercentage / 100 };
            AbsoluteLayout.SetLayoutBounds(batteryBar, new Rectangle(0.95, 0.2, 0.5, 0.15));
            AbsoluteLayout.SetLayoutFlags(batteryBar, AbsoluteLayoutFlags.All);

            absoluteLayout.Children.Add(batteryBar);
            absoluteLayout.Children.Add(batteryLabel);

            HRLabel = new Label { FontSize = Globals.screenHeight / 100 * 2.5, FontAttributes = FontAttributes.Bold, Text = $"HR {Globals.heartRate.ToString()}" };
            AbsoluteLayout.SetLayoutBounds(HRLabel, new Rectangle(0.5, 0.18, -1, -1));
            AbsoluteLayout.SetLayoutFlags(HRLabel, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(HRLabel);
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

        public static void updateHeartrateLabel(int heartrateHeartrateValue)
        {
            HRLabel.Text = $"HR: {heartrateHeartrateValue}";
        }

        public void GetHeartrate(Heartrate heartrate)
        {
            Console.WriteLine(heartrate.HeartrateValue);

            Device.BeginInvokeOnMainThread(delegate
            {
                updateHeartrateLabel(heartrate.HeartrateValue);
            });
            //            HRLabel.Text = $"HR: {heartrate.HeartrateValue}";

        }
    }
}