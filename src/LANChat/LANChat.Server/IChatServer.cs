using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LANChat.Server
{
    public interface IChatServer : IDisposable
    {
        event ServerLogEventHandler OnServerLog;

        void Start(string host);
    }
}
