using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmateApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FilmateApp.Services;
using FilmateApp.Models;

namespace FilmateApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupChatView : ContentPage
    {
        private GroupChatViewModel context;
        public GroupChatView(int chatId, ChatService chatService, List<string> groupIds)
        {
            InitializeComponent();
            context = new GroupChatViewModel(chatId, chatService, groupIds);
            context.MessagesLoaded += ScrollToBottom;
            this.BindingContext = context;
        }

        public void ScrollToBottom()
        {
            Msg lastMessage = context.Messages.FirstOrDefault();
            if (lastMessage != null)
                messages.ScrollTo(lastMessage, ScrollToPosition.End);
        }
    }
}