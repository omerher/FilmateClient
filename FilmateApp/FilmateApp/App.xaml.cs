using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FilmateApp.Views;
using FilmateApp.Models;
using FilmateApp.Services;

namespace FilmateApp
{
    public partial class App : Application
    {
        public static bool IsDevEnv
        {
            get
            {
                return true; //change this before release!
            }
        }

        public static string APIKey = "c80b17a14eb9a321ae1fc3f4680e410e";
        public Account CurrentAccount { get; set; }
        public App()
        {
            InitializeComponent();
            Sharpnado.Tabs.Initializer.Initialize(false, false);
            Sharpnado.Shades.Initializer.Initialize(loggerEnable: false);
            Sharpnado.CollectionView.Initializer.Initialize(true, false);

            string lightOrDarkMode = "light";
            string[] styles = new string[] { "MainBackgroundStyle", "InputFieldStyle", "TitleStyle", "TabStyle" };

            foreach (string style in styles)
            {
                Resources[style] = Resources[lightOrDarkMode + "_" + style];
            }

            if (lightOrDarkMode == "light")
            {
                Resources.Add("linear1", Color.FromHex("EB932E"));
                Resources.Add("linear2", Color.FromHex("E86B32"));
                Resources.Add("accent", Color.FromHex("E97F30"));
                Resources.Add("background", Color.FromHex("FFFFFF"));
                Resources.Add("buttonBG", Color.FromHex("E9E9E9"));
                Resources.Add("primaryText", Color.FromHex("414141"));
                Resources.Add("secondaryText", Color.FromHex("626262"));
                Resources.Add("tertiaryText", Color.FromHex("929293"));
            }

            MainPage = new NavigationPage(new ProfileView());
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
