using Plugin.BluetoothLE;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using WindesHeartApp.Pages;
using WindesHeartApp.Resources;
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
        private string _scanbuttonText;
        public event PropertyChangedEventHandler PropertyChanged;

        public DevicePageViewModel()
        {
            if (DeviceList == null)
                DeviceList = new ObservableCollection<BLEDevice>();
            if (Windesheart.ConnectedDevice == null)
                StatusText = "Disconnected";
            ScanButtonText = "Scan for devices";
        }
        public void DisconnectButtonClicked(object sender, EventArgs args)
        {
            IsLoading = true;
            Windesheart.ConnectedDevice?.Disconnect();
            IsLoading = false;
            StatusText = "Disconnected";
            Globals.HomePageViewModel.Heartrate = 0;
            Globals.HomePageViewModel.Battery = 0;
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

        public string ScanButtonText
        {
            get { return _scanbuttonText; }
            set
            {
                _scanbuttonText = value;
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
                DevicePage.Devicelist.SelectedItem = null;
                _selectedDevice = null;

            }
        }

        public async void ScanButtonClicked(object sender, EventArgs args)
        {
            DeviceList = new ObservableCollection<BLEDevice>();
            DisconnectButtonClicked(sender, EventArgs.Empty);
            try
            {
                //If already scanning, stop scanning
                if (CrossBleAdapter.Current.IsScanning)
                {
                    Windesheart.StopScanning();
                    ScanButtonText = "Scan for devices";
                    IsLoading = false;
                }
                else
                {

                    if (CrossBleAdapter.Current.Status == AdapterStatus.PoweredOff)
                    {
                        await Application.Current.MainPage.DisplayAlert("Bluetooth turned off",
                            "Bluetooth is turned off. Please enable bluetooth to start scanning for devices", "OK");
                        StatusText = "Bluetooth turned off";
                        return;
                    }

                    //If started scanning
                    if (Windesheart.StartScanning(OnDeviceFound))
                    {
                        ScanButtonText = "Stop scanning";
                        StatusText = "Scanning...";
                        IsLoading = true;
                    }
                    else
                    {
                        StatusText = "Could not start scanning.";
                        ScanButtonText = "Scan for devices";
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        /// <summary> f
        /// Called when leaving the page
        /// </summary>
        public void OnDisappearing()
        {
            Console.WriteLine("Stopping scanning...");
            Windesheart.StopScanning();
            IsLoading = false;
            StatusText = "";
            ScanButtonText = "Scan for devices";
            DeviceList = new ObservableCollection<BLEDevice>();
        }

        private void OnDeviceFound(BLEDevice device)
        {
            DeviceList.Add(device);
        }

        private void DeviceSelected(BLEDevice device)
        {
            try
            {
                ScanButtonText = "Scan for devices";
                Windesheart.StopScanning();

                StatusText = "Connecting...";
                IsLoading = true;
                device?.Connect(CallbackHandler.OnConnetionCallBack);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

    }
}
