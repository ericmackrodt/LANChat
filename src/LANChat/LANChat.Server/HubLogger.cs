using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LANChat.Server
{
    public class HubLogger : IHubLogger
    {
        public event ServerLogEventHandler OnHubLog;

        public void LogMessage(string message)
        {
            if (OnHubLog != null)
                OnHubLog(new Log(message, DateTime.Now));
        }
    }
}
