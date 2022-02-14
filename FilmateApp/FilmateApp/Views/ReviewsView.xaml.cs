using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmateApp.ViewModels;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Sharpnado.CollectionView;
using Xamarin.CommunityToolkit.ObjectModel;
using TMDbLib.Objects.Movies;
using System.Collections.ObjectModel;
using FFImageLoading.Transformations;

namespace FilmateApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReviewsView : ContentView
    {
        public ReviewsView()
        {
            InitializeComponent();
        }

        private void ViewStateChanged(object sender, EventArgs e)
        {
            return;
        }
    }
}