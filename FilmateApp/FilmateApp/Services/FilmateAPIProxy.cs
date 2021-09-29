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
using System.Web;

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

        public async Task<bool?> UsernameExists(string username)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/username-exists?username={username}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    bool? b = JsonSerializer.Deserialize<bool?>(content, options);
                    return b;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<bool?> EmailExists(string email)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/email-exists?email={email}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    bool? b = JsonSerializer.Deserialize<bool?>(content, options);
                    return b;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<Account> SignUpAccount(string email, string password, string username, int age)
        {
            try
            {
                HashSalt hashSalt = HashSalt.GenerateSaltedHash(password);
                string encodedHash = Uri.EscapeDataString(hashSalt.Hash);
                string encodedSalt = Uri.EscapeDataString(hashSalt.Salt);

                string uri = $"{this.baseUri}/register?email={email}&username={username}&password={encodedHash}&age={age}&salt={encodedSalt}";
                HttpResponseMessage response = await this.client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    Account returnedAccount = JsonSerializer.Deserialize<Account>(jsonContent, options);
                    return returnedAccount;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //public async Task<Account> SignUpAccount(string email, string password, string username, int age)
        //{
        //    try
        //    {
        //        HashSalt hashSalt = HashSalt.GenerateSaltedHash(password);
        //        Account a = new Account(){
        //            Email = email,
        //            Pass = hashSalt.Hash,
        //            Username = username,
        //            AccountName = "",
        //            Age = age,
        //            Salt = hashSalt.Salt
        //        };

        //        JsonSerializerOptions options = new JsonSerializerOptions
        //        {
        //            ReferenceHandler = ReferenceHandler.Preserve,
        //            PropertyNameCaseInsensitive = true
        //        };
        //        string json = JsonSerializer.Serialize<Account>(a, options);
        //        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        //        HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/register", content);
        //        if (response.IsSuccessStatusCode)
        //        {

        //            string jsonContent = await response.Content.ReadAsStringAsync();
        //            Account returnedAccount = JsonSerializer.Deserialize<Account>(jsonContent, options);
        //            return returnedAccount;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        return null;
        //    }
        //}

        public async Task<(string, string)> GetHashAndSaltByEmail(string email)
        {
            try
            {
                string salt = "";
                string hash = "";

                HttpResponseMessage saltResponse = await this.client.GetAsync($"{this.baseUri}/get-salt?email={email}");
                if (saltResponse.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string encodedSalt = await saltResponse.Content.ReadAsStringAsync();
                    salt = System.Web.HttpUtility.UrlDecode(encodedSalt);
                }
                else
                {
                    return ("Error", "Error");
                }

                HttpResponseMessage hashResponse = await this.client.GetAsync($"{this.baseUri}/get-hash?email={email}");
                if (hashResponse.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string encodedHash = await hashResponse.Content.ReadAsStringAsync();
                    hash = System.Web.HttpUtility.UrlDecode(encodedHash);
                }
                else
                {
                    return ("Error", "Error");
                }

                return (hash, salt);
            }
            catch
            {
                return ("Error", "Error");
            }
        }

        public async Task<bool?> VerifyPassword(string email, string password)
        {
            (string, string) hashAndSalt = await this.GetHashAndSaltByEmail(email);
            string hash = hashAndSalt.Item1;
            string salt = hashAndSalt.Item2;

            if (hash == "Error" || salt == "Error")
                return null;

            return HashSalt.VerifyPassword(password, hash, salt);
        }

        public async Task<Account> Login(string email, string password)
        {
            try
            {
                bool? verifyPassword = await VerifyPassword(email, password);

                if (verifyPassword == true)
                {
                    (string, string) hashAndSalt = await this.GetHashAndSaltByEmail(email);
                    string hash = hashAndSalt.Item1;
                    string escapedHash = Uri.EscapeDataString(hash);
                    HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/login?email={email}&password={escapedHash}");

                    if (response.IsSuccessStatusCode)
                    {
                        JsonSerializerOptions options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };
                        string jsonContent = await response.Content.ReadAsStringAsync();
                        Account returnedAccount = JsonSerializer.Deserialize<Account>(jsonContent, options);
                        return returnedAccount;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}