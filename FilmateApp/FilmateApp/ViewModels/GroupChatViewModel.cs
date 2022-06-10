using System;
using System.Collections.Generic;
using System.Text;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.General;
using FilmateApp.Services;
using FilmateApp.Models;
using System.Linq;
using FilmateApp.Views;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Syncfusion.XForms.Chat;

namespace FilmateApp.ViewModels
{
    internal class GroupChatViewModel : BaseViewModel
    {
        private int chatId;
        private TMDbClient client;
        private ChatService chatService;
        private FilmateAPIProxy proxy;
        public GroupChatViewModel(int chatId, ChatService chatService, List<string> groupIds)
        {
            CurrentAccount = ((App)App.Current).CurrentAccount;

            this.Groups = groupIds;
            this.chatId = chatId;
            this.client = new TMDbClient(App.APIKey);
            client.GetConfigAsync();
            this.chatService = chatService;
            this.proxy = FilmateAPIProxy.CreateProxy();

            LoadGroup();
            InitializeHub();
        }
        
        public async void InitializeHub()
        {
            try
            {
                chatService.RegisterToReceiveMessageFromGroup(ReceiveMessageFromGroup);
                await chatService.Connect(Groups.ToArray());
            }
            catch (System.Exception exp)
            {
                
            }
        }

        public Command SendMsgCommand => new Command(SendMsg);
        private async void SendMsg()
        {
            string text = Message;
            if (text != null && text != "")
            {
                Message = "";
                Msg message = new Msg()
                {
                    AccountId = CurrentAccount.AccountId,
                    ChatId = chatId,
                    Content = text,
                    SentDate = DateTime.Now,
                    Account = CurrentAccount
                };

                //await chatService.SendMessage(new MsgDTO(message));
                await chatService.SendMessageToGroup(new MsgDTO(message), Group.ChatId.ToString());
            }
        }
        
        private async void ReceiveMessageFromGroup(int accountId, string message, string groupId)
        {
            Account a = await proxy.GetAccountById(accountId);
            Msg msg = new Msg()
            {
                AccountId = accountId,
                ChatId = int.Parse(groupId),
                Content = message,
                SentDate = DateTime.Now,
                Account = a
            };
            AddMessage(msg);
        }

        private void AddMessage(Msg message)
        {
            Messages.Insert(0, message);
            message.Account.ProfilePicture = $"{proxy.baseUri}/imgs/{message.Account.ProfilePicture}";
            MessagesLoaded?.Invoke();
        }
        
        private async void LoadGroup()
        {
            Group = await proxy.GetGroup(chatId);

            Messages = new ObservableCollection<Msg>((from msg in Group.Msgs orderby msg.SentDate descending select msg));
            var accounts = Group.Msgs.GroupBy(m => m.Account).Select(x => x.First()).ToList();
            foreach (Msg msg in accounts)
            {
                msg.Account.ProfilePicture = $"{proxy.baseUri}/imgs/{msg.Account.ProfilePicture}";
            }
            
            MessagesLoaded?.Invoke();

            GetChatMovieRecommendation();
        }

        private async void GetChatMovieRecommendation()
        {
            FilmateAPIProxy proxy = FilmateAPIProxy.CreateProxy();
            List<int> likedMovies = await proxy.GetChatLikedMovies(chatId);
            Dictionary<int, int> recommendedMoviesDict = new Dictionary<int, int>();

            foreach (int movieId in likedMovies)
            {
                SearchContainer<SearchMovie> results = await client.GetMovieRecommendationsAsync(movieId);
                foreach (SearchMovie result in results.Results)
                {
                    try
                    {
                        recommendedMoviesDict[result.Id]++;
                    }
                    catch
                    {
                        recommendedMoviesDict[result.Id] = 1;
                    }
                }
            }
            
            var recommendedMovies = (from entry in recommendedMoviesDict orderby entry.Value descending select entry);
            RecommendedMovie = await client.GetMovieAsync(recommendedMovies.ElementAt(0).Key);
            if (RecommendedMovie != null)
            {
                Uri uri = client.GetImageUrl("w342", RecommendedMovie.PosterPath);
                RecommendedMoviePoster = uri.AbsoluteUri;
            }
        }

        public Command GoToMovie => new Command(() => App.Current.MainPage.Navigation.PushAsync(new MovieView(RecommendedMovie.Id)));
        public Command InfoCommand => new Command(() => App.Current.MainPage.Navigation.PushAsync(new ChatInfoView(Group)));


        private Chat group;
        public Chat Group
        {
            get => group;
            set => SetValue(ref group, value);
        }

        private Movie recommendedMovie;
        public Movie RecommendedMovie
        {
            get => recommendedMovie;
            set => SetValue(ref recommendedMovie, value);
        }

        private string recommendedMoviePoster;
        public string RecommendedMoviePoster
        {
            get => recommendedMoviePoster;
            set => SetValue(ref recommendedMoviePoster, value);
        }

        private ObservableCollection<Msg> messages;
        public ObservableCollection<Msg> Messages
        {
            get => messages;
            set => SetValue(ref messages, value);
        }

        private List<string> groups;
        public List<string> Groups
        {
            get => groups;
            set => SetValue(ref groups, value);
        }

        private Account currentAccount;
        public Account CurrentAccount
        {
            get => currentAccount;
            set => SetValue(ref currentAccount, value);
        }

        private string message;
        public string Message
        {
            get => message;
            set => SetValue(ref message, value);
        }

        public Action MessagesLoaded;
    }
}
