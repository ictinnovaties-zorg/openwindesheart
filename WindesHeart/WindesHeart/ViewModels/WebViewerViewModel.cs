using System;
using Xamarin.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ComponentModel;
using System.Diagnostics;
using WindesHeartSdk.Services;

namespace WindesHeart.ViewModels
{
    public class WebViewerViewModel : INotifyPropertyChanged
    {
        public WebViewerViewModel()
        {
            GetChart();
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private HtmlWebViewSource _chart;

        public HtmlWebViewSource Chart
        {
            get => _chart;
            set
            {
                _chart = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Chart"));
            }
        }

        public async void GetChart()
        {
            // Prepare http request
            var chartUri = "http://insulinepredictionplatform.com/mobileChart.php";
            var accessToken = RestService.GetUserAccessToken();
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken.ToString());

            try
            {
                var response = await client.PostAsync(chartUri, null);
                if (response.IsSuccessStatusCode)
                {
                    // Update webviewer
                    var html = await response.Content.ReadAsStringAsync();
                    var htmlSource = new HtmlWebViewSource();
                    htmlSource.Html = html;
                    Chart = htmlSource;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

    }
}
