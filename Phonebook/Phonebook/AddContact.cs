using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Phonebook
{
    public partial class AddContact : Form // наследование
    {
        public Contact contact;
        bool validated = false;

        public AddContact()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(validated)
            {
                contact = new Contact(textBox4.Text, textBox2.Text, textBox1.Text, textBox3.Text, textBox6.Text);
                try
                {
                    contact.SetAddress(textBox5.Text);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Адрес не распознан");
                }
            }
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (textBox1.Text == "")
            {
                errorProvider1.SetError((TextBox)sender, "Не заполнено поле Фамилия");
                validated = false;
            }
            else
            {
                validated = true;
            }
        }

        private void textBox4_Validating(object sender, CancelEventArgs e)
        {
            if (textBox4.Text == "")
            {
                errorProvider1.SetError((TextBox)sender, "Не заполнено поле Номер");
                validated = false;
            }
            else
            {
                validated = true;
            }
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            if (textBox2.Text == "")
            {
                errorProvider1.SetError((TextBox)sender, "Не заполнено поле Имя");
                validated = false;
            }
            else
            {
                validated = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK )
            {
                textBox6.Text = openFileDialog1.FileName;
            }
        }
    }
}
