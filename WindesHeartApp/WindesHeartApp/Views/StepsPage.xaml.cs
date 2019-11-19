using WindesHeartApp.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WindesHeartApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StepsPage : ContentPage
    {
        public static Label CurrentStepsLabel;
        public static Button ToggleRealTimeStepsButton;
        public StepsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            BindingContext = Globals.StepsViewModel;
            BuildPage();

        }

        private void BuildPage()
        {
            AbsoluteLayout absoluteLayout = new AbsoluteLayout();

            PageBuilder.BuildPageBasics(absoluteLayout, this);
            PageBuilder.BuildAndAddHeaderImages(absoluteLayout);
            PageBuilder.BuildAndAddLabel(absoluteLayout, "Steps", 0.05, 0.10);
            PageBuilder.BuildAndAddReturnButton(absoluteLayout, this);

            CurrentStepsLabel = PageBuilder.BuildAndAddLabel(absoluteLayout, "Steps:", 0.2, 0.2);
            CurrentStepsLabel.SetBinding(Label.TextProperty, new Binding("StepsLabelText"));

            Button getStepsbutton = new Button();
            getStepsbutton.SetBinding(Button.CommandProperty, new Binding("GetStepsBinding"));
            getStepsbutton.Text = "Get steps";
            AbsoluteLayout.SetLayoutBounds(getStepsbutton, new Rectangle(0.2, 0.4, 300, 50));
            AbsoluteLayout.SetLayoutFlags(getStepsbutton, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(getStepsbutton);

            ToggleRealTimeStepsButton = new Button();
            ToggleRealTimeStepsButton.SetBinding(Button.CommandProperty, new Binding() { Path = "ToggleRealTimeStepsBinding" });
            ToggleRealTimeStepsButton.Text = "Enable Realtime Steps";
            AbsoluteLayout.SetLayoutBounds(ToggleRealTimeStepsButton, new Rectangle(0.2, 0.5, 300, 50));
            AbsoluteLayout.SetLayoutFlags(ToggleRealTimeStepsButton, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(ToggleRealTimeStepsButton);
        }
    }
}