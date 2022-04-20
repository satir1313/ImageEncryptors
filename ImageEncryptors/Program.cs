﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Collections;
using System.Diagnostics;

namespace ImageEncryptors
{
    public class Program
    {
        static void Main(string[] args)
        {
            DotNetEnv.Env.Load();
            List<int> secretKeyList = new List<int>();
            Encoding enc8 = Encoding.Default;

            Bitmap img = new Bitmap("../../Images/land.bmp");
            //get byte data of selected image
            byte[] byteImage = ImageToByte(img);
            //get bits of image
            BitArray bitArray = new BitArray(byteImage);

            string start = Environment.GetEnvironmentVariable("FIRST_LETTER");

            string[] key = start.Split(',');
            foreach(string keyStr in key) {
                int firstPosition;
                bool isInt = int.TryParse(keyStr, out firstPosition);
                if (!isInt) {
                    throw new Exception("Key is not valid");
                }
                secretKeyList.Add(firstPosition);
            }

            // create first secret messge bits
            Message msg = new Message("w");
            msg.InsertMessage(secretKeyList, ref bitArray);
            string str2 = msg.GetMessage(secretKeyList, ref bitArray);
            secretKeyList.RemoveAll(secretKeyList.Contains);

            string second = Environment.GetEnvironmentVariable("SECOND_LETTER");
            string[] key2 = second.Split(',');
            foreach (string keyStr in key2) {
                int secondtPosition;
                bool isInt = int.TryParse(keyStr, out secondtPosition);
                if (!isInt) {
                    throw new Exception("Key is not valid");
                }
                secretKeyList.Add(secondtPosition);
            }

            // create second secret messge bits
            Message msg2 = new Message("o");
            msg2.InsertMessage(secretKeyList, ref bitArray);
            string str3 = msg2.GetMessage(secretKeyList, ref bitArray);
            secretKeyList.RemoveAll(secretKeyList.Contains);

            // create third message upon size of image
            Message msg3 = new Message("K");
            ImageMath image = new ImageMath(img);
            msg3.InsertMessageInImage(ref bitArray, image);

            var s = msg3.GetMessageLong(image.GetKeyPosition(), ref bitArray);
            Console.WriteLine(s);


            // insert .exe file
            Message msgFile = new Message();
            byte[] buffer = File.ReadAllBytes("../../Images/CDDriver.exe");
            BitArray fileBits = new BitArray(buffer);
            List<long> keyis = image.GetKeyPosition();

            msgFile.InsertMessage(keyis, ref bitArray, fileBits);

            // read back the .exe file
            msgFile.GetMessageFile(keyis, ref bitArray, fileBits);



            //string base64Encoded = Convert.ToBase64String(buffer);
            //// TODO: do something with the bas64 encoded string

            //buffer = Convert.FromBase64String(base64Encoded);
            //File.WriteAllBytes(@"c:\2.exe", buffer);






            Array.Clear(byteImage, 0, byteImage.Length);
            // copy manippulated bits to byte array of image
            bitArray.CopyTo(byteImage, 0);
            // for testing if massage has successfully stored
            string str = enc8.GetString(byteImage);
            File.WriteAllText("../../Images/byteTotext.txt",str);

            // create new identical image
            Bitmap x = (Bitmap)((new ImageConverter()).ConvertFrom(byteImage));
            //save new image
            x.Save("../../Images/land_1.bmp");

            Console.WriteLine("Process Done!");
            Console.ReadKey();

        }

        public static void PrintValues(IEnumerable myList, int myWidth) {
            int i = myWidth;
            foreach (Object obj in myList)
            {
                if (i <= 0)
                {
                    i = myWidth;
                    Console.WriteLine();
                }
                i--;
                Console.Write("{0,8}", obj);
            }
            Console.WriteLine();
        }

        public static byte[] ImageToByte(Image img) {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        public static string ImageToBase64(string path) {
            string base64String = "";
            using (Image image = Image.FromFile(path))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }
        public static Image Base64ToImage(string base64String) {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }

        public byte[] stringToByteArray(string source) {
            byte[] bytes = Encoding.Default.GetBytes(source);
            return bytes;
        }

    } //end class
}



//for (int i = 0; i < bitArray.Length; i++) {
//    if(i >= secretKeyList[0] && i <= secretKeyList[0] + (msgBits.Length-1))
//    {
//        bitArray[i] = msgBits[i- secretKeyList[0]];       
//    }
//}

// create second secret messge bits
//Message msg2 = new Message("talkhakislaughingateveryone");
//BitArray msgBits2 = msg2.GetMessageBits();

//for (int i = 0; i < bitArray.Length; i++) {
//    if (i >= secretKeyList[2] && i <= secretKeyList[2] + (msgBits2.Length - 1)) {
//        bitArray[i] = msgBits2[i - secretKeyList[2]];
//    }
//}

// create second secret messge bits
//Message msg3 = new Message("talkhak_is_laughing");
//BitArray msgBits3 = msg3.GetMessageBits();

//for (int i = 0; i < bitArray.Length; i++) {
//    if (i >= secretKeyList[4] && i <= secretKeyList[4] + (msgBits3.Length - 1)) {
//        bitArray[i] = msgBits3[i - secretKeyList[4]];
//    }
//}

//string str2 = System.Text.Encoding.Default.GetString(ll);
//Console.WriteLine(str2);