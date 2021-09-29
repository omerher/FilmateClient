using System;
using System.Collections.Generic;

namespace FilmateApp.Models
{
    public class ChatSuggestion
    {
        public int ChatId { get; set; }
        public int MovieId { get; set; }
        public bool? IsActive { get; set; }

        public virtual Chat Chat { get; set; }
    }
}
