using WindesHeartApp.Resources;
using WindesHeartApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WindesHeartApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DevicePage : ContentPage
    {
        private DevicePageViewModel dpvm;
        public ListView devicelist;
        public DevicePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            dpvm = new DevicePageViewModel();
            BindingContext = dpvm;
            BuildPage();
        }

        private void BuildPage()
        {
            absoluteLayout = new AbsoluteLayout();
            PageBuilder.BuildPageBasics(absoluteLayout, this);
            PageBuilder.AddHeaderImages(absoluteLayout);
            PageBuilder.AddLabel(absoluteLayout, "Device", 0.05, 0.10, Globals.lighttextColor);
            PageBuilder.AddReturnButton(absoluteLayout, this);

            #region scanbutton
            Button scanButton = PageBuilder.AddButton(absoluteLayout, "Scan for devices", "scanButtonCommand", 0.5, 0.25, 0.4, 0.07, AbsoluteLayoutFlags.All);
            scanButton.CornerRadius = (int)Globals.screenHeight / 100 * 7;
            scanButton.FontSize = Globals.screenHeight / 100 * 2;
            #endregion

            DataTemplate DeviceTemplate = new DataTemplate(() =>
            {
                ImageCell cell = new ImageCell();
                cell.SetBinding(ImageCell.TextProperty, "Name");
                cell.SetBinding(ImageCell.DetailProperty, "Rssi");
                cell.TextColor = Color.Black;

                cell.DetailColor = Color.Black;
                return cell;
            });
            #region device ListView
            devicelist = new ListView();
            devicelist.BackgroundColor = Color.Transparent;
            devicelist.SetBinding(ListView.SelectedItemProperty, new Binding("SelectedDevice", BindingMode.TwoWay));
            devicelist.ItemTemplate = DeviceTemplate;
            devicelist.SetBinding(ListView.ItemsSourceProperty, new Binding("DeviceList"));
            AbsoluteLayout.SetLayoutBounds(devicelist, new Rectangle(0, 0.8, 0.95, 0.3));
            AbsoluteLayout.SetLayoutFlags(devicelist, AbsoluteLayoutFlags.All);
            absoluteLayout.Children.Add(devicelist);
            #endregion

            #region disconnectButton
            Button disconnectButton = PageBuilder.AddButton(absoluteLayout, "Disconnect", "disconnectButtonCommand", 0.5, 0.85, 0.5, 0.07, AbsoluteLayoutFlags.All);
            disconnectButton.CornerRadius = (int)Globals.screenHeight / 100 * 7;
            disconnectButton.FontSize = Globals.screenHeight / 100 * 2;
            #endregion

        }
    }
}