using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Phonebook
{
    public partial class Form1 : Form
    {
        string loaded = "";
        List<Contact> contacts;
        List<ContactDisplay> displays;

        public Form1()
        {
            InitializeComponent();
            contacts = new List<Contact>();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                loaded = openFileDialog1.FileName;

                XmlSerializer serializer = new XmlSerializer(typeof(List<Contact>));
                using (FileStream open = new FileStream(loaded, FileMode.OpenOrCreate))
                {
                    contacts = (List<Contact>)serializer.Deserialize(open);
                }
            }
        }

        private void UpdateDisplay()
        {
            displays = new List<ContactDisplay>(contacts.Count);
            foreach (var contact in contacts)
            {
                displays.Add(new ContactDisplay(contact));
            }

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = displays;
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName = loaded == "" ? "autosave.xml" : loaded;

            SaveContacts(fileName);
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                SaveContacts(saveFileDialog1.FileName);
            }
        }

        private void SaveContacts(string fileName)
        {
            BinaryFormatter serializer = new BinaryFormatter();
            using (FileStream save = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                serializer.Serialize(save, contacts);
            }
        }

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddContact add = new AddContact();
            if(add.ShowDialog() == DialogResult.OK)
            {
                contacts.Add(add.contact);
                UpdateDisplay();
            }
        }
    }
}
