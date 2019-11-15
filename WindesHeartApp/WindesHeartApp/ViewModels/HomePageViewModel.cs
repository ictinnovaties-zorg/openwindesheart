using System.ComponentModel;
using System.Runtime.CompilerServices;
using WindesHeartApp.Resources;

namespace WindesHeartApp.ViewModels
{
    public class HomePageViewModel : INotifyPropertyChanged
    {
        private int battery;
        private int heartRate;
        private string batteryImage = "";
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public HomePageViewModel()
        {
            HeartRate = Globals.heartRate;
            Battery = Globals.batteryPercentage;
            BatteryImage = Globals.batteryImage;
        }

        public string BatteryImage
        {
            get { return batteryImage; }
            set
            {
                batteryImage = value;
                OnPropertyChanged();
            }
        }

        public int Battery
        {
            get { return battery; }
            set
            {
                battery = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayBattery));
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
        public string DisplayBattery
        {
            get { return $"{Battery.ToString()}%"; }
        }
    }
}