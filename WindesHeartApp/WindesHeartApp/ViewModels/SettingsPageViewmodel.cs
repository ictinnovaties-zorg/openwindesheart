using System.ComponentModel;
using System.Runtime.CompilerServices;
using WindesHeartApp.Data.Interfaces;
using WindesHeartSDK;
using Xamarin.Forms;

namespace WindesHeartApp.ViewModels
{
    public class SettingsPageViewmodel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly ISettingsRepository _settingsRepository;
        public bool toggle = true;
        public bool toggle2 = true;
        public Command ToggleDisplayFormatsCommand { get; }
        public Command ToggleWristActivatedCommand { get; }

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
            Windesheart.ConnectedDevice.SetTimeDisplayFormat(toggle);
            Windesheart.ConnectedDevice.SetDateDisplayFormat(toggle);
            toggle = !toggle;
        }

        private void ToggleWristActivatedClicked()
        {

            Windesheart.ConnectedDevice.SetActivateOnLiftWrist(toggle2);
            toggle2 = !toggle2;
        }

    }
}
