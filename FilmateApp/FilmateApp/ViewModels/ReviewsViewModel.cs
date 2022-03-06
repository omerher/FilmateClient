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
using System.Linq;

namespace FilmateApp.ViewModels
{
    public class ReviewsViewModel : BaseViewModel
    {
        private int movieId;
        public ReviewsViewModel(int movieId)
        {
            this.movieId = movieId;
            ArrowSource = "icon_arrow_down.svg";
            Reviews = new ObservableCollection<Review>();

            LoadReviews();
        }

        private async void LoadReviews()
        {
            TMDbClient client = new TMDbClient(App.APIKey);
            await client.GetConfigAsync();
            FilmateAPIProxy proxy = FilmateAPIProxy.CreateProxy();

            Reviews.Clear();
            List<Review> movieReviews = await proxy.GetReviews(movieId);
            foreach (Review review in movieReviews)
            {
                review.Account.ProfilePicture = $"{proxy.baseUri}/imgs/{review.Account.ProfilePicture}";
                Reviews.Add(review);
            }
        }

        public ICommand RefreshCommand => new Command(() =>
        {
            IsRefreshing = true;
            LoadReviews();
            IsRefreshing = false;
        });
        public ICommand ReviewMovieCommand => new Command(() => ReviewMovie());
        private void ReviewMovie()
        {
            ((App)App.Current).MainPage.Navigation.PushAsync(new AddReviewView(this, movieId));
        }

        public ICommand ViewChangeCommand => new Command<ReviewCommandHelper>((r) => ViewChanged(r));
        private void ViewChanged(ReviewCommandHelper r)
        {
            Label contentLabel = r.ContentLabel;
            Label changeViewStateLabel = r.ChangeViewStateLabel;
            
            if (changeViewStateLabel.Text == "View More") // if need to expand
            {
                contentLabel.MaxLines = 0;
                contentLabel.LineBreakMode = LineBreakMode.WordWrap;
                changeViewStateLabel.Text = "View Less";
                ArrowSource = "icon_arrow_up.svg";
            }
            else // if need to shrink
            {
                contentLabel.LineBreakMode = LineBreakMode.TailTruncation;
                contentLabel.MaxLines = 4;
                changeViewStateLabel.Text = "View More";
                ArrowSource = "icon_arrow_down.svg";
            }
        }

        private ObservableCollection<Review> reviews;
        public ObservableCollection<Review> Reviews
        {
            get => reviews;
            set => SetValue(ref reviews, value);
        }

        private string arrowSource;
        public string ArrowSource
        {
            get => arrowSource;
            set => SetValue(ref arrowSource, value);
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set => SetValue(ref isRefreshing, value);
        }
    }
}
