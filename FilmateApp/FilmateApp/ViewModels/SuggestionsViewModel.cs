using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.ComponentModel;
using FilmateApp.Models;
using Rg.Plugins.Popup.Services;
using FilmateApp.Views;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.General;
using System.Collections.ObjectModel;
using FilmateApp.Services;
using Xamarin.CommunityToolkit.ObjectModel;
using System.Windows.Input;

namespace FilmateApp.ViewModels
{
    public class SuggestionsViewModel : BaseViewModel
    {
        private int ogMovieId;
        public SuggestionsViewModel(int ogMovieId)
        {
            this.ogMovieId = ogMovieId;
            MoviesList = new ObservableRangeCollection<MovieSuggestionAccount>();
            LoadMovies();
        }

        public async void LoadMovies()
        {
            TMDbClient client = new TMDbClient(App.APIKey);
            await client.GetConfigAsync();
            FilmateAPIProxy proxy = FilmateAPIProxy.CreateProxy();

            MoviesList.Clear();
            List<Suggestion> suggestions = await proxy.GetSuggestions(ogMovieId);
            foreach (Suggestion suggestion in suggestions)
            {
                Movie movie = await client.GetMovieAsync(suggestion.SuggestionMovieId);
                Uri posterUrl = client.GetImageUrl("w500", movie.PosterPath);
                // movie.PosterPath = posterUrl.AbsoluteUri;
                MovieSuggestionAccount movieSuggestionAccount = new MovieSuggestionAccount(suggestion, posterUrl.AbsoluteUri);
                MoviesList.Add(movieSuggestionAccount);
            }
        }

        public async void NewSuggestion(Movie movie)
        {
            await PopupNavigation.Instance.PopAsync();
            FilmateAPIProxy proxy = FilmateAPIProxy.CreateProxy();
            bool success = await proxy.AddSuggestion(ogMovieId, movie.Id);
        }

        private ObservableRangeCollection<MovieSuggestionAccount> moviesList;
        public ObservableRangeCollection<MovieSuggestionAccount> MoviesList
        {
            get => moviesList;
            set => SetValue(ref moviesList, value);
        }
    }
}
