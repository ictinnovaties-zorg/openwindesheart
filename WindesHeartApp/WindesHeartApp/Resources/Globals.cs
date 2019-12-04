using System;
using System.Collections.Generic;
using WindesHeartApp.Data.Interfaces;
using WindesHeartApp.Services;
using WindesHeartApp.ViewModels;
using Xamarin.Forms;

namespace WindesHeartApp.Resources
{
    public static class Globals
    {
        public static HeartRatePageViewModel heartrateviewModel;
        public static SamplesService SamplesService { get; private set; }

        public static DevicePageViewModel DevicePageViewModel;
        public static HomePageViewModel homepageviewModel;
        public static SettingsPageViewmodel settingspageviewModel;
        public static double ScreenHeight { get; set; }
        public static double ScreenWidth { get; set; }
        public static Color PrimaryColor { get; set; } = Color.FromHex("#96d1ff");
        public static Color SecondaryColor { get; set; } = Color.FromHex("#53b1ff");
        public static Color LightTextColor { get; set; } = Color.FromHex("#999999");
        public static IStepsRepository StepsRepository { get; set; }
        public static float DailyStepsGoal { get; internal set; }

        public static Dictionary<string, Color> ColorDictionary;

        public static StepsViewModel StepsViewModel;

        public static void BuildGlobals(IHeartrateRepository heartrateRepository, ISleepRepository sleepRepository, IStepsRepository stepsRepository, ISettingsRepository settingsRepository)
        {
            DailyStepsGoal = 1000;

            StepsRepository = stepsRepository;

            heartrateviewModel = new HeartRatePageViewModel(heartrateRepository);
            SamplesService = new SamplesService(heartrateRepository, StepsRepository, sleepRepository);
            StepsViewModel = new StepsViewModel(stepsRepository);
            settingspageviewModel = new SettingsPageViewmodel(settingsRepository);
            DevicePageViewModel = new DevicePageViewModel();
            homepageviewModel = new HomePageViewModel();
            ColorDictionary = new Dictionary<string, Color>
            {
                { "Aqua", Color.Aqua},
                { "Black", Color.Black},
                { "LightBlue (Default)", Color.FromHex("#96d1ff")}, { "Fucshia", Color.Fuchsia },
                { "Gray", Color.Gray }, { "Green", Color.Green },
                { "Lime", Color.Lime }, { "Maroon", Color.Maroon },
                { "Navy", Color.Navy }, { "Olive", Color.Olive },
                { "Purple", Color.Purple }, { "Red", Color.Red },
                { "Silver", Color.Silver }, { "Teal", Color.Teal },
                { "White", Color.White }, { "Yellow", Color.Yellow }
            };
        }

        public static bool SaveDeviceInAppProperties(Guid guid)
        {
            if (guid != Guid.Empty)
            {
                if (!App.Current.Properties.ContainsKey("LastConnectedDeviceGuid"))
                {
                    App.Current.Properties.Add("LastConnectedDeviceGuid", guid);
                }
                else
                {
                    App.Current.Properties.Remove("LastConnectedDeviceGuid");
                    App.Current.Properties.Add("LastConnectedDeviceGuid", guid);
                }

                return true;
            }

            return false;
        }
    }
}