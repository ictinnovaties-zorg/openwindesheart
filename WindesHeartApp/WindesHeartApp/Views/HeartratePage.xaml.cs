using FormsControls.Base;
using SkiaSharp;
using System.Collections.Generic;
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
            PageBuilder.AddLabel(absoluteLayout, "Heartrate", 0.05, 0.10, Globals.LightTextColor, "", 0);
            PageBuilder.AddReturnButton(absoluteLayout, this);

            PageBuilder.AddButton(absoluteLayout, "add", "AddButtonCommand", 0.5, 0.5, 0.2, 0.05,
                AbsoluteLayoutFlags.All);
            PageBuilder.AddButton(absoluteLayout, "get", "GetButtonCommand", 0.5, 0.8, 0.2, 0.05,
                AbsoluteLayoutFlags.All);
            PageBuilder.AddLabel(absoluteLayout, "LOL", 0.5, 0.3, Color.Black, "SwagLabel", 15);


            var entries = new List<Microcharts.Entry>();

            Microcharts.Entry entry = new Microcharts.Entry(200) { ValueLabel = "200", TextColor = SKColor.Parse("#266489") };
            entries.Add(entry);
            Microcharts.Entry entry2 = new Microcharts.Entry(400) { ValueLabel = "400", TextColor = SKColor.Parse("#266489") };
            entries.Add(entry2);
            Microcharts.Entry entry3 = new Microcharts.Entry(10) { ValueLabel = "-100", TextColor = SKColor.Parse("#266489") };
            entries.Add(entry3);

        }
        public IPageAnimation PageAnimation { get; } = new SlidePageAnimation { Duration = AnimationDuration.Short, Subtype = AnimationSubtype.FromTop };

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