using System;
using System.Collections.Generic;
using System.Text;

namespace FilmateApp.Models
{
    public class MsgDTO
    {
        public MsgDTO(Msg msg)
        {
            this.AccountId = msg.AccountId;
            this.ChatId = msg.ChatId;
            this.Content = msg.Content;
            this.SentDate = msg.SentDate;
            this.AccountName = msg.Account.AccountName;
            this.ProfilePath = msg.Account.ProfilePicture;
        }

        public MsgDTO(int accountId, int chatId, string content, DateTime sentDate, string accountName, string profilePath)
        {
            AccountId = accountId;
            ChatId = chatId;
            Content = content;
            SentDate = sentDate;
        }

        public int AccountId { get; set; }
        public int ChatId { get; set; }
        public string Content { get; set; }
        public DateTime SentDate { get; set; }
        public string AccountName { get; set; }
        public string ProfilePath { get; set; }
    }
}
