using FormsControls.Base;
using System;
using WindesHeartApp.Resources;
using WindesHeartSDK;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Image = Xamarin.Forms.Image;
using Label = Xamarin.Forms.Label;

namespace WindesHeartApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage, IAnimationPage
    {
        public HomePage()
        {
            InitializeComponent();
            BindingContext = Globals.HomePageViewModel;
            BuildPage();
        }

        protected override void OnAppearing()
        {
            App.RequestLocationPermission();
            if (Windesheart.ConnectedDevice != null)
            {
                Globals.HomePageViewModel.ReadCurrentBattery();
                Globals.HomePageViewModel.BandNameLabel = Windesheart.ConnectedDevice.Device.Name;
            }
        }

        private void BuildPage()
        {
            absoluteLayout = new AbsoluteLayout();
            PageBuilder.BuildPageBasics(absoluteLayout, this);
            PageBuilder.AddLabel(absoluteLayout, "Home", 0.05, 0.10, Globals.LightTextColor, "", 30);
            PageBuilder.AddHeaderImages(absoluteLayout);

            #region define battery and hr Label
            Image batteryImage = new Image { HeightRequest = (int)(Globals.ScreenHeight / 100 * 2.5) };
            batteryImage.SetBinding(Image.SourceProperty, new Binding("BatteryImage"));
            AbsoluteLayout.SetLayoutBounds(batteryImage, new Rectangle(0.85, 0.18, -1, -1));
            AbsoluteLayout.SetLayoutFlags(batteryImage, AbsoluteLayoutFlags.PositionProportional);

            var bandNameLabel = PageBuilder.AddLabel(absoluteLayout, "", 0.95, 0.16, Color.Black, "BandNameLabel", 12);
            bandNameLabel.FontAttributes = FontAttributes.Bold;
            bandNameLabel.FontAttributes = FontAttributes.Italic;

            var batteryLabel = PageBuilder.AddLabel(absoluteLayout, "", 0.95, 0.18, Color.Black, "DisplayBattery", (int)(Globals.ScreenHeight / 100 * 2.5));
            batteryLabel.FontAttributes = FontAttributes.Bold;
            absoluteLayout.Children.Add(batteryImage);

            Label hrLabel = new Label { FontSize = Globals.ScreenHeight / 100 * 2.5, FontAttributes = FontAttributes.Bold };
            hrLabel.SetBinding(Label.TextProperty, new Binding("DisplayHeartRate"));
            AbsoluteLayout.SetLayoutBounds(hrLabel, new Rectangle(0.05, 0.18, -1, -1));
            AbsoluteLayout.SetLayoutFlags(hrLabel, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(hrLabel);
            #endregion

            PageBuilder.AddActivityIndicator(absoluteLayout, "IsLoading", 0.50, 0.65, 100, 100, AbsoluteLayoutFlags.PositionProportional, Globals.LightTextColor);

            int buttonSize = (int)(Globals.ScreenHeight / 100 * 8.3);
            PageBuilder.AddButton(absoluteLayout, "About", Globals.HomePageViewModel.AboutButtonClicked, 0.80, 0.90, buttonSize * 2, buttonSize * 2, buttonSize, 15, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            PageBuilder.AddButton(absoluteLayout, "Device", Globals.HomePageViewModel.DeviceButtonClicked, 0.80, 0.40, buttonSize * 2, buttonSize * 2, buttonSize, 15, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            PageBuilder.AddButton(absoluteLayout, "Heartrate", Globals.HomePageViewModel.HeartrateButtonClicked, 0.20, 0.40, buttonSize * 2, buttonSize * 2, buttonSize, 15, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            PageBuilder.AddButton(absoluteLayout, "Steps", Globals.HomePageViewModel.StepsButtonClicked, 0.90, 0.65, buttonSize * 2, buttonSize * 2, buttonSize, 15, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            PageBuilder.AddButton(absoluteLayout, "Settings", Globals.HomePageViewModel.SettingsButtonClicked, 0.20, 0.90, buttonSize * 2, buttonSize * 2, buttonSize, 15, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            PageBuilder.AddButton(absoluteLayout, "Sleep", Globals.HomePageViewModel.SleepButtonClicked, 0.10, 0.65, buttonSize * 2, buttonSize * 2, buttonSize, 15, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);

            var button =
                PageBuilder.AddButton(absoluteLayout, "TEST", TestButtonClicked, 0.5, 0.5, 0.4, 0.05, 0, 0, AbsoluteLayoutFlags.All, Globals.SecondaryColor);
        }

        private void TestButtonClicked(object sender, EventArgs e)
        {

            Navigation.PushAsync(new TestPage());
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