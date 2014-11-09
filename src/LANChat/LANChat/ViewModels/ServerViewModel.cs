using LANChat.Server;
using MVVMBasic;
using MVVMBasic.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LANChat.ViewModels
{
    public class ServerViewModel : BaseViewModel, IDisposable
    {
        private IChatServer _chatServer;

        private bool _isConnected;
        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                NotifyChanged();
            }
        }

        private string _host;
        public string Host
        {
            get { return _host; }
            set
            {
                _host = value;
                NotifyChanged();
            }
        }

        private ObservableCollection<Log> _serverLogs;
        public ObservableCollection<Log> ServerLogs
        {
            get { return _serverLogs; }
            set
            {
                _serverLogs = value;
                NotifyChanged();
            }
        }

        private ICommand _connectServerCommand;
        public ICommand ConnectServerCommand
        {
            get { return _connectServerCommand; }
        }

        public ServerViewModel(IChatServer chatServer)
        {
            _chatServer = chatServer;
            _chatServer.OnServerLog += o => Dispatch(OnServerLog, o);

            _connectServerCommand = new RelayCommand(ConnectServer);

            Host = "http://localhost:6667/";
        }

        private void ConnectServer(object obj)
        {
            _chatServer.Start(Host);
        }

        private void OnServerLog(Log data)
        {
            if (ServerLogs == null)
                ServerLogs = new ObservableCollection<Log>();

            ServerLogs.Add(data);
        }

        public void Dispose()
        {
            _chatServer.Dispose();
        }

        private void Dispatch<T>(Action<T> action, T content)
        {
            App.Current.Dispatcher.BeginInvoke((Action)delegate()
            {
                action(content);
            });
        }
    }
}
