using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;
using TMDbLib.Objects.Movies;
using System.Windows.Input;
using Xamarin.Forms;
using FilmateApp.Services;

namespace FilmateApp.Models
{
    public class MovieSuggestionAccount : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        public MovieSuggestionAccount Self
        {
            get
            {
                return this;
            }
        }

        public MovieSuggestionAccount(Suggestion suggestion, string posterUrl, string OGPosterUrl = null, Movie movie = null)
        {
            this.Account = suggestion.Account;
            this.Movie = movie;
            this.SuggestionId = suggestion.SuggestionId;
            this.OriginalMovieId = suggestion.OriginalMovieId;
            this.SuggestionMovieId = suggestion.SuggestionMovieId;
            this.Upvotes = suggestion.Upvotes;
            this.Downvotes = suggestion.Downvotes;
            UpvoteStringMap = new Dictionary<string, string>() { { "fill=\"#000000\"", GetHexString(Xamarin.Forms.Color.FromHex("#414141")) } }; // set initial svg icon color
            DownvoteStringMap = new Dictionary<string, string>() { { "fill=\"#000000\"", GetHexString(Xamarin.Forms.Color.FromHex("#414141")) } }; // set initial svg icon color

            AccountVotesHistory account = suggestion.AccountVotesHistories.FirstOrDefault(v => v.AccountId == this.Account.AccountId);
            if (account != null) // if user has voted on this suggestion
            {
                this.IsUpvoted = account.VoteType == true;
                this.IsDownvoted = account.VoteType == false;
            }
            else
            {
                this.IsUpvoted = false;
                this.IsDownvoted = false;
            }
            HandleVotesColor();

            this.PosterPath = posterUrl;
            this.OGPosterPath = OGPosterUrl;
            this.PosterHeight = ((App)App.Current).MainPage.Width * 0.26 * 1.5; // fixed poster height for the irregular poster sizes
            this.PosterWidth = PosterHeight * 0.6667;
            Movie = movie;
        }

        public void ChangeColor(Xamarin.Forms.Color color, VoteType voteType)
        {
            if (voteType == VoteType.Upvote)
                UpvoteStringMap = new Dictionary<string, string>() { { "fill=\"#000000\"", GetHexString(color) } }; // replace svg icon color
            else if (voteType == VoteType.Downvote)
                DownvoteStringMap = new Dictionary<string, string>() { { "fill=\"#000000\"", GetHexString(color) } }; // replace svg icon color
        }

        private string GetHexString(Xamarin.Forms.Color color)
        {
            var red = (int)(color.R * 255);
            var green = (int)(color.G * 255);
            var blue = (int)(color.B * 255);
            var hex = $"fill=\"#{red:X2}{green:X2}{blue:X2}\"";

            return hex;
        }

        public void SetVoted(bool voteType)
        {
            ResetVote();
            if (voteType)
            {
                Upvotes++;
                IsUpvoted = true;
            }
            else
            {
                Downvotes++;
                IsDownvoted = true;
            }
        }

        public void ResetVote()
        {
            if (IsUpvoted)
                Upvotes--;
            if (IsDownvoted)
                Downvotes--;

            IsUpvoted = false;
            IsDownvoted = false;
        }

        private void HandleVotesColor()
        {
            if (IsUpvoted)
                ChangeColor(Xamarin.Forms.Color.FromHex("FF4500"), VoteType.Upvote);
            else
                ChangeColor(Xamarin.Forms.Color.FromHex("414141"), VoteType.Upvote);

            if (IsDownvoted)
                ChangeColor(Xamarin.Forms.Color.FromHex("7193FF"), VoteType.Downvote);
            else
                ChangeColor(Xamarin.Forms.Color.FromHex("414141"), VoteType.Downvote);
        }


        public ICommand UpvotedCommand => new Command(() => Voted(true));
        public ICommand DownvotedCommand => new Command(() => Voted(false));
        private async void Voted(bool voteType)
        {
            FilmateAPIProxy proxy = FilmateAPIProxy.CreateProxy();
            if ((IsUpvoted && voteType == true) || (IsDownvoted && voteType == false))
            {
                bool success = await proxy.RemoveVote(SuggestionId);
                if (success)
                    ResetVote();
            }
            else
            {
                bool success = await proxy.AddVote(SuggestionId, voteType);
                if (success)
                    SetVoted(voteType);
            }

        }


        public enum VoteType
        {
            Upvote,
            Downvote
        }

        public Account Account { get; set; }
        public Movie Movie { get; set; }
        public int SuggestionId { get; set; }
        public int OriginalMovieId { get; set; }
        public int SuggestionMovieId { get; set; }
        #region Upvotes
        private int upvotes;
        public int Upvotes
        {
            get => upvotes;
            set
            {
                upvotes = value;
                OnPropertyChanged("Upvotes");
                OnPropertyChanged("CombinedVotes");
            }
        }
        #endregion
        #region Downvotes
        private int downvotes;
        public int Downvotes
        {
            get => downvotes;
            set
            {
                downvotes = value;
                OnPropertyChanged("Downvotes");
                OnPropertyChanged("CombinedVotes");
            }
        }
        #endregion
        #region Is Upvoted
        private bool isUpvoted;
        public bool IsUpvoted
        {
            get => isUpvoted;
            set
            {
                isUpvoted = value;
                OnPropertyChanged("IsUpvoted");
                HandleVotesColor();
            }
        }
        #endregion
        #region Is Downvoted
        private bool isDownvoted;
        public bool IsDownvoted
        {
            get => isDownvoted;
            set
            {
                isDownvoted = value;
                OnPropertyChanged("IsDownvoted");
                HandleVotesColor();
            }
        }
        #endregion
        public string PosterPath { get; set; }
        public string OGPosterPath { get; set; }
        public double PosterHeight { get; set; }
        public double PosterWidth { get; set; }
        #region Combined Votes
        private int combinedVotes;
        public int CombinedVotes
        {
            get => Upvotes - Downvotes;
            set
            {
                combinedVotes = value;
                OnPropertyChanged("CombinedVotes");
            }
        }
        #endregion
        #region Upvote String Map
        private Dictionary<string, string> upvoteStringMap;
        public Dictionary<string, string> UpvoteStringMap
        {
            get => upvoteStringMap;
            set
            {
                upvoteStringMap = value;
                OnPropertyChanged("UpvoteStringMap");
            }
        }
        #endregion
        #region Downvote String Map
        private Dictionary<string, string> downvoteStringMap;
        public Dictionary<string, string> DownvoteStringMap
        {
            get => downvoteStringMap;
            set
            {
                downvoteStringMap = value;
                OnPropertyChanged("DownvoteStringMap");
            }
        }
        #endregion

    }
}
