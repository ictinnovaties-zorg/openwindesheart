using System;
using System.ComponentModel;
using WindesHeartApp.Models;
using WindesHeartApp.Pages;
using WindesHeartApp.Resources;
using WindesHeartSDK;
using Xamarin.Forms;

namespace WindesHeartApp.ViewModels
{
    public class SettingsPageViewModel : INotifyPropertyChanged
    {
        private int _languageIndex = 0;
        private int _hourIndex = 0;
        private int _dateIndex = 0;
        private int _stepIndex = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsPageViewModel()
        {
        }

        public void OnAppearing()
        {
            //Set correct settings
            if (DeviceSettings.TimeFormat24Hour) SettingsPage.HourPicker.SelectedIndex = 0;
            else SettingsPage.HourPicker.SelectedIndex = 1;

            if (DeviceSettings.DateFormatDMY) SettingsPage.DatePicker.SelectedIndex = 0;
            else SettingsPage.DatePicker.SelectedIndex = 1;

            SettingsPage.WristSwitch.IsToggled = DeviceSettings.WristRaiseDisplay;

            for (int i = 0; i < SettingsPage.StepsPicker.Items.Count; i++)
            {
                if (DeviceSettings.DailyStepsGoal.ToString().Equals(SettingsPage.StepsPicker.Items[i]))
                {
                    SettingsPage.StepsPicker.SelectedIndex = i;
                }
            }

            //Add languages
            int index = 0;
            foreach (string key in Globals.LanguageDictionary.Keys)
            {
                SettingsPage.LanguagePicker.Items.Add(key);

                //Set selected
                Globals.LanguageDictionary.TryGetValue(key, out string code);
                if (DeviceSettings.DeviceLanguage.Equals(code)) SettingsPage.LanguagePicker.SelectedIndex = index;

                index++;
            }

        }

        public void DateIndexChanged(object sender, EventArgs args)
        {
            Picker picker = sender as Picker;
            if (picker.SelectedIndex != -1)
            {
                string format = picker.Items[picker.SelectedIndex];
                bool isDMY = format.Equals("DD/MM/YYYY");

                try
                {
                    //Set on device
                    Windesheart.PairedDevice?.SetDateDisplayFormat(isDMY);
                    DeviceSettings.DateFormatDMY = isDMY;
                    _dateIndex = picker.SelectedIndex;
                }
                catch (Exception)
                {
                    //Set picker index back to old value
                    picker.SelectedIndex = _dateIndex;
                    Console.WriteLine("Something went wrong!");
                }
            }
        }

        public void HourIndexChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            if (picker.SelectedIndex != -1)
            {
                string format = picker.Items[picker.SelectedIndex];
                bool is24 = format.Equals("24 hour");

                try
                {
                    //Set on device
                    Windesheart.PairedDevice?.SetTimeDisplayFormat(is24);
                    DeviceSettings.TimeFormat24Hour = is24;
                    _hourIndex = picker.SelectedIndex;
                }
                catch (Exception)
                {
                    //Set picker index back to old value
                    picker.SelectedIndex = _hourIndex;
                    Console.WriteLine("Something went wrong!");
                }
            }
        }

        public void LanguageIndexChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            if (picker.SelectedIndex != -1)
            {
                //Get language code
                string language = picker.Items[picker.SelectedIndex];
                Globals.LanguageDictionary.TryGetValue(language, out string languageCode);

                try
                {
                    //Set on device
                    Windesheart.PairedDevice?.SetLanguage(languageCode);
                    DeviceSettings.DeviceLanguage = languageCode;
                    _languageIndex = picker.SelectedIndex;
                }
                catch (Exception)
                {
                    //Set picker index back to old value
                    picker.SelectedIndex = _languageIndex;
                    Console.WriteLine("Something went wrong!");
                }
            }
        }

        public void StepsIndexChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            if (picker.SelectedIndex != -1)
            {
                int steps = int.Parse(picker.Items[picker.SelectedIndex]);
                try
                {
                    //Set on device
                    Windesheart.PairedDevice?.SetStepGoal(steps);
                    DeviceSettings.DailyStepsGoal = steps;
                    _stepIndex = picker.SelectedIndex;
                }
                catch (Exception)
                {
                    //Set picker index back to old value
                    picker.SelectedIndex = _stepIndex;
                    Console.WriteLine("Something went wrong!");
                }
            }
        }

        public void OnWristToggled(object sender, ToggledEventArgs e)
        {
            Switch sw = sender as Switch;
            bool toggled = sw.IsToggled;

            try
            {
                Windesheart.PairedDevice?.SetActivateOnLiftWrist(toggled);
                DeviceSettings.WristRaiseDisplay = toggled;
            }
            catch (Exception)
            {
                //toggle back the switch
                SettingsPage.WristSwitch.IsToggled = !toggled;
                Console.WriteLine("Something went wrong!");
            }
        }
    }
}
