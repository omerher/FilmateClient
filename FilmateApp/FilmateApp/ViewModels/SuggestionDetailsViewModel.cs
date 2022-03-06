using System;
using System.Collections.Generic;
using System.Text;
using FilmateApp.Models;
using FilmateApp.Services;
using FilmateApp.Views;
using System.Linq;
using Xamarin.Forms;
using System.Windows.Input;
using Rg.Plugins.Popup.Services;

namespace FilmateApp.ViewModels
{
    internal class SuggestionDetailsViewModel : BaseViewModel
    {
        private int movieID;
        public SuggestionDetailsViewModel(MovieSuggestionAccount movieSuggestionAccount)
        {
            Suggestion suggestion = ((App)App.Current).CurrentAccount.Suggestions.FirstOrDefault(s => s.SuggestionId == movieSuggestionAccount.SuggestionId);
            FilmateAPIProxy proxy = FilmateAPIProxy.CreateProxy();

            MovieName = movieSuggestionAccount.Movie.Title;
            ProfilePicture = $"{proxy.baseUri}/imgs/{movieSuggestionAccount.Account.ProfilePicture}";
            Username = movieSuggestionAccount.Account.Username;
            Date = suggestion.PostDate.ToString("MMMM dd yyyy");
            Poster = movieSuggestionAccount.PosterPath;
            PosterHeight = movieSuggestionAccount.PosterHeight * 1.25;
            PosterWidth = movieSuggestionAccount.PosterWidth * 1.25;

            this.movieID = suggestion.SuggestionMovieId;
        }

        public ICommand PressedMovie => new Command(async () =>
        {
            await PopupNavigation.Instance.PopAsync();
            await ((App)App.Current).MainPage.Navigation.PushAsync(new MovieView(movieID));
        });

        private string movieName;
        public string MovieName
        {
            get => movieName;
            set => SetValue(ref movieName, value);
        }

        private string username;
        public string Username
        {
            get => username;
            set => SetValue(ref username, value);
        }

        private string date;
        public string Date
        {
            get => date;
            set => SetValue(ref date, value);
        }

        private string profilePicture;
        public string ProfilePicture
        {
            get => profilePicture;
            set => SetValue(ref profilePicture, value);
        }

        private string poster;
        public string Poster
        {
            get => poster;
            set => SetValue(ref poster, value);
        }

        private double posterHeight;
        public double PosterHeight
        {
            get => posterHeight;
            set => SetValue(ref posterHeight, value);
        }

        private double posterWidth;
        public double PosterWidth
        {
            get => posterWidth;
            set => SetValue(ref posterWidth, value);
        }
    }
}
