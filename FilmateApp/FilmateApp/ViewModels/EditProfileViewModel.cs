using FilmateApp.Models;
using FilmateApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Xamarin.Essentials;

namespace FilmateApp.ViewModels
{
    class EditProfileViewModel : BaseViewModel
    {
        public EditProfileViewModel() : base()
        {
            FilmateAPIProxy proxy = FilmateAPIProxy.CreateProxy();

            Account = ((App)App.Current).CurrentAccount;
            Email = Account.Email;
            Username = Account.Username;
            Name = Account.AccountName;
            Age = Account.Age;
            ProfilePicture = $"{proxy.baseUri}/imgs/{Account.ProfilePicture}";
        }

        public Command SaveCommand => new Command(() => Save());
        public async void Save()
        {
            if (ValidateForm())
            {
                Loading = true;
                FilmateAPIProxy proxy = FilmateAPIProxy.CreateProxy();
                Account = ((App)App.Current).CurrentAccount;

                if (imageFileResult != null)
                {
                    bool uploadImageSuccess = await proxy.UploadImage(imageFileResult.FullPath, $"a{Account.AccountId}.jpg");
                    if (uploadImageSuccess)
                        Account.ProfilePicture = $"a{Account.AccountId}.jpg";
                }
                bool updateProfileSuccess = await proxy.UpdateProfile(Email, Username, Name, Age);
                if (updateProfileSuccess)
                {
                    Account.AccountName = Name;
                    Account.Age = Age;
                }

                Loading = false;
                await App.Current.MainPage.Navigation.PopAsync();
            }  
        }

        public Command ChangePfpCommand => new Command(() => ChangePfp());
        public async void ChangePfp() 
        {
            if (MediaPicker.IsCaptureSupported)
            {
                FileResult result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions()
                {
                    Title = "Pick a profile picture"
                });
                
                if (result != null)
                {
                    this.imageFileResult = result;

                    var stream = await result.OpenReadAsync();
                    ImageSource imgSource = ImageSource.FromStream(() => stream);
                    if (SetImageSourceEvent != null)
                        SetImageSourceEvent(imgSource);
                }
            }
            else
            {
                // add error popup
            }
        }

        private bool ValidateForm()
        {
            ValidateName();
            ValidateAge();

            return !(ShowNameError || ShowAgeError);
        }

        private Account account;
        public Account Account
        {
            get => account;
            set => SetValue(ref account, value);
        }

        private string profilePicture;
        public string ProfilePicture
        {
            get => profilePicture;
            set => SetValue(ref profilePicture, value);
        }

        private string email;
        public string Email
        {
            get => email;
            set => SetValue(ref email, value);
        }

        private string username;
        public string Username
        {
            get => username;
            set => SetValue(ref username, value);
        }

        private bool loading;
        public bool Loading
        {
            get => loading;
            set => SetValue(ref loading, value);
        }

        #region Age
        private bool showAgeError;

        public bool ShowAgeError
        {
            get => showAgeError;
            set => SetValue(ref showAgeError, value);
        }

        private int age;
        public int Age
        {
            get => age;
            set => SetValue(ref age, value);
        }

        private string ageError;
        public string AgeError
        {
            get => ageError;
            set => SetValue(ref ageError, value);
        }

        private void ValidateAge()
        {
            ShowAgeError = true;
            if (Age < 13)
                AgeError = "You must be older than 13 to sign up";
            else if (Age >= 100)
                AgeError = "Please enter a valid age";
            else
                ShowAgeError = false;
        }
        #endregion

        #region Name
        private bool showNameError;

        public bool ShowNameError
        {
            get => showNameError;
            set => SetValue(ref showNameError, value);
        }

        private string name;

        public string Name
        {
            get => name;
            set => SetValue(ref name, value);
        }

        private string nameError;

        public string NameError
        {
            get => nameError;
            set => SetValue(ref nameError, value);
        }

        private void ValidateName()
        {
            ShowNameError = string.IsNullOrEmpty(Name);
        }
        #endregion

        private FileResult imageFileResult;
        public event Action<ImageSource> SetImageSourceEvent;
    }
}