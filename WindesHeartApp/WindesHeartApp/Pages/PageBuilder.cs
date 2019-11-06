using System;
using WindesHeartApp.Resources;
using Xamarin.Forms;

namespace WindesHeartApp.Pages
{
    public static class PageBuilder
    {
        public static void BuildAndAddHeader(AbsoluteLayout layout)
        {
            Image heartonlyImage = new Image();
            heartonlyImage.Source = "HeartOnlyTransparent.png";
            AbsoluteLayout.SetLayoutFlags(heartonlyImage, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(heartonlyImage,
                new Rectangle(0.05, 0, Globals.screenWidth / 100 * 20, Globals.screenHeight / 100 * 10));
            layout.Children.Add(heartonlyImage);

            Image textonlyImage = new Image();
            textonlyImage.Source = "TextOnlyTransparent.png";
            AbsoluteLayout.SetLayoutFlags(textonlyImage, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(textonlyImage,
                new Rectangle(0.95, 0, Globals.screenWidth / 100 * 60, Globals.screenHeight / 100 * 10));
            layout.Children.Add(textonlyImage);

        }

        public static void BuildAndAddReturnButton(AbsoluteLayout layout, object sender)
        {
            //added extra grid behind imagebutton to make it clickable easier.
            Grid returnGrid = new Grid();
            AbsoluteLayout.SetLayoutBounds(returnGrid,
                new Rectangle(0.95, 0.95, Globals.screenHeight / 100 * 8, Globals.screenHeight / 100 * 8));
            AbsoluteLayout.SetLayoutFlags(returnGrid, AbsoluteLayoutFlags.PositionProportional);
            returnGrid.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(execute: () => { returnButton_Clicked2(sender, EventArgs.Empty); })
            });
            ImageButton returnButton = new ImageButton();
            returnButton.Source = "GoBack.png";
            returnButton.BackgroundColor = Color.Transparent;
            AbsoluteLayout.SetLayoutBounds(returnButton,
                new Rectangle(0.95, 0.95, Globals.screenHeight / 100 * 6, Globals.screenHeight / 100 * 6));
            AbsoluteLayout.SetLayoutFlags(returnButton, AbsoluteLayoutFlags.PositionProportional);
            returnButton.Clicked += returnButton_Clicked;
            layout.Children.Add(returnGrid);
            layout.Children.Add(returnButton);
        }

        private static void returnButton_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine(sender.ToString());
            ((ImageButton)sender).Navigation.PopAsync();

        }
        private static void returnButton_Clicked2(object sender, EventArgs e)
        {
            Console.WriteLine(sender.ToString());
            ((Page)sender).Navigation.PopAsync();

        }

    }
}
