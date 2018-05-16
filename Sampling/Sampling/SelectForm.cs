using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sampling
{
    public partial class SelectForm : Form
    {
        Sampler.SamplingData data;

        public bool Numeric { get; set; }
        public String Property { get; set; }
        public int Layer { get; set; }
        public double Percent { get; set; }

        public SelectForm(Sampler.SamplingData data)
        {
            InitializeComponent();
            this.data = data;
            this.comboBox1.DataSource = data.Captions.ToList();
            this.comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Property = comboBox1.SelectedValue.ToString();
            if (numericUpDown1.Visible)
            {
                Numeric = true;
                Layer = (int)numericUpDown1.Value;
            }

            int percent = 0;
            if (int.TryParse(textBox1.Text, out percent))
            {
                Percent = percent / 100.0;
            }
            else
            {
                MessageBox.Show("Неоходимо указать процент выборки");
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            string value = comboBox1.SelectedValue.ToString();
            if (double.TryParse(data[0][value], out _))
            {
                label3.Show();
                numericUpDown1.Show();
            }
            else
            {
                label3.Hide();
                numericUpDown1.Hide();
            }

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8) // цифры, клавиша BackSpace и запятая
            {
                e.Handled = true;
            }

            if (textBox1.Text.Length > 1 && textBox1.Text != "10" && number != '0' && number != 8)
                e.Handled = true;

            if (textBox1.Text.Length > 2 && number != 8)
                e.Handled = true;
        }
    }
}
