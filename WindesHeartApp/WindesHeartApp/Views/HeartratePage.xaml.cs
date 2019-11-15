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
            BindingContext = Globals.hrviewModel;
        }

        protected override void OnAppearing()
        {
            BuildPage();
        }

        private void BuildPage()
        {
            PageBuilder.BuildPageBasics(absoluteLayout, this);
            PageBuilder.BuildAndAddHeaderImages(absoluteLayout);
            PageBuilder.BuildAndAddLabel(absoluteLayout, "Heartrate", 0.05, 0.10);
            PageBuilder.BuildAndAddReturnButton(absoluteLayout, this);
            AbsoluteLayout.SetLayoutBounds(heartrateLabel, new Rectangle(0.1, 0.5, 300, 50));
            AbsoluteLayout.SetLayoutFlags(heartrateLabel, AbsoluteLayoutFlags.PositionProportional);
            heartrateLabel.FontSize = 15;

            Button button = new Button();
            button.SetBinding(Button.CommandProperty, new Binding() { Path = "buttonClickedCommand" });
            button.Text = "Click me, watch the HR change!";
            AbsoluteLayout.SetLayoutBounds(button, new Rectangle(0.1, 0.7, 300, 50));
            AbsoluteLayout.SetLayoutFlags(button, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(button);
        }
    }
}