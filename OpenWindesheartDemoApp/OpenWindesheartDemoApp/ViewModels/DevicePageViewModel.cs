using OpenWindesheart;
using OpenWindesheart.Models;
using OpenWindesheartDemoApp.Resources;
using OpenWindesheartDemoApp.Services;
using OpenWindesheartDemoApp.Views;
using Plugin.BluetoothLE;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace OpenWindesheartDemoApp.ViewModels
{
    public class DevicePageViewModel : INotifyPropertyChanged
    {
        private bool _isLoading;
        private string _statusText;
        private BLEScanResult _selectedDevice;
        private ObservableCollection<BLEScanResult> _deviceList;
        private string _scanbuttonText;
        public event PropertyChangedEventHandler PropertyChanged;

        public DevicePageViewModel()
        {
            if (DeviceList == null)
                DeviceList = new ObservableCollection<BLEScanResult>();
            if (Windesheart.PairedDevice == null)
                StatusText = "Disconnected";
            ScanButtonText = "Scan for devices";
        }

        public void DisconnectButtonClicked(object sender, EventArgs args)
        {
            DevicePage.DisconnectButton.IsEnabled = false;
            IsLoading = true;
            Windesheart.PairedDevice?.Disconnect();
            IsLoading = false;
            StatusText = "Disconnected";
            DeviceList = new ObservableCollection<BLEScanResult>();
            Globals.HomePageViewModel.Heartrate = 0;
            Globals.HomePageViewModel.Battery = 0;
        }

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    string err = e.InnerException.Message;
                    Trace.WriteLine(err);
                }
            }
        }

        public ObservableCollection<BLEScanResult> DeviceList
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

        public BLEScanResult SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                _selectedDevice = value;

                if (_selectedDevice == null)
                    return;
                DeviceSelected(_selectedDevice.Device);
                DevicePage.Devicelist.SelectedItem = null;
            }
        }

        public async void ScanButtonClicked(object sender, EventArgs args)
        {
            DisconnectButtonClicked(sender, EventArgs.Empty);
            try
            {
                //If already scanning, stop scanning
                if (Windesheart.IsScanning())
                {
                    Windesheart.StopScanning();
                    ScanButtonText = "Scan for devices";
                    IsLoading = false;
                }
                else
                {
                    if (Windesheart.AdapterStatus == AdapterStatus.PoweredOff)
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

        /// <summary> 
        /// Called when leaving the page
        /// </summary>
        public void OnDisappearing()
        {
            Console.WriteLine("Stopping scanning...");
            Windesheart.StopScanning();
            IsLoading = false;
            StatusText = "";
            ScanButtonText = "Scan for devices";
            DeviceList = new ObservableCollection<BLEScanResult>();
        }

        private void OnDeviceFound(BLEScanResult result)
        {
            DeviceList.Add(result);
        }

        private void DeviceSelected(BLEDevice device)
        {
            DevicePage.ReturnButton.IsVisible = false;
            DevicePage.ScanButton.IsEnabled = false;
            if (device == null)
            {
                return;
            }

            try
            {
                Windesheart.StopScanning();

                StatusText = "Connecting...";
                IsLoading = true;

                if (Application.Current.Properties.ContainsKey(device.IDevice.Uuid.ToString()))
                {
                    byte[] secretKey = (byte[])Application.Current.Properties[device.IDevice.Uuid.ToString()];
                    device.Connect(CallbackHandler.OnConnect, secretKey);
                }
                else
                {
                    device.Connect(CallbackHandler.OnConnect);
                }

                Globals.HomePageViewModel.EnableDisableButtons(false);
                Globals.HomePageViewModel.IsLoading = true;
                DeviceList = new ObservableCollection<BLEScanResult>();
                SelectedDevice = null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}