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
            this.SuggestionsViewModel = new SuggestionsViewModel();
            this.client = new TMDbClient(App.APIKey);
            client.GetConfigAsync();

            GetMovieInfo(movieID);
        }

        public async void GetMovieInfo(int movieID)
        {
            await client.GetConfigAsync();

            Movie = await client.GetMovieAsync(movieID);
            Uri uri = client.GetImageUrl("w500", Movie.PosterPath);
            PosterUrl = uri.AbsoluteUri;
        }

        public SuggestionsViewModel SuggestionsViewModel { get; }

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
    }
}
