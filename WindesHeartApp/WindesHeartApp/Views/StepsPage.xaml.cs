using FormsControls.Base;
using Microcharts.Forms;
using System;
using System.Globalization;
using WindesHeartApp.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WindesHeartApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StepsPage : ContentPage, IAnimationPage
    {
        public static Label CurrentStepsLabel;
        public static Label CurrentDayLabel;
        public static Label KilometersLabel;
        public static Label KcalLabel;

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
            BuildPage();
        }

        protected override void OnAppearing()
        {
            Globals.StepsViewModel.OnAppearing();
        }

        private void BuildPage()
        {
            absoluteLayout = new AbsoluteLayout();

            PageBuilder.BuildPageBasics(absoluteLayout, this);
            PageBuilder.AddHeaderImages(absoluteLayout);

            PageBuilder.AddLabel(absoluteLayout, "Steps", 0.10, 0.10, Globals.LightTextColor, "", 0);
            PageBuilder.AddReturnButton(absoluteLayout, this);

            ImageButton previousBtn = new ImageButton
            {
                Source = "arrow_left.png",
                BackgroundColor = Color.Transparent
            };
            AbsoluteLayout.SetLayoutFlags(previousBtn, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(previousBtn, new Rectangle(0.3, 0.135, 0.1, 0.1));
            previousBtn.Clicked += Globals.StepsViewModel.PreviousDayBtnClick;
            absoluteLayout.Children.Add(previousBtn);

            ImageButton nextBtn = new ImageButton
            {
                Source = "arrow_right.png",
                BackgroundColor = Color.Transparent
            };
            AbsoluteLayout.SetLayoutFlags(nextBtn, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(nextBtn, new Rectangle(0.7, 0.135, 0.1, 0.1));
            nextBtn.Clicked += Globals.StepsViewModel.NextDayBtnClick;
            absoluteLayout.Children.Add(nextBtn);

            CurrentDayLabel = PageBuilder.AddLabel(absoluteLayout, "Today", 0.5, 0.16, Color.Black, "", 15);

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

            KcalLabel = PageBuilder.AddLabel(absoluteLayout, "0 Kcal", 0.5, 0.65, Color.Black, "", 0);
            KcalLabel.FontSize = 20;

            KilometersLabel = PageBuilder.AddLabel(absoluteLayout, "0 Kilometers", 0.5, 0.73, Color.Black, "", 0);
            KilometersLabel.FontSize = 20;

            AddDayButtons(absoluteLayout);
        }

        private void AddDayButtons(AbsoluteLayout absoluteLayout)
        {
            var culture = CultureInfo.CurrentCulture;

            int fontsize = 14;
            float height = 0.84f;

            DateTime today = DateTime.Now;

            int size = (int)(Globals.ScreenHeight / 100 * 8.0);
            Day1Button = PageBuilder.AddButton(absoluteLayout, culture.DateTimeFormat.GetAbbreviatedDayName(today.AddDays(-6).DayOfWeek), Globals.StepsViewModel.Day1BtnClick, 0.05, height, size, size, size / 2, fontsize, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            Day1Button.BorderColor = Color.Black;
            Day1Button.BorderWidth = 2;

            Day2Button = PageBuilder.AddButton(absoluteLayout, culture.DateTimeFormat.GetAbbreviatedDayName(today.AddDays(-5).DayOfWeek), Globals.StepsViewModel.Day2BtnClick, 0.20, height, size, size, size / 2, fontsize, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            Day2Button.BorderColor = Color.Black;
            Day2Button.BorderWidth = 2;

            Day3Button = PageBuilder.AddButton(absoluteLayout, culture.DateTimeFormat.GetAbbreviatedDayName(today.AddDays(-4).DayOfWeek), Globals.StepsViewModel.Day3BtnClick, 0.35, height, size, size, size / 2, fontsize, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            Day3Button.BorderColor = Color.Black;
            Day3Button.BorderWidth = 2;

            Day4Button = PageBuilder.AddButton(absoluteLayout, culture.DateTimeFormat.GetAbbreviatedDayName(today.AddDays(-3).DayOfWeek), Globals.StepsViewModel.Day4BtnClick, 0.50, height, size, size, size / 2, fontsize, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            Day4Button.BorderColor = Color.Black;
            Day4Button.BorderWidth = 2;

            Day5Button = PageBuilder.AddButton(absoluteLayout, culture.DateTimeFormat.GetAbbreviatedDayName(today.AddDays(-2).DayOfWeek), Globals.StepsViewModel.Day5BtnClick, 0.65, height, size, size, size / 2, fontsize, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            Day5Button.BorderColor = Color.Black;
            Day5Button.BorderWidth = 2;

            Day6Button = PageBuilder.AddButton(absoluteLayout, culture.DateTimeFormat.GetAbbreviatedDayName(today.AddDays(-1).DayOfWeek), Globals.StepsViewModel.Day6BtnClick, 0.80, height, size, size, size / 2, fontsize, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            Day6Button.BorderColor = Color.Black;
            Day6Button.BorderWidth = 2;

            TodayButton = PageBuilder.AddButton(absoluteLayout, culture.DateTimeFormat.GetAbbreviatedDayName(today.DayOfWeek), Globals.StepsViewModel.TodayBtnClick, 0.95, height, size, size, size / 2, fontsize, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            TodayButton.BorderColor = Color.Black;
            TodayButton.BorderWidth = 2;

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