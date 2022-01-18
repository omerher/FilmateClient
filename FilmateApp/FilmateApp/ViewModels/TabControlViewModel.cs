using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using FilmateApp.Services;
using FilmateApp.Models;
using FilmateApp.Views;
using System.Threading.Tasks;
using FilmateApp.Resources.Renderers;

namespace FilmateApp.ViewModels
{
    class TabControlViewModel : BaseViewModel
    {
        public TabControlViewModel()
        {
            SelectedViewModelIndex = 0;
        }

        public void TabChanged()
        {
            if (SelectedViewModelIndex == 3)
            {
                var statusbar = DependencyService.Get<IStatusBarPlatformSpecific>();
                statusbar.SetStatusBarColor(Color.FromHex("#F58C3D"));
            }
            else
            {
                var statusbar = DependencyService.Get<IStatusBarPlatformSpecific>();
                statusbar.SetStatusBarColor(Color.White);
            }
        }

        #region Selected Tab Index
        private int selectedViewModelIndex;
        public int SelectedViewModelIndex
        {
            get => selectedViewModelIndex;
            set
            {
                SetValue(ref selectedViewModelIndex, value);
                TabChanged();
            }
        }
        #endregion

        public event Action<Page> Push;
    }
}
