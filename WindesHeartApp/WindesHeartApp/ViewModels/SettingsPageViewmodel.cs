using System.ComponentModel;
using System.Runtime.CompilerServices;
using WindesHeartApp.Data.Interfaces;
using WindesHeartSDK;
using Xamarin.Forms;

namespace WindesHeartApp.ViewModels
{
    public class SettingsPageViewmodel : INotifyPropertyChanged
    {
        public Command ToggleDisplayFormatsCommand { get; }
        public Command ToggleWristActivatedCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly ISettingsRepository _settingsRepository;
        private bool _toggle = true;
        private bool _toggle2 = true;

        public SettingsPageViewmodel(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
            ToggleDisplayFormatsCommand = new Command(ToggleDisplayFormatsClicked);
            ToggleWristActivatedCommand = new Command(ToggleWristActivatedClicked);
        }

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void ToggleDisplayFormatsClicked()
        {
            Windesheart.ConnectedDevice.SetTimeDisplayFormat(_toggle);
            Windesheart.ConnectedDevice.SetDateDisplayFormat(_toggle);
            _toggle = !_toggle;
        }

        private void ToggleWristActivatedClicked()
        {

            Windesheart.ConnectedDevice.SetActivateOnLiftWrist(_toggle2);
            _toggle2 = !_toggle2;
        }

    }
}
