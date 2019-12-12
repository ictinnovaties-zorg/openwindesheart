using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

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
            get => AppSettings.GetValueOrDefault(nameof(DeviceLanguage), "en_EN");
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
    }
}
