using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LANChat.Server
{
    public interface IHubLogger
    {
        event ServerLogEventHandler OnHubLog;
        void LogMessage(string message);
    }
}
