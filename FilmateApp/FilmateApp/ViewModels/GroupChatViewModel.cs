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
        public GroupChatViewModel(int chatId, ChatService chatService)
        {
            this.chatId = chatId;
            this.client = new TMDbClient(App.APIKey);
            client.GetConfigAsync();
            this.chatService = chatService;
            this.proxy = FilmateAPIProxy.CreateProxy();
            
            InitializeHub();
            LoadGroup();
        }
        
        public async void InitializeHub()
        {
            try
            {
                chatService.ReceiveMessage(GetMessage);
                await chatService.Connect();
            }
            catch (System.Exception exp)
            {
                
            }
        }

        public Command SendMsgCommand => new Command<SendMessageEventArgs>(SendMsg);
        private void SendMsg(SendMessageEventArgs args)
        {
            string text = args.Message.Text;
            Msg message = new Msg()
            {
                AccountId = CurrentAccount.AccountId,
                ChatId = chatId,
                Content = text,
                SentDate = DateTime.Now,
                Account = CurrentAccount
            };

            chatService.SendMessage(new MsgDTO(message));
            AddMessage(message);
        }

        private void GetMessage(MsgDTO message)
        {
            AddMessage(new Msg()
            {
                AccountId = message.AccountId,
                ChatId = message.ChatId,
                Content = message.Content,
                SentDate = message.SentDate,
                Account = new Account()
                {
                    AccountName = message.AccountName,
                    ProfilePicture = message.ProfilePath
                }
            });
        }

        private void AddMessage(Msg message)
        {
            Messages.Add(message);
        }

        private async void LoadGroup()
        {
            FilmateAPIProxy proxy = FilmateAPIProxy.CreateProxy();
            Chat group = await proxy.GetGroup(chatId);

            Messages = new ObservableCollection<Msg>(group.Msgs);
            CurrentAccount = ((App)App.Current).CurrentAccount;
            currentUser = new Author()
            {
                Name = CurrentAccount.AccountName,
                Avatar = $"{proxy.baseUri}/{CurrentAccount.ProfilePicture}"
            };

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
                RecommendedMovie.PosterPath = uri.AbsoluteUri;
            }
        }

        public Command GoToMovie => new Command(() => App.Current.MainPage.Navigation.PushAsync(new MovieView(RecommendedMovie.Id)));

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

        private ObservableCollection<Msg> messages;
        public ObservableCollection<Msg> Messages
        {
            get => messages;
            set => SetValue(ref messages, value);
        }

        private Account currentAccount;
        public Account CurrentAccount
        {
            get => currentAccount;
            set => SetValue(ref currentAccount, value);
        }

        private Author currentUser;
        public Author CurrentUser
        {
            get => currentUser;
            set => SetValue(ref currentUser, value);
        }
    }
}
