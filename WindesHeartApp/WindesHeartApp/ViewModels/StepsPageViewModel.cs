using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WindesHeartApp.Models;
using WindesHeartApp.Pages;
using WindesHeartApp.Resources;
using WindesHeartSDK;
using WindesHeartSDK.Models;
using Xamarin.Forms;
using Entry = Microcharts.Entry;

namespace WindesHeartApp.ViewModels
{
    public class StepsPageViewModel : INotifyPropertyChanged
    {
        public DateTime StartDate { get; }

        public DateTime SelectedDate;

        public event PropertyChangedEventHandler PropertyChanged;

        public static IEnumerable<Step> StepInfo = new List<Step>();

        public int TodayStepCount = 0;

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
            StepInfo = Globals.StepsRepository.GetAll();

            //Update chart
            int stepCount = await GetCurrentSteps();
            
            UpdateChart(stepCount);
            

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

        public StepsPageViewModel()
        {
            StartDate = DateTime.Today;
            SelectedDate = StartDate;
        }


        private async void UpdateInfo()
        {
            DateTime date = SelectedDate;
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
            int stepCount = await GetCurrentSteps();

            //Only update chart if selectedDate is still equal to the date requested (can be different due to await)
            if (SelectedDate.Equals(date))
            {
                UpdateChart(stepCount);
            }
        }

        public void OnStepsUpdated(int steps)
        {
            Debug.WriteLine("Steps viewmodel method!");

            //If looking at today
            if (SelectedDate.Equals(DateTime.Today))
            {
                //Update the chart on main thread
                Device.BeginInvokeOnMainThread(() =>
                {
                    //Update that info
                    Debug.WriteLine("Updating chart!!");
                    UpdateChart(steps);
                });
            }
        }

        private async Task<int> GetCurrentSteps()
        {
            //If looking at today
            if (SelectedDate.Equals(DateTime.Today))
            {
                Console.WriteLine("Today selected!");

                //If device is connected
                if (Windesheart.PairedDevice != null && Windesheart.PairedDevice.IsAuthenticated())
                {
                    //Read stepcount from device
                    try
                    {
                        StepData currentSteps = await Windesheart.PairedDevice.GetSteps();
                        return currentSteps.StepCount;
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Error while trying to get stepcount: " + e.Message);
                        Debug.WriteLine("Falling back to data from DB...");
                    }
                }
            }


            List<Step> steps = StepInfo.Where(s => s.DateTime.Year == SelectedDate.Year &&
            s.DateTime.Month == SelectedDate.Month &&
            s.DateTime.Day == SelectedDate.Day)
            .OrderBy(x => x.DateTime).ToList();

            //Get stepcount for that day by adding them together
            int stepCount = 0;

            if (SelectedDate == StartDate && Windesheart.PairedDevice != null)
            {
                var todaySteps = await Windesheart.PairedDevice.GetSteps();
                return todaySteps.StepCount;
            }

            steps.ForEach(x => stepCount += x.StepCount);
            return stepCount;
        }

        public async void UpdateChart(float stepCount)
        {
            List<Entry> entries = new List<Entry>();

            float percentageDone = stepCount / DeviceSettings.DailyStepsGoal;

            //Add part done
            entries.Add(new Entry(percentageDone) { Color = SKColors.Black });
            double kilometers = (double)stepCount / 1000;

            if (StepsPage.CurrentStepsLabel != null)
            {
                //Update labels
                StepsPage.CurrentStepsLabel.Text = stepCount.ToString();


                StepsPage.KilometersLabel.Text = Math.Floor(kilometers * 10) / 10 + " Kilometers";

                double calories = stepCount * 0.04;
                StepsPage.CaloriesLabel.Text = Math.Round(calories, 2) + " Calories";
            }

            //If goal not reached, fill other part transparent
            if (percentageDone < 1)
            {
                float percentageLeft = 1f - percentageDone;
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
