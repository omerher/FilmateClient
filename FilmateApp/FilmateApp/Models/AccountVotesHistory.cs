using System;
using System.Collections.Generic;

namespace FilmateApp.Models
{
    public class AccountVotesHistory
    {
        public int VoteId { get; set; }
        public int SuggestionId { get; set; }
        public int AccountId { get; set; }
        public DateTime VotedDate { get; set; }
        public bool VoteType { get; set; }

        public virtual Account Account { get; set; }
        public virtual Suggestion Suggestion { get; set; }
    }
}
