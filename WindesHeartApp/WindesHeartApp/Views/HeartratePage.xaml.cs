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

            var averageHeartrateLabel =
                PageBuilder.AddLabel(absoluteLayout, "", 0.5, 0.65, Color.Black, "AverageLabelText", 0);
            averageHeartrateLabel.FontSize = 20;

            var peakHeartrateLabel =
                PageBuilder.AddLabel(absoluteLayout, "", 0.5, 0.73, Color.Black, "PeakHeartrateText", 0);
            peakHeartrateLabel.FontSize = 20;

            #region heartrateinterval selector
            Image heartonlyImage2 = new Image { Source = "HeartOnlyTransparent.png" };
            AbsoluteLayout.SetLayoutFlags(heartonlyImage2, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(heartonlyImage2, new Rectangle(0.20, 0.80, 50, 50));
            PageBuilder.AddLabel(absoluteLayout, "Interval", 0.40, 0.80, Color.Black, "", 12);

            interval15Button = PageBuilder.AddButton(absoluteLayout, "15", "", 0.20, 0.90, 50, 50,
                AbsoluteLayoutFlags.PositionProportional);
            interval15Button.GestureRecognizers.Add(new TapGestureRecognizer(view => { OnIntervalLabelClicked(interval15Button); }));
            interval15Button.CornerRadius = 25;
            interval15Button.BorderWidth = 1;
            interval30Button = PageBuilder.AddButton(absoluteLayout, "30", "", 0.40, 0.90, 50, 50,
                AbsoluteLayoutFlags.PositionProportional);
            interval30Button.GestureRecognizers.Add(new TapGestureRecognizer(view => { OnIntervalLabelClicked(interval30Button); }));
            interval30Button.CornerRadius = 25;
            interval30Button.BorderWidth = 1;
            interval45Button = PageBuilder.AddButton(absoluteLayout, "40", "", 0.60, 0.90, 50, 50,
                AbsoluteLayoutFlags.PositionProportional);
            interval45Button.GestureRecognizers.Add(new TapGestureRecognizer(view => { OnIntervalLabelClicked(interval45Button); }));
            interval45Button.CornerRadius = 25;
            interval45Button.BorderWidth = 1;
            interval60Button = PageBuilder.AddButton(absoluteLayout, "60", "", 0.80, 0.90, 50, 50,
               AbsoluteLayoutFlags.PositionProportional);
            interval60Button.BorderWidth = 1;
            interval60Button.GestureRecognizers.Add(new TapGestureRecognizer(view => { OnIntervalLabelClicked(interval60Button); }));
            interval60Button.CornerRadius = 25;

            switch (Globals.heartrateviewModel.Interval)
            {
                case 15:
                    interval15Button.BorderColor = Color.Black;
                    interval30Button.BorderColor = Color.White;
                    interval45Button.BorderColor = Color.White;
                    interval60Button.BorderColor = Color.White;
                    break;
                case 30:
                    interval15Button.BorderColor = Color.White;
                    interval30Button.BorderColor = Color.Black;
                    interval45Button.BorderColor = Color.White;
                    interval60Button.BorderColor = Color.White;
                    break;
                case 45:
                    interval15Button.BorderColor = Color.White;
                    interval30Button.BorderColor = Color.White;
                    interval45Button.BorderColor = Color.Black;
                    interval60Button.BorderColor = Color.White;
                    break;
                case 60:
                    interval15Button.BorderColor = Color.White;
                    interval30Button.BorderColor = Color.White;
                    interval45Button.BorderColor = Color.White;
                    interval60Button.BorderColor = Color.Black;
                    break;
            }
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

