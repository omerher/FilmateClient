using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using FilmateApp.Views;
using System.ComponentModel;
using FilmateApp.Services;
using FilmateApp.Models;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;
using System.Collections.ObjectModel;

namespace FilmateApp.ViewModels
{
    class ProfileViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public ProfileViewModel()
        {
            Account = ((App)App.Current).CurrentAccount;
            LikedMovies = new ObservableCollection<Movie>();
            this.MovieCommand = new Command<Movie>((m) => GoToMovie(m));
            this.SuggestionsCommand = new Command(() => GoToSuggestions());
            this.VotesHistoryCommand = new Command(() => GoToVotesHistory());
            proxy = FilmateAPIProxy.CreateProxy();

            // to remove after server is working
            Account a = new Account()
            {
                AccountName = "Omer Hershkovitz",
                AccountId = 1,
                ProfilePicture = "https://i.pinimg.com/564x/8f/e6/66/8fe66626ec212bb54e13fa94e84c105c.jpg",
                Username = "omerhershkovitz",
                LikedMovies = new List<LikedMovie>()
                {
                    new LikedMovie()
                    {
                        AccountId = 1,
                        MovieId = 580489
                    },
                    new LikedMovie()
                    {
                        AccountId = 1,
                        MovieId = 157336
                    },
                    new LikedMovie()
                    {
                        AccountId = 1,
                        MovieId = 155
                    },
                    new LikedMovie()
                    {
                        AccountId = 1,
                        MovieId = 475557
                    }
                }
            };
            Account = a;

            if (1 == 2) // Account == null
                ProfilePicture = $"{proxy.baseUri}/imgs/{Account.ProfilePicture}";
            else
                ProfilePicture = "https://i.pinimg.com/564x/8f/e6/66/8fe66626ec212bb54e13fa94e84c105c.jpg";

            GetLikedMoviesAsync();
        }

        private async void GetLikedMoviesAsync()
        {
            await GetLikedMovies();
        }

        public async Task<bool> GetLikedMovies()
        {
            TMDbClient client = new TMDbClient(App.APIKey);
            await client.GetConfigAsync();

            foreach (LikedMovie likedMovie in Account.LikedMovies)
            {
                Movie movie = await client.GetMovieAsync(likedMovie.MovieId);
                Uri posterUrl = client.GetImageUrl("w342", movie.PosterPath);
                movie.PosterPath = posterUrl.AbsoluteUri;
                LikedMovies.Add(movie);
            }

            return true;
        }

        private FilmateAPIProxy proxy;

        #region Account
        private Account account;
        public Account Account
        {
            get => account;
            set
            {
                account = value;
                OnPropertyChanged("Account");
            }
        }
        #endregion

        #region Profile Picture
        private string profilePicture;
        public string ProfilePicture
        {
            get => profilePicture;
            set
            {
                profilePicture = value;
                OnPropertyChanged("ProfilePicture");
            }
        }
        #endregion

        #region Liked Movies
        private ObservableCollection<Movie> likedMovies;
        public ObservableCollection<Movie> LikedMovies
        {
            get => likedMovies;
            set
            {
                likedMovies = value;
                OnPropertyChanged("LikedMovies");
            }
        }
        #endregion

        public Command MovieCommand { protected set; get; }
        private void GoToMovie(Movie m)
        {
            Push.Invoke(new MovieView(m.Id));
        }

        public Command SuggestionsCommand { protected set; get; }
        private void GoToSuggestions()
        {
            Push.Invoke(new ProfileView()); // change ProfileView to SuggestionView
        }

        public Command VotesHistoryCommand { protected set; get; }
        private void GoToVotesHistory()
        {
            Push.Invoke(new ProfileView()); // change ProfileView to VotesHistoryView
        }

        public event Action<Page> Push;
    }
}
