using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFImageLoading;
using FFImageLoading.Svg.Platform;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FilmateApp.ViewModels;

namespace FilmateApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : ContentPage
    {
        public LoginView()
        {
            LoginViewModel context = new LoginViewModel();
            context.Push += (p) => Navigation.PushAsync(p);
            this.BindingContext = context;

            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegisterView());
        }
    }
}