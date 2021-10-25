using System;
using System.Collections.Generic;
using System.Text;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;

namespace FilmateApp.Services
{
    class TMDBAPIProxy
    {
        public TMDbClient client;
        private string apiKey;
        private string baseUri;
        
        public TMDBAPIProxy()
        {
            apiKey = "c80b17a14eb9a321ae1fc3f4680e410e";
            client = new TMDbClient(apiKey);
        }
    }
}
