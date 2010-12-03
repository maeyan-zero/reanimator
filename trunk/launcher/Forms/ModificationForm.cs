using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hellgate;
using Revival;
using ModData = Revival.Modification.Revival;
using Script = Revival.Modification.Revival.Modification.Script;
using Hellpack = Revival.Hellpack;

namespace Launcher.Forms
{
    public partial class ModificationForm : Form
    {
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr h, int msg, int wParam, int[] lParam);
        
        Modification RevivalMod { get; set; }
        FileManager HellgateFileManager { get; set; }

        public ModificationForm(Modification modification)
        {
            InitializeComponent();
            
            RevivalMod = modification;

            // Set tab size
            int EM_SETTABSTOPS = 0x00CB;
            Graphics graphics = releaseNotesTextBox.CreateGraphics();
            int characterWidth = (int)graphics.MeasureString("M", releaseNotesTextBox.Font).Width;
            int tabWidth = 1;
            SendMessage(releaseNotesTextBox.Handle, EM_SETTABSTOPS, 1, new int[] { tabWidth * characterWidth });
        }

        private void Modification_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel.Text = String.Empty;
            titleLabel.Text = RevivalMod.Data.Modifications.Release;
            versionLabel.Text = RevivalMod.Data.Modifications.Version;
            authorLabel.Text = RevivalMod.Data.Modifications.Author;
            websiteLabel.Text = RevivalMod.Data.Modifications.Website;
            releaseNotesTextBox.Text = RevivalMod.Data.Modifications.Description.Replace("\n", Environment.NewLine);

            string iconPath = Path.Combine(RevivalMod.DataPath, "icon.png");
            if (File.Exists(iconPath))
            {
                pictureBox.Image = Image.FromFile(iconPath);
            }

            foreach (Script script in RevivalMod.Data.Modifications.Scripts)
            {
                if (script.Type == "hidden") continue;
                optionalCheckedListBox.Items.Add(script, script.Type == "recommended" ? true : false);
            }
        }

        private void optionalCheckedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = optionalCheckedListBox.SelectedIndex;
            optionalTextBox.Text = ((Script)optionalCheckedListBox.Items[i]).Description;
        }

        private void installButton_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                DialogResult dialogResult;
                string caption = "Cancellation";
                string message = "Are you sure you want to cancel the installation?";
                dialogResult = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
                if (dialogResult == DialogResult.Yes)
                {
                    backgroundWorker.CancelAsync();
                    installButton.Enabled = false;
                }
                return;
            }
            // Check dependencies
            string dataPath = Path.Combine(Config.HglDir, "data");
            foreach (string dependency in RevivalMod.Data.Modifications.Dependencies.Patch)
            {
                if (Directory.GetFiles(dataPath).Where(str => Path.GetFileNameWithoutExtension(str) == dependency).Any() == false)
                {
                    string errCaption = "Error";
                    string errMessage = String.Format("Can not install, dependency {0} is missing. Please visit www.hellgateaus.net for more information.", dependency);
                    MessageBox.Show(errMessage, errCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            backgroundWorker.DoWork += new DoWorkEventHandler(bw_DoWork);
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            backgroundWorker.RunWorkerAsync();

            installButton.Text = "Cancel";
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            string cookingMessage = "Cooking {0}...";
            string path = RevivalMod.DataPath;
            string[] excelToCook = null;
            string[] stringsToCook = null;
            string[] xmlToCook = null;
            string[] filesToPack = null;

            // There is a check box that will allow the main mod compilation to be skipped
            if (optionalOnlycheckBox.Checked == false)
            {
                // Search for files to cook
                excelToCook = Hellpack.SearchForExcelFiles(path);
                stringsToCook = Hellpack.SearchForStringFiles(path);
                xmlToCook = Hellpack.SearchForXmlFiles(path);

                int cookingProgress = 1;
                int totalFilesToCook = 0;
                totalFilesToCook += excelToCook != null ? excelToCook.Length : 0;
                totalFilesToCook += stringsToCook != null ? stringsToCook.Length : 0;
                totalFilesToCook += xmlToCook != null ? xmlToCook.Length : 0;

                // Cook files
                // Excel
                if (excelToCook != null)
                {
                    for (int i = 0; i < excelToCook.Length; i++)
                    {
                        if (backgroundWorker.CancellationPending)
                        {
                            e.Cancel = true;
                            return;
                        }
                        string epath = excelToCook[i];
                        string hglPath = epath.Substring(epath.IndexOf("data"), epath.Length - epath.IndexOf("data"));
                        string reportMsg = String.Format(cookingMessage, hglPath);
                        int progress = ((cookingProgress * 100) / totalFilesToCook);
                        backgroundWorker.ReportProgress(progress, reportMsg);
                        Hellpack.CookExcelFile(epath);
                        cookingProgress++;
                    }
                }

                // Strings
                if (stringsToCook != null)
                {
                    for (int i = 0; i < stringsToCook.Length; i++)
                    {
                        if (backgroundWorker.CancellationPending)
                        {
                            e.Cancel = true;
                            return;
                        }
                        string epath = stringsToCook[i];
                        string hglPath = epath.Substring(epath.IndexOf("data"), epath.Length - epath.IndexOf("data"));
                        string reportMsg = String.Format(cookingMessage, hglPath);
                        int progress = ((cookingProgress * 100) / totalFilesToCook);
                        backgroundWorker.ReportProgress(progress, reportMsg);
                        Hellpack.CookStringFile(epath);
                        cookingProgress++;
                    }
                }

                // XML
                if (xmlToCook != null)
                {
                    if (HellgateFileManager == null && xmlToCook.Length > 0)
                    {
                        HellgateFileManager = new FileManager(Config.HglDir);
                        if (backgroundWorker.CancellationPending)
                        {
                            e.Cancel = true;
                            return;
                        }
                        HellgateFileManager.LoadTableFiles();
                    }
                    for (int i = 0; i < xmlToCook.Length; i++)
                    {
                        if (backgroundWorker.CancellationPending)
                        {
                            e.Cancel = true;
                            return;
                        }
                        string epath = xmlToCook[i];
                        string hglPath = epath.Substring(epath.IndexOf("data"), epath.Length - epath.IndexOf("data"));
                        string reportMsg = String.Format(cookingMessage, hglPath);
                        int progress = ((cookingProgress * 100) / totalFilesToCook);
                        backgroundWorker.ReportProgress(progress, reportMsg);
                        Hellpack.CookXmlFile(epath, HellgateFileManager);
                        cookingProgress++;
                    }
                }

                // Search for files to pack
                backgroundWorker.ReportProgress(100, "Searching for files to pack...");
                filesToPack = Hellpack.SearchForFilesToPack(path, true);

                // Pack files
                backgroundWorker.ReportProgress(100, String.Format("Packing {0}...", RevivalMod.Data.Modifications.ID + ".idx"));
                string hglDataPath = Path.Combine(Config.HglDir, "data");
                string datPath = Path.Combine(hglDataPath, RevivalMod.Data.Modifications.ID) + ".idx";
                bool packResult = Hellpack.PackDatFile(filesToPack, datPath);

                if (packResult == false)
                {
                    string message = "An error occurred while packing the modification.";
                    MessageBox.Show(message);
                    return;
                }
            }

            // Apply scripts
            if (optionalCheckedListBox.CheckedItems.Count == 0 &&
                RevivalMod.Data.Modifications.Scripts.Where(s => s.Type == "hidden").Any() == false)
            {
                return; // no scripts
            }

            backgroundWorker.ReportProgress(0, String.Format("Initializing the File Manager..."));
            if (HellgateFileManager == null)
            {
                HellgateFileManager = new FileManager(Config.HglDir);
            }
            else
            {
                HellgateFileManager.Reload();
            }
            HellgateFileManager.LoadTableFiles();

            int scriptCount = optionalCheckedListBox.CheckedItems.Count;
            int scriptProgress = 1;

            List<string> modifiedTables = new List<string>();
            foreach (Script script in optionalCheckedListBox.CheckedItems)
            {
                int progress = (scriptProgress * 100) / scriptCount;
                backgroundWorker.ReportProgress(progress, String.Format("Applying {0} script...", script.Title));
                Modification.ApplyScript(script, HellgateFileManager);
                foreach (Script.Table table in script.Tables)
                {
                    modifiedTables.Add(table.ID.ToUpper());
                }
            }

            // Repack the modified Tables
            string indexPath = Path.Combine(Config.HglDir, "data", RevivalMod.Data.Modifications.ID + "_125.idx");
            IndexFile indexFile = new IndexFile() { FilePath = indexPath };
            foreach (string tableID in modifiedTables)
            {
                DataFile dataTable = HellgateFileManager.DataFiles[tableID];
                byte[] ebuffer = dataTable.ToByteArray();
                indexFile.AddFile(dataTable.FileName, Path.GetDirectoryName(dataTable.FilePath), ebuffer);
            }

            byte[] ibuffer = indexFile.ToByteArray();
            Crypt.Encrypt(ibuffer);
            try
            {
                File.WriteAllBytes(indexPath, ibuffer);
            }
            catch
            {

            }
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBar.Value = e.ProgressPercentage;
            toolStripStatusLabel.Text = e.UserState as string ?? toolStripStatusLabel.Text;
        }


        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                toolStripStatusLabel.Text = "Canceled!";
            }
            else if (!(e.Error == null))
            {
                toolStripStatusLabel.Text = ("Error: " + e.Error.Message);
            }
            else
            {
                toolStripStatusLabel.Text = "Done!";
            }
            installButton.Text = "Install";
            installButton.Enabled = true;
            toolStripProgressBar.Value = 0;
        }
    }
}
