using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graph
{
    public partial class Form1 : Form
    {
        Graph graph;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int size = int.Parse(textBox1.Text);
                int start = int.Parse(textBox3.Text);
                int end = int.Parse(textBox4.Text);
                string[] elements = textBox2.Text.Split(new char[] { ' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
                int[] distances = new int[size * size];
                for (int i = 0; i < size * size; i++)
                {
                    if (elements[i] != "inf")
                        distances[i] = int.Parse(elements[i]);
                    else
                        distances[i] = Graph.Infinity;
                }

                graph = new Graph(size);
                graph.Initialize(distances);
                graph.SetStart(start);
                graph.SetEnd(end);
                richTextBox1.Text = graph.ToString();
                richTextBox2.Text = graph.Distances();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void алгоритмДейкстрыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graph.Dijkstra();
            richTextBox2.Text = graph.Distances();
            richTextBox1.Text += $"\n Кратчайший путь от узла {graph.Start.Number} до узла {graph.End.Number}: " + graph.Way();
        }

        private void дейкстраToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox2.Text = "inf 1 inf inf inf inf inf inf 5 2 inf 7 inf inf inf 1 inf 1 2 inf 1 inf 4 inf inf inf inf 4 inf inf inf inf inf inf 1 inf ";
            textBox1.Text = "6";
            textBox3.Text = "0";
            textBox4.Text = "5";

            richTextBox1.Text = "";
            richTextBox2.Text = "";
        }

        private void динамПрогрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox2.Text = "inf 2 3 1 inf inf inf inf inf inf inf 3 inf inf 8 inf inf inf inf inf inf 4 6 inf inf inf inf inf inf inf inf 4 inf 5 inf inf inf inf inf inf inf 1 inf 3 inf inf inf inf inf inf inf inf inf 4 inf inf inf inf inf inf inf 3 8 inf inf inf inf inf inf inf inf 5 inf inf inf inf inf inf inf inf inf ";
            textBox1.Text = "9";
            textBox3.Text = "0";
            textBox4.Text = "8";

            richTextBox1.Text = "";
            richTextBox2.Text = "";
        }

        private void методДинамическогоПрограммированияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graph.Dynamic();
            richTextBox2.Text = graph.Distances();
            StringBuilder sb = new StringBuilder();
            sb.Append("( ");
            foreach (var item in graph.Nodes)
                sb.Append($"{item.Distance} ");

            sb.Append(")");
            richTextBox1.Text += "\n Полученный вектор пометок: " + sb.ToString();

            MessageBox.Show(graph.Way());
        }
    }
}
