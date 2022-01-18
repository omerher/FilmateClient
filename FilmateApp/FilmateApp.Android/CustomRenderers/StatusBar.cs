using System;
using Android.OS;
using FilmateApp.Droid.CustomRenderers;
using FilmateApp.Resources.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(Statusbar))]
namespace FilmateApp.Droid.CustomRenderers
{
    public class Statusbar : IStatusBarPlatformSpecific
    {
        public Statusbar()
        {
        }

        public void SetStatusBarColor(Color color)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                var androidColor = color.ToAndroid();
                Xamarin.Essentials.Platform.CurrentActivity.Window.SetStatusBarColor(androidColor);
            }
        }
    }

}
