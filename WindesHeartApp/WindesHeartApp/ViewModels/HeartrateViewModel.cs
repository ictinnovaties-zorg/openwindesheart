using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WindesHeartApp.Resources;
using Xamarin.Forms;

namespace WindesHeartApp.ViewModels
{
    public class HeartrateViewModel : INotifyPropertyChanged
    {
        public HeartrateViewModel()
        {
            buttonClickedCommand = new Command(async () => await SaveSomething());
            HeartRate = Globals.heartRate;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public Command buttonClickedCommand { get; }

        private bool bestFriend;
        private int heartRate;
        private bool isBusy;
        public bool BestFriend
        {
            get { return bestFriend; }
            set { bestFriend = value; }
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

        public string DisplayHeartrateMessage
        {
            get { return $"Your new heartbeat is: {HeartRate.ToString()} and this databinding shit is awesome."; }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        async Task SaveSomething()
        {
            IsBusy = true;
            IsBusy = false;
            HeartRate++;
            Globals.heartRate = HeartRate;
        }


    }
}