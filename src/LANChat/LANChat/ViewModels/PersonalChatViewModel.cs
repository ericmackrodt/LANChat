using LANChat.Client;
using LANChat.Common;
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
    public class PersonalChatViewModel : BaseViewModel, IChatViewModel
    {
        public event EventHandler MessageArrived;

        private IChatClient _chatClient;
        private User _self;

        private User _sessionUser;
        public User SessionUser
        {
            get { return _sessionUser; }
            set
            {
                _sessionUser = value;
                NotifyChanged();
            }
        }

        private ObservableCollection<ChatMessage> _messages;
        public ObservableCollection<ChatMessage> Messages
        {
            get { return _messages; }
            set
            {
                _messages = value;
                NotifyChanged();
            }
        }

        public PersonalChatViewModel(IChatClient chatClient, User sessionUser, User self)
        {
            _chatClient = chatClient;
            _self = self;
            SessionUser = sessionUser;

            _chatClient.OnPersonalMessageReceived += (message) => Dispatch(OnPersonalMessageReceived, message);
        }

        public async Task SendMessage(User currentUser, string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;

            await _chatClient.SendPersonalMessage(SessionUser, currentUser, message);
        }

        private void OnPersonalMessageReceived(ChatMessage message)
        {
            if ((_self.UserID == message.From.UserID && SessionUser.UserID == message.To.UserID) || (_self.UserID == message.To.UserID && SessionUser.UserID == message.From.UserID))
            {
                if (Messages == null)
                    Messages = new ObservableCollection<ChatMessage>();

                Messages.Add(message);

                if (MessageArrived != null)
                    MessageArrived(this, new EventArgs());
            }
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
