using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmateApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FilmateApp.Models;

namespace FilmateApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatInfoView : ContentPage
    {
        public ChatInfoView(Chat chat)
        {
            ChatInfoViewModel context = new ChatInfoViewModel(chat);
            this.BindingContext = context;
            InitializeComponent();
        }
    }
}