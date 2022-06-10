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
    class MovieViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private TMDbClient client;
        public MovieViewModel(int movieID)
        {
            this.client = new TMDbClient(App.APIKey);
            client.GetConfigAsync();
            LoadAsync(movieID);

            SuggestionsViewModel = new SuggestionsViewModel(movieID);
            ReviewViewModel = new ReviewsViewModel(movieID);

            this.LikeMovieCommand = new Command(() => LikeMovie());
            this.UnlikeMovieCommand = new Command(() => UnlikeMovie());
        }

        private async void LoadAsync(int movieID)
        {
            await GetMovieInfo(movieID);
        }

        public async Task GetMovieInfo(int movieID)
        {
            await client.GetConfigAsync();

            Movie = await client.GetMovieAsync(movieID);
            Uri uri = client.GetImageUrl("w500", Movie.PosterPath);
            PosterUrl = uri.AbsoluteUri;

            Account currentAccount = ((App)App.Current).CurrentAccount;
            IsLikedMovie = currentAccount.LikedMovies.Exists(m => m.MovieId == Movie.Id);
        }

        public Command LikeMovieCommand { protected set; get; }
        private async void LikeMovie()
        {
            FilmateAPIProxy proxy = FilmateAPIProxy.CreateProxy();
            bool worked = await proxy.AddLikedMovie(Movie.Id);
            IsLikedMovie = worked;

            if (worked)
                ((App)App.Current).CurrentAccount.LikedMovies.Add(new LikedMovie()
                {
                    AccountId = ((App)App.Current).CurrentAccount.AccountId,
                    MovieId = Movie.Id
                });
        }

        public Command UnlikeMovieCommand { protected set; get; }
        private async void UnlikeMovie()
        {
            FilmateAPIProxy proxy = FilmateAPIProxy.CreateProxy();
            bool worked = await proxy.RemoveLikedMovie(Movie.Id);
            IsLikedMovie = !worked;

            if (worked)
            {
                LikedMovie likedMovie = ((App)App.Current).CurrentAccount.LikedMovies.Find(m => m.MovieId == Movie.Id);
                if (likedMovie != null)
                    ((App)App.Current).CurrentAccount.LikedMovies.Remove(likedMovie);
            }
        }

        #region Movie
        private Movie movie;
        public Movie Movie
        {
            get => movie;
            set
            {
                movie = value;
                OnPropertyChanged("Movie");
            }
        }
        #endregion

        #region PosterUrl
        private string posterUrl;
        public string PosterUrl
        {
            get => posterUrl;
            set
            {
                posterUrl = value;
                OnPropertyChanged("PosterUrl");
            }
        }
        #endregion

        #region Selected View Model Index
        private int selectedViewModelIndex;
        public int SelectedViewModelIndex
        {
            get => selectedViewModelIndex;
            set
            {
                selectedViewModelIndex = value;
                OnPropertyChanged("SelectedViewModelIndex");
            }
        }
        #endregion

        public SuggestionsViewModel SuggestionsViewModel { get; set; }
        public ReviewsViewModel ReviewViewModel { get; set; }

        #region Is Liked Movie
        private bool isLikedMovie;
        public bool IsLikedMovie
        {
            get => isLikedMovie;
            set
            {
                isLikedMovie = value;
                OnPropertyChanged("IsLikedMovie");
            }
        }
        #endregion

        public event Action<Page> Push;
    }
}
