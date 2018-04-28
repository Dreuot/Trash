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
                    grayImage = TransformToGrayScale();

                return grayImage;
            }
        }

        public Bitmap LastOpearationResult { get; private set; }

        public int Width => Image.Width;
        public int Height => Image.Height;


        public Transformer(string fileName)
        {
            Image = new Bitmap(fileName);
        }

        public Transformer(Bitmap image)
        {
            Image = image;
        }

        public Transformer(Stream stream)
        {
            Image = new Bitmap(stream);
        }

        public Transformer(Image image)
        {
            Image = new Bitmap(image);
        }

        private Bitmap TransformToGrayScale()
        {
            Bitmap result = CloneImage(Image);
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
            LastOpearationResult = GrayImage;
        }

        #region Морфологические операции

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

        public async Task<Bitmap> Dilation(Bitmap image)
        {
            int[,] mask = {
                { 0, 1, 0},
                { 1, 1, 1},
                { 0, 1, 0}
            };
            return await Task.Run(() => _Dilation(image, mask, 3));
        }

        public async Task<Bitmap> Dilation()
        {
            int[,] mask = {
                { 0, 1, 0},
                { 1, 1, 1},
                { 0, 1, 0}
            };
            return await Task.Run(() => _Dilation(LastOpearationResult, mask, 3));
        }

        private Bitmap _Dilation(Bitmap image, int[,] mask, int dim)
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
