using WindesHeartApp.Pages;
using WindesHeartApp.Resources;
using WindesHeartApp.Services;
using Xamarin.Forms;

namespace WindesHeartApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            DependencyService.Register<MockDataStore>();
            MainPage = new HomePage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
