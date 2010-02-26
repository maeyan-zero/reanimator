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
using System.Reflection;

namespace Reanimator
{
    public partial class Reanimator : Form
    {
        private Options options;
        private List<string> indexFilesOpen;
        private ExcelTables excelTables;
        private TablesLoaded tablesLoaded;
        private TableDataSet tableDataSet;
        private StringsTables stringsTables;
        private UpdateCheckerParams currentVersionInfos;
        private UpdateForm updateForm;

        private int childFormNumber = 0;

        public Reanimator()
        {
            options = new Options();
            indexFilesOpen = new List<string>();

            currentVersionInfos = new UpdateCheckerParams();
            currentVersionInfos.installedVersion.name = "Test";
            currentVersionInfos.installedVersion.version.CurrentVersion = "1_0_0";
            currentVersionInfos.installedVersion.link = "http://www.hellgateaus.net/forum/viewtopic.php?f=47&t=1279&p=18796#p18796";
            currentVersionInfos.saveFolder = @"C:\";

            InitializeComponent();
        }

        private void CheckEnvironment()
        {
            if (Directory.Exists(Config.HglDir) == false)
            {
                MessageBox.Show("It looks like your using Reanimator for the first time. Please set your Hellgate: London directory.");
                DialogResult result = options.ShowDialog();
            }
            if (Config.DatUnpacked == false)
            {
                DialogResult result = MessageBox.Show("To use Reanimator, you must extract files from the latest patch 1.2. Continue?", "Initialization", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    // Extract DAT appropriatly
                }
            }
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
            openFileDialog.InitialDirectory = Config.HglDir + "\\Data";

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
            openFileDialog.InitialDirectory = Config.DataDirsRoot;

            if (openFileDialog.ShowDialog(this) == DialogResult.OK && openFileDialog.FileName.EndsWith("cooked"))
            {
                OpenFile_COOKED(openFileDialog.FileName);
            }
        }

        private void StringsFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Strings Files (*.xls.uni.cooked)|*.xls.uni.cooked|All Files (*.*)|*.*";
            openFileDialog.InitialDirectory = Config.DataDirsRoot;

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
                Mod.excelTables = excelTables;
                bool pass = Mod.Parse(szFileName);

                if (pass)
                {
                    Mod.Revival revivalMod = Mod.Deserialize(szFileName);
                    ModificationForm modificationForm = new ModificationForm(revivalMod);
                    modificationForm.MdiParent = this;
                    modificationForm.Show();
                }

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
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
            String excelError = "You must have all excel tables loaded to use the Hero Editor!";
            if (excelTables == null)
            {
                MessageBox.Show(excelError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!excelTables.AllTablesLoaded)
            {
                MessageBox.Show(excelError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            HeroEditor heroEditor = new HeroEditor(heroUnit, tableDataSet, fileName);
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

            ExcelTableForm etf = new ExcelTableForm(excelTable, tableDataSet);
            etf.Text = "Excel Table: " + fileName;
            etf.MdiParent = this;
            etf.Show();
        }

        private void OpenFile_STRINGS(String fileName)
        {
            try
            {
                FileStream stringsFile = new FileStream(fileName, FileMode.Open);
                StringsFile strings = new StringsFile(FileTools.StreamToByteArray(stringsFile));
                TableForm indexExplorer = new TableForm(strings);
                StringsFile.StringBlock[] stringBlocks = strings.GetFileTable();
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
                if (childForm.Name != "ExcelTablesLoaded")
                {
                    childForm.Close();
                }
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            options.ShowDialog(this);
        }

        //private void ClientPatcherToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void Reanimator_ResizeEnd(object sender, EventArgs e)
        {
            Config.ClientHeight = this.Height;
            Config.ClientWidth = this.Width;
        }

        private void Reanimator_Load(object sender, EventArgs e)
        {
            this.Height = Config.ClientHeight;
            this.Width = Config.ClientWidth;
            this.Show();
            this.Refresh();

            ProgressForm progress = new ProgressForm(LoadTables, null);
            progress.Disposed += delegate { LoadAndDisplayCurrentlyLoadedExcelTables(); };
            progress.ShowDialog(this);

            this.GenerateCache(false);
        }

        private void LoadAndDisplayCurrentlyLoadedExcelTables()
        {
            tablesLoaded = new TablesLoaded(tableDataSet);
            tablesLoaded.MdiParent = this;
            tablesLoaded.Show();
            foreach (ExcelTable et in excelTables.GetLoadedTables())
            {
                tablesLoaded.AddItem(et);
            }
            foreach (StringsFile sf in stringsTables.GetLoadedTables())
            {
                tablesLoaded.AddItem(sf);
            }
            tablesLoaded.Text = "Currently Loaded Tables [" + (excelTables.LoadedTableCount + stringsTables.Count) + "]";
        }

        private void LoadTables(ProgressForm progress, Object var)
        {
            // begin loading in dataSet.dat right away
            Thread loadTableDataSet = new Thread(() => { tableDataSet = new TableDataSet(); });
            loadTableDataSet.Start();


            // read in .t.c files
            string excelFilePath = Config.DataDirsRoot + "\\data_common\\excel\\exceltables.txt.cooked";
            try
            {
                using (FileStream excelFile = new FileStream(excelFilePath, FileMode.Open))
                {
                    excelTables = new ExcelTables(FileTools.StreamToByteArray(excelFile));
                }

                progress.ConfigBar(0, excelTables.Count, 1);
                progress.SetLoadingText("Loading in excel tables (" + excelTables.Count + ")...");
                excelTables.LoadTables(Config.DataDirsRoot + "\\data_common\\excel\\", progress);
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to load exceltables!\nPlease ensure your directories are set correctly.\n\nFile: \n" + excelFilePath + "\n\n" + e.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


            // read in strings files
            try
            {
                Excel.StringsFiles stringsFiles = (Excel.StringsFiles)excelTables.GetTable("STRING_FILES");
                if (stringsFiles != null)
                {
                    progress.SetLoadingText("Loading in strings files (" + stringsFiles.Count + ")...");
                    progress.ConfigBar(0, stringsFiles.Count, 1);
                    stringsTables = new StringsTables();
                    stringsTables.LoadStringsTables(progress, stringsFiles);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to load in string tables!\nPlease ensure your directories are set correctly.\n\n" + e.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


            // wait for the cache to finish loading in if it hasn't already
            progress.SetStyle(ProgressBarStyle.Marquee);
            progress.SetLoadingText("Loading table cache data...");
            progress.SetCurrentItemText("Please wait...");
            while (loadTableDataSet.ThreadState == ThreadState.Running)
            {
                Thread.Sleep(50);
            }

            tableDataSet.ExcelTables = excelTables;
            tableDataSet.StringsTables = stringsTables;
        }

        private void CacheTables(ProgressForm progress, Object var)
        {
            List<ExcelTable> loadedTables = var as List<ExcelTable>;
            if (loadedTables == null)
            {
                return;
            }


            progress.SetLoadingText("First use table caching...");
            progress.ConfigBar(0, loadedTables.Count, 1);
            foreach (ExcelTable excelTable in loadedTables)
            {
                progress.SetCurrentItemText("Caching: " + excelTable.StringId);
                ProgressForm tableProgress = new ProgressForm(CacheExcelTable, excelTable);
                tableProgress.StartPosition = FormStartPosition.CenterScreen;
                tableProgress.ShowDialog();
            }


            progress.SetLoadingText("Caching strings tables (" + stringsTables.Count + ")...");
            progress.ConfigBar(0, stringsTables.Count, 1);
            foreach (StringsFile stringsFile in stringsTables.GetLoadedTables())
            {
                tableDataSet.LoadTable(progress, stringsFile);
            }


            this.GenerateRelations(progress, loadedTables);


            progress.SetLoadingText("Saving cache data...");
            progress.SetCurrentItemText("Please wait...");
            tableDataSet.SaveDataSet();
        }

        private void CacheExcelTable(ProgressForm progress, Object var)
        {
            ExcelTable excelTable = var as ExcelTable;
            tableDataSet.LoadTable(progress, excelTable);
        }

        private void GenerateRelations(ProgressForm progress, Object var)
        {
            List<ExcelTable> loadedTables = var as List<ExcelTable>;
            if (loadedTables == null)
            {
                return;
            }

            progress.SetLoadingText("Generating table relations...");
            progress.ConfigBar(0, loadedTables.Count, 1);
            tableDataSet.ClearRelations();
            foreach (ExcelTable excelTable in loadedTables)
            {
                progress.SetCurrentItemText(excelTable.StringId);
                tableDataSet.GenerateRelations(excelTable);
            }
        }

        private void SaveToolStripButton_Click(object sender, EventArgs e)
        {
            IMdiChildBase mdiChildBase = this.ActiveMdiChild as IMdiChildBase;
            if (mdiChildBase != null)
            {
                mdiChildBase.SaveButton();
            }
        }

        private void CSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TableForm excelTable = (TableForm)this.ActiveMdiChild;
                string strValue;

                if (excelTable != null)
                {
                    // Prompts the user to choose what columns to export
                    CSVSelection select = new CSVSelection(excelTable.dataGridView);
                    DialogResult result = select.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        // Compiles the CSV string
                        strValue = Export.CSV(excelTable.dataGridView, select.selected, select.comboBoxDelimiter.Text);

                        // Prompts the user to choose where to save the file
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";
                        saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                        //saveFileDialog.FileName = excelTable.GetExcelTableName();

                        if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                        {
                            string FileName = saveFileDialog.FileName;
                            File.WriteAllText(FileName, strValue);
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Export of this form not supported at this time.");
            }
        }

        private void BypassSecurityx64ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Exectuable Files (*.exe)|*.exe|All Files (*.*)|*.*";
            openFileDialog.InitialDirectory = Config.HglDir;

            if (openFileDialog.ShowDialog(this) == DialogResult.OK && openFileDialog.FileName.EndsWith("exe"))
            {
                ClientPatcher hglexe = new ClientPatcher(File.ReadAllBytes(openFileDialog.FileName));
                try
                {
                    hglexe.ApplyHardcorePatch();
                    File.WriteAllBytes(openFileDialog.FileName.Insert(openFileDialog.FileName.Length - 4, "-patched"), hglexe.Buffer);
                    MessageBox.Show("Patch successfully applied!");
                }
                catch
                {
                    MessageBox.Show("Problem Applying Patch. :(");
                }
            }
        }

        private void CacheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CacheInfo info = new CacheInfo(@"cache\");
            info.MdiParent = this;
            info.Show();
        }

        private void ShowCacheInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(Config.CacheFilePath))
            {
                FileInfo fileInfo = new FileInfo(Config.CacheFilePath);
                MessageBox.Show("Your cache file is using " + fileInfo.Length + " bytes", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("You have no cache saved!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void GenerateCacheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.GenerateCache(true);
        }

        private void GenerateCache(bool manualRequest)
        {
            DialogResult dr = DialogResult.No;

            bool partialGeneration = false;
            if (!File.Exists(Config.CacheFilePath) || tableDataSet.LoadedTableCount == 0)
            {
                dr = MessageBox.Show("Reanimator has detected no cached table data.\nDo you wish to generate it now? (this may take a few minutes)", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            else if (manualRequest)
            {
                dr = MessageBox.Show("Are you sure you wish to regenerate the cache? (this will take a few minutes)", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            else if (tableDataSet.LoadedTableCount < excelTables.LoadedTableCount + stringsTables.Count)
            {
                dr = MessageBox.Show("Reanimator has detected that not all tables have been cached.\nDo you wish to generate the remaining now? (this may take a few minutes)", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                partialGeneration = true;
            }

            if (dr == DialogResult.Yes)
            {
                if (!partialGeneration)
                {
                    tableDataSet.ClearDataSet();
                }
                ProgressForm cachingProgress = new ProgressForm(CacheTables, excelTables.GetLoadedTables());
                cachingProgress.ShowDialog(this);
            }
        }

        private void RegenerateRelationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProgressForm progress = new ProgressForm(GenerateRelations, excelTables.GetLoadedTables());
            progress.ShowDialog(this);
        }

        private void ShowExcelTablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (showExcelTablesToolStripMenuItem.Checked)
            {
                tablesLoaded.StartPosition = FormStartPosition.Manual;
                tablesLoaded.Location = new Point(0, 0);
                tablesLoaded.Show();
            }
            else
            {
                tablesLoaded.Hide();
            }
        }

        private void CheckForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (updateForm == null)
            {
                updateForm = new UpdateForm(currentVersionInfos);
                updateForm.StartPosition = FormStartPosition.CenterScreen;
                updateForm.MdiParent = this;
            }

            updateForm.Show();

            //foreach (NewMod mod in mods)
            //{
            //    // if the mod file defines its own extension use that one)
            //    extension = mod.extension == null ? extension : mod.extension;

            //    // might also want to check if the mod is the most up-to-date one (compared to possible other mods)
            //    if (!File.Exists(folder + mod.name + "_" + mod.version.CurrentVersion + extension) && !installedVersion.IsNewestVersion(mod))
            //    {
            //        Console.WriteLine("Newer version found! Downloading file to " + folder + "...");
            //        UpdateChecker.DownloadFile(mod, folder, extension);
            //    }
            //    else
            //    {
            //        Console.WriteLine("You already have the newest version installed or downloaded!");
            //    }
            //}
        }
    }
}