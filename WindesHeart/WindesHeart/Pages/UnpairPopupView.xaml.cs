using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using CarouselView.FormsPlugin.Abstractions;
using Rg.Plugins.Popup.Services;
using WindesHeart.Model;

using Xamarin.Forms.Xaml;

namespace WindesHeart.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UnpairPopupView
    {
        private readonly ResourceManager _rm = new ResourceManager("WindesHeart.Resources.AppResources", typeof(PairDevicePage).GetTypeInfo().Assembly);

        public UnpairPopupView()
        {
            InitializeComponent();

            PopulateCarousel();
        }

        private void PopulateCarousel()
        {
            var steps = new List<CarouselItem>();
            steps.Add(GetItemByName("UnpairPopup_Intro", "Swipe_Gesture"));
            steps.Add(GetItemByName("UnpairPopup_Step1"));
            steps.Add(GetItemByName("UnpairPopup_Step2"));
            steps.Add(GetItemByName("UnpairPopup_Final"));

            CarouselView.ItemsSource = steps;
        }

        private CarouselItem GetItemByName(string textSource, string image = "")
        {
            var text = _rm.GetString(textSource);
            return new CarouselItem()
            {
                Title = _rm.GetString($"{textSource}_Title"),
                Text = text?.Replace("\\n", Environment.NewLine),
                Image = image
            };
        }

        private void SkipBtn_OnClicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}