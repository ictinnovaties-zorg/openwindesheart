using System.ComponentModel;
using System.Runtime.CompilerServices;
using WindesHeartApp.Data.Interfaces;

namespace WindesHeartApp.ViewModels
{
    public class HeartRatePageViewModel : INotifyPropertyChanged
    {
        private int _heartrate;
        private readonly IHeartrateRepository _heartrateRepository;
        private int _averageHeartrate;
        private int _peakHeartrate;
        public event PropertyChangedEventHandler PropertyChanged;

        public HeartRatePageViewModel(IHeartrateRepository heartrateRepository)
        {
            _heartrateRepository = heartrateRepository;
        }

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public int AverageHeartrate
        {
            get { return _averageHeartrate; }
            set
            {
                _averageHeartrate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AverageHeartrate));
            }
        }

        public int PeakHeartrate
        {
            get { return _peakHeartrate; }
            set
            {
                _peakHeartrate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PeakHeartrateText));
            }
        }

        public string AverageLabelText
        {
            get { return $"Average heartrate of last 12 hours: {AverageHeartrate.ToString()}"; }
        }

        public string PeakHeartrateText
        {
            get { return $"Your peak heartrate of the last 12 hours: {PeakHeartrate.ToString()}"; }
        }

    }
}