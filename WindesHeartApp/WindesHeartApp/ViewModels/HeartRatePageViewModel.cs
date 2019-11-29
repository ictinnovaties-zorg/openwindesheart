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
        public Command GetButtonCommand { get; }
        public Command AddButtonCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        public HeartRatePageViewModel(IHeartrateRepository heartrateRepository)
        {
            _heartrateRepository = heartrateRepository;
            GetButtonCommand = new Command(GetButtonClicked);
            AddButtonCommand = new Command(AddButtonClicked);
            counter = 44;
        }

        public void OnPropertyChanged([CallerMemberName] string name = "")
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
                OnPropertyChanged(nameof(SwagLabel));
            }
        }

        public string SwagLabel
        {
            get
            {
                return
                    $"Your heartrate coming from the database is : {HeartRate.ToString()}, and this DB binding is pretty cool.";
            }

        }

        public async void AddButtonClicked()
        {
            Heartrate heartrate = new Heartrate() { HeartrateValue = counter };
            var lol = await _heartrateRepository.AddHeartrateAsync(heartrate);
            counter++;

        }
        public async void GetButtonClicked()
        {
            IEnumerable<Heartrate> heartrates = await _heartrateRepository.GetHeartRatesAsync();
            var lastadded = heartrates.Last();
            HeartRate = lastadded.HeartrateValue;
        }
    }
}