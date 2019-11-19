using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WindesHeartApp.Pages;
using WindesHeartApp.Resources;
using WindesHeartApp.Services;
using WindesHeartSDK.Models;
using Xamarin.Forms;

namespace WindesHeartApp.ViewModels
{
    public class StepsViewModel : INotifyPropertyChanged
    {
        private int steps;
        private bool realTimeStepsEnabled = false;
        public Command GetStepsBinding { get; }
        public Command ToggleRealTimeStepsBinding { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public StepsViewModel()
        {
            GetStepsBinding = new Command(HandleGetSteps);
            ToggleRealTimeStepsBinding = new Command(ToggleRealTimeSteps);
        }

        private async void HandleGetSteps()
        {
            Console.WriteLine("GetSteps Clicked!");
            if (Globals.device != null)
            {
                try
                {
                    StepInfo info = await Globals.device.GetSteps();
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
            if (Globals.device != null)
            {
                if (realTimeStepsEnabled)
                {
                    try
                    {
                        Globals.device.DisableRealTimeSteps();
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
                        Globals.device.EnableRealTimeSteps(CallbackHandler.OnStepsUpdated);
                        StepsPage.ToggleRealTimeStepsButton.Text = "Disable Realtime Steps";
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("An error occured: " + e.Message);
                        return;
                    }
                }
                realTimeStepsEnabled = !realTimeStepsEnabled;
            }
        }
        public int Steps
        {
            get { return steps; }
            set
            {
                steps = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(StepsLabelText));
            }
        }

        public string StepsLabelText
        {
            get { return "Steps: " + Steps.ToString(); }
        }


        public void OnStepsUpdated(StepInfo stepInfo)
        {
            StepsPage.CurrentStepsLabel.Text = "Steps: " + stepInfo.StepCount;
        }
    }
}
