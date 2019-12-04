using Microcharts;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using WindesHeartApp.Data.Interfaces;
using WindesHeartApp.Resources;
using WindesHeartSDK.Models;
using Xamarin.Forms;
using Entry = Microcharts.Entry;

namespace WindesHeartApp.ViewModels
{
    public class HeartRatePageViewModel : INotifyPropertyChanged
    {
        private int _heartrate;
        private readonly IHeartrateRepository _heartrateRepository;
        private int _averageHeartrate;
        public int Interval;
        private int _peakHeartrate;
        public DateTime _dateTime;
        private Chart _chart;
        public event PropertyChangedEventHandler PropertyChanged;
        public Command NextDayBinding { get; set; }
        public Command PreviousDayBinding { get; set; }

        public HeartRatePageViewModel(IHeartrateRepository heartrateRepository)
        {
            _heartrateRepository = heartrateRepository;
            NextDayBinding = new Command(NextDayBtnClick);
            PreviousDayBinding = new Command(PreviousDayBtnClick);
        }

        private async void PreviousDayBtnClick(object obj)
        {
            _dateTime = _dateTime.AddHours(-24);
            var heartrates = await _heartrateRepository.HeartratesByQueryAsync(x => x.DateTime > _dateTime);
            var heartratesprevious24hours = from a in heartrates
                                            where a.DateTime < _dateTime.AddHours(24)
                                            select a;

            //checks for null data

            foreach (Heartrate heartrate in heartratesprevious24hours)
            {
                List<Entry> list = new List<Entry>();
                var entry = new Entry(heartrate.HeartrateValue);
                entry.ValueLabel = heartrate.HeartrateValue.ToString();
                entry.Color = Globals.PrimaryColor.ToSKColor();
                list.Add(entry);

                Chart = new PointChart()
                {
                    Entries = list
                };

            }
        }

        private async void NextDayBtnClick(object obj)
        {
            //checks for null data
            _dateTime = _dateTime.AddHours(24);
            var heartrates = await _heartrateRepository.HeartratesByQueryAsync(x => x.DateTime > _dateTime);
            var heartratesprevious24hours = from a in heartrates
                                            where a.DateTime < _dateTime.AddHours(24)
                                            select a;

            foreach (Heartrate heartrate in heartratesprevious24hours)
            {
                List<Entry> list = new List<Entry>();
                var entry = new Entry(heartrate.HeartrateValue);
                entry.ValueLabel = heartrate.HeartrateValue.ToString();
                entry.Color = Globals.PrimaryColor.ToSKColor();
                list.Add(entry);

                Chart = new PointChart()
                {
                    Entries = list
                };

            }
        }

        public async void InitLabels()
        {
            var heartrateslast24horus = await _heartrateRepository.HeartratesByQueryAsync(x => x.DateTime >= DateTime.Now.AddHours(-24));
            if (heartrateslast24horus.Count() != 0)
            {
                AverageHeartrate = Convert.ToInt32(heartrateslast24horus?.Select(x => x.HeartrateValue).Average());
                PeakHeartrate = Convert.ToInt32((heartrateslast24horus?.Select(x => x.HeartrateValue).Max()));
            }

        }

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public async void InitChart()
        {
            for (int i = 0; i < 20; i++)
            {
                Heartrate rate = new Heartrate(new byte[0xFF]);
                Random random2 = new Random();
                var lol = random2.Next(24);
                DateTime date = DateTime.Today.AddHours(lol);
                rate.DateTime = date;
                var random = new Random();
                var next = random.Next(100);
                rate.HeartrateValue = Convert.ToInt32(next);

            }

            var heartrates =
                    await _heartrateRepository.HeartratesByQueryAsync(x => x.DateTime == DateTime.Now.AddHours(-24));
            List<Entry> list = new List<Entry>();

            foreach (Heartrate heartrate in heartrates)
            {
                var entry = new Entry(heartrate.HeartrateValue);
                entry.ValueLabel = heartrate.HeartrateValue.ToString();
                entry.Color = Globals.PrimaryColor.ToSKColor();
                list.Add(entry);

                Chart = new PointChart()
                {
                    Entries = list
                };

            }
        }

        public Chart Chart
        {
            get => _chart;
            set
            {
                _chart = value;
                OnPropertyChanged();
            }
        }

        public int Heartrate
        {
            get => _heartrate;
            set
            {
                _heartrate = value;
                OnPropertyChanged();
            }
        }

        public int AverageHeartrate
        {
            get => _averageHeartrate;
            set
            {
                _averageHeartrate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AverageLabelText));
            }
        }

        public int PeakHeartrate
        {
            get => _peakHeartrate;
            set
            {
                _peakHeartrate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PeakHeartrateText));
            }
        }

        public string AverageLabelText => AverageHeartrate != 0 ? $"Average heartrate of last 12 hours: {AverageHeartrate.ToString()}" : "";

        public string PeakHeartrateText => PeakHeartrate != 0 ? $"Your peak heartrate of the last 12 hours: {PeakHeartrate.ToString()}" : "";

        public async void UpdateInterval(int interval)
        {
            Interval = interval;
            var heartrates = await _heartrateRepository.GetHeartRatesAsync();
            var list = heartrates.ToList();
            var count = list.Count;
            var result = new List<Entry>();
            for (int i = 0; i < count; i++)
            {
                if ((i % Interval) == 0)
                {
                    var heartrate = list[i];
                    result.Add(new Entry(heartrate.HeartrateValue)
                    {
                        TextColor = Globals.PrimaryColor.ToSKColor(),
                        ValueLabel = heartrate.HeartrateValue.ToString()
                    });
                }
            }

            Chart = new PointChart()
            {
                Entries = result
            };
        }



    }
}