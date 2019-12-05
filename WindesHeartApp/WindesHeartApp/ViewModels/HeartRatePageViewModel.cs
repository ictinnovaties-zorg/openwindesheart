using Microcharts;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
        public event PropertyChangedEventHandler PropertyChanged;
        public Command NextDayBinding { get; set; }
        public Command PreviousDayBinding { get; set; }

        public HeartRatePageViewModel(IHeartrateRepository heartrateRepository)
        {
            _heartrateRepository = heartrateRepository;
            NextDayBinding = new Command(NextDayBtnClick);
            PreviousDayBinding = new Command(PreviousDayBtnClick);
        }

        private async void PreviousDayBtnClick()
        {
            _dateTime = _dateTime.AddHours(-12);
            var heartrates = await _heartrateRepository.HeartratesByQueryAsync(x => x.DateTime >= _dateTime);
            if (heartrates != null)
            {
                var heartratesprevious24hours = heartrates.Where(a => a.DateTime <= _dateTime.AddHours(12));
                //checks for null data

                foreach (Heartrate heartrate in heartratesprevious24hours)
                {
                    List<Entry> list = new List<Entry>();
                    var entry = new Entry(heartrate.HeartrateValue) { ValueLabel = heartrate.HeartrateValue.ToString(), Color = SKColors.Black, Label = $"{heartrate.DateTime.ToString(CultureInfo.InvariantCulture)} ", TextColor = SKColors.Black };
                    list.Add(entry);

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
        }

        private async void NextDayBtnClick()
        {
            //checks for null data
            _dateTime = _dateTime.AddHours(12);
            var heartrates = await _heartrateRepository.HeartratesByQueryAsync(x => x.DateTime >= _dateTime);
            if (heartrates != null)
            {
                var heartratesnext24hours = heartrates.Where(a => a.DateTime <= _dateTime.AddHours(12));

                foreach (Heartrate heartrate in heartratesnext24hours)
                {
                    List<Entry> list = new List<Entry>();
                    var entry = new Entry(heartrate.HeartrateValue) { ValueLabel = heartrate.HeartrateValue.ToString(), Color = SKColors.Black, Label = $"{heartrate.DateTime.ToString(CultureInfo.InvariantCulture)}  ", TextColor = SKColors.Black };
                    ;
                    list.Add(entry);

                    Chart = new PointChart() { Entries = list, BackgroundColor = Globals.PrimaryColor.ToSKColor(), LabelTextSize = 15, MaxValue = 150, MinValue = 30, PointMode = PointMode.Square, PointSize = 10 };

                }
            }
        }

        public async Task InitLabels()
        {
            var heartrateslast24horus = await _heartrateRepository.HeartratesByQueryAsync(x => x.DateTime >= DateTime.Now.AddHours(-24));
            if (heartrateslast24horus != null)
            {
                AverageHeartrate = Convert.ToInt32(heartrateslast24horus?.Select(x => x.HeartrateValue).Average());
                PeakHeartrate = Convert.ToInt32((heartrateslast24horus?.Select(x => x.HeartrateValue).Max()));
            }

        }

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public async Task InitChart()
        {
            _dateTime = DateTime.Now.AddHours(-12);
            var heartrates =
                await _heartrateRepository.HeartratesByQueryAsync(x => x.DateTime >= _dateTime);
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
            var heartrates = await _heartrateRepository.GetAllAsync();
            var list = heartrates.ToList();
            if (list.Count != 0)
            {
                var count = list.Count;
                var result = new List<Entry>();
                for (int i = 0; i < count; i++)
                {
                    if ((i % Interval) == 0)
                    {
                        var heartrate = list[i];
                        result.Add(new Entry(heartrate.HeartrateValue)
                        {
                            ValueLabel = heartrate.HeartrateValue.ToString(),
                            Color = SKColors.Black,
                            Label = $"{heartrate.DateTime.ToString(CultureInfo.InvariantCulture)} ",
                            TextColor = SKColors.Black
                        });
                    }
                }

                Chart = new PointChart() { Entries = result, BackgroundColor = Globals.PrimaryColor.ToSKColor(), LabelTextSize = 15, MaxValue = 150, MinValue = 30, PointMode = PointMode.Square, PointSize = 10 };
            }
        }
    }



}
