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
        public event PropertyChangedEventHandler PropertyChanged;

        public HomePageViewModel()
        {
            if (Windesheart.ConnectedDevice != null)
                ReadCurrentBattery();
        }

        public async Task ReadCurrentBattery()
        {
            //catch!!
            var battery = await Windesheart.ConnectedDevice.GetBattery();
            UpdateBattery(battery);
        }

        public void UpdateBattery(Battery battery)
        {
            Battery = battery.BatteryPercentage;
            if (battery.Status == StatusEnum.Charging)
            {
                BatteryImage = "BatteryCharging.png";
                return;
            }

            if (battery.BatteryPercentage >= 0 && battery.BatteryPercentage < 26)
            {
                BatteryImage = "BatteryQuart.png";
            }
            else if (battery.BatteryPercentage >= 26 && battery.BatteryPercentage < 51)
            {
                BatteryImage = "BatteryHalf.png";
            }
            else if (battery.BatteryPercentage >= 51 && battery.BatteryPercentage < 76)
            {
                BatteryImage = "BatteryThreeQuarts.png";
            }
            else if (battery.BatteryPercentage >= 76)
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
        public string DisplayHeartRate => Heartrate != 0 ? $"Your Last heartbeat was: {Heartrate.ToString()}" : "";

        public string DisplayBattery => Battery != 0 ? $"{Battery.ToString()}%" : "";

        public async void AboutButtonClicked(object sender, EventArgs args)
        {
            IsLoading = true;
            await Application.Current.MainPage.Navigation.PushAsync(new AboutPage());
            IsLoading = false;
        }
        public async void SettingsButtonClicked(object sender, EventArgs args)
        {
            IsLoading = true;
            await Application.Current.MainPage.Navigation.PushAsync(new SettingsPage()
            {
                BindingContext = Globals.SettingsPageViewModel
            });
            IsLoading = false;
        }

        public async void StepsButtonClicked(object sender, EventArgs args)
        {
            IsLoading = true;
            await Application.Current.MainPage.Navigation.PushAsync(new StepsPage()
            {
                BindingContext = Globals.StepsPageViewModel
            });
            IsLoading = false;
        }

        public async void HeartrateButtonClicked(object sender, EventArgs args)
        {
            IsLoading = true;

            await Application.Current.MainPage.Navigation.PushAsync(new HeartratePage()
            {
                BindingContext = Globals.HeartratePageViewModel
            });
            IsLoading = false;
        }

        public async void SleepButtonClicked(object sender, EventArgs args)
        {
            IsLoading = true;
            await Application.Current.MainPage.Navigation.PushAsync(new SleepPage()
            {
                BindingContext = Globals.SleepPageViewModel
            });
            IsLoading = false;
        }

        public async void DeviceButtonClicked(object sender, EventArgs args)
        {
            IsLoading = true;
            await Application.Current.MainPage.Navigation.PushAsync(new DevicePage()
            {
                BindingContext = Globals.DevicePageViewModel
            });
            IsLoading = false;

        }
    }
}
