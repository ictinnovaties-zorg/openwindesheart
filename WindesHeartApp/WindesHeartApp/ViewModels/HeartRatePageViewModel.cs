using Microcharts;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            Interval = 0;
            _dateTime2 = DateTime.Now;
            _dateTime = DateTime.Now.AddHours(-6);

            DayLabelText = $"{_dateTime.ToString()} - {_dateTime2.ToString()}";
            var rates = await _heartrateRepository.GetAllAsync();
            if (rates.Count() != 0)
            {
                _heartrates = rates;
                InitLabels();

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

        private void InitLabels()
        {
            var heartrateslast6hours = _heartrates.Where(x => x.DateTime >= _dateTime).Where(x => x.HeartrateValue != 0);
            if (heartrateslast6hours != null)
            {
                AverageHeartrate = Convert.ToInt32(heartrateslast6hours?.Select(x => x.HeartrateValue).Average());
                PeakHeartrate = Convert.ToInt32((heartrateslast6hours?.Select(x => x.HeartrateValue).Max()));
            }

        }

        public void PreviousDayBtnClick(object sender, EventArgs args)
        {
            _dateTime = _dateTime.AddHours(-6);
            _dateTime2 = _dateTime2.AddHours(-6);
            DayLabelText = $"{_dateTime.ToString()} - {_dateTime2.ToString()}";
            DrawChart();

        }

        public void NextDayBtnClick(object sender, EventArgs args)
        {
            if (!(_dateTime2.AddHours(6) > DateTime.Now))
            {
                _dateTime = _dateTime.AddHours(6);
                _dateTime2 = _dateTime2.AddHours(6);
                DayLabelText = $"{_dateTime.ToString()} - {_dateTime2.ToString()}";
                DrawChart();
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
                    heartrates = heartrates.Where((x, i) => i % Interval == 0);
                }

                List<Entry> list = new List<Entry>();
                int dateLabelInterval = 5;
                //switch (Interval)

                //{
                //    case 1:
                //        dateLabelInterval = 5;
                //        break;
                //    case 5:
                //        dateLabelInterval = 10;
                //        break;
                //    case 15:
                //        dateLabelInterval = 10;
                //        break;
                //    case 30:
                //        dateLabelInterval = 10;
                //        break;

                //}

                var labelcounter = 1;
                foreach (Heartrate heartrate in heartrates)
                {
                    Entry entry;
                    if (labelcounter % dateLabelInterval == 0)
                    {
                        entry = new Entry(heartrate.HeartrateValue)
                        {
                            ValueLabel = heartrate.HeartrateValue.ToString(),
                            Color = SKColors.Black,
                            Label = $"{heartrate.DateTime.Hour.ToString()}:{heartrate.DateTime.Minute.ToString()} ",
                            TextColor = SKColors.Black
                        };
                    }
                    else
                    {
                        entry = new Entry(heartrate.HeartrateValue)
                        {
                            ValueLabel = heartrate.HeartrateValue.ToString(),
                            Color = SKColors.Black,
                            Label = null
                        };
                    }

                    list.Add(entry);
                    labelcounter++;
                }

                Chart = new PointChart()
                {
                    Entries = list,
                    BackgroundColor = Globals.PrimaryColor.ToSKColor(),
                    LabelTextSize = 15,
                    MaxValue = 150,
                    MinValue = 30,
                    PointMode = PointMode.Square,
                    PointSize = 10
                };

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
        public string AverageLabelText => AverageHeartrate != 0 ? $"Average heartrate of the last 6 hours: {AverageHeartrate.ToString()}" : "";
        public string PeakHeartrateText => PeakHeartrate != 0 ? $"Peak heartrate of the last 6 hours: {PeakHeartrate.ToString()}" : "";
    }
}
