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
        public SettingsPage()
        {
            InitializeComponent();


        }

        protected override void OnAppearing()
        {
            Globals.BuildGlobals();
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

            Label header = new Label
            {
                Text = "Picker",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            Picker picker = new Picker
            {
                Title = "Color",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            AbsoluteLayout.SetLayoutBounds(picker, new Rectangle(0.5, 0.5, 1, 1));
            AbsoluteLayout.SetLayoutFlags(picker, AbsoluteLayoutFlags.All);
            absoluteLayout.Children.Add(picker);

            foreach (string colorName in Globals.colorDictionary.Keys)
            {
                picker.Items.Add(colorName);
            }


            picker.SelectedIndexChanged += (sender, args) =>
                    {
                        if (picker.SelectedIndex == -1)
                        {
                            Globals.primaryColor = Color.FromHex("#96d1ff");
                        }
                        else
                        {
                            string colorName = picker.Items[picker.SelectedIndex];
                            Globals.primaryColor = Globals.colorDictionary[colorName];
                        }
                    };

            #region learn more Button
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

        }

        private void savechangesButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void returnButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
