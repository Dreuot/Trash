using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing
{
    public class YiqImage
    {
        public int Width { get; set; }
        public int Height { get; set; }

        //public YIQ[,] YIQ { get; set; }
        public double[,] Y { get; set; }
        public double[,] Ic { get; set; }
        public double[,] Qc { get; set; }

        private int x;

        private YiqImage(int width, int height)
        {
            Width = width;
            Height = height;
            Y = new double[Width, Height];
            Ic = new double[Width, Height];
            Qc = new double[Width, Height];
        }

        public async Task<Bitmap> ToBitmapAsync()
        {
            RgbImage rgb = await RgbImage.FromYiqAsync(this);
            return await rgb.ToBitmapAsync();
        }

        public Bitmap ToBitmap()
        {
            RgbImage rgb = RgbImage.FromYiq(this);
            return rgb.ToBitmap();
        }

        public static async Task<YiqImage> FromBitmapAsync(Bitmap image)
        {
            RgbImage rgb = await RgbImage.FromBitmapAsync(image);
            return await YiqImage.FromRgbAsync(rgb);
        }

        public static YiqImage FromBitmap(Bitmap image)
        {
            RgbImage rgb = RgbImage.FromBitmap(image);
            return YiqImage.FromRgb(rgb);
        }

        public static async Task<YiqImage> FromRgbAsync(RgbImage image)
        {
            return await Task.Run(() => FromRgb(image));
        }

        public static YiqImage FromRgb(RgbImage image)
        {
            int width = image.Width;
            int height = image.Height;

            YiqImage result = new YiqImage(width, height);

            Matrix m = new Matrix(3, 3);
            m.Init(0.299, 0.587, 0.114, 0.596, -0.274, -0.321, 0.211, -0.526, 0.311);
            Matrix rbg = new Matrix(3, 1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    rbg.Init(image.R[x, y], image.G[x, y], image.B[x, y]);
                    Matrix yiq = m * rbg;
                    //result.YIQ[x, y] = new YIQ(yiq[0, 0], yiq[1, 0], yiq[2, 0]);
                    result.Y[x, y] = yiq[0, 0];
                    result.Ic[x, y] = yiq[1, 0];
                    result.Qc[x, y] = yiq[2, 0];
                }
            }

            return result;
        }

        public int[] GetBarChart()
        {
            int[] result = new int[256];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    result[(int)Y[x, y]]++;
                }
            }

            return result;
        }

        public YiqImage Clone()
        {
            YiqImage result = new YiqImage(this.Width, this.Height);
            result.Y = (double[,])Y.Clone();
            result.Ic = (double[,])Ic.Clone();
            result.Qc = (double[,])Qc.Clone();

            return result;
        }
    }
}
