using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WindesHeartApp.Pages;
using Xamarin.Forms;

namespace WindesHeartApp.ViewModels
{
    public class DevicePageViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private DevicePage _page;
        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private ObservableCollection<string> deviceList;

        public Command scanButtonCommand { get; }

        public DevicePageViewModel(DevicePage page)
        {
            scanButtonCommand = new Command(scanButtonClicked);
            DeviceList = new ObservableCollectionListSource<string>();
        }


        public ObservableCollection<string> DeviceList
        {
            get { return deviceList; }
            set
            {
                deviceList = value;
                OnPropertyChanged();
            }
        }

        private void scanButtonClicked()
        {
            DeviceList.Add("SWAG");
        }
    }
}
