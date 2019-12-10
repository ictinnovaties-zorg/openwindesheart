using Microcharts;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using WindesHeartApp.Data.Interfaces;
using WindesHeartApp.Models;
using WindesHeartApp.Resources;
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
        private IEnumerable<Heartrate> _heartrates;
        public event PropertyChangedEventHandler PropertyChanged;
        public Command NextDayBinding { get; set; }
        public Command PreviousDayBinding { get; set; }

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public HeartRatePageViewModel(IHeartrateRepository heartrateRepository)
        {
            _heartrateRepository = heartrateRepository;
            NextDayBinding = new Command(NextDayBtnClick);
            PreviousDayBinding = new Command(PreviousDayBtnClick);
        }

        public async void OnAppearing()
        {
            _dateTime = DateTime.Now.AddHours(-6);
            var rates = await _heartrateRepository.GetAllAsync();
            _heartrates = rates;
            InitLabels();
            InitChart();
        }

        private void InitLabels()
        {
            var heartrateslast6hours = _heartrates.Where(x => x.DateTime >= _dateTime);
            if (heartrateslast6hours != null)
            {
                AverageHeartrate = Convert.ToInt32(heartrateslast6hours?.Select(x => x.HeartrateValue).Average());
                PeakHeartrate = Convert.ToInt32((heartrateslast6hours?.Select(x => x.HeartrateValue).Max()));
            }

        }

        private void PreviousDayBtnClick()
        {
            _dateTime = _dateTime.AddHours(-6);
            var heartrates = _heartrates.Where(x => x.DateTime >= _dateTime);
            heartrates = heartrates.Where(x => x.DateTime <= _dateTime.AddHours(6));
            List<Entry> list = new List<Entry>();

            if (heartrates.Count() != 0)
            {
                foreach (Heartrate heartrate in heartrates)
                {
                    var entry = new Entry(heartrate.HeartrateValue)
                    {
                        ValueLabel = heartrate.HeartrateValue.ToString(),
                        Color = SKColors.Black,
                        Label = $"{heartrate.DateTime.ToString(CultureInfo.InvariantCulture)} ",
                        TextColor = SKColors.Black
                    };
                    list.Add(entry);
                    Chart = new PointChart() { Entries = list, BackgroundColor = Globals.PrimaryColor.ToSKColor(), LabelTextSize = 15, MaxValue = 150, MinValue = 30, PointMode = PointMode.Square, PointSize = 10 };

                }
            }
        }

        private void NextDayBtnClick()
        {
            _dateTime = _dateTime.AddHours(6);
            var heartrates = _heartrates.Where(x => x.DateTime >= _dateTime);
            heartrates = heartrates.Where(x => x.DateTime <= _dateTime.AddHours(6));
            List<Entry> list = new List<Entry>();

            if (heartrates.Count() != 0)
            {
                foreach (Heartrate heartrate in heartrates)
                {
                    var entry = new Entry(heartrate.HeartrateValue)
                    {
                        ValueLabel = heartrate.HeartrateValue.ToString(),
                        Color = SKColors.Black,
                        Label = $"{heartrate.DateTime.ToString(CultureInfo.InvariantCulture)} ",
                        TextColor = SKColors.Black
                    };
                    list.Add(entry);
                    Chart = new PointChart() { Entries = list, BackgroundColor = Globals.PrimaryColor.ToSKColor(), LabelTextSize = 15, MaxValue = 150, MinValue = 30, PointMode = PointMode.Square, PointSize = 10 };

                }
            }
        }

        private void InitChart()
        {
            var heartrates = _heartrates.Where(x => x.DateTime >= _dateTime);
            List<Entry> list = new List<Entry>();

            if (heartrates != null)
            {
                foreach (Heartrate heartrate in heartrates)
                {
                    var entry = new Entry(heartrate.HeartrateValue)
                    {
                        ValueLabel = heartrate.HeartrateValue.ToString(),
                        Color = SKColors.Black,
                        Label = $"{heartrate.DateTime.ToString(CultureInfo.InvariantCulture)} ",
                        TextColor = SKColors.Black
                    };
                    list.Add(entry);
                    Chart = new PointChart() { Entries = list, BackgroundColor = Globals.PrimaryColor.ToSKColor(), LabelTextSize = 15, MaxValue = 150, MinValue = 30, PointMode = PointMode.Square, PointSize = 10 };

                }
            }
        }

        public void UpdateInterval(int interval)
        {
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
    }
}
