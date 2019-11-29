using System.ComponentModel;
using System.Runtime.CompilerServices;
using WindesHeartApp.Data.Interfaces;

namespace WindesHeartApp.ViewModels
{
    public class SettingsPageViewmodel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly ISettingsRepository _settingsRepository;
        public SettingsPageViewmodel(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}