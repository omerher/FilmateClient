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
using Xamarin.CommunityToolkit.ObjectModel;

namespace FilmateApp.ViewModels
{
    class ExploreViewModel : BaseViewModel
    {
        private TMDbClient client;
        public ExploreViewModel() : base()
        {
            this.client = new TMDbClient(App.APIKey);
            client.GetConfigAsync();

            GetTrendingMovies();
        }

        public async void GetTrendingMovies()
        {
            TrendingMovies = new ObservableRangeCollection<Movie>();

            SearchContainer<SearchMovie> results = await client.GetTrendingMoviesAsync(TMDbLib.Objects.Trending.TimeWindow.Week);

            foreach (SearchMovie result in results.Results)
            {
                Movie movie = await client.GetMovieAsync(result.Id);
                Uri uri = client.GetImageUrl("w342", movie.PosterPath);
                movie.PosterPath = uri.AbsoluteUri;
                TrendingMovies.Add(movie);
            }
        }

        public Command SearchMoviesCommand => new Command(() => Push?.Invoke(new SearchView(MovieSearch)));
        public Command ExpandMoviesCommand => new Command<ObservableRangeCollection<Movie>>((l) => Push?.Invoke(new MovieListView(l, "Trending Movies")));
        public Command MovieCommand => new Command<Movie>((m) => Push?.Invoke(new MovieView(m.Id)));

        private ObservableRangeCollection<Movie> trendingMovies;
        public ObservableRangeCollection<Movie> TrendingMovies
        {
            get => trendingMovies;
            set => SetValue(ref trendingMovies, value);
        }

        private string movieSearch;
        public string MovieSearch
        {
            get => movieSearch;
            set => SetValue(ref movieSearch, value);
        }

        public event Action<Page> Push;
    }
}
