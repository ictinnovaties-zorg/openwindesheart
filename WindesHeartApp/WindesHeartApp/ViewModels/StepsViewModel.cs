using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WindesHeartApp.Data.Interfaces;
using WindesHeartApp.Pages;
using WindesHeartApp.Services;
using WindesHeartSDK;
using WindesHeartSDK.Models;
using Xamarin.Forms;

namespace WindesHeartApp.ViewModels
{
    public class StepsViewModel : INotifyPropertyChanged
    {
        private int _steps;
        private bool _realtimeStepsEnabled = false;
        private readonly IStepsRepository _stepsRepository;
        public DateTime StartDate { get; }

        public DateTime SelectedDate;
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
            UpdateCurrentDayLabel();
        }

        private void UpdateCurrentDayLabel()
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
            UpdateCurrentDayLabel();
        }

        private void TodayBtnClick(object obj)
        {
            Console.WriteLine("Today clicked!");
            SelectedDate = StartDate;
            SetDayEnabled(StepsPage.TodayButton);
            UpdateCurrentDayLabel();
        }

        private void Day6BtnClick(object obj)
        {
            Console.WriteLine("6 clicked!");
            SelectedDate = StartDate.AddDays(-1);
            SetDayEnabled(StepsPage.Day6Button);
            UpdateCurrentDayLabel();
        }

        private void Day5BtnClick(object obj)
        {
            Console.WriteLine("5 clicked!");
            SelectedDate = StartDate.AddDays(-2);
            SetDayEnabled(StepsPage.Day5Button);
            UpdateCurrentDayLabel();
        }

        private void Day4BtnClick(object obj)
        {
            Console.WriteLine("4 clicked!");
            SelectedDate = StartDate.AddDays(-3);
            SetDayEnabled(StepsPage.Day4Button);
            UpdateCurrentDayLabel();
        }

        private void Day3BtnClick(object obj)
        {
            Console.WriteLine("3 clicked!");
            SelectedDate = StartDate.AddDays(-4);
            SetDayEnabled(StepsPage.Day3Button);
            UpdateCurrentDayLabel();
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
            UpdateCurrentDayLabel();
        }

        private void Day1BtnClick(object obj)
        {
            Console.WriteLine("1 clicked!");
            SelectedDate = StartDate.AddDays(-6);
            SetDayEnabled(StepsPage.Day1Button);
            UpdateCurrentDayLabel();
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



        public void OnStepsUpdated(StepInfo stepInfo)
        {
            StepsPage.CurrentStepsLabel.Text = stepInfo.StepCount.ToString();
        }
    }
}
