using FormsControls.Base;
using Microcharts.Forms;
using System;
using System.Globalization;
using WindesHeartApp.Resources;
using WindesHeartSDK;
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
        public static Label CaloriesLabel;

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
            Globals.StepsPageViewModel.OnAppearing();
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
            previousBtn.Clicked += Globals.StepsPageViewModel.PreviousDayBtnClick;
            absoluteLayout.Children.Add(previousBtn);

            ImageButton nextBtn = new ImageButton
            {
                Source = "arrow_right.png",
                BackgroundColor = Color.Transparent
            };
            AbsoluteLayout.SetLayoutFlags(nextBtn, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(nextBtn, new Rectangle(0.7, 0.135, 0.1, 0.1));
            nextBtn.Clicked += Globals.StepsPageViewModel.NextDayBtnClick;
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

            CaloriesLabel = PageBuilder.AddLabel(absoluteLayout, "0 Calories", 0.5, 0.65, Color.Black, "", 0);
            CaloriesLabel.FontSize = 20;

            KilometersLabel = PageBuilder.AddLabel(absoluteLayout, "0 Kilometers", 0.5, 0.73, Color.Black, "", 0);
            KilometersLabel.FontSize = 20;

            AddDayButtons(absoluteLayout);

            #region refreshbutton
            Grid grid = new Grid
            {
            };
            Frame frame = new Frame
            {
                CornerRadius = 10,
                BorderColor = Color.White,
                BackgroundColor = Globals.SecondaryColor,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HasShadow = true
            };

            grid.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(execute: () => { RefreshButtonClicked(this, EventArgs.Empty); })
            });
            grid.Opacity = 20;
            AbsoluteLayout.SetLayoutBounds(grid, new Rectangle(0.15, 0.95, Globals.ScreenHeight / 100 * 10, Globals.ScreenHeight / 100 * 6));
            AbsoluteLayout.SetLayoutFlags(grid, AbsoluteLayoutFlags.PositionProportional);
            ImageButton RefreshButton = new ImageButton
            {
                Source = "Refresh.png",
                HorizontalOptions = LayoutOptions.Start,
                HeightRequest = Globals.ScreenHeight / 100 * 4.5,
                BackgroundColor = Color.Transparent,
            };
            RefreshButton.Margin = new Thickness(2, 0, 0, 0);
            RefreshButton.Clicked += RefreshButtonClicked;
            grid.Children.Add(frame);
            grid.Children.Add(RefreshButton); ;

            Label RefreshLabel = new Label() { Text = "data", VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.End, FontSize = 20, FontAttributes = FontAttributes.Italic };
            RefreshLabel.Margin = new Thickness(0, 0, 2, 0);
            grid.Children.Add(RefreshLabel);

            RefreshLabel.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(execute: () => { RefreshButtonClicked(this, EventArgs.Empty); }),
            });
            absoluteLayout.Children.Add(grid);
            #endregion
        }

        private void AddDayButtons(AbsoluteLayout absoluteLayout)
        {
            var culture = CultureInfo.CurrentCulture;

            int fontsize = 8;
            float height = 0.84f;

            DateTime today = DateTime.Now;

            int size = (int)(Globals.ScreenHeight / 100 * 8.0);
            Day1Button = PageBuilder.AddButton(absoluteLayout, culture.DateTimeFormat.GetAbbreviatedDayName(today.AddDays(-6).DayOfWeek), Globals.StepsPageViewModel.Day1BtnClick, 0.05, height, size, size, size / 2, fontsize, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            Day1Button.BorderColor = Color.Black;
            Day1Button.BorderWidth = 2;

            Day2Button = PageBuilder.AddButton(absoluteLayout, culture.DateTimeFormat.GetAbbreviatedDayName(today.AddDays(-5).DayOfWeek), Globals.StepsPageViewModel.Day2BtnClick, 0.20, height, size, size, size / 2, fontsize, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            Day2Button.BorderColor = Color.Black;
            Day2Button.BorderWidth = 2;

            Day3Button = PageBuilder.AddButton(absoluteLayout, culture.DateTimeFormat.GetAbbreviatedDayName(today.AddDays(-4).DayOfWeek), Globals.StepsPageViewModel.Day3BtnClick, 0.35, height, size, size, size / 2, fontsize, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            Day3Button.BorderColor = Color.Black;
            Day3Button.BorderWidth = 2;

            Day4Button = PageBuilder.AddButton(absoluteLayout, culture.DateTimeFormat.GetAbbreviatedDayName(today.AddDays(-3).DayOfWeek), Globals.StepsPageViewModel.Day4BtnClick, 0.50, height, size, size, size / 2, fontsize, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            Day4Button.BorderColor = Color.Black;
            Day4Button.BorderWidth = 2;

            Day5Button = PageBuilder.AddButton(absoluteLayout, culture.DateTimeFormat.GetAbbreviatedDayName(today.AddDays(-2).DayOfWeek), Globals.StepsPageViewModel.Day5BtnClick, 0.65, height, size, size, size / 2, fontsize, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            Day5Button.BorderColor = Color.Black;
            Day5Button.BorderWidth = 2;

            Day6Button = PageBuilder.AddButton(absoluteLayout, culture.DateTimeFormat.GetAbbreviatedDayName(today.AddDays(-1).DayOfWeek), Globals.StepsPageViewModel.Day6BtnClick, 0.80, height, size, size, size / 2, fontsize, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            Day6Button.BorderColor = Color.Black;
            Day6Button.BorderWidth = 2;

            TodayButton = PageBuilder.AddButton(absoluteLayout, culture.DateTimeFormat.GetAbbreviatedDayName(today.DayOfWeek), Globals.StepsPageViewModel.TodayBtnClick, 0.95, height, size, size, size / 2, fontsize, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            TodayButton.BorderColor = Color.Black;
            TodayButton.BorderWidth = 2;

        }

        private async void RefreshButtonClicked(object sender, EventArgs e)
        {
            if (Windesheart.PairedDevice == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error while refreshing data",
                    "Can only refresh data when connected to a device!", "Ok");
                return;
            }
            Application.Current.MainPage.Navigation.PopAsync();
            Globals.SamplesService.StartFetching();
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