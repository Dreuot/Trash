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
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = folderBrowserDialog1.SelectedPath;
                string[] catalogs = fileName.Split(new char[] { Path.DirectorySeparatorChar});//Разбиваем полный путь на имена каталогов

                NTFS ntfs = new NTFS(catalogs[0]);

                int nextRecord = 5;
                MFT root;
                for (int i = 1; i < catalogs.Length; i++)
                {
                    root = ntfs.ReturnMFTRecord(nextRecord);
                    nextRecord = FoundSubdir(root, catalogs[i]);
                }

                MFT catalog = ntfs.ReturnMFTRecord(nextRecord);
            }
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
    }
}
