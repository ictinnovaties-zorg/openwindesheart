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
            layout.BackgroundColor = Globals.PrimaryColor;
            ((ContentPage)sender).Content = layout;
        }

        public static void AddHeaderImages(AbsoluteLayout layout)
        {
            Image heartonlyImage = new Image { Source = "HeartOnlyTransparent.png" };
            AbsoluteLayout.SetLayoutFlags(heartonlyImage, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(heartonlyImage, new Rectangle(0.05, 0, Globals.ScreenWidth / 100 * 20, Globals.ScreenHeight / 100 * 10));

            Image textonlyImage = new Image { Source = "TextOnlyTransparent.png" };
            AbsoluteLayout.SetLayoutFlags(textonlyImage, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(textonlyImage, new Rectangle(0.95, 0, Globals.ScreenWidth / 100 * 60, Globals.ScreenHeight / 100 * 10));

            layout.Children.Add(heartonlyImage);
            layout.Children.Add(textonlyImage);
        }

        public static void AddReturnButton(AbsoluteLayout layout, object sender)
        {
            //added extra grid behind imagebutton to make it clickable easier.
            Grid returnGrid = new Grid();
            returnGrid.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(execute: () => { ReturnButton_Clicked(sender, EventArgs.Empty); })
            });
            AbsoluteLayout.SetLayoutBounds(returnGrid, new Rectangle(0.95, 0.95, Globals.ScreenHeight / 100 * 8.5, Globals.ScreenHeight / 100 * 8.5));
            AbsoluteLayout.SetLayoutFlags(returnGrid, AbsoluteLayoutFlags.PositionProportional);

            ImageButton returnButton = new ImageButton { Source = "GoBack.png", BackgroundColor = Color.Transparent };
            returnButton.Clicked += ReturnButton_Clicked;
            AbsoluteLayout.SetLayoutBounds(returnButton, new Rectangle(0.95, 0.95, Globals.ScreenHeight / 100 * 6, Globals.ScreenHeight / 100 * 6));
            AbsoluteLayout.SetLayoutFlags(returnButton, AbsoluteLayoutFlags.PositionProportional);

            layout.Children.Add(returnGrid);
            layout.Children.Add(returnButton);
        }

        private static void ReturnButton_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        public static ActivityIndicator AddActivityIndicator(AbsoluteLayout layout, string bindingPath, double x, double y, double width, double height, AbsoluteLayoutFlags flags, Color color)
        {
            ActivityIndicator indicator = new ActivityIndicator { Color = color };
            indicator.SetBinding(ActivityIndicator.IsRunningProperty, new Binding() { Path = bindingPath });
            AbsoluteLayout.SetLayoutBounds(indicator, new Rectangle(x, y, width, height));
            AbsoluteLayout.SetLayoutFlags(indicator, flags);
            layout.Children.Add(indicator);
            return indicator;
        }
        public static Label AddLabel(AbsoluteLayout absoluteLayout, string text, double x, double y, Color color, string bindingPath, int fontSize)
        {
            Label label = new Label
            {
                Text = text,
                TextColor = color,
                FontSize = Globals.ScreenHeight / 100 * 3

            };

            AbsoluteLayout.SetLayoutFlags(label, AbsoluteLayoutFlags.PositionProportional);
            if (!string.IsNullOrEmpty(bindingPath))
                label.SetBinding(Label.TextProperty, new Binding() { Path = bindingPath });

            AbsoluteLayout.SetLayoutBounds(label, new Rectangle(x, y, -1, -1));
            if (fontSize != 0)
                label.FontSize = fontSize;

            absoluteLayout.Children.Add(label);
            return label;
        }

        public static Button AddButton(AbsoluteLayout absoluteLayout, string text, EventHandler onclick, double x, double y, double width, double height, int cornerradius, int fontsize, AbsoluteLayoutFlags flags, Color backgroundColor)
        {
            Button button = new Button() { Text = text };
            if (fontsize != 0)
                button.FontSize = fontsize;
            button.BackgroundColor = backgroundColor;
            if (cornerradius != 0)
                button.CornerRadius = cornerradius;
            button.Clicked += onclick;
            AbsoluteLayout.SetLayoutFlags(button, flags);
            AbsoluteLayout.SetLayoutBounds(button, new Rectangle(x, y, width, height));
            absoluteLayout.Children.Add(button);
            return button;
        }
    }
}
