//using ChatApp.Mobile.Services.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FilmateApp.Models;


namespace FilmateApp.Services
{
    public class ChatService
    {
        private readonly HubConnection hubConnection;
        public ChatService()
        {
            string baseUri = FilmateAPIProxy.CreateProxy().baseUri;
            hubConnection = new HubConnectionBuilder().WithUrl($"{baseUri}/chathub").Build();  
        }

        public async Task Connect()
        {
            await hubConnection.StartAsync();
        }

        public async Task Disconnect()
        {
            await hubConnection.StopAsync();
        }

        public async Task SendMessage(MsgDTO m)
        {
            await hubConnection.InvokeAsync("SendMessage", m);
        }

        public void ReceiveMessage(Action<MsgDTO> message)
        {
            hubConnection.On("ReceiveMessage", message);
        }
    }
}
