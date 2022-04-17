using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageEncryptors
{
    internal class Message
    {
        public string message { get; set; }
        public Message(string message)
        {
            this.message = message;
        }

        public BitArray GetMessageBits()
        {
            byte[] bytes = Encoding.Default.GetBytes(this.message);
            BitArray bits = new BitArray(bytes);
            return bits;
        }
    }
}
