using System;
using System.Collections.Generic;

namespace FilmateApp.Models
{
    public class Suggestion
    {
        public Suggestion()
        {
            AccountVotesHistories = new List<AccountVotesHistory>();
        }

        public int SuggestionId { get; set; }
        public int AccountId { get; set; }
        public int OriginalMovieId { get; set; }
        public int SuggestionMovieId { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public DateTime PostDate { get; set; }

        public virtual Account Account { get; set; }
        public virtual List<AccountVotesHistory> AccountVotesHistories { get; set; }
    }
}
