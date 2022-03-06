using System;
using System.Collections.Generic;

namespace FilmateApp.Models
{
    public class ChatMember
    {
        public int AccountId { get; set; }
        public int ChatId { get; set; }
        public bool IsAdmin { get; set; }

        public virtual Account Account { get; set; }
        public virtual Chat Chat { get; set; }
    }
}
