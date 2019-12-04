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
            await Globals.StepsRepository.AddAsync(new Step(DateTime.Today, 700));
            await Globals.StepsRepository.AddAsync(new Step(DateTime.Today.AddDays(-1), 480));
            await Globals.StepsRepository.AddAsync(new Step(DateTime.Today.AddDays(-2), 1200));
            await Globals.StepsRepository.AddAsync(new Step(DateTime.Today.AddDays(-3), 3500));
            await Globals.StepsRepository.AddAsync(new Step(DateTime.Today.AddDays(-4), 1300));
            await Globals.StepsRepository.AddAsync(new Step(DateTime.Today.AddDays(-5), 2200));
            await Globals.StepsRepository.AddAsync(new Step(DateTime.Today.AddDays(-6), 1400));
            await Globals.StepsRepository.AddAsync(new Step(DateTime.Today.AddDays(-7), 8310));
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
