using System;
using WindesHeartApp.Resources;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WindesHeartApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            BuildPage();
        }

        private void BuildPage()
        {
            layout.BackgroundColor = Globals.primaryColor;
            NavigationPage.SetHasNavigationBar(this, false);
            Grid grid = new Grid();
            Image windesheartImage = new Image();
            windesheartImage.Source = "WindesHeartTransparent.png";
            windesheartImage.BackgroundColor = Color.Transparent;
            windesheartImage.VerticalOptions = LayoutOptions.Start;
            windesheartImage.HorizontalOptions = LayoutOptions.Center;
            windesheartImage.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(execute: () => { Logo_Clicked(this, EventArgs.Empty); })
            });

            AbsoluteLayout.SetLayoutFlags(grid, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(grid, new Rectangle(0.5, 0, Globals.screenWidth, Globals.screenHeight / 100 * 35));
            layout.Children.Add(grid);
            grid.Children.Add(windesheartImage);



            //            <Label TranslationY="-50" Margin="10" Text="About" HorizontalTextAlignment="Start"  FontSize="Title" ></Label>

            Label aboutLabel = new Label
            {
                Text = "About",
                TextColor = Globals.lighttextColor,
                HorizontalOptions = LayoutOptions.Start,
                FontSize = Globals.screenHeight / 100 * 3,
            };
            Grid box = new Grid();
            box.Margin = new Thickness(10, 0, 10, 0);
            AbsoluteLayout.SetLayoutBounds(box, new Rectangle(0, (Globals.screenHeight / 100 * 35) - Globals.screenHeight / 100 * 5, Globals.screenWidth, Globals.screenHeight / 100 * 3.5));
            box.Children.Add(aboutLabel);
            //layout.Children.Add(aboutLabel);
            //ImageButton returnButton = new ImageButton();
            //returnButton.Source = "GoBack.png";
            //returnButton.BackgroundColor = Color.Transparent; ;
            //returnButton.CornerRadius = (int)(Globals.screenWidth / 20);
            //returnButton.Clicked += returnButton_Clicked;
            //returnButton.WidthRequest = Globals.screenHeight / 10;
            //returnButton.HeightRequest = Globals.screenHeight / 10;
            //layout.Children.Add(returnButton);\




            Grid grid1 = new Grid();
            grid1.BackgroundColor = Color.Transparent;
            grid1.Margin = new Thickness(25, 0, 25, 0);
            AbsoluteLayout.SetLayoutFlags(grid1, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(grid1, new Rectangle(0, 0.5, Globals.screenWidth, Globals.screenHeight / 100 * 2.5));

            Label writtenLabel = new Label
            {
                Text = "This app is written by Windesheim Students",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                LineBreakMode = LineBreakMode.WordWrap
            };
            grid1.Children.Add(writtenLabel);

            layout.Children.Add(box);
            layout.Children.Add(grid1);
        }

        private void returnButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void Logo_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("Logo - Clicked.");
            Navigation.PopAsync();
        }

        private void LearnMore_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("Learn More - Clicked.");
            Vibration.Vibrate(5000);
        }
    }
}