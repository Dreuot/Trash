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
            int pixelCount = image.Width * image.Height; // количество пикселей

            int levels = 0;
            for (int i = 0; i < barChart.Length; i++)
                levels += barChart[i] == 0 ? 0 : 1;

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

            double contrast = 0;
            int max = 0;
            int min = 0;
            for (int i = 0; i < barChart.Length; i++)
            {
                if(barChart[i] != 0)
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

            double infLevel = 1.0 * levels / 256;

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
                    if(p != 0)
                        res += p * Math.Log(p, 2);
                }

                return res * -1;
            };

            double entropy = H() / 8;

            double disp = 0;
            double averageI = 0;
            for (int i = 0; i < barChart.Length; i++)
                averageI += i * P(i);

            for (int i = 0; i < barChart.Length; i++)
                disp += P(i) * Math.Pow(i - averageI, 2);

            disp = Math.Sqrt(disp);
            disp = disp <= 50 ? disp / 50 : (100 - disp) / 50;

            double IPK = 0.33 * averageBright + 0.27 * disp + 0.2 * contrast + 0.13 * infLevel + 0.07 * entropy;

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

        public Bitmap GetImage()
        {
            return image;
        }

        public Task<Bitmap> ToGrayScaleAsync()
        {
            return Task.Run(() => ToGrayScale());
        }

        public Bitmap ToGrayScale()
        {
            return GrayArrayToImage(inner);
        }

        public static Task<int[,]> ImageToGrayArrayAsync(Bitmap image)
        {
            return Task.Run(() => ImageToGrayArray(image));
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

        public static Task<int[,]> ImageToColoredArrayAsync(Bitmap image)
        {
            return Task.Run(() => ImageToColoredArray(image));
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
