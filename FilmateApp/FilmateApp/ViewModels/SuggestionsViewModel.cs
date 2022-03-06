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

        public ICommand RefreshCommand => new Command(() =>
        {
            IsRefreshing = true;
            LoadMovies();
            IsRefreshing = false;
        });

        public ICommand PressedMovie => new Command<int>(async (id) => await ((App)App.Current).MainPage.Navigation.PushAsync(new MovieView(id)));

        public ICommand LongPressedMovie => new Command<MovieSuggestionAccount>(async (msa) =>
        {
            await PopupNavigation.Instance.PushAsync(new SuggestionDetailsPopupView(msa));
        });

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
                MovieSuggestionAccount movieSuggestionAccount = new MovieSuggestionAccount(suggestion, posterUrl.AbsoluteUri, movie: movie);
                MoviesList.Add(movieSuggestionAccount);
            }
        }

        public async void NewSuggestion(Movie movie)
        {
            if (movie.Id != ogMovieId)
            {
                await PopupNavigation.Instance.PopAsync();
                FilmateAPIProxy proxy = FilmateAPIProxy.CreateProxy();
                bool success = await proxy.AddSuggestion(ogMovieId, movie.Id);
            }
        }

        private ObservableRangeCollection<MovieSuggestionAccount> moviesList;
        public ObservableRangeCollection<MovieSuggestionAccount> MoviesList
        {
            get => moviesList;
            set => SetValue(ref moviesList, value);
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set => SetValue(ref isRefreshing, value);
        }
    }
}
