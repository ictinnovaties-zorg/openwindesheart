using System;

using Xamarin.Forms;

namespace HeartRateDataBase.Pages
{
    public class WebViewer : ContentPage
    {
        public WebViewer()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

