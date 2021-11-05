using System;
using System.Collections.Generic;
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

namespace FilmateApp.ViewModels
{
    class SearchViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private TMDbClient client;

        public SearchViewModel(string movieQuery)
        {
            this.client = new TMDbClient(App.APIKey);
            client.GetConfigAsync();

            MovieSearch = movieQuery;
            SearchMovies();

            this.FoundMovies = new ObservableCollection<Movie>();
            this.MovieCommand = new Command<Movie>((m) => GoToMovie(m));
            this.SearchMoviesCommand = new Command(() => SearchMovies());
        }

        public Command SearchMoviesCommand { protected set; get; }
        public async void SearchMovies()
        {
            FoundMovies = new ObservableCollection<Movie>();

            if (MovieSearch != null && MovieSearch != "")
            {
                SearchContainer<SearchMovie> results = await client.SearchMovieAsync(MovieSearch);

                foreach (SearchMovie result in results.Results)
                {
                    Movie movie = await client.GetMovieAsync(result.Id);
                    Uri uri = client.GetImageUrl("w342", movie.PosterPath);
                    movie.PosterPath = uri.AbsoluteUri;
                    FoundMovies.Add(movie);
                }
            };
        }

        #region Movie Search Input
        private string movieSearch;
        public string MovieSearch
        {
            get => movieSearch;
            set
            {
                movieSearch = value;
                OnPropertyChanged("MovieSearch");
            }
        }
        #endregion

        #region Found Movies
        private ObservableCollection<Movie> foundMovies;
        public ObservableCollection<Movie> FoundMovies
        {
            get => foundMovies;
            set
            {
                foundMovies = value;
                OnPropertyChanged("FoundMovies");
            }
        }
        #endregion

        public Command MovieCommand { protected set; get; }
        private void GoToMovie(Movie m)
        {
            Push.Invoke(new MovieView(m.Id));
        }

        public event Action<Page> Push;
    }
}
