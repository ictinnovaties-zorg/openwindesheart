using FormsControls.Base;
using Microcharts;
using Microcharts.Forms;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using WindesHeartApp.Data.Models;
using WindesHeartApp.Resources;
using WindesHeartApp.ViewModels;
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
        public static Label KilometersLabel;

        public static Button ToggleRealTimeStepsButton;

        public static Button Day1Button;
        public static Button Day2Button;
        public static Button Day3Button;
        public static Button Day4Button;
        public static Button Day5Button;
        public static Button Day6Button;
        public static Button TodayButton;

        public StepsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            BuildPage();

            Globals.StepsViewModel.OnAppearing();
        }

        private void BuildPage()
        {
            AbsoluteLayout absoluteLayout = new AbsoluteLayout();

            PageBuilder.BuildPageBasics(absoluteLayout, this);
            PageBuilder.AddHeaderImages(absoluteLayout);

            PageBuilder.AddLabel(absoluteLayout, "Steps", 0.10, 0.10, Globals.lighttextColor, "", 0);
            PageBuilder.AddReturnButton(absoluteLayout, this);

            ImageButton previousBtn = new ImageButton
            {
                Source = "arrow_left.png",
                BackgroundColor = Color.Transparent
            };
            AbsoluteLayout.SetLayoutFlags(previousBtn, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(previousBtn, new Rectangle(0.3, 0.135, 0.1, 0.1));
            previousBtn.SetBinding(Button.CommandProperty, new Binding() { Path = "PreviousDayBinding" });
            absoluteLayout.Children.Add(previousBtn);

            ImageButton nextBtn = new ImageButton
            {
                Source = "arrow_right.png",
                BackgroundColor = Color.Transparent
            };
            AbsoluteLayout.SetLayoutFlags(nextBtn, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(nextBtn, new Rectangle(0.7, 0.135, 0.1, 0.1));
            nextBtn.SetBinding(Button.CommandProperty, new Binding() { Path = "NextDayBinding" });
            absoluteLayout.Children.Add(nextBtn);

            CurrentDayLabel = PageBuilder.AddLabel(absoluteLayout, "Today", 0.5, 0.16, Color.Black, "", 0);
            CurrentDayLabel.FontSize = 15;

            CurrentStepsLabel = PageBuilder.AddLabel(absoluteLayout, "0", 0.5, 0.37, Color.Black, "", 0);
            CurrentStepsLabel.SetBinding(Label.TextProperty, new Binding("StepsLabelText"));
            CurrentStepsLabel.FontSize = 40;

            var label = PageBuilder.AddLabel(absoluteLayout, "STEPS", 0.5, 0.45, Color.Black, "", 0);
            label.FontSize = 20;

            ChartView chart = new ChartView
            {
                Rotation = 180
            };
            chart.SetBinding(ChartView.ChartProperty, "Chart");
            AbsoluteLayout.SetLayoutFlags(chart, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(chart, new Rectangle(0.5, 0.25, 0.60, 0.60));
            absoluteLayout.Children.Add(chart);

            var label2 = PageBuilder.AddLabel(absoluteLayout, "0 Kcal", 0.5, 0.65, Color.Black, "", 0);
            label2.FontSize = 20;

            KilometersLabel = PageBuilder.AddLabel(absoluteLayout, "0 Kilometers", 0.5, 0.73, Color.Black, "", 0);
            KilometersLabel.FontSize = 20;

            AddDayButtons(absoluteLayout);
        }

        private void AddDayButtons(AbsoluteLayout absoluteLayout)
        {
            DateTime today = DateTime.Now;
            DayOfWeek day = today.DayOfWeek;
            DateTimeFormatInfo dtfi = new CultureInfo("en-US").DateTimeFormat;
            float y = 0.85f;
            int fontSize = 12;
            int cornerRadius = 100;

            Day1Button = PageBuilder.AddButton(absoluteLayout, dtfi.GetAbbreviatedDayName(today.AddDays(-6).DayOfWeek), "Day1Binding", 0.05, y, 0.1, 0.05, AbsoluteLayoutFlags.All);
            Day1Button.CornerRadius = cornerRadius;
            Day1Button.FontSize = fontSize;

            Day2Button = PageBuilder.AddButton(absoluteLayout, dtfi.GetAbbreviatedDayName(today.AddDays(-5).DayOfWeek), "Day2Binding", 0.20, y, 0.1, 0.05, AbsoluteLayoutFlags.All);
            Day2Button.CornerRadius = cornerRadius;
            Day2Button.FontSize = fontSize;

            Day3Button = PageBuilder.AddButton(absoluteLayout, dtfi.GetAbbreviatedDayName(today.AddDays(-4).DayOfWeek), "Day3Binding", 0.35, y, 0.1, 0.05, AbsoluteLayoutFlags.All);
            Day3Button.CornerRadius = cornerRadius;
            Day3Button.FontSize = fontSize;

            Day4Button = PageBuilder.AddButton(absoluteLayout, dtfi.GetAbbreviatedDayName(today.AddDays(-3).DayOfWeek), "Day4Binding", 0.50, y, 0.1, 0.05, AbsoluteLayoutFlags.All);
            Day4Button.CornerRadius = cornerRadius;
            Day4Button.FontSize = fontSize;

            Day5Button = PageBuilder.AddButton(absoluteLayout, dtfi.GetAbbreviatedDayName(today.AddDays(-2).DayOfWeek), "Day5Binding", 0.65, y, 0.1, 0.05, AbsoluteLayoutFlags.All);
            Day5Button.CornerRadius = cornerRadius;
            Day5Button.FontSize = fontSize;

            Day6Button = PageBuilder.AddButton(absoluteLayout, dtfi.GetAbbreviatedDayName(today.AddDays(-1).DayOfWeek), "Day6Binding", 0.80, y, 0.1, 0.05, AbsoluteLayoutFlags.All);
            Day6Button.CornerRadius = cornerRadius;
            Day6Button.FontSize = fontSize;

            TodayButton = PageBuilder.AddButton(absoluteLayout, "Today", "TodayBinding", 0.95, y, 0.1, 0.05, AbsoluteLayoutFlags.All);
            TodayButton.CornerRadius = cornerRadius;
            TodayButton.FontSize = fontSize;
            TodayButton.IsEnabled = false;
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