using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using WindesHeartApp.Data.Interfaces;
using WindesHeartApp.Models;
using WindesHeartApp.Views;
using Xamarin.Forms;
using Entry = Microcharts.Entry;

namespace WindesHeartApp.ViewModels
{
    public class SleepPageViewModel : INotifyPropertyChanged
    {
        public DateTime StartDate { get; }

        public DateTime SelectedDate;
        private ISleepRepository _sleepRepository;

        public event PropertyChangedEventHandler PropertyChanged;

        public static IEnumerable<Sleep> SleepInfo = new List<Sleep>();

        public string AwakeColor = "#ffffff";
        public string LightColor = "#1281ff";
        public string DeepColor = "#002bba";

        private ButtonRow _buttonRow;

        private Chart _chart;

        public Chart Chart
        {
            get => _chart;
            set
            {
                _chart = value;
                OnPropertyChanged();
            }
        }

        public async void OnAppearing()
        {
            //Get all sleep data from DB
            SleepInfo = await _sleepRepository.GetAllAsync();

            if(SleepInfo.Count() == 0)
            {
                Device.BeginInvokeOnMainThread(async delegate
                {
                    await Application.Current.MainPage.DisplayAlert("No data", "Unfortunately, no sleep-data was found.", "Ok");
                });
            }
            //Update chart
            UpdateChart();

            //Init buttons on bottom
            List<Button> dayButtons = new List<Button>
            {
                SleepPage.Day1Button,
                SleepPage.Day2Button,
                SleepPage.Day3Button,
                SleepPage.Day4Button,
                SleepPage.Day5Button,
                SleepPage.Day6Button,
                SleepPage.TodayButton
            };
            _buttonRow = new ButtonRow(dayButtons);

            //Switch to today
            TodayBtnClick(SleepPage.TodayButton, new EventArgs());
        }

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public SleepPageViewModel(ISleepRepository sleepRepository)
        {
            _sleepRepository = sleepRepository;
            StartDate = DateTime.Today;
            SelectedDate = StartDate;
        }

        private void UpdateInfo()
        {
            if (SelectedDate == StartDate)
            {
                SleepPage.CurrentDayLabel.Text = "Today";
            }
            else if (SelectedDate >= StartDate.AddDays(-6))
            {
                SleepPage.CurrentDayLabel.Text = SelectedDate.DayOfWeek.ToString();
            }
            else
            {
                SleepPage.CurrentDayLabel.Text = SelectedDate.ToString("dd/MM/yyyy");
            }


            //Update chart
            UpdateChart();
        }

        private List<Sleep> GetCurrentSleep()
        {
            return SleepInfo.Where(s => s.DateTime.Year == SelectedDate.Year &&
            s.DateTime.Month == SelectedDate.Month &&
            s.DateTime > SelectedDate.AddHours(-4) &&
            s.DateTime < SelectedDate.AddHours(12)).
            OrderBy(x => x.DateTime).ToList();
        }

        public void UpdateChart()
        {
            List<Sleep> sleepData = GetCurrentSleep();
            List<Entry> entries = new List<Entry>();

            //For each hour
            for (int i = 20; i < 36; i++)
            {
                int hour = i;
                if (i >= 24) hour -= 24;

                //Get sleep data for that hour
                List<Sleep> data = sleepData.Where(x => x.DateTime.Hour == hour).ToList();

                for(int j = 0; j < 60; j++)
                {
                    if(data.ElementAtOrDefault(j) != null)
                    {
                        switch (data[j].SleepType)
                        {
                            case SleepType.Awake:
                                Entry awakeEntry = new Entry(1);
                                awakeEntry.Color = SKColor.Parse(AwakeColor);
                                entries.Add(awakeEntry);
                                break;
                            case SleepType.Light:
                                Entry lightEntry = new Entry(1);
                                lightEntry.Color = SKColor.Parse(LightColor);
                                entries.Add(lightEntry);
                                break;
                            case SleepType.Deep:
                                Entry deepEntry = new Entry(1);
                                deepEntry.Color = SKColor.Parse(DeepColor);
                                entries.Add(deepEntry);
                                break;
                        }
                    } else
                    {
                        Entry entry = new Entry(1);
                        entry.Color = SKColor.Parse(AwakeColor);
                        entries.Add(entry);
                    }
                }
            }

            Chart = new BarChart
            {
                Entries = entries,
                BackgroundColor = SKColors.Transparent,
                Margin = 0
            };
        }

        public void PreviousDayBtnClick(object sender, EventArgs args)
        {
            Console.WriteLine("Previous day clicked!");

            //You can always go back
            _buttonRow.ToPrevious();
            SelectedDate = SelectedDate.AddDays(-1);
            UpdateInfo();
        }

        public void NextDayBtnClick(object sender, EventArgs args)
        {
            //If already today, you cant go next
            if (_buttonRow.ToNext())
            {
                SelectedDate = SelectedDate.AddDays(1);
                UpdateInfo();
            }
        }

        public void TodayBtnClick(object sender, EventArgs args)
        {
            SelectedDate = StartDate;
            if (_buttonRow.SwitchTo(sender as Button))
            {
                UpdateInfo();
            }
        }

        public void Day6BtnClick(object sender, EventArgs args)
        {
            SelectedDate = StartDate.AddDays(-1);
            if (_buttonRow.SwitchTo(sender as Button))
            {
                UpdateInfo();
            }
        }

        public void Day5BtnClick(object sender, EventArgs args)
        {
            SelectedDate = StartDate.AddDays(-2);
            if (_buttonRow.SwitchTo(sender as Button))
            {
                UpdateInfo();
            }
        }

        public void Day4BtnClick(object sender, EventArgs args)
        {
            SelectedDate = StartDate.AddDays(-3);
            if (_buttonRow.SwitchTo(sender as Button))
            {
                UpdateInfo();
            }
        }

        public void Day3BtnClick(object sender, EventArgs args)
        {
            SelectedDate = StartDate.AddDays(-4);
            if (_buttonRow.SwitchTo(sender as Button))
            {
                UpdateInfo();
            }
        }

        public void Day2BtnClick(object sender, EventArgs args)
        {
            SelectedDate = StartDate.AddDays(-5);
            if (_buttonRow.SwitchTo(sender as Button))
            {
                UpdateInfo();
            }
        }

        public void Day1BtnClick(object sender, EventArgs args)
        {
            SelectedDate = StartDate.AddDays(-6);
            if (_buttonRow.SwitchTo(sender as Button))
            {
                UpdateInfo();
            }
        }
    }
}
