using FormsControls.Base;
using OpenWindesheartDemoApp.Data;
using OpenWindesheartDemoApp.Data.Repository;
using OpenWindesheartDemoApp.Resources;
using OpenWindesheartDemoApp.Views;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace OpenWindesheartDemoApp
{
    public partial class App
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
            //Handle when your app starts
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
