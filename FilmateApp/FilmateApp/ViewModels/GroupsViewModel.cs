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
using System.Linq;

namespace FilmateApp.ViewModels
{
    internal class GroupsViewModel : BaseViewModel
    {
        private readonly ChatService chatService;
        public GroupsViewModel()
        {
            Groups = new ObservableCollection<Chat>();
            GetGroups();
            TabControlViewModel.RefreshGroups += GetGroups;

            chatService = new ChatService();
        }

        public ICommand NewGroupCommand => new Command(async () => {
            await PopupNavigation.Instance.PushAsync(new NewGroupPopupView());
            GetGroups();
        });

        public ICommand GroupCommand => new Command(async () =>
        {
            if (SelectedGroup != null)
            {
                int chatId = SelectedGroup.ChatId;
                await App.Current.MainPage.Navigation.PushAsync(new GroupChatView(chatId, chatService));
                SelectedGroup = null;

            }
        });

        public async void GetGroups()
        {
            FilmateAPIProxy proxy = FilmateAPIProxy.CreateProxy();
            List<Chat> userGroups = await proxy.GetGroups();
            Groups.Clear();
            foreach (Chat chat in userGroups)
            {
                chat.Icon = $"{proxy.baseUri}/imgs/{chat.Icon}";
                chat.LastMessage = chat.Msgs.OrderByDescending(m => m.SentDate).FirstOrDefault();
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
