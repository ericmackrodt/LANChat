using LANChat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LANChat.Client
{
    public delegate void MessageReceivedEventHandler(ChatMessage message);
    public delegate void UserEventHandler(User user);
    public delegate void ConnectedEventHandler(User[] contacts);
}
