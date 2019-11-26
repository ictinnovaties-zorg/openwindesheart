using WindesHeartApp.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WindesHeartApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeartratePage : ContentPage
    {
        public HeartratePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            BindingContext = Globals.heartrateviewModel;
            BuildPage();
        }

        private void BuildPage()
        {
            PageBuilder.BuildPageBasics(absoluteLayout, this);
            PageBuilder.BuildAndAddHeaderImages(absoluteLayout);
            PageBuilder.BuildAndAddLabel(absoluteLayout, "Heartrate", 0.05, 0.10);
            PageBuilder.BuildAndAddReturnButton(absoluteLayout, this);
            var heartrateLabel = PageBuilder.BuildAndAddLabel(absoluteLayout, "", 0.1, 0.5);

            heartrateLabel.FontSize = 15;
            heartrateLabel.SetBinding(Label.TextProperty, new Binding("DisplayHeartrateMessage"));

        }
    }
}