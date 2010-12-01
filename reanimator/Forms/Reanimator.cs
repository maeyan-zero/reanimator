using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Reanimator.Forms;
using Hellgate;
using Revival.Common;

namespace Reanimator
{
    public partial class Reanimator : Form
    {
        private FileExplorer _fileExplorer;
        private TablesLoaded _tablesLoaded;
        private FileManager _fileManager;
        private readonly Options _optionsForm = new Options();
        private readonly List<TableForm> _openTableForms = new List<TableForm>();
        private readonly List<ExcelTableForm> _openExcelTableForms = new List<ExcelTableForm>();
        private List<HeroEditor> _openHeroEditorForms = new List<HeroEditor>();

        public Reanimator()
        {
            InitializeComponent();

            #region alexs_stuff

            if (true) return;

            //const String idxReadPath = @"D:\Games\Hellgate London\data\hellgate000.idx";
            //String idxWritePath = Path.Combine(Path.GetDirectoryName(idxReadPath),
            //                                   Path.GetFileNameWithoutExtension(idxReadPath) + ".dec.idx");
            //byte[] idxBytes = File.ReadAllBytes(idxReadPath);
            //Crypt.Decrypt(idxBytes);
            //File.WriteAllBytes(idxWritePath, idxBytes);





            //const String hashStr1 = @"data\background\catacombs\";
            //const String hashStr2 = "ct_connb_path.xml.cooked";
            //byte[] data1 = FileTools.StringToASCIIByteArray(hashStr1);
            //byte[] data2 = FileTools.StringToASCIIByteArray(hashStr2);

            //SHA1 sha = new SHA1CryptoServiceProvider();
            //byte[] result1 = sha.ComputeHash(data1);
            //byte[] result2 = sha.ComputeHash(data2);

            //byte[] cryptoBytes = Crypt.GetStringsSHA1Bytes(hashStr1, hashStr2);
            //UInt64 cryptoValue = Crypt.GetStringsSHA1UInt64(hashStr1, hashStr2);



            const String filePath = @"D:\Games\Hellgate London\MP_x64\hellgate_mp_dx9_x64.txt";
            String[] strings = File.ReadAllLines(filePath);
            foreach (String str in strings)
            {
                if (str.Length <= 37) continue;

                String subStr = str.Substring(37);
                UInt32 strHash = Crypt.GetStringHash(subStr);

                if (strHash == 3479992004) // "ROOM_LAYOUT_FLAG_AI_NODE_STONE"
                {
                    int bp = 0;
                }

                if (strHash == 3896768623) // "ENV_LIGHT_DEFINITION"
                {
                    int bp = 0;
                }

                if (strHash == 4254997457) // "DecayLFRatio"
                {
                    int bp = 0;
                }

                if (strHash == 3788988200) // "INVENTORY_VIEW_INFO"
                {
                    int bp = 0;
                }
            }




            //String str2 = "achievements.txt.cooked";
            //String str2 = @"data\background\catacombs\ct_connb_path.xml.cooked";
            //String str2 = @"data\excel\"; //3188197601 0xBE0808E1
            //UInt32 strHash2 = Crypt.GetStringHash(str2);

            //tw = new StreamWriter(@"C:\asdf.txt");
            //filestream = new FileStream(@"C:\asdf.txt", FileMode.Create, FileAccess.ReadWrite);
            _fileManager = new FileManager(Config.HglDir, Config.LoadMPVersion);
            _fileManager.LoadTableFiles();
            XmlCookedFile.Initialize(_fileManager);

            _UncookAllXml();

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
            //_DoFolder(@"D:\Games\Hellgate London\data\ai\");
            //tw.Close();
            // this.Close();
            #endregion
        }

        #region alexs_stuff
        //private TextWriter tw;

        private static void _UncookAllXml()
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            const String root = @"D:\Games\Hellgate London\data\";
            DirectoryInfo directoryInfo = new DirectoryInfo(root);
            List<String> xmlFiles = new List<String>(Directory.GetFiles(root, "*.xml.cooked", SearchOption.AllDirectories));

            int count = 0;
            foreach (String xmlFilePath in xmlFiles)
            {
                String path = xmlFilePath;
                //path = @"D:\Games\Hellgate London\data\background\tunneltransb\tunneltransb_path.xml.cooked";

                if (path.Contains("datChecksum") || path.Contains("mp_hellgate")) continue;


                XmlCookedFile xmlCookedFile = new XmlCookedFile();
                byte[] data = File.ReadAllBytes(path);

                String fileName = path.Replace(@"D:\Games\Hellgate London\", "");
                Console.WriteLine("Uncooking: " + fileName);
                //if (fileName != "test_appearance.xml.cooked") continue;
                {
                    int bp = 0;
                }

                try
                {
                    xmlCookedFile.Uncook(data);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Debug.Assert(false, "Failed to uncook: " + e);
                    continue;
                }


                xmlCookedFile.SaveXml(path.Replace(".cooked", ""));
                count++;

                XmlCookedFile recookedXmlFile = new XmlCookedFile();
                byte[] recookedData = recookedXmlFile.CookXmlDocument(xmlCookedFile.XmlDoc);
                byte[] originalHash = md5.ComputeHash(data);
                byte[] recookedHash = md5.ComputeHash(recookedData);
                if (!originalHash.SequenceEqual(recookedHash))
                {
                    File.WriteAllBytes(path + "2", recookedData);
                    int bp = 0;
                }
            }
            Console.WriteLine("XML Files Uncooked: " + count);
        }

        private static void _DoFolder(String folderDir)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(folderDir);
            FileInfo[] files = directoryInfo.GetFiles("*.xml.cooked");

            XmlCookedFile xmlAdrenaline = null;
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();

            foreach (FileInfo fileInfo in files)
            {
                XmlCookedFile xmlCookedFile = new XmlCookedFile();

                byte[] data = File.ReadAllBytes(fileInfo.FullName);
                if (fileInfo.FullName.Contains("electriclasers.xml.cooked"))
                {
                    xmlAdrenaline = xmlCookedFile;
                    int bp = 0;
                }

                Debug.Assert(xmlCookedFile.Uncook(data));

                xmlCookedFile.SaveXml(fileInfo.FullName.Replace(".cooked", ""));

                XmlCookedFile recookedXmlFile = new XmlCookedFile();
                byte[] recookedData = recookedXmlFile.CookXmlDocument(xmlCookedFile.XmlDoc);
                byte[] originalHash = x.ComputeHash(data);
                byte[] recookedHash = x.ComputeHash(recookedData);

                if (!originalHash.SequenceEqual(recookedHash))
                {
                    int bp = 0;
                }


                //String blah = xmlCookedFile.Blah();
                //if (blah != null)
                //{
                //    //tw.WriteLine(fileInfo.FullName);
                //    //tw.WriteLine(blah);
                //}
            }

            if (xmlAdrenaline != null)
            {
                // xmlAdrenaline.SaveXmlCooked(@"c:\asdf.xml.cooked");
            }
        }
        #endregion

        /// <summary>
        /// Checks the registry for the Hellgate path, if it doesn't exist prompt the user to find it.
        /// </summary>
        /// <returns>True if the installation is okay.</returns>
        private static bool CheckInstallation()
        {
            if ((Directory.Exists(Config.HglDir)))
                return true;

            string caption = "Reanimator Installation";
            string message = "Please locate your Hellgate London installation directory.\n" +
                             "For this program to work correctly, please ensure the latest Single Player patch is installed.\n" +
                             "For more information, please visit our website: http://www.hellgateaus.net";
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);

            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            DialogResult installResult;

            do
            {
                DialogResult selectPathResult = folderBrowser.ShowDialog();
                if ((selectPathResult == DialogResult.OK))
                {
                    Config.HglDir = folderBrowser.SelectedPath;
                    Config.HglDataDir = Path.Combine(Config.HglDir, "\\data");
                    return true;
                }

                caption = "Installation Error";
                message = "You must have Hellgate: London installed and the directory set to use Reanimator.";
                installResult =  MessageBox.Show(message, caption, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
            while (installResult == DialogResult.Retry);

            return false;
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = (!(String.IsNullOrEmpty(Config.LastDirectory))) ? Config.LastDirectory : Config.HglDataDir,
                Filter = "Hellgate London Files (*.*)|*.idx;*.txt.cooked;*.xls.uni.cooked;*.xml.cooked;*.hg1|" + 
                         "Index Files|*.idx|" +
                         "Excel Files|*.txt.cooked|" +
                         "String Files|*.xls.uni.cooked|" +
                         "XML Files|*.xml.cooked|" +
                         "Save Files|*.hg1"
            };

            if ((openFileDialog.ShowDialog(this) != DialogResult.OK)) return;

            string fileName = openFileDialog.FileName;
            Config.LastDirectory = Path.GetDirectoryName(fileName);

            if ((fileName.EndsWith(".idx")))
            {
                OpenIndexFile(fileName);
                return;
            }

            if ((fileName.EndsWith(".txt.cooked")))
            {
                OpenExcelFile(fileName);
                return;
            }

            if ((fileName.EndsWith(".xls.uni.cooked")))
            {
                // Open String File
                return;
            }

            if ((fileName.EndsWith(".xml.cooked")))
            {
                // Open Xml File
                return;
            }

            if ((fileName.EndsWith(".hg1")))
            {
                // Open Save File
                return;
            }
        }

        /// <summary>
        /// Opens a TableForm based on the path to a Index or StringsFile.
        /// </summary>
        /// <param name="filePath">Path to the Index or StringsFile.</param>
        private void OpenIndexFile(String filePath)
        {
            byte[] buffer;
            Hellgate.IndexFile indexFile;
            TableForm tableForm;

            // Check if the form is already open.
            // If true, then activate the form.
            bool isOpen = _openTableForms.Where(tf => tf.FilePath == filePath).Any();
            if (isOpen)
            {
                tableForm = _openTableForms.Where(tf => tf.FilePath == filePath).First();
                if ((tableForm.Created))
                {
                    tableForm.Select();
                    return;
                }
            }

            // Try read the file.
            // If an exception is caught, log the error and inform the user.
            try
            {
                buffer = File.ReadAllBytes(filePath);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, false);
                return;
            }

            // Initialize the indexFile.
            indexFile = new Hellgate.IndexFile(buffer)
            {
                FilePath = filePath
            };

            // If the Index file is initialized without error, load the form.
            // Otherwise, show a message box.
            if ((indexFile.HasIntegrity == true))
            {
                tableForm = new TableForm(indexFile)
                {
                    MdiParent = this
                };
                if (!(_openTableForms.Contains(tableForm)))
                    _openTableForms.Add(tableForm);
                tableForm.Show();
            }
            else
            {
                string message = String.Format("The index file {0} appears invalid or malformed.", filePath);
                string caption = "Bad File Format";
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Opens a ExcelTableForm based on the given path of a Excel/Strings file.
        /// </summary>
        /// <param name="filePath">Path to the file to open.</param>
        private void OpenExcelFile(String filePath)
        {
            //byte[] buffer;
            //Hellgate.ExcelFile excelFile;
            //ExcelTableForm excelTableForm;

            //// Check if the form is already open.
            //// If true, then activate the form.
            //bool isOpen = _openExcelTableForms.Where(etf => etf.FilePath == filePath).Any();
            //if (isOpen)
            //{
            //    excelTableForm = _openExcelTableForms.Where(etf => etf.FilePath == filePath).First();
            //    if ((excelTableForm.Created))
            //    {
            //        excelTableForm.Select();
            //        return;
            //    }
            //}

            //// Try read the file.
            //// If an exception is caught, log the error and inform the user.
            //try
            //{
            //    buffer = File.ReadAllBytes(filePath);
            //}
            //catch (Exception ex)
            //{
            //    ExceptionLogger.LogException(ex, false);
            //    return;
            //}

            //// Initialize the ExcelFile.
            //excelFile = new Hellgate.ExcelFile(buffer)
            //{
            //    FilePath = filePath
            //};

            //// If the Excel file is initialized without error, load the form.
            //// Otherwise, show a message box.
            //if ((excelFile.IntegrityCheck == true))
            //{
            //    excelTableForm = new ExcelTableForm(excelFile)
            //    {
            //        MdiParent = this
            //    };
            //    if (!(_openExcelTableForms.Contains(excelTableForm)))
            //        _openExcelTableForms.Add(excelTableForm);
            //    excelTableForm.Show();
            //}
            //else
            //{
            //    string message = String.Format("The excel file {0} appears invalid or malformed.", filePath);
            //    string caption = "Bad File Format";
            //    MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }


        private void OpemXmlFile(String filePath)
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


        //private void _OpenFileHg1(String fileName)
        //{
        //    try
        //    {
        //        Unit heroUnit = UnitHelpFunctions.OpenCharacterFile(_tableDataSet, fileName);

        //        //Unit wrapper test
        //        //UnitWrapper wrapper = new UnitWrapper(heroUnit);
        //        //wrapper.Mode.IsElite = true;
        //        ////wrapper.Mode.IsElite = true;
        //        ////UnitWrapper w = new UnitWrapper(wrapper.Items.Items[2]);

        //        ////UnitWrapper drone = new UnitWrapper(wrapper.Drone.Drone);
        //        ////CharacterValues values = drone.Values;

        //        //UnitHelpFunctions.SaveCharacterFile(heroUnit, @"F:\test.hg1");

        //        //Comment me when testing the unit wrapper!!!

        //        HeroEditor2 heroEditor = new HeroEditor2(fileName, _tableDataSet)
        //        {
        //            Text = "Hero Editor: " + fileName,
        //            MdiParent = this
        //        };
        //        heroEditor.Show();
        //        //if (heroUnit.IsGood)
        //        //{
        //        //    HeroEditor heroEditor = new HeroEditor(heroUnit, _tableDataSet, fileName)
        //        //    {
        //        //        Text = "Hero Editor: " + fileName,
        //        //        MdiParent = this
        //        //    };
        //        //    heroEditor.Show();
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogger.LogException(ex, "OpenFileHg1", false);
        //    }
        //}


        //private void _OpenFileStrings(String fileName)
        //{
        //    // todo: make me neater etc - i.e. merge with cooked file above
        //    // copy-paste for most part
        //    try
        //    {
        //        String name = Path.GetFileNameWithoutExtension(fileName);


        //        // todo: this doesn't work 100% as string IDs are stored with each first letter capitalized
        //        DataFile excelTable = _tableFiles.GetTableFromFileName(name);
        //        // todo: Add check for file differing from what's in dataset, and open as new file if different etc
        //        if (excelTable == null)
        //        {
        //            MessageBox.Show("TODO");
        //            return;
        //        }

        //        ExcelTableForm excelTableForm = new ExcelTableForm(excelTable, _tableDataSet)
        //        {
        //            Text = "Excel Table: " + fileName,
        //            MdiParent = this
        //        };
        //        excelTableForm.Show();
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogger.LogException(ex, "OpenFileCooked", false);
        //    }
        //}

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
            _optionsForm.ShowDialog(this);
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


                if (CheckInstallation())
                {
                    _fileManager = new FileManager(Config.HglDir, Config.LoadMPVersion);
                    if (!_fileManager.LoadTableFiles())
                    {
                        MessageBox.Show("Failed to load excel files!", "Excel Table Error", MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                    }
                    _tablesLoaded = new TablesLoaded(_fileManager) { MdiParent = this };
                    _tablesLoaded.Show();

                    _fileExplorer = new FileExplorer(_fileManager) { MdiParent = this };
                    _fileExplorer.Show();

                    XmlCookedFile.Initialize(_fileManager);
                }


                //ProgressForm fileExplorerProgress = new ProgressForm(_fileExplorer.LoadIndexFiles, Config.LoadMPVersion);
                //fileExplorerProgress.ShowDialog(this);
                //_fileExplorer.Show();

                ////_indexFiles = _fileExplorer.IndexFiles;
                //_tableFiles.FileExplorer = _fileExplorer;

                //ProgressForm progress = new ProgressForm(_LoadTables, null);
                //progress.ShowDialog(this);
                //_tableDataSet.TableFiles = _tableFiles;
                //_LoadAndDisplayCurrentlyLoadedExcelTables();

                //XmlCookedFile.Initialize(_tableDataSet);

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

        private void _SaveToolStripButton_Click(object sender, EventArgs e)
        {
            IMdiChildBase mdiChildBase = ActiveMdiChild as IMdiChildBase;
            if (mdiChildBase == null) return;

            mdiChildBase.SaveButton();
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
            //try
            //{
            //    if (showExcelTablesToolStripMenuItem.Checked)
            //    {
            //        _tablesLoaded.StartPosition = FormStartPosition.Manual;
            //        _tablesLoaded.Location = new Point(0, 0);
            //        _tablesLoaded.Show();
            //    }
            //    else
            //    {
            //        _tablesLoaded.Hide();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ExceptionLogger.LogException(ex, "_ShowExcelTablesToolStripMenuItem_Click", false);
            //}
        }

        private void _ApplyModificationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    ModificationForm modificationForm = new ModificationForm(_tableDataSet);
            //    modificationForm.ShowDialog();
            //}
            //catch (Exception ex)
            //{
            //    ExceptionLogger.LogException(ex, "_ApplyModificationsToolStripMenuItem_Click", false);
            //}
        }

        private void _TradeItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    //MessageBox.Show("Tables are being loaded. This may take a few seconds!");

            //    //ItemTransferForm transfer = new ItemTransferForm(_tableDataSet, _tableFiles);
            //    ItemTransferForm transfer = new ItemTransferForm(_tableDataSet, _fileExplorer);
            //    //Displays a warning message before opening the item trading window.
            //    transfer.DisplayWarningMessage(null, null);
            //    transfer.ShowDialog(this);
            //    transfer.Dispose();
            //}
            //catch (Exception ex)
            //{
            //    ExceptionLogger.LogException(ex, "_TradeItemsToolStripMenuItem_Click", false);
            //}
        }

        private void _SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void _ItemShopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    CharacterShop shop = new CharacterShop(_tableDataSet, _tableFiles);
            //    shop.ShowDialog(this);
            //}
            //catch (Exception ex)
            //{
            //    ExceptionLogger.LogException(ex, "_ItemShopToolStripMenuItem_Click", false);
            //}
        }

        private void _SearchTablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    TableSearch search = new TableSearch(_tableDataSet, _tableFiles);
            //    search.ShowDialog(this);
            //}
            //catch (Exception ex)
            //{
            //    ExceptionLogger.LogException(ex, "_SearchTablesToolStripMenuItem_Click", false);
            //}
        }

        private void _AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Reanimator by the Revival Team (c) 2009-2010" + Environment.NewLine
                + "Credits: Maeyan, Alex2069, Kite & Malachor" + Environment.NewLine
                + "For more info visit us at: http://www.hellgateaus.net", "Credits", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void _ScriptEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    ScriptEditor scriptEditor = new ScriptEditor(_tableDataSet);
            //    scriptEditor.MdiParent = this;
            //    scriptEditor.Show();
            //}
            //catch (Exception ex)
            //{
            //    ExceptionLogger.LogException(ex, "_ScriptEditorToolStripMenuItem_Click", false);
            //}
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

        private void _ConvertTestCenterFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //// Select Dump location
            //FolderBrowserDialog folderBrower = new FolderBrowserDialog();
            //folderBrower.SelectedPath = Config.HglDataDir;
            //DialogResult dialogResult = folderBrower.ShowDialog();
            //if (dialogResult == DialogResult.Cancel) return;

            //// cache all tables
            //ProgressForm cacheTableProgress = new ProgressForm(_LoadAllExcelTables, null);
            //cacheTableProgress.SetLoadingText("Caching all tables.");
            //cacheTableProgress.ShowDialog();

            //// generate all tables
            //ProgressForm generateTableProgress = new ProgressForm(_ConvertTestCenterFiles, folderBrower.SelectedPath);
            //generateTableProgress.SetLoadingText("Generating all converted tables, this will take a while.");
            //generateTableProgress.SetStyle(ProgressBarStyle.Marquee);
            //generateTableProgress.ShowDialog();

            //MessageBox.Show("Complete");
        }

        private void _SaveSinglePlayerFiles(ProgressForm progress, object obj)
        {
        //    string savePath = (string)obj;

        //    foreach (DataTable spDataTable in _tableDataSet.XlsDataSet.Tables)
        //    {
        //        if (spDataTable.TableName.Contains("_TCv4_")) continue;
        //        if (spDataTable.TableName.Contains("Strings_")) continue;

        //        progress.SetCurrentItemText("Current table... " + spDataTable.TableName);

        //        ExcelFile spExcelFile = _tableDataSet.TableFiles.GetExcelTableFromId(spDataTable.TableName);

        //        byte[] buffer = spExcelFile.GenerateFile(spDataTable);
        //        string path = Path.Combine(savePath, spExcelFile.FilePath);
        //        string filename = spExcelFile.FileName + "." + spExcelFile.FileExtension;

        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }

        //        File.WriteAllBytes(path + filename, buffer);
        //    }
        }

        //private void _ConvertTestCenterFiles(ProgressForm progress, object obj)
        //{
        //    string savePath = (string)obj;

        //    foreach (DataTable tcDataTable in _tableDataSet.XlsDataSet.Tables)
        //    {
        //        if (!tcDataTable.TableName.Contains("_TCv4_")) continue;
        //        string spVersion = tcDataTable.TableName.Replace("_TCv4_", "");

        //        progress.SetCurrentItemText("Current table... " + spVersion);

        //        DataTable spDataTable = _tableDataSet.XlsDataSet.Tables[spVersion];
        //        DataTable convertedDataTable = ExcelFile.ConvertToSinglePlayerVersion(spDataTable, tcDataTable);
        //        ExcelFile tcExcelFile = _tableDataSet.TableFiles.GetExcelTableFromId(tcDataTable.TableName);
        //        ExcelFile spExcelFile = _tableDataSet.TableFiles.GetExcelTableFromId(spVersion);

        //        spExcelFile.MyshBytes = tcExcelFile.MyshBytes;

        //        byte[] buffer = spExcelFile.GenerateFile(convertedDataTable);
        //        string path = Path.Combine(savePath, spExcelFile.FilePath);
        //        string filename = spExcelFile.FileName + "." + spExcelFile.FileExtension;

        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }

        //        File.WriteAllBytes(path + filename, buffer);
        //    }
        //}

        //private void _LoadAllExcelTables(ProgressForm progress, object obj)
        //{
        //    foreach (DictionaryEntry rawDataFile in _tableDataSet.TableFiles.DataFiles)
        //    {
        //        DataFile dataFile = (DataFile)rawDataFile.Value;
        //        if (dataFile.IsStringsFile) continue;
        //        _tableDataSet.LoadTable(progress, dataFile);
        //    }
        //}

        //private void saveSinglePlayerFilesToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    // Select Dump location
        //    FolderBrowserDialog folderBrower = new FolderBrowserDialog();
        //    folderBrower.SelectedPath = Config.HglDataDir;
        //    DialogResult dialogResult = folderBrower.ShowDialog();
        //    if (dialogResult == DialogResult.Cancel) return;

        //    // cache all tables
        //    ProgressForm cacheTableProgress = new ProgressForm(_LoadAllExcelTables, null);
        //    cacheTableProgress.SetLoadingText("Caching all tables.");
        //    cacheTableProgress.ShowDialog();

        //    // generate all tables
        //    ProgressForm generateTableProgress = new ProgressForm(_SaveSinglePlayerFiles, folderBrower.SelectedPath);
        //    generateTableProgress.SetLoadingText("Saving all single player files, this will take a while.");
        //    generateTableProgress.SetStyle(ProgressBarStyle.Marquee);
        //    generateTableProgress.ShowDialog();

        //    MessageBox.Show("Complete");
        //}
    }
}