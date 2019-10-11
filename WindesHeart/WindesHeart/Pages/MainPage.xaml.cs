using System;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using WindesHeart.ViewModels;

namespace WindesHeart.Pages
{
    public partial class MainPage
    {
        private readonly MainViewModel _viewModel;
        private readonly ResourceManager _rm = new ResourceManager("WindesHeart.Resources.AppResources", typeof(MainPage).GetTypeInfo().Assembly);

        public MainPage()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            BindingContext = _viewModel;

            NoDeviceLayout.GestureRecognizers.Add(
                new TapGestureRecognizer()
                {
                    Command = _viewModel.PairDeviceCommand
                });
            DeviceLayout.GestureRecognizers.Add(
                new TapGestureRecognizer()
                {
                    Command = _viewModel.ConnectDeviceCommand
                });
            DeviceLayout.GestureRecognizers.Add(
                new SwipeGestureRecognizer
                {
                    Direction = SwipeDirection.Left,
                    Command = new Command(RemoveDevice)
                }
            );
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            RequestPermission();
        }

        private async void RemoveDevice()
        {
            var answer = await DisplayAlert(_rm.GetString("Alert_Warning"), _rm.GetString("Main_RemoveDevice"), _rm.GetString("Alert_Yes"), _rm.GetString("Alert_No"));

            if (answer)
            {
                _viewModel.RemoveDeviceCommand.Execute(null);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void RequestPermission()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status == PermissionStatus.Granted)
                {
                    return;
                }

                await DisplayAlert(_rm.GetString("Main_Location_Title"), _rm.GetString("Main_Location_Text"), _rm.GetString("Main_Ok"));
                await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Oh snap: {0}", ex);
            }
        }

    }
}
