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
using FilmateApp.Models;

namespace FilmateApp.ViewModels
{
    public class MovieListViewModel : BaseViewModel
    {
        private ObservableCollection<Movie> moviesReference;
        public MovieListViewModel(ObservableCollection<Movie> movies, string title) : base()
        {
            MoviesList = movies;
            moviesReference = movies;
            Title = title;
        }

        private ObservableCollection<Movie> moviesList;
        public ObservableCollection<Movie> MoviesList
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

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set => SetValue(ref isRefreshing, value);
        }

        public ICommand MovieCommand => new Command(GoToMovie);
        public void GoToMovie(object obj)
        {
            Movie movie = (Movie)obj;
            Push?.Invoke(new MovieView(movie.Id));
        }

        public ICommand RefreshCommand => new Command(Refresh);
        public void Refresh()
        {
            IsRefreshing = true;
            if (Title == "Liked Movies")
            {
                MoviesList.Clear();
                List<LikedMovie> lst = ((App)App.Current).CurrentAccount.LikedMovies;
                foreach (LikedMovie likedMovie in lst)
                {
                    MoviesList.Add(likedMovie.Movie);
                }
            }

            IsRefreshing = false;
        }

        public event Action<Page> Push;
    }
}
