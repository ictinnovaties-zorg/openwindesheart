using System;
using System.Collections.Generic;
using Xamarin.Forms;
using WindesHeart.ViewModels;


namespace WindesHeart.Pages
{
    public partial class WebViewer : ContentPage
    {
        private readonly WebViewerViewModel _viewModel;
        public WebViewer()
        {

            InitializeComponent();

            _viewModel = new WebViewerViewModel();
            BindingContext = _viewModel;



        }

    }
}
