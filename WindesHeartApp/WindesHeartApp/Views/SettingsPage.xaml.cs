using FormsControls.Base;
using WindesHeartApp.Resources;
using WindesHeartSDK;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rectangle = Xamarin.Forms.Rectangle;

namespace WindesHeartApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage, IAnimationPage
    {

        public static Picker DatePicker;
        public static Picker HourPicker;
        public static Switch WristSwitch;
        public static Picker StepsPicker;
        public static Picker LanguagePicker;

        public SettingsPage()
        {
            BindingContext = Globals.SettingsPageViewModel;
            InitializeComponent();   
            BuildPage();
        }

        protected override void OnAppearing()
        {
            Globals.SettingsPageViewModel.OnAppearing();
        }

        private void BuildPage()
        {
            AbsoluteLayout absoluteLayout = new AbsoluteLayout();

            PageBuilder.BuildPageBasics(absoluteLayout, this);
            PageBuilder.AddHeaderImages(absoluteLayout);
            PageBuilder.AddLabel(absoluteLayout, "Settings", 0.05, 0.10, Globals.LightTextColor, "", 0);
            PageBuilder.AddReturnButton(absoluteLayout, this);

            #region Datetime format
            Label dateLabel = new Label { Text = "Date Format", TextColor = Color.Black, FontSize = Globals.ScreenHeight / 100 * 2.5, HorizontalTextAlignment = TextAlignment.Center };
            AbsoluteLayout.SetLayoutBounds(dateLabel, new Rectangle(0.5, 0.2, -1, -1));
            AbsoluteLayout.SetLayoutFlags(dateLabel, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(dateLabel);

            DatePicker = new Picker { FontSize = Globals.ScreenHeight / 100 * 2.5 };
            DatePicker.Items.Add("DD/MM/YYYY");
            DatePicker.Items.Add("MM/DD/YYYY");
            DatePicker.SelectedIndexChanged += Globals.SettingsPageViewModel.DateIndexChanged;
            AbsoluteLayout.SetLayoutBounds(DatePicker, new Rectangle(0.5, 0.25, Globals.ScreenHeight / 100 * 18, -1));
            AbsoluteLayout.SetLayoutFlags(DatePicker, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(DatePicker);
            #endregion

            #region Hour notation format
            Label hourLabel = new Label { Text = "Hour Notation", TextColor = Color.Black, FontSize = Globals.ScreenHeight / 100 * 2.5, HorizontalTextAlignment = TextAlignment.Center };
            AbsoluteLayout.SetLayoutBounds(hourLabel, new Rectangle(0.5, 0.35, -1, -1));
            AbsoluteLayout.SetLayoutFlags(hourLabel, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(hourLabel);

            HourPicker = new Picker { Title = "Set Notation", FontSize = Globals.ScreenHeight / 100 * 2.5 };
            HourPicker.Items.Add("24 hour");
            HourPicker.Items.Add("12 hour");
            HourPicker.SelectedIndexChanged += Globals.SettingsPageViewModel.HourIndexChanged;
            AbsoluteLayout.SetLayoutBounds(HourPicker, new Rectangle(0.5, 0.4, Globals.ScreenHeight / 100 * 10, -1));
            AbsoluteLayout.SetLayoutFlags(HourPicker, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(HourPicker);
            #endregion

            #region Toggle Wrist Activation
            Label wristLabel = new Label { Text = "Activate Screen On Wrist Raise", TextColor = Color.Black, FontSize = Globals.ScreenHeight / 100 * 2.5, HorizontalTextAlignment = TextAlignment.Center };
            AbsoluteLayout.SetLayoutBounds(wristLabel, new Rectangle(0.5, 0.5, -1, -1));
            AbsoluteLayout.SetLayoutFlags(wristLabel, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(wristLabel);

            WristSwitch = new Switch();
            WristSwitch.Toggled += Globals.SettingsPageViewModel.OnWristToggled;
            AbsoluteLayout.SetLayoutBounds(WristSwitch, new Rectangle(0.5, 0.55, -1, -1));
            AbsoluteLayout.SetLayoutFlags(WristSwitch, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(WristSwitch);
            #endregion

            #region Daily step goal
            Label stepsLabel = new Label { Text = "Daily Steps Goal", TextColor = Color.Black, FontSize = Globals.ScreenHeight / 100 * 2.5, HorizontalTextAlignment = TextAlignment.Center };
            AbsoluteLayout.SetLayoutBounds(stepsLabel, new Rectangle(0.5, 0.65, -1, -1));
            AbsoluteLayout.SetLayoutFlags(stepsLabel, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(stepsLabel);

            StepsPicker = new Picker { Title = "Set Goal", FontSize = Globals.ScreenHeight / 100 * 2.5 };
            for (int i = 1; i < 21; i++) StepsPicker.Items.Add((i * 1000).ToString());
            StepsPicker.SelectedIndexChanged += Globals.SettingsPageViewModel.StepsIndexChanged;
            AbsoluteLayout.SetLayoutBounds(StepsPicker, new Rectangle(0.5, 0.7, Globals.ScreenHeight / 100 * 8, -1));
            AbsoluteLayout.SetLayoutFlags(StepsPicker, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(StepsPicker);
            #endregion

            #region Device Language
            Label formatLabel = new Label { Text = "Device Language", TextColor = Color.Black, FontSize = Globals.ScreenHeight / 100 * 2.5, HorizontalTextAlignment = TextAlignment.Center };
            AbsoluteLayout.SetLayoutBounds(formatLabel, new Rectangle(0.5, 0.8, -1, -1));
            AbsoluteLayout.SetLayoutFlags(formatLabel, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(formatLabel);

            LanguagePicker = new Picker { Title = "Set Language", FontSize = Globals.ScreenHeight / 100 * 2.5 };
            LanguagePicker.SelectedIndexChanged += Globals.SettingsPageViewModel.LanguageIndexChanged;
            AbsoluteLayout.SetLayoutBounds(LanguagePicker, new Rectangle(0.5, 0.85, Globals.ScreenHeight / 100 * 15, -1));
            AbsoluteLayout.SetLayoutFlags(LanguagePicker, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(LanguagePicker);
            #endregion
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
