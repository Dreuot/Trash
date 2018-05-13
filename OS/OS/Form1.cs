using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using NtfsLib;

namespace OS
{
    public partial class Form1 : Form
    {
        List<DisplayMFT> display = new List<DisplayMFT>();

        public Form1()
        {
            InitializeComponent();
            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }

        private int FoundSubdir(MFT record, string dir) // функция поиска файла в индексных элементах каталога по имени файла
        {
            int result = 0;

            foreach (var index in record.Indexes) // для всех индексных элементов каталога проверяем 
            {
                if (index.FileNameString == dir) // если имя файла совпадает с введенным, то сохраняем номер записи МФТ из индексного элемента
                    result = (int)index.IndexedFile;
            }

            return result; // возвращаем найденный номер
        }

        private void поискToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) // Если выбрали катало
            {
                string fileName = folderBrowserDialog1.SelectedPath; // путь к выбранному каталогу
                string[] catalogs = fileName.Split(new char[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries );//Разбиваем полный путь на имена каталогов

                NTFS ntfs = new NTFS(catalogs[0]); // создаем объектное представление файловой системы логического диска

                int nextRecord = 5; // 5 запись -- корневой каталог, поиск начинаем с него
                MFT root; // переменная для хранения текущего каталога
                for (int i = 1; i < catalogs.Length; i++)
                {
                    root = ntfs.GetMftRecord(nextRecord); // читаем следующую запись МФТ со всеми аттрибутами, включая INDEX_ROOT и INDEX_ALLOCATION
                    nextRecord = FoundSubdir(root, catalogs[i]); // Ищем номер записи МФТ следующего каталога
                }

                MFT catalog = ntfs.GetMftRecord(nextRecord); // Читаем запись найденного каталога
                display = new List<DisplayMFT>();
                display.Add(new DisplayMFT(catalog)); // Первым отображается запись самого каталога
                foreach (var index in catalog.Indexes)
                {
                    display.Add(new DisplayMFT(ntfs.GetMftRecord((int)index.IndexedFile))); // добавляем в отображаемые записи сведения о файле в каталоге
                }

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = display; // выводим данные в таблицу
            }
        }

        private void копированиеКаталогаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string source = folderBrowserDialog1.SelectedPath;// исходный каталог
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    string target = folderBrowserDialog1.SelectedPath; // целевой каталог
                    target = Path.Combine(target, Path.GetFileName(source));

                    // Копируем корневую папку
                    Directory.CreateDirectory(target);

                    // Копируем структуру каталогов
                    foreach (string dirPath in Directory.GetDirectories(source, "*",
                        SearchOption.AllDirectories))
                        Directory.CreateDirectory(dirPath.Replace(source, target));

                    // Копируем файлы
                    foreach (string newPath in Directory.GetFiles(source, "*.*",
                        SearchOption.AllDirectories))
                        File.Copy(newPath, newPath.Replace(source, target), true);
                }
            }
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.CheckPathExists = false;
            op.CheckFileExists = false; // отключаем проверку существования файла для возможности записать в новый
            if (op.ShowDialog() == DialogResult.OK) //если выбрали файл
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in display) // для всех записей из каталога
                {
                    sb.Append($"{item.FileName}\t{item.MftNumber}\t{item.Clusters}"); // добавляем строку в результат
                    sb.Append("\n");
                }

                File.WriteAllText(op.FileName, sb.ToString()); //сохраняем все строки в файл
            }
        }
    }
}
