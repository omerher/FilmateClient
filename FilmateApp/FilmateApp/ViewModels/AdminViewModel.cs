using System;
using System.Collections.Generic;
using System.Text;
using FilmateApp.Models;
using FilmateApp.Services;

namespace FilmateApp.ViewModels
{
    public class AdminViewModel : BaseViewModel
    {
        public AdminViewModel()
        {
            LoadStats();
        }

        public async void LoadStats()
        {
            FilmateAPIProxy proxy = FilmateAPIProxy.CreateProxy();
            ServerStats = await proxy.GetAdminStats();
        }

        private ServerStatsDTO serverStats;
        public ServerStatsDTO ServerStats
        {
            get => serverStats;
            set
            {
                serverStats = value;
                OnPropertyChanged("ServerStats");
            }
        }
    }
}
