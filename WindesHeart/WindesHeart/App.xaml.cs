using System;
using WindesHeartSdk.Services;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using WindesHeart.MiBand;
using WindesHeart.Services;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace WindesHeart
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();
            WindesHeart.Configure("LoginPage", typeof(Pages.LoginPage));
            WindesHeart.Configure("MainPage", typeof(Pages.MainPage));
            WindesHeart.Configure("PairDevicePage", typeof(Pages.PairDevicePage));
            WindesHeart.Configure("Visualize", typeof(Pages.Visualize));
            WindesHeart.Configure("WebViewer", typeof(Pages.WebViewer));

            if (Current.Properties.ContainsKey("device_id"))
            {
                BleService.KnownDeviceId = (Guid)Current.Properties["device_id"];
            }
            
            MiBandDevice = new MiBandDevice();

            var mainPage = ((ViewNavigationService) WindesHeart).SetRootPage("LoginPage");
            MainPage = mainPage;
        }

        public static INavigationService WindesHeart { get; } = new ViewNavigationService();

        public static MiBandDevice MiBandDevice { get; set; }

        protected override void OnStart()
        {
            AppCenter.Start("android=0e213655-6f5c-429e-a62c-0b4b4fc8b0fc;" +
                            "ios={e1f9db72-6cd1-4a07-95eb-a3986577b0d1}",
                typeof(Analytics), typeof(Crashes));

            MiBandDevice.Connect();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            if (MiBandDevice.LastSyncTime != null && MiBandDevice.LastSyncTime < DateTime.Now.AddMinutes(-5))
            {
                Console.WriteLine("Start Fetching Data");
                MiBandDevice.StartFetching();
            }
        }
    }
}