using System;
using System.Collections.Generic;
using WindesHeartSDK;
using Xamarin.Forms;

namespace WindesHeartApp.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public void ScanningForDevices(object sender, EventArgs e)
        {
            BluetoothService.ScanForUniqueDevices();
        }
    }
}
