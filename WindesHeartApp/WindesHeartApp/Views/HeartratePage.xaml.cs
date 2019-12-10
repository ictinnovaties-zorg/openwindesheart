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
        public Button intervaldefaultButton;
        public HeartratePage()
        {
            InitializeComponent();
            BuildPage();
        }

        protected override void OnAppearing()
        {
            Globals.HeartratePageViewModel.OnAppearing();
        }


        private void BuildPage()
        {
            absoluteLayout = new AbsoluteLayout();
            PageBuilder.BuildPageBasics(absoluteLayout, this);
            PageBuilder.AddHeaderImages(absoluteLayout);
            PageBuilder.AddLabel(absoluteLayout, "Heartrate", 0.05, 0.10, Globals.LightTextColor, "", 0);
            PageBuilder.AddReturnButton(absoluteLayout, this);

            ImageButton previousBtn = new ImageButton
            {
                Source = "arrow_left.png",
                BackgroundColor = Color.Transparent
            };
            AbsoluteLayout.SetLayoutFlags(previousBtn, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(previousBtn, new Rectangle(0.3, 0.135, 0.1, 0.1));
            previousBtn.SetBinding(Button.CommandProperty, new Binding() { Path = "PreviousDayBinding" });
            previousBtn.Clicked += Globals.HeartratePageViewModel.PreviousDayBtnClick;
            absoluteLayout.Children.Add(previousBtn);

            ImageButton nextBtn = new ImageButton
            {
                Source = "arrow_right.png",
                BackgroundColor = Color.Transparent
            };
            AbsoluteLayout.SetLayoutFlags(nextBtn, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(nextBtn, new Rectangle(0.7, 0.135, 0.1, 0.1));
            nextBtn.Clicked += Globals.HeartratePageViewModel.NextDayBtnClick;
            absoluteLayout.Children.Add(nextBtn);

            PageBuilder.AddLabel(absoluteLayout, "", 0.5, 0.20, Color.Black, "DayLabelText", 15);

            ChartView chart = new ChartView { BackgroundColor = Globals.PrimaryColor };
            //chart.Chart.BackgroundColor = Globals.PrimaryColor.ToSKColor();

            chart.SetBinding(ChartView.ChartProperty, "Chart");
            AbsoluteLayout.SetLayoutBounds(chart, new Rectangle(0.5, 0.4, 0.95, 0.35));
            AbsoluteLayout.SetLayoutFlags(chart, AbsoluteLayoutFlags.All);

            absoluteLayout.Children.Add(chart);

            var averageHeartrateLabel =
                PageBuilder.AddLabel(absoluteLayout, "", 0.5, 0.65, Color.Black, "AverageLabelText", 20);

            var peakHeartrateLabel =
                PageBuilder.AddLabel(absoluteLayout, "", 0.5, 0.68, Color.Black, "PeakHeartrateText", 20);

            #region heartrateinterval selector
            Image heartonlyImage2 = new Image { Source = "HeartOnlyTransparent.png" };
            AbsoluteLayout.SetLayoutFlags(heartonlyImage2, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(heartonlyImage2, new Rectangle(0.15, 0.85, 40, 40));
            absoluteLayout.Children.Add(heartonlyImage2);
            var itnervallabel = PageBuilder.AddLabel(absoluteLayout, "Interval:", 0.30, 0.84, Color.Black, "", 15);

            intervaldefaultButton = PageBuilder.AddButton(absoluteLayout, "5", OnIntervalLabelClicked, 0.50, 0.85, 50, 50, 25, 0, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            intervaldefaultButton.BorderWidth = 1;

            interval15Button = PageBuilder.AddButton(absoluteLayout, "15", OnIntervalLabelClicked, 0.70, 0.85, 50, 50, 25, 0, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            interval15Button.BorderWidth = 1;

            interval30Button = PageBuilder.AddButton(absoluteLayout, "30", OnIntervalLabelClicked, 0.90, 0.85, 50, 50, 25, 0, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            interval30Button.BorderWidth = 1;
            #endregion
        }

        private async void OnIntervalLabelClicked(object sender, EventArgs args)
        {
            var intervalButton = sender as Button;

            intervaldefaultButton.BorderColor = Color.White;
            interval15Button.BorderColor = Color.White;
            interval30Button.BorderColor = Color.White;
            intervalButton.BorderColor = Color.Black;

            var interval = Convert.ToInt32(intervalButton.Text);
            Globals.HeartratePageViewModel.UpdateInterval(interval);
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

