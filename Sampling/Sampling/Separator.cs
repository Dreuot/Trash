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
    public partial class SeparatorForm : Form
    {
        public string Separator { get; set; }

        public SeparatorForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                Separator = "\t";
            else
                Separator = ";";

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
