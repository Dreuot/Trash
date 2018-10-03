using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FirstLib
{
    public class ImageWrapper
    {
        const int colorCount = 3; // RGB
        const int r = 0;
        const int g = 1;
        const int b = 2;

        private Bitmap image;
        private int[,] inner;
        private int[,] colored;

        public ImageWrapper(Bitmap image)
        {
            this.image = image;
            inner = ImageToGrayArray(image);
            colored = ImageToColoredArray(image);
        }

        int this[int x, int y]
        {
            get
            {
                return inner[x, y];
            }
            set
            {
                inner[x, y] = value;
            }
        }

        public static int[,] ImageToGrayArray(Bitmap image)
        {
            int[,] result = new int[image.Width, image.Height];
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    var pixel = image.GetPixel(i, j);
                    result[i, j] = (int)Math.Floor(pixel.R * 0.3 + pixel.G * 0.59 + pixel.B * 0.11);
                }
            }

            return result;
        }

        public static int[,] ImageToColoredArray(Bitmap image)
        {
            int[,] result = new int[image.Width * colorCount, image.Height];
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    var pixel = image.GetPixel(i, j);
                    result[i + r, j] = pixel.R;
                    result[i + g, j] = pixel.G;
                    result[i + b, j] = pixel.B;
                }
            }

            return result;
        }

        public static Bitmap GrayArrayToImage(int[,] array)
        {
            Bitmap image = new Bitmap(array.GetLength(0), array.GetLength(1));
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    image.SetPixel(x, y, Color.FromArgb(array[x, y], array[x, y], array[x, y]));
                }
            }

            return image;
        }
    }
}
