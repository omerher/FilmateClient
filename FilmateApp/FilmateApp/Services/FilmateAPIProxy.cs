using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FilmateApp.Models;
using System.Text;
using Newtonsoft.Json;
using System.Text.Unicode;
using System.Text.Encodings.Web;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.IO;
using System.Web;
using System.Threading;

namespace FilmateApp.Services
{
    public class FilmateAPIProxy
    {
        private const string CLOUD_URL = "TBD"; //API url when going on the cloud
        private const string DEV_ANDROID_EMULATOR_URL = "http://192.168.1.207:5001"; //API url when using emulator on android
        private const string DEV_ANDROID_PHYSICAL_URL = "http://192.168.1.207:5001"; //API url when using physucal device on android
        private const string DEV_WINDOWS_URL = "http://192.168.1.207:5001"; //API url when using windoes on development

        private HttpClient client;
        public string baseUri;
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
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            //Create client with the handler!
            this.client = new HttpClient(handler, true);
            this.baseUri = baseUri;
        }

        public async Task<bool?> UsernameExists(string username)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/api/username-exists?username={username}");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    bool? b = JsonConvert.DeserializeObject<bool?>(content);
                    return b;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool?> EmailExists(string email)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/api/email-exists?email={email}");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    bool? b = JsonConvert.DeserializeObject<bool?>(content);
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
                Account a = new Account(){
                    Email = email,
                    Pass = password,
                    Username = username,
                    AccountName = username,
                    Age = age,
                    // temp values for properties that will get set server-side
                    Salt = "1",
                    ProfilePicture = "1"
                };
                
                string json = JsonConvert.SerializeObject(a);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/api/signup", content);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerSettings options = new JsonSerializerSettings
                    {
                        PreserveReferencesHandling = PreserveReferencesHandling.All
                    };

                    string jsonContent = await response.Content.ReadAsStringAsync();
                    Account returnedAccount = JsonConvert.DeserializeObject<Account>(jsonContent, options);
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

        public async Task<Account> Login(string email, string password)
        {
            try
            {
                string json = JsonConvert.SerializeObject((email, password));
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/api/login", content);

                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    Account returnedAccount = JsonConvert.DeserializeObject<Account>(jsonContent);

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

        public async Task<string> GenerateToken()
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/api/generate-token");
                if (response.IsSuccessStatusCode)
                {
                    string token = await response.Content.ReadAsStringAsync();
                    return token;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Account> LoginToken(string token)
        {
            try
            {
                using var cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromSeconds(30));
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/api/login-token?token={token}", cts.Token);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Account account = JsonConvert.DeserializeObject<Account>(content);

                    return account;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> AddLikedMovie(int movieID)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/api/add-liked-movie?movieID={movieID}");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    bool worked = JsonConvert.DeserializeObject<bool>(content);
                    return worked;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> RemoveLikedMovie(int movieID)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/api/remove-liked-movie?movieID={movieID}");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    bool worked = JsonConvert.DeserializeObject<bool>(content);
                    return worked;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> UploadImage(string fullPath, string targetFileName, int chatId = 0)
        {
            try
            {
                var multipartFormDataContent = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(File.ReadAllBytes(fullPath));
                multipartFormDataContent.Add(fileContent, "file", targetFileName);
                string url = $"{this.baseUri}/api/upload-image";
                if (chatId != 0)
                    url += "?chatId=" + chatId;

                HttpResponseMessage response = await client.PostAsync(url, multipartFormDataContent);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> UpdateProfile(string email, string username, string name, int age)
        {
            try
            {
                string url = Uri.EscapeUriString($"{this.baseUri}/api/update-profile?email={email}&username={username}&name={name}&age={age}");
                HttpResponseMessage response = await this.client.GetAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> AddSuggestion(int ogMovieID, int suggMovieID)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/api/add-suggestion?ogMovieID={ogMovieID}&suggMovieID={suggMovieID}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<List<Suggestion>> GetSuggestions(int ogMovieID)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/api/get-suggestions?movieID={ogMovieID}");
                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    List<Suggestion> suggestions = JsonConvert.DeserializeObject<List<Suggestion>>(jsonContent);

                    return suggestions;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> AddVote(int suggestionID, bool voteType)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/api/add-vote?suggestionID={suggestionID}&voteType={voteType}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> RemoveVote(int suggestionID)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/api/remove-vote?suggestionID={suggestionID}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<Review> AddReview(Review review)
        {
            try
            {
                string json = JsonConvert.SerializeObject(review);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/api/add-review", content);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerSettings options = new JsonSerializerSettings
                    {
                        PreserveReferencesHandling = PreserveReferencesHandling.All
                    };

                    string jsonContent = await response.Content.ReadAsStringAsync();
                    Review returnedReview = JsonConvert.DeserializeObject<Review>(jsonContent, options);
                    return returnedReview;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteReview(int reviewID)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/api/delete-review?reviewID={reviewID}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<List<Review>> GetReviews(int movieID)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/api/get-reviews?movieID={movieID}");
                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    List<Review> reviews = JsonConvert.DeserializeObject<List<Review>>(jsonContent);

                    return reviews;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Chat> CreateGroup(Chat chat)
        {
            try
            {
                string json = JsonConvert.SerializeObject(chat);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/api/create-group", content);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerSettings options = new JsonSerializerSettings
                    {
                        PreserveReferencesHandling = PreserveReferencesHandling.All
                    };

                    string jsonContent = await response.Content.ReadAsStringAsync();
                    Chat returnedGroup = JsonConvert.DeserializeObject<Chat>(jsonContent, options);
                    return returnedGroup;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Chat>> GetGroups()
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/api/get-groups");
                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    List<Chat> groups = JsonConvert.DeserializeObject<List<Chat>>(jsonContent);

                    return groups;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<Chat> GetGroup(int chatId)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/api/get-group?chatId={chatId}");
                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    Chat chat = JsonConvert.DeserializeObject<Chat>(jsonContent);

                    return chat;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<int>> GetChatLikedMovies(int chatId)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/api/get-chat-liked?chatId={chatId}");
                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    List<int> likedMovies = JsonConvert.DeserializeObject<List<int>>(jsonContent);

                    return likedMovies;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> Logout(string authToken)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/api/logout?authToken={authToken}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}