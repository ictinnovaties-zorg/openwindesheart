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
            PageBuilder.AddHeaderImages(absoluteLayout);
            PageBuilder.AddLabel(absoluteLayout, "Steps", 0.05, 0.10);
            PageBuilder.AddReturnButton(absoluteLayout, this);

            CurrentStepsLabel = PageBuilder.AddLabel(absoluteLayout, "Steps:", 0.2, 0.2);
            CurrentStepsLabel.SetBinding(Label.TextProperty, new Binding("StepsLabelText"));

            PageBuilder.AddButton(absoluteLayout, "Get steps", "GetStepsBinding", 0.2, 0.4, 300, 50);
            ToggleRealTimeStepsButton = PageBuilder.AddButton(absoluteLayout, "Enable realtime steps", "ToggleRealTimeStepsBinding", 0.2, 0.5, 300, 50);
        }
    }
}