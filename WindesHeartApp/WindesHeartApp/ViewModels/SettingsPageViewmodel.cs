using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WindesHeartApp.Data.Interfaces;
using WindesHeartSDK;

namespace WindesHeartApp.ViewModels
{
    public class SettingsPageViewmodel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly ISettingsRepository _settingsRepository;
        public bool toggle = true;
        public bool toggle2 = true;

        public SettingsPageViewmodel(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void ToggleDisplayFormatsClicked(object sender, EventArgs args)
        {
            Windesheart.ConnectedDevice.SetTimeDisplayFormat(toggle);
            Windesheart.ConnectedDevice.SetDateDisplayFormat(toggle);
            toggle = !toggle;
        }

        public void ToggleWristActivatedClicked(object sender, EventArgs args)
        {

            Windesheart.ConnectedDevice.SetActivateOnLiftWrist(toggle2);
            toggle2 = !toggle2;
        }

    }
}
