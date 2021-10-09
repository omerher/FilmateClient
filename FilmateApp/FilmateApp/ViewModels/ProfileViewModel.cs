using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using FilmateApp.Views;
using System.ComponentModel;
using FilmateApp.Services;
using FilmateApp.Models;

namespace FilmateApp.ViewModels
{
    class ProfileViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public ProfileViewModel()
        {
            Account = ((App)App.Current).CurrentAccount;
            proxy = FilmateAPIProxy.CreateProxy();

            ProfilePicture = $"{proxy.baseUri}/imgs/{Account.ProfilePicture}";
        }

        private FilmateAPIProxy proxy;

        private Account account;

        public Account Account
        {
            get => account;
            set
            {
                account = value;
                OnPropertyChanged("Account");
            }
        }

        private string profilePicture;

        public string ProfilePicture
        {
            get => profilePicture;
            set
            {
                profilePicture = value;
                OnPropertyChanged("ProfilePicture");
            }
        }
    }
}
