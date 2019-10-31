using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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