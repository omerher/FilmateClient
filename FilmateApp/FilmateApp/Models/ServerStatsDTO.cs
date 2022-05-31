using System;
using System.Collections.Generic;
using System.Text;

namespace FilmateApp.Models
{
    public class ServerStatsDTO
    {
        public int NumAccounts { get; set; }
        public int TodaysMessages { get; set; }
        public int TotalMessages { get; set; }
        public int NumGroups { get; set; }
        public int NumSuggestions { get; set; }
    }
}
