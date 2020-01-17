using FormsControls.Base;
using Microcharts.Forms;
using OpenWindesheart;
using OpenWindesheartDemoApp.Resources;
using OpenWindesheartDemoApp.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OpenWindesheartDemoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeartratePage : IAnimationPage
    {
        private Button _interval15Button;
        private Button _interval10Button;
        private Button _interval5Button;
        private Button _intervaldefaultButton;
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
            #region absoluteLayout
            absoluteLayout = new AbsoluteLayout();
            PageBuilder.BuildPageBasics(absoluteLayout, this);
            PageBuilder.AddHeaderImages(absoluteLayout);
            PageBuilder.AddLabel(absoluteLayout, "Heartrate", 0.05, 0.10, Globals.LightTextColor, "", 0);
            PageBuilder.AddReturnButton(absoluteLayout);
            #endregion

            #region previousButton
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
            #endregion

            #region nextButton
            ImageButton nextBtn = new ImageButton
            {
                Source = "arrow_right.png",
                BackgroundColor = Color.Transparent
            };
            AbsoluteLayout.SetLayoutFlags(nextBtn, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(nextBtn, new Rectangle(0.7, 0.135, 0.1, 0.1));
            nextBtn.Clicked += Globals.HeartratePageViewModel.NextDayBtnClick;
            absoluteLayout.Children.Add(nextBtn);
            #endregion

            #region chart & labels
            PageBuilder.AddLabel(absoluteLayout, "", 0.5, 0.20, Color.Black, "DayLabelText", 12);
            ChartView chart = new ChartView { BackgroundColor = Globals.PrimaryColor };
            //chart.Chart.BackgroundColor = Globals.PrimaryColor.ToSKColor();

            chart.SetBinding(ChartView.ChartProperty, "Chart");
            AbsoluteLayout.SetLayoutBounds(chart, new Rectangle(0.5, 0.4, 0.95, 0.35));
            AbsoluteLayout.SetLayoutFlags(chart, AbsoluteLayoutFlags.All);

            absoluteLayout.Children.Add(chart);
            PageBuilder.AddLabel(absoluteLayout, "", 0.5, 0.65, Color.Black, "AverageLabelText", 16);
            PageBuilder.AddLabel(absoluteLayout, "", 0.5, 0.70, Color.Black, "PeakHeartrateText", 16);
            #endregion

            #region heartrateinterval selector
            Image heartonlyImage2 = new Image { Source = "HeartOnlyTransparent.png" };
            AbsoluteLayout.SetLayoutFlags(heartonlyImage2, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(heartonlyImage2, new Rectangle(0.05, 0.85, 40, 40));
            absoluteLayout.Children.Add(heartonlyImage2);
            PageBuilder.AddLabel(absoluteLayout, "Interval:", 0.22, 0.84, Color.Black, "", 13);

            _intervaldefaultButton = PageBuilder.AddButton(absoluteLayout, "1", OnIntervalLabelClicked, 0.50, 0.85, 40, 40, 25, 0, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            _intervaldefaultButton.BorderWidth = 1;
            _intervaldefaultButton.BorderColor = Color.White;


            _interval5Button = PageBuilder.AddButton(absoluteLayout, "5", OnIntervalLabelClicked, 0.65, 0.85, 40, 40, 25, 0, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            _interval5Button.BorderWidth = 1;
            _interval5Button.BorderColor = Color.Black;

            _interval10Button = PageBuilder.AddButton(absoluteLayout, "10", OnIntervalLabelClicked, 0.80, 0.85, 40, 40, 25, 0, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            _interval10Button.BorderWidth = 1;
            _interval10Button.BorderColor = Color.White;

            _interval15Button = PageBuilder.AddButton(absoluteLayout, "15", OnIntervalLabelClicked, 0.95, 0.85, 40, 40, 25, 0, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            _interval15Button.BorderWidth = 1;
            _interval15Button.BorderColor = Color.White;
            #endregion

            #region RFbutton

            Grid grid = new Grid();
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
            ImageButton refreshButton = new ImageButton
            {
                Source = "Refresh.png",
                HorizontalOptions = LayoutOptions.Start,
                HeightRequest = Globals.ScreenHeight / 100 * 4.5,
                BackgroundColor = Color.Transparent,
                Margin = new Thickness(2, 0, 0, 0),
            };
            refreshButton.Clicked += RefreshButtonClicked;
            grid.Children.Add(frame);
            grid.Children.Add(refreshButton);

            var refreshLabel = new Label
            {
                Text = "Data",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.End,
                FontSize = 15,
                FontAttributes = FontAttributes.Italic,
                Margin = new Thickness(0, 0, 2, 0)
            };

            grid.Children.Add(refreshLabel);

            refreshLabel.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(execute: () => { RefreshButtonClicked(this, EventArgs.Empty); }),
            });
            absoluteLayout.Children.Add(grid);
            #endregion
        }

        private async void RefreshButtonClicked(object sender, EventArgs e)
        {
            if (Windesheart.PairedDevice == null || !Windesheart.PairedDevice.IsConnected())
            {
                await Application.Current.MainPage.DisplayAlert("Error while refreshing data",
                    "Can only refresh data when connected to a device!", "Ok");
                return;
            }
            await Application.Current.MainPage.Navigation.PopAsync();
            Globals.SamplesService.StartFetching();
        }

        private void OnIntervalLabelClicked(object sender, EventArgs args)
        {
            var intervalButton = sender as Button;

            _intervaldefaultButton.BorderColor = Color.White;
            _interval5Button.BorderColor = Color.White;
            _interval15Button.BorderColor = Color.White;
            _interval10Button.BorderColor = Color.White;
            if (intervalButton == null) return;
            intervalButton.BorderColor = Color.Black;

            var interval = Convert.ToInt32(intervalButton.Text);
            Globals.HeartratePageViewModel.UpdateInterval(interval);
        }

        #region pageAnimation
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
        #endregion
    }
}