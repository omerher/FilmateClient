using System;
using System.Collections.Generic;
using TMDbLib.Objects.Movies;

namespace FilmateApp.Models
{
    public class LikedMovie
    {
        public int AccountId { get; set; }
        public int MovieId { get; set; }

        public virtual Account Account { get; set; }

        // added
        public virtual Movie Movie { get; set; }
    }
}
