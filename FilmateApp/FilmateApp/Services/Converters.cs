using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using Xamarin.Forms;
using FilmateApp.Models;
using Syncfusion.XForms.Chat;

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


    /// <summary>
    /// Defines methods that can be used to convert data objects to chat messages and vice versa.
    /// </summary>
    public class MessageConverter : IChatMessageConverter
    {
        /// <summary>
        /// Converts given data object to a chat message.
        /// </summary>
        /// <param name="data">The data object to be converted as a chat message.</param>
        /// <param name="chat">Instance of <see cref="SfChat"/>. </param>
        /// <returns>Returns the data object as a chat message.</returns>
        public IMessage ConvertToChatMessage(object data, SfChat chat)
        {
            var message = new TextMessage();
            var item = data as Msg;
            FilmateAPIProxy proxy = FilmateAPIProxy.CreateProxy();

            message.Text = item.Content;
            message.Author = new Author()
            {
                Avatar = $"{proxy.baseUri}/{item.Account.ProfilePicture}",
                Name = item.Account.AccountName
            };
            message.Data = item;
            //if (item.Suggestions != null)
            //{
            //    message.Suggestions = item.Suggestions;
            //}
            return message;
        }

        /// <summary>
        /// Converts the given chat message to a data object.
        /// </summary>
        /// <param name="chatMessage">The chat message that is to be converted as a data object.</param>
        /// <param name="chat">Instance of <see cref="SfChat"/>. </param>
        /// <returns>Returns the chat message as a data object.</returns>
        public object ConvertToData(object chatMessage, SfChat chat)
        {
            var message = new Msg();
            var item = chatMessage as TextMessage;

            message.Content = item.Text;
            //message.Account = chat.CurrentUser;
            //if (message.Suggestions != null)
            //{
            //    message.Suggestions = chat.Suggestions;
            //}
            return message;
        }
    }

}
