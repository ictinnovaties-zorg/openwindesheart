using System;
using System.ComponentModel;
using System.Reflection;
using System.Resources;
using System.Windows.Input;
using Rg.Plugins.Popup.Services;
using WindesHeartSdk.Services;
using WindesHeart.Pages;
using WindesHeart.Services;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace WindesHeart.ViewModels
{
    class PairDeviceViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private INavigationService WindesHeart { get; } = App.WindesHeart;
        private readonly ResourceManager _rm = new ResourceManager("WindesHeart.Resources.AppResources", typeof(PairDevicePage).GetTypeInfo().Assembly);

        public ICommand RetryCommand { protected set; get; }
        private IDisposable _pairDeviceSub;

        private string _infoLabel;
        private bool _retryBtnVisible;

        public PairDeviceViewModel()
        {
            RetryCommand = new Command(FindAndConnect);
            WirePairResponse();
            FindAndConnect();
        }

        public string InfoLabel
        {
            get => _infoLabel;
            set
            {
                _infoLabel = value.Replace("\\n", Environment.NewLine);
                PropertyChanged(this, new PropertyChangedEventArgs("InfoLabel"));
            }
        }

        public bool RetryBtnVisible
        {
            get => _retryBtnVisible;
            set
            {
                _retryBtnVisible = value;
                PropertyChanged(this, new PropertyChangedEventArgs("RetryBtnVisible"));
            }
        }

        public void FindAndConnect()
        {
            InfoLabel = _rm.GetString("PairDevice_Scanning");
            RetryBtnVisible = false;
            BleService.ScanNearbyDevices();
        }

        // Update pairing instructions
        private void WirePairResponse()
        {
            _pairDeviceSub?.Dispose();
            _pairDeviceSub = BleService.PairResultSubject.Subscribe(result =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    switch (result)
                    {
                        case BleService.PairResult.Success:
                            App.MiBandDevice.IsPaired = true;
                            Vibration.Vibrate();
                            await WindesHeart.GoBack();
                            break;
                        case BleService.PairResult.Connecting:
                            InfoLabel = _rm.GetString("PairDevice_Connecting");
                            break;
                        case BleService.PairResult.Authenticating:
                            InfoLabel = _rm.GetString("PairDevice_Authorizing");
                            break;
                        case BleService.PairResult.WaitingUser:
                            InfoLabel = _rm.GetString("PairDevice_TabToPair");
                            break;
                        case BleService.PairResult.Failed:
                            InfoLabel = _rm.GetString("PairDevice_Failed");
                            RetryBtnVisible = true;
                            break;
                        case BleService.PairResult.NoDevice:
                            InfoLabel = _rm.GetString("PairDevice_NoDevicesFound");
                            RetryBtnVisible = true;
                            break;
                        case BleService.PairResult.Conflict:
                            InfoLabel = _rm.GetString("PairDevice_Conflict");
                            await PopupNavigation.Instance.PushAsync(new UnpairPopupView());
                            RetryBtnVisible = true;
                            break;
                    }
                });
            });
        }
    }
}
