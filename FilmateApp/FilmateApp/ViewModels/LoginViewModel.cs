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

namespace FilmateApp.ViewModels
{
    class LoginViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private FilmateAPIProxy proxy;

        public LoginViewModel()
        {
            EmailError = "Must be a valid email address";

            this.LoginCommand = new Command(() => Login());
            this.RegisterCommand = new Command(() => Register());

            proxy = FilmateAPIProxy.CreateProxy();
        }

        #region Email
        private bool showEmailError;

        public bool ShowEmailError
        {
            get => showEmailError;
            set
            {
                showEmailError = value;
                OnPropertyChanged("ShowEmailError");
            }
        }

        private string email;

        public string Email
        {
            get => email;
            set
            {
                email = value;
                //if (this.ShowEmailError)
                //    ValidateEmail();
                OnPropertyChanged("Email");
            }
        }

        private string emailError;

        public string EmailError
        {
            get => emailError;
            set
            {
                emailError = value;
                OnPropertyChanged("EmailError");
            }
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
            set
            {
                showPasswordError = value;
                OnPropertyChanged("ShowPasswordError");
            }
        }

        private string password;

        public string Password
        {
            get => password;
            set
            {
                password = value;
                //if (this.ShowEmailError)
                //    ValidatePassword();
                OnPropertyChanged("Password");
            }
        }

        private string passwordError;

        public string PasswordError
        {
            get => passwordError;
            set
            {
                passwordError = value;
                OnPropertyChanged("PasswordError");
            }
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
            set
            {
                showGeneralError = value;
                OnPropertyChanged("ShowGeneralError");
            }
        }

        private string generalError;

        public string GeneralError
        {
            get => generalError;
            set
            {
                generalError = value;
                OnPropertyChanged("GeneralError");
            }
        }
        #endregion

        #region Remember Me
        private bool rememberMeChecked;

        public bool RememberMeChecked
        {
            get => rememberMeChecked;
            set
            {
                rememberMeChecked = value;
                OnPropertyChanged("RememberMeChecked");
            }
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
                Account account = await proxy.Login(Email, Password);
                if (account != null)
                {
                    ((App)App.Current).CurrentAccount = account;

                    if (RememberMeChecked)
                    {
                        string token = await proxy.GenerateToken();
                        await SecureStorage.SetAsync("auth_token", token);
                    }

                    Push.Invoke(new TabControlView());
                }
                else
                {
                    GeneralError = "Unknown error occurred. Please try again later";
                }
                
            }
        }

        public Command RegisterCommand { protected set; get; }
        private void Register()
        {
            Push.Invoke(new RegisterView());
        }


        public event Action<Page> Push;

        
    }
}
