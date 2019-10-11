using Xamarin.Forms;
using System.ComponentModel;
using System.Windows.Input;
using WindesHeart.MiBand;
using WindesHeart.Pages;
using WindesHeart.Services;

namespace WindesHeart.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private readonly INavigationService _navigationService;

        public ICommand PairDeviceCommand { protected set; get; }
        public ICommand ConnectDeviceCommand { protected set; get; }
        public ICommand RemoveDeviceCommand { protected set; get; }
        public ICommand StartFetchingCommand { protected set; get; }
        public ICommand VisualizeCommand { protected set; get; }

        public MiBandDevice MiBand => App.MiBandDevice;

        public MainViewModel()
        {
            PairDeviceCommand = new Command(PairDevice);
            ConnectDeviceCommand = new Command(ConnectDevice);
            StartFetchingCommand = new Command(StartFetching);
            RemoveDeviceCommand = new Command(RemoveDevice);
            VisualizeCommand = new Command(Visualize);

            _navigationService = App.WindesHeart;
        }

        private async void PairDevice()
        {
            await _navigationService.NavigateAsync(nameof(PairDevicePage));
        }

        private void ConnectDevice()
        {
            App.MiBandDevice.Connect();
        }

        private void StartFetching() 
        {
            App.MiBandDevice.StartFetching();
        }

        private void RemoveDevice() 
        {
            App.MiBandDevice.RemoveDevice();
        }

        private async void Visualize()
        {
            await _navigationService.NavigateAsync(nameof(WebViewer));
        }
    }
}
