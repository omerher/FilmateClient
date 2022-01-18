using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmateApp.Models;
using FilmateApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FilmateApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OGMovieCompareView : ContentPage
    {
        public OGMovieCompareView(string title, string comparisionString)
        {
            this.Title = title;
            OGMovieCompareViewModel context = new OGMovieCompareViewModel(comparisionString);
            this.BindingContext = context;
            InitializeComponent();

            comparisonLabel.Text = comparisionString;
        }
    }
}