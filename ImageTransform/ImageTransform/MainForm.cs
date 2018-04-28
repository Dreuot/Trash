using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            pictureBox2.Image = transformer.GrayImage;
        }

        private async void эрозияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = await transformer.Erosion();
        }

        private async void дилекцияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = await transformer.Dilation();
        }
    }
}
