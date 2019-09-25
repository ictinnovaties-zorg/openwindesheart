using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeartRateDataBase.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
        private INavigationService HeartRateDataBase { get; } = App.HeartRateDataBase;
        public HomePage ()
		{
			InitializeComponent ();
		}

        public HomePage(string parameter) : this()
        {
           
        }

        private void Sync_Clicked(object sender, EventArgs e)
        {
            (sender as Button).Text = "You pressed me!";
        }

    }
}