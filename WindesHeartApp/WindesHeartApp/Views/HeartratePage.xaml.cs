using FormsControls.Base;
using WindesHeartApp.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WindesHeartApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeartratePage : ContentPage, IAnimationPage
    {
        public HeartratePage()
        {
            InitializeComponent();
            BuildPage();
        }

        private void BuildPage()
        {
            absoluteLayout = new AbsoluteLayout();
            PageBuilder.BuildPageBasics(absoluteLayout, this);
            PageBuilder.AddHeaderImages(absoluteLayout);
            PageBuilder.AddLabel(absoluteLayout, "Heartrate", 0.05, 0.10, Globals.lighttextColor, "", 0);
            PageBuilder.AddReturnButton(absoluteLayout, this);

            PageBuilder.AddButton(absoluteLayout, "add", "addButtonCommand", 0.5, 0.5, 0.2, 0.05,
                AbsoluteLayoutFlags.All);
            PageBuilder.AddButton(absoluteLayout, "get", "getButtonCommand", 0.5, 0.8, 0.2, 0.05,
                AbsoluteLayoutFlags.All);
            PageBuilder.AddLabel(absoluteLayout, "LOL", 0.5, 0.3, Color.Black, "swagLabel", 15);



        }
        public IPageAnimation PageAnimation { get; } = new SlidePageAnimation { Duration = AnimationDuration.Long, Subtype = AnimationSubtype.FromTop };

        public void OnAnimationStarted(bool isPopAnimation)
        {
            // Put your code here but leaving empty works just fine
        }

        public void OnAnimationFinished(bool isPopAnimation)
        {
            // Put your code here but leaving empty works just fine
        }
    }
}