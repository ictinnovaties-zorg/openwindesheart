using FormsControls.Base;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using WindesHeartApp.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Microcharts.Entry;

namespace WindesHeartApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StepsPage : ContentPage, IAnimationPage
    {
        public static Label CurrentStepsLabel;
        public static Label CurrentDayLabel;
        public static Button ToggleRealTimeStepsButton;

        public static Button Day1Button;
        public static Button Day2Button;
        public static Button Day3Button;
        public static Button Day4Button;
        public static Button Day5Button;
        public static Button Day6Button;
        public static Button TodayButton;

        private readonly List<Entry> Entries = new List<Entry>();

        public StepsPage()
        {
            InitializeComponent();
            BuildPage();
            FillChart(80, 100);
            chartView.Chart = new DonutChart
            {
                Entries = Entries,
                BackgroundColor = SKColors.Transparent,
                HoleRadius = 0.7f
            };
            chartView.Rotation = 180;
        }

        public void FillChart(int stepCount, int goal)
        {
            float percentageDone = (float)stepCount / (float)goal;
            Entries.Add(new Entry(percentageDone) { Color = SKColors.Black });

            //If goal not reached, fill other part transparent
            if (percentageDone < 1)
            {
                float percentageLeft = 1 - percentageDone;
                Entries.Add(new Entry(percentageLeft) { Color = SKColors.Transparent });
            }
        }

        private void BuildPage()
        {
            absoluteLayout = new AbsoluteLayout();

            PageBuilder.BuildPageBasics(absoluteLayout, this);
            PageBuilder.AddHeaderImages(absoluteLayout);

            PageBuilder.AddLabel(absoluteLayout, "Steps", 0.10, 0.10, Globals.LightTextColor, "", 0);
            PageBuilder.AddReturnButton(absoluteLayout, this);

            var previousBtn = PageBuilder.AddButton(absoluteLayout, "Previous", "PreviousDayBinding", 0.1, 0.15, 0.25, 0.07, 0, 12, AbsoluteLayoutFlags.All, Globals.SecondaryColor);

            var nextBtn = PageBuilder.AddButton(absoluteLayout, "Next", "NextDayBinding", 0.9, 0.15, 0.25, 0.07, 0, 12, AbsoluteLayoutFlags.All, Globals.SecondaryColor);

            CurrentDayLabel = PageBuilder.AddLabel(absoluteLayout, "Today", 0.5, 0.16, Color.Black, "", 0);
            CurrentDayLabel.FontSize = 15;

            CurrentStepsLabel = PageBuilder.AddLabel(absoluteLayout, "0", 0.5, 0.37, Color.Black, "", 0);
            CurrentStepsLabel.SetBinding(Label.TextProperty, new Binding("StepsLabelText"));
            CurrentStepsLabel.FontSize = 40;

            var label = PageBuilder.AddLabel(absoluteLayout, "STEPS", 0.5, 0.45, Color.Black, "", 0);
            label.FontSize = 20;

            //PageBuilder.AddButton(absoluteLayout, "Get steps", "GetStepsBinding", 0.2, 0.28, 300, 50);
            //ToggleRealTimeStepsButton = PageBuilder.AddButton(absoluteLayout, "Enable realtime steps", "ToggleRealTimeStepsBinding", 0.2, 0.38, 300, 50);

            AbsoluteLayout.SetLayoutFlags(chartView, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(chartView, new Rectangle(0.5, 0.20, 0.65, 0.65));
            absoluteLayout.Children.Add(chartView);

            var label2 = PageBuilder.AddLabel(absoluteLayout, "0 Kcal", 0.5, 0.65, Color.Black, "", 0);
            label2.FontSize = 20;

            var label3 = PageBuilder.AddLabel(absoluteLayout, "0 Kilometers", 0.5, 0.73, Color.Black, "", 0);
            label3.FontSize = 20;

            AddDayButtons(absoluteLayout);
        }

        private void AddDayButtons(AbsoluteLayout absoluteLayout)
        {
            DateTime today = DateTime.Now;
            float y = 0.85f;
            Day1Button = PageBuilder.AddButton(absoluteLayout, today.AddDays(-6).DayOfWeek.ToString(), "Day1Binding", 0.05, y, 0.15, 0.1, 0, 0, AbsoluteLayoutFlags.All, Globals.SecondaryColor);
            Day2Button = PageBuilder.AddButton(absoluteLayout, today.AddDays(-5).DayOfWeek.ToString(), "Day2Binding", 0.20, y, 0.15, 0.1, 0, 0, AbsoluteLayoutFlags.All, Globals.SecondaryColor);
            Day3Button = PageBuilder.AddButton(absoluteLayout, today.AddDays(-4).DayOfWeek.ToString(), "Day3Binding", 0.35, y, 0.15, 0.1, 0, 0, AbsoluteLayoutFlags.All, Globals.SecondaryColor);
            Day4Button = PageBuilder.AddButton(absoluteLayout, today.AddDays(-3).DayOfWeek.ToString(), "Day4Binding", 0.50, y, 0.15, 0.1, 0, 0, AbsoluteLayoutFlags.All, Globals.SecondaryColor);
            Day5Button = PageBuilder.AddButton(absoluteLayout, today.AddDays(-2).DayOfWeek.ToString(), "Day5Binding", 0.65, y, 0.15, 0.1, 0, 0, AbsoluteLayoutFlags.All, Globals.SecondaryColor);
            Day6Button = PageBuilder.AddButton(absoluteLayout, today.AddDays(-1).DayOfWeek.ToString(), "Day6Binding", 0.80, y, 0.15, 0.1, 0, 0, AbsoluteLayoutFlags.All, Globals.SecondaryColor);
            TodayButton = PageBuilder.AddButton(absoluteLayout, "TODAY", "TodayBinding", 0.95, y, 0.15, 0.1, 0, 0, AbsoluteLayoutFlags.All, Globals.SecondaryColor);
            TodayButton.IsEnabled = false;
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