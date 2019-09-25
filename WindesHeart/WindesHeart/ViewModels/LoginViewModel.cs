using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr;
using WindesHeartSdk.Services;
using WindesHeart.Services;
using Xamarin.Forms;

namespace WindesHeart.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public Action DisplayInvalidLoginPrompt;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private readonly INavigationService _navigationService;

        private string _username;
        private string _password;
        private bool _isBusy;

        public ICommand SubmitCommand { protected set; get; }

        public LoginViewModel()
        {
            _navigationService = App.WindesHeart;
            SubmitCommand = new Command(OnSubmit);
            FillCredentials();
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Username"));
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsBusy"));
            }
        }

        public async void OnSubmit()
        {
            if (_username.IsEmpty() || _password.IsEmpty())
            {
                return;
            }
            await Login(_username, _password);
        }

        public async Task Login(string username, string password)
        {
            IsBusy = true;
            var success = await RestService.Login(username, password);
            IsBusy = false;

            if (success)
            {
                Application.Current.Properties["username"] = username;
                Application.Current.Properties["password"] = password;

                await _navigationService.NavigateAsync(nameof(MainPage));
            }
            else
            {
                DisplayInvalidLoginPrompt.Invoke();
            }
        }

        public async void AttemptAutoLogin()
        {
            var username = "";
            var password = "";

            if (Application.Current.Properties.ContainsKey("username"))
            {
                username = (string)Application.Current.Properties["username"];
            }

            if (Application.Current.Properties.ContainsKey("password"))
            {
                password = (string)Application.Current.Properties["password"];
            }

            if (!username.IsEmpty() && !password.IsEmpty())
            {

                IsBusy = true;
                await RestService.Login(username, password);
                IsBusy = false;

                await _navigationService.NavigateAsync(nameof(MainPage));
            }
        }

        private void FillCredentials()
        {
            if (Application.Current.Properties.ContainsKey("username"))
            {
                Username = (string)Application.Current.Properties["username"];
            }

            if (Application.Current.Properties.ContainsKey("password"))
            {
                Password = (string)Application.Current.Properties["password"];
            }
        }
    }
}
