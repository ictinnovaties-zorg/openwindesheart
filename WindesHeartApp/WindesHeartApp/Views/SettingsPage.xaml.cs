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
        public SettingsPage()
        {
            InitializeComponent();
            BuildPage();
        }

        private void BuildPage()
        {
            AbsoluteLayout absoluteLayout = new AbsoluteLayout();

            PageBuilder.BuildPageBasics(absoluteLayout, this);
            PageBuilder.AddHeaderImages(absoluteLayout);
            PageBuilder.AddLabel(absoluteLayout, "Settings", 0.05, 0.10, Globals.LightTextColor, "", 0);
            PageBuilder.AddReturnButton(absoluteLayout, this);

            #region Datetime format
            Label dateLabel = new Label { Text = "Date Format", TextColor = Globals.LightTextColor, FontSize = Globals.ScreenHeight / 100 * 2.5, HorizontalTextAlignment = TextAlignment.Center };
            AbsoluteLayout.SetLayoutBounds(dateLabel, new Rectangle(0.5, 0.2, -1, -1));
            AbsoluteLayout.SetLayoutFlags(dateLabel, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(dateLabel);

            Picker datePicker = new Picker { Title = "Set Format...", FontSize = Globals.ScreenHeight / 100 * 2.5 };
            datePicker.Items.Add("MM/DD/YYYY");
            datePicker.Items.Add("DD/MM/YYYY");
            datePicker.SelectedIndexChanged += Globals.SettingsPageViewModel.DateIndexChanged;
            AbsoluteLayout.SetLayoutBounds(datePicker, new Rectangle(0.5, 0.25, -1, -1));
            AbsoluteLayout.SetLayoutFlags(datePicker, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(datePicker);
            #endregion

            #region hour notation format
            Label hourLabel = new Label { Text = "Hour Notation", TextColor = Globals.LightTextColor, FontSize = Globals.ScreenHeight / 100 * 2.5, HorizontalTextAlignment = TextAlignment.Center };
            AbsoluteLayout.SetLayoutBounds(hourLabel, new Rectangle(0.5, 0.3, -1, -1));
            AbsoluteLayout.SetLayoutFlags(hourLabel, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(hourLabel);

            Picker hourPicker = new Picker { Title = "Set Notation...", FontSize = Globals.ScreenHeight / 100 * 2.5 };
            hourPicker.Items.Add("24 hour");
            hourPicker.Items.Add("12 hour");
            hourPicker.SelectedIndexChanged += Globals.SettingsPageViewModel.HourIndexChanged;
            AbsoluteLayout.SetLayoutBounds(hourPicker, new Rectangle(0.5, 0.35, -1, -1));
            AbsoluteLayout.SetLayoutFlags(hourPicker, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(hourPicker);
            #endregion

            #region Toggle Wrist Activation
            Button ToggleWristActivation = PageBuilder.AddButton(absoluteLayout, "Toggle wristactivation", Globals.SettingsPageViewModel.ToggleWristActivatedClicked, 0.95, 0.2, 0.45, 0.05, 10, 12, AbsoluteLayoutFlags.All, Globals.SecondaryColor);
            #endregion


            #region Daily step goal
            Label stepsLabel = new Label { Text = "Daily Steps Goal", TextColor = Globals.LightTextColor, FontSize = Globals.ScreenHeight / 100 * 2.5, HorizontalTextAlignment = TextAlignment.Center };
            AbsoluteLayout.SetLayoutBounds(stepsLabel, new Rectangle(0.5, 0.4, -1, -1));
            AbsoluteLayout.SetLayoutFlags(stepsLabel, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(stepsLabel);

            Picker stepsPicker = new Picker { Title = Globals.DailyStepsGoal.ToString(), FontSize = Globals.ScreenHeight / 100 * 2.5 };
            for (int i = 1; i < 21; i++)
            {
                stepsPicker.Items.Add((i * 1000).ToString());
            }
            stepsPicker.SelectedIndexChanged += (sender, args) =>
            {
                if (stepsPicker.SelectedIndex != -1)
                {
                    string steps = stepsPicker.Items[stepsPicker.SelectedIndex];
                    Globals.DailyStepsGoal = int.Parse(steps);
                }
            };
            AbsoluteLayout.SetLayoutBounds(stepsPicker, new Rectangle(0.5, 0.45, -1, -1));
            AbsoluteLayout.SetLayoutFlags(stepsPicker, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(stepsPicker);
            #endregion

            #region Format picker
            Label formatLabel = new Label { Text = "Device Language", TextColor = Globals.LightTextColor, FontSize = Globals.ScreenHeight / 100 * 2.5, HorizontalTextAlignment = TextAlignment.Center };
            AbsoluteLayout.SetLayoutBounds(formatLabel, new Rectangle(0.5, 0.45, -1, -1));
            AbsoluteLayout.SetLayoutFlags(formatLabel, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(formatLabel);

            Picker FormatPicker = new Picker { Title = "Set Language...", FontSize = Globals.ScreenHeight / 100 * 2.5 };
            foreach (string format in Globals.languageDictionary.Keys)
            {
                FormatPicker.Items.Add(format);
            }
            FormatPicker.SelectedIndexChanged += (sender, args) =>
            {
                if (FormatPicker.SelectedIndex != -1)
                {
                    Windesheart.ConnectedDevice?.SetLanguage(FormatPicker.Items[FormatPicker.SelectedIndex]);
                }
            };
            AbsoluteLayout.SetLayoutBounds(FormatPicker, new Rectangle(0.5, 0.5, Globals.ScreenWidth - Globals.ScreenWidth / 100 * 4, -1));
            AbsoluteLayout.SetLayoutFlags(FormatPicker, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(FormatPicker);
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
