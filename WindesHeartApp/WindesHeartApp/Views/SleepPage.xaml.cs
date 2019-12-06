
using FormsControls.Base;
using Microcharts.Forms;
using System;
using System.Globalization;
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

            PageBuilder.AddLabel(absoluteLayout, "Sleep", 0.09, 0.10, Globals.LightTextColor, "", 0);
            PageBuilder.AddReturnButton(absoluteLayout, this);

            ImageButton previousBtn = new ImageButton
            {
                Source = "arrow_left.png",
                BackgroundColor = Color.Transparent
            };
            AbsoluteLayout.SetLayoutFlags(previousBtn, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(previousBtn, new Rectangle(0.3, 0.175, 0.1, 0.1));
            previousBtn.SetBinding(Button.CommandProperty, new Binding() { Path = "PreviousDayBinding" });
            absoluteLayout.Children.Add(previousBtn);

            ImageButton nextBtn = new ImageButton
            {
                Source = "arrow_right.png",
                BackgroundColor = Color.Transparent
            };
            AbsoluteLayout.SetLayoutFlags(nextBtn, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(nextBtn, new Rectangle(0.7, 0.175, 0.1, 0.1));
            nextBtn.SetBinding(Button.CommandProperty, new Binding() { Path = "NextDayBinding" });
            absoluteLayout.Children.Add(nextBtn);

            CurrentDayLabel = PageBuilder.AddLabel(absoluteLayout, "Today", 0.5, 0.2, Color.Black, "", 0);
            CurrentDayLabel.FontSize = 15;

            BoxView awakeRectangle = new BoxView();
            awakeRectangle.Color = Color.FromHex(Globals.SleepViewModel.AwakeColor);
            AbsoluteLayout.SetLayoutFlags(awakeRectangle, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(awakeRectangle, new Rectangle(0.1, 0.3, 20, 20));
            absoluteLayout.Children.Add(awakeRectangle);

            PageBuilder.AddLabel(absoluteLayout, "Awake", 0.18, 0.3, Color.Black, "", 14);


            BoxView lightRectangle = new BoxView();
            lightRectangle.Color = Color.FromHex(Globals.SleepViewModel.LightColor);
            AbsoluteLayout.SetLayoutFlags(lightRectangle, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(lightRectangle, new Rectangle(0.39, 0.3, 20, 20));
            absoluteLayout.Children.Add(lightRectangle);

            PageBuilder.AddLabel(absoluteLayout, "Light sleep", 0.52, 0.3, Color.Black, "", 14);

            BoxView deepRectangle = new BoxView();
            deepRectangle.Color = Color.FromHex(Globals.SleepViewModel.DeepColor);
            AbsoluteLayout.SetLayoutFlags(deepRectangle, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(deepRectangle, new Rectangle(0.7, 0.3, 20, 20));
            absoluteLayout.Children.Add(deepRectangle);

            PageBuilder.AddLabel(absoluteLayout, "Deep sleep", 0.87, 0.3, Color.Black, "", 14);

            ChartView chart = new ChartView();
            chart.SetBinding(ChartView.ChartProperty, "Chart");
            AbsoluteLayout.SetLayoutFlags(chart, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(chart, new Rectangle(0.5, 0.48, 0.9, 0.26));
            absoluteLayout.Children.Add(chart);

            //Add hour labels
            int starthour = 20;
            for (int i = starthour; i <= 36; i += 2)
            {
                int hour = i;
                if (i > 24) hour = i - 24;

                PageBuilder.AddLabel(absoluteLayout, hour.ToString(), 0.022 + 0.059 * (i - starthour), 0.65, Color.Black, "", 17);

            }
            AddDayButtons(absoluteLayout);
        }

        private void AddDayButtons(AbsoluteLayout absoluteLayout)
        {
            var culture = CultureInfo.CurrentCulture;

            int fontsize = 14;
            int size = 55;
            float height = 0.80f;

            DateTime today = DateTime.Now;
            Day1Button = PageBuilder.AddButton(absoluteLayout, culture.DateTimeFormat.GetAbbreviatedDayName(today.AddDays(-6).DayOfWeek), "Day1Binding", 0.05, height, size, size, 200, fontsize, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            Day1Button.BorderColor = Color.Black;
            Day1Button.BorderWidth = 2;

            Day2Button = PageBuilder.AddButton(absoluteLayout, culture.DateTimeFormat.GetAbbreviatedDayName(today.AddDays(-5).DayOfWeek), "Day2Binding", 0.20, height, size, size, 200, fontsize, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            Day2Button.BorderColor = Color.Black;
            Day2Button.BorderWidth = 2;

            Day3Button = PageBuilder.AddButton(absoluteLayout, culture.DateTimeFormat.GetAbbreviatedDayName(today.AddDays(-4).DayOfWeek), "Day3Binding", 0.35, height, size, size, 200, fontsize, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            Day3Button.BorderColor = Color.Black;
            Day3Button.BorderWidth = 2;

            Day4Button = PageBuilder.AddButton(absoluteLayout, culture.DateTimeFormat.GetAbbreviatedDayName(today.AddDays(-3).DayOfWeek), "Day4Binding", 0.50, height, size, size, 200, fontsize, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            Day4Button.BorderColor = Color.Black;
            Day4Button.BorderWidth = 2;

            Day5Button = PageBuilder.AddButton(absoluteLayout, culture.DateTimeFormat.GetAbbreviatedDayName(today.AddDays(-2).DayOfWeek), "Day5Binding", 0.65, height, size, size, 200, fontsize, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            Day5Button.BorderColor = Color.Black;
            Day5Button.BorderWidth = 2;

            Day6Button = PageBuilder.AddButton(absoluteLayout, culture.DateTimeFormat.GetAbbreviatedDayName(today.AddDays(-1).DayOfWeek), "Day6Binding", 0.80, height, size, size, 200, fontsize, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            Day6Button.BorderColor = Color.Black;
            Day6Button.BorderWidth = 2;

            TodayButton = PageBuilder.AddButton(absoluteLayout, culture.DateTimeFormat.GetAbbreviatedDayName(today.DayOfWeek), "TodayBinding", 0.95, height, size, size, 200, fontsize, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
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