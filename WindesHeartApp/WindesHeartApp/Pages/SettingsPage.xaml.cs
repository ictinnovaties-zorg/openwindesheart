using System;
using WindesHeartApp.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Xamarin.Forms.AbsoluteLayout;

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
            NavigationPage.SetHasNavigationBar(this, false);
            absoluteLayout.BackgroundColor = Globals.primaryColor;

            #region define and add Images
            Image heartonlyImage = new Image();
            heartonlyImage.Source = "HeartOnlyTransparent.png";
            heartonlyImage.BackgroundColor = Globals.primaryColor;
            AbsoluteLayout.SetLayoutFlags(heartonlyImage, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(heartonlyImage, new Rectangle(0.05, 0, Globals.screenWidth / 100 * 20, Globals.screenHeight / 100 * 10));
            absoluteLayout.Children.Add(heartonlyImage);

            Image textonlyImage = new Image();
            textonlyImage.Source = "TextOnlyTransparent.png";
            textonlyImage.BackgroundColor = Globals.primaryColor;
            AbsoluteLayout.SetLayoutFlags(textonlyImage, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(textonlyImage, new Rectangle(0.95, 0, Globals.screenWidth / 100 * 60, Globals.screenHeight / 100 * 10));
            absoluteLayout.Children.Add(textonlyImage);
            #endregion

            #region define return Button
            //added extra grid behind imagebutton to make it clickable easier.
            Grid returnGrid = new Grid();
            SetLayoutBounds(returnGrid, new Rectangle(0.95, 0.95, Globals.screenHeight / 100 * 8, Globals.screenHeight / 100 * 8));
            SetLayoutFlags(returnGrid, AbsoluteLayoutFlags.PositionProportional);
            returnGrid.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(execute: () => { returnButton_Clicked(this, EventArgs.Empty); })
            });
            ImageButton returnButton = new ImageButton();
            returnButton.Source = "GoBack.png";
            returnButton.BackgroundColor = Color.Transparent;
            SetLayoutBounds(returnButton, new Rectangle(0.95, 0.95, (int)Globals.screenHeight / 100 * 6, Globals.screenHeight / 100 * 6));
            SetLayoutFlags(returnButton, AbsoluteLayoutFlags.PositionProportional);
            returnButton.Clicked += returnButton_Clicked;
            absoluteLayout.Children.Add(returnGrid);
            absoluteLayout.Children.Add(returnButton);
            #endregion

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

            #region color Pickers with Labels
            Label primarycolorpickerLabel = new Label { Text = "Select Primary Color", TextColor = Globals.lighttextColor, FontSize = Globals.screenHeight / 100 * 2.5, HorizontalTextAlignment = TextAlignment.Center };
            AbsoluteLayout.SetLayoutBounds(primarycolorpickerLabel, new Rectangle(0.5, 0.38, Globals.screenWidth / 2, Globals.screenHeight / 100 * 5));
            AbsoluteLayout.SetLayoutFlags(primarycolorpickerLabel, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(primarycolorpickerLabel);

            Picker primaryPicker = new Picker { Title = "Select..", FontSize = Globals.screenHeight / 100 * 2.5 };
            foreach (string colorName in Globals.colorDictionary.Keys)
            {
                primaryPicker.Items.Add(colorName);
            }
            primaryPicker.SelectedIndexChanged += (sender, args) =>
            {
                _tempprimaryColor = primaryPicker.Items[primaryPicker.SelectedIndex];
                absoluteLayout.BackgroundColor = Globals.colorDictionary[_tempprimaryColor];
                heartonlyImage.BackgroundColor = Globals.colorDictionary[_tempprimaryColor];
                textonlyImage.BackgroundColor = Globals.colorDictionary[_tempprimaryColor];
                returnButton.BackgroundColor = Globals.colorDictionary[_tempprimaryColor];
            };
            AbsoluteLayout.SetLayoutBounds(primaryPicker, new Rectangle(0.5, 0.4, Globals.screenWidth - Globals.screenWidth / 100 * 4, Globals.screenHeight / 100 * 5));
            AbsoluteLayout.SetLayoutFlags(primaryPicker, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(primaryPicker);


            Label secondarycolorpickerlabel = new Label { Text = "Select Secondary Color", TextColor = Globals.lighttextColor, FontSize = Globals.screenHeight / 100 * 2.5, HorizontalTextAlignment = TextAlignment.Center };
            AbsoluteLayout.SetLayoutBounds(secondarycolorpickerlabel, new Rectangle(0.5, 0.48, Globals.screenWidth - Globals.screenWidth / 100 * 4, Globals.screenHeight / 100 * 5));
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
                            savechangesButton.BackgroundColor = Globals.colorDictionary[_tempsecondaryColor];
                        }
                    };
            AbsoluteLayout.SetLayoutBounds(secondaryPicker, new Rectangle(0.5, 0.5, Globals.screenWidth - Globals.screenWidth / 100 * 2, Globals.screenHeight / 100 * 5));
            AbsoluteLayout.SetLayoutFlags(secondaryPicker, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(secondaryPicker);
            #endregion
        }

        private void savechangesButton_Clicked(object sender, EventArgs e)
        {
            Globals.BuildGlobals(_tempprimaryColor, _tempsecondaryColor);
            Navigation.PopAsync();
        }

        private void returnButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
