using LANChat.Common;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LANChat.Server
{
    [HubName("ChatHub")]
    public class ChatHub : Hub
    {
        private static readonly ConcurrentDictionary<string, ConnectedUser> Users = new ConcurrentDictionary<string, ConnectedUser>();

        private IHubLogger _logger;

        public ChatHub(IHubLogger logger) 
        {
            _logger = logger;
        }

        public void SendChatMessage(ChatMessage message)
        {
            _logger.LogMessage(string.Format("User {0} sent message: \"{1}\"", message.From.Name, message.Content));
            Clients.All.sendChatMessage(message);
        }

        public void SendPersonalChatMessage(ChatMessage message)
        {
            var from = Users[message.From.UserID];
            var to = Users[message.To.UserID];
            var connections = from.ConnectionIds.Union(to.ConnectionIds).ToArray();
            _logger.LogMessage(string.Format("User {0} sent message to {1}: \"{2}\"", message.From.Name, message.To.Name, message.Content));
            Clients.Clients(connections).sendPersonalChatMessage(message);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string userName = Context.Headers["User-Name"];
            string userId = Context.Headers["User-Id"];
            string connectionId = Context.ConnectionId;

            ConnectedUser inUser;
            if (Users.TryGetValue(userId, out inUser))
            {
                lock (inUser.ConnectionIds)
                {
                    inUser.ConnectionIds.RemoveWhere(o => o.Equals(connectionId));

                    if (!inUser.ConnectionIds.Any())
                    {
                        ConnectedUser removedUser;
                        if (Users.TryRemove(userId, out removedUser))
                        {
                            Clients.Others.userDisconnected(removedUser.Data);
                        }
                    }
                }
            }

            _logger.LogMessage(string.Format("Hub OnDisconnected {0}\n", Context.ConnectionId));
            return (base.OnDisconnected(stopCalled));
        }

        public override Task OnConnected()
        {
            string userName = Context.Headers["User-Name"];
            string userId = Context.Headers["User-Id"];

            string connectionId = Context.ConnectionId;

            var user = Users.GetOrAdd(userId, o => new ConnectedUser()
            {
                Data = new User()
                {
                    UserID = userId,
                    Name = userName,
                },
                ConnectionIds = new HashSet<string>()
            });

            lock (user.ConnectionIds)
            {
                user.ConnectionIds.Add(connectionId);
            }

            if (user.ConnectionIds.Count == 1)
            {
                Clients.Others.userConnected(user.Data);
            }

            var connectedUsers = Users.Where(o => o.Key != userId).Select(o => o.Value.Data);
            Clients.Caller.onConnected(connectedUsers);

            _logger.LogMessage(string.Format("Client {0} - {1} Connected {2}", user.Data.UserID, user.Data.Name, Context.ConnectionId));
            return (base.OnConnected());
        }
    }
}
