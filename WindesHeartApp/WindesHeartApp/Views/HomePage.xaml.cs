using FormsControls.Base;
using System;
using System.Threading.Tasks;
using WindesHeartApp.Pages;
using WindesHeartApp.Resources;
using WindesHeartApp.Services;
using WindesHeartSDK;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Image = Xamarin.Forms.Image;
using Label = Xamarin.Forms.Label;

namespace WindesHeartApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage, IAnimationPage
    {
        public static Button AboutButton;
        public static Button DeviceButton;
        public static Button SleepButton;
        public static Button StepsButton;
        public static Button HeartrateButton;
        public static Button SettingsButton;

        private readonly string _propertyKey = "LastConnectedDevice";
        public HomePage()
        {
            InitializeComponent();
            BindingContext = Globals.HomePageViewModel;
            BuildPage();
        }

        protected override void OnAppearing()
        {

            App.RequestLocationPermission();
            if (Windesheart.PairedDevice == null)
                return;
        }

        //Set UUID in App-properties
        private void SetApplicationProperties()
        {
            if (Windesheart.PairedDevice != null)
            {
                App.Current.Properties[_propertyKey] = Windesheart.PairedDevice.Uuid;
            }
        }

        //Handle Auto-connect to the last connected device with App-properties
        private async Task HandleAutoConnect()
        {
            var knownGuid = App.Current.Properties[_propertyKey].ToString();
            if (!string.IsNullOrEmpty(knownGuid))
            {
                var knownDevice = await Windesheart.GetKnownDevice(Guid.Parse(knownGuid));
                knownDevice.Connect(CallbackHandler.OnConnect);
            }
        }

        private void BuildPage()
        {
            absoluteLayout = new AbsoluteLayout();
            PageBuilder.BuildPageBasics(absoluteLayout, this);
            PageBuilder.AddLabel(absoluteLayout, "Home", 0.05, 0.10, Globals.LightTextColor, "", 30);
            PageBuilder.AddHeaderImages(absoluteLayout);

            #region define fetch progressbar 
            ProgressBar fetchProgressBar = new ProgressBar
            {
                ProgressColor = Color.Red,
                HeightRequest = 20
            };

            fetchProgressBar.SetBinding(ProgressBar.ProgressProperty, new Binding("FetchProgress"));
            fetchProgressBar.SetBinding(ProgressBar.IsVisibleProperty, new Binding("FetchProgressVisible"));

            AbsoluteLayout.SetLayoutBounds(fetchProgressBar, new Rectangle(0.5, 0.25, 0.95, -1));
            AbsoluteLayout.SetLayoutFlags(fetchProgressBar, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);

            absoluteLayout.Children.Add(fetchProgressBar);
            #endregion

            #region define battery and hr Label
            Image batteryImage = new Image { HeightRequest = (int)(Globals.ScreenHeight / 100 * 2.5) };
            batteryImage.SetBinding(Image.SourceProperty, new Binding("BatteryImage"));
            AbsoluteLayout.SetLayoutBounds(batteryImage, new Rectangle(0.85, 0.183, -1, -1));
            AbsoluteLayout.SetLayoutFlags(batteryImage, AbsoluteLayoutFlags.PositionProportional);

            var bandNameLabel = PageBuilder.AddLabel(absoluteLayout, "", 0.95, 0.155, Color.Black, "BandNameLabel", 9);
            bandNameLabel.FontAttributes = FontAttributes.Bold;
            bandNameLabel.FontAttributes = FontAttributes.Italic;

            var batteryLabel = PageBuilder.AddLabel(absoluteLayout, "", 0.95, 0.18, Color.Black, "DisplayBattery", (int)(Globals.ScreenHeight / 100 * 2.5));
            batteryLabel.FontAttributes = FontAttributes.Bold;
            absoluteLayout.Children.Add(batteryImage);

            Label hrLabel = new Label { FontSize = Globals.ScreenHeight / 100 * 2.5, FontAttributes = FontAttributes.Bold };
            hrLabel.SetBinding(Label.TextProperty, new Binding("DisplayHeartRate"));
            AbsoluteLayout.SetLayoutBounds(hrLabel, new Rectangle(0.15, 0.18, -1, -1));
            AbsoluteLayout.SetLayoutFlags(hrLabel, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(hrLabel);
            #endregion

            PageBuilder.AddActivityIndicator(absoluteLayout, "IsLoading", 0.50, 0.65, 80, 80, AbsoluteLayoutFlags.PositionProportional, Globals.LightTextColor);

            int buttonSize = (int)(Globals.ScreenHeight / 100 * 8.5);
            AboutButton = PageBuilder.AddButton(absoluteLayout, "About", Globals.HomePageViewModel.AboutButtonClicked, 0.80, 0.90, buttonSize * 2, buttonSize * 2, buttonSize, (int)(buttonSize / 5), AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            DeviceButton = PageBuilder.AddButton(absoluteLayout, "Device", Globals.HomePageViewModel.DeviceButtonClicked, 0.80, 0.40, buttonSize * 2, buttonSize * 2, buttonSize, (int)(buttonSize / 5), AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            HeartrateButton = PageBuilder.AddButton(absoluteLayout, "Heartrate", Globals.HomePageViewModel.HeartrateButtonClicked, 0.20, 0.40, buttonSize * 2, buttonSize * 2, buttonSize, (int)(buttonSize / 5), AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            StepsButton = PageBuilder.AddButton(absoluteLayout, "Steps", Globals.HomePageViewModel.StepsButtonClicked, 0.90, 0.65, buttonSize * 2, buttonSize * 2, buttonSize, (int)(buttonSize / 5), AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            SettingsButton = PageBuilder.AddButton(absoluteLayout, "Settings", Globals.HomePageViewModel.SettingsButtonClicked, 0.20, 0.90, buttonSize * 2, buttonSize * 2, buttonSize, (int)(buttonSize / 5), AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            SleepButton = PageBuilder.AddButton(absoluteLayout, "Sleep", Globals.HomePageViewModel.SleepButtonClicked, 0.10, 0.65, buttonSize * 2, buttonSize * 2, buttonSize, (int)(buttonSize / 5), AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);

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