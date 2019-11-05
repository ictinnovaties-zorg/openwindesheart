using System;
using WindesHeartApp.Resources;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WindesHeartApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            BuildPage();
        }

        private void BuildPage()
        {
            layout.BackgroundColor = Globals.primaryColor;
            NavigationPage.SetHasNavigationBar(this, false);

            #region define Image 
            Grid grid = new Grid();
            AbsoluteLayout.SetLayoutFlags(grid, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(grid, new Rectangle(0.5, 0, 1, 0.3));
            layout.Children.Add(grid);

            Image windesheartImage = new Image();
            windesheartImage.Source = "WindesHeartTransparent.png";
            windesheartImage.BackgroundColor = Color.Transparent;
            windesheartImage.VerticalOptions = LayoutOptions.Start;
            windesheartImage.HorizontalOptions = LayoutOptions.Center;
            windesheartImage.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(execute: () => { Logo_Clicked(this, EventArgs.Empty); })
            });
            grid.Children.Add(windesheartImage);

            #endregion

            #region define Text
            Label aboutLabel = new Label
            {
                Text = "About",
                TextColor = Globals.lighttextColor,
                FontSize = Globals.screenHeight / 100 * 3,
            };
            AbsoluteLayout.SetLayoutFlags(aboutLabel, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(aboutLabel, new Rectangle(Globals.screenWidth / 15, 0.28, Globals.screenWidth, Globals.screenHeight / 100 * 3));
            layout.Children.Add(aboutLabel);

            Grid grid1 = new Grid();
            grid1.BackgroundColor = Color.Transparent;
            grid1.Margin = new Thickness(15, 0, 15, 0);
            AbsoluteLayout.SetLayoutFlags(grid1, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(grid1, new Rectangle(0, 0.5, Globals.screenWidth, Globals.screenHeight / 100 * 25));
            layout.Children.Add(grid1);

            Label writtenLabel = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                LineBreakMode = LineBreakMode.WordWrap,
                XAlign = TextAlignment.Center,
            };
            var formattedText = new FormattedString();
            formattedText.Spans.Add(new Span
            {
                Text = "This app is written by Windesheim Students\nIt's",
                FontSize = Globals.screenHeight / 100 * 2.5
            });
            formattedText.Spans.Add(new Span { Text = " main purpose", FontAttributes = FontAttributes.Bold, FontSize = Globals.screenHeight / 100 * 2.5 });
            formattedText.Spans.Add(new Span
            {
                Text = " is to",
                FontSize = Globals.screenHeight / 100 * 2.5
            });
            formattedText.Spans.Add(new Span { Text = " demo", FontAttributes = FontAttributes.Bold, FontSize = Globals.screenHeight / 100 * 2.5 });
            formattedText.Spans.Add(new Span
            {
                Text = " the ",
                FontSize = Globals.screenHeight / 100 * 2.5
            });
            formattedText.Spans.Add(new Span { Text = " WindesHeartSDK", FontAttributes = FontAttributes.Bold, FontSize = Globals.screenHeight / 100 * 2.5 });
            writtenLabel.FormattedText = formattedText;
            grid1.Children.Add(writtenLabel);

            FormattedString versionLabelText = new FormattedString();
            versionLabelText.Spans.Add(new Span { Text = "Version", FontSize = Globals.screenHeight / 100 * 2 });
            versionLabelText.Spans.Add(new Span { Text = " 2.0", FontSize = Globals.screenHeight / 100 * 2, FontAttributes = FontAttributes.Bold });
            Label versionLabel = new Label
            {
                HorizontalOptions = LayoutOptions.End,
                FontSize = Globals.screenHeight / 100 * 2,
                FormattedText = versionLabelText
            };
            AbsoluteLayout.SetLayoutBounds(versionLabel, new Rectangle(0.95, 0.80, Globals.screenHeight / 100 * 10, Globals.screenHeight / 100 * 5));
            AbsoluteLayout.SetLayoutFlags(versionLabel, AbsoluteLayoutFlags.PositionProportional);
            layout.Children.Add(versionLabel);

            #endregion

            #region learn more Button
            Button learnmoreButton = new Button();
            learnmoreButton.Text = "Learn More";
            learnmoreButton.BackgroundColor = Globals.secondaryColor;
            learnmoreButton.CornerRadius = (int)Globals.screenHeight / 100 * 7;
            AbsoluteLayout.SetLayoutBounds(learnmoreButton, new Rectangle(0.5, 0.70, Globals.screenHeight / 100 * 40, Globals.screenHeight / 100 * 7));
            AbsoluteLayout.SetLayoutFlags(learnmoreButton, AbsoluteLayoutFlags.PositionProportional);
            learnmoreButton.Clicked += learnmoreButton_Clicked;
            layout.Children.Add(learnmoreButton);
            #endregion

            #region define return Button
            //added extra grid behind imagebutton to make it clickable easier.
            Grid returnGrid = new Grid();
            AbsoluteLayout.SetLayoutBounds(returnGrid, new Rectangle(0.95, 0.95, Globals.screenHeight / 100 * 8, Globals.screenHeight / 100 * 8));
            AbsoluteLayout.SetLayoutFlags(returnGrid, AbsoluteLayoutFlags.PositionProportional);
            returnGrid.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(execute: () => { returnButton_Clicked(this, EventArgs.Empty); })
            });
            ImageButton returnButton = new ImageButton();
            returnButton.Source = "GoBack.png";
            returnButton.BackgroundColor = Color.Transparent;
            AbsoluteLayout.SetLayoutBounds(returnButton, new Rectangle(0.95, 0.95, Globals.screenHeight / 100 * 6, Globals.screenHeight / 100 * 6));
            AbsoluteLayout.SetLayoutFlags(returnButton, AbsoluteLayoutFlags.PositionProportional);
            returnButton.Clicked += returnButton_Clicked;
            layout.Children.Add(returnGrid);
            layout.Children.Add(returnButton);

            #endregion
        }

        private void learnmoreButton_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("OPEN GITHUB PAGE?? ");
        }

        private void returnButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void Logo_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("Logo - Clicked.");
            Navigation.PopAsync();
            Vibration.Vibrate(4200);
        }

        private void LearnMore_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("Learn More - Clicked.");
            Vibration.Vibrate(4200);
        }
    }
}
