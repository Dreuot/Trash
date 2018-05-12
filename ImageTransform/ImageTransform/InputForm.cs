using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageTransform
{
    public partial class InputForm : Form
    {
        public string Data { get; set; }

        public InputForm(string message)
        {
            InitializeComponent();
            MessageLBL.Text = message;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Data = DataTB.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
