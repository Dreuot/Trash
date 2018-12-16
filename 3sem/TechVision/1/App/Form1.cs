using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
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
                sfd.Filter = "(*.PNG*, .BMP, *.JPG, *.JPEG)|*.png;*.bmp;*.jpg;*.jpeg";
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
            try
            {
                Console.ShowMessage(error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void вГрадацияхСерогоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Image = await Wrapper.ToGrayScaleAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void сбросToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ResetImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void гистограммаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Console.DrawChart("Гистограмма", Wrapper.GetBarChart());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Console.Clear();
                Console.DrawChart("Гистограмма", Wrapper.GetBarChart());
                Console.ShowMessage(Wrapper.GetInfo());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void собельToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Image = await Wrapper.SoebelAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
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
                Console.ShowMessage($"Значение среднеквадратичного отклонения равно: {Math.Sqrt(disp)}");

                ResetImage();
                var g = Graphics.FromImage(pictureBox1.Image);
                g.DrawRectangle(new Pen(Color.Red), (int)x, (int)y, d_size, d_size);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ResetImage()
        {
            try
            {
                Image = (Bitmap)Wrapper.GetImage().Clone();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void дисторсияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                double L = double.Parse(Interaction.InputBox("Введите коэффициент дисторсии", "Коэффициент дисторсии", "0,3"));
                var result = await Wrapper.DistorsionAsync(L);
                Image = result;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void графикиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Console.DrawChart("Гистограмма", Wrapper.GetBarChart());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void линейноеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var result = await Wrapper.LinearContrastAsync();
                Image = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void эквализацияToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                var result = await Wrapper.EqualizContrastAsync();
                Image = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void линейноеToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                var image = await Wrapper.LinearContrastYIQ();
                Image = image;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void эквализацияToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                var image = await Wrapper.EqualizContrastYIQ();
                Image = image;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            d_size = 15;
            MessageBox.Show("Выберите область");
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            d_size = 31;
            MessageBox.Show("Выберите область");
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            d_size = 40;
            MessageBox.Show("Выберите область");
        }

        private async void комплексированиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string tv = Interaction.InputBox("Введите путь до изображения с ТВ камеры", "Путь к файлу", @"C:\Users\Андрей\Pictures\TV.bmp");
                string ir = Interaction.InputBox("Введите путь до изображения с ТПВ камеры", "Путь к файлу", @"C:\Users\Андрей\Pictures\TPV.bmp");
                Bitmap _tv = (Bitmap)Bitmap.FromFile(tv);
                Bitmap _ir = (Bitmap)Bitmap.FromFile(ir);
                Image = await Wrapper.ComplexAsync(_tv, _ir);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void кластеризацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int count = int.Parse(Interaction.InputBox("Введите число кластеров", "Число кластеров", "5"));
                int iteration = int.Parse(Interaction.InputBox("Введите число прогонов", "Число прогонов", "5"));
                Image = await Wrapper.ClusterAsync(count, iteration);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void цветноеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                double gamma = double.Parse(Interaction.InputBox("Введите значение 'гамма'", "Гамма", "5,5"));
                Image = await Wrapper.ColoredBlurAsync(gamma);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private async void сероеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                double gamma = double.Parse(Interaction.InputBox("Введите значение 'гамма'", "Гамма", "5,5"));
                Image = await Wrapper.BlurAsync(gamma);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
