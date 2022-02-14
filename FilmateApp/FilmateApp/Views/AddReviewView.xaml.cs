using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmateApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FilmateApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddReviewView : ContentPage
    {
        public AddReviewView(ReviewsViewModel reviewsViewModel, int movieId)
        {
            InitializeComponent();
            AddReviewViewModel context = new AddReviewViewModel(reviewsViewModel, movieId);
            this.BindingContext = context;
        }
    }
}