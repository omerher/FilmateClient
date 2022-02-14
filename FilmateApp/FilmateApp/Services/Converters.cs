using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using Xamarin.Forms;
using FilmateApp.Models;

namespace FilmateApp.Services
{
    public class ImageSourceConverter : IValueConverter
    {
        static WebClient Client = new WebClient();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            byte[] byteArray;
            try
            {
                byteArray = Client.DownloadData(value.ToString());
            }
            catch (System.Net.WebException e)
            {
                return null;
            }

            return ImageSource.FromStream(() => new MemoryStream(byteArray));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RuntimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int)
            {
                TimeSpan t =TimeSpan.FromMinutes((int)value);

                if (t.Hours > 0)
                    return $"{t.Hours}h {t.Minutes}m";
                else
                    return $"{t.Minutes}m";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PosterSizeExpandConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double)
            {
                return (double)value * 1.3;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DoubleHalfConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double)
            {
                return (double)value * 0.5;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NegateBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }
    }

    public class ReviewCommandConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values != null && values.Length == 2)
            {
                Label contentLabel = (Label)values[0];
                Label changeViewStateLabel = (Label)values[1];

                return new ReviewCommandHelper { ContentLabel = contentLabel, ChangeViewStateLabel = changeViewStateLabel };
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    //public class ReviewCommandHelper : IMultiValueConverter
    //{
    //    private Label contentLabel;
    //    private Label changeViewStateLabel;

    //    public ReviewCommandHelper(Label contentLabel, Label changeViewStateLabel)
    //    {
    //        this.contentLabel = contentLabel;
    //        this.changeViewStateLabel = changeViewStateLabel;
    //    }

    //    public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        if (values != null && values.Length == 2)
    //        {
    //            Label contentLabel = (Label)values[0];
    //            Label changeViewStateLabel = (Label)values[1];

    //            return new ReviewCommandHelper(contentLabel, changeViewStateLabel);
    //        }
    //        return null;
    //    }

    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

}
