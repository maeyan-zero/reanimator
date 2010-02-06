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
using PluginInterface;
using System.Reflection;

namespace Reanimator
{
    public partial class Reanimator : Form, IPluginHost
    {
        #region PLUGINS



        bool showInitializationMessage = false;
        private List<IPlugin> pluginList;

        public bool Register(IPlugin ipi)
        {
            pluginList.Add(ipi);
            return true;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public void ShowMessage(string message, string title)
        {
            MessageBox.Show(message, title);
        }

        public void LoadPlugins()
        {
            string path = Application.StartupPath + @"\Plugins\";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string[] pluginFiles = Directory.GetFiles(path, "*.dll");

            for (int i = 0; i < pluginFiles.Length; i++)
            {
                string args = pluginFiles[i].Substring(
                  pluginFiles[i].LastIndexOf("\\") + 1,
                  pluginFiles[i].IndexOf(".dll") -
                  pluginFiles[i].LastIndexOf("\\") - 1);

                Type ObjType = null;
                // load the dll
                try
                {
                    // load it
                    Assembly ass = null;
                    ass = Assembly.LoadFile(pluginFiles[i]);
                    if (ass != null)
                    {
                        ObjType = ass.GetType(args + ".PlugIn");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                try
                {
                    // OK Lets create the object as we have the Report Type
                    if (ObjType != null)
                    {
                        IPlugin plugin = (IPlugin)Activator.CreateInstance(ObjType);
                        plugin.Parent = this;
                        plugin.Host = this;
                        plugin.HostMenu = this.menuStrip;
                        plugin.InitializePlugIn(showInitializationMessage);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public IPlugin[] GetPluginList()
        {
            return pluginList.ToArray();
        }

        public string GetHGLDirectory()
        {
            return Config.hglDir;
        }

        public string GetRootDir()
        {
            return Config.dataDirsRoot;
        }

        public string GetClientDir()
        {
            return Config.gameClientPath;
        }

        public ExcelTables GetExcelTables()
        {
            return excelTables;
        }




        #endregion

        private Options options;
        private List<string> indexFilesOpen;
        private ExcelTables excelTables;
        private ExcelTablesLoaded excelTablesLoaded;

        private int childFormNumber = 0;

        public Reanimator()
        {
            options = new Options();
            indexFilesOpen = new List<string>();

            #region PLUGIN
            pluginList = new List<IPlugin>();
            #endregion

            InitializeComponent();

            excelTablesLoaded = new ExcelTablesLoaded();
            excelTablesLoaded.Text = "Currently Loaded Excel Tables";
            excelTablesLoaded.MdiParent = this;
            excelTablesLoaded.Show();
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
                else if (openFileDialog.FileName.EndsWith("xls.uni.cooked"))
                {
                    OpenFile_STRINGS(openFileDialog.FileName);
                }
                else if (openFileDialog.FileName.EndsWith("cooked"))
                {
                    OpenFile_COOKED(openFileDialog.FileName);
                }
                else if (openFileDialog.FileName.EndsWith("mod") || openFileDialog.FileName.EndsWith("xml"))
                {
                    OpenFile_MOD(openFileDialog.FileName);
                }
            }

        }

        private void OpenIndexFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Index Files (*.idx)|*.idx|All Files (*.*)|*.*";
            openFileDialog.InitialDirectory = Config.hglDir + "\\Data";

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
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Cooked Files (*.cooked)|*.cooked|All Files (*.*)|*.*";
            openFileDialog.InitialDirectory = Config.dataDirsRoot;

            if (openFileDialog.ShowDialog(this) == DialogResult.OK && openFileDialog.FileName.EndsWith("cooked"))
            {
                OpenFile_COOKED(openFileDialog.FileName);
            }
        }

        private void StringsFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Strings Files (*.xls.uni.cooked)|*.xls.uni.cooked|All Files (*.*)|*.*";
            openFileDialog.InitialDirectory = Config.dataDirsRoot;

            if (openFileDialog.ShowDialog(this) == DialogResult.OK && openFileDialog.FileName.EndsWith("cooked"))
            {
                OpenFile_STRINGS(openFileDialog.FileName);
            }
        }

        private void OpenModFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Modification Files (*.mod, *.xml)|*.mod;*.xml|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog(this) == DialogResult.OK && (openFileDialog.FileName.EndsWith("mod") || openFileDialog.FileName.EndsWith("xml")))
            {
                OpenFile_MOD(openFileDialog.FileName);
            }
        }

        private bool OpenFile_MOD(string szFileName)
        {
            if (indexFilesOpen.Contains(szFileName))
            {
                return false;
            }

            try
            {
                Mod.Parse(szFileName);
                return true;
            }
            catch(Exception)
            {
                return false;
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
            catch (Exception e)
            {
                MessageBox.Show("Failed to open file: " + szFileName + "\n\n" + e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            Index index = new Index(indexFile);
            TableForm indexExplorer = new TableForm(index);
            indexExplorer.dataGridView.DataSource = index.GetFileTable();
            indexExplorer.Text += ": " + szFileName;
            indexExplorer.MdiParent = this;
            indexExplorer.Show();

            indexFilesOpen.Add(indexFile.Name);

            return true;
        }

        private bool OpenFile_HG1(string fileName)
        {
            if (excelTables == null)
            {
                MessageBox.Show("You must open the exceltable.txt.cooked file before viewing a character (dirty test implementation requirement)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            FileStream heroFile;
            try
            {
                heroFile = new FileStream(fileName, FileMode.Open);
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to open file: " + fileName + "\n\n" + e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            BitBuffer bitBuffer = new BitBuffer(FileTools.StreamToByteArray(heroFile));
            bitBuffer.DataByteOffset = 0x2028;

            Unit heroUnit = new Unit(bitBuffer);
            heroUnit.ReadUnit(ref heroUnit);

            HeroEditor heroEditor = new HeroEditor(heroUnit, excelTables, fileName);
            heroEditor.Text = "Hero Editor: " + fileName;
            heroEditor.MdiParent = this;
            heroEditor.Show();

            return true;
        }

        private void OpenFile_COOKED(String fileName)
        {
            int indexStart = fileName.LastIndexOf("\\") + 1;
            int indexEnd = fileName.LastIndexOf(".txt");
            string name = fileName.Substring(indexStart, indexEnd - indexStart);

            ExcelTable excelTable = excelTables.GetTable(name);
            if (excelTable == null)
            {
                return;
            }

            ExcelTableForm etf = new ExcelTableForm(excelTable);
            etf.Text = "Excel Table: " + fileName;
            etf.MdiParent = this;
            etf.Show();
        }

        private void OpenFile_STRINGS(String fileName)
        {
            try
            {
                FileStream stringsFile = new FileStream(fileName, FileMode.Open);
                Strings strings = new Strings(FileTools.StreamToByteArray(stringsFile));
                TableForm indexExplorer = new TableForm(strings);
                Strings.StringBlock[] stringBlocks = strings.GetFileTable();
                indexExplorer.dataGridView.DataSource = stringBlocks;
                indexExplorer.Text += ": " + fileName;
                indexExplorer.MdiParent = this;
                indexExplorer.Show();
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to open file!\n\n" + e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        }

        #region CONVERTED TO PLUGIN
        //private void clientPatcherToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    openFileDialog.Filter = "EXE Files (*.exe)|*.exe|All Files (*.*)|*.*";
        //    openFileDialog.InitialDirectory = Config.hglDir + "\\SP_x64";
        //    if (openFileDialog.ShowDialog(this) != DialogResult.OK)
        //    {
        //        return;
        //    }

        //    FileStream clientFile;
        //    try
        //    {
        //        clientFile = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.ReadWrite);
        //    }
        //    catch (Exception)
        //    {
        //        return;
        //    }

        //    ClientPatcher clientPatcher = new ClientPatcher(FileTools.StreamToByteArray(clientFile));
        //    if (clientPatcher.ApplyHardcorePatch())
        //    {
        //        FileStream fileOut = new FileStream(openFileDialog.FileName + ".patched.exe", FileMode.Create);
        //        fileOut.Write(clientPatcher.Buffer, 0, clientPatcher.Buffer.Length);
        //        fileOut.Dispose();
        //        MessageBox.Show("Hardcore patch applied!");
        //    }
        //    else
        //    {
        //        MessageBox.Show("Failed to apply Hardcore patch!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    clientFile.Dispose();
        //}
        #endregion

        private void LoadExcelTables(object sender, EventArgs e)
        {
            Progress progress = (Progress)sender;
            FileStream excelFile;

            string excelFilePath = Config.dataDirsRoot + "\\data_common\\excel\\exceltables.txt.cooked";
            try
            {
                excelFile = new FileStream(excelFilePath, FileMode.Open);
                excelTables = new ExcelTables(FileTools.StreamToByteArray(excelFile));
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to load exceltables!\nPlease ensure your directories are set correctly.\n\nFile: \n" + excelFilePath, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                progress.Close();
                progress.Dispose();
                return;
            }

            progress.ConfigBar(0, excelTables.Count, 1);
            progress.SetLoadingText("Loading in excel tables (" + excelTables.Count + ")...");
            excelTables.LoadTables(Config.dataDirsRoot + "\\data_common\\excel\\", progress.GetItemLabel(), excelTablesLoaded.GetTablesListBox());
            progress.Dispose();
        }

        private void Reanimator_ResizeEnd(object sender, EventArgs e)
        {
            Config.clientHeight = this.Height;
            Config.clientWidth = this.Width;
        }

        private void Reanimator_Load(object sender, EventArgs e)
        {
            this.Height = Config.clientHeight;
            this.Width = Config.clientWidth;
            this.Show();
            this.Refresh();

            Progress progress = new Progress();
            progress.Shown += new EventHandler(LoadExcelTables);
            progress.ShowDialog(this);
            // this fixes a weird windows API bug causing the ShowDialog to minimise the main client
            this.Hide();
            this.Show();

            #region PLUGIN
            try
            {
                LoadPlugins();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            #endregion
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            TableForm tableForm = this.ActiveMdiChild as TableForm;
            if (tableForm != null)
            {
                tableForm.SaveButton();
            }
        }
    }
}