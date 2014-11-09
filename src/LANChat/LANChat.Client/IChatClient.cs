using LANChat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LANChat.Client
{
    public interface IChatClient
    {
        event MessageReceivedEventHandler OnMessageReceived;
        event MessageReceivedEventHandler OnPersonalMessageReceived;
        event UserEventHandler OnUserConnected;
        event UserEventHandler OnUserDisconnected;
        event ConnectedEventHandler OnConnected;
        Task Connect(string host, User currentUser);
        Task SendMessage(User from, string content);
        Task SendPersonalMessage(User from, User to, string content);
        void Disconnect(User user);
    }
}
