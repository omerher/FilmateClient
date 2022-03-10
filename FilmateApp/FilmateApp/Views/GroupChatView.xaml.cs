using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmateApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FilmateApp.Services;

namespace FilmateApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupChatView : ContentPage
    {
        public GroupChatView(int chatId, ChatService chatService)
        {
            GroupChatViewModel contex = new GroupChatViewModel(chatId, chatService);
            this.BindingContext = contex;
            InitializeComponent();
        }
    }
}