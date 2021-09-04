﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FilmateApp.Views;

namespace FilmateApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Resources.Add("linear1", Color.FromHex("EB932E"));
            Resources.Add("linear2", Color.FromHex("E86B32"));
            Resources.Add("accent", Color.FromHex("E97F30"));
            Resources.Add("background", Color.FromHex("FFFFFF"));
            Resources.Add("buttonBG", Color.FromHex("E9E9E9"));
            Resources.Add("primaryText", Color.FromHex("414141"));
            Resources.Add("secondaryText", Color.FromHex("626262"));
            Resources.Add("tertiaryText", Color.FromHex("929293"));

            MainPage = new NavigationPage(new LoginView());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}