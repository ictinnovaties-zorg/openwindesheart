using FormsControls.Base;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using WindesHeartApp.Data;
using WindesHeartApp.Data.Repository;
using WindesHeartApp.Resources;
using WindesHeartApp.Views;
using Xamarin.Forms;

namespace WindesHeartApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var database = new Database();
            Globals.BuildGlobals(new HeartrateRepository(database), new SleepRepository(database), new StepsRepository(database), database);
            //database.EmptyDatabase();
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

        public void CreateDatabase()
        {

        }
    }
}
