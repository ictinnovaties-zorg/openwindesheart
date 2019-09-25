using WindesHeart.ViewModels;
using WindesHeartSdk.Services;
using Xamarin.Forms.Xaml;

namespace WindesHeart.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PairDevicePage
	{
        public PairDevicePage ()
		{
            InitializeComponent();
            BindingContext = new PairDeviceViewModel();
        }

        protected override void OnDisappearing()
        {
            BleService.CancelPairing();
        }
    }
}