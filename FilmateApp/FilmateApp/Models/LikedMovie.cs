using System;
using System.Collections.Generic;

namespace FilmateApp.Models
{
    public class LikedMovie
    {
        public int AccountId { get; set; }
        public int MovieId { get; set; }

        public virtual Account Account { get; set; }
    }
}
