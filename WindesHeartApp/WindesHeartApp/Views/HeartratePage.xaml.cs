using FormsControls.Base;
using Microcharts.Forms;
using System;
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
            Globals.heartrateviewModel.InitChart();
            Globals.heartrateviewModel.InitLabels();
            Globals.heartrateviewModel._dateTime = DateTime.Now.AddHours(-24);
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

            ChartView chart = new ChartView();
            chart.SetBinding(ChartView.ChartProperty, "Chart");
            AbsoluteLayout.SetLayoutBounds(chart, new Rectangle(0.5, 0.4, 0.95, 0.35));
            AbsoluteLayout.SetLayoutFlags(chart, AbsoluteLayoutFlags.All);

            absoluteLayout.Children.Add(chart);

            var averageHeartrateLabel = PageBuilder.AddLabel(absoluteLayout, "", 0.5, 0.65, Color.Black, "AverageLabelText", 0);
            averageHeartrateLabel.FontSize = 20;

            var peakHeartrateLabel = PageBuilder.AddLabel(absoluteLayout, "", 0.5, 0.73, Color.Black, "PeakHeartrateText", 0);
            peakHeartrateLabel.FontSize = 20;

            #region heartrateinterval selector
            BoxView circle = new BoxView();
            circle.WidthRequest = 100;
            circle.HeightRequest = 100;
            circle.CornerRadius = 50;
            circle.BackgroundColor = Color.Orange;
            circle.Color = Color.Blue;
            absoluteLayout.Children.Add(circle);
            AbsoluteLayout.SetLayoutBounds(circle, new Rectangle(0.20, 0.85, 100, 100));
            AbsoluteLayout.SetLayoutFlags(circle, AbsoluteLayoutFlags.PositionProportional);

            var intervalLabel1 = PageBuilder.AddLabel(absoluteLayout, "15", 0.20, 0.85, Color.Black, "", 15);
            intervalLabel1.GestureRecognizers.Add(new TapGestureRecognizer((view) => OnIntervalLabelClicked(intervalLabel1)));
            var intervalLabel2 = PageBuilder.AddLabel(absoluteLayout, "30", 0.40, 0.85, Color.Black, "", 15);
            intervalLabel2.GestureRecognizers.Add(new TapGestureRecognizer((view) => OnIntervalLabelClicked(intervalLabel2)));
            var intervalLabel3 = PageBuilder.AddLabel(absoluteLayout, "45", 0.60, 0.85, Color.Black, "", 15);
            intervalLabel3.GestureRecognizers.Add(new TapGestureRecognizer((view) => OnIntervalLabelClicked(intervalLabel3)));
            var intervalLabel4 = PageBuilder.AddLabel(absoluteLayout, "60", 0.80, 0.85, Color.Black, "", 15);
            intervalLabel4.GestureRecognizers.Add(new TapGestureRecognizer((view) => OnIntervalLabelClicked(intervalLabel4)));
            #endregion
        }

        private async void OnIntervalLabelClicked(Label intervalLabel)
        {
            var interval = Convert.ToInt32(intervalLabel.Text);
            await DisplayAlert("Heartrate", $"Changed heartrate measurement interval to {interval}", "OK");
            Globals.heartrateviewModel.UpdateInterval(interval);

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

