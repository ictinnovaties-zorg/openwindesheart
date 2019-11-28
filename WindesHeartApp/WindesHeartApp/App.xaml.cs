using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using WindesHeartApp.Pages;
using WindesHeartApp.Resources;
using WindesHeartSDK.Models;
using Xamarin.Forms;

namespace WindesHeartApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new HomePage());
        }

        protected override void OnStart()
        {
            Globals.BuildGlobals();
            FillDatabase();
        }

        private async void FillDatabase()
        {
            await Globals.HeartrateRepository.AddHeartrateAsync(new Heartrate(new DateTime(2019, 11, 25), new byte[] { 1, 2, 3, 4 }));
            await Globals.StepsRepository.AddStepInfoAsync(new StepInfo(new DateTime(2019, 11, 25), new byte[] { 1, 2, 3, 4 }));
            await Globals.StepsRepository.AddStepInfoAsync(new StepInfo(new DateTime(2019, 11, 26), new byte[] { 1, 2, 3, 4 }));
            await Globals.StepsRepository.AddStepInfoAsync(new StepInfo(new DateTime(2019, 11, 27), new byte[] { 2, 1, 3, 4 }));
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
