//using ChatApp.Mobile.Services.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FilmateApp.Models;

namespace FilmateApp.Services
{
    public class ChatService : IChatService
    {
        private readonly HubConnection hubConnection;
        public ChatService()
        {
            string baseUri = FilmateAPIProxy.CreateProxy().baseUri;
            hubConnection = new HubConnectionBuilder().WithUrl($"{baseUri}/chathub").Build();  
        }

        public async Task Connect(string[] groupsIds)
        {
            if (hubConnection.State == HubConnectionState.Disconnected)
                await hubConnection.StartAsync();
            await hubConnection.InvokeAsync("OnConnect", groupsIds);
        }

        public async Task Disconnect(string[] groupsIds)
        {
            await hubConnection.InvokeAsync("OnDisconnect", groupsIds);
            await hubConnection.StopAsync();
        }

        public async Task SendMessageToGroup(MsgDTO message, string groupId)
        {
            await hubConnection.InvokeAsync("SendMessageToGroup", message, groupId);
        }

        public void RegisterToReceiveMessageFromGroup(Action<int, string, string> GetMessageAndUserFromGroup)
        {
            hubConnection.On("ReceiveMessageFromGroup", GetMessageAndUserFromGroup);
        }
    }
}
