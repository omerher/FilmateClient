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
        public int ChatDescription { get; set; }
        public DateTime CreationDate { get; set; }
        public string Icon { get; set; }

        public virtual List<ChatMember> ChatMembers { get; set; }
        public virtual List<Msg> Msgs { get; set; }
    }
}
