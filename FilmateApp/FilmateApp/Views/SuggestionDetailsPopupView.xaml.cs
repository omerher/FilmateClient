using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmateApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using FilmateApp.Models;

namespace FilmateApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SuggestionDetailsPopupView : PopupPage
    {
        public SuggestionDetailsPopupView(MovieSuggestionAccount movieSuggestionAccount)
        {
            SuggestionDetailsViewModel context = new SuggestionDetailsViewModel(movieSuggestionAccount);
            this.BindingContext = context;
            InitializeComponent();
        }

        private async void ClosePopup(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}