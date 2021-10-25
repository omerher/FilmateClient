using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.ComponentModel;
using FilmateApp.Models;

namespace FilmateApp.ViewModels
{
    class SuggestionsViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public SuggestionsViewModel()
        {
            Test = "a";
        }

        #region Movie
        private string test;
        public string Test
        {
            get => test;
            set
            {
                test = value;
                OnPropertyChanged("Test");
            }
        }
        #endregion
    }
}
