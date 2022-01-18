using System;
using System.Collections.Generic;
using System.Text;

namespace FilmateApp.ViewModels
{
    public class ReviewsViewModel : BaseViewModel
    {
        private int movieId;
        public ReviewsViewModel(int movieId)
        {
            this.movieId = movieId;
            Test = "Hell";
        }

        public string Test { get ; set; }  
    }
}
