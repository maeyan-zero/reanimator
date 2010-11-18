using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Reanimator.Forms
{
    public partial class CacheInfo : Form
    {
        string cachePath;
        FileInfo[] info;

        public CacheInfo(string cachePath)
        {
            InitializeComponent();

            this.cachePath = cachePath;

            UpdateList();
        }

        private void b_update_Click(object sender, EventArgs e)
        {
            UpdateList();
        }

        private void UpdateList()
        {
            string[] files = Directory.GetFiles(cachePath);
            info = new FileInfo[files.Length];
            long size = 0;

            for (int counter = 0; counter < files.Length; counter++)
            {
                FileInfo tmp = new FileInfo(files[counter]);
                info[counter] = tmp;
                size += tmp.Length;
            }

            lb_cachedFiles.DataSource = info;
            l_fileNumber.Text = files.Length.ToString();
            l_totalSize.Text = string.Format("{0:0,000}", size / 1024) + " KB";
        }
    }
}
