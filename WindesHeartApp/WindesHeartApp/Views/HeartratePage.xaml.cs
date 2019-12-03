using FormsControls.Base;
using Microcharts;
using Microcharts.Forms;
using SkiaSharp;
using System;
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

            var previousBtn = PageBuilder.AddButton(absoluteLayout, "Previous", "PreviousDayBinding", 0.1, 0.15, 0.25,
                0.07, AbsoluteLayoutFlags.All);
            previousBtn.FontSize = 12;

            var nextBtn = PageBuilder.AddButton(absoluteLayout, "Next", "NextDayBinding", 0.9, 0.15, 0.25, 0.07,
                AbsoluteLayoutFlags.All);
            nextBtn.FontSize = 12;

            var entries = new List<Microcharts.Entry>();

            Microcharts.Entry entry = new Microcharts.Entry(200)
            { ValueLabel = "200", TextColor = SKColor.Parse("#266489") };
            entries.Add(entry);
            Microcharts.Entry entry2 = new Microcharts.Entry(400)
            { ValueLabel = "400", TextColor = SKColor.Parse("#266489") };
            entries.Add(entry2);
            Microcharts.Entry entry3 = new Microcharts.Entry(10)
            { ValueLabel = "-100", TextColor = SKColor.Parse("#266489") };
            entries.Add(entry3);
            Microcharts.Entry entry4 = new Microcharts.Entry(50)
            { ValueLabel = "50", TextColor = SKColor.Parse("#266489") };
            entries.Add(entry4);



            var chart = new LineChart() { Entries = entries };

            var view = new ChartView { Chart = chart };
            AbsoluteLayout.SetLayoutBounds(view, new Rectangle(0.5, 0.5, 0.5, 0.3));
            AbsoluteLayout.SetLayoutFlags(view, AbsoluteLayoutFlags.All);

            absoluteLayout.Children.Add(view);

            var averageHeartrateLabel = PageBuilder.AddLabel(absoluteLayout, "", 0.5, 0.65, Color.Black, "AverageLabelText", 0);
            averageHeartrateLabel.FontSize = 20;

            var peakHeartrateLabel = PageBuilder.AddLabel(absoluteLayout, "", 0.5, 0.73, Color.Black, "PeakHeartrateText", 0);
            peakHeartrateLabel.FontSize = 20;

            DateTime today = DateTime.Now;
            float y = 0.85f;
            var Day1Button = PageBuilder.AddButton(absoluteLayout, today.AddDays(-6).DayOfWeek.ToString(),
                "Day1Binding", 0.05, y, 0.15, 0.1, AbsoluteLayoutFlags.All);
            var Day2Button = PageBuilder.AddButton(absoluteLayout, today.AddDays(-5).DayOfWeek.ToString(),
                "Day2Binding", 0.20, y, 0.15, 0.1, AbsoluteLayoutFlags.All);
            var Day3Button = PageBuilder.AddButton(absoluteLayout, today.AddDays(-4).DayOfWeek.ToString(),
                "Day3Binding", 0.35, y, 0.15, 0.1, AbsoluteLayoutFlags.All);
            var Day4Button = PageBuilder.AddButton(absoluteLayout, today.AddDays(-3).DayOfWeek.ToString(),
                "Day4Binding", 0.50, y, 0.15, 0.1, AbsoluteLayoutFlags.All);
            var Day5Button = PageBuilder.AddButton(absoluteLayout, today.AddDays(-2).DayOfWeek.ToString(),
                "Day5Binding", 0.65, y, 0.15, 0.1, AbsoluteLayoutFlags.All);
            var Day6Button = PageBuilder.AddButton(absoluteLayout, today.AddDays(-1).DayOfWeek.ToString(),
                "Day6Binding", 0.80, y, 0.15, 0.1, AbsoluteLayoutFlags.All);
            var TodayButton = PageBuilder.AddButton(absoluteLayout, "TODAY", "TodayBinding", 0.95, y, 0.15, 0.1,
                AbsoluteLayoutFlags.All);
            TodayButton.IsEnabled = false;


        }

        public IPageAnimation PageAnimation { get; } = new SlidePageAnimation
        { Duration = AnimationDuration.Short, Subtype = AnimationSubtype.FromTop };

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

