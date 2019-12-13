using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WindesHeartApp.Data.Interfaces;
using WindesHeartSDK;

namespace WindesHeartApp.ViewModels
{
    public class SettingsPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly ISettingsRepository _settingsRepository;
        private bool _toggle;
        private bool _toggle2;

        public SettingsPageViewModel(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void ToggleDisplayFormatsClicked(object sender, EventArgs args)
        {
            Windesheart.ConnectedDevice.SetTimeDisplayFormat(_toggle);
            Windesheart.ConnectedDevice.SetDateDisplayFormat(_toggle);
            _toggle = !_toggle;
        }

        public void ToggleWristActivatedClicked(object sender, EventArgs args)
        {
            Windesheart.ConnectedDevice.SetActivateOnLiftWrist(_toggle2);
            _toggle2 = !_toggle2;
        }

    }
}
