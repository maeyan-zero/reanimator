using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using Hellgate;
using Revival;
using Ionic.Zip;
using FileEntry = Hellgate.IndexFile.FileEntry;
using System.Threading;
using System.Globalization;
using Config = Revival.Common.Config;

namespace Launcher.Forms
{
    public partial class Launcher : Form
    {
        FileManager HellgateFileManager { get; set; }
        FileStream fileStream { get; set; } // Console loggin
        StreamWriter streamWriter { get; set; }

        public Launcher()
        {
            InitializeComponent();
        }

        private void Launcher_Load(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            CheckEnvironment();
            StartConsoleLogging();
        }

        private void StartConsoleLogging()
        {
            try
            {
                fileStream = new FileStream("./output.txt", FileMode.OpenOrCreate, FileAccess.Write);
                streamWriter = new StreamWriter(fileStream);
            }
            catch (Exception ex)
            {
                Revival.Common.ExceptionLogger.LogException(ex);
                return;
            }
            Console.SetOut(streamWriter);
        }

        private void EndConsoleLogging()
        {
            streamWriter.Close();
            fileStream.Close();
        }

        private void CheckEnvironment()
        {
            if (Directory.Exists(Config.HglDir) == false)
            {
                String caption = "Installation";
                String message = "Before the Hellgate: Revival Launcher can be used, you must configure the paths.\nPress okay to continue.";
                DialogResult result = MessageBox.Show(message, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.Cancel) return;

                OptionsForm optionsForm = new OptionsForm();
                optionsForm.ShowDialog(this);
            }

            if (File.Exists(Config.GameClientPath) == false)
            {
                Config.GameClientPath = Path.Combine(Config.HglDir, @"SP_x86\hellgate_sp_dx9_x86.exe");
            }
            if (File.Exists(Config.GameClientPath) == false)
            {
                Config.GameClientPath = Path.Combine(Config.HglDir, @"SP_x64\hellgate_sp_dx9_x64.exe");
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsForm optionsForm = new OptionsForm();
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
            string extractPath = Path.Combine(tempPath, fileName + DateTime.Now.ToFileTime().ToString());
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
            DialogResult dialogResult;
            string caption = "Revert";
            string message = "Are you sure you want to remove all modifications?";
            dialogResult = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                backgroundWorker.DoWork += new DoWorkEventHandler(bw_DoWorkRevert);
                backgroundWorker.RunWorkerAsync();
            }
        }

        private void tradeItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Coming Soon...");
        }

        private void labelUnleahHell_Click(object sender, EventArgs e)
        {
            if (File.Exists(Config.GameClientPath))
            {
                Process process = new Process();
                //process.StartInfo.Arguments = String.Format("-load\"{0}\"", @"C:\Users\Administrator\Documents\My Games\Hellgate\Save\Singleplayer\Samm.hg1");
                process.StartInfo.FileName = Config.GameClientPath;
                process.Start();
                WindowState = FormWindowState.Minimized;
            }
            else
            {
                string errCaption = "File not found";
                string errMessage = String.Format("The file {0} does not exist, please check the program options and try again.", Config.GameClientPath);
                MessageBox.Show(errMessage, errCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void labelTradeItems_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Coming Soon...");
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

        private void label1_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.hellgateaus.net");
            WindowState = FormWindowState.Minimized;
        }

        private void bw_DoWorkRevert(object sender, DoWorkEventArgs e)
        {
            HellgateFileManager = new FileManager(Config.HglDir);
            if (HellgateFileManager.HasIntegrity == false)
            {
                string errCaption = "Error";
                string errMessage = "Could not initialize the File Manager. The Hellgate London directory is not set correctly or the installation is damaged beyond repair.";
                MessageBox.Show(errMessage, errCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool errorOccurred = false;

            // Remove the backup prefix, used in pre 1.5 patches or Reanimator/Hellzap
            foreach (IndexFile indexFile in HellgateFileManager.IndexFiles)
            {
                bool isModified = false;
                foreach (FileEntry fileEntry in indexFile.Files)
                {
                    if (fileEntry.IsPatchedOut)
                    {
                        fileEntry.IsPatchedOut = false;
                        isModified = true;
                    }
                }
                if (isModified)
                {
                    byte[] ibuffer = indexFile.ToByteArray();
                    Crypt.Encrypt(ibuffer);
                    try
                    {
                        File.WriteAllBytes(indexFile.FilePath, ibuffer);
                    }
                    catch
                    {
                        errorOccurred = true;
                    }
                }
            }

            // Delete all dats the arn't original
            string hellgateDatPath = Path.Combine(Config.HglDir, "data");
            List<string> hellgateDats = new List<string>();
            hellgateDats.AddRange(Directory.GetFiles(hellgateDatPath, "*.idx"));
            hellgateDats.AddRange(Directory.GetFiles(hellgateDatPath, "*.dat"));
            foreach (string datPath in hellgateDats)
            {
                string fileName = Path.GetFileName(datPath);
                if (Common.OriginalDats.Where(dat => String.Format("{0}.idx", dat) == fileName ||
                                                      String.Format("{0}.dat", dat) == fileName).Any() == false)
                {
                    try
                    {
                        File.Delete(datPath);
                    }
                    catch
                    {
                        errorOccurred = true;
                    }
                }
            }

            string captionMsg = "Revert Complete";
            string revertMsg = errorOccurred ? "Revert complete but with errors." : "Revert successfully complete";
            MessageBox.Show(revertMsg, captionMsg, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Launcher_FormClosing(object sender, FormClosingEventArgs e)
        {
            EndConsoleLogging();
        }
    }
}
