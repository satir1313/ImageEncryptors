using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageEncryptors {
    internal class ImageMath {

        Image Image_math;
        public ImageMath(Image image) {
            this.Image_math = image;
        }

        private long[] GetImageDimension() {

            int heigth = Image_math.Height;
            int width = Image_math.Width;

            long dimension = heigth * width;

            long numberOfBytes = (dimension * 4) + 14; // 14 bytes Bitmap overhead
            long numberOfBits = numberOfBytes * 8;

            long[] bitByte = { numberOfBits, numberOfBytes};

            return bitByte;
        }

        public List<long> GetKeyPosition() {

            List<long> keyPositions = new List<long>();

            long[] bitBytes = GetImageDimension();

            long convertibleBite = bitBytes[0] - (14 * 8);

            for (int i = 0; i < convertibleBite; i++) { 
                if(i % 8 == 0 && i != 0 && i > 5000 && i < convertibleBite-5000) {
                    keyPositions.Add(i);
                }
            }

            return keyPositions;
        }

    }
}
