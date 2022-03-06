using System;
using System.Collections.Generic;

namespace FilmateApp.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int AccountId { get; set; }
        public int MovieId { get; set; }
        public int Rating { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public DateTime PostDate { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }

        public virtual Account Account { get; set; }
    }
}
