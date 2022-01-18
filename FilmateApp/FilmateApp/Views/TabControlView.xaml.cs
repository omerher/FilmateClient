using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmateApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FilmateApp.Resources.Renderers;
using Rg.Plugins.Popup.Services;

namespace FilmateApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabControlView : ContentPage
    {
        private TabControlViewModel context;
        public TabControlView()
        {
            context = new TabControlViewModel();
            context.Push += (p) => Navigation.PushAsync(p);
            this.BindingContext = context;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            context.TabChanged();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            var statusbar = DependencyService.Get<IStatusBarPlatformSpecific>();
            statusbar.SetStatusBarColor(Color.White);
        }
    }
}