using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LANChat.Common
{
    public class ChatMessage
    {
        public string Content { get; set; }
        public User From { get; set; }
        public User To { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
