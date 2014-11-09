using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LANChat.Server
{
    public class Log
    {
        public string Message { get; set; }
        public DateTime Date { get; set; }

        public Log(string message, DateTime date)
        {
            Message = message;
            Date = date;
        }
    }
}
