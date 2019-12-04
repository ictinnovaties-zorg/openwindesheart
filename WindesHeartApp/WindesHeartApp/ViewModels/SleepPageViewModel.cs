using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using WindesHeartApp.Data.Interfaces;
using WindesHeartApp.Models;
using WindesHeartApp.Views;
using Xamarin.Forms;
using Entry = Microcharts.Entry;

namespace WindesHeartApp.ViewModels
{
    public class SleepPageViewModel
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
            Sleep sleep = GetCurrentSleep();
            UpdateChart(sleep);
        }

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public SleepPageViewModel(ISleepRepository sleepRepository)
        {
            _sleepRepository = sleepRepository;
            StartDate = DateTime.Now;
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
            Sleep sleep = GetCurrentSleep();
            UpdateChart(sleep);
        }

        private Sleep GetCurrentSleep()
        {
            foreach (Sleep info in SleepInfo)
            {
                //If the same day
                if (info.DateTime.Year == SelectedDate.Year && info.DateTime.Month == SelectedDate.Month && info.DateTime.Day == SelectedDate.Day)
                {
                    //we found our info!
                    return info;
                }
            }
            return null;
        }

        public void UpdateChart(Sleep sleep)
        {
            List<Entry> entries = new List<Entry>();

            //Update labels
            SleepPage.CurrentSleepLabel.Text = "Hallo";

            Chart = new BarChart
            {
                Entries = entries,
                BackgroundColor = SKColors.Transparent
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
