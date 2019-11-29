using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using WindesHeartApp.Data.Interfaces;
using WindesHeartSDK.Models;
using Xamarin.Forms;

namespace WindesHeartApp.ViewModels
{
    public class HeartRatePageViewModel : INotifyPropertyChanged
    {
        private int _heartrate;
        private int counter;
        private readonly IHeartrateRepository _heartrateRepository;
        public Command getButtonCommand { get; }
        public Command addButtonCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        public HeartRatePageViewModel(IHeartrateRepository heartrateRepository)
        {
            _heartrateRepository = heartrateRepository;
            getButtonCommand = new Command(getButtonClicked);
            addButtonCommand = new Command(addButtonClicked);
            counter = 44;
        }

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public int HeartRate
        {
            get { return _heartrate; }
            set
            {
                _heartrate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(swagLabel));
            }
        }

        public string swagLabel
        {
            get
            {
                return
                    $"Your heartrate coming from the database is : {HeartRate.ToString()}, and this DB binding is pretty cool.";
            }

        }

        public async void addButtonClicked()
        {
            Heartrate heartrate = new Heartrate();
            heartrate.HeartrateValue = counter;
            var lol = await _heartrateRepository.AddHeartrateAsync(heartrate);
            counter++;

        }
        public async void getButtonClicked()
        {
            IEnumerable<Heartrate> heartrates = await _heartrateRepository.GetHeartRatesAsync();
            var lastadded = heartrates.Last();
            HeartRate = lastadded.HeartrateValue;
        }
    }
}