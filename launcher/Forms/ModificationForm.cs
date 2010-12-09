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
using Revival.Common;
using ModData = Revival.Modification.Revival;
using Script = Revival.Modification.Revival.Modification.Script;
using Hellpack = Revival.Hellpack;
using FileEntry = Hellgate.IndexFile.FileEntry;

namespace Launcher.Forms
{
    public partial class ModificationForm : Form
    {
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr h, int msg, int wParam, int[] lParam);
        
        Modification RevivalMod { get; set; }

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
                if (script.Type != "optional" && script.Type != "recommended") continue;
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
            toolStripProgressBar.MarqueeAnimationSpeed = 30;
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            FileManager HellgateFileManager;
            string cookingMessage = "Cooking {0}...";
            string path = RevivalMod.DataPath;
            string[] excelToCook = null;
            string[] stringsToCook = null;
            string[] xmlToCook = null;
            string[] filesToPack = null;


            // Step One: Initialize the FileManager
            toolStripStatusLabel.Text = "Initializing the Hellgate File Manager...";
            HellgateFileManager = new FileManager(Config.HglDir);

            if (HellgateFileManager.HasIntegrity == false)
            {
                Console.WriteLine("Could not initialize the File Manager, Integrity check failed.");
                e.Cancel = true;

                string caption = "Integrity Error";
                string message = "Could not initialize the File Manager. Check Hellgate London is not running and that the installation is not corrupt.";
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                return;
            }

            // Check if the user wants to cancel.
            if (backgroundWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            // Step Two: Revert modifications automatically if flagged in the mod install xml.
            if (RevivalMod.Data.Modifications.DoRevert == true)
            {
                DialogResult dialogResult = DialogResult.Retry;
                while (dialogResult == DialogResult.Retry)
                {
                    toolStripStatusLabel.Text = "Reverting previous Hellgate London modifications...";
                    bool error = Modification.RemoveModifications(HellgateFileManager);
                    if (error == true)
                    {
                        string caption = "Reversion Error";
                        string message = "One or more errors occurred while reverting Hellgate London modifications. Check Hellgate London is not running and the files are not in use. It is highly recommended you ammend this before continuing, even if it means reinstalling the game.";
                        dialogResult = MessageBox.Show(message, caption, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);

                        if (dialogResult == DialogResult.Abort)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                toolStripStatusLabel.Text = "Reloading the Hellgate File Manager...";
                HellgateFileManager.Reload();
            }

            // Check if the user wants to cancel.
            if (backgroundWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }


            // Load the Excel tables
            toolStripStatusLabel.Text = "Caching the Hellgate London excel tables...";
            HellgateFileManager.LoadTableFiles();


            // Step Three: Cook Excel, String and XML files.

            // Search for files to cook
            excelToCook = Hellpack.SearchForExcelFiles(path);
            stringsToCook = Hellpack.SearchForStringFiles(path);
            xmlToCook = Hellpack.SearchForXmlFiles(path);

            if (excelToCook != null)
            {
                for (int i = 0; i < excelToCook.Length; i++)
                {
                    // Check if the user wants to cancel.
                    if (backgroundWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }

                    string epath = excelToCook[i];
                    string hglPath = epath.Substring(epath.IndexOf("data"), epath.Length - epath.IndexOf("data"));
                    string report = String.Format(cookingMessage, hglPath);
                    toolStripStatusLabel.Text = report;
                    Hellpack.CookExcelFile(epath);
                }
            }

            if (stringsToCook != null)
            {
                for (int i = 0; i < stringsToCook.Length; i++)
                {
                    // Check if the user wants to cancel.
                    if (backgroundWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }

                    string epath = stringsToCook[i];
                    string hglPath = epath.Substring(epath.IndexOf("data"), epath.Length - epath.IndexOf("data"));
                    string report = String.Format(cookingMessage, hglPath);
                    toolStripStatusLabel.Text = report;
                    Hellpack.CookStringFile(epath);
                }
            }

            if (xmlToCook != null)
            {
                for (int i = 0; i < xmlToCook.Length; i++)
                {
                    // Check if the user wants to cancel.
                    if (backgroundWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }

                    string epath = xmlToCook[i];
                    string hglPath = epath.Substring(epath.IndexOf("data"), epath.Length - epath.IndexOf("data"));
                    string report = String.Format(cookingMessage, hglPath);
                    toolStripStatusLabel.Text = report;
                    Hellpack.CookXmlFile(epath, HellgateFileManager);
                }
            }

            // Step Four: Pack the base idx/dat.
            filesToPack = Hellpack.SearchForFilesToPack(path, true);
            toolStripStatusLabel.Text = String.Format("Packing {0}...", RevivalMod.Data.Modifications.ID + ".idx");
            string hglDataPath = Path.Combine(Config.HglDir, "data");
            string modDatPath = Path.Combine(hglDataPath, RevivalMod.Data.Modifications.ID) + ".idx";
            bool packResult = Hellpack.PackDatFile(filesToPack, modDatPath);

            if (packResult == false)
            {
                HellgateFileManager.Dispose();

                string caption = "Fatal Error";
                string message = "An error occurred while packing the modification.";
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            // Step Five: Apply scripts. Scripts are only applied if they are checked or are hidden.
            if (optionalCheckedListBox.CheckedItems.Count != 0 ||
                RevivalMod.Data.Modifications.Scripts.Where(s => s.Type == "hidden").Any() == true)
            {
                List<Script> scriptList = new List<Script>();

                // Append optional list
                foreach (Script script in optionalCheckedListBox.CheckedItems)
                {
                    scriptList.Add(script);
                }

                // Append hidden list
                foreach (Script script in RevivalMod.Data.Modifications.Scripts.Where(s => s.Type == "hidden"))
                {
                    scriptList.Add(script);
                }


                // Reinitalize the File Manager
                HellgateFileManager.Reload();
                HellgateFileManager.LoadTableFiles();


                // Apply the scripts
                List<string> modifiedTables = new List<string>();
                foreach (Script script in scriptList)
                {
                    // Interface stuff
                    toolStripStatusLabel.Text =  String.Format("Applying {0} script...", script.Title);
                    if (script.Extraction != null)
                    {
                        toolStripStatusLabel.Text = "MP conversion in progress, this may between 5 and 10 minutes.";
                    }

                    // The script
                    Modification.ApplyScript(script, ref HellgateFileManager);
                    if (script.Tables != null)
                    {
                        foreach (Script.Table table in script.Tables)
                        {
                            string tableID = table.ID.ToUpper();
                            if (modifiedTables.Contains(tableID) == false)
                                modifiedTables.Add(tableID);
                        }
                    }
                }

                // Repack the modified Tables.
                // These go in their own idx/dat under the same name as the base modification with the string _125 appended.
                string indexPath = Path.Combine(Config.HglDir, "data", RevivalMod.Data.Modifications.ID + "_125.idx");
                IndexFile indexFile = new IndexFile() { FilePath = indexPath };
                foreach (string tableID in modifiedTables)
                {
                    DataFile dataTable = HellgateFileManager.DataFiles[tableID];
                    byte[] ebuffer = dataTable.ToByteArray();
                    string fileName = Path.GetFileName(dataTable.FilePath);
                    string fileDir = Path.GetDirectoryName(dataTable.FilePath) + "\\";
                    indexFile.AddFile(fileDir, fileName, ebuffer);
                }

                if (indexFile.Count > 0)
                {
                    // Write the index
                    byte[] ibuffer = indexFile.ToByteArray();
                    Crypt.Encrypt(ibuffer);
                    try
                    {
                        File.WriteAllBytes(indexPath, ibuffer);
                        HellgateFileManager.Dispose();
                        indexFile.EndDatAccess();
                        indexFile.Dispose();
                    }
                    catch(Exception ex)
                    {
                        ExceptionLogger.LogException(ex);
                        HellgateFileManager.Dispose();
                        indexFile.EndDatAccess();
                        indexFile.Dispose();

                        string caption = "Fatal Error";
                        string message = "An error occurred while packing the optional modification.";
                        MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            string msgCaption = "Success";
            string msgDescription = "Modification successfully installed!";
            MessageBox.Show(msgDescription, msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            // Back to normal
            installButton.Text = "Install";
            installButton.Enabled = true;
            toolStripProgressBar.MarqueeAnimationSpeed = 0;
        }
    }
}
