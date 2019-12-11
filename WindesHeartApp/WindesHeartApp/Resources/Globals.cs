using System.Collections.Generic;
using WindesHeartApp.Data.Interfaces;
using WindesHeartApp.Services;
using WindesHeartApp.ViewModels;
using Xamarin.Forms;

namespace WindesHeartApp.Resources
{
    public static class Globals
    {
        public static HeartratePageViewModel HeartratePageViewModel;
        public static SamplesService SamplesService { get; private set; }
        public static DevicePageViewModel DevicePageViewModel;
        public static HomePageViewModel HomePageViewModel;
        public static SettingsPageViewModel SettingsPageViewModel;
        public static StepsPageViewModel StepsViewModel;
        public static SleepPageViewModel SleepPageViewModel;
        public static double ScreenHeight { get; set; }
        public static double ScreenWidth { get; set; }
        public static Color PrimaryColor { get; set; } = Color.FromHex("#96d1ff");
        public static Color SecondaryColor { get; set; } = Color.FromHex("#53b1ff");
        public static Color LightTextColor { get; set; } = Color.FromHex("#999999");
        public static IStepsRepository StepsRepository { get; set; }
        public static ISleepRepository SleepRepository { get; set; }
        public static ISettingsRepository SettingsRepository { get; set; }
        public static IHeartrateRepository HeartrateRepository { get; set; }
        public static int DailyStepsGoal { get; internal set; }
        public static Dictionary<string, string> languageDictionary;


        public static void BuildGlobals(IHeartrateRepository heartrateRepository, ISleepRepository sleepRepository, IStepsRepository stepsRepository, ISettingsRepository settingsRepository)
        {
            DailyStepsGoal = 10000;
            StepsRepository = stepsRepository;
            SleepRepository = sleepRepository;
            SettingsRepository = settingsRepository;
            HeartrateRepository = heartrateRepository;
            HeartratePageViewModel = new HeartratePageViewModel(HeartrateRepository);
            SamplesService = new SamplesService(HeartrateRepository, StepsRepository, SleepRepository);
            StepsViewModel = new StepsPageViewModel(StepsRepository);
            SettingsPageViewModel = new SettingsPageViewModel(SettingsRepository);
            SleepPageViewModel = new SleepPageViewModel(sleepRepository);
            DevicePageViewModel = new DevicePageViewModel();
            HomePageViewModel = new HomePageViewModel();

            languageDictionary = new Dictionary<string, string>
            {
                {"Nederlands", "nl-NL"},
                {"English", "en-EN"},
                {"Deutsch", "du-DU"}
            };
        }
    }
}