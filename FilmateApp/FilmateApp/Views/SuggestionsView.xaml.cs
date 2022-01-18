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
    public partial class SuggestionsView : ContentView
    {
        SuggestionsViewModel context;
        public SuggestionsView()
        {
            InitializeComponent();

            SuggestionsCollectionView.ItemWidth = Application.Current.MainPage.Width * 0.26;
            SuggestionsCollectionView.ItemHeight = SuggestionsCollectionView.ItemWidth * 1.9;
        }

        private async void SuggestionMovie(object sender, EventArgs e)
        {
            if (this.BindingContext is SuggestionsViewModel)
                await PopupNavigation.Instance.PushAsync(new SuggestMoviePopupView((SuggestionsViewModel)this.BindingContext));
        }
    }
}