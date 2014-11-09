using LANChat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LANChat.Server
{
    public class ConnectedUser
    {
        public User Data { get; set; }
        public HashSet<string> ConnectionIds { get; set; }
    }
}
