using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Reanimator.Forms;
using Reanimator.Forms.ItemTransfer;

namespace Reanimator
{
    public partial class Reanimator : Form
    {
        private readonly Options _options;
        private readonly List<String> _indexFilesOpen;
        private TablesLoaded _tablesLoaded;
        private readonly TableDataSet _tableDataSet;
        private int _childFormNumber;
        private readonly TableFiles _tableFiles;

        public Reanimator()
        {
            _options = new Options();
            _indexFilesOpen = new List<string>();
            _tableFiles = new TableFiles();
            _tableDataSet = new TableDataSet();

            CheckEnvironment();
            InitializeComponent();
        }

        private static void CheckEnvironment()
        {
            const string lastUnpacked = "1.1";
            if (Config.DatLastUnpacked == lastUnpacked) return;

            try
            {
                MessageBox.Show("It looks like your using Reanimator for the first time, or you need to re-unpack your files.\nPlease locate your HGL installation.", "Installation", MessageBoxButtons.OK, MessageBoxIcon.Information);

                FolderBrowserDialog folder = new FolderBrowserDialog { SelectedPath = Config.HglDir };
                folder.ShowDialog();

                Config.HglDir = folder.SelectedPath;

                int[] unpack = new[] { Index.Base000, Index.LatestPatch, Index.LatestPatchLocalized };

                foreach (int dat in unpack)
                {
                    Index index = new Index(Config.HglDir + @"\data\" + Index.FileNames[dat] + ".idx");

                    if (index.Modified)
                    {
                        MessageBox.Show("Your index file is modified. Please restore it and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw new Exception("Index error");
                    }

                    foreach (Index.FileIndex file in index.FileTable)
                    {
                        if (!file.DirectoryString.Contains("excel")) continue;

                        string directory = Config.HglDir + @"\Reanimator\" + file.DirectoryString;
                        string filename = directory + file.FileNameString;
                        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
                        FileStream stream = new FileStream(@filename, FileMode.Create);
                        byte[] buffer = index.ReadDataFile(file);
                        stream.Write(buffer, 0, buffer.Length);
                        stream.Close();
                    }

                    index.Dispose();
                }

                Config.DatLastUnpacked = lastUnpacked;
                Config.DataDirsRoot = Config.HglDir + @"\Reanimator\";

                MessageBox.Show("Installation success!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Config.DatLastUnpacked = "Never";
                ExceptionLogger.LogException(ex, "CheckEnvironment");
                MessageBox.Show("An error occured during the installation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form { MdiParent = this, Text = "Window " + _childFormNumber++ };
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "HGL Files (*.idx, *.cooked)|*.idx;*.cooked|All Files (*.*)|*.*"
                };

                if (openFileDialog.ShowDialog(this) != DialogResult.OK) return;

                if (openFileDialog.FileName.EndsWith("idx"))
                {
                    OpenFileIdx(openFileDialog.FileName);
                }
                else if (openFileDialog.FileName.EndsWith("hg1"))
                {
                    //OpenFileHg1(openFileDialog.FileName);
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
                    //OpenFileMod(openFileDialog.FileName);
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
                    InitialDirectory = Config.HglDir + @"\data"
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
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Games\\Hellgate\\Save\\Singleplayer"
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
                    Filter = "Strings Files (*.xls.uni.cooked)|*.xls.uni.cooked|All Files (*.*)|*.*",
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

        private void OpenFileMod(String fileName)
        {
            if (_indexFilesOpen.Contains(fileName)) return;

            try
            {
                // Check the XML is valid before declaring an object
                bool pass = true;// Modification.Parse(fileName);

                if (pass)
                {
                    //Mod revivalMod = new Mod(szFileName, index);
                    //ModificationForm modificationForm = new ModificationForm();
                    //modificationForm.MdiParent = this;
                    //modificationForm.ShowDialog();
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
                dataGridView = { DataSource = index.FileTable },
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
                Unit heroUnit = UnitHelpFunctions.OpenCharacterFile(_tableFiles, fileName);
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
                // todo: is there a better/"normal" way to do this?
                int indexStart = fileName.LastIndexOf("\\") + 1;
                int indexEnd = fileName.LastIndexOf(".txt");
                string name = fileName.Substring(indexStart, indexEnd - indexStart);

                DataFile excelTable = _tableFiles.GetTableFromFileName(name.ToUpper());
                // todo: Add check for file differing from what's in dataset, and open as new file if different etc
                if (excelTable == null)
                {
                    MessageBox.Show("TODO");
                    return;
                }

                ExcelTableForm excelTableForm = new ExcelTableForm(excelTable, _tableDataSet)
                {
                    Text = "Excel Table: " + fileName,
                    MdiParent = this
                };
                excelTableForm.Show();
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "OpenFileCooked");
            }
        }

        private void OpenFileStrings(String fileName)
        {
            // todo: make me neater etc - i.e. merge with cooked file above
            // copy-paste for most part
            try
            {
                // todo: is there a better/"normal" way to do this?
                int indexStart = fileName.LastIndexOf("\\") + 1;
                int indexEnd = fileName.LastIndexOf(".xls");
                string name = fileName.Substring(indexStart, indexEnd - indexStart);

                // todo: this doesn't work 100% as string IDs are stored with each first letter capitalized
                DataFile excelTable = _tableFiles.GetTableFromFileName(name);
                // todo: Add check for file differing from what's in dataset, and open as new file if different etc
                if (excelTable == null)
                {
                    MessageBox.Show("TODO");
                    return;
                }

                ExcelTableForm excelTableForm = new ExcelTableForm(excelTable, _tableDataSet)
                {
                    Text = "Excel Table: " + fileName,
                    MdiParent = this
                };
                excelTableForm.Show();
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "OpenFileCooked");
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* Doesn't appear to do anything...
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
                };

                if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    string fileName = saveFileDialog.FileName;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "SaveAsToolStripMenuItem_Click");
            }
             */
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

        private void ClientPatcherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "EXE Files (*.exe)|*.exe|All Files (*.*)|*.*";
            openFileDialog.InitialDirectory = Config.HglDir + "\\SP_x64";
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

            Patches clientPatcher = new Patches(FileTools.StreamToByteArray(clientFile));
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
                if (_tableFiles == null || _tableFiles.LoadedFileCount <= 0) return;

                _tablesLoaded = new TablesLoaded(_tableDataSet) { MdiParent = this };

                foreach (DataFile dataFile in from DictionaryEntry de in _tableFiles.DataFiles
                                              select de.Value as DataFile)
                {
                    _tablesLoaded.AddItem(dataFile);
                }

                _tablesLoaded.Text = String.Format("Currently Loaded Tables [{0}]", _tableFiles.LoadedFileCount);
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
                // check exists
                String excelFilePath = String.Format("{0}{1}exceltables.{2}", Config.DataDirsRoot, ExcelFile.FolderPath, ExcelFile.FileExtention);
                if (!File.Exists(excelFilePath))
                {
                    MessageBox.Show(excelFilePath + " not found!\nPlease ensure your directories are set correctly:\nTools > Options", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //WaitForCacheLoad(progress, loadTableDataSetThread);
                    return;
                }


                // read in .t.c files
                try
                {
                    byte[] excelData = File.ReadAllBytes(excelFilePath);
                    if (!_tableFiles.LoadExcelFiles(progress, excelData))
                    {
                        MessageBox.Show("Failed to load/parse all exceltables!\nPlease ensure your directories are set correctly.\nTools > Options", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionLogger.LogException(ex, "LoadTables - read t.c file");
                    MessageBox.Show(
                        "Failed to read exceltables.txt.cooked!\nPlease ensure your directories are set correctly.\nTools > Options\n\nFile: \n" +
                        excelFilePath + "\n\n" + ex, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }


                // read in strings files
                ExcelFile stringsFile = _tableFiles["STRING_FILES"] as ExcelFile;
                if (stringsFile != null)
                {
                    if (!_tableFiles.LoadStringsFiles(progress, stringsFile))
                    {
                        MessageBox.Show("Failed to load/parse all string files!\nPlease ensure your directories are set correctly.\nTools > Options", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    MessageBox.Show("String files not loaded due to no strings_files.txt.cooked parsed!\nPlease ensure your directories are set correctly.\nTools > Options", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }


                // wait for the cache to finish loading in if it hasn't already
                //WaitForCacheLoad(progress, loadTableDataSetThread);
                _tableDataSet.TableFiles = _tableFiles;
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "LoadTables");
                MessageBox.Show(ex.Message, "LoadTables");
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
                // ensure we're trying to export from a valid form
                ExcelTableForm excelTable = ActiveMdiChild as ExcelTableForm;
                if (excelTable == null) return;

                // what columns do we want to export?
                CSVSelection select = new CSVSelection(excelTable.tableData_DataGridView);
                if (select.ShowDialog(this) != DialogResult.OK) return;

                // compiles the CSV string
                string strValue = Export.CSV(excelTable.tableData_DataGridView, select.selected, select.comboBoxDelimiter.Text);

                // prompts the user to choose where to save the file
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
                };
                if (saveFileDialog.ShowDialog(this) != DialogResult.OK) return;

                // done
                File.WriteAllText(saveFileDialog.FileName, strValue);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export of this form not supported at this time or unknown error!\n\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            if (openFileDialog.ShowDialog(this) != DialogResult.OK || !openFileDialog.FileName.EndsWith("exe")) return;


            Patches hglexe = new Patches(File.ReadAllBytes(openFileDialog.FileName));
            try
            {
                hglexe.ApplyHardcorePatch();
                File.WriteAllBytes(openFileDialog.FileName.Insert(openFileDialog.FileName.Length - 4, "-patched"),
                                   hglexe.Buffer);
                MessageBox.Show("Patch successfully applied!");
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "BypassSecurityx64ToolStripMenuItem_Click");
                MessageBox.Show("Problem Applying Patch. :(");
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
                ModificationForm modificationForm = new ModificationForm(_tableDataSet);
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
                // open file
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Model Files (*.am)|*.am|All Files (*.*)|*.*",
                    InitialDirectory = Config.HglDir
                };
                if (openFileDialog.ShowDialog(this) != DialogResult.OK ||
                    (!openFileDialog.FileName.EndsWith("am") && !openFileDialog.FileName.EndsWith("m"))) return;

                // do stuffs
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
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "modelFileToolStripMenuItem_Click");
            }
        }

        private void havokFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // open file
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Havok Files (*.hkx)|*.hkx|All Files (*.*)|*.*",
                    InitialDirectory = Config.HglDir
                };
                if (openFileDialog.ShowDialog(this) != DialogResult.OK || (!openFileDialog.FileName.EndsWith("hkx"))) return;

                // do stuffs
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
                //ItemTransferForm transfer = new ItemTransferForm(_tableDataSet, _tableFiles);
                ComplexItemTransferForm transfer = new ComplexItemTransferForm(_tableDataSet, _tableFiles);
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
                CharacterShop shop = new CharacterShop(_tableDataSet, _tableFiles);
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
                TableSearch search = new TableSearch(_tableDataSet, _tableFiles);
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

        private void scriptEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptEditor scriptEditor = new ScriptEditor(_tableDataSet);
                scriptEditor.MdiParent = this;
                scriptEditor.Show();
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "scriptEditorToolStripMenuItem_Click");
            }
        }
    }
}