using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FilmateApp.Models;

namespace FilmateApp.Services
{
    public interface IChatService
    {
        Task Connect(string[] groupNames);
        Task Disconnect(string[] groupNames);
        //Task SendMessage(string userId, string message);
        Task SendMessageToGroup(MsgDTO message, string groupName);
        //void RegisterToReceiveMessage(Action<string, string> GetMessageAndUser);
        void RegisterToReceiveMessageFromGroup(Action<int, string, string> GetMessageAndUserFromGroup);
    }
}
