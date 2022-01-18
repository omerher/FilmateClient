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
using System.Windows.Input;

namespace FilmateApp.ViewModels
{
    public class SuggestionPopupViewModel : BaseViewModel
    {
        private TMDbClient client;
        private SuggestionsViewModel suggestionsViewModel;
        public SuggestionPopupViewModel(SuggestionsViewModel suggestionsViewModel)
        {
            this.suggestionsViewModel = suggestionsViewModel;
            this.client = new TMDbClient(App.APIKey);
            client.GetConfigAsync();

            this.FoundMovies = new ObservableCollection<Movie>();
        }

        public Command SearchMoviesCommand => new Command(() => SearchMovies());
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
                    if (movie.PosterPath != null)
                    {
                        movie.PosterPath = uri.AbsoluteUri;
                        FoundMovies.Add(movie);
                    }
                }
            };
        }

        public ICommand SelectedMovieCommand => new Command(SelectedMovie);
        public void SelectedMovie(object obj)
        {
            Movie movie = (Movie)obj;
            suggestionsViewModel.NewSuggestion(movie);
        }

        private string movieSearch;
        public string MovieSearch
        {
            get => movieSearch;
            set => SetValue(ref movieSearch, value);
        }

        private ObservableCollection<Movie> foundMovies;
        public ObservableCollection<Movie> FoundMovies
        {
            get => foundMovies;
            set => SetValue(ref foundMovies, value);
        }
    }
}
