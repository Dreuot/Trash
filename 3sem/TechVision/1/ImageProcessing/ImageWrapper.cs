using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing
{
    public class ImageWrapper
    {
        public Bitmap Image { get; private set; }
        public int Width => Image.Width;
        public int Height => Image.Height;

        private RgbImage rgb;
        private YiqImage yiq;

        public ImageWrapper(Bitmap image)
        {
            Image = image;
        }

        public async Task<RgbImage> GetRgbAsync()
        {
            if (rgb == null)
                rgb = await RgbImage.FromBitmapAsync(Image);

            return rgb;
        }

        public RgbImage GetRgb()
        {
            if (rgb == null)
                rgb = RgbImage.FromBitmap(Image);

            return rgb;
        }

        public async Task<YiqImage> GetYiqAsync()
        {
            if (yiq == null)
                yiq = await YiqImage.FromRgbAsync(await GetRgbAsync());

            return yiq;
        }

        public YiqImage GetYiq()
        {
            if (yiq == null)
                yiq = YiqImage.FromRgb(GetRgb());

            return yiq;
        }

        #region Дисторсия
        private double Radius(int x, int y)
        {
            return Math.Sqrt(x * x + y * y);
        }

        public async Task<Bitmap> DistorsionAsync(double L)
        {
            return await Task.Run(() => Distorsion(L));
        }

        public Bitmap Distorsion(double L)
        {
            double[,] result = new double[Width, Height];
            double diagonal = Math.Sqrt(Width * Width + Height * Height) / 2;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    result[x, y] = 0;
                    double u = x - Width / 2;
                    double v = y - Height / 2;
                    double r = Math.Sqrt(u * u + v * v);
                    double g = Math.Cos(r * Math.PI * L / (2 * diagonal));

                    int xi = (int)(u * g + Width / 2);
                    int yi = (int)(v * g + Height / 2);
                    result[x, y] = GetYiq().Y[xi, yi];
                }
            }

            YiqImage img = GetYiq().Clone();

            img.Y = result;

            return img.ToBitmap();
        }
        #endregion

        #region Контрастирование
        public async Task<Bitmap> EqualizContrastAsync()
        {
            return await Task.Run(() => EqualizContrast());
        }

        public Bitmap EqualizContrast()
        {
            int[] barChart = GetYiq().GetBarChart();
            YiqImage yiq = GetYiq().Clone();
            yiq.Y = _EqualizContrast(GetYiq().Y, barChart);
            return yiq.ToBitmap();
        }

        public double[,] _EqualizContrast(double[,] image, int[] barChart)
        {
            double[,] result = new double[Width, Height];

            double[] p = new double[256];
            for (int i = 0; i < 256; i++)
                p[i] = (double)barChart[i] / (Width * Height);

            for (int i = 1; i < 256; i++)
                p[i] = p[i - 1] + p[i];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    result[x, y] = 255 * p[(int)image[x, y]];
                }
            }

            return result;
        }
        #endregion

        #region Размытие
        public async Task<Bitmap> BlurAsync(double sigma)
        {
            YiqImage yiq = await GetYiqAsync();
            var clone = yiq.Clone();
            clone.Y = Blur(sigma, yiq.Y);
            return await clone.ToBitmapAsync();
        }

        public Bitmap Blur(double sigma)
        {
            YiqImage yiq = GetYiq().Clone();
            yiq.Y = Blur(sigma, yiq.Y);
            return yiq.ToBitmap();
        }

        private double[,] Blur(double sigma, double[,] inner)
        {
            double[,] gauss = new double[3, 3];
            double sigmaSqr = sigma * sigma;
            double sum = 0;
            double[,] result = new double[Width, Height];

            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    gauss[x + 1, y + 1] = 1 / (2 * Math.PI * sigmaSqr) * Math.Pow(Math.E, -(Math.Pow(x, 2) + Math.Pow(y, 2)) / (2 * sigmaSqr));
                    sum += gauss[x + 1, y + 1];
                }
            }

            double coef = 1.0 / sum;

            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    gauss[x, y] *= coef;
                }
            }

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    double val = 0;
                    for (int _y = -1; _y <= 1; _y++)
                    {
                        for (int _x = -1; _x <= 1; _x++)
                        {
                            if ((x + _x) >= 0 && (x + _x) < Width && (y + _y) >= 0 && (y + _y) < Height)
                            {
                                val += inner[x + _x, y + _y] * gauss[_x + 1, _y + 1];
                            }
                        }
                    }

                    result[x, y] = val;
                }
            }

            return result;
        }

        #endregion

        #region Свертка
        public static double[,] Convolution(double[,] image, double[,] matrix)
        {
            int width = image.GetLength(0);
            int height = image.GetLength(1);
            int matrixWidth = matrix.GetLength(0);
            int matrixHeight = matrix.GetLength(1);
            double[,] result = new double[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    double value = 0;
                    for (int _y = - matrixHeight / 2; _y < matrixHeight / 2; _y++)
                    {
                        for (int _x = -matrixWidth / 2; _x < matrixWidth / 2; _x++)
                        {
                            if ((x + _x) >= 0 && (x + _x) < width && (y + _y) >= 0 && (y + _y) < height)
                                value += image[x + _x, y + _y] * matrix[x + _x, y + _y];
                        }
                    }

                    result[x, y] = value;
                }
            }

            return result;
        }
        #endregion


        #region Автофокус
        public static async Task<double[,]> FocusAsync(double[,] image)
        {
            return await Task.Run(() => _Focus(image));
        }

        public static double[,] Focus(double[,] image)
        {
            return _Focus(image);
        }

        private static double[,] _Focus(double[,] image)
        {
            var Width = image.GetLength(0);
            var Height = image.GetLength(1);
            int[,] lx = { { 0,  0,  0 },
                          { -1, 2, -1 },
                          { 0,  0,  0 } };
            int[,] ly = { { 0, -1, 0 },
                          { 0,  2, 0 },
                          { 0, -1, 0 } };

            int[,] lx1 = { { 0,  0, 1 },
                           { 0, -2, 0 },
                           { 1,  0, 0 } };
            int[,] lx2 = { { 1,  0, 0 },
                           { 0, -2, 0 },
                           { 0,  0, 1 } };

            double[,] result = new double[Width, Height];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    double i_lx = 0;
                    double i_ly = 0;
                    double i_lx1 = 0;
                    double i_lx2 = 0;

                    for (int _y = -1; _y < 1; _y++)
                    {
                        for (int _x = -1; _x < 1; _x++)
                        {
                            if ((x + _x) >= 0 && (x + _x) < Width && (y + _y) >= 0 && (y + _y) < Height)
                            {
                                i_lx += image[x + _x, y + _y] * lx[1 + _x, 1 + _y];
                                i_ly += image[x + _x, y + _y] * ly[1 + _x, 1 + _y];
                                i_lx1 += image[x + _x, y + _y] * 1 / Math.Sqrt(2) * lx1[1 + _x, 1 + _y];
                                i_lx2 += image[x + _x, y + _y] * 1 / Math.Sqrt(2) * lx2[1 + _x, 1 + _y];
                            }
                        }
                    }

                    result[x, y] = Math.Abs(i_lx) + Math.Abs(i_ly) + Math.Abs(i_lx1) + Math.Abs(i_lx2);
                }
            }

            return result;
        }

        public static async Task<double> FocusAsync(double[,] image, int x, int y)
        {
            return await Task.Run(() => _Focus(image, x, y));
        }

        public static double Focus(double[,] image, int x, int y)
        {
            return _Focus(image, x, y);
        }

        private static double _Focus(double[,] image, int x, int y)
        {
            var Width = image.GetLength(0);
            var Height = image.GetLength(1);
            int[,] lx = { { 0,  0,  0 },
                          { -1, 2, -1 },
                          { 0,  0,  0 } };
            int[,] ly = { { 0, -1, 0 },
                          { 0,  2, 0 },
                          { 0, -1, 0 } };

            int[,] lx1 = { { 0,  0, 1 },
                           { 0, -2, 0 },
                           { 1,  0, 0 } };
            int[,] lx2 = { { 1,  0, 0 },
                           { 0, -2, 0 },
                           { 0,  0, 1 } };

            double result = 0;

            double i_lx = 0;
            double i_ly = 0;
            double i_lx1 = 0;
            double i_lx2 = 0;

            for (int _y = -1; _y <= 1; _y++)
            {
                for (int _x = -1; _x <= 1; _x++)
                {
                    if ((x + _x) >= 0 && (x + _x) < Width && (y + _y) >= 0 && (y + _y) < Height)
                    {
                        i_lx += image[x + _x, y + _y] * lx[1 + _x, 1 + _y];
                        i_ly += image[x + _x, y + _y] * ly[1 + _x, 1 + _y];
                        i_lx1 += image[x + _x, y + _y] * 1 / Math.Sqrt(2) * lx1[1 + _x, 1 + _y];
                        i_lx2 += image[x + _x, y + _y] * 1 / Math.Sqrt(2) * lx2[1 + _x, 1 + _y];
                    }
                }
            }

            result = Math.Abs(i_lx) + Math.Abs(i_ly) + Math.Abs(i_lx1) + Math.Abs(i_lx2);

            return result;
        }
        #endregion
    }
}
