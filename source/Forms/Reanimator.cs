using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Reanimator.Forms;
using Reanimator.Excel;
using System.Threading;
using Reanimator.Forms.ItemTransfer;

namespace Reanimator
{
    public partial class Reanimator : Form
    {
        private readonly Options _options;
        private readonly List<string> _indexFilesOpen;
        private ExcelTables _excelTables;
        private TablesLoaded _tablesLoaded;
        private TableDataSet _tableDataSet;
        private StringsTables _stringsTables;
        private int _childFormNumber;

        public Reanimator()
        {
            try
            {
                _options = new Options();
                _indexFilesOpen = new List<string>();

                CheckEnvironment();
                InitializeComponent();
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "Reanimator");
                MessageBox.Show(ex.Message, "Reanimator");
            }
        }

        private void CheckEnvironment()
        {
            if (!Config.DatUnpacked)
            {
                try
                {
                    MessageBox.Show("Welcome to the Reanimator installation wizard. First, select your HGL directory.", "Installation", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    FolderBrowserDialog folder = new FolderBrowserDialog();
                    folder.SelectedPath = Config.HglDir;
                    folder.ShowDialog();

                    Config.HglDir = folder.SelectedPath;

                    if (Config.DatUnpacked == false)
                    {
                        int[] unpack = new int[] { Index.LatestPatch, Index.LatestPatchLocalized };

                        DialogResult result = MessageBox.Show("To use Reanimator, you must extract files from the latest patch 1.2. Continue?", "Initialization", MessageBoxButtons.OKCancel);

                        if (result == DialogResult.OK)
                        {
                            foreach (int dat in unpack)
                            {
                                Index index = new Index(Config.HglDir + "//data//" + Index.FileNames[dat] + ".idx");

                                if (index == null)
                                {
                                    MessageBox.Show("Please check you have your directory set correctly and have the latest patch installed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    throw new Exception("Dir error");
                                }

                                if (index.Modified)
                                {
                                    MessageBox.Show("Your index file is modified. Please restore it and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    throw new Exception("Index error");
                                }

                                foreach (Index.FileIndex file in index.FileTable)
                                {
                                    if (file.DirectoryString.Contains("excel"))
                                    {
                                        string directory = Config.HglDir + "\\Reanimator\\" + file.DirectoryString;
                                        string filename = directory + file.FileNameString;
                                        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
                                        FileStream stream = new FileStream(@filename, FileMode.Create);
                                        byte[] buffer = index.ReadDataFile(file);
                                        stream.Write(buffer, 0, buffer.Length);
                                        stream.Close();
                                    }
                                }

                                index.Dispose();
                            }

                            Config.DatUnpacked = true;
                            Config.DataDirsRoot = Config.HglDir + "\\Reanimator\\";

                            MessageBox.Show("Installation success!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch(Exception ex)
                {
                    Config.DatUnpacked = false;
                    ExceptionLogger.LogException(ex, "CheckEnvironment");
                    MessageBox.Show("An error occured during the installation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form {MdiParent = this, Text = "Window " + _childFormNumber++};
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                                                    {
                                                        Filter =
                                                            "HGL Files (*.idx, *.hg1, *.cooked)|*.idx;*.hg1;*.cooked|All Files (*.*)|*.*"
                                                    };

                if (openFileDialog.ShowDialog(this) != DialogResult.OK) return;

                if (openFileDialog.FileName.EndsWith("idx"))
                {
                    OpenFileIdx(openFileDialog.FileName);
                }
                else if (openFileDialog.FileName.EndsWith("hg1"))
                {
                    OpenFileHg1(openFileDialog.FileName);
                }
                else if (openFileDialog.FileName.EndsWith("xls.uni.cooked"))
                {
                    OpenFileStrings(openFileDialog.FileName);
                }
                else if (openFileDialog.FileName.EndsWith("cooked"))
                {
                    OpenFileCooked(openFileDialog.FileName);
                }
                else if (openFileDialog.FileName.EndsWith("mod") || openFileDialog.FileName.EndsWith("xml"))
                {
                    OpenFileMod(openFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "OpenFile");
            }
        }

        private void OpenIndexFile(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                                                    {
                                                        Filter = "Index Files (*.idx)|*.idx|All Files (*.*)|*.*",
                                                        InitialDirectory = Config.HglDir + "\\Data"
                                                    };

                if (openFileDialog.ShowDialog(this) == DialogResult.OK && openFileDialog.FileName.EndsWith("idx"))
                {
                    OpenFileIdx(openFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "OpenIndexFile");
            }
        }

        private void OpenCharacterFile(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                                                    {
                                                        Filter = "Character Files (*.hg1)|*.hg1|All Files (*.*)|*.*",
                                                        InitialDirectory =
                                                            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                                            "\\My Games\\Hellgate\\Save\\Singleplayer"
                                                    };

                if (openFileDialog.ShowDialog(this) == DialogResult.OK && openFileDialog.FileName.EndsWith("hg1"))
                {
                    OpenFileHg1(openFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "OpenCharacterFile");
            }
        }

        private void OpenCookedFile(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                                                    {
                                                        Filter = "Cooked Files (*.cooked)|*.cooked|All Files (*.*)|*.*",
                                                        InitialDirectory = Config.DataDirsRoot
                                                    };

                if (openFileDialog.ShowDialog(this) == DialogResult.OK && openFileDialog.FileName.EndsWith("cooked"))
                {
                    OpenFileCooked(openFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "OpenCookedFile");
            }
        }

        private void StringsFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                                                    {
                                                        Filter =
                                                            "Strings Files (*.xls.uni.cooked)|*.xls.uni.cooked|All Files (*.*)|*.*",
                                                        InitialDirectory = Config.DataDirsRoot
                                                    };

                if (openFileDialog.ShowDialog(this) == DialogResult.OK && openFileDialog.FileName.EndsWith("cooked"))
                {
                    OpenFileStrings(openFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "StringsFileToolStripMenuItem_Click");
            }
        }

        private void OpenModFile(object sender, EventArgs e)
        {
            //ModificationForm modificationForm = new ModificationForm(_tableDataSet);
            //modificationForm.MdiParent = this;
            //modificationForm.Show();
        }

        private void OpenFileMod(string szFileName)
        {
            if (_indexFilesOpen.Contains(szFileName)) return;

            try
            {
                // Check the XML is valid before declaring an object
                bool pass = RevivalMod.Parse(szFileName);

                if (pass)
                {
                    //Mod revivalMod = new Mod(szFileName, index);
                    ModificationForm modificationForm = new ModificationForm();
                    //modificationForm.MdiParent = this;
                    modificationForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("The Modification appears to be invalid. Check syntax and try again.");
                }

                return;
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "OpenFileMod");
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void OpenFileIdx(String fileName)
        {
            if (_indexFilesOpen.Contains(fileName)) return;

            Index index;
            try
            {
                index = new Index(fileName);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "OpenFileIdx");
                MessageBox.Show("Failed to open file: " + fileName + "\n\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            TableForm indexExplorer = new TableForm(index)
                                          {
                                              dataGridView = {DataSource = index.FileTable},
                                              MdiParent = this
                                          };
            indexExplorer.Text += ": " + fileName;
            indexExplorer.Show();

            _indexFilesOpen.Add(fileName);

            return;
        }

        private void OpenFileHg1(string fileName)
        {
            try
            {
                // TODO give some sort of decent error or something
                if (_excelTables == null) return;

                Unit heroUnit = UnitHelpFunctions.OpenCharacterFile(ref _excelTables, fileName);

                HeroEditor heroEditor = new HeroEditor(heroUnit, _tableDataSet, fileName)
                                            {
                                                Text = "Hero Editor: " + fileName,
                                                MdiParent = this
                                            };
                heroEditor.Show();
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "OpenFileHg1");
            }
        }

        private void OpenFileCooked(String fileName)
        {
            try
            {
                // TODO give some sort of decent error or something
                if (_excelTables == null) return;

                int indexStart = fileName.LastIndexOf("\\") + 1;
                int indexEnd = fileName.LastIndexOf(".txt");
                string name = fileName.Substring(indexStart, indexEnd - indexStart);

                ExcelTable excelTable = _excelTables.GetTable(name);
                if (excelTable == null)
                {
                    return;
                }

                ExcelTableForm etf = new ExcelTableForm(excelTable, _tableDataSet)
                                         {
                                             Text = "Excel Table: " + fileName,
                                             MdiParent = this
                                         };
                etf.Show();
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "OpenFileCooked");
            }
        }

        private void OpenFileStrings(String fileName)
        {
            try
            {
                FileStream stringsFile = new FileStream(fileName, FileMode.Open);
                StringsFile strings = new StringsFile(FileTools.StreamToByteArray(stringsFile));
                if (!strings.IsGood) return;

                strings.FilePath = fileName;
                StringsFile.StringBlock[] stringBlocks = strings.GetFileTable();

                TableForm indexExplorer = new TableForm(strings)
                                              {
                                                  dataGridView = {DataSource = stringBlocks},
                                                  MdiParent = this
                                              };
                indexExplorer.Text += ": " + fileName;
                indexExplorer.Show();
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "OpenFileStrings");
                MessageBox.Show("Failed to open file!\n\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                                                    {
                                                        Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                                                        InitialDirectory =
                                                            Environment.GetFolderPath(Environment.SpecialFolder.Personal)
                                                    };

                if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    //string fileName = saveFileDialog.FileName;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "SaveAsToolStripMenuItem_Click");
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
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
            _options.ShowDialog(this);
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
            Config.ClientHeight = Height;
            Config.ClientWidth = Width;
        }

        private void Reanimator_Load(object sender, EventArgs e)
        {
            try
            {
                Height = Config.ClientHeight;
                Width = Config.ClientWidth;
                Show();
                Refresh();

                ProgressForm progress = new ProgressForm(LoadTables, null);
                progress.Disposed += delegate { LoadAndDisplayCurrentlyLoadedExcelTables(); };
                progress.ShowDialog(this);

                GenerateCache(false);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "Reanimator_Load");
                MessageBox.Show(ex.Message, "Reanimator_Load");
            }
        }

        private void LoadAndDisplayCurrentlyLoadedExcelTables()
        {
            try
            {
                _tablesLoaded = new TablesLoaded(_tableDataSet) { MdiParent = this };
                int loadedTableCount = 0;

                if (_excelTables != null)
                {
                    foreach (ExcelTable et in _excelTables.GetLoadedTables())
                    {
                        _tablesLoaded.AddItem(et);
                    }
                    loadedTableCount += _excelTables.LoadedTableCount;
                }

                if (_stringsTables != null)
                {
                    foreach (StringsFile sf in _stringsTables.GetLoadedTables())
                    {
                        _tablesLoaded.AddItem(sf);
                    }
                    loadedTableCount += _stringsTables.Count;
                }

                if (loadedTableCount <= 0) return;

                _tablesLoaded.Text = "Currently Loaded Tables [" + loadedTableCount + "]";
                _tablesLoaded.Show();
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "LoadAndDisplayCurrentlyLoadedExcelTables");
                MessageBox.Show(ex.Message, "LoadAndDisplayCurrentlyLoadedExcelTables");
            }
        }

        private void LoadTables(ProgressForm progress, Object var)
        {
            try
            {
                // begin loading in dataSet.dat right away
                Thread loadTableDataSet = new Thread(() => { _tableDataSet = new TableDataSet(); });
                loadTableDataSet.Start();


                // read in .t.c files
                string excelFilePath = Config.DataDirsRoot + "\\data_common\\excel\\exceltables.txt.cooked";
                try
                {
                    using (FileStream excelFile = new FileStream(excelFilePath, FileMode.Open))
                    {
                        _excelTables = new ExcelTables(FileTools.StreamToByteArray(excelFile));
                    }

                    progress.ConfigBar(0, _excelTables.Count, 1);
                    progress.SetLoadingText("Loading in excel tables (" + _excelTables.Count + ")...");
                    _excelTables.LoadTables(Config.DataDirsRoot + "\\data_common\\excel\\", progress);
                }
                catch (Exception ex)
                {
                    ExceptionLogger.LogException(ex, "LoadTables - read t.c file");
                    MessageBox.Show(
                        "Failed to load exceltables!\nPlease ensure your directories are set correctly.\nTools > Options\n\nFile: \n" +
                        excelFilePath + "\n\n" + ex, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }


                // read in strings files
                if (_excelTables != null)
                {
                    try
                    {
                        StringsFiles stringsFiles = (StringsFiles)_excelTables.GetTable("STRING_FILES");
                        if (stringsFiles != null)
                        {
                            progress.SetLoadingText("Loading in strings files (" + stringsFiles.Count + ")...");
                            progress.ConfigBar(0, stringsFiles.Count, 1);
                            _stringsTables = new StringsTables();
                            _stringsTables.LoadStringsTables(progress, stringsFiles);
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionLogger.LogException(ex, "LoadTables - read strings file");
                        MessageBox.Show(
                            "Failed to load in string tables!\nPlease ensure your directories are set correctly.\n\n" +
                            ex, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }


                // wait for the cache to finish loading in if it hasn't already
                progress.SetStyle(ProgressBarStyle.Marquee);
                progress.SetLoadingText("Loading table cache data...");
                progress.SetCurrentItemText("Please wait...");
                while (loadTableDataSet.ThreadState == ThreadState.Running)
                {
                    Thread.Sleep(50);
                }

                _tableDataSet.ExcelTables = _excelTables;
                _tableDataSet.StringsTables = _stringsTables;
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "LoadTables");
                MessageBox.Show(ex.Message, "LoadTables");
            }
        }

        private void CacheTables(ProgressForm progress, Object var)
        {
            try
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
                    ProgressForm tableProgress = new ProgressForm(CacheExcelTable, excelTable) { StartPosition = FormStartPosition.CenterScreen };
                    tableProgress.ShowDialog();
                }


                progress.SetLoadingText("Caching strings tables (" + _stringsTables.Count + ")...");
                progress.ConfigBar(0, _stringsTables.Count, 1);
                foreach (StringsFile stringsFile in _stringsTables.GetLoadedTables())
                {
                    _tableDataSet.LoadTable(progress, stringsFile);
                }


                GenerateRelations(progress, loadedTables);


                progress.SetLoadingText("Saving cache data...");
                progress.SetCurrentItemText("Please wait...");
                _tableDataSet.SaveDataSet();
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "CacheTables");
                MessageBox.Show(ex.Message, "CacheTables");
            }
        }

        private void CacheExcelTable(ProgressForm progress, Object var)
        {
            try
            {
                ExcelTable excelTable = var as ExcelTable;
                _tableDataSet.LoadTable(progress, excelTable);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "CacheExcelTable");
                MessageBox.Show(ex.Message, "CacheExcelTable");
            }
        }

        private void GenerateRelations(ProgressForm progress, Object var)
        {
            try
            {
                List<ExcelTable> loadedTables = var as List<ExcelTable>;
                if (loadedTables == null)
                {
                    return;
                }

                progress.SetLoadingText("Generating table relations...");
                progress.ConfigBar(0, loadedTables.Count, 1);
                _tableDataSet.ClearRelations();
                foreach (ExcelTable excelTable in loadedTables)
                {
                    progress.SetCurrentItemText(excelTable.StringId);
                    _tableDataSet.GenerateRelations(excelTable);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "GenerateRelations");
                MessageBox.Show(ex.Message, "GenerateRelations");
            }
        }

        private void SaveToolStripButton_Click(object sender, EventArgs e)
        {
            IMdiChildBase mdiChildBase = ActiveMdiChild as IMdiChildBase;
            if (mdiChildBase == null) return;

            mdiChildBase.SaveButton();
        }

        private void CSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ExcelTableForm excelTable = (ExcelTableForm)ActiveMdiChild;

                if (excelTable != null)
                {
                    // Prompts the user to choose what columns to export
                    CSVSelection select = new CSVSelection(excelTable.dataGridView);
                    DialogResult result = select.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        // Compiles the CSV string
                        string strValue = Export.CSV(excelTable.dataGridView, select.selected, select.comboBoxDelimiter.Text);

                        // Prompts the user to choose where to save the file
                        SaveFileDialog saveFileDialog = new SaveFileDialog
                                                            {
                                                                Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",
                                                                InitialDirectory =
                                                                    Environment.GetFolderPath(
                                                                    Environment.SpecialFolder.Personal)
                                                            };

                        if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                        {
                            string fileName = saveFileDialog.FileName;
                            File.WriteAllText(fileName, strValue);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Export of this form not supported at this time.");
                ExceptionLogger.LogException(ex, "CSVToolStripMenuItem_Click");
            }
        }

        private void BypassSecurityx64ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
                                                {
                                                    Filter = "Exectuable Files (*.exe)|*.exe|All Files (*.*)|*.*",
                                                    InitialDirectory = Config.HglDir
                                                };

            if (openFileDialog.ShowDialog(this) == DialogResult.OK && openFileDialog.FileName.EndsWith("exe"))
            {
                ClientPatcher hglexe = new ClientPatcher(File.ReadAllBytes(openFileDialog.FileName));
                try
                {
                    hglexe.ApplyHardcorePatch();
                    File.WriteAllBytes(openFileDialog.FileName.Insert(openFileDialog.FileName.Length - 4, "-patched"), hglexe.Buffer);
                    MessageBox.Show("Patch successfully applied!");
                }
                catch(Exception ex)
                {
                    ExceptionLogger.LogException(ex, "BypassSecurityx64ToolStripMenuItem_Click");
                    MessageBox.Show("Problem Applying Patch. :(");
                }
            }
        }

        private void CacheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CacheInfo info = new CacheInfo(@"cache\") {MdiParent = this};
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
            GenerateCache(true);
        }

        private void GenerateCache(bool manualRequest)
        {
            try
            {
                if (_excelTables == null) return;

                DialogResult dr = DialogResult.No;

                bool partialGeneration = false;
                if (!File.Exists(Config.CacheFilePath) || _tableDataSet.LoadedTableCount == 0)
                {
                    dr = MessageBox.Show("Reanimator has detected no cached table data.\nDo you wish to generate it now? (this may take a few minutes)", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }
                else if (manualRequest)
                {
                    dr = MessageBox.Show("Are you sure you wish to regenerate the cache? (this will take a few minutes)", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }
                else if (_tableDataSet.LoadedTableCount < _excelTables.LoadedTableCount + _stringsTables.Count)
                {
                    dr = MessageBox.Show("Reanimator has detected that not all tables have been cached.\nDo you wish to generate the remaining now? (this may take a few minutes)", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    partialGeneration = true;
                }

                if (dr == DialogResult.Yes)
                {
                    if (!partialGeneration)
                    {
                        _tableDataSet.ClearDataSet();
                    }
                    ProgressForm cachingProgress = new ProgressForm(CacheTables, _excelTables.GetLoadedTables());
                    cachingProgress.ShowDialog(this);
                }
                else if (_tableDataSet.RegenerateRelations && _excelTables != null && _tableDataSet.LoadedTableCount > 0)
                {
                    dr = MessageBox.Show("Reanimator has detected your table relations are out of date.\nDo you wish to regenerate them?", "Regenerate Relations", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        ProgressForm progress = new ProgressForm(GenerateRelations, _excelTables.GetLoadedTables());
                        progress.ShowDialog(this);
                        _tableDataSet.SaveDataSet();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "GenerateCache");
            }
        }

        private void RegenerateRelationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_excelTables == null) return;

                ProgressForm progress = new ProgressForm(GenerateRelations, _excelTables.GetLoadedTables());
                progress.ShowDialog(this);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "RegenerateRelationsToolStripMenuItem_Click");
            }
        }

        private void ShowExcelTablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (showExcelTablesToolStripMenuItem.Checked)
                {
                    _tablesLoaded.StartPosition = FormStartPosition.Manual;
                    _tablesLoaded.Location = new Point(0, 0);
                    _tablesLoaded.Show();
                }
                else
                {
                    _tablesLoaded.Hide();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "ShowExcelTablesToolStripMenuItem_Click");
            }
        }

        private void applyModificationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ModificationForm modificationForm = new ModificationForm();
                modificationForm.ShowDialog();
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "applyModificationsToolStripMenuItem_Click");
            }
        }

        private void modelFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                                                    {
                                                        Filter = "Model Files (*.am)|*.am|All Files (*.*)|*.*",
                                                        InitialDirectory = Config.HglDir
                                                    };

                if (openFileDialog.ShowDialog(this) == DialogResult.OK && (openFileDialog.FileName.EndsWith("am") || openFileDialog.FileName.EndsWith("m")))
                {
                    FileStream stream = new FileStream(@openFileDialog.FileName, FileMode.Open);
                    Model model = new Model(new BinaryReader(stream));
                    stream.Close();
                    stream = new FileStream(@"d:\out.obj", FileMode.OpenOrCreate);
                    stream.Flush();
                    string exportedModel = Export.Asset.ToObj(model);
                    System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                    byte[] buffer = encoding.GetBytes(exportedModel);
                    stream.Write(buffer, 0, buffer.Length);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "modelFileToolStripMenuItem_Click");
            }
        }

        private void havokFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                                                    {
                                                        Filter = "Havok Files (*.hkx)|*.hkx|All Files (*.*)|*.*",
                                                        InitialDirectory = Config.HglDir
                                                    };

                if (openFileDialog.ShowDialog(this) != DialogResult.OK || (!openFileDialog.FileName.EndsWith("hkx"))) return;

                FileStream stream = new FileStream(@openFileDialog.FileName, FileMode.Open);
                //Havok havok = new Havok(new BinaryReader(stream));
                stream.Close();
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "havokFileToolStripMenuItem_Click");
            }
        }

        private void tradeItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ItemTransferForm transfer = new ItemTransferForm(ref _tableDataSet, ref _excelTables);
                ComplexItemTransferForm transfer = new ComplexItemTransferForm(ref _tableDataSet, ref _excelTables);
                transfer.ShowDialog(this);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "tradeItemsToolStripMenuItem_Click");
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TableForm tableForm = ActiveMdiChild as TableForm;

                if (tableForm == null) return;
                if (!tableForm.IsIndexFile) return;

                tableForm.SaveButton();
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "saveToolStripMenuItem_Click");
            }
        }

        private void itemShopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CharacterShop shop = new CharacterShop(ref _tableDataSet, ref _excelTables);
                shop.ShowDialog(this);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "itemShopToolStripMenuItem_Click");
            }
        }

        private void searchTablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TableSearch search = new TableSearch(ref _tableDataSet, ref _excelTables);
                search.ShowDialog(this);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "searchTablesToolStripMenuItem_Click");
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Reanimator by the Revival Team (c) 2009-2010" + Environment.NewLine
                + "Credits: Maeyan, Alex2069, Kite & Malachor" + Environment.NewLine
                + "For more info visit us at: http://www.hellgateaus.net", "Credits", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}