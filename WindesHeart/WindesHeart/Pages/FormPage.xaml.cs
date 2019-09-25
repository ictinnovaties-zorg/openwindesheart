using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindesHeart.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WindesHeart.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FormPage : ContentPage
    {
        private readonly INavigationService _navigationService;

        public string Sugar { get; set; }
        public string Carbs { get; set; }
        public int Bolus { get; set; }
        public FormPage()
        {
            InitializeComponent();
            SetSugar();
            _navigationService = App.WindesHeart;
        }

        private void SetSugar()
        {
            Random rnd = new Random();
            double sugar = rnd.Next(55, 75);
            SugarEntry.Text = (sugar / 10).ToString(CultureInfo.InvariantCulture);
        }

        private void ActivityBtnY_OnClicked(object sender, EventArgs e)
        {
            ButtonActive(ActivityBtnY);
            ButtonInactive(ActivityBtnN);
            BasaalLabel.IsVisible = true;
        }

        private void ActivityBtnN_OnClicked(object sender, EventArgs e)
        {
            ButtonActive(ActivityBtnN);
            ButtonInactive(ActivityBtnY);
            BasaalLabel.IsVisible = false;
        }

        private void ButtonActive(Button button)
        {
            button.BackgroundColor = Color.FromHex("2196F3");
            button.TextColor = Color.White;
        }

        private void ButtonInactive(Button button)
        {
            button.BackgroundColor = Color.White;
            button.TextColor = Color.FromHex("2196F3");
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            await _navigationService.GoBack();
        }

        private void SugarEntry_OnCompleted(object sender, EventArgs e)
        {
            CarbsEntry.Focus();
        }

        private void CarbsEntry_OnCompleted(object sender, EventArgs e)
        {
            var carbs = double.Parse(CarbsEntry.Text);
            double bolus = carbs / 6;
            BolusEntry.Text = Math.Round(bolus, 1, MidpointRounding.ToEven).ToString(CultureInfo.InvariantCulture);
        }
    }
}