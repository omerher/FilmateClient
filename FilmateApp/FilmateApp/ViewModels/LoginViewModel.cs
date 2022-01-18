using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using FilmateApp.Views;
using System.ComponentModel;
using FilmateApp.Services;
using FilmateApp.Models;
using Xamarin.Essentials;
using System.Threading.Tasks;
using System.Linq;

namespace FilmateApp.ViewModels
{
    class LoginViewModel : BaseViewModel
    {
        private FilmateAPIProxy proxy;

        public LoginViewModel()
        {
            EmailError = "Must be a valid email address";
            Loading = false;

            this.LoginCommand = new Command(() => Login());
            this.RegisterCommand = new Command(() => Register());

            proxy = FilmateAPIProxy.CreateProxy();
        }

        #region Email
        private bool showEmailError;

        public bool ShowEmailError
        {
            get => showEmailError;
            set => SetValue(ref showEmailError, value);
        }

        private string email;

        public string Email
        {
            get => email;
            set => SetValue(ref email, value);
        }

        private string emailError;

        public string EmailError
        {
            get => emailError;
            set => SetValue(ref emailError, value);
        }

        private void ValidateEmail()
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                this.ShowEmailError = addr.Address != email;
            }
            catch
            {
                this.ShowEmailError = true;
            }

        }
        #endregion

        #region Password
        private bool showPasswordError;

        public bool ShowPasswordError
        {
            get => showPasswordError;
            set => SetValue(ref showPasswordError, value);
        }

        private string password;

        public string Password
        {
            get => password;
            set => SetValue(ref password, value);
        }

        private string passwordError;

        public string PasswordError
        {
            get => passwordError;
            set => SetValue(ref passwordError, value);
        }

        private void ValidatePassword()
        {
            ShowPasswordError = true;
            if (string.IsNullOrEmpty(Password))
                PasswordError = "Password cannot be blank";
            else if (Password.Length < 8)
                PasswordError = "Password must be more than 8 characters";
            else
                ShowPasswordError = false;
        }
        #endregion

        #region General Error
        private bool showGeneralError;

        public bool ShowGeneralError
        {
            get => showGeneralError;
            set => SetValue(ref showGeneralError, value);
        }

        private string generalError;

        public string GeneralError
        {
            get => generalError;
            set => SetValue(ref generalError, value);
        }
        #endregion

        #region Remember Me
        private bool rememberMeChecked;

        public bool RememberMeChecked
        {
            get => rememberMeChecked;
            set => SetValue(ref rememberMeChecked, value);
        }
        #endregion

        #region Loading
        private bool loading;

        public bool Loading
        {
            get => loading;
            set => SetValue(ref loading, value);
        }
        #endregion

        private bool ValidateForm()
        {
            if (((App)App.Current).CurrentAccount != null)
            {
                GeneralError = "You are already logged in";
                return false;
            }

            ValidateEmail();
            ValidatePassword();

            return !(ShowEmailError || ShowPasswordError);
        }

        public Command LoginCommand { protected set; get; }
        private async void Login()
        {
            if (ValidateForm())
            {
                Loading = true;
                Account account = await proxy.Login(Email, Password);
                if (account != null)
                {
                    ((App)App.Current).CurrentAccount = account;

                    if (RememberMeChecked)
                    {
                        string token = await proxy.GenerateToken();
                        await SecureStorage.SetAsync("auth_token", token);
                    }
                    Loading = false;
                    await PopAllTo(new TabControlView());
                }
                else
                {
                    GeneralError = "Unknown error occurred. Please try again later";
                }
            }
            Loading = false;
        }

        public async Task PopAllTo(Page page) // clear navigation stack and goes to page
        {
            App.Current.MainPage.Navigation.InsertPageBefore(page, App.Current.MainPage.Navigation.NavigationStack.First());
            await App.Current.MainPage.Navigation.PopToRootAsync();
        }


        public Command RegisterCommand { protected set; get; }
        private void Register()
        {
            Push.Invoke(new RegisterView());
        }


        public event Action<Page> Push;

        
    }
}
