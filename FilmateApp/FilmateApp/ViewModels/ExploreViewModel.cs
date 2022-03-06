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
using FilmateApp.Models;
using System.Linq;

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
            GetPersonalSuggestions();
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

        public async void GetPersonalSuggestions()
        {
            PersonalSuggestions = new ObservableRangeCollection<Movie>();
            List<int> moviesIDs = new List<int>();
            Dictionary<int, int> keyValuePairs = new Dictionary<int, int>();

            List<LikedMovie> likedMovies = ((App)App.Current).CurrentAccount.LikedMovies;
            foreach (LikedMovie likedMovie in likedMovies)
            {
                SearchContainer<SearchMovie> results = await client.GetMovieRecommendationsAsync(likedMovie.MovieId);
                foreach (SearchMovie result in results.Results)
                {
                    moviesIDs.Add(result.Id);
                    try
                    {
                        keyValuePairs[result.Id]++;
                    }
                    catch
                    {
                        keyValuePairs[result.Id] = 1;
                    }
                }
            }

            var a = (from entry in keyValuePairs orderby entry.Value descending select entry).Take(20);
            foreach (KeyValuePair<int,int> result in a)
            {
                Movie movie = await client.GetMovieAsync(result.Key);
                Uri uri = client.GetImageUrl("w342", movie.PosterPath);
                movie.PosterPath = uri.AbsoluteUri;
                PersonalSuggestions.Add(movie);
            }
        }

        public Command SearchMoviesCommand => new Command(() => Push?.Invoke(new SearchView(MovieSearch)));
        public Command ExpandTrendingCommand => new Command(() => Push?.Invoke(new MovieListView(TrendingMovies, "Trending Movies")));
        public Command ExpandSuggestionsCommand => new Command(() => Push?.Invoke(new MovieListView(PersonalSuggestions, "Suggested For You")));

        public Command MovieCommand => new Command(() =>
        {
            if (SelectedMovie != null)
            {
                int id = SelectedMovie.Id;
                Push?.Invoke(new MovieView(id));
                SelectedMovie = null;
            }
        });

        private ObservableRangeCollection<Movie> trendingMovies;
        public ObservableRangeCollection<Movie> TrendingMovies
        {
            get => trendingMovies;
            set => SetValue(ref trendingMovies, value);
        }

        private ObservableRangeCollection<Movie> personalSuggestions;
        public ObservableRangeCollection<Movie> PersonalSuggestions
        {
            get => personalSuggestions;
            set => SetValue(ref personalSuggestions, value);
        }

        private Movie selectedMovie;
        public Movie SelectedMovie
        {
            get => selectedMovie;
            set => SetValue(ref selectedMovie, value);
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