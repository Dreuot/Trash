using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

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
            op.Filter = "(*.csv)|*.csv";
            if (op.ShowDialog() == DialogResult.OK)
            {
                SeparatorForm sep = new SeparatorForm();
                if (sep.ShowDialog() == DialogResult.OK)
                {
                    string fileName = op.FileName;
                    sampler = new Sampler.Sampler(sep.Separator);
                    sampler.LoadData(fileName, true);
                    selected = sampler.Data;
                    Display();
                }
            }
        }

        private void Display()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            var captions = selected.Captions;
            foreach (var item in captions)
            {
                dataGridView1.Columns.Add(item, item);
            }

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
            sd.Filter = "(*.csv)|*.csv";
            if (sd.ShowDialog() == DialogResult.OK)
            {
                string fileName = sd.FileName;
                StringBuilder sb = new StringBuilder();
                var captions = selected.Captions;
                foreach (var item in captions)
                    sb.Append(item + ";");
                sb.Remove(sb.Length - 1, 1);
                sb.Append("\n");

                for (int i = 0; i < selected.RowCount; i++)
                {
                    foreach (var caption in captions)
                    {
                        sb.Append(selected[i][caption]);
                        sb.Append(";");
                    }

                    sb.Remove(sb.Length - 1, 1);
                    sb.Append("\n");
                }

                File.WriteAllText(fileName, sb.ToString());
            }
        }

        private void выбратьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (sampler == null || sampler.Data == null)
            {
                MessageBox.Show("Необходимо загрузить файл");
                return;
            }

            SelectForm sf = new SelectForm(sampler.Data);
            if (sf.ShowDialog() == DialogResult.OK)
            {
                double percent = sf.Percent;
                int layer = sf.Layer;
                bool numeric = sf.Numeric;
                string prop = sf.Property;

                if (numeric)
                    selected = sampler.MakeSelect(percent, prop, layer);
                else
                    selected = sampler.MakeSelect(percent, prop);

                Display();
            }
        }

        private void сбросToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(sampler == null || sampler.Data == null)
            {
                MessageBox.Show("Необходимо выбрать файл");
                return;
            }

            selected = sampler.Data;
            Display();
        }
    }
}
