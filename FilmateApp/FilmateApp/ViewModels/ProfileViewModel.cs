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
using Xamarin.CommunityToolkit.ObjectModel;

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

        private FilmateAPIProxy proxy;
        public ProfileViewModel()
        {
            LikedMovies = new ObservableRangeCollection<Movie>();
            proxy = FilmateAPIProxy.CreateProxy();

            AccountLoaded = false;
            LoadProfile();
        }

        public async void GetLikedMovies()
        {
            TMDbClient client = new TMDbClient(App.APIKey);
            await client.GetConfigAsync();

            LikedMovies.Clear();
            foreach (LikedMovie likedMovie in Account.LikedMovies)
            {
                Movie movie = await client.GetMovieAsync(likedMovie.MovieId);
                Uri posterUrl = client.GetImageUrl("w342", movie.PosterPath);
                movie.PosterPath = posterUrl.AbsoluteUri;
                LikedMovies.Add(movie);
            }
        }

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
        private ObservableRangeCollection<Movie> likedMovies;
        public ObservableRangeCollection<Movie> LikedMovies
        {
            get => likedMovies;
            set
            {
                likedMovies = value;
                OnPropertyChanged("LikedMovies");
            }
        }
        #endregion

        #region Current Index
        private int currentIndex;
        public int CurrentIndex
        {
            get => currentIndex;
            set
            {
                currentIndex = value;
                OnPropertyChanged("CurrentIndex");
            }
        }
        #endregion

        #region Is Refreshing
        private bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                isRefreshing = value;
                OnPropertyChanged("IsRefreshing");
            }
        }
        #endregion

        #region Account Loaded
        private bool accountLoaded;
        public bool AccountLoaded
        {
            get => accountLoaded;
            set
            {
                accountLoaded = value;
                OnPropertyChanged("AccountLoaded");
            }
        }
        #endregion

        public Command MovieCommand => new Command<Movie>((m) => Push?.Invoke(new MovieView(m.Id)));

        public Command SuggestionsCommand => new Command(() => GoToSuggestions());
        private void GoToSuggestions()
        {
            //Push.Invoke(new ProfileView()); // change ProfileView to SuggestionView
        }

        public Command VotesHistoryCommand => new Command(() => GoToVotesHistory());
        private void GoToVotesHistory()
        {
            //Push.Invoke(new ProfileView()); // change ProfileView to VotesHistoryView
        }

        public Command LoginCommand => new Command(() => GoToLogin());
        private void GoToLogin()
        {
            Push.Invoke(new LoginView());
        }

        public Command LoadProfileCommand => new Command(() => LoadProfile());
        public void LoadProfile()
        {
            IsRefreshing = true;
            Account = ((App)App.Current).CurrentAccount;

            if (Account != null)
            {
                ProfilePicture = $"{proxy.baseUri}/imgs/{Account.ProfilePicture}";
                GetLikedMovies();
                AccountLoaded = true;
            }

            IsRefreshing = false;
        }

        public Command ExpandLikedMoviesCommand => new Command(() => Push?.Invoke(new MovieListView(LikedMovies, "Liked Movies")));

        public event Action<Page> Push;
    }
}
