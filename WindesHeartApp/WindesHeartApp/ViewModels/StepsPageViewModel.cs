using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using WindesHeartApp.Data.Interfaces;
using WindesHeartApp.Models;
using WindesHeartApp.Pages;
using Xamarin.Forms;
using Entry = Microcharts.Entry;

namespace WindesHeartApp.ViewModels
{
    public class StepsPageViewModel : INotifyPropertyChanged
    {
        public DateTime StartDate { get; }

        public DateTime SelectedDate;
        private readonly IStepsRepository _stepsRepository;

        public event PropertyChangedEventHandler PropertyChanged;

        public static IEnumerable<Step> StepInfo = new List<Step>();

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
            //Get all steps from DB
            StepInfo = await _stepsRepository.GetAllAsync();

            //Update chart
            UpdateChart();

            //Init buttons on bottom
            List<Button> dayButtons = new List<Button>
            {
                StepsPage.Day1Button,
                StepsPage.Day2Button,
                StepsPage.Day3Button,
                StepsPage.Day4Button,
                StepsPage.Day5Button,
                StepsPage.Day6Button,
                StepsPage.TodayButton
            };
            _buttonRow = new ButtonRow(dayButtons);

            //Switch to today
            TodayBtnClick(StepsPage.TodayButton, new EventArgs());
        }

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public StepsPageViewModel(IStepsRepository stepsRepository)
        {
            _stepsRepository = stepsRepository;
            StartDate = DateTime.Now;
            SelectedDate = StartDate;
        }


        private void UpdateInfo()
        {
            if (SelectedDate == StartDate)
            {
                StepsPage.CurrentDayLabel.Text = "Today";
            }
            else if (SelectedDate >= StartDate.AddDays(-6))
            {
                StepsPage.CurrentDayLabel.Text = SelectedDate.DayOfWeek.ToString();
            }
            else
            {
                StepsPage.CurrentDayLabel.Text = SelectedDate.ToString("dd/MM/yyyy");
            }

            //Update chart
            UpdateChart();
        }

        private int GetCurrentSteps()
        {
            List<Step> steps = StepInfo.Where(s => s.DateTime.Year == SelectedDate.Year &&
            s.DateTime.Month == SelectedDate.Month &&
            s.DateTime > SelectedDate.AddHours(-12) &&
            s.DateTime < SelectedDate.AddHours(12)).
            OrderBy(x => x.DateTime).ToList();

            //Get stepcount for that day by adding them together
            int stepCount = 0;
            steps.ForEach(x => stepCount += x.StepCount);
            return stepCount;
        }

        public void UpdateChart()
        {
            int stepCount = GetCurrentSteps();

            List<Entry> entries = new List<Entry>();

            float percentageDone = (float)stepCount / 2000;

            //Add part done
            entries.Add(new Entry(percentageDone) { Color = SKColors.Black });

            //Update labels
            StepsPage.CurrentStepsLabel.Text = stepCount.ToString();

            double kilometers = (double)stepCount / 1000;
            StepsPage.KilometersLabel.Text = Math.Floor(kilometers * 10) / 10 + " Kilometers";

            StepsPage.KcalLabel.Text = ((double)(stepCount / 20) / 1000) + " Kcal";

            //If goal not reached, fill other part transparent
            if (percentageDone < 1)
            {
                float percentageLeft = 1 - percentageDone;
                entries.Add(new Entry(percentageLeft) { Color = SKColors.Transparent });
            }

            Chart = new DonutChart
            {
                Entries = entries,
                BackgroundColor = SKColors.Transparent,
                HoleRadius = 0.7f
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
