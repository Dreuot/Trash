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

        public int[] GetBarChart()
        {
            int[] result = new int[byte.MaxValue + 1];
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    result[inner[x, y]]++;
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
                    all += inner[x, y];

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

            var sX = TransformPixel(inner, Lx);
            var sY = TransformPixel(inner, Ly);

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
            return GrayArrayToImage(inner);
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
    }
}
