using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using FilmateApp.Models;
using FilmateApp.Services;

namespace FilmateApp.ViewModels
{
    public class ChatInfoViewModel : BaseViewModel
    {
        private FilmateAPIProxy proxy;
        public ChatInfoViewModel(Chat chat)
        {
            Chat = chat;
            proxy = FilmateAPIProxy.CreateProxy();

            GetInfo();
        }

        public void GetInfo()
        {
            GroupPicture = $"{proxy.baseUri}/imgs/{Chat.Icon}";
            Members = new ObservableCollection<Account>();
            foreach (ChatMember chatMember in Chat.ChatMembers)
            {
                Account a = chatMember.Account;
                //a.ProfilePicture = $"{proxy.baseUri}/imgs/{a.ProfilePicture}";
                Members.Add(a);
            }
        }

        private Chat chat;
        public Chat Chat
        {
            get => chat;
            set => SetValue(ref chat, value);
        }

        public ObservableCollection<Account> members;
        public ObservableCollection<Account> Members
        {
            get => members;
            set => SetValue(ref members, value);
        }

        private string groupPicture;
        public string GroupPicture
        {
            get => groupPicture;
            set => SetValue(ref groupPicture, value);
        }
    }
}
