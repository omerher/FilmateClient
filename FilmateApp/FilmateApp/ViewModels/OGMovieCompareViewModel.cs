using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using FilmateApp.Models;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.General;
using FilmateApp.Services;
using Xamarin.CommunityToolkit.ObjectModel;
using System.Windows.Input;

namespace FilmateApp.ViewModels
{
    public class OGMovieCompareViewModel : BaseViewModel
    {
        private string comparisionType;
        public OGMovieCompareViewModel(string comparisionType)
        {
            this.comparisionType = comparisionType;

            LoadProfile();
        }


        public async void LoadVotesHistory()
        {
            TMDbClient client = new TMDbClient(App.APIKey);
            await client.GetConfigAsync();
            FilmateAPIProxy proxy = FilmateAPIProxy.CreateProxy();

            ComparedMovies = new ObservableCollection<MovieSuggestionAccount>();
            List<AccountVotesHistory> accountVotes = Account.AccountVotesHistories;
            foreach (AccountVotesHistory vote in accountVotes)
            {
                Movie movie = await client.GetMovieAsync(vote.Suggestion.SuggestionMovieId);
                Uri posterUrl = client.GetImageUrl("w500", movie.PosterPath);

                Movie OGMovie = await client.GetMovieAsync(vote.Suggestion.OriginalMovieId);
                Uri OGPosterUrl = client.GetImageUrl("w500", OGMovie.PosterPath);
                MovieSuggestionAccount movieSuggestionAccount = new MovieSuggestionAccount(vote.Suggestion, posterUrl.AbsoluteUri, OGPosterUrl.AbsoluteUri);
                ComparedMovies.Add(movieSuggestionAccount);
            }
        }

        public async void LoadSuggestions()
        {
            TMDbClient client = new TMDbClient(App.APIKey);
            await client.GetConfigAsync();
            FilmateAPIProxy proxy = FilmateAPIProxy.CreateProxy();

            ComparedMovies = new ObservableCollection<MovieSuggestionAccount>();
            List<Suggestion> accountSuggestions = Account.Suggestions;
            foreach (Suggestion suggestion in accountSuggestions)
            {
                Movie movie = await client.GetMovieAsync(suggestion.SuggestionMovieId);
                Uri posterUrl = client.GetImageUrl("w500", movie.PosterPath);

                Movie OGMovie = await client.GetMovieAsync(suggestion.OriginalMovieId);
                Uri OGPosterUrl = client.GetImageUrl("w500", OGMovie.PosterPath);
                MovieSuggestionAccount movieSuggestionAccount = new MovieSuggestionAccount(suggestion, posterUrl.AbsoluteUri, OGPosterUrl.AbsoluteUri);
                ComparedMovies.Add(movieSuggestionAccount);
            }
        }

        public void LoadProfile()
        {
            Account = ((App)App.Current).CurrentAccount;
            if (comparisionType == "Suggestions")
                LoadSuggestions();
            if (comparisionType == "Your Votes")
                LoadVotesHistory();

        }


        private Account account;
        public Account Account
        {
            get => account;
            set => SetValue(ref account, value);
        }

        private ObservableCollection<MovieSuggestionAccount> comparedMovies;
        public ObservableCollection<MovieSuggestionAccount> ComparedMovies
        {
            get => comparedMovies;
            set => SetValue(ref comparedMovies, value);
        }
    }
}
