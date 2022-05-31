using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmateApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FilmateApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupsView : ContentView
    {
        private GroupsViewModel context;
        public GroupsView()
        {
            context = new GroupsViewModel();
            this.BindingContext = context;
            InitializeComponent();
        }
    }
}