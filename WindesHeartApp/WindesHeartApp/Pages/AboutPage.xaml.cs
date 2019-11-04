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
            NavigationPage.SetHasNavigationBar(this, false);
            ImageButton returnButton = new ImageButton();
            returnButton.Source = "GoBack.png";
            returnButton.BackgroundColor = Color.Transparent; ;
            returnButton.CornerRadius = (int)(Globals.screenWidth / 20);
            returnButton.Clicked += returnButton_Clicked;
            returnButton.WidthRequest = Globals.screenHeight / 10;
            returnButton.HeightRequest = Globals.screenHeight / 10;
            layout.Children.Add(returnButton);
        }

        private void returnButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void Logo_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("Logo - Clicked.");
            Navigation.PushModalAsync(new HomePage());
        }

        private void LearnMore_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("Learn More - Clicked.");
            Vibration.Vibrate(5000);
        }
    }
}