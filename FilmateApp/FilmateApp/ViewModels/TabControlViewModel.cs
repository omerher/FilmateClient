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

namespace FilmateApp.ViewModels
{
    class TabControlViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public TabControlViewModel()
        {
            SelectedViewModelIndex = 0;
        }


        #region Selected Tab Index
        private int selectedViewModelIndex;
        public int SelectedViewModelIndex
        {
            get => selectedViewModelIndex;
            set
            {
                selectedViewModelIndex = value;
                OnPropertyChanged("SelectedViewModelIndex");
            }
        }
        #endregion

        public event Action<Page> Push;
    }
}
