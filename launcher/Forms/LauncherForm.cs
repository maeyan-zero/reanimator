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
using Revival;
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
            string fileName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(fileDialog.FileName));
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

            Modification modification = new Modification(installFilePath);
            if (modification.IntegrityCheck == false)
            {
                string errCaption = "Error";
                string errMessage = "The modification install.xml contains bad syntax, could not install.";
                MessageBox.Show(errMessage, errCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ModificationForm modificationForm = new ModificationForm(modification);
            modificationForm.Show(this);
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

        private void label_MouseEnter(object sender, EventArgs e)
        {
            Label label = sender as Label;
            if (label == null) return;

            Font prototype = label.Font;
            label.Font = new Font(prototype, FontStyle.Underline);

            if (label.Text == "Unleash Hell")
            {
                toolStripStatusLabel1.Text = "Launch Hellgate London: " + Path.GetFileName(Config.GameClientPath);
            }
            if (label.Text == "Trade Items")
            {
                toolStripStatusLabel1.Text = "Trade Items between your characters.";
            }
            if (label.Text == "Visit Website")
            {
                toolStripStatusLabel1.Text = "Join us at www.hellgateaus.net!";
            }
        }

        private void label_MouseLeave(object sender, EventArgs e)
        {
            Label label = sender as Label;
            if (label == null) return;

            Font prototype = label.Font;
            label.Font = new Font(prototype, FontStyle.Regular);

            toolStripStatusLabel1.Text = String.Empty;
        }
    }
}
