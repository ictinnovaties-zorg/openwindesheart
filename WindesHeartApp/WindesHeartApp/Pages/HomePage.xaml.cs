using System;
using WindesHeartApp.Resources;
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
            Globals.BuildGlobals();
            BuildPage();
        }

        private void BuildPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            absoluteLayout.BackgroundColor = Globals.primaryColor;

            #region define and add Images
            Image heartonlyImage = new Image();
            heartonlyImage.Source = "HeartOnlyTransparent.png";
            AbsoluteLayout.SetLayoutFlags(heartonlyImage, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(heartonlyImage, new Rectangle(0.05, 0, Globals.screenWidth / 100 * 20, Globals.screenHeight / 100 * 10));
            absoluteLayout.Children.Add(heartonlyImage);

            Image textonlyImage = new Image();
            textonlyImage.Source = "TextOnlyTransparent.png";
            AbsoluteLayout.SetLayoutFlags(textonlyImage, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(textonlyImage, new Rectangle(0.95, 0, Globals.screenWidth / 100 * 60, Globals.screenHeight / 100 * 10));
            absoluteLayout.Children.Add(textonlyImage);
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


        private void testButton_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("TestButton CLICKED");
            Navigation.PushAsync(new TestPage());
        }

        private void settingsButton_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("SettingsButton CLICKED");
        }

        private void stepsButton_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("StepsButton CLICKED");
        }

        private void heartrateButton_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("HeartrateButton CLICKED");
        }

        private void deviceButton_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("DeiceButton Clicked");
        }

        private void aboutbutton_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("AboutButton Clicked!");
            Navigation.PushAsync(new AboutPage());
        }
    }
}