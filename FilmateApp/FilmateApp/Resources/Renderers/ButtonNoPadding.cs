using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FilmateApp.Resources.Renderers
{
    public class ButtonNoPadding : Button
    {
        public ButtonNoPadding() { Padding.Left = 0; Padding.Right = 0; Padding.Top = 0; Padding.Left = 0; }

        private InnerPadding _innerPadding = new InnerPadding();

        public InnerPadding Padding { get { return _innerPadding; } }
    }

    public class InnerPadding
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
    }
}
