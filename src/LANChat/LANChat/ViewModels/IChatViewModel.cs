using LANChat.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LANChat.ViewModels
{
    public interface IChatViewModel : INotifyPropertyChanged
    {
        event EventHandler MessageArrived;
        User SessionUser { get; set; }
        ObservableCollection<ChatMessage> Messages { get; set; }
        Task SendMessage(User currentUser, string message);
    }
}
