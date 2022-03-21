using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FilmateApp.Views;
using FilmateApp.Models;
using FilmateApp.Services;
using Xamarin.Essentials;
using System.Threading.Tasks;

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
        public event Action UserChanged;
        public Account currentAccount;
        public Account CurrentAccount
        {
            get => currentAccount;
            set
            {
                currentAccount = value;
                UserChanged?.Invoke();
            }
        }
        public App()
        {
            InitializeComponent();
            Sharpnado.Tabs.Initializer.Initialize(false, false);
            Sharpnado.Shades.Initializer.Initialize(loggerEnable: false);
            Sharpnado.CollectionView.Initializer.Initialize(true, false);

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTU5NDI3QDMxMzkyZTM0MmUzMEsrbmJib0kwRXB2c05aS1hVdGJHN1FXTy9ibDJsOTJIcDExT003aEVaeEk9");

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

            var result = Task.Run(async () => await CheckToken()).Result;
            MainPage = new NavigationPage(result);
        }

        public async Task<ContentPage> CheckToken()
        {
            string token = await SecureStorage.GetAsync("auth_token");

            if (token != null)
            {
                FilmateAPIProxy proxy = FilmateAPIProxy.CreateProxy();
                Account account = await proxy.LoginToken(token);

                if (account != null)
                {
                    CurrentAccount = account;
                    return new TabControlView();
                }
            }

            return new LoginView();
        }
    }
}
