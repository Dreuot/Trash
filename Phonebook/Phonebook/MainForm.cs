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

                BinaryFormatter serializer = new BinaryFormatter(); // сериализатор-для чтения и записи
                using (FileStream open = new FileStream(loaded, FileMode.OpenOrCreate))
                {
                    contacts = (List<Contact>)serializer.Deserialize(open);
                }

                UpdateDisplay();
            }
        }

        private void UpdateDisplay()
        {
            displays = new List<ContactDisplay>(contacts.Count);
            foreach (var contact in contacts)
            {
                displays.Add(new ContactDisplay(contact));
            }

            displays = displays.OrderBy(c => c.FullName).ToList();

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = displays; // вывод списка контактов в таблицу
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName = loaded == "" ? "autosave" : loaded;

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
            BinaryFormatter serializer = new BinaryFormatter(); // объект для сохранения списка контактов
            using (FileStream save = new FileStream(fileName, FileMode.OpenOrCreate)) // открываем поток файла для записи
            {
                serializer.Serialize(save, contacts); // Сохраняем контакты в файл в двоичном виде
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

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = 0;
            Contact contact = new Contact();
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selected = dataGridView1.CurrentRow.Index;
            string FIO = dataGridView1.Rows[selected].Cells[0].Value.ToString();
            string phone = dataGridView1.Rows[selected].Cells[1].Value.ToString();

            Contact contact = contacts.Where(c => c.Phone == phone && c.FullName == FIO).FirstOrDefault(); // поиск контакта с выбранным условием
            if(contact != null)
            {
                contacts.Remove(contact);
                UpdateDisplay();
            }
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int selected = dataGridView1.CurrentRow.Index;
            string FIO = dataGridView1.Rows[selected].Cells[0].Value.ToString();
            string phone = dataGridView1.Rows[selected].Cells[1].Value.ToString();

            Contact contact = contacts.Where(c => c.Phone == phone && c.FullName == FIO).FirstOrDefault(); // поиск контакта с выбранным условием
            ContactCard card = new ContactCard(contact);
            card.Show();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            string searchString = textBox1.Text;

            //функция поиска, 
            Func<Contact, bool> selector = contact =>
            {
                bool found = false;
                if (contact.FullName.Contains(searchString))
                    found = true;

                if (contact.Address.Contains(searchString))
                    found = true;

                if (contact.Phone.Contains(searchString))
                    found = true;

                return found;
            };

            if (searchString == "")
            {
                UpdateDisplay();
            }
            else
            {
                List<Contact> result = contacts.Where(c => selector(c)).ToList();
                List<ContactDisplay> displayResult = new List<ContactDisplay>();
                for (int i = 0; i < result.Count; i++)
                {
                    displayResult.Add(new ContactDisplay(result[i]));
                }

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = displayResult;
            }
        }
    }
}
