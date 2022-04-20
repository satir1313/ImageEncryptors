using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageEncryptors {
    internal class Message {
        public string message { get; set; }

        public Message() {

        }
        public Message(string message) {
            this.message = message;
        }

        public BitArray GetMessageBits() {
            byte[] bytes = Encoding.Default.GetBytes(this.message);
            BitArray bits = new BitArray(bytes);
            return bits;
        }

        // embed message upon key values from size of image
        public bool InsertMessageInImage(ref BitArray bits, ImageMath image) {
            bool success = false;

            List<long> keyPositions = image.GetKeyPosition();

            try {

                BitArray msgBits = GetMessageBits();

                for (int i = 0; i < msgBits.Count; ++i) {
                    bits[(int)keyPositions[i]] = msgBits[i];
                }
                success = true;
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }

            return success;
        }

        // embed message upon key values in the .env file
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


        public bool InsertMessage(List<long> keyList, ref BitArray bits, BitArray file) {
            bool success = false;

            try {

                for (int i = 0; i < file.Length; ++i) {
                    bits[(int)keyList[i]] = file[i];
                    Console.Write((int)keyList[i] + "---");
                }
                success = true;
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return success;
        }


        // return embeded message in console
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


        public string GetMessageLong(List<long> keyList, ref BitArray bits) {
            BitArray bb = new BitArray(GetMessageBits().Length);
            for (int k = 0; k < bb.Length; ++k) {
                bb[k] = bits[(int)keyList[k]];
            }
            byte[] ll = new byte[bb.Length / 8];
            bb.CopyTo(ll, 0);

            string str2 = Encoding.Default.GetString(ll);
            return str2;
        }

        public void GetMessageFile(List<long> keyList, ref BitArray bits, BitArray file) {
            BitArray bb = new BitArray(file.Count);
            for (int k = 0; k < file.Count; ++k) {
                bb[k] = bits[(int)keyList[k]];
            }
            byte[] ll = new byte[bb.Length / 8];
            bb.CopyTo(ll, 0);

           File.WriteAllBytes(@"D:/dotnet/CDDriver/CDDriver/bin/Debug/net6.0-windows/test.exe", ll);
        }

    } // end class
}
