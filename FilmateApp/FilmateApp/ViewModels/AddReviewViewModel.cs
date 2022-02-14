using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;
using Xamarin.Forms;
using FilmateApp.Models;
using FilmateApp.Services;
using System.Linq;

namespace FilmateApp.ViewModels
{
    public class AddReviewViewModel : BaseViewModel
    {
        private ReviewsViewModel reviewContext;
        private int movieId;
        private TMDbClient client;
        public AddReviewViewModel(ReviewsViewModel reviewsViewModel, int movieId)
        {
            reviewContext = reviewsViewModel;
            this.movieId = movieId;

            this.client = new TMDbClient(App.APIKey);
            client.GetConfigAsync();
            LoadAsync(movieId);
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
        }

        private bool ValidateForm()
        {
            ValidateTitle();
            ValidateContent();
            ValidateRating();

            return !(ShowTitleError || ShowContentError || ShowRatingError);
        }

        public ICommand SubmitCommand => new Command(() => Submit());
        private async void Submit()
        {
            if (ValidateForm())
            {
                Account currentAccount = ((App)App.Current).CurrentAccount;
                Review review = new Review()
                {
                    AccountId = currentAccount.AccountId,
                    Rating = (int)(Rating * 2),
                    Content = ReviewContent,
                    Title = ReviewTitle,
                    PostDate = DateTime.Now
                };

                Review existingReview = currentAccount.Reviews.FirstOrDefault(r => r.AccountId == currentAccount.AccountId);
                FilmateAPIProxy proxy = FilmateAPIProxy.CreateProxy();
                if (existingReview != null)
                {
                    bool result = await App.Current.MainPage.DisplayAlert("", "You already have a review for this, would you like to delete your existing review?", "Yes", "No");
                    if (result)
                    {
                        bool success = await proxy.DeleteReview(existingReview.ReviewId);
                        if (success)
                        {
                            currentAccount.Reviews.Remove(existingReview);
                            ShowGeneralError = false;
                        }
                    }
                    else
                    {
                        GeneralError = "You can not add multiple reviews";
                        ShowGeneralError = true;
                    }
                }
                else
                {
                    ShowGeneralError = false;
                    
                    Review returnedReview = await proxy.AddReview(review);
                    if (returnedReview != null)
                    {
                        ((App)App.Current).CurrentAccount.Reviews.Add(returnedReview);
                        await ((App)App.Current).MainPage.Navigation.PopAsync();
                    }
                    else
                    {
                        GeneralError = "There was an error with the server. Please try again later.";
                        ShowGeneralError= true;
                    }
                }
            }
        }

        private Movie movie;
        public Movie Movie
        {
            get => movie;
            set => SetValue(ref movie, value);
        }

        private string posterUrl;
        public string PosterUrl
        {
            get => posterUrl;
            set => SetValue(ref posterUrl, value);
        }

        #region Title
        private string reviewTitle;
        public string ReviewTitle
        {
            get => reviewTitle;
            set => SetValue(ref reviewTitle, value);
        }

        private string titleError;
        public string TitleError
        {
            get => titleError;
            set => SetValue(ref titleError, value);
        }

        private bool showTitleError;
        public bool ShowTitleError
        {
            get => showTitleError;
            set => SetValue(ref showTitleError, value);
        }

        private void ValidateTitle()
        {
            TitleError = "Title must not be blank";
            ShowTitleError = string.IsNullOrEmpty(ReviewTitle);
        }
        #endregion

        #region Content
        private string reviewContent;
        public string ReviewContent
        {
            get => reviewContent;
            set => SetValue(ref reviewContent, value);
        }

        private string contentError;
        public string ContentError
        {
            get => contentError;
            set => SetValue(ref contentError, value);
        }

        private bool showContentError;
        public bool ShowContentError
        {
            get => showContentError;
            set => SetValue(ref showContentError, value);
        }

        private void ValidateContent()
        {
            ContentError = "Content must not be blank";
            ShowContentError = string.IsNullOrEmpty(ReviewContent);
        }
        #endregion

        #region Rating
        private double rating;
        public double Rating
        {
            get => rating;
            set => SetValue(ref rating, value);
        }

        private string ratingError;
        public string RatingError
        {
            get => ratingError;
            set => SetValue(ref ratingError, value);
        }

        private bool showRatingError;
        public bool ShowRatingError
        {
            get => showRatingError;
            set => SetValue(ref showRatingError, value);
        }

        private void ValidateRating()
        {
            RatingError = "Rating must not be blank";
            ShowRatingError = Rating == 0;
        }
        #endregion

        #region General Error
        private bool showGeneralError;

        public bool ShowGeneralError
        {
            get => showGeneralError;
            set
            {
                showGeneralError = value;
                OnPropertyChanged("ShowGeneralError");
            }
        }

        private string generalError;

        public string GeneralError
        {
            get => generalError;
            set
            {
                generalError = value;
                OnPropertyChanged("GeneralError");
            }
        }
        #endregion
    }
}
