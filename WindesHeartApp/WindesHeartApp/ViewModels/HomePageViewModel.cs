using System.ComponentModel;
using System.Runtime.CompilerServices;
using WindesHeartApp.Resources;

namespace WindesHeartApp.ViewModels
{
    public class HomePageViewModel : INotifyPropertyChanged
    {
        public HomePageViewModel()
        {
            HeartRate = Globals.heartRate;
            Battery = Globals.batteryPercentage;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private int battery;
        private int heartRate;

        public int Battery
        {
            get { return battery; }
            set
            {
                battery = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayBatteryLevel));
            }
        }

        public int HeartRate
        {
            get { return heartRate; }
            set
            {
                heartRate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayHeartRate));
            }
        }

        public string DisplayHeartRate
        {
            get { return $"Your Last heartbeat was: {HeartRate.ToString()}"; }
        }

        public string DisplayBatteryLevel
        {
            get { return $"Your Batterylevel is: {Battery.ToString()}"; }
        }
    }
}