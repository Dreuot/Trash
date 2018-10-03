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
                ofd.Filter = "(*.BMP, *.JPG, *.JPEG)|*.bmp;*.jpg;*.jpeg";
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
            Image = Wrapper.GetImage();
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
    }
}
