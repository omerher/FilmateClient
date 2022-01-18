using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FilmateApp.Resources.Renderers
{
    public interface IStatusBarPlatformSpecific
    {
        void SetStatusBarColor(Color color);
    }
}
