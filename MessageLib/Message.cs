using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLib
{
    [Serializable]
    public class Message
    {
        public string Sender { get; set; }
        public string Content { get; set; }
        public DateTime SendedAt { get; set; }
        public override string ToString()
        {
            return Sender + " : (" + SendedAt + ")" + "=>" + Content;
        }
    }
}
