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
        public Button interval15Button;
        public Button interval30Button;
        public Button interval45Button;
        public Button interval60Button;
        public HeartratePage()
        {
            InitializeComponent();
            BuildPage();
        }

        protected override void OnAppearing()
        {
            Globals.heartrateviewModel.InitChart();
            Globals.heartrateviewModel.InitLabels();
        }

        private void BuildPage()
        {
            absoluteLayout = new AbsoluteLayout();
            PageBuilder.BuildPageBasics(absoluteLayout, this);
            PageBuilder.AddHeaderImages(absoluteLayout);
            PageBuilder.AddLabel(absoluteLayout, "Heartrate", 0.05, 0.10, Globals.LightTextColor, "", 0);
            PageBuilder.AddReturnButton(absoluteLayout, this);

            var previousBtn = PageBuilder.AddButton(absoluteLayout, "Previous", Globals.heartrateviewModel.PreviousDayBtnClick, 0.1, 0.15, 0.25, 0.07, 0, 12, AbsoluteLayoutFlags.All, Globals.SecondaryColor);

            var nextBtn = PageBuilder.AddButton(absoluteLayout, "Next", Globals.heartrateviewModel.NextDayBtnClick, 0.9, 0.15, 0.25, 0.07, 0, 12, AbsoluteLayoutFlags.All, Globals.SecondaryColor);
            nextBtn.FontSize = 12;

            ChartView chart = new ChartView();
            chart.SetBinding(ChartView.ChartProperty, "Chart");
            AbsoluteLayout.SetLayoutBounds(chart, new Rectangle(0.5, 0.4, 0.95, 0.35));
            AbsoluteLayout.SetLayoutFlags(chart, AbsoluteLayoutFlags.All);

            absoluteLayout.Children.Add(chart);

            var averageHeartrateLabel =
                PageBuilder.AddLabel(absoluteLayout, "", 0.5, 0.65, Color.Black, "AverageLabelText", 20);

            var peakHeartrateLabel =
                PageBuilder.AddLabel(absoluteLayout, "", 0.5, 0.73, Color.Black, "PeakHeartrateText", 20);

            #region heartrateinterval selector
            Image heartonlyImage2 = new Image { Source = "HeartOnlyTransparent.png" };
            AbsoluteLayout.SetLayoutFlags(heartonlyImage2, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(heartonlyImage2, new Rectangle(0.20, 0.80, 40, 40));
            absoluteLayout.Children.Add(heartonlyImage2);
            PageBuilder.AddLabel(absoluteLayout, "Interval", 0.50, 0.80, Color.Black, "", 15);

            interval15Button = PageBuilder.AddButton(absoluteLayout, "15", null, 0.20, 0.90, 50, 50, 25, 0, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            interval15Button.BorderWidth = 1;
            interval15Button.BorderColor = Globals.heartrateviewModel.Interval == 15 ? Color.Black : Color.White;
            interval15Button.Clicked += async (s, e) => OnIntervalLabelClicked(interval15Button);

            interval30Button = PageBuilder.AddButton(absoluteLayout, "30", null, 0.40, 0.90, 50, 50, 25, 0, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            interval30Button.BorderWidth = 1;
            interval30Button.BorderColor = Globals.heartrateviewModel.Interval == 30 ? Color.Black : Color.White;
            interval30Button.Clicked += async (s, e) => OnIntervalLabelClicked(interval30Button);

            interval45Button = PageBuilder.AddButton(absoluteLayout, "45", null, 0.60, 0.90, 50, 50, 25, 0, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            interval45Button.BorderWidth = 1;
            interval45Button.BorderColor = Globals.heartrateviewModel.Interval == 45 ? Color.Black : Color.White;
            interval45Button.Clicked += async (s, e) => OnIntervalLabelClicked(interval45Button);

            interval60Button = PageBuilder.AddButton(absoluteLayout, "60", null, 0.80, 0.90, 50, 50, 25, 0, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            interval60Button.BorderWidth = 1;
            interval60Button.BorderColor = Globals.heartrateviewModel.Interval == 60 ? Color.Black : Color.White;
            interval60Button.Clicked += async (s, e) => { OnIntervalLabelClicked(interval60Button); };
            #endregion
        }

        private async void OnIntervalLabelClicked(Button intervalLabel)
        {
            interval15Button.BorderColor = Color.White;
            interval30Button.BorderColor = Color.White;
            interval45Button.BorderColor = Color.White;
            interval60Button.BorderColor = Color.White;
            intervalLabel.BorderColor = Color.Black;

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

