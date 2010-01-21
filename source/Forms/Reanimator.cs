using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Reanimator.Forms;
using Reanimator.Excel;
using System.Threading;
using System.Runtime.InteropServices;

namespace Reanimator
{
    public partial class Reanimator : Form
    {
        private Options options;
        private Config config;
        private List<string> indexFilesOpen;
        private ExcelTables excelTables;

        private int childFormNumber = 0;
  
        public Reanimator()
        {
            config = new Config("config.xml");
            options = new Options(config);
            indexFilesOpen = new List<string>();
               
            InitializeComponent();
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
            openFileDialog.Filter = "HGL Files (*.idx, *.hg1, *.cooked)|*.idx;*.hg1;*.cooked|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                if (openFileDialog.FileName.EndsWith("idx"))
                {
                    OpenFile_IDX(openFileDialog.FileName);
                }
                else if (openFileDialog.FileName.EndsWith("hg1"))
                {
                    OpenFile_HG1(openFileDialog.FileName);
                }
                else if (openFileDialog.FileName.EndsWith("cooked"))
                {
                    OpenFile_COOKED(openFileDialog.FileName);
                }
            }

        }

        private void OpenIndexFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Index Files (*.idx)|*.idx|All Files (*.*)|*.*";
            openFileDialog.InitialDirectory = config["hglDir"] + "\\Data";

            if (openFileDialog.ShowDialog(this) == DialogResult.OK && openFileDialog.FileName.EndsWith("idx"))
            {
                OpenFile_IDX(openFileDialog.FileName);
            }
        }

        private void OpenCharacterFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Character Files (*.hg1)|*.hg1|All Files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Games\\Hellgate\\Save\\Singleplayer";

            if (openFileDialog.ShowDialog(this) == DialogResult.OK && openFileDialog.FileName.EndsWith("hg1"))
            {
                OpenFile_HG1(openFileDialog.FileName);
            }
        }

        private void OpenCookedFile(object sender, EventArgs e)
        {
            MessageBox.Show("Testing function! Must open exceltablex.txt.cooked!\n I'd recommend extracting all dat files and keeping the dir structure for it to work correctly (this will be fixed in time).", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Cooked Files (*.cooked)|*.cooked|All Files (*.*)|*.*";
            openFileDialog.InitialDirectory = config["dataDir"];

            if (openFileDialog.ShowDialog(this) == DialogResult.OK && openFileDialog.FileName.EndsWith("cooked"))
            {
                OpenFile_COOKED(openFileDialog.FileName);
            }
        }

        private bool OpenFile_IDX(string szFileName)
        {
            if (indexFilesOpen.Contains(szFileName))
            {
                return false;
            }

            FileStream indexFile;
            try
            {
                indexFile = new FileStream(szFileName, FileMode.Open);
            }
            catch (Exception)
            {
                return false;
            }

            Index index = new Index(FileTools.StreamToByteArray(indexFile));
            IndexExplorer indexExplorer = new IndexExplorer(indexFile, index);
            indexExplorer.dataGridView.DataSource = index.GetFileTable();
            indexExplorer.Text += ": " + szFileName;
            indexExplorer.MdiParent = this;
            indexExplorer.Show();
            indexExplorer.FormClosed += new FormClosedEventHandler(indexExplorer_FormClosed);

            indexFilesOpen.Add(indexFile.Name);

            return true;
        }

        private void OpenFile_HG1(string szFileName)
        {
            if (excelTables == null)
            {
                MessageBox.Show("You must open the exceltable.txt.cooked file before viewing a character (dirty test implementation requirement)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FileStream heroFile = new FileStream(szFileName, FileMode.Open);
            HeroUnit heroUnit = new HeroUnit(FileTools.StreamToByteArray(heroFile));
            HeroEditor heroEditor = new HeroEditor(heroUnit, excelTables);
            heroEditor.Text = "Hero Editor: " + szFileName;
            heroEditor.MdiParent = this;
            heroEditor.Show();
        }

        private void OpenFile_COOKED(string szFileName)
        {
            MessageBox.Show("Todo");
        }

        private void indexExplorer_FormClosed(object sender, FormClosedEventArgs e)
        {
            IndexExplorer indexExplorer = (IndexExplorer)sender;
            indexFilesOpen.Remove(indexExplorer.IndexFile.Name);
            indexExplorer.IndexFile.Dispose();
            indexExplorer.Dispose();
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

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

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
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
            options.ShowDialog(this);
            this.BringToFront();
        }

        private void clientPatcherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "EXE Files (*.exe)|*.exe|All Files (*.*)|*.*";
            openFileDialog.InitialDirectory = config["hglDir"] + "\\SP_x64";
            if (openFileDialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            FileStream clientFile;
            try
            {
                clientFile = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.ReadWrite);
            }
            catch (Exception)
            {
                return;
            }

            ClientPatcher clientPatcher = new ClientPatcher(FileTools.StreamToByteArray(clientFile));
            if (clientPatcher.ApplyHardcorePatch())
            {
                FileStream fileOut = new FileStream(openFileDialog.FileName + ".patched.exe", FileMode.Create);
                fileOut.Write(clientPatcher.Buffer, 0, clientPatcher.Buffer.Length);
                fileOut.Dispose();
                MessageBox.Show("Hardcore patch applied!");
            }
            else
            {
                MessageBox.Show("Failed to apply Hardcore patch!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            clientFile.Dispose();
        }

        private void LoadExcelTables(object sender, EventArgs e)
        {
            Progress progress = (Progress)sender;

            FileStream excelFile = new FileStream(config["dataDir"] + "\\data_common\\excel\\exceltables.txt.cooked", FileMode.Open);
            excelTables = new ExcelTables(FileTools.StreamToByteArray(excelFile));
            progress.ConfigBar(0, excelTables.Count, 1);
            progress.SetLoadingText("Loading in excel tables (" + excelTables.Count + ")...");
            excelTables.LoadTables(config["dataDir"] + "\\data_common\\excel\\", progress);
            progress.Dispose();
        }

        // this fixes a weird windows API bug causing the ShowDialog to minimise the main client
        private void Reanimator_MouseClick(object sender, MouseEventArgs e)
        {
            this.Focus();
        }

        private void Reanimator_Load(object sender, EventArgs e)
        {
            this.Show();
            this.Refresh();
            // this fixes a weird windows API bug causing the ShowDialog to minimise the main client
            this.OnMouseClick(new MouseEventArgs(MouseButtons.Left, 1, this.Left+10, this.Top+10, 0));

            Progress progress = new Progress();
            progress.Activated += new EventHandler(LoadExcelTables);
            progress.ShowDialog(this);
        }
    }
}