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
        public DevicePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            dpvm = new DevicePageViewModel(this);
            BindingContext = dpvm;
            BuildPage();
        }

        private void BuildPage()
        {
            absoluteLayout = new AbsoluteLayout();
            PageBuilder.BuildPageBasics(absoluteLayout, this);
            PageBuilder.BuildAndAddHeaderImages(absoluteLayout);
            PageBuilder.BuildAndAddLabel(absoluteLayout, "Device", 0.05, 0.10);
            PageBuilder.BuildAndAddReturnButton(absoluteLayout, this);

            #region scanbutton
            Button scanButton = PageBuilder.BuildAddButton(absoluteLayout, "Scan for devices", "scanbuttonCommand", 0.5, 0.25,
                Globals.screenHeight / 100 * 40, Globals.screenHeight / 100 * 7);
            scanButton.CornerRadius = (int)Globals.screenHeight / 100 * 7;
            scanButton.FontSize = Globals.screenHeight / 100 * 2;
            #endregion

            #region device ListView
            ListView devicelist = new ListView();
            devicelist = new ListView();
            devicelist.ItemsSource = dpvm.DeviceList;
            devicelist.SetBinding(ListView.ItemsSourceProperty, new Binding("DeviceList"));
            AbsoluteLayout.SetLayoutBounds(devicelist, new Rectangle(0, 0.8, 1, 0.3));
            AbsoluteLayout.SetLayoutFlags(devicelist, AbsoluteLayoutFlags.All);
            absoluteLayout.Children.Add(devicelist);
            #endregion
            PageBuilder.AddHeaderImages(absoluteLayout);
            PageBuilder.AddLabel(absoluteLayout, "Device", 0.05, 0.10, Globals.lighttextColor);
            PageBuilder.AddReturnButton(absoluteLayout, this);
        }
    }
}