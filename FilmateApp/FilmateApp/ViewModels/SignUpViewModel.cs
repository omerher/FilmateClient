using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using FilmateApp.Views;
using System.ComponentModel;
using FilmateApp.Services;
using FilmateApp.Models;

namespace FilmateApp.ViewModels
{
    class SignUpViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private FilmateAPIProxy proxy;

        public SignUpViewModel()
        {
            this.GoToLoginCommand = new Command(() => GoToLogin());
            this.SignUpCommand = new Command(() => SignUp());

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

        private async void ValidateEmail()
        {
            bool? exists = await proxy.EmailExists(Email);
            if (exists == true)
            {
                this.ShowEmailError = true;
                this.EmailError = "Email address already exists";
            }
            else if (exists == null)
            {
                ShowGeneralError = true;
                GeneralError = "Unknown error occured. Try again later";
            }
            else
            {
                try
                {
                    var addr = new System.Net.Mail.MailAddress(Email);
                    this.ShowEmailError = addr.Address != Email;
                }
                catch
                {
                    EmailError = "Invalid email address";
                    this.ShowEmailError = true;
                }
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

            ShowConfirmPasswordError = true;
            if (string.IsNullOrEmpty(ConfirmPassword))
                ConfirmPasswordError = "Confirm Password cannot be blank";
            else if (ConfirmPassword != Password)
                ConfirmPasswordError = "Passwords must match";
            else
                ShowConfirmPasswordError = false;
        }
        #endregion

        #region Confirm Password
        private bool showConfirmPasswordError;

        public bool ShowConfirmPasswordError
        {
            get => showConfirmPasswordError;
            set
            {
                showConfirmPasswordError = value;
                OnPropertyChanged("ShowConfirmPasswordError");
            }
        }

        private string confirmPassword;

        public string ConfirmPassword
        {
            get => confirmPassword;
            set
            {
                confirmPassword = value;
                //if (this.ShowEmailError)
                //    ValidateConfirmPassword();
                OnPropertyChanged("ConfirmPassword");
            }
        }

        private string confirmPasswordError;

        public string ConfirmPasswordError
        {
            get => confirmPasswordError;
            set
            {
                confirmPasswordError = value;
                OnPropertyChanged("ConfirmPasswordError");
            }
        }
        #endregion

        #region Username
        private bool showUsernameError;

        public bool ShowUsernameError
        {
            get => showUsernameError;
            set
            {
                showUsernameError = value;
                OnPropertyChanged("ShowUsernameError");
            }
        }

        private string username;

        public string Username
        {
            get => username;
            set
            {
                username = value;
                //if (this.ShowEmailError)
                //    ValidateUsername();
                OnPropertyChanged("Username");
            }
        }

        private string usernameError;

        public string UsernameError
        {
            get => usernameError;
            set
            {
                usernameError = value;
                OnPropertyChanged("UsernameError");
            }
        }

        private async void ValidateUsername()
        {
            ShowUsernameError = false;
            bool? exists = await proxy.UsernameExists(Username);

            if (string.IsNullOrEmpty(Username))
            {
                UsernameError = "Username cannot be blank";
                ShowUsernameError = true;
            }
            else if (exists == true)
            {
                UsernameError = "Username is already taken";
                ShowUsernameError = true;
            }
            else if (exists == null)
            {
                GeneralError = "Unknown error occured. Try again later";
                ShowGeneralError = true;
            }
        }
        #endregion

        #region Age
        private bool showAgeError;

        public bool ShowAgeError
        {
            get => showAgeError;
            set
            {
                showAgeError = value;
                OnPropertyChanged("ShowAgeError");
            }
        }

        private int? age;

        public int? Age
        {
            get => age;
            set
            {
                age = value;
                //if (this.ShowEmailError)
                //    ValidateAge();
                OnPropertyChanged("Age");
            }
        }

        private string ageError;

        public string AgeError
        {
            get => ageError;
            set
            {
                ageError = value;
                OnPropertyChanged("AgeError");
            }
        }

        private void ValidateAge()
        {
            ShowAgeError = true;
            if (Age == null)
                AgeError = "Age cannot be blank";
            else if (Age < 13)
                AgeError = "You must be older than 13 to sign up";
            else if (Age >= 100)
                AgeError = "Please enter a valid age";
            else
                ShowAgeError = false;
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

        #region Loading
        private bool loading;
        public bool Loading
        {
            get => loading;
            set
            {
                loading = value;
                OnPropertyChanged("Loading");
            }
        }
        #endregion

        private bool ValidateForm()
        {
            ValidateEmail();
            ValidatePassword();
            ValidateUsername();
            ValidateAge();

            return !(ShowEmailError || ShowPasswordError || ShowConfirmPasswordError || ShowUsernameError || ShowAgeError);
        }

        public Command GoToLoginCommand { protected set; get; }
        private void GoToLogin()
        {

        }

        public Command SignUpCommand { protected set; get; }
        private async void SignUp()
        {
            Loading = true;
            if (ValidateForm())
            {
                Account account = await proxy.SignUpAccount(Email, Password, Username, (int)Age);
                if (account != null)
                {
                    ((App)App.Current).CurrentAccount = account;
                    Push?.Invoke(new TabControlView());
                }
            }

            Loading = false;
        }

        public event Action<Page> Push;
    }
}
