using System;
using System.Collections.Generic;

namespace FilmateApp.Models
{
    public class Chat
    {
        public Chat()
        {
            ChatMembers = new List<ChatMember>();
            Msgs = new List<Msg>();
        }

        public int ChatId { get; set; }
        public string ChatName { get; set; }
        public string ChatDescription { get; set; }
        public DateTime CreationDate { get; set; }
        public int? SuggestedMovieId { get; set; }
        public string Icon { get; set; }
        public string InviteCode { get; set; }

        public virtual List<ChatMember> ChatMembers { get; set; }
        public virtual List<Msg> Msgs { get; set; }

        // added
        public Msg LastMessage { get; set; }
    }
}
