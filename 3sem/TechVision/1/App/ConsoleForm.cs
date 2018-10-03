using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App
{
    public partial class ConsoleForm : Form
    {
        public ConsoleForm()
        {
            InitializeComponent();
            SetDefault();
        }

        private void SetDefault()
        {
            richTextBox1.Font = new Font("Times New Roman", 14);
        }

        public void ShowMessage(string text)
        {
            richTextBox1.AppendText(text + "\r\n");
        }

        public void DrawChart(string name, int[] points)
        {
            chart1.Series.Clear();
            chart1.Series.Add(name);
            chart1.ChartAreas[0].AxisY.Maximum = points.Max();
            for (int i = 0; i < points.Length; i++)
            {
                chart1.Series[name].Points.AddY(points[i]);
            }
        }

        public void Clear()
        {
            richTextBox1.Clear();
        }
    }
}
