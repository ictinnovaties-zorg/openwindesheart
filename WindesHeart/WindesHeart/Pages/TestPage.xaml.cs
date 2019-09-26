using System.Reflection;
using System.Resources;
using Xamarin.Forms;

namespace WindesHeart.Pages
{
    public partial class TestPage : ContentPage
    {
        private readonly ResourceManager _rm = new ResourceManager("WindesHeart.Resources.AppResources", typeof(TestPage).GetTypeInfo().Assembly);

        public TestPage()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            // Place test method here
            var result = TestFunction();
            resultLabel.Text = result.ToString();
        }

        private bool TestFunction()
        {
            return true;
        }

    }
}
