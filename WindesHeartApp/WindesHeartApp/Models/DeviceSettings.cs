using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace WindesHeartApp.Models
{
    public class DeviceSettings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public static bool WristRaiseDisplay
        {
            get => AppSettings.GetValueOrDefault(nameof(WristRaiseDisplay), false);
            set => AppSettings.AddOrUpdateValue(nameof(WristRaiseDisplay), value);
        }

        public static string DeviceLanguage
        {
            get => AppSettings.GetValueOrDefault(nameof(DeviceLanguage), "en-EN");
            set => AppSettings.AddOrUpdateValue(nameof(DeviceLanguage), value);
        }

        public static bool DateFormatDMY
        {
            get => AppSettings.GetValueOrDefault(nameof(DateFormatDMY), true);
            set => AppSettings.AddOrUpdateValue(nameof(DateFormatDMY), value);
        }

        public static bool TimeFormat24Hour
        {
            get => AppSettings.GetValueOrDefault(nameof(TimeFormat24Hour), true);
            set => AppSettings.AddOrUpdateValue(nameof(TimeFormat24Hour), value);
        }

        public static int DailyStepsGoal
        {
            get => AppSettings.GetValueOrDefault(nameof(DailyStepsGoal), 10000);
            set => AppSettings.AddOrUpdateValue(nameof(DailyStepsGoal), value);
        }
    }
}
