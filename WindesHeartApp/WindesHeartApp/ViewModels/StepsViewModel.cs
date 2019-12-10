using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using WindesHeartApp.Data.Interfaces;
using WindesHeartApp.Models;
using WindesHeartApp.Pages;
using Xamarin.Forms;
using Entry = Microcharts.Entry;

namespace WindesHeartApp.ViewModels
{
    public class StepsViewModel : INotifyPropertyChanged
    {
        public DateTime StartDate { get; }

        public DateTime SelectedDate;
        private readonly IStepsRepository _stepsRepository;

        public event PropertyChangedEventHandler PropertyChanged;

        public static IEnumerable<Step> StepInfo = new List<Step>();

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
            Step steps = GetCurrentSteps();
            if (steps != null) UpdateChart(steps.StepCount);
            else UpdateChart(0);
        }

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public StepsViewModel(IStepsRepository stepsRepository)
        {
            _stepsRepository = stepsRepository;
            StartDate = DateTime.Now;
            SelectedDate = StartDate;
        }

        public void PreviousDayBtnClick(object sender, EventArgs args)
        {
            Debug.WriteLine("Previous day clicked!");
            SelectedDate = SelectedDate.AddDays(-1);
            Debug.WriteLine(SelectedDate);

            if (!StepsPage.Day1Button.IsEnabled)
            {
                StepsPage.Day1Button.IsEnabled = true;
            }
            else if (!StepsPage.Day2Button.IsEnabled)
            {
                StepsPage.Day1Button.IsEnabled = false;
                StepsPage.Day2Button.IsEnabled = true;
            }
            else if (!StepsPage.Day3Button.IsEnabled)
            {
                StepsPage.Day2Button.IsEnabled = false;
                StepsPage.Day3Button.IsEnabled = true;
            }
            else if (!StepsPage.Day4Button.IsEnabled)
            {
                StepsPage.Day3Button.IsEnabled = false;
                StepsPage.Day4Button.IsEnabled = true;
            }
            else if (!StepsPage.Day5Button.IsEnabled)
            {
                StepsPage.Day4Button.IsEnabled = false;
                StepsPage.Day5Button.IsEnabled = true;
            }
            else if (!StepsPage.Day6Button.IsEnabled)
            {
                StepsPage.Day5Button.IsEnabled = false;
                StepsPage.Day6Button.IsEnabled = true;
            }
            else if (!StepsPage.TodayButton.IsEnabled)
            {
                StepsPage.Day6Button.IsEnabled = false;
                StepsPage.TodayButton.IsEnabled = true;
            }
            UpdateDay();
        }

        private void UpdateDay()
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
            Step steps = GetCurrentSteps();
            if (steps != null) UpdateChart(steps.StepCount);
            else UpdateChart(0);
        }

        private Step GetCurrentSteps()
        {
            if (StepInfo != null)
            {
                foreach (Step info in StepInfo)
                {
                    //If the same day
                    if (info.DateTime.Year == SelectedDate.Year && info.DateTime.Month == SelectedDate.Month &&
                        info.DateTime.Day == SelectedDate.Day)
                    {
                        //we found our info!
                        return info;
                    }
                }

                return null;
            }

            return null;
        }

        public void UpdateChart(int stepCount)
        {
            if (stepCount != null)
            {
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
        }

        public void NextDayBtnClick(object sender, EventArgs args)
        {
            //Dont go in the future
            if (SelectedDate < StartDate)
            {
                SelectedDate = SelectedDate.AddDays(1);
            }
            Debug.WriteLine(SelectedDate);

            //Set right day button selected
            if (!StepsPage.Day1Button.IsEnabled)
            {
                StepsPage.Day2Button.IsEnabled = false;
                StepsPage.Day1Button.IsEnabled = true;
            }
            else if (!StepsPage.Day2Button.IsEnabled)
            {
                StepsPage.Day3Button.IsEnabled = false;
                StepsPage.Day2Button.IsEnabled = true;
            }
            else if (!StepsPage.Day3Button.IsEnabled)
            {
                StepsPage.Day4Button.IsEnabled = false;
                StepsPage.Day3Button.IsEnabled = true;
            }
            else if (!StepsPage.Day4Button.IsEnabled)
            {
                StepsPage.Day5Button.IsEnabled = false;
                StepsPage.Day4Button.IsEnabled = true;
            }
            else if (!StepsPage.Day5Button.IsEnabled)
            {
                StepsPage.Day6Button.IsEnabled = false;
                StepsPage.Day5Button.IsEnabled = true;
            }
            else if (!StepsPage.Day6Button.IsEnabled)
            {
                StepsPage.TodayButton.IsEnabled = false;
                StepsPage.Day6Button.IsEnabled = true;
            }
            else if (!StepsPage.TodayButton.IsEnabled)
            {
                StepsPage.TodayButton.IsEnabled = false;
            }
            else
            {
                //If SelectedDate is at 6 days back again
                if (SelectedDate == StartDate.AddDays(-6))
                {
                    StepsPage.Day1Button.IsEnabled = false;
                }
            }
            UpdateDay();
        }

        public void TodayBtnClick(object sender, EventArgs args)
        {
            SelectedDate = StartDate;
            SetDayEnabled(StepsPage.TodayButton);
            UpdateDay();
        }

        public void Day6BtnClick(object sender, EventArgs args)
        {
            SelectedDate = StartDate.AddDays(-1);
            SetDayEnabled(StepsPage.Day6Button);
            UpdateDay();
        }

        public void Day5BtnClick(object sender, EventArgs args)
        {
            SelectedDate = StartDate.AddDays(-2);
            SetDayEnabled(StepsPage.Day5Button);
            UpdateDay();
        }

        public void Day4BtnClick(object sender, EventArgs args)
        {
            SelectedDate = StartDate.AddDays(-3);
            SetDayEnabled(StepsPage.Day4Button);
            UpdateDay();
        }

        public void Day3BtnClick(object sender, EventArgs args)
        {
            SelectedDate = StartDate.AddDays(-4);
            SetDayEnabled(StepsPage.Day3Button);
            UpdateDay();
        }

        private void SetDayEnabled(Button button)
        {
            //First, enable all buttons
            StepsPage.Day1Button.IsEnabled = true;
            StepsPage.Day2Button.IsEnabled = true;
            StepsPage.Day3Button.IsEnabled = true;
            StepsPage.Day4Button.IsEnabled = true;
            StepsPage.Day5Button.IsEnabled = true;
            StepsPage.Day6Button.IsEnabled = true;
            StepsPage.TodayButton.IsEnabled = true;

            //Then disable the selected Day
            button.IsEnabled = false;
        }

        public void Day2BtnClick(object sender, EventArgs args)
        {
            SelectedDate = StartDate.AddDays(-5);
            SetDayEnabled(StepsPage.Day2Button);
            UpdateDay();
        }

        public void Day1BtnClick(object sender, EventArgs args)
        {
            SelectedDate = StartDate.AddDays(-6);
            SetDayEnabled(StepsPage.Day1Button);
            UpdateDay();
        }
    }
}
