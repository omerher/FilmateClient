using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using FilmateApp.Models;
using FilmateApp.Services;
using FilmateApp.Views;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;

namespace FilmateApp.ViewModels
{
    internal class GroupsViewModel : BaseViewModel
    {
        public GroupsViewModel()
        {
            Groups = new ObservableCollection<Chat>();
            GetGroups();
        }

        public ICommand NewGroupCommand => new Command(async () => {
            await PopupNavigation.Instance.PushAsync(new NewGroupPopupView());
            GetGroups();
        });

        private async void GetGroups()
        {
            FilmateAPIProxy proxy = FilmateAPIProxy.CreateProxy();
            List<Chat> userGroups = await proxy.GetGroups();
            Groups.Clear();
            foreach (Chat chat in userGroups)
            {
                chat.Icon = $"{proxy.baseUri}/imgs/{chat.Icon}";
                Groups.Add(chat);
            }
        }

        private ObservableCollection<Chat> groups;
        public ObservableCollection<Chat> Groups
        {
            get => groups;
            set => SetValue(ref groups, value);
        }

        private Chat selectedGroup;
        public Chat SelectedGroup
        {
            get => selectedGroup;
            set => SetValue(ref selectedGroup, value);
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set => SetValue(ref isRefreshing, value);
        }

        public ICommand RefreshCommand => new Command(() =>
        {
            IsRefreshing = true;
            GetGroups();
            IsRefreshing = false;
        });
    }
}
