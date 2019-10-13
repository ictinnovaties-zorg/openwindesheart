using Xamarin.Forms;
using WindesHeartApp.Services;
using WindesHeartApp.Pages;

namespace WindesHeartApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());

            DependencyService.Register<MockDataStore>();
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
