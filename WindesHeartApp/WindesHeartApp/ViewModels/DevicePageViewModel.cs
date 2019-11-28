using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WindesHeartApp.Pages;
using WindesHeartApp.Resources;
using WindesHeartSDK;
using Xamarin.Forms;

namespace WindesHeartApp.ViewModels
{
    public class DevicePageViewModel : INotifyPropertyChanged
    {
        private string key = "LastConnectedDeviceGuid";
        private BLEDevice _selectedDevice;
        private int _heartrateInterval;
        private BLEDevice _device;
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<BLEDevice> deviceList;
        private bool _isLoading;
        private string _statusText;
        public Command scanButtonCommand { get; }
        public Command disconnectButtonCommand { get; }

        public DevicePageViewModel()
        {
            scanButtonCommand = new Command(scanButtonClicked);
            disconnectButtonCommand = new Command(disconnectButtonClicked);
            if (DeviceList == null)
                DeviceList = new ObservableCollection<BLEDevice>();
            _heartrateInterval = Globals.heartrateInterval;
            if (Windesheart.ConnectedDevice == null)
                StatusText = "Disconnected";
        }

        private async void disconnectButtonClicked()
        {
            IsLoading = true;
            Windesheart.ConnectedDevice.Disconnect();
            await Task.Delay(500);
            IsLoading = false;
            StatusText = "Disconnected";
        }

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ObservableCollection<BLEDevice> DeviceList
        {
            get { return deviceList; }
            set
            {
                deviceList = value;
                OnPropertyChanged();
            }
        }

        public string StatusText
        {
            get { return _statusText; }
            set
            {
                _statusText = value;
                OnPropertyChanged();
            }
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

        public BLEDevice SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                _selectedDevice = value;

                if (_selectedDevice == null)
                    return;
                OnPropertyChanged();
                deviceSelected(_selectedDevice);
                DevicePage.devicelist.SelectedItem = null;
                _selectedDevice = null;

            }
        }
        private async void scanButtonClicked()
        {
            try
            {
                StatusText = "Scanning for devices";
                IsLoading = true;
                var devices = await Windesheart.ScanForDevices();
                if (devices != null)
                {
                    DeviceList = devices;
                }

                StatusText = $"Results found: {devices.Count}";
                IsLoading = false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private async void deviceSelected(BLEDevice device)
        {
            try
            {
                StatusText = $"Connecting to {device.Name}";
                IsLoading = true;
                device.Connect();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void SaveDeviceInAppProperties(Guid guid)
        {
            if (guid != Guid.Empty)
            {
                if (App.Current.Properties.ContainsKey(key))
                {
                    App.Current.Properties.Remove(key);
                }

                App.Current.Properties.Add(key, guid);
            }
        }
    }
}
