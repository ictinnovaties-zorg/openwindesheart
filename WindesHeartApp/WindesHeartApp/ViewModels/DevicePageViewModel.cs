using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WindesHeartApp.Pages;
using WindesHeartApp.Services;
using WindesHeartSDK;
using Xamarin.Forms;

namespace WindesHeartApp.ViewModels
{
    public class DevicePageViewModel : INotifyPropertyChanged
    {
        private bool _isLoading;
        private string _statusText;
        private BLEDevice _selectedDevice;
        private ObservableCollection<BLEDevice> _deviceList;
        public event PropertyChangedEventHandler PropertyChanged;

        public Command ScanButtonCommand { get; }
        public Command DisconnectButtonCommand { get; }

        public DevicePageViewModel()
        {
            ScanButtonCommand = new Command(ScanButtonClicked);
            DisconnectButtonCommand = new Command(DisconnectButtonClicked);
            if (DeviceList == null)
                DeviceList = new ObservableCollection<BLEDevice>();
            if (Windesheart.ConnectedDevice == null)
                StatusText = "Disconnected";
        }
        private void DisconnectButtonClicked()
        {
            IsLoading = true;
            Windesheart.ConnectedDevice.Disconnect();
            IsLoading = false;
            StatusText = "Disconnected";
        }
        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public ObservableCollection<BLEDevice> DeviceList
        {
            get { return _deviceList; }
            set
            {
                _deviceList = value;
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
                DeviceSelected(_selectedDevice);
                DevicePage.devicelist.SelectedItem = null;
                _selectedDevice = null;

            }
        }
        private async void ScanButtonClicked()
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
        private void DeviceSelected(BLEDevice device)
        {
            try
            {
                StatusText = $"Connecting to {device.Name}";
                IsLoading = true;
                device.Connect(CallbackHandler.OnConnetionCallBack);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}
