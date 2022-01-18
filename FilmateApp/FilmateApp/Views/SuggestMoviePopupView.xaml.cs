using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmateApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Pages;

namespace FilmateApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SuggestMoviePopupView : PopupPage
    {
        public SuggestMoviePopupView(SuggestionsViewModel suggestionsViewModel)
        {
            SuggestionPopupViewModel context = new SuggestionPopupViewModel(suggestionsViewModel);
            this.BindingContext = context;
            InitializeComponent();

            CollectionView.ItemWidth = Application.Current.MainPage.Width * 0.27;
            CollectionView.ItemHeight = CollectionView.ItemWidth * 1.9;
        }
    }
}