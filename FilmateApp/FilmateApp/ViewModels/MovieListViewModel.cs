using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.General;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FilmateApp.Views;
using System.Threading;
using Xamarin.CommunityToolkit.ObjectModel;
using System.Windows.Input;

namespace FilmateApp.ViewModels
{
    public class MovieListViewModel : BaseViewModel
    {
        public MovieListViewModel(ObservableRangeCollection<Movie> movies, string title) : base()
        {
            MoviesList = movies;
            Title = title;
        }

        private ObservableRangeCollection<Movie> moviesList;
        public ObservableRangeCollection<Movie> MoviesList
        {
            get => moviesList;
            set => SetValue(ref moviesList, value);
        }

        private string title;
        public string Title
        {
            get => title;
            set => SetValue(ref title, value);
        }

        private int currentIndex;
        public int CurrentIndex
        {
            get => currentIndex;
            set => SetValue(ref currentIndex, value);
        }

        public ICommand MovieCommand => new Command(GoToMovie);
        public void GoToMovie(object obj)
        {
            Movie movie = (Movie)obj;
            Push?.Invoke(new MovieView(movie.Id));
        }


        public event Action<Page> Push;
    }
}
