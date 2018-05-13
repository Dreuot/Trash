using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSearch
{
    class SearchEngine
    {
        public const double Alpha = 1;
        public const double Beta = 0.5;
        public const double Gamma = 2;
        public const double Eps = 0.001;

        private int[,] currentGray;
        private int[,] referenceGray;

        public Bitmap Current { get; set; }
        public Bitmap Reference { get; set; }
        public Bitmap Visualize { get; set; }
        public int Milliseconds { get; private set; }

        public int[,] CurrentGray
        {
            get
            {
                if (currentGray == null)
                    currentGray = ToGrayScale(Current);

                return currentGray;
            }
        }

        public int[,] ReferenceGray
        {
            get
            {
                if (referenceGray == null)
                    referenceGray = ToGrayScale(Reference);

                return referenceGray;
            }
        }

        public SearchEngine(Bitmap current, Bitmap reference)
        {
            Current = current;
            Reference = reference;
            Reset();
        }

        private void Reset()
        {
            Visualize = Current.Clone() as Bitmap;
        }

        private int[,] ToGrayScale(Bitmap image)
        {
            int[,] result = new int[image.Width, image.Height];
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color current = image.GetPixel(x, y);
                    int grayscale = (int)((current.R) * 0.3 + (current.G * 0.59) + (current.B * 0.11));
                    result[x, y] = grayscale;
                }
            }

            return result;
        }

        private double F(Point point)
        {
            long numerator = 0;
            long denumeratorRef = 0;
            long denumeratorCur = 0;

            for (int x = 0; x < Reference.Width; x++)
            {
                for (int y = 0; y < Reference.Height; y++)
                {
                    numerator += ReferenceGray[x, y] * CurrentGray[x + point.X, y + point.Y];
                    denumeratorCur += CurrentGray[x + point.X, y + point.Y] * CurrentGray[x + point.X, y + point.Y];
                    denumeratorRef += ReferenceGray[x, y] * ReferenceGray[x, y];
                }
            }

            double result = (double)numerator / (Math.Sqrt(denumeratorCur) * Math.Sqrt(denumeratorRef));
            result = 1 - result;

            return result;
        }

        public void Search()
        {
            int MaxHeight = Current.Height - Reference.Height;
            int MaxWidth = Current.Width - Reference.Width;
            Reset();

            DateTime start = DateTime.Now;

            PointValue[] points = new PointValue[3];
            int row = 0;
            int column = 0;
            bool xOverflow = false;
            bool yOverflow = false;
            Action<PointValue> ChechBound = (point) =>
            {
                point.X = (point.X >= 0) ? point.X : 0;
                point.X = (point.X >= MaxWidth) ? MaxWidth : point.X;
                point.Y = (point.Y >= 0) ? point.Y : 0;
                point.Y = (point.Y >= MaxHeight) ? MaxHeight : point.Y;
            };

            Action SetPoints = () =>
            {
                xOverflow = false;
                yOverflow = false;
                for (int i = 0; i < 3; i++)
                    points[i] = new PointValue(new Point(), 0);

                int xStep = 30 * column;
                int yStep = 30 * row;

                points[0].Y = 30 + yStep;
                points[1].Y = 30 + yStep;
                points[2].Y = 60 + yStep;

                points[0].X = 0 + xStep;
                points[1].X = 50 + xStep;
                points[2].X = 30 + xStep;
                if(points[1].X > MaxWidth)
                {
                    points[0].X = 0;
                    points[1].X = 50;
                    points[2].X = 30;
                    row++;
                    column = 0;
                    xOverflow = true;
                }

                if(points[2].Y > MaxHeight)
                {
                    points[0].Y = MaxHeight - 30;
                    points[1].Y = MaxHeight - 30;
                    points[2].Y = MaxHeight;
                    yOverflow = true;
                }
            };

            double delta = 0;
            do
            {
                SetPoints();
                points[0].Value = F(points[0].Point);
                points[1].Value = F(points[1].Point);
                points[2].Value = F(points[2].Point);
                column++;
                do
                {
                    // Шаг 1. Сортировка
                    Array.Sort(points);
                    // Шаг 2. Вычисление центра тяжести
                    PointValue center = new PointValue();
                    center.X = (points[0].X + points[1].X) / 2;
                    center.X = (points[0].X + points[1].X) / 2;
                    ChechBound(center);

                    // Шаг 3. Отражение
                    PointValue reflection = new PointValue();
                    reflection.X = (int)(center.X * (1 + Alpha) - points[2].X * Alpha);
                    reflection.Y = (int)(center.Y * (1 + Alpha) - points[2].Y * Alpha);
                    ChechBound(reflection);
                    reflection.Value = F(reflection.Point);
                    bool flag = true;
                    // Шаг 4.
                    if(reflection.Value < points[1].Value)
                    {
                        points[2] = reflection;
                        flag = false;
                    }
                    else
                    {
                        if(reflection.Value < points[1].Value)
                        {
                            points[2] = reflection;
                            flag = false;
                        }

                        PointValue c = new PointValue();
                        c.X = (points[2].X + center.X) / 2;
                        c.Y = (points[2].Y + center.Y) / 2;
                        c.Value = F(c.Point);

                        if(c.Value < points[2].Value)
                        {
                            points[2] = c;
                            flag = false;
                        }
                    }

                    if(reflection.Value < points[0].Value)
                    {
                        // Шаг 4a. Растяжение
                        PointValue stretch = new PointValue();
                        stretch.X = (int)(center.X * (1 - Gamma) + reflection.X * Gamma);
                        stretch.Y = (int)(center.Y * (1 - Gamma) + reflection.Y * Gamma);
                        ChechBound(stretch);
                        stretch.Value = F(stretch.Point);

                        if (stretch.Value < reflection.Value)
                            points[2] = stretch;
                        else
                            points[2] = reflection;

                        flag = false;
                    }

                    if(reflection.Value > points[1].Value)
                    {
                        // Шаг 5. Сжатие
                        PointValue compr = new PointValue();
                        compr.X = (int)(Beta * points[2].X + (1 - Beta) * center.X);
                        compr.Y = (int)(Beta * points[2].Y + (1 - Beta) * center.Y);
                        compr.Value = F(compr.Point);

                        if (compr.Value < points[2].Value)
                        {
                            points[2] = compr;
                            flag = false;
                        }
                    }

                    if(flag)
                    {
                        points[1].X = points[0].X + (points[1].X - points[0].X) / 2;
                        points[1].Y = points[0].Y + (points[1].Y - points[0].Y) / 2;
                        points[1].Value = F(points[1].Point);

                        points[2].X = points[0].X + (points[2].X - points[0].X) / 2;
                        points[2].Y = points[0].Y + (points[2].Y - points[0].Y) / 2;
                        points[2].Value = F(points[2].Point);
                    }

                    DrawTriangle(points);
                    delta = ((points[0].X - points[2].X) * (points[1].Y - points[2].Y) - (points[1].X - points[2].X) * (points[0].Y - points[2].X)) / 2;
                    delta = Math.Abs(delta);
                } while (delta > 0.001);
            } while (points[2].Value > 0.01 && !(xOverflow && yOverflow));

            DateTime finish = DateTime.Now;
            Milliseconds = (finish - start).Milliseconds;
            DrawResult(points[0].Point, Reference.Width, Reference.Height);
        }

        private Point[] Sort(Point[] points)
        {
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    if (F(points[j]) > F(points[j + 1]))
                    {
                        var temp = points[j];
                        points[j] = points[j + 1];
                        points[j + 1] = temp;
                    }

            return points;
        }

        private void DrawTriangle(PointValue[] points)
        {
             Graphics g = Graphics.FromImage(Visualize);
             g.DrawLine(Pens.LimeGreen, points[0].Point, points[1].Point);
             g.DrawLine(Pens.LimeGreen, points[1].Point, points[2].Point);
             g.DrawLine(Pens.LimeGreen, points[2].Point, points[0].Point);
        }

        private void DrawResult(Point p, int width, int height)
        {
            Graphics g = Graphics.FromImage(Visualize);
            g.DrawRectangle(Pens.Red, new Rectangle(p, new Size(width, height)));
        }
    }
}
