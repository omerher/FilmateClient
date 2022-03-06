using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmateApp.ViewModels;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FilmateApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewGroupPopupView : PopupPage
    {
        public NewGroupPopupView()
        {
            NewGroupPopupViewModel context = new NewGroupPopupViewModel();
            context.SetImageSourceEvent += ChangeIcon;
            this.BindingContext = context;
            InitializeComponent();
        }

        public void ChangeIcon(ImageSource imgSource)
        {
            iconImage.Source = imgSource;
        }
    }
}