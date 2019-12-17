using Microcharts;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        private readonly IHeartrateRepository _heartrateRepository;
        private int _averageHeartrate;
        private int _peakHeartrate;
        public DateTime _dateTime;
        private Chart _chart;
        private IEnumerable<Heartrate> _heartrates;
        private string _daylabelText;
        private DateTime _dateTime2;
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public HeartRatePageViewModel(IHeartrateRepository heartrateRepository)
        {
            _heartrateRepository = heartrateRepository;
        }

        public async void OnAppearing()
        {
            Interval = 5;
            _dateTime2 = DateTime.Now;
            _dateTime = DateTime.Now.AddHours(-1);

            DayLabelText = $"{_dateTime.ToString()} - {_dateTime2.ToString()}";
            var rates = _heartrateRepository.GetAll();
            if (rates.Count() != 0)
            {
                _heartrates = rates;
                DrawLabels();

            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Heartrates", "Unfortunately, no heartrate data was found.", "Ok");
            }
            DrawChart();
        }

        public string DayLabelText
        {
            get { return _daylabelText; }
            set
            {
                _daylabelText = value;
                OnPropertyChanged();
            }
        }

        private void DrawLabels()
        {
            var heartrates = _heartrates
                .Where(x => x.DateTime >= _dateTime)
                .Where(x => x.DateTime <= _dateTime2)
                .Where(x => x.HeartrateValue != 0);
            if (heartrates.Count() != 0)
            {
                AverageHeartrate = Convert.ToInt32(heartrates?.Select(x => x.HeartrateValue).Average());
                PeakHeartrate = Convert.ToInt32((heartrates?.Select(x => x.HeartrateValue).Max()));
            }

        }

        public void PreviousDayBtnClick(object sender, EventArgs args)
        {
            _dateTime = _dateTime.AddHours(-1);
            _dateTime2 = _dateTime2.AddHours(-1);
            DayLabelText = $"{_dateTime.ToString()} - {_dateTime2.ToString()}";
            DrawChart();
            DrawLabels();


        }

        public void NextDayBtnClick(object sender, EventArgs args)
        {
            if (!(_dateTime2.AddHours(1) > DateTime.Now))
            {
                _dateTime = _dateTime.AddHours(1);
                _dateTime2 = _dateTime2.AddHours(1);
                DayLabelText = $"{_dateTime.ToString()} - {_dateTime2.ToString()}";
                DrawChart();
                DrawLabels();
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Heartrates", "Unfortunately, you can't foresee the future.", "Ok");
            }
        }

        private void DrawChart()
        {
            if (_heartrates != null)
            {
                var heartrates = _heartrates
                    .Where(x => x.DateTime >= _dateTime)
                    .Where(x => x.DateTime <= _dateTime2);

                if (Interval != 0 && Interval != 1)
                {
                    heartrates = heartrates
                        .Where(x => x.HeartrateValue != 0)
                        .Where((x, i) => i % Interval == 0);
                }

                List<Entry> list = new List<Entry>();
                foreach (Heartrate heartrate in heartrates)
                {
                    Entry entry;
                    entry = new Entry(heartrate.HeartrateValue)
                    {
                        ValueLabel = heartrate.HeartrateValue.ToString(),
                        Color = SKColors.Black,
                        Label = Interval != 1 ? $"{heartrate.DateTime:HH:mm}" : "",
                        TextColor = SKColors.Black
                    };

                    list.Add(entry);
                }

                if (Interval == 1)
                {
                    Chart = new PointChart()
                    {
                        Entries = list,
                        BackgroundColor = Globals.PrimaryColor.ToSKColor(),
                        PointMode = PointMode.Circle,
                        PointSize = 10,
                        MinValue = 40,
                        MaxValue = 180
                    };
                }
                else
                {
                    Chart = new LineChart()
                    {
                        Entries = list,
                        LineMode = LineMode.Spline,
                        BackgroundColor = Globals.PrimaryColor.ToSKColor(),
                        PointMode = PointMode.Circle,
                        PointSize = 10,
                        LabelTextSize = 20,
                        MinValue = 40,
                        MaxValue = 180
                    };
                }
            }
        }

        public void UpdateInterval(int interval)
        {
            Interval = interval;
            DrawChart();
        }

        public int Interval { get; set; }

        public Chart Chart
        {
            get => _chart;
            set
            {
                _chart = value;
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
        public string AverageLabelText => AverageHeartrate != 0 ? $"Average heartrate: {AverageHeartrate.ToString()}(Failed measurements ignored)" : "";
        public string PeakHeartrateText => PeakHeartrate != 0 ? $"Peak heartrate: {PeakHeartrate.ToString()}" : "";
    }
}
