using SkiaSharp;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WindesHeartApp.Data.Interfaces;
using WindesHeartApp.Pages;
using WindesHeartApp.Services;
using WindesHeartSDK;
using WindesHeartSDK.Models;
using Xamarin.Forms;
using Entry = Microcharts.Entry;

namespace WindesHeartApp.ViewModels
{
    public class StepsViewModel : INotifyPropertyChanged
    {
        private int _steps;
        private bool _realtimeStepsEnabled = false;

        public DateTime StartDate { get; }

        public DateTime SelectedDate;
        private IStepsRepository _stepsRepository;

        public Command NextDayBinding { get; }
        public Command PreviousDayBinding { get; }
        public Command Day1Binding { get; }
        public Command Day2Binding { get; }
        public Command Day3Binding { get; }
        public Command Day4Binding { get; }
        public Command Day5Binding { get; }
        public Command Day6Binding { get; }
        public Command TodayBinding { get; }
        public Command GetStepsBinding { get; }
        public Command ToggleRealTimeStepsBinding { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public StepsViewModel(IStepsRepository stepsRepository)
        {
            _stepsRepository = stepsRepository;
            StartDate = DateTime.Now;
            SelectedDate = StartDate;
            GetStepsBinding = new Command(HandleGetSteps);
            ToggleRealTimeStepsBinding = new Command(ToggleRealTimeSteps);

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

            //Get all stepinfo from the Database
            StepInfo StepInfo = null;

            foreach (StepInfo info in StepsPage.StepInfo)
            {
                //If the same day
                if (info.DateTime.Year == SelectedDate.Year && info.DateTime.Month == SelectedDate.Month && info.DateTime.Day == SelectedDate.Day)
                {
                    //we found our info!
                    StepInfo = info;
                    break;
                }
            }

            if (StepInfo != null)
            {
                //Set correct chart
                FillChart(StepInfo.StepCount);
            }
            else
            {
                FillChart(0);
            }
        }

        public void FillChart(int stepCount)
        {
            float percentageDone = stepCount / 2000;
            StepsPage.Entries.Add(new Entry(percentageDone) { Color = SKColors.Black });

            //If goal not reached, fill other part transparent
            if (percentageDone < 100)
            {
                float percentageLeft = 100 - percentageDone;
                StepsPage.Entries.Add(new Entry(percentageLeft) { Color = SKColors.Transparent });
            }
        }

        private void NextDayBtnClick(object obj)
        {
            Console.WriteLine("Next day clicked!");

            //Dont go in the future
            if (SelectedDate < StartDate)
            {
                SelectedDate = SelectedDate.AddDays(1);
            }
            Console.WriteLine(SelectedDate);

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

        private void TodayBtnClick(object obj)
        {
            Console.WriteLine("Today clicked!");
            SelectedDate = StartDate;
            SetDayEnabled(StepsPage.TodayButton);
            UpdateDay();
        }

        private void Day6BtnClick(object obj)
        {
            Console.WriteLine("6 clicked!");
            SelectedDate = StartDate.AddDays(-1);
            SetDayEnabled(StepsPage.Day6Button);
            UpdateDay();
        }

        private void Day5BtnClick(object obj)
        {
            Console.WriteLine("5 clicked!");
            SelectedDate = StartDate.AddDays(-2);
            SetDayEnabled(StepsPage.Day5Button);
            UpdateDay();
        }

        private void Day4BtnClick(object obj)
        {
            Console.WriteLine("4 clicked!");
            SelectedDate = StartDate.AddDays(-3);
            SetDayEnabled(StepsPage.Day4Button);
            UpdateDay();
        }

        private void Day3BtnClick(object obj)
        {
            Console.WriteLine("3 clicked!");
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

        private void Day2BtnClick(object obj)
        {
            Console.WriteLine("2 clicked!");
            SelectedDate = StartDate.AddDays(-5);
            SetDayEnabled(StepsPage.Day2Button);
            UpdateDay();
        }

        private void Day1BtnClick(object obj)
        {
            Console.WriteLine("1 clicked!");
            SelectedDate = StartDate.AddDays(-6);
            SetDayEnabled(StepsPage.Day1Button);
            UpdateDay();
        }

        private async void HandleGetSteps()
        {
            Console.WriteLine("GetSteps Clicked!");
            if (Windesheart.ConnectedDevice != null)
            {
                try
                {
                    StepInfo info = await Windesheart.ConnectedDevice.GetSteps();
                    StepsPage.CurrentStepsLabel.Text = "Steps: " + info.StepCount;
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occured: " + e.Message);
                }
            }
        }

        private void ToggleRealTimeSteps()
        {
            if (Windesheart.ConnectedDevice != null)
            {
                if (_realtimeStepsEnabled)
                {
                    try
                    {
                        Windesheart.ConnectedDevice.DisableRealTimeSteps();
                        StepsPage.ToggleRealTimeStepsButton.Text = "Enable Realtime Steps";
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("An error occured: " + e.Message);
                        return;
                    }
                }
                else
                {
                    try
                    {
                        Windesheart.ConnectedDevice.EnableRealTimeSteps(CallbackHandler.OnStepsUpdated);
                        StepsPage.ToggleRealTimeStepsButton.Text = "Disable Realtime Steps";
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("An error occured: " + e.Message);
                        return;
                    }
                }
                _realtimeStepsEnabled = !_realtimeStepsEnabled;
            }
        }
        public int Steps
        {
            get { return _steps; }
            set
            {
                _steps = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(StepsLabelText));
            }
        }

        public string StepsLabelText
        {
            get { return Steps.ToString(); }
        }


        public void OnStepsUpdated(StepInfo stepInfo)
        {
            StepsPage.CurrentStepsLabel.Text = stepInfo.StepCount.ToString();
        }
    }
}
