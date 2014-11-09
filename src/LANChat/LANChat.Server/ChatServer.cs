using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using Owin;
using Microsoft.Owin.Cors;
using Microsoft.AspNet.SignalR.Hubs;

namespace LANChat.Server
{
    public class ChatServer : IChatServer
    {
        public event ServerLogEventHandler OnServerLog;

        private List<Log> _serverLog;
        public List<Log> ServerLog
        {
            get { return _serverLog; }
            set { _serverLog = value; }
        }

        private IDisposable _webApp;
        private IHubLogger _hubLogger;

        public ChatServer()
        {
            _hubLogger = new HubLogger();
            _hubLogger.OnHubLog += OnHubLog;
            GlobalHost.DependencyResolver.Register(typeof(ChatHub), () => new ChatHub(_hubLogger));

            _serverLog = new List<Log>();
        }

        public void Start(string host)
        {
            _webApp = WebApp.Start(host);
            _hubLogger.LogMessage(string.Format("Server running on {0}", host));
        }

        private void OnHubLog(Log log)
        {
            ServerLog.Add(log);

            if (OnServerLog != null)
                OnServerLog(log);
        }

        public void Dispose()
        {
            _webApp.Dispose();
        }
    }
}
