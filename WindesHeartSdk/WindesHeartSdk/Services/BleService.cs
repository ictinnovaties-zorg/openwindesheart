using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Plugin.BluetoothLE;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using WindesHeartSdk.Helpers;
using WindesHeartSdk.MiBand;
using IAdapter = Plugin.BLE.Abstractions.Contracts.IAdapter;
using IDevice = Plugin.BluetoothLE.IDevice;

namespace WindesHeartSdk.Services
{
    public static class BleService
    {
        public enum PairResult { Success, Failed, NoDevice, WaitingUser, Connecting, Authenticating, Configuring, Conflict }
        public static Subject<PairResult> PairResultSubject = new Subject<PairResult>();
        public static Subject<ConnectionStatus> ConnectionStatusSubject = new Subject<ConnectionStatus>();
        public static Guid KnownDeviceId;

        private static readonly ObservableCollection<Plugin.BLE.Abstractions.Contracts.IDevice> DeviceList = new ObservableCollection<Plugin.BLE.Abstractions.Contracts.IDevice>();
        private static readonly IBluetoothLE Ble = CrossBluetoothLE.Current;
        private static readonly IAdapter Adapter = CrossBluetoothLE.Current.Adapter;

        private static IDevice _device;
        private static readonly byte[] AuthKey = { 0x01, 0x08, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x40, 0x41, 0x42, 0x43, 0x44, 0x45 };
        private static readonly byte[] RequestNumber = { 0x02, 0x08 };

        private static bool _needsAuth;
        private static bool _isPairing;
        private static bool _isNewDevice;
        private static bool _isNewConnection = true;
        private static System.Timers.Timer _connectionTimer;

        private static IDisposable _statusSub;
        private static IDisposable _adapterSub;
        private static IDisposable _charAuthSub;

        private static IDisposable _characteristicsSub;

        /// <summary>
        /// Scan and connect to closest Mi Band device
        /// </summary>
        public static async void ScanNearbyDevices()
        {
            DeviceList.Clear();
            _isNewDevice = true;

            Adapter.ScanTimeout = 15000;
            Adapter.DeviceDiscovered += (s, a) =>
            {
                Console.WriteLine("Found: {0}", a.Device.Name);
                DeviceList.Add(a.Device);
            };

            if (!Ble.Adapter.IsScanning)
            {
                await Adapter.StartScanningForDevicesAsync();
                ConnectClosestDevice();
            }
        }

        /// <summary>
        /// Connect to last known device
        /// </summary>
        public static async void ConnectKnownDevice()
        {
            await CrossBleAdapter.Current.WhenReady().FirstAsync();

            if (BleTransactionHelper.Device != null && BleTransactionHelper.Device.IsConnected())
            {
                _device = BleTransactionHelper.Device;
                ListenForConnectionChanges();
                ListenForAdapterChanges();

                PairResultSubject.OnNext(PairResult.Success);
                return;
            }

            if (KnownDeviceId != Guid.Empty)
            {
                _isNewDevice = false;
                
                _device = await CrossBleAdapter.Current.GetKnownDevice(KnownDeviceId);
                BleTransactionHelper.Device = _device;

                if (_device != null)
                {
                    ListenForConnectionChanges();

                    PairResultSubject.OnNext(PairResult.Connecting);
                    _device.Connect(new ConnectionConfig { AndroidConnectionPriority = ConnectionPriority.High, AutoConnect = true });
                }
            }
        }

        private static async void ConnectDeviceById(Guid deviceId)
        {
            _device = await CrossBleAdapter.Current.GetKnownDevice(deviceId);

            if (_device != null && _device.IsConnected())
            {
                PairResultSubject.OnNext(PairResult.Conflict);
            }
            else if (_device != null)
            {
                ListenForConnectionChanges();

                PairResultSubject.OnNext(PairResult.Connecting);
                _device.Connect(new ConnectionConfig { AndroidConnectionPriority = ConnectionPriority.High, AutoConnect = true });
            }
        }

        private static void ConnectClosestDevice()
        {
            var device = DeviceList.Where(x => x.Name == "Mi Band 3").OrderByDescending(d => d.Rssi).FirstOrDefault();

            if (device != null)
            {
                _needsAuth = true;
                ConnectDeviceById(device.Id);
            }
            else
            {
                PairResultSubject.OnNext( PairResult.NoDevice);
            }
        }

        private static void ListenForConnectionChanges()
        {
            _statusSub?.Dispose();
            _statusSub = _device.WhenStatusChanged().Subscribe(async status =>
            {
                Console.WriteLine(status);
                ConnectionStatusSubject.OnNext(status);

                //Ignores the initial status 
                if (KnownDeviceId != Guid.Empty && _isNewConnection)
                {
                    _isNewConnection = false;
                    return;
                }
                switch (status)
                {
                    case ConnectionStatus.Connected:
                        await Task.Delay(5000);

                        _connectionTimer?.Dispose();
                        Handshake();
                        break;
                    case ConnectionStatus.Connecting:
                        FindAllCharacteristics();

                        _connectionTimer = new System.Timers.Timer(30000);
                        _connectionTimer.Elapsed += (o, e) =>
                        {
                            if (_device != null && !_device.IsConnected())
                            {
                                Console.WriteLine("Connecting to device took too long, cancelling connection");
                                PairingFailed();
                                DisconnectDevice();
                            }
                        };
                        _connectionTimer.Enabled = true;
                        _connectionTimer.AutoReset = false;
                        break;
                    case ConnectionStatus.Disconnected:
                        FindAllCharacteristics();
                        break;
                }
                
            });
        }

        private static void Handshake()
        {
            if (BleTransactionHelper.GetCharacteristic(MiBandResources.GuidCharacteristicAuth) == null)
            {
                PairingFailed();
                DisconnectDevice();
                return;
            }
            _charAuthSub?.Dispose();
            _charAuthSub = BleTransactionHelper.GetCharacteristic(MiBandResources.GuidCharacteristicAuth).RegisterAndNotify().Timeout(TimeSpan.FromSeconds(20)).Subscribe(
                x =>
                {
                    HandleAuthentication(x.Characteristic.Value);
                },
                x =>
                {
                    PairingFailed();
                });

            if (_needsAuth)
            {
                RequestAuth();
                _needsAuth = false;
            }
            else
            {
                RequestAuthNumber();
            }
        }

        private static async void HandleAuthentication(byte[] response)
        {
            if (response == null)
            {
                return;
            }
            if (response[0] == MiBandResources.AuthResponse && response[1] == MiBandResources.AuthSendKey && response[2] == MiBandResources.AuthSuccess)
            {
                PairResultSubject.OnNext(PairResult.Authenticating);
                RequestAuthNumber();
            }
            else if (response[0] == MiBandResources.AuthResponse && response[1] == MiBandResources.AuthRequestRandomAuthNumber && response[2] == MiBandResources.AuthSuccess)
            {
                await BleTransactionHelper.TryWriteWithoutResponse(MiBandResources.GuidCharacteristicAuth, BleTypeConversions.CreateKey(response));
                await MiBandSupport.SetCurrentTime();
            }
            else if (response[0] == MiBandResources.AuthResponse && response[1] == MiBandResources.AuthSendEncryptedAuthNumber && response[2] == MiBandResources.AuthSuccess)
            {
                Console.WriteLine("Authorization accepted");

                if (_device.IsPairingAvailable() && _device.PairingStatus != PairingStatus.Paired)
                {
                    _device.PairingRequest().Subscribe(isSuccessful =>
                    {
                        if (isSuccessful)
                        {
                            PairingSuccess();
                        }
                        else
                        {
                            Console.WriteLine("Pairing was unsuccessful");
                            PairingFailed();
                        }
                    });
                }
                else
                {
                    PairingSuccess();
                }
            }
            else
            {
                Console.WriteLine("Authorization refused by device");
                PairingFailed();
            }
        }

        private static async void PairingSuccess()
        {
            PairResultSubject.OnNext(PairResult.Configuring);
            _isPairing = false;
            _isNewDevice = false;

            KnownDeviceId = _device.Uuid;
            _charAuthSub?.Dispose();
            ListenForAdapterChanges();

            await MiBandSupport.RestoreSettings();

            PairResultSubject.OnNext(PairResult.Success);
        }

        private static void PairingFailed()
        {
            _isPairing = false;
            PairResultSubject.OnNext(PairResult.Failed);
            _charAuthSub?.Dispose();

            if (_isNewDevice)
            {
                DisconnectDevice();
            }
        }

        /// <summary>
        /// Disconnect connected device.
        /// </summary>
        public static void DisconnectDevice()
        {
            _adapterSub?.Dispose();
            _statusSub?.Dispose();
            _device?.CancelConnection();
        }

        /// <summary>
        /// Cancel pairing process and disconnects connected device.
        /// </summary>
        public static void CancelPairing()
        {
            if (!_isPairing)
            {
                return;
            }
            _connectionTimer?.Dispose();
            _charAuthSub?.Dispose();

            DisconnectDevice();
        }

        private static void ListenForAdapterChanges()
        {
            _adapterSub?.Dispose();
            _adapterSub = CrossBleAdapter.Current.WhenStatusChanged().Where(x => x == AdapterStatus.PoweredOn)
                .Subscribe(status =>
                {
                    if (status == AdapterStatus.PoweredOn && _device != null && _device.IsDisconnected())
                    {
                        _device.Connect(new ConnectionConfig { AndroidConnectionPriority = ConnectionPriority.High, AutoConnect = true });
                    }
                });
        }

        private static async void RequestAuthNumber()
        {
            await BleTransactionHelper.TryWriteWithoutResponse(MiBandResources.GuidCharacteristicAuth, RequestNumber);
        }

        private static async void RequestAuth()
        {
            PairResultSubject.OnNext(PairResult.WaitingUser);
            await BleTransactionHelper.TryWriteWithoutResponse(MiBandResources.GuidCharacteristicAuth, AuthKey);
        }

        private static void FindAllCharacteristics()
        {
            BleTransactionHelper.Characteristics.Clear();
            _characteristicsSub?.Dispose();
            _characteristicsSub = _device.WhenAnyCharacteristicDiscovered().Subscribe(characteristic =>
            {
                if (BleTransactionHelper.Characteristics.Count < 39)
                {
                    BleTransactionHelper.Characteristics.Add(characteristic);
                }
            }, x =>
            {
                Console.WriteLine(x);
            });

        }
        
    }
}
