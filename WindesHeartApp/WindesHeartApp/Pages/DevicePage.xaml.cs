
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WindesHeartApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DevicePage : ContentPage
    {
        public DevicePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            BuildPage();
        }

        private void BuildPage()
        {
            AbsoluteLayout absoluteLayout = new AbsoluteLayout();

            PageBuilder.BuildPageBasics(absoluteLayout, this);
            PageBuilder.BuildAndAddHeaderImages(absoluteLayout);
            PageBuilder.BuildAndAddLabel(absoluteLayout, "Device", 0.05, 0.10);
            PageBuilder.BuildAndAddReturnButton(absoluteLayout, this);
        }
    }
}