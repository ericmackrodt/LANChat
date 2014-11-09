using LANChat.Common;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LANChat.Client
{
    public class ChatClient : IChatClient
    {
        public event MessageReceivedEventHandler OnMessageReceived;
        public event MessageReceivedEventHandler OnPersonalMessageReceived;
        public event UserEventHandler OnUserConnected;
        public event UserEventHandler OnUserDisconnected;
        public event ConnectedEventHandler OnConnected;

        IHubProxy chatHubProxy;
        HubConnection hubConnection;

        public async Task Connect(string host, User currentUser)
        {
            hubConnection = new HubConnection(host);

            hubConnection.Headers.Add("User-Name", currentUser.Name);
            hubConnection.Headers.Add("User-Id", currentUser.UserID);

            chatHubProxy = hubConnection.CreateHubProxy("ChatHub");

            chatHubProxy.On<ChatMessage>("sendChatMessage", MessageReceived);
            chatHubProxy.On<ChatMessage>("sendPersonalChatMessage", PersonalMessageReceived);
            chatHubProxy.On<User[]>("onConnected", Connected);
            chatHubProxy.On<User>("userConnected", UserConnected);
            chatHubProxy.On<User>("userDisconnected", UserDisconnected);

            hubConnection.CookieContainer = new System.Net.CookieContainer();

            await hubConnection.Start();
        }

        public void Disconnect(User user)
        {
            hubConnection.Stop();
        }

        public async Task SendMessage(User currentUser, string content)
        {
            var message = new ChatMessage() 
            {
                Content = content,
                From = currentUser,
                TimeStamp = DateTime.Now
            };
            await chatHubProxy.Invoke("SendChatMessage", message);
        }

        public async Task SendPersonalMessage(User to, User from, string content)
        {
            var message = new ChatMessage()
            {
                Content = content,
                From = from,
                To = to,
                TimeStamp = DateTime.Now
            };
            await chatHubProxy.Invoke("SendPersonalChatMessage", message);
        }

        private void MessageReceived(ChatMessage obj)
        {
            if (OnMessageReceived != null)
                OnMessageReceived(obj);
        }

        private void PersonalMessageReceived(ChatMessage message)
        {
            if (OnMessageReceived != null)
                OnPersonalMessageReceived(message);
        }

        private void UserConnected(User obj)
        {
            if (OnUserConnected != null)
                OnUserConnected(obj);
        }

        private void UserDisconnected(User obj)
        {
            if (OnUserDisconnected != null)
                OnUserDisconnected(obj);
        }

        private void Connected(User[] contacts)
        {
            if (OnConnected != null)
                OnConnected(contacts);
        }
    }
}

