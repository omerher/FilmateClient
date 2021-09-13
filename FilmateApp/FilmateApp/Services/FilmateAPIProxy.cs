using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FilmateApp.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Text.Encodings.Web;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.IO;

namespace FilmateApp.Services
{
    class FilmateAPIProxy
    {
        private const string CLOUD_URL = "TBD"; //API url when going on the cloud
        private const string DEV_ANDROID_EMULATOR_URL = "http://10.0.2.2:53594/api"; //API url when using emulator on android
        private const string DEV_ANDROID_PHYSICAL_URL = "http://192.168.1.14:53594/api"; //API url when using physucal device on android
        private const string DEV_WINDOWS_URL = "https://localhost:44380/api"; //API url when using windoes on development

        private HttpClient client;
        private string baseUri;
        private static FilmateAPIProxy proxy = null;

        public static FilmateAPIProxy CreateProxy()
        {
            string baseUri;
            if (App.IsDevEnv)
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    if (DeviceInfo.DeviceType == DeviceType.Virtual)
                    {
                        baseUri = DEV_ANDROID_EMULATOR_URL;
                    }
                    else
                    {
                        baseUri = DEV_ANDROID_PHYSICAL_URL;
                    }
                }
                else
                {
                    baseUri = DEV_WINDOWS_URL;
                }
            }
            else
            {
                baseUri = CLOUD_URL;
            }

            if (proxy == null)
                proxy = new FilmateAPIProxy(baseUri);
            return proxy;
        }

        private FilmateAPIProxy(string baseUri)
        {
            //Set client handler to support cookies!!
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new System.Net.CookieContainer();

            //Create client with the handler!
            this.client = new HttpClient(handler, true);
            this.baseUri = baseUri;
        }
    }
}