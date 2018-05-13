using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageSearch
{
    public partial class Form1 : Form
    {
        Bitmap current;
        Bitmap reference;

        public Form1()
        {
            InitializeComponent();
        }

        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
            if (op.ShowDialog() == DialogResult.OK)
            {
                current = new Bitmap(op.FileName);
                pictureBox1.Image = (Image)current.Clone();
            }
        }

        private void выбратьЭталонноеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
            if (op.ShowDialog() == DialogResult.OK)
            {
                reference = new Bitmap(op.FileName);
                pictureBox2.Image = (Image)reference.Clone();
            }
        }

        private async void поискToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (reference == null || current == null)
                MessageBox.Show("Необходимо выбрать изображения");

            SearchEngine engine = new SearchEngine(current, reference);

            await Task.Run( () => engine.Search());
            pictureBox1.Image = engine.Visualize;

            MessageBox.Show($"Завершено за {engine.Milliseconds} миллисекунд");
        }
    }
}
