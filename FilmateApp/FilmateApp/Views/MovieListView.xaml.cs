using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmateApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.CommunityToolkit.ObjectModel;
using TMDbLib.Objects.Movies;

namespace FilmateApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MovieListView : ContentPage
    {
        public MovieListView(ObservableRangeCollection<Movie> movies, string title)
        {
            MovieListViewModel context = new MovieListViewModel(movies, title);
            context.Push += (p) => Navigation.PushAsync(p);
            this.BindingContext = context;
            InitializeComponent();
        }
    }
}