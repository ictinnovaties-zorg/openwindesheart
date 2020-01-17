using OpenWindesheart;
using OpenWindesheartDemoApp.Models;
using OpenWindesheartDemoApp.Resources;
using OpenWindesheartDemoApp.Views;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Switch = Xamarin.Forms.Switch;

namespace OpenWindesheartDemoApp.ViewModels
{
    public class SettingsPageViewModel
    {
        private int _languageIndex = 0;
        private int _hourIndex = 0;
        private int _dateIndex = 0;
        private int _stepIndex = 0;

        public void OnAppearing()
        {
            //Set correct settings
            SettingsPage.HourPicker.SelectedIndex = DeviceSettings.TimeFormat24Hour ? 0 : 1;

            SettingsPage.DatePicker.SelectedIndex = DeviceSettings.DateFormatDMY ? 0 : 1;

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
            if (!(sender is Picker picker) || picker.SelectedIndex == -1) return;
            string format = picker.Items[picker.SelectedIndex];
            bool isDMY = format.Equals("DD/MM/YYYY");

            try
            {
                if (Windesheart.PairedDevice != null)
                {
                    if (Windesheart.PairedDevice.IsConnected())
                    {
                        Windesheart.PairedDevice.SetDateDisplayFormat(isDMY);
                        DeviceSettings.DateFormatDMY = isDMY;
                        _dateIndex = picker.SelectedIndex;
                    }
                }
            }
            catch (Exception)
            {
                //Set picker index back to old value
                picker.SelectedIndex = _dateIndex;
                Debug.WriteLine("Something went wrong!");
            }
        }

        public void HourIndexChanged(object sender, EventArgs e)
        {
            if (!(sender is Picker picker) || picker.SelectedIndex == -1) return;
            string format = picker.Items[picker.SelectedIndex];
            bool is24 = format.Equals("24 hour");

            try
            {
                if (Windesheart.PairedDevice != null)
                {
                    if (Windesheart.PairedDevice.IsConnected())
                    {
                        Windesheart.PairedDevice.SetTimeDisplayFormat(is24);
                        DeviceSettings.TimeFormat24Hour = is24;
                        _hourIndex = picker.SelectedIndex;
                    }
                }
            }
            catch (Exception)
            {
                //Set picker index back to old value
                picker.SelectedIndex = _hourIndex;
                Debug.WriteLine("Something went wrong!");
            }
        }

        public void LanguageIndexChanged(object sender, EventArgs e)
        {
            if (!(sender is Picker picker) || picker.SelectedIndex == -1) return;
            //Get language code
            string language = picker.Items[picker.SelectedIndex];
            Globals.LanguageDictionary.TryGetValue(language, out string languageCode);

            try
            {
                if (Windesheart.PairedDevice != null)
                {
                    if (Windesheart.PairedDevice.IsConnected())
                    {
                        Windesheart.PairedDevice.SetLanguage(languageCode);
                        DeviceSettings.DeviceLanguage = languageCode;
                        _languageIndex = picker.SelectedIndex;
                    }
                }
            }
            catch (Exception)
            {
                //Set picker index back to old value
                picker.SelectedIndex = _languageIndex;
                Debug.WriteLine("Something went wrong!");
            }
        }

        public void StepsIndexChanged(object sender, EventArgs e)
        {
            if (!(sender is Picker picker) || picker.SelectedIndex == -1) return;
            int steps = int.Parse(picker.Items[picker.SelectedIndex]);
            try
            {
                if (Windesheart.PairedDevice != null)
                {
                    if (Windesheart.PairedDevice.IsConnected())
                    {
                        Windesheart.PairedDevice.SetStepGoal(steps);
                        DeviceSettings.DailyStepsGoal = steps;
                        _stepIndex = picker.SelectedIndex;
                    }
                }
            }
            catch (Exception)
            {
                //Set picker index back to old value
                picker.SelectedIndex = _stepIndex;
                Debug.WriteLine("Something went wrong!");
            }
        }

        public void OnWristToggled(object sender, ToggledEventArgs e)
        {
            Switch sw = sender as Switch;
            bool toggled = sw.IsToggled;

            try
            {
                if (Windesheart.PairedDevice == null) return;
                if (!Windesheart.PairedDevice.IsConnected()) return;
                Windesheart.PairedDevice.SetActivateOnLiftWrist(toggled);
                DeviceSettings.WristRaiseDisplay = toggled;
            }
            catch (Exception)
            {
                //toggle back the switch
                SettingsPage.WristSwitch.IsToggled = !toggled;
                Debug.WriteLine("Something went wrong!");
            }
        }
    }
}
