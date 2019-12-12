using FormsControls.Base;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using WindesHeartApp.Data;
using WindesHeartApp.Data.Interfaces;
using WindesHeartApp.Pages;
using WindesHeartApp.Resources;
using Xamarin.Forms;

namespace WindesHeartApp
{
    public partial class App : Application
    {
        public App(IHeartrateRepository heartrateRepository, ISleepRepository sleepRepository, IStepsRepository stepsRepository, ISettingsRepository settingsRepository, DatabaseContext databaseContext)
        {
            InitializeComponent();
            Globals.BuildGlobals(heartrateRepository, sleepRepository, stepsRepository, settingsRepository, databaseContext);
            MainPage = new AnimationNavigationPage(new HomePage());
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public static async void RequestLocationPermission()
        {
            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (permissionStatus != PermissionStatus.Granted)
            {
                await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
            }
        }
    }
}
