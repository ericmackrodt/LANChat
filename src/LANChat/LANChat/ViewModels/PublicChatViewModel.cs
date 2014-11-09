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
    public class PublicChatViewModel : BaseViewModel, IChatViewModel
    {
        public event EventHandler MessageArrived;

        private IChatClient _chatClient;

        public User SessionUser { get; set; }

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

        public PublicChatViewModel(IChatClient chatClient)
        {
            _chatClient = chatClient;

            _chatClient.OnMessageReceived += o => Dispatch(OnMessageReceived, o);
        }

        public async Task SendMessage(User currentUser, string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;

            await _chatClient.SendMessage(currentUser, message);
        }

        private void OnMessageReceived(ChatMessage message)
        {
            if (Messages == null)
                Messages = new ObservableCollection<ChatMessage>();

            Messages.Add(message);

            if (MessageArrived != null)
                MessageArrived(this, new EventArgs());
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
