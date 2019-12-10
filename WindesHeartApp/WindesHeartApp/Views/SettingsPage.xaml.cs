using FormsControls.Base;
using System;
using WindesHeartApp.Resources;
using WindesHeartSDK;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WindesHeartApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage, IAnimationPage
    {
        private string _tempsecondaryColor;
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

            #region save changes Button

            Button savechangesButton = new Button
            {
                Text = "Save Changes",
                BackgroundColor = Globals.SecondaryColor,
                FontSize = Globals.ScreenHeight / 100 * 2,
                CornerRadius = (int)Globals.ScreenHeight / 100 * 7
            };
            AbsoluteLayout.SetLayoutBounds(savechangesButton, new Rectangle(0.5, 0.90, Globals.ScreenHeight / 100 * 30, Globals.ScreenHeight / 100 * 7));
            AbsoluteLayout.SetLayoutFlags(savechangesButton, AbsoluteLayoutFlags.PositionProportional);
            savechangesButton.Clicked += SaveChangesButtonClicked;
            absoluteLayout.Children.Add(savechangesButton);
            #endregion

            Button ToggleFormatButton = PageBuilder.AddButton(absoluteLayout, "Toggle 12/24H", Globals.SettingsPageViewmodel.ToggleDisplayFormatsClicked, 0.05, 0.35, 0.45, 0.05, 10, 12, AbsoluteLayoutFlags.All, Globals.SecondaryColor);
            Button ToggleWristActivation = PageBuilder.AddButton(absoluteLayout, "Toggle wristactivation", Globals.SettingsPageViewmodel.ToggleWristActivatedClicked, 0.95, 0.35, 0.45, 0.05, 10, 12, AbsoluteLayoutFlags.All, Globals.SecondaryColor);



            Label ToggleDatetimeFormatLabel = new Label { Text = "Select Secondary Color", TextColor = Globals.LightTextColor, FontSize = Globals.ScreenHeight / 100 * 2.5, HorizontalTextAlignment = TextAlignment.Center };
            AbsoluteLayout.SetLayoutBounds(ToggleDatetimeFormatLabel, new Rectangle(0.5, 0.45, -1, -1));
            AbsoluteLayout.SetLayoutFlags(ToggleDatetimeFormatLabel, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(ToggleDatetimeFormatLabel);

            Picker FormatPicker = new Picker { Title = "Select..", FontSize = Globals.ScreenHeight / 100 * 2.5 };
            foreach (string colorName in Globals.ColorDictionary.Keys)
            {
                FormatPicker.Items.Add(colorName);
            }
            FormatPicker.SelectedIndexChanged += (sender, args) =>
                    {
                        if (FormatPicker.SelectedIndex == -1)
                        {
                        }
                        else
                        {
                            Windesheart.ConnectedDevice.SetLanguage("nl-NL");
                        }
                    };
            AbsoluteLayout.SetLayoutBounds(FormatPicker, new Rectangle(0.5, 0.5, Globals.ScreenWidth - Globals.ScreenWidth / 100 * 4, -1));
            AbsoluteLayout.SetLayoutFlags(FormatPicker, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(FormatPicker);


            #region button-color picker with Label

            Label secondarycolorpickerlabel = new Label { Text = "Select Secondary Color", TextColor = Globals.LightTextColor, FontSize = Globals.ScreenHeight / 100 * 2.5, HorizontalTextAlignment = TextAlignment.Center };
            AbsoluteLayout.SetLayoutBounds(secondarycolorpickerlabel, new Rectangle(0.5, 0.55, -1, -1));
            AbsoluteLayout.SetLayoutFlags(secondarycolorpickerlabel, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(secondarycolorpickerlabel);

            Picker secondaryPicker = new Picker { Title = "Select..", FontSize = Globals.ScreenHeight / 100 * 2.5 };
            foreach (string colorName in Globals.ColorDictionary.Keys)
            {
                secondaryPicker.Items.Add(colorName);
            }
            secondaryPicker.SelectedIndexChanged += (sender, args) =>
                    {
                        if (secondaryPicker.SelectedIndex == -1)
                        {
                            Globals.PrimaryColor = Color.FromHex("#96d1ff");
                        }
                        else
                        {
                            _tempsecondaryColor = secondaryPicker.Items[secondaryPicker.SelectedIndex];
                            secondaryPicker.BackgroundColor = Globals.ColorDictionary[_tempsecondaryColor];
                        }
                    };
            AbsoluteLayout.SetLayoutBounds(secondaryPicker, new Rectangle(0.5, 0.6, Globals.ScreenWidth - Globals.ScreenWidth / 100 * 4, -1));
            AbsoluteLayout.SetLayoutFlags(secondaryPicker, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(secondaryPicker);
            #endregion
        }

        private void SaveChangesButtonClicked(object sender, EventArgs e)
        {
            if (_tempsecondaryColor != null)
                Globals.SecondaryColor = Globals.ColorDictionary[_tempsecondaryColor];
            Navigation.PopAsync();
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
