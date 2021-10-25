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
    public partial class MovieView : ContentPage
    {
        public MovieView(int movieID)
        {
            MovieViewModel context = new MovieViewModel(movieID);

            this.BindingContext = context;
            InitializeComponent();
        }
    }
}