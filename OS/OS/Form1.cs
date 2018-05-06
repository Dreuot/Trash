using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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
                string[] catalogs = fileName.Split(new char[] { Path.DirectorySeparatorChar});

                NTFS ntfs = new NTFS(catalogs[0]);
                MFT root = ntfs.ReturnMFTRecord(5);
                var x = FoundSubdir(root, catalogs[1]);
            }
        }

        private ulong FoundSubdir(MFT record, string dir)
        {
            ulong result = 0;
            foreach (var index in record.Indexes)
            {
                if (index.FileNameString == dir)
                    result = index.IndexedFile;
            }

            return result;
        }
    }
}
