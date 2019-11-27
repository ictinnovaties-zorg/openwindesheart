using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WindesHeartApp.ViewModels
{
    public class HeartRatePageViewModel : INotifyPropertyChanged
    {
        private int heartRate;
        private bool _isBusy = true;
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public int HeartRate
        {
            get { return heartRate; }
            set
            {
                heartRate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayHeartrateMessage));
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public string DisplayHeartrateMessage
        {
            get { return $"Your new heartbeat is: {HeartRate.ToString()} and this databinding shit is awesome."; }
        }
    }
}