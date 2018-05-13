using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageTransformer;

namespace ImageTransform
{
    public partial class MainForm : Form
    {
        Transformer transformer;
        Bitmap salt;
        List<Point> firstPoints = new List<Point>();
        List<Point> secondPoints = new List<Point>();

        Bitmap first;
        Bitmap second;

        public MainForm()
        {
            InitializeComponent();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                transformer = new Transformer(openFileDialog1.FileName);
                pictureBox1.Image = transformer.Image;
            }
        }

        private void сольИПерецToolStripMenuItem_Click(object sender, EventArgs e)
        {
            salt = transformer.SaltAndPepper(0.05);
            pictureBox2.Image = (Bitmap)salt.Clone();
        }

        private async void x3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = await transformer.MedianAsync(3);
        }

        private async void x5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = await transformer.MedianAsync(5);
        }

        private async void x7ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = await transformer.MedianAsync(7);
        }

        private async void x9ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = await transformer.MedianAsync(9);
        }

        private async void гаусToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = await transformer.Gause(0.5, 0.75);
        }

        private async void фильтрГауссаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = await transformer.GauseFilter();
        }

        private async void собельToolStripMenuItem_Click(object sender, EventArgs e)
        {
            transformer.Reset();
            pictureBox2.Image = await transformer.Soebel();
        }

        private async void всеНаправленияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            transformer.Reset();
            pictureBox2.Image = await transformer.AllDirection();
        }

        private async void чернобелоеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = await transformer.TransformToBinary();
        }

        private void сброситьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            transformer.Reset();
            pictureBox2.Image = transformer.LastOpearationResult;
        }

        private async void эрозияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = await transformer.Erosion();
        }

        private async void дилекцияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = await transformer.Dilation();
        }

        private async void поворотToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputForm input = new InputForm("Введите угол в градусах");
            if (input.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    double angle = double.Parse(input.Data);
                    angle = angle * Math.PI / 180;
                    pictureBox2.Image = await transformer.Round(angle);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private async void растяжениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputForm input = new InputForm("Растяжение по X");
            {
                if (input.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        double x_mult = double.Parse(input.Data);
                        input = new InputForm("Растяжение по Y");
                        if(input.ShowDialog() == DialogResult.OK)
                        {
                            double y_mult = double.Parse(input.Data);
                            pictureBox2.Image = await transformer.Stretch(x_mult, y_mult);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private async void градацииСерогоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = await transformer.TransformToGrayScale();
        }

        private async void вертикальноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = await transformer.Mirror(Direction.Vertical);
        }

        private async void горизонтальноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = await transformer.Mirror(Direction.Horisontal);
        }

        private async void сдвигToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputForm input = new InputForm("Сдвиг по X");
            {
                if (input.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        double x = double.Parse(input.Data);
                        input = new InputForm("Сдвиг по Y");
                        if (input.ShowDialog() == DialogResult.OK)
                        {
                            double y = double.Parse(input.Data);
                            Matrix vector = new Matrix(2, 1);
                            vector.Init(x, y);
                            pictureBox2.Image = await transformer.Move(vector);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (firstPoints.Count < 4 && pictureBox1.Image != null)
            {
                MouseEventArgs m = (MouseEventArgs)e;
                Point coordinates = m.Location;
                Bitmap image = pictureBox1.Image as Bitmap;
                Point onImage = GetPointOnImage(image, coordinates);
                firstPoints.Add(onImage);

                if (onImage.X < image.Width && onImage.X > 0 && onImage.Y > 0 && onImage.Y < image.Height)
                {
                    image.SetPixel(onImage.X, onImage.Y, Color.Yellow);
                    pictureBox1.Image = image;
                }
            }
        }

        private Point GetPointOnImage(Bitmap image, Point p)
        {
            double xRatio = 1.0 * pictureBox1.Width / image.Width;
            double yRatio = 1.0 * pictureBox1.Height / image.Height;
            double ratio = xRatio < yRatio ? xRatio : yRatio;
            int halfX = pictureBox1.Width / 2;
            int halfY = pictureBox1.Height / 2;
            int halfImageX = image.Width / 2;
            int halfImageY = image.Height / 2;
            int x = (int)((double)(p.X - halfX) / ratio + halfImageX);
            int y = (int)((double)(p.Y - halfY) / ratio + halfImageY);
            return new Point(x, y);
        }

        private void сброToolStripMenuItem_Click(object sender, EventArgs e)
        {
            firstPoints.Clear();
            secondPoints.Clear();
            pictureBox1.Image = transformer.Image;
            pictureBox2.Image = second;
        }

        private void выбратьИзображенияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                second = new Bitmap(openFileDialog1.FileName);
            }

            pictureBox2.Image = (Image)second.Clone();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (secondPoints.Count < 4 && pictureBox2.Image != null)
            {
                MouseEventArgs m = (MouseEventArgs)e;
                Point coordinates = m.Location;
                Bitmap image = pictureBox2.Image as Bitmap;
                Point onImage = GetPointOnImage(image, coordinates);

                if (onImage.X < image.Width && onImage.X > 0 && onImage.Y > 0 && onImage.Y < image.Height)
                {
                    secondPoints.Add(onImage);

                    image.SetPixel(onImage.X, onImage.Y, Color.Yellow);
                    pictureBox2.Image = image;
                }
            }
        }

        private async void выполнитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (firstPoints.Count != 4 || secondPoints.Count != 4)
            {
                MessageBox.Show("Выберите по 4 точки на каждом изображении");
                return;
            }

            pictureBox2.Image = await transformer.HomographyAsync(second, firstPoints.ToArray(), secondPoints.ToArray());
        }

        public static Bitmap SetImgOpacity(Image imgPic, float imgOpac)
        {

            Bitmap bmpPic = new Bitmap(imgPic.Width, imgPic.Height);
            Graphics gfxPic = Graphics.FromImage(bmpPic);
            ColorMatrix cmxPic = new ColorMatrix();
            cmxPic.Matrix33 = imgOpac;

            ImageAttributes iaPic = new ImageAttributes();
            iaPic.SetColorMatrix(cmxPic, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            gfxPic.DrawImage(imgPic, new Rectangle(0, 0, bmpPic.Width, bmpPic.Height), 0, 0, imgPic.Width, imgPic.Height, GraphicsUnit.Pixel, iaPic);
            gfxPic.Dispose();

            return bmpPic;
        }
    }
}
