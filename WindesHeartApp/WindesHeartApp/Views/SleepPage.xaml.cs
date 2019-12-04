
using FormsControls.Base;
using Microcharts.Forms;
using System;
using WindesHeartApp.Pages;
using WindesHeartApp.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WindesHeartApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SleepPage : ContentPage, IAnimationPage
    {
        public static Label CurrentDayLabel;

        public static Button Day1Button;
        public static Button Day2Button;
        public static Button Day3Button;
        public static Button Day4Button;
        public static Button Day5Button;
        public static Button Day6Button;
        public static Button TodayButton;

        public SleepPage()
        {
            InitializeComponent();
            BuildPage();
        }

        protected override void OnAppearing()
        {
            Globals.SleepViewModel.OnAppearing();
            TodayButton.IsEnabled = false;
        }

        private void BuildPage()
        {
            absoluteLayout = new AbsoluteLayout();

            PageBuilder.BuildPageBasics(absoluteLayout, this);
            PageBuilder.AddHeaderImages(absoluteLayout);

            PageBuilder.AddLabel(absoluteLayout, "Sleep", 0.10, 0.10, Globals.LightTextColor, "", 0);
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

            ChartView chart = new ChartView();
            chart.SetBinding(ChartView.ChartProperty, "Chart");
            AbsoluteLayout.SetLayoutFlags(chart, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(chart, new Rectangle(0.5, 0.45, 0.9, 0.5));
            absoluteLayout.Children.Add(chart);

            AddDayButtons(absoluteLayout);
        }

        private void AddDayButtons(AbsoluteLayout absoluteLayout)
        {
            DateTime today = DateTime.Now;
            Day1Button = PageBuilder.AddButton(absoluteLayout, today.AddDays(-6).DayOfWeek.ToString(), "Day1Binding", 0.05, 0.85f, 0.13, 0.1, 200, 11, AbsoluteLayoutFlags.All, Globals.SecondaryColor);
            Day2Button = PageBuilder.AddButton(absoluteLayout, today.AddDays(-5).DayOfWeek.ToString(), "Day2Binding", 0.20, 0.85f, 0.13, 0.1, 200, 11, AbsoluteLayoutFlags.All, Globals.SecondaryColor);
            Day3Button = PageBuilder.AddButton(absoluteLayout, today.AddDays(-4).DayOfWeek.ToString(), "Day3Binding", 0.35, 0.85f, 0.13, 0.1, 200, 11, AbsoluteLayoutFlags.All, Globals.SecondaryColor);
            Day4Button = PageBuilder.AddButton(absoluteLayout, today.AddDays(-3).DayOfWeek.ToString(), "Day4Binding", 0.50, 0.85f, 0.13, 0.1, 200, 11, AbsoluteLayoutFlags.All, Globals.SecondaryColor);
            Day5Button = PageBuilder.AddButton(absoluteLayout, today.AddDays(-2).DayOfWeek.ToString(), "Day5Binding", 0.65, 0.85f, 0.13, 0.1, 200, 11, AbsoluteLayoutFlags.All, Globals.SecondaryColor);
            Day6Button = PageBuilder.AddButton(absoluteLayout, today.AddDays(-1).DayOfWeek.ToString(), "Day6Binding", 0.80, 0.85f, 0.13, 0.1, 200, 11, AbsoluteLayoutFlags.All, Globals.SecondaryColor);
            TodayButton = PageBuilder.AddButton(absoluteLayout, "TODAY", "TodayBinding", 0.95, 0.85f, 0.13, 0.1, 200, 11, AbsoluteLayoutFlags.All, Globals.SecondaryColor);
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