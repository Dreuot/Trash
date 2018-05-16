using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sampler;

namespace Sampling
{
    public partial class Form1 : Form
    {
        Sampler.Sampler sampler;
        Sampler.SamplingData selected;

        public Form1()
        {
            InitializeComponent();
        }


        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            if (op.ShowDialog() == DialogResult.OK)
            {
                string fileName = op.FileName;
                sampler = new Sampler.Sampler(";");
                sampler.LoadData(fileName, true);
                selected = sampler.Data;
                Display();
            }
        }

        private void Display()
        {
            var captions = selected.Captions;
            foreach (var item in captions)
            {
                dataGridView1.Columns.Add(item, item);
            }

            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add(captions);
            for (int i = 0; i < selected.RowCount; i++)
            {
                List<string> line = new List<string>();
                foreach (var item in captions)
                {
                    line.Add(selected[i][item]);
                }

                dataGridView1.Rows.Add(line.ToArray());
            }
        }

        private void выбратьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                MessageBox.Show("Необходимо осуществить выборку");
                return;
            }

            SaveFileDialog sd = new SaveFileDialog();
            if (sd.ShowDialog() == DialogResult.OK)
            {
                string fileName = sd.FileName;
                StringBuilder sb = new StringBuilder();
                var captions = selected.Captions;
                foreach (var item in captions)
                    sb.Append(item + ";");
                sb.Append("\n");

                for (int i = 0; i < selected.RowCount; i++)
                {
                    foreach (var caption in captions)
                    {
                        sb.Append(selected[i][caption]);
                        sb.Append("\n");
                    }
                }

                File.WriteAllText(fileName, sb.ToString());
            }
        }
    }
}
