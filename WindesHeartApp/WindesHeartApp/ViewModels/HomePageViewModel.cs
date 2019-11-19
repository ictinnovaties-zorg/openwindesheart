using System.ComponentModel;
using System.Runtime.CompilerServices;

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
            get
            {
                if (HeartRate != 0)
                    return $"Your Last heartbeat was: {HeartRate.ToString()}";
                else
                {
                    return "";
                }
            }
        }
        public string DisplayBattery
        {

            get
            {
                if (Battery != 0)
                    return $"{Battery.ToString()}%";
                else
                {
                    return "";
                }
            }
        }
    }
}