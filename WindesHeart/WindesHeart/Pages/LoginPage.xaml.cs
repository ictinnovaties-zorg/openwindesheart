using Xamarin.Forms.Xaml;
using WindesHeart.ViewModels;

namespace WindesHeart.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage
    {
        private readonly LoginViewModel _viewModel;
        public LoginPage()
        {
            _viewModel = new LoginViewModel();
            BindingContext = _viewModel;
            _viewModel.DisplayInvalidLoginPrompt += () => DisplayAlert("Error", "Invalid Login, try again", "OK");
            InitializeComponent();

            Username.Completed += (sender, e) =>
            {
                Password.Focus();
            };

            Password.Completed += (sender, e) =>
            {
                _viewModel.SubmitCommand.Execute(null);
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.AttemptAutoLogin();
        }
    }
}