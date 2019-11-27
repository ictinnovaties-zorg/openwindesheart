using System;
using WindesHeartApp.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WindesHeartApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private string _tempprimaryColor;
        private string _tempsecondaryColor;
        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            BuildPage();
        }

        private void BuildPage()
        {
            AbsoluteLayout absoluteLayout = new AbsoluteLayout();

            PageBuilder.BuildPageBasics(absoluteLayout, this);
            PageBuilder.AddHeaderImages(absoluteLayout);
            PageBuilder.AddLabel(absoluteLayout, "Settings", 0.05, 0.10, Globals.lighttextColor, "", 0);
            PageBuilder.AddReturnButton(absoluteLayout, this);

            #region save changes Button
            Button savechangesButton = new Button();
            savechangesButton.Text = "Save Changes";
            savechangesButton.BackgroundColor = Globals.secondaryColor;
            savechangesButton.FontSize = Globals.screenHeight / 100 * 2;
            savechangesButton.CornerRadius = (int)Globals.screenHeight / 100 * 7;
            AbsoluteLayout.SetLayoutBounds(savechangesButton, new Rectangle(0.5, 0.90, Globals.screenHeight / 100 * 30, Globals.screenHeight / 100 * 7));
            AbsoluteLayout.SetLayoutFlags(savechangesButton, AbsoluteLayoutFlags.PositionProportional);
            savechangesButton.Clicked += savechangesButton_Clicked;
            absoluteLayout.Children.Add(savechangesButton);
            #endregion

            #region button-color picker with Label

            Label secondarycolorpickerlabel = new Label { Text = "Select Secondary Color", TextColor = Globals.lighttextColor, FontSize = Globals.screenHeight / 100 * 2.5, HorizontalTextAlignment = TextAlignment.Center };
            AbsoluteLayout.SetLayoutBounds(secondarycolorpickerlabel, new Rectangle(0.5, 0.45, -1, -1));
            AbsoluteLayout.SetLayoutFlags(secondarycolorpickerlabel, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(secondarycolorpickerlabel);

            Picker secondaryPicker = new Picker { Title = "Select..", FontSize = Globals.screenHeight / 100 * 2.5 };
            foreach (string colorName in Globals.colorDictionary.Keys)
            {
                secondaryPicker.Items.Add(colorName);
            }
            secondaryPicker.SelectedIndexChanged += (sender, args) =>
                    {
                        if (secondaryPicker.SelectedIndex == -1)
                        {
                            Globals.primaryColor = Color.FromHex("#96d1ff");
                        }
                        else
                        {
                            _tempsecondaryColor = secondaryPicker.Items[secondaryPicker.SelectedIndex];
                            secondaryPicker.BackgroundColor = Globals.colorDictionary[_tempsecondaryColor];
                        }
                    };
            AbsoluteLayout.SetLayoutBounds(secondaryPicker, new Rectangle(0.5, 0.5, Globals.screenWidth - Globals.screenWidth / 100 * 4, -1));
            AbsoluteLayout.SetLayoutFlags(secondaryPicker, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(secondaryPicker);
            #endregion
        }

        private void savechangesButton_Clicked(object sender, EventArgs e)
        {
            if (_tempprimaryColor != null)
                Globals.primaryColor = Globals.colorDictionary[_tempprimaryColor];
            if (_tempsecondaryColor != null)
                Globals.secondaryColor = Globals.colorDictionary[_tempsecondaryColor];
            Navigation.PopAsync();
        }
    }
}
