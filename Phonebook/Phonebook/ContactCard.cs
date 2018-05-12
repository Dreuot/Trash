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
    public partial class ContactCard : Form
    {
        public ContactCard(Contact contact)
        {
            InitializeComponent();
            label10.Text = contact.LastName;
            label11.Text = contact.FirstName;
            label12.Text = contact.MiddleName;
            label13.Text = contact.Phone;
            label14.Text = contact.Country;
            label15.Text = contact.City;
            label16.Text = contact.Street;
            label17.Text = contact.House;
            label18.Text = contact.Flat;

            pictureBox1.Image = contact.Photo;
        }

        private void ContactCard_Load(object sender, EventArgs e)
        {

        }
    }
}
