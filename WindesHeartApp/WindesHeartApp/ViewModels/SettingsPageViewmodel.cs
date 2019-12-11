using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WindesHeartApp.Data.Interfaces;
using WindesHeartSDK;
using Xamarin.Forms;

namespace WindesHeartApp.ViewModels
{
    public class SettingsPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _toggle2 = true;

        public SettingsPageViewModel(ISettingsRepository settingsRepository)
        {
        }

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void DateIndexChanged(object sender, EventArgs args)
        {
            Picker picker = sender as Picker;
            if (picker.SelectedIndex != -1)
            {
                string format = picker.Items[picker.SelectedIndex];
                if (format.Equals("MM/DD/YYY"))
                {
                    Windesheart.ConnectedDevice?.SetDateDisplayFormat(false);
                }
                else
                {
                    Windesheart.ConnectedDevice?.SetDateDisplayFormat(true);
                }
            }
        }
        public void HourIndexChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            if (picker.SelectedIndex != -1)
            {
                string format = picker.Items[picker.SelectedIndex];
                if (format.Equals("24 hour"))
                {
                    Windesheart.ConnectedDevice?.SetTimeDisplayFormat(true);
                }
                else
                {
                    Windesheart.ConnectedDevice?.SetTimeDisplayFormat(false);
                }
            }
        }

        public void ToggleWristActivatedClicked(object sender, EventArgs args)
        {

            Windesheart.ConnectedDevice?.SetActivateOnLiftWrist(_toggle2);
            _toggle2 = !_toggle2;
        }
    }
}
