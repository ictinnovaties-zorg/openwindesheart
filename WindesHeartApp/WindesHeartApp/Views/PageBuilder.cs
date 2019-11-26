using System;
using WindesHeartApp.Resources;
using Xamarin.Forms;

namespace WindesHeartApp.Pages
{
    //Builds and adds certain recurrent views to any AbsoluteLayout 
    public static class PageBuilder
    {
        public static void BuildPageBasics(AbsoluteLayout layout, object sender)
        {
            NavigationPage.SetHasNavigationBar((ContentPage)sender, false);
            layout.BackgroundColor = Globals.primaryColor;
            ((ContentPage)sender).Content = layout;
        }

        public static void AddHeaderImages(AbsoluteLayout layout)
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

        public static void AddReturnButton(AbsoluteLayout layout, object sender)
        {
            //added extra grid behind imagebutton to make it clickable easier.
            Grid returnGrid = new Grid();
            AbsoluteLayout.SetLayoutBounds(returnGrid,
                new Rectangle(0.95, 0.95, Globals.screenHeight / 100 * 8, Globals.screenHeight / 100 * 8));
            AbsoluteLayout.SetLayoutFlags(returnGrid, AbsoluteLayoutFlags.PositionProportional);
            returnGrid.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(execute: () => { returnButton_Clicked(sender, EventArgs.Empty); })
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
            Application.Current.MainPage.Navigation.PopAsync();
        }

        public static Label AddLabel(AbsoluteLayout absoluteLayout, string text, double x, double y, Color color)
        {
            Label label = new Label
            {
                Text = text,
                TextColor = color,
                FontSize = Globals.screenHeight / 100 * 3,
            };
            AbsoluteLayout.SetLayoutFlags(label, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(label, new Rectangle(x, y, -1, -1));
            absoluteLayout.Children.Add(label);
            return label;
        }

        public static Button AddButton(AbsoluteLayout absoluteLayout, string text, string bindingPath, double x, double y, double width, double height, AbsoluteLayoutFlags flags)
        {
            Button button = new Button
            {
                Text = text
            };
            button.SetBinding(Button.CommandProperty, new Binding() { Path = bindingPath });
            button.BackgroundColor = Globals.secondaryColor;
            AbsoluteLayout.SetLayoutFlags(button, flags);
            AbsoluteLayout.SetLayoutBounds(button, new Rectangle(x, y, width, height));
            absoluteLayout.Children.Add(button);
            return button;
        }
    }
}
