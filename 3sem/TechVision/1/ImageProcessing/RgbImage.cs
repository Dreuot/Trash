using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing
{
    public class RgbImage
    {
        public int Width { get; set; }
        public int Height { get; set; }

        //public RGB[,] RGB { get; set; }
        public double[,] R { get; set; }
        public double[,] G { get; set; }
        public double[,] B { get; set; }

        private RgbImage(int width, int height)
        {
            Width = width;
            Height = height;
            //RGB = new RGB[Width, Height];
            R = new double[Width, Height];
            G = new double[Width, Height];
            B = new double[Width, Height];
        }

        public async Task<Bitmap> ToBitmapAsync()
        {
            return await Task.Run(() => ToBitmap());
        }

        public Bitmap ToBitmap()
        {
            Bitmap result = new Bitmap(Width, Height);
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    result.SetPixel(x, y, Color.FromArgb((int)Normalize(R[x, y]), (int)Normalize(G[x, y]), (int)Normalize(B[x, y])));
                }
            }

            return result;
        }

        public async Task<double[,]> ToGrayScaleAsync()
        {
            return await Task.Run(() => ToGrayScale());
        }

        public double[,] ToGrayScale()
        {
            double[,] result = new double[Width, Height];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    result[x, y] = R[x, y] * 0.3 + G[x, y] * 0.59 + B[x, y] * 0.11;
                }
            }

            return result;
        }

        public static async Task<RgbImage> FromBitmapAsync(Bitmap image)
        {
            return await Task.Run(() => FromBitmap(image));
        }

        public static RgbImage FromBitmap(Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;
            RgbImage result = new RgbImage(width, height);
            Color pixel;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    pixel = image.GetPixel(x, y);
                    //result.RGB[x, y] = new RGB(pixel.R, pixel.G, pixel.B);
                    result.R[x, y] = pixel.R;
                    result.G[x, y] = pixel.G;
                    result.B[x, y] = pixel.B;
                }
            }

            return result;
        }

        public static async Task<RgbImage> FromYiqAsync(YiqImage image)
        {
            return await Task.Run(() => FromYiq(image));
        }

        public static RgbImage FromYiq(YiqImage image)
        {
            int width = image.Width;
            int height = image.Height;
            RgbImage result = new RgbImage(width, height);

            Matrix m = new Matrix(3, 3);
            m.Init(1, 0.956, 0.623, 1, -0.272, -0.648, 1, -1.105, 1.705);
            Matrix yiq = new Matrix(3, 1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    yiq.Init(image.Y[x, y], image.Ic[x, y], image.Qc[x, y]);
                    Matrix rbg = m * yiq;
                    double nr = Normalize(rbg[0, 0]);
                    double ng = Normalize(rbg[1, 0]);
                    double nb = Normalize(rbg[2, 0]);
                    //result.RGB[x, y] = new RGB(nr, ng, nb);
                    result.R[x, y] = nr;
                    result.G[x, y] = ng;
                    result.B[x, y] = nb;
                }
            }

            return result;
        }

        private static double Normalize(double value)
        {
            return value > 255 ? 255 : value < 0 ? 0 : value;
        }
    }
}
