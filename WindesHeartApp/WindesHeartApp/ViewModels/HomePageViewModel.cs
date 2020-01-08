using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WindesHeartApp.Pages;
using WindesHeartApp.Resources;
using WindesHeartApp.Views;
using WindesHeartSDK;
using WindesHeartSDK.Models;
using Xamarin.Forms;

namespace WindesHeartApp.ViewModels
{
    public class HomePageViewModel : INotifyPropertyChanged
    {
        private int _battery;
        private int _heartrate;
        private string _batteryImage = "";
        private bool _isLoading;
        public bool toggle;
        private string _bandnameLabel;
        private float _fetchProgress;
        private bool _fetchProgressVisible;
        public event PropertyChangedEventHandler PropertyChanged;

        public HomePageViewModel()
        {
            if (Windesheart.PairedDevice != null)
            {
                ReadCurrentBattery();
                BandNameLabel = Windesheart.PairedDevice.Name;
            }

            toggle = false;
        }

        public async Task ReadCurrentBattery()
        {
            //catch!!
            var battery = await Windesheart.PairedDevice.GetBattery();
            UpdateBattery(battery);
        }

        public void UpdateBattery(BatteryData battery)
        {
            if (battery.Percentage == 0)
            {
                BatteryImage = "";
                return;
            };
            Battery = battery.Percentage;
            if (battery.Status == BatteryStatus.Charging)
            {
                BatteryImage = "BatteryCharging.png";
                return;
            }

            if (battery.Percentage >= 0 && battery.Percentage < 26)
            {
                BatteryImage = "BatteryQuart.png";
            }
            else if (battery.Percentage >= 26 && battery.Percentage < 51)
            {
                BatteryImage = "BatteryHalf.png";
            }
            else if (battery.Percentage >= 51 && battery.Percentage < 76)
            {
                BatteryImage = "BatteryThreeQuarts.png";
            }
            else if (battery.Percentage >= 76)
            {
                BatteryImage = "BatteryFull.png";
            }
        }


        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public string BatteryImage
        {
            get => _batteryImage;
            set
            {
                _batteryImage = value;
                OnPropertyChanged();
            }
        }

        public int Battery
        {
            get => _battery;
            set
            {
                _battery = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayBattery));
            }
        }
        public int Heartrate
        {
            get => _heartrate;
            set
            {
                _heartrate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayHeartRate));
            }
        }

        public string BandNameLabel
        {
            get => _bandnameLabel;
            set
            {
                _bandnameLabel = value;
                OnPropertyChanged();
            }
        }

        public float FetchProgress
        {
            get => _fetchProgress;
            set
            {
                _fetchProgress = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayFetchProgress));


            }
        }

        public bool FetchProgressVisible
        {
            get => _fetchProgressVisible;
            set
            {
                _fetchProgressVisible = value;
                OnPropertyChanged();
            }
        }
        public string DisplayHeartRate => Heartrate != 0 ? $"Last Heartbeat: {Heartrate.ToString()}" : "";

        public string DisplayBattery => Battery != 0 ? $"{Battery.ToString()}%" : "";

        public float DisplayFetchProgress => FetchProgress != 0 ? FetchProgress : 0;

        public async void AboutButtonClicked(object sender, EventArgs args)
        {
            EnableDisableButtons(false);

            IsLoading = true;
            await Application.Current.MainPage.Navigation.PushAsync(new AboutPage());
            IsLoading = false;
            EnableDisableButtons(true);

        }
        public async void SettingsButtonClicked(object sender, EventArgs args)
        {
            EnableDisableButtons(false);
            IsLoading = true;
            await Application.Current.MainPage.Navigation.PushAsync(new SettingsPage()
            {
                BindingContext = Globals.SettingsPageViewModel
            });
            IsLoading = false;
            EnableDisableButtons(true);
        }

        public async void StepsButtonClicked(object sender, EventArgs args)
        {
            EnableDisableButtons(false);
            IsLoading = true;
            await Application.Current.MainPage.Navigation.PushAsync(new StepsPage()
            {
                BindingContext = Globals.StepsPageViewModel
            });
            IsLoading = false;
            EnableDisableButtons(true);
        }

        public async void HeartrateButtonClicked(object sender, EventArgs args)
        {
            EnableDisableButtons(false);
            IsLoading = true;
            await Application.Current.MainPage.Navigation.PushAsync(new HeartratePage()
            {
                BindingContext = Globals.HeartratePageViewModel
            });
            IsLoading = false;
            EnableDisableButtons(true);


        }

        public async void SleepButtonClicked(object sender, EventArgs args)
        {
            EnableDisableButtons(false);

            IsLoading = true;
            await Application.Current.MainPage.Navigation.PushAsync(new SleepPage()
            {
                BindingContext = Globals.SleepPageViewModel
            });
            IsLoading = false;
            EnableDisableButtons(true);

        }

        public async void DeviceButtonClicked(object sender, EventArgs args)
        {
            EnableDisableButtons(false);

            IsLoading = true;
            await Application.Current.MainPage.Navigation.PushAsync(new DevicePage()
            {
                BindingContext = Globals.DevicePageViewModel
            });
            IsLoading = false;
            EnableDisableButtons(true);


        }

        public void EnableDisableButtons(bool enable)
        {
            HomePage.SleepButton.IsEnabled = enable;
            HomePage.AboutButton.IsEnabled = enable;
            HomePage.SettingsButton.IsEnabled = enable;
            HomePage.StepsButton.IsEnabled = enable;
            HomePage.HeartrateButton.IsEnabled = enable;
            HomePage.DeviceButton.IsEnabled = enable;
        }

        public void ShowFetchProgress(float progress)
        {
            FetchProgress = progress;
            if (progress == 1f)
            {
                FetchProgressVisible = false;
            }
            else
            {
                FetchProgressVisible = true;
            }
        }
    }
}
