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
            PageBuilder.AddHeaderImages(absoluteLayout);
            PageBuilder.AddLabel(absoluteLayout, "Heartrate", 0.05, 0.10, Globals.lighttextColor);
            PageBuilder.AddReturnButton(absoluteLayout, this);

            var heartrateLabel = PageBuilder.AddLabel(absoluteLayout, "", 0.1, 0.5, Color.Black);
            AbsoluteLayout.SetLayoutBounds(heartrateLabel, new Rectangle(0.1, 0.5, 300, 50));
            AbsoluteLayout.SetLayoutFlags(heartrateLabel, AbsoluteLayoutFlags.PositionProportional);
            heartrateLabel.SetBinding(Label.TextProperty, new Binding("DisplayHeartrateMessage"));
            heartrateLabel.FontSize = 15;
        }
    }
}