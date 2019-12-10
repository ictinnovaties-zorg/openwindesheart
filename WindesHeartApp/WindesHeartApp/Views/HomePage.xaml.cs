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
            BindingContext = Globals.HomepageviewModel;
            BuildPage();
        }

        protected override void OnAppearing()
        {
            App.RequestLocationPermission();
            if (Windesheart.ConnectedDevice != null)
                Globals.HomepageviewModel.ReadCurrentBattery();
        }

        private void BuildPage()
        {
            absoluteLayout = new AbsoluteLayout();
            PageBuilder.BuildPageBasics(absoluteLayout, this);
            PageBuilder.AddLabel(absoluteLayout, "Home", 0.05, 0.10, Globals.LightTextColor, "", 30);
            PageBuilder.AddHeaderImages(absoluteLayout);

            #region define battery and hr Label
            Image batteryImage = new Image();
            batteryImage.SetBinding(Image.SourceProperty, new Binding("BatteryImage"));
            batteryImage.HeightRequest = Globals.ScreenHeight / 100 * 2.5;

            AbsoluteLayout.SetLayoutBounds(batteryImage, new Rectangle(0.85, 0.18, -1, -1));
            AbsoluteLayout.SetLayoutFlags(batteryImage, AbsoluteLayoutFlags.PositionProportional);

            Label batteryLabel = new Label
            { FontSize = Globals.ScreenHeight / 100 * 2.5, FontAttributes = FontAttributes.Bold };
            batteryLabel.SetBinding(Label.TextProperty, new Binding("DisplayBattery"));
            AbsoluteLayout.SetLayoutBounds(batteryLabel, new Rectangle(0.95, 0.18, -1, -1));
            AbsoluteLayout.SetLayoutFlags(batteryLabel, AbsoluteLayoutFlags.PositionProportional);

            absoluteLayout.Children.Add(batteryImage);
            absoluteLayout.Children.Add(batteryLabel);

            Image heartrateImage = new Image();
            heartrateImage.SetBinding(Image.SourceProperty, new Binding("HeartImage"));
            heartrateImage.HeightRequest = Globals.ScreenHeight / 100 * 2.5;

            AbsoluteLayout.SetLayoutBounds(heartrateImage, new Rectangle(0.85, 0.18, -1, -1));
            AbsoluteLayout.SetLayoutFlags(heartrateImage, AbsoluteLayoutFlags.PositionProportional);
            Label HRLabel = new Label
            { FontSize = Globals.ScreenHeight / 100 * 2.5, FontAttributes = FontAttributes.Bold };
            HRLabel.SetBinding(Label.TextProperty, new Binding("DisplayHeartRate"));
            AbsoluteLayout.SetLayoutBounds(HRLabel, new Rectangle(0.05, 0.18, -1, -1));
            AbsoluteLayout.SetLayoutFlags(HRLabel, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(HRLabel);
            absoluteLayout.Children.Add(heartrateImage);
            #endregion 

            PageBuilder.AddActivityIndicator(absoluteLayout, "IsLoading", 0.50, 0.65, 100, 100, AbsoluteLayoutFlags.PositionProportional, Globals.LightTextColor);

            int buttonSize = (int)(Globals.ScreenHeight / 100 * 8.3);
            PageBuilder.AddButton(absoluteLayout, "About", Globals.HomepageviewModel.AboutButtonClicked, 0.80, 0.90, buttonSize * 2, buttonSize * 2, buttonSize, 15, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            PageBuilder.AddButton(absoluteLayout, "Device", Globals.HomepageviewModel.DeviceButtonClicked, 0.80, 0.40, buttonSize * 2, buttonSize * 2, buttonSize, 15, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            PageBuilder.AddButton(absoluteLayout, "Heartrate", Globals.HomepageviewModel.HeartrateButtonClicked, 0.20, 0.40, buttonSize * 2, buttonSize * 2, buttonSize, 15, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            PageBuilder.AddButton(absoluteLayout, "Steps", Globals.HomepageviewModel.StepsButtonClicked, 0.90, 0.65, buttonSize * 2, buttonSize * 2, buttonSize, 15, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            PageBuilder.AddButton(absoluteLayout, "Settings", Globals.HomepageviewModel.SettingsButtonClicked, 0.20, 0.90, buttonSize * 2, buttonSize * 2, buttonSize, 15, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            PageBuilder.AddButton(absoluteLayout, "Sleep", Globals.HomepageviewModel.SleepButtonClicked, 0.10, 0.65, buttonSize * 2, buttonSize * 2, buttonSize, 15, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);

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