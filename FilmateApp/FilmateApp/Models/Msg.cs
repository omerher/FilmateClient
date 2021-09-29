using System;
using System.Collections.Generic;

namespace FilmateApp.Models
{
    public class Msg
    {
        public int MsgId { get; set; }
        public int AccountId { get; set; }
        public int ChatId { get; set; }
        public string Content { get; set; }
        public DateTime SentDate { get; set; }

        public virtual Account Account { get; set; }
        public virtual Chat Chat { get; set; }
    }
}
