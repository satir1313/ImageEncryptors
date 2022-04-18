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

        public bool InsertMessage(List<int> keyList, ref BitArray bits) {
            bool success = false;

            try {
                BitArray msgBits = GetMessageBits();

                for (int i = 0; i < bits.Length; ++i) {
                    for (int j = 0; j < keyList.Count; ++j) {
                        if (keyList[j] == i) {
                            bits[i] = msgBits[j];
                        }
                    }
                }
                success = true;
            }
            catch (Exception e) { 
                Console.WriteLine(e.Message);   
            }
            return success;
        }

        public string GetMessage(List<int> keyList, ref BitArray bits) {
            BitArray bb = new BitArray(GetMessageBits().Length);
            for (int k = 0; k < keyList.Count; ++k) {
                bb[k] = bits[keyList[k]];
            }
            byte[] ll = new byte[bb.Length / 8];
            bb.CopyTo(ll, 0);

            string str2 = Encoding.Default.GetString(ll);
            return str2;
        }

    } // end class
}
