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

        private int FoundSubdir(MFT record, string dir)
        {
            int result = 0;
            var fileNames = record.Indexes.Select(n => n.FileNameString).ToArray();

            foreach (var index in record.Indexes)
            {
                if (index.FileNameString == dir)
                    result = (int)index.IndexedFile;
            }

            return result;
        }

        private void поискToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = folderBrowserDialog1.SelectedPath;
                string[] catalogs = fileName.Split(new char[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries );//Разбиваем полный путь на имена каталогов

                NTFS ntfs = new NTFS(catalogs[0]);

                int nextRecord = 5;
                MFT root;
                for (int i = 1; i < catalogs.Length; i++)
                {
                    root = ntfs.GetMftRecord(nextRecord);
                    nextRecord = FoundSubdir(root, catalogs[i]);
                }

                MFT catalog = ntfs.GetMftRecord(nextRecord);
   display = new List<DisplayMFT>();
                foreach (var index in catalog.Indexes)
                {
                    display.Add(new DisplayMFT(ntfs.GetMftRecord((int)index.IndexedFile)));
                }

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = display;
            }
        }

        private void копированиеКаталогаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string source = folderBrowserDialog1.SelectedPath;
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    string target = folderBrowserDialog1.SelectedPath;
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
            op.CheckFileExists = false;
            if (op.ShowDialog() == DialogResult.OK)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in display)
                {
                    sb.Append($"{item.FileName}\t{item.MftNumber}\t{item.Clusters}");
                    sb.Append("\n");
                }

                File.WriteAllText(op.FileName, sb.ToString());
            }
        }
    }
}
