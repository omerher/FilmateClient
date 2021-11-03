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
    public partial class SearchView : ContentView
    {
        public SearchView()
        {
            SearchViewModel context = new SearchViewModel();
            context.Push += (p) => Navigation.PushAsync(p);
            this.BindingContext = context;
            InitializeComponent();
        }
    }
}