using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Hellgate;
using Ionic.Zip;

namespace Launcher.Forms
{
    public partial class Launcher : Form
    {
        FileManager HellgateFileManager { get; set; }

        public Launcher()
        {
            InitializeComponent();
        }

        private void Launcher_Load(object sender, EventArgs e)
        {
            CheckEnvironment();
        }

        private void CheckEnvironment()
        {
            if (Directory.Exists(Config.HglDir)) return;

            String caption = "Installation";
            String message = "Before the Hellgate: Revival Launcher can be used, you must configure the paths.\nPress okay to continue.";
            DialogResult result = MessageBox.Show(message, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.Cancel) return;

            Options optionsForm = new Options();
            optionsForm.ShowDialog(this);
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options optionsForm = new Options();
            optionsForm.ShowDialog(this);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void installToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Modifications|*.mod.zip";
            DialogResult result = fileDialog.ShowDialog();
            if (result == DialogResult.Cancel) return;

            string tempPath = Path.GetTempPath();
            string fileName = Path.GetFileNameWithoutExtension(fileDialog.FileName);
            string extractPath = Path.Combine(tempPath, fileName);
            string installFilePath = Path.Combine(extractPath, "install.xml");

            try
            {
                if (Directory.Exists(extractPath))
                    Directory.Delete(extractPath, true);
                Directory.CreateDirectory(extractPath);
                using (ZipFile zipFile = new ZipFile(fileDialog.FileName))
                {
                    zipFile.ExtractAll(extractPath);
                }
            }
            catch
            {
                string errCaption = "Error";
                string errMessage = "Error extracting modification to temporary directory.";
                MessageBox.Show(errMessage, errCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (File.Exists(installFilePath) == false)
            {
                string errCaption = "Error";
                string errMessage = String.Format("The modification {0} does not contain a install.xml file.", fileName);
                MessageBox.Show(errMessage, errCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {

            }
            catch
            {

            }
        }

        private void revertToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void tradeItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void labelUnleahHell_Click(object sender, EventArgs e)
        {

        }

        private void labelTradeItems_Click(object sender, EventArgs e)
        {

        }
    }
}
