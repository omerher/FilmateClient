using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmateApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TMDbLib.Objects.Movies;
using System.Collections.ObjectModel;

namespace FilmateApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MovieListView : ContentPage
    {
        public MovieListView(ObservableCollection<Movie> movies, string title)
        {
            MovieListViewModel context = new MovieListViewModel(movies, title);
            context.Push += (p) => Navigation.PushAsync(p);
            this.BindingContext = context;
            InitializeComponent();

            CollectionView.ItemWidth = Application.Current.MainPage.Width * 0.29;
            CollectionView.ItemHeight = CollectionView.ItemWidth * 1.9;
        }
    }
}