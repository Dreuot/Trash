using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTransformer
{
    public class Transformer
    {
        private Bitmap grayImage = null;

        public Bitmap Image { get; private set; }
        public Bitmap GrayImage
        {
            get
            {
                if(grayImage == null)
                    grayImage = _TransformToGrayScale(Image);

                return grayImage;
            }
        }

        public Bitmap LastOpearationResult { get; private set; }

        public int Width => Image.Width;
        public int Height => Image.Height;


        public Transformer(string fileName)
        {
            Image = new Bitmap(fileName);
            Reset();
        }

        public Transformer(Bitmap image)
        {
            Image = image;
            Reset();
        }

        public Transformer(Stream stream)
        {
            Image = new Bitmap(stream);
            Reset();
        }

        public Transformer(Image image)
        {
            Image = new Bitmap(image);
            Reset();
        }

        private Bitmap _TransformToGrayScale(Bitmap image)
        {
            Bitmap result = CloneImage(image);
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Color current = result.GetPixel(x, y);
                    int grayscale = (int)((current.R) * 0.3 + (current.G * 0.59) + (current.B * 0.11));
                    result.SetPixel(x, y, Color.FromArgb(current.A, grayscale, grayscale, grayscale));
                }
            }

            LastOpearationResult = result;
            return result;
        }

        public async Task<Bitmap> TransformToGrayScale(Bitmap image)
        {
            return await Task.Run(() => _TransformToGrayScale(image));
        }

        public async Task<Bitmap> TransformToGrayScale()
        {
            return await Task.Run(() => _TransformToGrayScale(LastOpearationResult));
        }

        public async Task<Bitmap> TransformToBinary(Bitmap image)
        {
            return await Task.Run(() => _TransformToBinary(image));
        }

        public async Task<Bitmap> TransformToBinary()
        {
            return await Task.Run(() => _TransformToBinary(LastOpearationResult));
        }

        private Bitmap _TransformToBinary(Bitmap image)
        {
            var result = CloneImage(image);
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int color = result.GetPixel(x, y).R;
                    color = color > 128 ? 255 : 0;
                    result.SetPixel(x, y, Color.FromArgb(255, color, color, color));
                }
            }

            LastOpearationResult = result;
            return result;
        }

        public void Reset()
        {
            LastOpearationResult = Image;
        }

        public static Bitmap Homography(Bitmap first, Bitmap second, Matrix H)
        {
            var result = new Bitmap(first.Width, first.Height);
            for (int x = 0; x < second.Width; x++)
            {
                for (int y = 0; y < second.Height; y++)
                {
                    double z = H[2, 0] * x + H[2, 1] * y + 1;
                    double x1 = (H[0, 0] * x + H[0, 1] * y + H[0, 2]) / z;
                    double y1 = (H[1, 0] * x + H[1, 1] * y + H[1, 2]) / z;

                    if(x1 < result.Width && x1 > 0 & y1 > 0 && y1 < result.Height)
                        result.SetPixel((int)x1, (int)y1, second.GetPixel(x, y));
                }
            }

            return result;
        }

        public async Task<Bitmap> HomographyAsync(Bitmap second, Point[] firstPoints, Point[] secondPoints)
        {
            return await Task.Run(() => _Homography(LastOpearationResult, second, firstPoints, secondPoints));
        }

        public async Task<Bitmap> HomographyAsync(Bitmap first, Bitmap second, Point[] firstPoints, Point[] secondPoints)
        {
            return await Task.Run(() => _Homography(first, second, firstPoints, secondPoints));
        }

        private Bitmap _Homography(Bitmap first, Bitmap second, Point[] firstPoints, Point[] secondPoints)
        {
            if (firstPoints.Length != 4 || secondPoints.Length != 4)
                throw new ArgumentException("Выберите по 4 точки на каждом изображении");

            Matrix A = new Matrix(8, 8);
            A.Init(secondPoints[0].X, secondPoints[0].Y, 1, 0, 0, 0, -firstPoints[0].X * secondPoints[0].X, -firstPoints[0].X * secondPoints[0].Y,
                    secondPoints[1].X, secondPoints[1].Y, 1, 0, 0, 0, -firstPoints[1].X * secondPoints[1].X, -firstPoints[1].X * secondPoints[1].Y,
                    secondPoints[2].X, secondPoints[2].Y, 1, 0, 0, 0, -firstPoints[2].X * secondPoints[1].X, -firstPoints[2].X * secondPoints[2].Y,
                    secondPoints[3].X, secondPoints[3].Y, 1, 0, 0, 0, -firstPoints[3].X * secondPoints[1].X, -firstPoints[3].X * secondPoints[3].Y,
                    0, 0, 0, secondPoints[0].X, secondPoints[0].Y, 1, -firstPoints[0].Y * secondPoints[0].X, -firstPoints[0].Y * secondPoints[0].Y,
                    0, 0, 0, secondPoints[1].X, secondPoints[1].Y, 1, -firstPoints[1].Y * secondPoints[1].X, -firstPoints[1].Y * secondPoints[1].Y,
                    0, 0, 0, secondPoints[2].X, secondPoints[2].Y, 1, -firstPoints[2].Y * secondPoints[2].X, -firstPoints[2].Y * secondPoints[2].Y,
                    0, 0, 0, secondPoints[3].X, secondPoints[3].Y, 1, -firstPoints[3].Y * secondPoints[3].X, -firstPoints[3].Y * secondPoints[3].Y
                );

            Matrix B = new Matrix(8, 1);
            B.Init(firstPoints[0].X, firstPoints[1].X, firstPoints[2].X, firstPoints[3].X,
                    firstPoints[0].Y, firstPoints[1].Y, firstPoints[2].Y, firstPoints[3].Y);

            Matrix G = A.Gauss(B);
            List<double> h = G.ToArray().ToList();
            h.Add(1);
            Matrix H = new Matrix(3, 3);
            H.Init(h.ToArray());
            Bitmap transformed = new Bitmap(first.Width, first.Height);
            for (int x = 0; x < second.Width; x++)
            {
                for (int y = 0; y < second.Height; y++)
                {
                    double z = H[2, 0] * x + H[2, 1] * y + 1;
                    double x1 = (H[0, 0] * x + H[0, 1] * y + H[0, 2]) / z;
                    double y1 = (H[1, 0] * x + H[1, 1] * y + H[1, 2]) / z;

                    if (x1 < transformed.Width && x1 > 0 & y1 > 0 && y1 < transformed.Height)
                        transformed.SetPixel((int)x1, (int)y1, second.GetPixel(x, y));
                }
            }

            Bitmap result = (Bitmap)first.Clone();
            Graphics g = Graphics.FromImage(result);
            g.DrawImage(transformed, 0, 0);

            return result;
        }

        public static async Task<Bitmap> HomographyAsync(Bitmap first, Bitmap second, Matrix H)
        {
            return await Task.Run(() => Homography(first, second, H));
        }

        #region Афинные
        public async Task<Bitmap> Round(Bitmap image, double angle)
        {
            return await Task.Run(() => _Round(image, angle));
        }

        public async Task<Bitmap> Round(double angle)
        {
            return await Task.Run(() => _Round(LastOpearationResult, angle));
        }

        private Bitmap _Round(Bitmap image, double angle)
        {
            var result = new Bitmap(Width * 2, Height * 2);
            Matrix m = new Matrix(2, 2);
            m.Init(Math.Cos(angle), -Math.Sin(angle), Math.Sin(angle), Math.Cos(angle));
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var pixel = image.GetPixel(x, y);
                    var xy = new Matrix(2, 1);
                    xy.Init(x - (Width / 2), y - (Height / 2));
                    var coordinate = m * xy;
                    if (coordinate[0, 0] + Width < result.Width && coordinate[0, 0] + Width > 0 & coordinate[1, 0] + Height > 0 && coordinate[1, 0] + Height < result.Height)
                        result.SetPixel((int)coordinate[0, 0] + Width, (int)coordinate[1, 0] + Height, pixel);

                }
            }

            LastOpearationResult = result;
            return result;
        }

        public async Task<Bitmap> Move(Bitmap image, Matrix vector)
        {
            return await Task.Run(() => _Move(image, vector));
        }

        public async Task<Bitmap> Move(Matrix vector)
        {
            return await Task.Run(() => _Move(LastOpearationResult, vector));
        }

        private Bitmap _Move(Bitmap image, Matrix vector)
        {
            var result = new Bitmap(Width, Height);
            if (vector.Rows != 2 && vector.Columns != 1)
                throw new ArgumentException($"Размерность матрицы-вектора должна быть 2 x 1");

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var pixel = image.GetPixel(x, y);
                    var xy = new Matrix(2, 1);
                    xy.Init(x, y);
                    var coordinate = xy + vector;
                    if (coordinate[0, 0] < result.Width && coordinate[0, 0] > 0 & coordinate[1, 0] > 0 && coordinate[1, 0] < result.Height)
                        result.SetPixel((int)coordinate[0, 0], (int)coordinate[1, 0], pixel);
                }
            }

            LastOpearationResult = result;
            return result;
        }

        public async Task<Bitmap> Stretch(Bitmap image, double x_mult, double y_mult)
        {
            return await Task.Run(() => _Stretch(image, x_mult, y_mult));
        }

        public async Task<Bitmap> Stretch(double x_mult, double y_mult)
        {
            return await Task.Run(() => _Stretch(LastOpearationResult, x_mult, y_mult));
        }

        private Bitmap _Stretch(Bitmap image, double x_mult, double y_mult)
        {
            var result = new Bitmap((int)(Width * x_mult), (int)(Height * y_mult));
            Matrix m = new Matrix(2, 2);
            m.Init(x_mult, 0, 0, y_mult);
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var pixel = image.GetPixel(x, y);
                    var xy = new Matrix(2, 1);
                    xy.Init(x, y);
                    var coordinate = m * xy;
                    if (coordinate[0, 0] < result.Width && coordinate[0, 0] > 0 & coordinate[1, 0] > 0 && coordinate[1, 0] < result.Height)
                    {
                        int x1 = (int)Math.Ceiling(coordinate[0, 0]);
                        int x2 = (int)Math.Floor(coordinate[0, 0]);
                        int y1 = (int)Math.Ceiling(coordinate[1, 0]);
                        int y2 = (int)Math.Floor(coordinate[1, 0]);
                        //result.SetPixel((int)coordinate[0, 0], (int)coordinate[1, 0], pixel);
                        if (x1 < result.Width && x1 > 0 && y1 < result.Height && y1 > 0)
                            result.SetPixel(x1, y1, pixel);
                        if (x2 < result.Width && x2 > 0 && y2 < result.Height && y2 > 0)
                            result.SetPixel(x2, y2, pixel);
                        if (x1 < result.Width && x1 > 0 && y2 < result.Height && y2 > 0)
                            result.SetPixel(x1, y2, pixel);
                        if (x2 < result.Width && x2 > 0 && y1 < result.Height && y1 > 0)
                            result.SetPixel(x2, y1, pixel);
                    }
                }
            }

            LastOpearationResult = result;
            return result;
        }

        public async Task<Bitmap> Mirror(Bitmap image, Direction dir)
        {
            return await Task.Run(() => _Mirror(image, dir));
        }

        public async Task<Bitmap> Mirror(Direction dir)
        {
            return await Task.Run(() => _Mirror(LastOpearationResult, dir));
        }

        private Bitmap _Mirror(Bitmap image, Direction dir)
        {
            var result = new Bitmap(Width, Height);
            Matrix m = new Matrix(2, 2);
            switch (dir)
            {
                case Direction.Vertical:
                    m.Init(1, 0, 0, -1);
                    break;
                case Direction.Horisontal:
                    m.Init(-1, 0, 0, 1);
                    break;
            }
            
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var pixel = image.GetPixel(x, y);
                    var xy = new Matrix(2, 1);
                    xy.Init(x - (Width / 2), y - (Height / 2));
                    var coordinate = m * xy;
                    if (coordinate[0, 0] + (Width / 2) < result.Width && coordinate[0, 0] + (Width / 2) > 0 & coordinate[1, 0] + (Height / 2) > 0 && coordinate[1, 0] + (Height / 2) < result.Height)
                        result.SetPixel((int)coordinate[0, 0] + (Width / 2), (int)coordinate[1, 0] + (Height / 2), pixel);
                }
            }

            LastOpearationResult = result;
            return result;
        }
#endregion

        #region Морфологические операции

        public async Task<Bitmap> Dilatation(Bitmap image)
        {
            int[,] mask = {
                { 0, 1, 0},
                { 1, 1, 1},
                { 0, 1, 0}
            };
            return await Task.Run(() => _Dilatation(image, mask, 3));
        }

        public async Task<Bitmap> Dilatation()
        {
            int[,] mask = {
                { 0, 1, 0},
                { 1, 1, 1},
                { 0, 1, 0}
            };
            return await Task.Run(() => _Dilatation(LastOpearationResult, mask, 3));
        }

        private Bitmap _Dilatation(Bitmap image, int[,] mask, int dim)
        {
            var result = _TransformToBinary(image);
            int one = 255;

            for (int x = 0; x < Width; x++)
            { 
                for (int y = 0; y < Height; y++)
                { 
                    for (int i = 0; i < dim; i++)
                        for (int j = 0; j < dim; j++)
                            if ((x - (dim / 2) + j > 0 && x - (dim / 2) + j < Width) && (y - (dim / 2) + i > 0 && y - (dim / 2) + i < Height))
                            {
                                int center = image.GetPixel(x, y).R;
                                if(center == 0 && mask[i, j] == 1)
                                    result.SetPixel(x - (dim / 2) + j, y - (dim / 2) + i, Color.FromArgb(255, one, one, one));
                            }
                }
            }

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    bool all = true;
                    for (int i = 0; i < dim; i++)
                        for (int j = 0; j < dim; j++)
                            if ((x - (dim / 2) + j > 0 && x - (dim / 2) + j < Width) && (y - (dim / 2) + i > 0 && y - (dim / 2) + i < Height))
                                if (mask[i, j] == 1 && image.GetPixel(x - (dim / 2) + j, y - (dim / 2) + i).R != 0)
                                    all = false;

                    if (all)
                    {
                        result.SetPixel(x, y, Color.FromArgb(255, 0, 0, 0));
                    }
                }
            }

            LastOpearationResult = result;
            return result;
        }

        public async Task<Bitmap> Erosion(Bitmap image)
        {
            int[,] mask = {
                { 0, 1, 0},
                { 1, 1, 1},
                { 0, 1, 0}
            };
            return await Task.Run(() => _Erosion(image, mask, 3));
        }

        public async Task<Bitmap> Erosion()
        {
            int[,] mask = {
                { 0, 1, 0},
                { 1, 1, 1},
                { 0, 1, 0}
            };
            return await Task.Run(() => _Erosion(LastOpearationResult, mask, 3));
        }

        private Bitmap _Erosion(Bitmap image, int[,] mask, int dim)
        {
            var result = CloneImage(image);
            int zero = 0;

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int i = 0; i < dim; i++)
                    {
                        for (int j = 0; j < dim; j++)
                        {
                            if ((x - (dim / 2) + j > 0 && x - (dim / 2) + j < Width) && (y - (dim / 2) + i > 0 && y - (dim / 2) + i < Height))
                            {
                                int center = image.GetPixel(x, y).R;
                                if (center == 0 && mask[i, j] == 1)
                                {
                                    result.SetPixel(x - (dim / 2) + j, y - (dim / 2) + i, Color.FromArgb(255, zero, zero, zero));
                                }
                            }
                        }
                    }
                }
            }

            LastOpearationResult = result;
            return result;
        }
        #endregion

        #region Фильтрация
        public Bitmap SaltAndPepper(double intensive)
        {
            Bitmap result = CloneImage(GrayImage);
            return _SaltAndPepper(intensive, result);
        }

        public Bitmap SaltAndPepper(Bitmap image, double intensive)
        {
            Bitmap result = CloneImage(image);
            return _SaltAndPepper(intensive, result);
        }

        private Bitmap _SaltAndPepper(double intensive, Bitmap result)
        {
            Random r = new Random();
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    bool transform = (r.Next(1, 101) < intensive * 100) ? true : false;
                    if (transform)
                    {
                        int color = (r.Next(0, 2) == 0) ? 0 : 255;
                        result.SetPixel(x, y, Color.FromArgb(255, color, color, color));
                    }
                }
            }

            LastOpearationResult = result;
            return result;
        }

        public async Task<Bitmap> Gause(double m, double d)
        {
            var result = CloneImage(GrayImage);
            await Task.Run(() =>
            {
                Random r = new Random();
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        int c = result.GetPixel(x, y).R;
                        double rand = 0;
                        for (int i = 1; i < 256; i++)
                            rand += r.Next(0, 2);

                        rand -= 128;
                        rand *= (m + d);
                        c += (int)rand;
                        c = Normalize(c);

                        result.SetPixel(x, y, Color.FromArgb(225, c, c, c));
                    }
                }
            });
            

            LastOpearationResult = result;
            return result;
        }

        private static int Normalize(int c)
        {
            c = c < 0 ? 0 : c;
            c = c > 255 ? 225 : c;
            return c;
        }

        public async Task<Bitmap> GauseFilter(Bitmap image)
        {
            Bitmap result = CloneImage(image);
            return await Task.Run(() => _GauseFilter(result));
        }

        public async Task<Bitmap> GauseFilter()
        {
            Bitmap result = CloneImage(LastOpearationResult);
            return await Task.Run(() => _GauseFilter(result));
        }

        private Bitmap _GauseFilter(Bitmap image)
        {
            Bitmap result = CloneImage(image);
            double[,] filter = {
                { 0.0509, 0.1238, 0.0509},
                { 0.1238, 0.3012, 0.1238},
                { 0.0509, 0.1238, 0.0509}
            };

            double color = 0;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    color = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if ((x - 1 + j > 0 && x - 1 + j < Width) &&(y - 1 + i > 0 && y - 1 + i < Height))
                                color += filter[i, j] * image.GetPixel(x - 1 + j, y - 1 + i).R;
                        }
                    }

                    int c = (int)color;
                    c = Normalize(c);
                    result.SetPixel(x, y, Color.FromArgb(225, c, c, c));
                }
            }

            LastOpearationResult = result;
            return result;
        }

        public async Task<Bitmap> MedianAsync(int size)
        {
            return await Task.Run(() => _Median(LastOpearationResult, size));
        }

        public async Task<Bitmap> MedianAsync(Bitmap image, int size)
        {
            return await Task.Run(() => _Median(image, size));
        }

        private Bitmap _Median(Bitmap image, int size)
        {
            Bitmap result = CloneImage(image);
            int[,] kerner = CreateFilter(size);
            int xCen = size / 2;
            int yCen = size / 2;
            int[] pixels = new int[size * size];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            if ((x - xCen + j > 0 && x - xCen + j < Width) && (y - yCen + i > 0 && y - yCen + i < Height))
                                pixels[i * size + j] = kerner[i, j] * image.GetPixel(x - xCen + j, y - yCen + i).R;
                        }
                    }

                    Array.Sort(pixels);
                    int color = pixels[size * size / 2];
                    result.SetPixel(x, y, Color.FromArgb(225, color, color, color));
                }
            }

            LastOpearationResult = result;
            return result;
        }

        private static Bitmap CloneImage(Bitmap image)
        {
            return (Bitmap)image.Clone();
        }

        private int[,] CreateFilter(int size)
        {
            int[,] kernel = new int[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    kernel[i, j] = 1;

            return kernel;
        }
        #endregion

        #region Границы

        public async Task<Bitmap> Soebel(Bitmap image)
        {
            return await Task.Run(() => _Soebel(image));
        }

        public async Task<Bitmap> Soebel()
        {
            return await Task.Run(() => _Soebel(LastOpearationResult));
        }

        public async Task<Bitmap> AllDirection()
        {
            return await Task.Run(() => _AllDirection(LastOpearationResult));
        }

        public async Task<Bitmap> AllDirection(Bitmap image)
        {
            return await Task.Run(() => _AllDirection(image));
        }

        private Bitmap _AllDirection(Bitmap image)
        {
            Bitmap result = new Bitmap(Width, Height);

            int[,] N = {
                { 1, 1, 1},
                { 1, -2, 1},
                { -1, -1, -1}
            };
            int[,] NE = {
                { 1, 1, 1},
                { -1, -2, 1},
                { -1, -1, 1}
            };
            int[,] E = {
                { -1, 1, 1},
                { -1, -2, 1},
                { -1, 1, 1}
            };
            int[,] SE = {
                { -1, -1, 1},
                { -1, -2, 1},
                { 1, 1, 1}
            };
            int[,] S = {
                { -1, -1, -1},
                { 1, -2, 1},
                { 1, 1, 1}
            };
            int[,] SW = {
                { 1, -1, -1},
                { 1, -2, -1},
                { 1, 1, 1}
            };
            int[,] W = {
                { 1, 1, -1},
                { 1, -2, -1},
                { 1, 1, -1}
            };
            int[,] NW = {
                { 1, 1, 1},
                { 1, -2, -1},
                { 1, -1, -1}
            };

            var n = TransformPixels(image, N, 3);
            var ne = TransformPixels(image, N, 3);
            var e = TransformPixels(image, N, 3);
            var se = TransformPixels(image, N, 3);
            var s = TransformPixels(image, N, 3);
            var sw = TransformPixels(image, N, 3);
            var w = TransformPixels(image, N, 3);
            var nw = TransformPixels(image, N, 3);

            result = new Bitmap(Width, Height);
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int color = (int)Math.Sqrt(Math.Pow(n.GetPixel(x, y).R, 2)
                        + Math.Pow(ne.GetPixel(x, y).R, 2)
                        + Math.Pow(e.GetPixel(x, y).R, 2)
                        + Math.Pow(se.GetPixel(x, y).R, 2)
                        + Math.Pow(s.GetPixel(x, y).R, 2)
                        + Math.Pow(sw.GetPixel(x, y).R, 2)
                        + Math.Pow(w.GetPixel(x, y).R, 2)
                        + Math.Pow(nw.GetPixel(x, y).R, 2));
                    color = Normalize(color);
                    result.SetPixel(x, y, Color.FromArgb(255, color, color, color));
                }
            }

            LastOpearationResult = result;
            return result;
        }


        private Bitmap _Soebel(Bitmap image)
        {
            Bitmap result;
            int[,] Lx = {
                { -1, -2, -1},
                { 0, 0, 0},
                { 1, 2, 1}
            };

            int[,] Ly = {
                { -1, 0, 1},
                { -2, 0, 2},
                { -1, 0, 1}
            };

            var resX = TransformPixels(image, Lx, 3);
            var resY = TransformPixels(image, Ly, 3);

            result = new Bitmap(Width, Height);
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int color = (int)Math.Sqrt(Math.Pow(resX.GetPixel(x, y).R, 2) + Math.Pow(resY.GetPixel(x, y).R, 2));
                    color = Normalize(color);
                    result.SetPixel(x, y, Color.FromArgb(255, color, color, color));
                }
            }

            LastOpearationResult = result;
            return result;
        }

        private Bitmap TransformPixels(Bitmap image, int[,] mask, int dim)
        {
            Bitmap result = CloneImage(image);
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int c = 0;
                    for (int i = 0; i < dim; i++)
                    {
                        for (int j = 0; j < dim; j++)
                        {
                            if ((x - (dim / 2) + j > 0 && x - (dim / 2) + j < Width) && (y - (dim / 2) + i > 0 && y - (dim / 2) + i < Height))
                                c += mask[i, j] * image.GetPixel(x - (dim / 2) + j, y - (dim / 2) + i).R;
                        }
                    }

                    c = Normalize(c);

                    result.SetPixel(x, y, Color.FromArgb(255, c, c, c));
                }
            }

            return result;
        }
#endregion
    }
}
