using FormsControls.Base;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using WindesHeartApp.Data.Interfaces;
using WindesHeartApp.Data.Models;
using WindesHeartApp.Pages;
using WindesHeartApp.Resources;
using WindesHeartSDK.Models;
using Xamarin.Forms;

namespace WindesHeartApp
{
    public partial class App : Application
    {
        public App(IHeartrateRepository heartrateRepository, ISleepRepository sleepRepository, IStepsRepository stepsRepository, ISettingsRepository settingsRepository)
        {
            InitializeComponent();
            Globals.BuildGlobals(heartrateRepository, sleepRepository, stepsRepository, settingsRepository);
            MainPage = new AnimationNavigationPage(new HomePage());
            FillDatabase();
        }

        protected override void OnStart()
        {
           
        }

        private async void FillDatabase()
        {
            Globals.StepsRepository.RemoveSteps();
            await Globals.StepsRepository.AddStepsAsync(new StepsModel(new DateTime(2019, 12, 1), new byte[] { 4, 2, 6, 4 }));
            await Globals.StepsRepository.AddStepsAsync(new StepsModel(new DateTime(2019, 12, 2), new byte[] { 1, 2, 3, 9 }));
            await Globals.StepsRepository.AddStepsAsync(new StepsModel(new DateTime(2019, 12, 3), new byte[] { 2, 7, 3, 6 }));
            await Globals.StepsRepository.AddStepsAsync(new StepsModel(new DateTime(2019, 11, 30), new byte[] { 8, 6, 5, 5 }));
            await Globals.StepsRepository.AddStepsAsync(new StepsModel(new DateTime(2019, 11, 29), new byte[] { 0, 6, 3, 3 }));
            await Globals.StepsRepository.AddStepsAsync(new StepsModel(new DateTime(2019, 11, 28), new byte[] { 3, 6, 8, 2 }));
            await Globals.StepsRepository.AddStepsAsync(new StepsModel(new DateTime(2019, 11, 27), new byte[] { 2, 6, 6, 8 }));
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
