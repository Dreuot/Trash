using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FirstLib;

namespace App
{
    public partial class Form1 : Form
    {
        private Bitmap image;
        public Bitmap Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                pictureBox1.Image = image;
            }
        }

        private int d_size;

        private ImageWrapper Wrapper;

        private ConsoleForm console = null;
        ConsoleForm Console
        {
            get
            {
                if (console != null)
                    return console;

                console = new ConsoleForm();
                return console;
            }
        }

        public Form1()
        {
            InitializeComponent();
            Console.Show();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var ofd = new OpenFileDialog();
                ofd.CheckFileExists = true;
                ofd.Filter = "(*.PNG*, .BMP, *.JPG, *.JPEG)|*.png;*.bmp;*.jpg;*.jpeg";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Image = (Bitmap)Bitmap.FromFile(ofd.FileName);
                    Wrapper = new ImageWrapper(Image);
                }
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
            
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var sfd = new SaveFileDialog();
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    Image.Save(sfd.FileName);
                }
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }

        private void LogError(string error)
        {
            Console.ShowMessage(error);
        }

        private async void вГрадацияхСерогоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image = await Wrapper.ToGrayScaleAsync();
        }

        private void сбросToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetImage();
        }

        private void гистограммаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.DrawChart("Гистограмма", Wrapper.GetBarChart());
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.Clear();
            Console.DrawChart("Гистограмма", Wrapper.GetBarChart());
            Console.ShowMessage(Wrapper.GetInfo());
        }

        private async void собельToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image = await Wrapper.SoebelAsync();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MouseEventArgs ev = e as MouseEventArgs;
            double x = ev.X;
            double y = ev.Y;

            double x_stretch = 1.0 * Wrapper.Width / pictureBox1.Width;
            double y_stretch = 1.0 * Wrapper.Height / pictureBox1.Height;

            x *= x_stretch;
            y *= y_stretch;
            x = Math.Truncate(x);
            y = Math.Truncate(y);

            var disp = Wrapper.GetDispersion((int)x, (int)y, d_size, d_size);
            Console.Clear();
            Console.ShowMessage($"Значение дисперсии равно: {disp}");

            ResetImage();
            var g = Graphics.FromImage(pictureBox1.Image);
            g.DrawRectangle(new Pen(Color.Red), (int)x, (int)y, d_size, d_size);
        }

        private void ResetImage()
        {
            Image = (Bitmap)Wrapper.GetImage().Clone();
        }

        private void x15ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            d_size = 15;
            MessageBox.Show("Выберите область");
        }

        private void x31ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            d_size = 31;
            MessageBox.Show("Выберите область");
        }

        private void x40ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            d_size = 40;
            MessageBox.Show("Выберите область");
        }

        private async void дисторсияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = await Wrapper.DistorsionAsync(-0.00000476);
            Image = ImageWrapper.GrayArrayToImage(result);
        }

        private async void контрастированиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = await Wrapper.ContrastAsync();
            Image = result;
        }
    }
}
