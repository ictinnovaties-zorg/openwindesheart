using FormsControls.Base;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using WindesHeartApp.Data.Interfaces;
using WindesHeartApp.Models;
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
            Globals.StepsRepository.RemoveAll();
            await Globals.StepsRepository.AddAsync(new Step(new DateTime(2019, 12, 1), 4));
            await Globals.StepsRepository.AddAsync(new Step(new DateTime(2019, 12, 2), 5));
            await Globals.StepsRepository.AddAsync(new Step(new DateTime(2019, 12, 3), 6));
            await Globals.StepsRepository.AddAsync(new Step(new DateTime(2019, 11, 30), 7));
            await Globals.StepsRepository.AddAsync(new Step(new DateTime(2019, 11, 29), 8));
            await Globals.StepsRepository.AddAsync(new Step(new DateTime(2019, 11, 28), 9));
            await Globals.StepsRepository.AddAsync(new Step(new DateTime(2019, 11, 27), 10));
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
