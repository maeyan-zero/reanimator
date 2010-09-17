using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;

using Hellgate;
using Reanimator.Forms;

namespace Reanimator
{
    public partial class Reanimator : Form
    {
        private int childFormNumber = 0;
        private FileManager FileManager { get; set; }

        private ExplorerForm ExplorerForm { get; set; }
        private List<ExcelForm> ExcelForms { get; set; }

        private String DefaultMessage { get { return "Reanimator (c) 2009-2010. www.hellgateaus.net"; } }

        public Reanimator()
        {
            InitializeComponent();
            Thread splash = new Thread(new ThreadStart(DoSplash));
            splash.Start();
            Thread.Sleep(3000);
            splash.Abort();
            Thread.Sleep(1000);
        }

        private void DoSplash()
        {
            SplashForm splashForm = new SplashForm();
            splashForm.ShowDialog();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Config.HellgatePath;
            openFileDialog.Filter = "Hellgate London FileList|*.idx;*.txt.cooked|" +
                                    "Parent FileList (*.idx)|*.idx|" +
                                    "Cooked Excel FileList (*.txt.cooked)|*.txt.cooked|" +
                                    "All FileList (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
                string FileExtension = Path.GetExtension(FileName);

                switch (FileExtension)
                {
                    case ".idx":

                        break;
                    case ".cooked":
                        ExcelFile excelFile = new ExcelFile(File.ReadAllBytes(FileName));
                        if (excelFile.IntegrityCheck == true)
                        {
                            //ExcelForm excelForm = new ExcelForm(excelFile);
                            //excelForm.MdiParent = this;
                            //excelForm.Show();
                        }
                        break;
                }
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text FileList (*.txt)|*.txt|All FileList (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsForm options = new OptionsForm();
            options.ShowDialog();
        }

        private void Reanimator_ResizeEnd(object sender, EventArgs e)
        {
            Config.ClientWidth = this.Width;
            Config.ClientHeight = this.Height;
        }

        public void Status_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBar.Value = e.ProgressPercentage;
            if (e.UserState != null)
            {
                String message = e.UserState as String;
                toolStripStatusLabel.Text = message ?? String.Empty;
            }
        }

        public void Status_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripStatusLabel.Text = DefaultMessage;
            toolStripProgressBar.Value = 0;
            toolStripStatusCancel.Visible = false;

            if (!(e.Error == null))
            {
                MessageBox.Show("Sorry, an error has occured. Check the Hellgate files arn't corrupt or in use.",
                    "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (e.Cancelled)
            {
                MessageBox.Show("The operation was cancelled.",
                    "Cancelled Message", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                MessageBox.Show("The operation has successfully completed.",
                    "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }
        private void Reanimator_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel.Text = DefaultMessage;

            Width = Config.ClientWidth;
            Height = Config.ClientHeight;

            // First time users.
            if (!Directory.Exists(Config.HellgatePath))
            {
                String message = "Your Hellgate London directory is not currently set.\n" +
                     "Some of Reanimators main features will not be enabled until its set.\n" +
                     "Please specify the location in the Reanimator program options (Tools -> Options).\n\n" +

                     "Would you like to set your program options now?";

                if (MessageBox.Show(message, "Hellgate London not found.",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    optionsToolStripMenuItem_Click(this, null);
                }
                else
                {
                    return;
                }
            }

            if (Config.LoadIndexFilesOnStartup)
            {
                ExplorerToolStripMenuItem_Click(this, null);
            }

            if (Config.LoadExcelFilesOnStartup)
            {
                //backgroundWorker.DoWork += new DoWorkEventHandler(HellgateFileManager.LoadIndexFiles);
            }
        }

        private void ExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FileManager == null)
            {
                FileManager = new FileManager(Config.HellgatePath);
            }
            if (!(FileManager.Initialized))
            {
                if (!(FileManager.LoadIndexFiles()))
                {
                    MessageBox.Show("There is a problem loading the Entry Manger.\n" +
                        "Please check Hellgate London is not currently open and that your Hellgate files are not corrupt.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                    explorerToolStripMenuItem.Checked = false;
                    return;
                }
            }

            if (ExplorerForm == null)
            {
                ExplorerForm = new ExplorerForm(FileManager)
                {
                    MdiParent = this,
                    Dock = DockStyle.Left
                };
                ExplorerForm.Show();
                explorerToolStripMenuItem.Checked = true;
            }
            else
            {
                ExplorerForm.Visible = explorerToolStripMenuItem.Checked;
            }
        }

        private void toolStripStatusCancel_Click(object sender, EventArgs e)
        {
            ExplorerForm.CancelExtraction();
            toolStripStatusCancel.Visible = false;
        }

        public void EnableCancelButton()
        {
            toolStripStatusCancel.Visible = true;
        }
    }
}