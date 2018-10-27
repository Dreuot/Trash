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
        private int[,] integral;

        public int Width => Inner.GetLength(0);
        public int Height => Inner.GetLength(1);

        public int DispX { get; set; }
        public int DispY { get; set; }

        private int[,] Inner
        {
            get
            {
                if (inner == null)
                    inner = ImageToGrayArray(image);

                return inner;
            }
        }

        private int[,] Colored
        {
            get
            {
                if (colored == null)
                    colored = ImageToColoredArray(image);

                return colored;
            }
        }

        private int[,] Integral
        {
            get
            {
                if (integral == null)
                    integral = GetIntegral(Inner);

                return integral;
            }
        }

        public ImageWrapper(Bitmap image)
        {
            this.image = image;
        }

        int this[int x, int y]
        {
            get
            {
                return Inner[x, y];
            }
            set
            {
                Inner[x, y] = value;
            }
        }

        public int[] GetBarChart()
        {
            int[] result = new int[byte.MaxValue + 1];
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    result[Inner[x, y]]++;
                }
            }

            return result;
        }

        public string GetInfo()
        {
            int[] barChart = GetBarChart();

            int levels = GetLevels(barChart);

            double averageBright = GetAverageBright();

            double contrast = GetContrast(barChart);

            double infLevel = GetInfLevel(barChart);

            double entropy = GetEnthropy(barChart);

            double disp = GetDeviation(barChart);

            double IPK = CalcIPK(averageBright, contrast, infLevel, entropy, disp);

            StringBuilder sb = new StringBuilder();
            sb.Append("Количество уровней: ");
            sb.Append(levels.ToString() + "\r\n");
            sb.Append("Количество информативных уровней: ");
            sb.Append(infLevel.ToString() + "\r\n");
            sb.Append("Контраст: ");
            sb.Append(contrast.ToString() + "\r\n");
            sb.Append("Средняя яркость: ");
            sb.Append(averageBright.ToString() + "\r\n");
            sb.Append("СКО: ");
            sb.Append(disp.ToString() + "\r\n");
            sb.Append("Энтропия: ");
            sb.Append(entropy.ToString() + "\r\n");
            sb.Append("ИПК: ");
            sb.Append(IPK.ToString() + "\r\n");
            return sb.ToString();
        }

        private static double CalcIPK(double averageBright, double contrast, double infLevel, double entropy, double disp)
        {
            return 0.33 * averageBright + 0.27 * disp + 0.2 * contrast + 0.13 * infLevel + 0.07 * entropy;
        }

        public async static Task<int[,]> GetIntegralAsync(int[,] image)
        {
            return await Task.Run(() => GetIntegral(image));
        }

        public static int[,] GetIntegral(int[,] image)
        {
            int width = image.GetLength(0);
            int height = image.GetLength(1);
            int[,] result = new int[width, height];

            Func<int, bool> CheckX = x => !(x < 0 || x >= width);
            Func<int, bool> CheckY = y => !(y < 0 || y >= height);
            Func<int, int, int> GetInteg = (x, y) => CheckX(x) && CheckY(y) ? result[x, y] : 0;
            
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    result[x, y] = image[x, y] - GetInteg(x - 1, y - 1)
                        + GetInteg(x, y - 1) + GetInteg(x - 1, y);
                }
            }

            return result;
        }
         
        private double GetDeviation(int[] barChart)
        {
            double disp = 0;
            double averageI = 0;
            int pixelCount = image.Width * image.Height;
            Func<int, double> P = (i) =>
            {
                return (double)barChart[i] / pixelCount;
            };

            for (int i = 0; i < barChart.Length; i++)
                averageI += i * P(i);

            for (int i = 0; i < barChart.Length; i++)
                disp += P(i) * Math.Pow(i - averageI, 2);

            disp = Math.Sqrt(disp);
            disp = disp <= 50 ? disp / 50 : (100 - disp) / 50;
            return disp;
        }

        private double GetInfLevel(int[] barchart)
        {
            return 1.0 * GetLevels(barchart) / 256;
        }

        private double GetEnthropy(int[] barChart)
        {
            var pixelCount = image.Width * image.Height;
            Func<int, double> P = (i) =>
            {
                return (double)barChart[i] / pixelCount;
            };

            Func<double> H = () =>
            {
                double res = 0;
                for (int i = 0; i < barChart.Length; i++)
                {
                    var p = P(i);
                    if (p != 0)
                        res += p * Math.Log(p, 2);
                }

                return res * -1;
            };
            return H() / 8;
        }

        private double GetContrast(int[] barChart)
        {
            double contrast = 0;
            int max = 0;
            int min = 0;
            for (int i = 0; i < barChart.Length; i++)
            {
                if (barChart[i] != 0)
                {
                    min = i;
                    break;
                }
            }

            for (int i = barChart.Length - 1; i > 0; i--)
            {
                if (barChart[i] != 0)
                {
                    max = i;
                    break;
                }
            }

            contrast = (double)(max - min) / byte.MaxValue;
            return contrast;
        }

        private double GetAverageBright()
        {
            int pixelCount = image.Width * image.Height; // количество пикселей
            double all = 0;
            for (int y = 0; y < image.Height; y++)
                for (int x = 0; x < image.Width; x++)
                    all += Inner[x, y];

            double averageBright = all / pixelCount;
            if (averageBright <= 107)
                averageBright = averageBright / 128;
            else if (averageBright > 147)
                averageBright = (255 - averageBright) / 128;
            else
                averageBright = 1;
            return averageBright;
        }

        private int GetLevels(int[] barChart)
        {
            int levels = 0;
            for (int i = 0; i < barChart.Length; i++)
                levels += barChart[i] == 0 ? 0 : 1;
            return levels;
        }

        private Bitmap Soebel()
        {
            double[,] Lx =
            {
                { 1, 0, -1},
                { 2, 0, -2},
                { 1, 0, -1}
            };

            double[,] Ly =
            {
                { 1, 2, 1},
                { 0, 0, 0},
                { -1, -2, -1}
            };

            var sX = TransformPixel(Inner, Lx);
            var sY = TransformPixel(Inner, Ly);

            int[,] result = new int[image.Width, image.Height];

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    int color = (int)Math.Sqrt(Math.Pow(sX[x, y], 2) + Math.Pow(sY[x, y], 2));
                    color = Normalize(color);
                    result[x, y] = 255 - color;
                }
            }

            return GrayArrayToImage(result);
        }

        private static int Normalize(int color)
        {
            return color > 255 ? 255 : (color < 0 ? 0 : color);
        }

        public async Task<Bitmap> SoebelAsync()
        {
            return await Task.Run(() => Soebel());
        }

        private bool IsBound(int x, int y, int dX, int dY)
        {
            return ((x + dX > 0) && (x + dX < image.Width) && (y + dY > 0) && (y + dY < image.Height));
        }

        private int[,] TransformPixel(int[,] image, double[,] mask)
        {
            int width = image.GetLength(0);
            int height = image.GetLength(1);
            int dimX = mask.GetLength(0);
            int dimY = mask.GetLength(1);
            int[,] result = new int[width, height];
            for (int x = 0; x < width; x++)
            {
                int color = 0;
                for (int y = 0; y < height; y++)
                {
                    color = 0;
                    for (int i = 0; i < dimX; i++)
                    {
                        for (int j = 0; j < dimY; j++)
                        {
                            int maskX = (dimX / 2) + i;
                            int maskY = (dimY / 2) + j;
                            if (IsBound(x, y, maskX, maskY))
                               color += (int)(image[x + maskX, y + maskY] * mask[i, j]);
                        }
                    }

                    result[x, y] = Normalize(color);
                }
            }

            return result;
        }

        public Bitmap GetImage()
        {
            return image;
        }

        public async Task<Bitmap> ToGrayScaleAsync()
        {
            return await Task.Run(() => ToGrayScale());
        }

        public Bitmap ToGrayScale()
        {
            return GrayArrayToImage(Inner);
        }

        public static async Task<int[,]> ImageToGrayArrayAsync(Bitmap image)
        {
            return await Task.Run(() => ImageToGrayArray(image));
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

        public static async Task<int[,]> ImageToColoredArrayAsync(Bitmap image)
        {
            return await Task.Run(() => ImageToColoredArray(image));
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

        int[,] sqrInner;
        public int[,] SqrInner
        {
            get
            {
                if (sqrInner == null)
                    sqrInner = CreateGraySqr();

                return sqrInner;
            }
        }

        int[,] sqrIntegral;
        public int[,] SqrIntegral
        {
            get
            {
                if (sqrIntegral == null)
                    sqrIntegral = GetIntegral(SqrInner);

                return sqrIntegral;
            }
        }

        private int[,] CreateGraySqr()
        {
            int[,] result = new int[Width, Height];
            for (int _y = 0; _y < Height; _y++)
            {
                for (int _x = 0; _x < Width; _x++)
                {
                    result[_x, _y] = Inner[_x, _y] * Inner[_x, _y];
                }
            }

            return result;
        }

        public double GetDispersion(int x, int y, int width, int height)
        {
            int max_x = x + width >= Width ? Width : x + width - 1;
            int max_y = y + height >= Height ? Height - 1: y + height;

            int area = (max_x - x) * (max_y - y);

            double sumSqrOfRect = SqrIntegral[x, y] + SqrIntegral[max_x, max_y]
                - SqrIntegral[x, max_y] - SqrIntegral[max_x, y];
            double sumOfRect = Integral[x, y] + Integral[max_x, max_y]
                - Integral[x, max_y] - Integral[max_x, y];

            double sqare = sumOfRect * sumOfRect;

            return (sumSqrOfRect - sqare);
        }

        private int Center_x => Width / 2;
        private int Center_y => Height / 2;

        private int ConvertX(int x)
        {
            return x - Center_x;
        }

        private int ConvertY(int y)
        {
            return y - Center_y;
        }

        private double Radius(int x, int y)
        {
            return Math.Sqrt(x * x + y * y);
        }

        public async Task<int[,]> DistorsionAsync(double d)
        {
            return await Task.Run(() => Distorsion(d));
        }

        public int[,] Distorsion(double d)
        {
            int[,] result = new int[Width, Height];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    result[x, y] = 0;
                    int r_x = ConvertX(x);
                    int r_y = ConvertY(y);
                    double radius = Radius(r_x, r_y);
                    double cos = r == 0 ? 1 : r_x / radius;
                    double sin = r == 0 ? 0 : r_x / radius;

                    double distorsedR = radius * (1 + d * radius * radius);

                    int d_x = (int)(distorsedR * cos + r_x);
                    int d_y = (int)(distorsedR * sin + r_y);
                    if(!(d_x < 0 || d_x >= Width || d_y < 0 || d_y >= Height))
                        result[d_x + Center_x, d_y + Center_y] = Inner[x, y];
                }
            }

            return result;
        }

        public async Task<Bitmap> ContrastAsync()
        {
            return await Task.Run(() => Contrast());
        }

        public Bitmap Contrast()
        {
            int[,] result = new int[Width, Height];
            var barChart = GetBarChart();
            int L = 0;
            int R = 0;
            for (int i = 0; i < barChart.Length; i++)
            {
                if (barChart[i] > 0)
                {
                    L = i;
                    break;
                }
            }

            for (int i = barChart.Length - 1; i >= 0; i--)
            {
                if (barChart[i] > 0)
                {
                    R = i;
                    break;
                }
            }

            double d = byte.MaxValue / (R - L);
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    result[x, y] = (int)((Inner[x, y] - L) * d);

            return GrayArrayToImage(result);
        }
    }
}
