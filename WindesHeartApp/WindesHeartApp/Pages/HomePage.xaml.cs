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

            grid = new Grid();
            grid.BackgroundColor = Color.DeepPink;


            layout.Children.Add(grid);

            #region define Rows+Columns
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
            #endregion

            #region add Rows+Columns
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


            #endregion

            #region define and add Images
            Image heartonlyImage = new Image();
            heartonlyImage.Source = "HeartOnlyTransparent.png";
            heartonlyImage.HorizontalOptions = LayoutOptions.StartAndExpand;
            heartonlyImage.Margin = new Thickness(10, 0, 0, 0);
            heartonlyImage.BackgroundColor = Globals.primaryColor;
            grid.Children.Add(heartonlyImage, 0, 0);

            Image textonlyImage = new Image();
            textonlyImage.Source = "TextOnlyTransparent.png";
            textonlyImage.HorizontalOptions = LayoutOptions.End;
            textonlyImage.Margin = new Thickness(0, 0, 7, 0);
            textonlyImage.BackgroundColor = Globals.primaryColor;
            grid.Children.Add(textonlyImage, 1, 0);
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
                        Property = Button.MarginProperty,
                        Value = new Thickness(10, 10, 10, 10)
                    },
                    new Setter
                    {
                        Property = Button.CornerRadiusProperty,
                        Value = (int)Globals.cornerRadius
                    },
                    new Setter
                    {
                        Property = Button.WidthRequestProperty,
                        Value = ((int)Globals.cornerRadius) * 2
                    },
                    new Setter
                    {
                        Property = Button.HeightRequestProperty,
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
            aboutButton.HorizontalOptions = LayoutOptions.End;
            aboutButton.Clicked += aboutbutton_Clicked;
            aboutButton.Style = buttonStyle;
            grid.Children.Add(aboutButton, 0, 2);

            Button deviceButton = new Button();
            deviceButton.Text = "Device";
            deviceButton.HorizontalOptions = LayoutOptions.Start;
            deviceButton.Clicked += deviceButton_Clicked;
            deviceButton.Style = buttonStyle;
            grid.Children.Add(deviceButton, 1, 2);

            Button heartrateButton = new Button();
            heartrateButton.Text = "Heartrate";
            heartrateButton.HorizontalOptions = LayoutOptions.End;
            heartrateButton.Clicked += heartrateButton_Clicked;
            heartrateButton.Style = buttonStyle;
            heartrateButton.TranslationX = Globals.screenWidth * 0.5 / -4;
            grid.Children.Add(heartrateButton, 0, 3);

            Button stepsButton = new Button();
            stepsButton.Text = "Steps";
            stepsButton.HorizontalOptions = LayoutOptions.Start;
            stepsButton.Clicked += stepsButton_Clicked;
            stepsButton.Style = buttonStyle;
            stepsButton.TranslationX = Globals.screenWidth * 0.5 / 4;
            grid.Children.Add(stepsButton, 1, 3);

            Button settingsButton = new Button();
            settingsButton.Text = "Settings";
            settingsButton.HorizontalOptions = LayoutOptions.End;
            settingsButton.Clicked += settingsButton_Clicked;
            settingsButton.Style = buttonStyle;
            grid.Children.Add(settingsButton, 0, 4);

            Button testButton = new Button();
            testButton.Text = "Test";
            testButton.HorizontalOptions = LayoutOptions.Start;
            testButton.Clicked += testButton_Clicked;
            testButton.Style = buttonStyle;
            grid.Children.Add(testButton, 1, 4);
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