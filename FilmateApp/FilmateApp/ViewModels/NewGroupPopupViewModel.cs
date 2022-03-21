using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using FilmateApp.Services;
using FilmateApp.Models;
using Rg.Plugins.Popup.Services;

namespace FilmateApp.ViewModels
{
    internal class NewGroupPopupViewModel : BaseViewModel
    {
        public NewGroupPopupViewModel()
        {

        }

        public Command ChangeIconCommand => new Command(() => ChangeIcon());
        public Command CreateGroupCommand => new Command(CreateGroup);
        private async void CreateGroup()
        {
            if (ValidateForm())
            {
                FilmateAPIProxy proxy = FilmateAPIProxy.CreateProxy();

                Chat c = new Chat()
                {
                    ChatName = GroupName,
                    ChatDescription = GroupDesc,
                    CreationDate = DateTime.Now,
                    Icon = "default_chat_pfp.png",
                    InviteCode = "1" // temp
                };
                Chat returnedGroup = await proxy.CreateGroup(c);

                if (imageFileResult != null)
                {
                    bool uploadImageSuccess = await proxy.UploadImage(imageFileResult.FullPath, $"g{returnedGroup.ChatId}.jpg", chatId: returnedGroup.ChatId);
                    if (uploadImageSuccess)
                        returnedGroup.Icon = $"g{returnedGroup.ChatId}.jpg";
                }

                if (returnedGroup != null)
                {
                    await PopupNavigation.Instance.PopAsync();
                }
                else
                {
                    GeneralError = "Something went wrong. Please try again later.";
                    ShowGeneralError = true;
                }
            }
        }

        private bool ValidateForm()
        {
            ValidateName();
            ValidateDescription();

            return !(ShowNameError || ShowDescError);
        }

        public async void ChangeIcon()
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

        private FileResult imageFileResult;
        public event Action<ImageSource> SetImageSourceEvent;

        #region Group Name
        private string groupName;
        public string GroupName
        {
            get => groupName;
            set => SetValue(ref groupName, value);
        }

        private string nameError;
        public string NameError
        {
            get => nameError;
            set => SetValue(ref nameError, value);
        }

        private bool showNameError;
        public bool ShowNameError
        {
            get => showNameError;
            set => SetValue(ref showNameError, value);
        }

        private void ValidateName()
        {
            NameError = "Name must not be blank";
            ShowNameError = string.IsNullOrEmpty(GroupName);
        }
        #endregion

        #region Group Description
        private string groupDesc;
        public string GroupDesc
        {
            get => groupDesc;
            set => SetValue(ref groupDesc, value);
        }

        private string descError;
        public string DescError
        {
            get => descError;
            set => SetValue(ref descError, value);
        }

        private bool showDescError;
        public bool ShowDescError
        {
            get => showDescError;
            set => SetValue(ref showDescError, value);
        }

        private void ValidateDescription()
        {
            DescError = "Description must not be blank";
            ShowDescError = string.IsNullOrEmpty(GroupDesc);
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
    }
}
