using FormsControls.Base;
using System;
using System.Diagnostics;
using WindesHeartApp.Resources;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WindesHeartApp.Pages
{
    public partial class AboutPage : ContentPage, IAnimationPage
    {
        public AboutPage()
        {
            InitializeComponent();
            BuildPage();
        }

        private void BuildPage()
        {
            absoluteLayout = new AbsoluteLayout();

            PageBuilder.BuildPageBasics(absoluteLayout, this);

            #region define Image 
            Grid grid = new Grid();
            AbsoluteLayout.SetLayoutFlags(grid, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(grid, new Rectangle(0.5, 0, 1, 0.3));
            absoluteLayout.Children.Add(grid);

            Image windesheartImage = new Image
            {
                Source = "WindesHeartTransparent.png",
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center
            };
            windesheartImage.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(execute: () => { Logo_Clicked(this, EventArgs.Empty); })
            });
            grid.Children.Add(windesheartImage);

            #endregion

            PageBuilder.AddLabel(absoluteLayout, "About", 0.05, 0.27, Globals.LightTextColor, "", 0);

            #region define Text

            Grid grid1 = new Grid { BackgroundColor = Color.Transparent, Margin = new Thickness(15, 0, 15, 0) };
            AbsoluteLayout.SetLayoutFlags(grid1, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(grid1,
                new Rectangle(0, 0.5, Globals.ScreenWidth, Globals.ScreenHeight / 100 * 25));
            absoluteLayout.Children.Add(grid1);

            Label writtenLabel = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                LineBreakMode = LineBreakMode.WordWrap,
                HorizontalTextAlignment = TextAlignment.Center,
            };
            var formattedText = new FormattedString();
            formattedText.Spans.Add(new Span
            {
                Text = "This app is written by Windesheim-students.\nIt's",
                FontSize = Globals.ScreenHeight / 100 * 2.5
            });
            formattedText.Spans.Add(new Span
            {
                Text = " main purpose",
                FontAttributes = FontAttributes.Bold,
                FontSize = Globals.ScreenHeight / 100 * 2.5
            });
            formattedText.Spans.Add(new Span
            {
                Text = " is to",
                FontSize = Globals.ScreenHeight / 100 * 2.5
            });
            formattedText.Spans.Add(new Span
            { Text = " demo", FontAttributes = FontAttributes.Bold, FontSize = Globals.ScreenHeight / 100 * 2.5 });
            formattedText.Spans.Add(new Span
            {
                Text = " the open source ",
                FontSize = Globals.ScreenHeight / 100 * 2.5
            });
            formattedText.Spans.Add(new Span
            {
                Text = "WindesHeartSDK.",
                FontAttributes = FontAttributes.Bold,
                FontSize = Globals.ScreenHeight / 100 * 2.5
            });
            writtenLabel.FormattedText = formattedText;
            grid1.Children.Add(writtenLabel);

            FormattedString versionLabelText = new FormattedString();
            versionLabelText.Spans.Add(new Span { Text = "Version", FontSize = Globals.ScreenHeight / 100 * 2 });
            versionLabelText.Spans.Add(new Span
            { Text = " 2.0", FontSize = Globals.ScreenHeight / 100 * 2, FontAttributes = FontAttributes.Bold });
            Label versionLabel = new Label
            {
                HorizontalOptions = LayoutOptions.End,
                FontSize = Globals.ScreenHeight / 100 * 2,
                FormattedText = versionLabelText
            };
            AbsoluteLayout.SetLayoutBounds(versionLabel,
                new Rectangle(0.95, 0.70, Globals.ScreenHeight / 100 * 10, Globals.ScreenHeight / 100 * 5));
            AbsoluteLayout.SetLayoutFlags(versionLabel, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(versionLabel);

            #endregion

            #region learn more Button

            Button learnmoreButton = new Button
            {
                Text = "Learn More",
                BackgroundColor = Globals.SecondaryColor,
                CornerRadius = (int)Globals.ScreenHeight / 100 * 7,
                FontSize = Globals.ScreenHeight / 100 * 2
            };
            AbsoluteLayout.SetLayoutBounds(learnmoreButton,
                new Rectangle(0.5, 0.85, Globals.ScreenHeight / 100 * 40, Globals.ScreenHeight / 100 * 7));
            AbsoluteLayout.SetLayoutFlags(learnmoreButton, AbsoluteLayoutFlags.PositionProportional);
            learnmoreButton.Clicked += LearnmoreButton_Clicked;
            absoluteLayout.Children.Add(learnmoreButton);
            #endregion

            PageBuilder.AddReturnButton(absoluteLayout, this);

        }

        private void LearnmoreButton_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("OPEN GITHUB PAGE?? ");
        }



        private void Logo_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("Logo - Clicked.");
            Navigation.PopAsync();
            Vibration.Vibrate(4200);
        }

        private void LearnMore_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("Learn More - Clicked.");
            Vibration.Vibrate(4200);
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