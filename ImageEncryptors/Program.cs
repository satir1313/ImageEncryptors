using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Collections;

namespace ImageEncryptors
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Image image = Image.FromFile("../../Images/bitcoin.jpg");
            Encoding enc8 = Encoding.Default;

            Bitmap img = new Bitmap("../../Images/bitcoin.jpg");
            //get byte data of selected image
            byte[] byteImage = ImageToByte(img);
            //get bits of image
            BitArray bitArray = new BitArray(byteImage);

            // resize byte and Bit arrays
            //Array.Resize<byte>(ref byteImage, (byteImage.Length)+1);
            //bitArray.Length += 8;

            // create secret messge bits
            Message msg = new Message("test");
            BitArray msgBits = msg.GetMessageBits();

            for(int i = 0; i < bitArray.Length; i++)
            {
                if(i >= 1600 && i <= 1600 + (msgBits.Length-1))
                {
                    bitArray[i] = msgBits[i-1600];       
                }
            }

            Array.Clear(byteImage, 0, byteImage.Length);
            // copy manippulated bits to byte array of image
            bitArray.CopyTo(byteImage, 0);
            // for testing if massage has successfully stored
            string str = enc8.GetString(byteImage);
            File.WriteAllText("../../Images/byteTotext.txt",str);

            // create new identical image
            Bitmap x = (Bitmap)((new ImageConverter()).ConvertFrom(byteImage));
            //save new image
            x.Save("../../Images/bitcoin_1.bmp");

            // find difference in base64 strings of original image and manuplated image
            //FindDiffrence(origin, current);

            Console.WriteLine("Process Done!");
            Console.ReadKey();

        }

        private static void FindDiffrence(string a, string b)
        {

            File.WriteAllText("../../Images/origin.txt", a);
            File.WriteAllText("../../Images/current.txt", b);

            if (a.Equals(b))
                Console.WriteLine("Nothing changed!!!!");

            char[] originChar = a.ToCharArray();
            char[] currentChar = b.ToCharArray();

            Console.WriteLine("originChar   Length:   {0}", originChar.Length);
            Console.WriteLine("currentChar   Length:   {0}", currentChar.Length);

            for (int h = 0; h < originChar.Length; h++)
            {
                if (originChar[h] != currentChar[h])
                {
                    Console.WriteLine("diff is in index {0} and char is {1}", h, currentChar[h]);
                }

            }
        }


        public static void PrintValues(IEnumerable myList, int myWidth)
        {
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

        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        public static string ToReadableByteArray(byte[] bytes)
        {
            return string.Join(", ", bytes);
        }

        public static string ImageToBase64(string path)
        {
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
        public static Image Base64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }

        public byte[] stringToByteArray(string source)
        {
            byte[] bytes = Encoding.Default.GetBytes(source);
            return bytes;
        }

    } //end class
}


