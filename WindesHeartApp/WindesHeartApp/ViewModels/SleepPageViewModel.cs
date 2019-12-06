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

        public Command NextDayBinding { get; }
        public Command PreviousDayBinding { get; }
        public Command Day1Binding { get; }
        public Command Day2Binding { get; }
        public Command Day3Binding { get; }
        public Command Day4Binding { get; }
        public Command Day5Binding { get; }
        public Command Day6Binding { get; }
        public Command TodayBinding { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public static IEnumerable<Sleep> SleepInfo = new List<Sleep>();

        public string AwakeColor = "#ffffff";
        public string LightColor = "#1281ff";
        public string DeepColor = "#002bba";

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

            //Update chart
            UpdateChart();
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

            NextDayBinding = new Command(NextDayBtnClick);
            PreviousDayBinding = new Command(PreviousDayBtnClick);
            Day1Binding = new Command(Day1BtnClick);
            Day2Binding = new Command(Day2BtnClick);
            Day3Binding = new Command(Day3BtnClick);
            Day4Binding = new Command(Day4BtnClick);
            Day5Binding = new Command(Day5BtnClick);
            Day6Binding = new Command(Day6BtnClick);
            TodayBinding = new Command(TodayBtnClick);
        }

        private void PreviousDayBtnClick(object obj)
        {
            Console.WriteLine("Previous day clicked!");
            SelectedDate = SelectedDate.AddDays(-1);
            Console.WriteLine(SelectedDate);

            if (!SleepPage.Day1Button.IsEnabled)
            {
                SleepPage.Day1Button.IsEnabled = true;
            }
            else if (!SleepPage.Day2Button.IsEnabled)
            {
                SleepPage.Day1Button.IsEnabled = false;
                SleepPage.Day2Button.IsEnabled = true;
            }
            else if (!SleepPage.Day3Button.IsEnabled)
            {
                SleepPage.Day2Button.IsEnabled = false;
                SleepPage.Day3Button.IsEnabled = true;
            }
            else if (!SleepPage.Day4Button.IsEnabled)
            {
                SleepPage.Day3Button.IsEnabled = false;
                SleepPage.Day4Button.IsEnabled = true;
            }
            else if (!SleepPage.Day5Button.IsEnabled)
            {
                SleepPage.Day4Button.IsEnabled = false;
                SleepPage.Day5Button.IsEnabled = true;
            }
            else if (!SleepPage.Day6Button.IsEnabled)
            {
                SleepPage.Day5Button.IsEnabled = false;
                SleepPage.Day6Button.IsEnabled = true;
            }
            else if (!SleepPage.TodayButton.IsEnabled)
            {
                SleepPage.Day6Button.IsEnabled = false;
                SleepPage.TodayButton.IsEnabled = true;
            }
            UpdateDay();
        }

        private void UpdateDay()
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
            s.DateTime > SelectedDate.AddHours(-12) &&
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

                //If there is sleepdata for that hour, add that
                if (data != null && data.Count > 0)
                {
                    //Set Right color for entry according to sleep type
                    foreach (Sleep s in data)
                    {
                        switch (s.SleepType)
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
                    }

                }
                else
                {
                    //If no sleep data, add awake entry
                    Entry entry = new Entry(1);
                    entry.Color = SKColor.Parse(AwakeColor);
                    entries.Add(entry);
                }
            }

            Chart = new BarChart
            {
                Entries = entries,
                BackgroundColor = SKColors.Transparent,
                Margin = 0
            };
        }

        private void NextDayBtnClick(object obj)
        {
            //Dont go in the future
            if (SelectedDate < StartDate)
            {
                SelectedDate = SelectedDate.AddDays(1);
            }
            Console.WriteLine(SelectedDate);

            //Set right day button selected
            if (!SleepPage.Day1Button.IsEnabled)
            {
                SleepPage.Day2Button.IsEnabled = false;
                SleepPage.Day1Button.IsEnabled = true;
            }
            else if (!SleepPage.Day2Button.IsEnabled)
            {
                SleepPage.Day3Button.IsEnabled = false;
                SleepPage.Day2Button.IsEnabled = true;
            }
            else if (!SleepPage.Day3Button.IsEnabled)
            {
                SleepPage.Day4Button.IsEnabled = false;
                SleepPage.Day3Button.IsEnabled = true;
            }
            else if (!SleepPage.Day4Button.IsEnabled)
            {
                SleepPage.Day5Button.IsEnabled = false;
                SleepPage.Day4Button.IsEnabled = true;
            }
            else if (!SleepPage.Day5Button.IsEnabled)
            {
                SleepPage.Day6Button.IsEnabled = false;
                SleepPage.Day5Button.IsEnabled = true;
            }
            else if (!SleepPage.Day6Button.IsEnabled)
            {
                SleepPage.TodayButton.IsEnabled = false;
                SleepPage.Day6Button.IsEnabled = true;
            }
            else if (!SleepPage.TodayButton.IsEnabled)
            {
                SleepPage.TodayButton.IsEnabled = false;
            }
            else
            {
                //If SelectedDate is at 6 days back again
                if (SelectedDate == StartDate.AddDays(-6))
                {
                    SleepPage.Day1Button.IsEnabled = false;
                }
            }
            UpdateDay();
        }

        private void TodayBtnClick(object obj)
        {
            SelectedDate = StartDate;
            SetDayEnabled(SleepPage.TodayButton);
            UpdateDay();
        }

        private void Day6BtnClick(object obj)
        {
            SelectedDate = StartDate.AddDays(-1);
            SetDayEnabled(SleepPage.Day6Button);
            UpdateDay();
        }

        private void Day5BtnClick(object obj)
        {
            SelectedDate = StartDate.AddDays(-2);
            SetDayEnabled(SleepPage.Day5Button);
            UpdateDay();
        }

        private void Day4BtnClick(object obj)
        {
            SelectedDate = StartDate.AddDays(-3);
            SetDayEnabled(SleepPage.Day4Button);
            UpdateDay();
        }

        private void Day3BtnClick(object obj)
        {
            SelectedDate = StartDate.AddDays(-4);
            SetDayEnabled(SleepPage.Day3Button);
            UpdateDay();
        }

        private void SetDayEnabled(Button button)
        {
            //First, enable all buttons
            SleepPage.Day1Button.IsEnabled = true;
            SleepPage.Day2Button.IsEnabled = true;
            SleepPage.Day3Button.IsEnabled = true;
            SleepPage.Day4Button.IsEnabled = true;
            SleepPage.Day5Button.IsEnabled = true;
            SleepPage.Day6Button.IsEnabled = true;
            SleepPage.TodayButton.IsEnabled = true;

            //Then disable the selected Day
            button.IsEnabled = false;
        }

        private void Day2BtnClick(object obj)
        {
            SelectedDate = StartDate.AddDays(-5);
            SetDayEnabled(SleepPage.Day2Button);
            UpdateDay();
        }

        private void Day1BtnClick(object obj)
        {
            SelectedDate = StartDate.AddDays(-6);
            SetDayEnabled(SleepPage.Day1Button);
            UpdateDay();
        }
    }
}
