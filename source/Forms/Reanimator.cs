using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using Reanimator.Forms;
using Reanimator.Forms.ItemTransfer;

namespace Reanimator
{
    public partial class Reanimator : Form
    {
        // Forms
        private readonly Options _options;
        private readonly FileExplorer _fileExplorer;

        // Dependencies
        private readonly TableFiles _tableFiles;
        private readonly TableDataSet _tableDataSet;

        // Variables
        private readonly List<String> _indexFilesOpen; // do we need this?
        private TablesLoaded _tablesLoaded; // do we need this?
        private int _childFormNumber; // do we need this?


        public Reanimator()
        {
            InitializeComponent();

            _options = new Options();
            _fileExplorer = new FileExplorer { MdiParent = this };
            
            _tableFiles = new TableFiles();
            _tableDataSet = new TableDataSet();

            _indexFilesOpen = new List<String>();
            _CheckHellgateInstallation();

#if DEBUG
            characterFileToolStripMenuItem.Visible = true;
            convertTCv4FilesToolStripMenuItem.Visible = true;
#endif

            //String str = "pLongConnections";
            //String str = @"data\background\catacombs\ct_connb_path.xml.cooked";
            //UInt32 strHash = Crypt.GetStringHash(str);

            //tw = new StreamWriter(@"C:\asdf.txt");
            //filestream = new FileStream(@"C:\asdf.txt", FileMode.Create, FileAccess.ReadWrite);
            //_DoFolder(@"D:\Games\Hellgate London\data\skills\consumable\");
            //_DoFolder(@"D:\Games\Hellgate London\data\skills\destructible\");
            //_DoFolder(@"D:\Games\Hellgate London\data\skills\cabalist\");
            //_DoFolder(@"D:\Games\Hellgate London\data\skills\hunter\");
            //_DoFolder(@"D:\Games\Hellgate London\data\skills\monster\");
            //_DoFolder(@"D:\Games\Hellgate London\data\skills\proc\");
            //_DoFolder(@"D:\Games\Hellgate London\data\skills\quest\");
            //_DoFolder(@"D:\Games\Hellgate London\data\skills\templar\");
            //_DoFolder(@"D:\Games\Hellgate London\data\skills\weapon\");
            //_DoFolder(@"D:\Games\Hellgate London\data\skills\weapon\melee\");
            //_DoFolder(@"D:\Games\Hellgate London\data\skills\");
            //tw.Close();
            // this.Close();

        }

        //private TextWriter tw;

        //private void _DoFolder(String folderDir)
        //{
        //    DirectoryInfo directoryInfo = new DirectoryInfo(folderDir);
        //    FileInfo[] files = directoryInfo.GetFiles("*.xml.cooked");

        //    XmlCookedFile xmlAdrenaline = null;

        //    foreach (FileInfo fileInfo in files)
        //    {
        //        XmlCookedFile xmlCookedFile = new XmlCookedFile();

        //        byte[] data = File.ReadAllBytes(fileInfo.FullName);
        //        if (fileInfo.FullName.Contains("electriclasers.xml.cooked"))
        //        {
        //            xmlAdrenaline = xmlCookedFile;
        //            int bp = 0;
        //        }

        //        Debug.Assert(xmlCookedFile.Uncook(data));

        //        xmlCookedFile.SaveXml(fileInfo.FullName.Replace(".cooked", ""));

        //        //String blah = xmlCookedFile.Blah();
        //        //if (blah != null)
        //        //{
        //        //    //tw.WriteLine(fileInfo.FullName);
        //        //    //tw.WriteLine(blah);
        //        //}
        //    }

        //    if (xmlAdrenaline != null)
        //    {
        //        // xmlAdrenaline.SaveXmlCooked(@"c:\asdf.xml.cooked");
        //    }
        //}

        private static void _CheckHellgateInstallation()
        {
            if (Directory.Exists(Config.HglDir))
            {
                return;
            }

            MessageBox.Show("Please locate your Hellgate London installation directory.\n" +
                "For this program to work correctly, please ensure the latest Single Player patch is installed.\n" +
                "For more information, please visit our website: http://www.hellgateaus.net",
                "Installation", MessageBoxButtons.OK, MessageBoxIcon.Information);

            DialogResult installResult;

            do
            {
                FolderBrowserDialog folder = new FolderBrowserDialog { SelectedPath = Config.HglDir };
                DialogResult selectPathResult = folder.ShowDialog();

                if (selectPathResult == DialogResult.OK)
                {
                    Config.HglDir = folder.SelectedPath;
                    Config.HglDataDir = Path.Combine(Config.HglDir, "\\data");
                    return;
                }

                installResult =
                    MessageBox.Show("You must have Hellgate: London installed and the directory set to use Reanimator.",
                        "Installation Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
            while (installResult == DialogResult.Retry);
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

                String filePath = openFileDialog.FileName;
                if (openFileDialog.FileName.EndsWith("idx"))
                {
                    _OpenFileIdx(openFileDialog.FileName);
                }
                else if (openFileDialog.FileName.EndsWith("hg1"))
                {
                    //OpenFileHg1(openFileDialog.FileName);
                }
                else if (openFileDialog.FileName.EndsWith("xls.uni.cooked"))
                {
                    _OpenFileStrings(openFileDialog.FileName);
                }
                else if (filePath.EndsWith(XmlCookedFile.FileExtention))
                {
                    _OpenFileXmlCooked(filePath);
                }
                else if (openFileDialog.FileName.EndsWith("cooked"))
                {
                    _OpenFileCooked(openFileDialog.FileName);
                }
                else if (openFileDialog.FileName.EndsWith("mod") || openFileDialog.FileName.EndsWith("xml"))
                {
                    //OpenFileMod(openFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "OpenFile", false);
            }
        }

        private void _OpenFileXmlCooked(String filePath)
        {
            byte[] xmlCookedBytes;
            try
            {
                xmlCookedBytes = File.ReadAllBytes(filePath);
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to read in file!\n\n" + e, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            XmlCookedFile xmlCookedFile = new XmlCookedFile();
            if (!xmlCookedFile.Uncook(xmlCookedBytes))
            {
                MessageBox.Show("Failed to uncook xml file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            String xmlPath = filePath.Replace(".cooked", "");
            try
            {
                xmlCookedFile.SaveXml(xmlPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save uncooked xml file!\n\n" + ex, "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            // this is a bit dodgy using exceptions as if-else, but meh
            // todo: add check for file name existence etc
            try
            {
                Process notePad = new Process { StartInfo = { FileName = "notepad++.exe", Arguments = xmlPath } };
                notePad.Start();
            }
            catch (Exception)
            {
                Process notePad = new Process { StartInfo = { FileName = "notepad.exe", Arguments = xmlPath } };
                notePad.Start();
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
                    _OpenFileIdx(openFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "OpenIndexFile", false);
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
                    _OpenFileHg1(openFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "OpenCharacterFile", false);
            }
        }

        private void OpenCookedFile(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Cooked Files (*.cooked)|*.cooked|All Files (*.*)|*.*",
                    InitialDirectory = Config.HglDataDir
                };

                if (openFileDialog.ShowDialog(this) == DialogResult.OK && openFileDialog.FileName.EndsWith("cooked"))
                {
                    _OpenFileCooked(openFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "OpenCookedFile", false);
            }
        }

        private void StringsFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Strings Files (*.xls.uni.cooked)|*.xls.uni.cooked|All Files (*.*)|*.*",
                    InitialDirectory = Config.HglDataDir
                };

                if (openFileDialog.ShowDialog(this) == DialogResult.OK && openFileDialog.FileName.EndsWith("cooked"))
                {
                    _OpenFileStrings(openFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "StringsFileToolStripMenuItem_Click", false);
            }
        }

        private void _OpenFileMod(String fileName)
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
                ExceptionLogger.LogException(ex, "OpenFileMod", false);
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void _OpenFileIdx(String fileName)
        {
            if (_indexFilesOpen.Contains(fileName)) return;

            Index index = new Index();
            try
            {
                byte[] indexData = File.ReadAllBytes(fileName);

                index.ParseData(indexData, fileName);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "OpenFileIdx", false);
                MessageBox.Show("Failed to open file: " + fileName + "\n\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            TableForm indexExplorer = new TableForm(index)
            {
                dataGridView = { DataSource = index.Files.ToArray() },
                MdiParent = this
            };
            indexExplorer.Text += ": " + fileName;
            indexExplorer.Show();

            _indexFilesOpen.Add(fileName);

            return;
        }

        private void _OpenFileHg1(String fileName)
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
                ExceptionLogger.LogException(ex, "OpenFileHg1", false);
            }
        }

        private void _OpenFileCooked(String fileName)
        {
            try
            {
                String name = Path.GetFileNameWithoutExtension(fileName);

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
                ExceptionLogger.LogException(ex, "OpenFileCooked", false);
            }
        }

        private void _OpenFileStrings(String fileName)
        {
            // todo: make me neater etc - i.e. merge with cooked file above
            // copy-paste for most part
            try
            {
                String name = Path.GetFileNameWithoutExtension(fileName);


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
                ExceptionLogger.LogException(ex, "OpenFileCooked", false);
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

        private void _OptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _options.ShowDialog(this);
        }

        private void _ClientPatcherToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void _Reanimator_ResizeEnd(object sender, EventArgs e)
        {
            Config.ClientHeight = Height;
            Config.ClientWidth = Width;
        }

        private void _Reanimator_Load(object sender, EventArgs e)
        {
            try
            {
                Height = Config.ClientHeight;
                Width = Config.ClientWidth;
                Show();
                Refresh();

                ProgressForm fileExplorerProgress = new ProgressForm(_fileExplorer.LoadIndexFiles, null);
                fileExplorerProgress.ShowDialog(this);
                _fileExplorer.Show();

                //_indexFiles = _fileExplorer.IndexFiles;
                _tableFiles.FileExplorer = _fileExplorer;

                ProgressForm progress = new ProgressForm(_LoadTables, null);
                progress.ShowDialog(this);
                _tableDataSet.TableFiles = _tableFiles;
                _LoadAndDisplayCurrentlyLoadedExcelTables();

                XmlCookedFile.Initialize(_tableDataSet);

          //      if (false)
                //{
                //XmlDocument xmlDocument = new XmlDocument();
                //xmlDocument.Load(@"D:\Games\Hellgate London\data\skills\consumable\summoncocomoko.xml");
                ////xmlDocument.Load(@"D:\Games\Hellgate London\data\materials\agitator.xml");

                //    XmlCookedFile asdf = new XmlCookedFile();
                //    byte[] bytes = asdf.CookXmlDocument(xmlDocument);
                //    if (bytes != null)
                //        File.WriteAllBytes(
                //            @"D:\Games\Hellgate London\data\skills\consumable\summoncocomoko.xml.cooked2", bytes);
                    //asdf.Uncook(File.ReadAllBytes(@"D:\Games\Hellgate London\data\skills\consumable\summoncocomoko.xml.cooked"));

                //}
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "Reanimator_Load", false);
                MessageBox.Show(ex.Message, "Reanimator_Load");
            }
        }

        private void _LoadTables(ProgressForm progress, Object var)
        {
            try
            {
                if (!_tableFiles.LoadTableFiles(progress))
                {
                    throw new Exception("Failed to load/parse all excel and strings tables!\n" +
                        "Please ensure your directories are set correctly.\nTools > Options");
                }

#if DEBUG
                if (!_tableFiles.LoadTCv4Files(progress))
                {
                    //throw new Exception("Failed to load/parse all excel and strings tables!\n" +
                    //    "Please ensure your directories are set correctly.\nTools > Options");
                }
#endif
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "_LoadTables", true);
            }
        }

        private void _LoadAndDisplayCurrentlyLoadedExcelTables()
        {
            try
            {
                if (_tableFiles.LoadedFileCount <= 0) return;

                _tablesLoaded = new TablesLoaded(_tableDataSet) { MdiParent = this };

                foreach (DataFile dataFile in from DictionaryEntry de in _tableFiles.DataFiles
                                              select de.Value as DataFile)
                {
                    _tablesLoaded.AddItem(dataFile);
                }

                _tablesLoaded.SetBounds(_fileExplorer.Size.Width + 10, 0, 300, 350);
                _tablesLoaded.Text = String.Format("Currently Loaded Tables [{0}]", _tableFiles.LoadedFileCount);
                _tablesLoaded.Show();
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "LoadAndDisplayCurrentlyLoadedExcelTables", false);
            }
        }

        private void _SaveToolStripButton_Click(object sender, EventArgs e)
        {
            IMdiChildBase mdiChildBase = ActiveMdiChild as IMdiChildBase;
            if (mdiChildBase == null) return;

            mdiChildBase.SaveButton();
        }

        private void _ExportCSVToolStripMenuItem_Click(object sender, EventArgs e)
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
                ExceptionLogger.LogException(ex, "_ExportCSVToolStripMenuItem_Click", false);
            }
        }

        private void _HardcoreModex64DX9ToolStripMenuItem_Click(object sender, EventArgs e)
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
                ExceptionLogger.LogException(ex, "_HardcoreModex64DX9ToolStripMenuItem_Click", false);
                MessageBox.Show("Problem Applying Patch. :(");
            }
        }

        private void _ShowExcelTablesToolStripMenuItem_Click(object sender, EventArgs e)
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
                ExceptionLogger.LogException(ex, "_ShowExcelTablesToolStripMenuItem_Click", false);
            }
        }

        private void _ApplyModificationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ModificationForm modificationForm = new ModificationForm(_tableDataSet);
                modificationForm.ShowDialog();
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "_ApplyModificationsToolStripMenuItem_Click", false);
            }
        }

        private void _TradeItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //MessageBox.Show("Tables are being loaded. This may take a few seconds!");

                //ItemTransferForm transfer = new ItemTransferForm(_tableDataSet, _tableFiles);
                ComplexItemTransferForm transfer = new ComplexItemTransferForm(_tableDataSet, _tableFiles);
                //Displays a warning message before opening the item trading window.
                transfer.DisplayWarningMessage(null, null);
                transfer.ShowDialog(this);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "_TradeItemsToolStripMenuItem_Click", false);
            }
        }

        private void _SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void _ItemShopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CharacterShop shop = new CharacterShop(_tableDataSet, _tableFiles);
                shop.ShowDialog(this);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "_ItemShopToolStripMenuItem_Click", false);
            }
        }

        private void _SearchTablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TableSearch search = new TableSearch(_tableDataSet, _tableFiles);
                search.ShowDialog(this);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "_SearchTablesToolStripMenuItem_Click", false);
            }
        }

        private void _AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Reanimator by the Revival Team (c) 2009-2010" + Environment.NewLine
                + "Credits: Maeyan, Alex2069, Kite & Malachor" + Environment.NewLine
                + "For more info visit us at: http://www.hellgateaus.net", "Credits", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void _ScriptEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptEditor scriptEditor = new ScriptEditor(_tableDataSet);
                scriptEditor.MdiParent = this;
                scriptEditor.Show();
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "_ScriptEditorToolStripMenuItem_Click", false);
            }
        }

        private void _PatchToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                PatchForm patchForm = new PatchForm { MdiParent = this };
                patchForm.Show();
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "_PatchToolToolStripMenuItem_Click", false);
            }
        }

        private void _ConvertTCv4FilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Select Dump location
            FolderBrowserDialog folderBrower = new FolderBrowserDialog();
            folderBrower.SelectedPath = Config.HglDataDir;
            DialogResult dialogResult = folderBrower.ShowDialog();
            if (dialogResult == DialogResult.Cancel) return;

            // cache all tables
            ProgressForm cacheTableProgress = new ProgressForm(_LoadAllExcelTables, null);
            cacheTableProgress.SetLoadingText("Caching all tables.");
            cacheTableProgress.ShowDialog();

            // generate all tables
            ProgressForm generateTableProgress = new ProgressForm(_GenerateExcelFiles, folderBrower.SelectedPath);
            generateTableProgress.SetLoadingText("Generating all converted tables, this will take a while.");
            generateTableProgress.SetStyle(ProgressBarStyle.Marquee);
            generateTableProgress.ShowDialog();

            MessageBox.Show("Complete");
        }

        private void _GenerateExcelFiles(ProgressForm progress, object obj)
        {
            string savePath = (string)obj;

            foreach (DataTable tcDataTable in _tableDataSet.XlsDataSet.Tables)
            {
                if (!tcDataTable.TableName.Contains("_TCv4_")) continue;
                string spVersion = tcDataTable.TableName.Replace("_TCv4_", "");

                progress.SetCurrentItemText("Current table... " + spVersion);

                DataTable spDataTable = _tableDataSet.XlsDataSet.Tables[spVersion];
                DataTable convertedDataTable = ExcelFile.ConvertToSinglePlayerVersion(spDataTable, tcDataTable);
                ExcelFile excelFile = _tableDataSet.TableFiles.GetExcelTableFromId(spVersion);

                byte[] buffer = excelFile.GenerateFile(convertedDataTable);
                string path = Path.Combine(savePath, excelFile.FilePath);
                string filename = excelFile.FileName + "." + excelFile.FileExtension;

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                File.WriteAllBytes(path + filename, buffer);
            }
        }

        private void _LoadAllExcelTables(ProgressForm progress, object obj)
        {
            foreach (DictionaryEntry rawDataFile in _tableDataSet.TableFiles.DataFiles)
            {
                DataFile dataFile = (DataFile)rawDataFile.Value;
                if (dataFile.IsStringsFile) continue;
                _tableDataSet.LoadTable(progress, dataFile);
            }
        }
    }
}