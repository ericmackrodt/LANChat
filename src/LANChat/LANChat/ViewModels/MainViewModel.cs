using LANChat.Client;
using LANChat.Common;
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
    public class MainViewModel : BaseViewModel
    {
        public event EventHandler MessageArrived;

        private IChatClient _chatClient;
        private User _user;

        private ObservableCollection<IChatViewModel> _connectedUsers;
        public ObservableCollection<IChatViewModel> ConnectedUsers
        {
            get { return _connectedUsers; }
            set
            {
                _connectedUsers = value;
                NotifyChanged();
            }
        }

        private IChatViewModel _selectedChatSession;
        public IChatViewModel SelectedChatSession
        {
            get { return _selectedChatSession; }
            set
            {
                _selectedChatSession = value;
                NotifyChanged();
            }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                NotifyChanged();
            }
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                NotifyChanged();
            }
        }

        private ICommand _sendMessageCommand;
        public ICommand SendMessageCommand
        {
            get { return _sendMessageCommand; }
        }

        private ICommand _connectToServerCommand;
        public ICommand ConnectToServerCommand
        {
            get { return _connectToServerCommand; }
        }

        private ICommand _disconnectFromServerCommand;
        public ICommand DisconnectFromServerCommand
        {
            get { return _disconnectFromServerCommand; }
        }

        public MainViewModel(IChatClient chatClient)
        {
            _sendMessageCommand = new AsyncRelayCommand(SendMessage);
            _connectToServerCommand = new AsyncRelayCommand(ConnectToServer);
            _disconnectFromServerCommand = new RelayCommand(DisconnectFromServer);

            _chatClient = chatClient;
            _chatClient.OnConnected += o => Dispatch(OnConnected, o);
            _chatClient.OnUserConnected += o => Dispatch(OnUserConnected, o);
            _chatClient.OnUserDisconnected += o => Dispatch(OnUserDisconnected, o);

            ConnectedUsers = new ObservableCollection<IChatViewModel>();
            var publicSession = new PublicChatViewModel(_chatClient);
            publicSession.MessageArrived += OnMessageReceived;
            ConnectedUsers.Add(publicSession);
            SelectedChatSession = publicSession;
        }

        private async Task ConnectToServer(object obj)
        {
            if (string.IsNullOrWhiteSpace(Username)) return;

            if (_user == null)
                _user = new User()
                {
                    Name = Username,
                    UserID = Guid.NewGuid().ToString()
                };
            //192.168.25.43:8089
            await _chatClient.Connect("http://localhost:6667", _user);
        }

        private void DisconnectFromServer(object obj)
        {
            _chatClient.Disconnect(_user);
        }

        private void OnMessageReceived(object sender, EventArgs args)
        {
            if (MessageArrived != null)
                MessageArrived(this, new EventArgs());
        }

        private void OnUserConnected(User user)
        {
            if (!ConnectedUsers.Any(o => o.SessionUser != null && o.SessionUser.UserID == user.UserID))
            {
                var personalChat = new PersonalChatViewModel(_chatClient, user, _user);
                personalChat.MessageArrived += OnMessageReceived;
                ConnectedUsers.Add(personalChat);
            }
        }

        private void OnUserDisconnected(User user)
        {
            var u = ConnectedUsers.FirstOrDefault(o => o.SessionUser != null && o.SessionUser.UserID == user.UserID);
            ConnectedUsers.Remove(u);
        }

        private void OnConnected(User[] contacts)
        {
            foreach (var user in contacts)
            {
                var personalChat = new PersonalChatViewModel(_chatClient, user, _user);
                personalChat.MessageArrived += OnMessageReceived;
                ConnectedUsers.Add(personalChat);
            }
        }

        private async Task SendMessage(object obj)
        {
            if (string.IsNullOrWhiteSpace(Message)) return;

            await SelectedChatSession.SendMessage(_user, Message);

            Message = null;
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
