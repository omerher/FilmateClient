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
    public partial class EditProfileView : ContentPage
    {
        public EditProfileView()
        {
            EditProfileViewModel context = new EditProfileViewModel();
            context.SetImageSourceEvent += ChangePfpSource;
            this.BindingContext = context;
            InitializeComponent();
        }

        public void ChangePfpSource(ImageSource imgSource)
        {
            pfpImage.Source = imgSource;
        }
    }
}