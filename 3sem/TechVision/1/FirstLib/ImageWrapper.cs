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
        private Bitmap image;
        private int[,] inner;
        private RGB[,] colored;
        private int[,] integral;

        public int Width => Inner.GetLength(0);
        public int Height => Inner.GetLength(1);

        public int DispX { get; set; }
        public int DispY { get; set; }

        public int[,] Inner
        {
            get
            {
                if (inner == null)
                    inner = ImageToGrayArray(image);

                return inner;
            }
        }

        public RGB[,] Colored
        {
            get
            {
                if (colored == null)
                    colored = ImageToRGB(image);

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

        public Bitmap GetImage()
        {
            return image;
        }

        #region Информация об изображении
        public int[] GetBarChart()
        {
            return GetBarChart(Inner);
        }

        public int[] GetBarChart(int[,] image)
        {
            int width = image.GetLength(0);
            int height = image.GetLength(1);
            int[] result = new int[byte.MaxValue + 1];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    result[image[x, y]]++;
                }
            }

            return result;
        }

        public string GetInfo()
        {
            int[] barChart = GetBarChart(Inner);

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
        #endregion

        #region Собель
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
        #endregion

        #region Преобразование в разные форматы
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

        public static async Task<RGB[,]> ImageToRGBAsync(Bitmap image)
        {
            return await Task.Run(() => ImageToRGB(image));
        }

        public static RGB[,] ImageToRGB(Bitmap image)
        {
            RGB[,] result = new RGB[image.Width, image.Height];
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    var pixel = image.GetPixel(x, y);
                    result[x, y] = new RGB(pixel.R, pixel.G, pixel.B);
                }
            }

            return result;
        }

        public async Task<YIQ[,]> RgbToYiqAsync(RGB[,] image)
        {
            return await Task.Run(() => RgbToYiq(image));
        }

        public YIQ[,] RgbToYiq(RGB[,] image)
        {
            int width = image.GetLength(0);
            int height = image.GetLength(1);
            YIQ[,] result = new YIQ[width, height];
            Matrix m = new Matrix(3, 3);
            m.Init(0.299, 0.587, 0.114, 0.596, -0.274, -0.321, 0.211, -0.526, 0.311);
            Matrix rbg = new Matrix(3, 1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    rbg.Init(image[x, y].R, image[x, y].G, image[x, y].B);
                    Matrix yic = m * rbg;
                    result[x, y] = new YIQ(yic[0, 0], yic[1, 0], yic[2, 0]);
                }
            }

            return result;
        }

        public async Task<RGB[,]> YiqToRgbAsync(YIQ[,] image)
        {
            return await Task.Run(() => YiqToRgb(image));
        }

        public RGB[,] YiqToRgb(YIQ[,] image)
        {
            int width = image.GetLength(0);
            int height = image.GetLength(1);
            RGB[,] result = new RGB[width, height];
            Matrix m = new Matrix(3, 3);
            m.Init(1, 0.956, 0.623, 1, -0.272, -0.648, 1, -1.105, 1.705);
            Matrix yic = new Matrix(3, 1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    yic.Init(image[x, y].Y, image[x, y].Ic, image[x, y].Qc);
                    Matrix rbg = m * yic;
                    result[x, y] = new RGB(Normalize((int)rbg[0, 0]), Normalize((int)rbg[1, 0]), Normalize((int)rbg[2, 0]));
                }
            }

            return result;
        }

        public async Task<int[,]> YiqToGrayAsync(YIQ[,] image)
        {
            return await Task.Run(() => YiqToGray(image));
        }

        public int[,] YiqToGray(YIQ[,] image)
        {
            int width = image.GetLength(0);
            int height = image.GetLength(1);
            int[,] result = new int[width, height];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    result[x, y] = Normalize((int)image[x, y].Y);

            return result;
        }

        public static async Task<Bitmap> RGBToImageAsync(RGB[,] image)
        {
            return await Task.Run(() => RGBToImage(image));
        }

        public static Bitmap RGBToImage(RGB[,] image)
        {
            int width = image.GetLength(0);
            int height = image.GetLength(1);
            Bitmap result = new Bitmap(width, height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    result.SetPixel(x, y, Color.FromArgb(image[x, y].R, image[x, y].G, image[x, y].B));
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
        #endregion

        #region Дисперсия в окне
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

            int elements = (max_x - x) * (max_y - y);
            sumOfRect = sumOfRect / elements;

            return (sumSqrOfRect / elements - sumOfRect * sumOfRect);
        }
        #endregion

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
            int[,] result = new int[Width, Height];
            double diagonal = Math.Sqrt(Width * Width + Height * Height) / 2;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    result[x, y] = 0;
                    int u = x - Width / 2;
                    int v = y - Height / 2;
                    int r = (int)Math.Sqrt(u * u + v * v);
                    double g = Math.Cos(r * Math.PI * L / (2 * diagonal));

                    int xi = (int)(u * g + Width / 2);
                    int yi = (int)(v * g + Height / 2);
                    // double radius = Radius(u, v);
                    //double cos = Math.Cos(radius * Math.PI * L / (2 * h))
                    //double cos = radius == 0 ? 1 : (u / radius);
                    //double sin = radius == 0 ? 0 : (y / radius);
                    //double distorsed = radius * (1 + L * radius * radius);

                    //int xi = Normalize((int)(distorsed * cos + Width / 2));
                    //int yj = Normalize((int)(distorsed * sin + Height / 2));
                    result[x, y] = Inner[xi, yi];
                }
            }

            return GrayArrayToImage(result);
        }
        #endregion

        #region Контрастирование
        public async Task<Bitmap> LinearContrastAsync()
        {
            return await Task.Run(() => LinearContrast());
        }

        public async Task<int[,]> LinearContrastAsync(int[,] image)
        {
            return await Task.Run(() => _LinearContrast(image));
        }

        public Bitmap LinearContrast()
        {
            return LinearContrast(Inner);
        }

        public Bitmap LinearContrast(int[,] image)
        {
            return GrayArrayToImage(_LinearContrast(image));
        }

        private int[,] _LinearContrast(int[,] image)
        {
            int[,] result = new int[Width, Height];
            var barChart = GetBarChart(image);
            double fivePercent = Width * Height * 0.05;
            int L = 0;
            int R = 0;
            int sum = 0;
            for (int i = 0; i < barChart.Length; i++)
            {
                sum += barChart[i];
                if (sum > fivePercent)
                {
                    L = i;
                    break;
                }
            }

            sum = 0;
            for (int i = barChart.Length - 1; i >= 0; i--)
            {
                sum += barChart[i];
                if (sum > fivePercent)
                {
                    R = i;
                    break;
                }
            }

            double d = byte.MaxValue / (R - L);
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    result[x, y] = Normalize((int)((image[x, y] - L) * d));

            return result;
        }

        public async Task<Bitmap> EqualizContrastAsync()
        {
            return await Task.Run(() => EqualizContrast());
        }

        public async Task<int[,]> EqualizContrastAsync(int[,] image)
        {
            return await Task.Run(() => _EqualizContrast(image));
        }

        public Bitmap EqualizContrast()
        {
            return GrayArrayToImage(_EqualizContrast(Inner));
        }

        public int[,] _EqualizContrast(int[,] image)
        {
            int width = image.GetLength(0);
            int height = image.GetLength(1);
            int[,] result = new int[width, height];

            int[] barChart = GetBarChart(image);
            double[] p = new double[256];
            for (int i = 0; i < 256; i++)
                p[i] = (double)barChart[i] / (width * height);

            for (int i = 1; i < 256; i++)
                p[i] = p[i - 1] + p[i];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    result[x, y] = Normalize((int)(255 * p[image[x, y]]));
                }
            }

            return result;
        }

        public async Task<Bitmap> LinearContrastYIQ()
        {
            var yiq = await RgbToYiqAsync(Colored);
            var gray = await YiqToGrayAsync(yiq);
            var contrasted = await LinearContrastAsync(gray);
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    yiq[x, y].Y = contrasted[x, y];

            return await RGBToImageAsync(await YiqToRgbAsync(yiq));
        }

        public async Task<Bitmap> EqualizContrastYIQ()
        {
            var yiq = await RgbToYiqAsync(Colored);
            var gray = await YiqToGrayAsync(yiq);
            var contrasted = await EqualizContrastAsync(gray);
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    yiq[x, y].Y = contrasted[x, y];

            return await RGBToImageAsync(await YiqToRgbAsync(yiq));
        }
        #endregion

        #region Комплексирование
        public async Task<Bitmap> ComplexAsync(Bitmap teleImg, Bitmap teploImg)
        {
            return await Task.Run(() => Complex(teleImg, teploImg));
        }

        public Bitmap Complex(Bitmap teleImg, Bitmap teploImg)
        {
            var TV = ImageToGrayArray(teleImg);
            var IR = ImageToGrayArray(teploImg);

            int l = teleImg.Width;
            int h = teleImg.Height;
            double _IR = 0;
            for (int y = 0; y < h; y++)
                for (int x = 0; x < l; x++)
                    _IR += IR[x, y];

            _IR /= l * h;
            double _dIR = 0;
            for (int y = 0; y < h; y++)
                for (int x = 0; x < l; x++)
                    _dIR += IR[x, y] - _IR;

            _dIR /= l * h;

            int[,] result = new int[l, h];
            for (int y = 0; y < h; y++)
                for (int x = 0; x < l; x++)
                    result[x, y] = Normalize((int)(TV[x, y] + Math.Abs(IR[x, y] - _IR) - _dIR));

            return GrayArrayToImage(result);
        }

        #endregion

        #region Кластеризация
        public async Task<Bitmap> ClusterAsync(int count, int iteration)
        {
            return await Task.Run(() => Cluster(count, iteration));
        }

        public Bitmap Cluster(int count, int iteration)
        {
            int pixelsCount = Width * Height;
            Cluster[] cluster = new Cluster[pixelsCount];
            int[] centers = new int[count];
            int step = 255 / count;
            int current = step / 2;
            for (int i = 0; i < count; i++, current += step)
                centers[i] = current;

            Func<int, int, int> Distance = (x, y) =>
            {
                return Math.Abs(y - x);
            };

            Func<int, int> Assign = (bright) => {
                int min = 255;
                int num = 0;
                for (int i = 0; i < count; i++)
                {
                    var distance = Distance(bright, centers[i]);
                    if(distance < min)
                    {
                        min = distance;
                        num = i;
                    }
                }

                return num;
            };

            // Привязка точек начальным кластерам
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int num = Assign(Inner[x, y]);
                    cluster[y * Width + x] = new FirstLib.Cluster(x, y, num);
                }
            }

            for (int j = 0; j < iteration; j++)
            {
                for (int i = 0; i < count; i++)
                {
                    var currentCluster = cluster.Where(c => c.Num == i).ToList();
                    int sum = 0;
                    foreach (var item in currentCluster)
                        sum += Inner[item.X, item.Y];

                    int average = 0;
                    if (currentCluster.Count() != 0)
                        average = sum / currentCluster.Count();
                    centers[i] = average;
                }

                for (int i = 0; i < pixelsCount; i++)
                {
                    cluster[i].Num = Assign(Inner[cluster[i].X, cluster[i].Y]);
                }
            }

            //Color[] colors = new Color[]
            //{
            //    Color.FromArgb(63,187,102),
            //    Color.FromArgb(111,204,141),
            //    Color.FromArgb(160,222,179),
            //    Color.FromArgb(207,238,217),
            //    Color.FromArgb(236,249,240),
            //    Color.FromArgb(54,159,87),
            //    Color.FromArgb(47,140,77),
            //    Color.FromArgb(32,94,51),
            //    Color.FromArgb(16,47,26),
            //    Color.FromArgb( 6,19,10)
            //};

            Color[] colors = new Color[]
            {
                Color.FromArgb(0,154,176),
                Color.FromArgb(207,188,166),
                Color.FromArgb(0,0,1),
                Color.FromArgb(130,239,238),
                Color.FromArgb(18,138,8),
                Color.FromArgb(77,0,1),
                Color.FromArgb(162,4,23),
                Color.FromArgb(237,66,85),
                Color.FromArgb(0,113,68),
                Color.FromArgb(0,64,25)
            };

            Bitmap result = (Bitmap)this.image.Clone();
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    result.SetPixel(x, y, colors[cluster[y * Width + x].Num]);
                }
            }

            return result;
        }
        #endregion
    }
}
