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
using FilmateApp.Resources.Renderers;
using Xamarin.Essentials;
using System.Linq;

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
            LikedMovies = new ObservableCollection<Movie>();
            proxy = FilmateAPIProxy.CreateProxy();

            AccountLoaded = false;
            LoadProfile();
        }

        public async void GetLikedMovies()
        {
            TMDbClient client = new TMDbClient(App.APIKey);
            await client.GetConfigAsync();

            LikedMovies.Clear();
            int index = 0;
            foreach (LikedMovie likedMovie in Account.LikedMovies)
            {
                Movie movie = await client.GetMovieAsync(likedMovie.MovieId);
                Uri posterUrl = client.GetImageUrl("w342", movie.PosterPath);
                movie.PosterPath = posterUrl.AbsoluteUri;
                Account.LikedMovies[index].Movie = movie;
                LikedMovies.Add(movie);
                index++;
            }
        }

        public Command MovieCommand => new Command(() =>
        {
            if (SelectedLikedMovie != null)
            {
                int id = SelectedLikedMovie.Id;
                Push?.Invoke(new MovieView(id));
                SelectedLikedMovie = null;
            }
        });

        public Command SuggestionsCommand => new Command(() => Push.Invoke(new OGMovieCompareView("Suggestions", "Suggestions")));
        public Command VotesHistoryCommand => new Command(() => Push.Invoke(new OGMovieCompareView("Vote History", "Your Votes")));
        public Command ExpandLikedMoviesCommand => new Command(() => Push?.Invoke(new MovieListView(likedMovies, "Liked Movies")));
        public Command LoginCommand => new Command(() => Push?.Invoke(new LoginView()));
        public Command EditProfileCommand => new Command(() => Push.Invoke(new EditProfileView()));
        public Command AdminCommand => new Command(() => Push.Invoke(new AdminView()));

        public Command LoadProfileCommand => new Command(() => LoadProfile());
        public async void LoadProfile()
        {
            IsRefreshing = true;
            Account = await proxy.GetAccountById(((App)App.Current).CurrentAccount.AccountId);
            ((App)App.Current).CurrentAccount = Account;

            if (Account != null)
            {
                ProfilePicture = $"{proxy.baseUri}/imgs/{Account.ProfilePicture}";
                GetLikedMovies();
                AccountLoaded = true;
            }

            IsRefreshing = false;
        }

        public Command LogoutCommand => new Command(() => Logout());
        public async void Logout()
        {
            string token = await SecureStorage.GetAsync("auth_token");
            bool success = await proxy.Logout(token);
            if (success)
            {
                SecureStorage.Remove("auth_token");
                ((App)App.Current).CurrentAccount = null;
                await PopAllTo(new LoginView());
            }
        }

        public async Task PopAllTo(Page page) // clear navigation stack and goes to page
        {
            App.Current.MainPage.Navigation.InsertPageBefore(page, App.Current.MainPage.Navigation.NavigationStack.First());
            await App.Current.MainPage.Navigation.PopToRootAsync();
        }

        public event Action<Page> Push;

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

        #region Selected Liked Movie
        private Movie selectedLikedMovie;
        public Movie SelectedLikedMovie
        {
            get => selectedLikedMovie;
            set
            {
                selectedLikedMovie = value;
                OnPropertyChanged("SelectedLikedMovie");
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
    }
}
