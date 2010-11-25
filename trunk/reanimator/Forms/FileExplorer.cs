using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Hellgate;

namespace Reanimator.Forms
{
    public partial class FileExplorer : Form
    {
        private readonly FileManager _fileManager;

        public FileExplorer(FileManager fileManager)
        {
            // init stuffs
            InitializeComponent();
            _files_fileTreeView.DoubleBuffered(true);
            _fileManager = fileManager;
            backupKey_label.ForeColor = BackupColor;
            noEditorKey_label.ForeColor = NoEditColor;

            // load icons
            ImageList imageList = new ImageList { ColorDepth = ColorDepth.Depth32Bit };
            foreach (Icon icon in Icons)
            {
                imageList.Images.Add(icon);
            }
            _files_fileTreeView.ImageList = imageList;

            // generate tree data
            _files_fileTreeView.BeginUpdate();
            _GenerateFileTree();
            _files_fileTreeView.TreeViewNodeSorter = new NodeSorter();
            _files_fileTreeView.EndUpdate();
        }

        private void _FilesTreeView_AfterSelect(Object sender, TreeViewEventArgs e)
        {
            TreeView treeView = (TreeView)sender;
            TreeNode selectedNode = treeView.SelectedNode;
            NodeObject nodeObject = (NodeObject)selectedNode.Tag;

            fileName_textBox.DataBindings.Clear();
            fileSize_textBox.DataBindings.Clear();
            fileCompressed_textBox.DataBindings.Clear();
            loadingLocation_textBox.DataBindings.Clear();


            // if it's a folder, it's rather boring
            if (nodeObject.IsFolder)
            {
                fileName_textBox.Text = selectedNode.Text;
                fileSize_textBox.Text = String.Empty;
                fileCompressed_textBox.Text = String.Empty;
                loadingLocation_textBox.Text = String.Empty;
                fileTime_textBox.Text = String.Empty;

                return;
            }


            IndexFile.FileEntry fileIndex = nodeObject.FileEntry;

            // no file index means it's either an uncooked file, or a new file
            if (fileIndex == null)
            {
                // todo: this entires if-block is a little dodgy and assumes alot

                // assuming only uncooked at the moment
                Debug.Assert(nodeObject.IsUncookedVersion);

                // if it's the uncooked version, we need to use the parents node path
                TreeNode parentNode = selectedNode.Parent;
                String filePath = Path.Combine(Config.HglDir, parentNode.Name.Replace(".cooked", ""));

                FileInfo fileInfo;
                try
                {
                    fileInfo = new FileInfo(filePath);
                }
                catch (Exception ex)
                {
                    // todo remove file from tree as it has been moved(?)
                    return;
                }

                fileName_textBox.Text = fileInfo.Name;
                fileName_textBox.ReadOnly = true;
                fileSize_textBox.Text = fileInfo.Length.ToString();
                fileCompressed_textBox.Text = String.Empty;
                fileTime_textBox.Text = fileInfo.CreationTime.ToString();
                loadingLocation_textBox.Text = filePath;

                return;
            }


            fileName_textBox.ReadOnly = false;
            fileName_textBox.DataBindings.Add("Text", fileIndex, "FileNameString");
            fileSize_textBox.DataBindings.Add("Text", fileIndex, "UncompressedSize");
            fileCompressed_textBox.DataBindings.Add("Text", fileIndex, "CompressedSize");
            fileTime_textBox.Text = (DateTime.FromFileTime(fileIndex.FileStruct.FileTime)).ToString();

            if (fileIndex.IsBackup)
            {
                String fileDataPath = Path.Combine(fileIndex.DirectoryString.Replace(IndexFile.BackupPrefix, ""), fileIndex.FileNameString);
                String filePath = Config.HglDir + fileDataPath;
                if (File.Exists(filePath))
                {
                    loadingLocation_textBox.Text = filePath;
                }
                else
                {
                    loadingLocation_textBox.DataBindings.Add("Text", fileIndex, "InIndex");
                }
            }
            else
            {
                loadingLocation_textBox.DataBindings.Add("Text", fileIndex, "InIndex");
            }
        }

        private void _FilesTreeView_DoubleClick(Object sender, EventArgs e)
        {
            TreeView treeView = (TreeView)sender;
            TreeNode selectedNode = treeView.SelectedNode;
            NodeObject nodeObject = (NodeObject)selectedNode.Tag;
            Debug.Assert(nodeObject != null);

            if (nodeObject.IsFolder || !nodeObject.CanEdit) return;

            String nodeFullPath = selectedNode.FullPath;
            String filePath = Path.Combine(Config.HglDir, nodeFullPath); // todo: need to remove nested filename from path for uncooked files
            String editorPath = null;

            // todo: this section needs a good cleaning
            // todo: implementation of choosing default program in the options menu
            // todo: current implementation overwites already extracted/uncooked file without asking - open it instead or ask?
            if (nodeFullPath.EndsWith(ExcelFile.FileExtention))
            {
                 MessageBox.Show("todo");
            }
            else if (nodeFullPath.EndsWith(StringsFile.FileExtention))
            {
                MessageBox.Show("todo");
            }
            else if (nodeFullPath.EndsWith(XmlCookedFile.FileExtention))
            {
                IndexFile.FileEntry fileIndex = nodeObject.FileEntry;
                String xmlDataPath = Path.Combine(Config.HglDir, nodeFullPath.Replace(".cooked", ""));

                byte[] fileData = _fileManager.GetFileBytes(fileIndex);
                if (fileData == null)
                {
                    MessageBox.Show("Failed to read xml.cooked from source!", "Error", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }

                XmlCookedFile xmlCookedFile = new XmlCookedFile();
                if (!xmlCookedFile.Uncook(fileData))
                {
                    MessageBox.Show("Failed to uncook xml file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(xmlDataPath));
                    xmlCookedFile.SaveXml(xmlDataPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to save uncooked xml file!\n\n" + ex, "Error", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }

                DialogResult drOpen = MessageBox.Show("Uncooked XML file saved at " + xmlDataPath + "\n\nOpen with editor?", "Success",
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (drOpen == DialogResult.Yes)
                {
                    try
                    {
                        Process notePad = new Process { StartInfo = { FileName = Config.XmlEditor, Arguments = xmlDataPath } };
                        notePad.Start();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to start editor!\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (nodeFullPath.EndsWith(".xml"))
            {
                editorPath = Config.XmlEditor;
            }
            else if (nodeFullPath.EndsWith(".txt"))
            {
                editorPath = Config.TxtEditor;
            }
            else if (nodeFullPath.EndsWith(".csv"))
            {
                editorPath = Config.CsvEditor;
            }
            else
            {
                MessageBox.Show("Unexpected editable file!\n(this shouldn't happen - please report this)", "Warning",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


            if (String.IsNullOrEmpty(editorPath) || String.IsNullOrEmpty(filePath)) return;

            try
            {
                Process process = new Process { StartInfo = { FileName = editorPath, Arguments = filePath } };
                process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to start editor!\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void _ExtractButton_Click(object sender, EventArgs e)
        {
            // make sure we have at least 1 checked file
            List<TreeNode> checkedNodes = new List<TreeNode>();
            if (_GetCheckedNodes(_files_fileTreeView.Nodes, checkedNodes) == 0)
            {
                MessageBox.Show("No files checked for extraction!", "Need Tables", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            // where do we want to save it
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog { SelectedPath = Config.HglDir };
            if (folderBrowserDialog.ShowDialog(this) != DialogResult.OK) return;

            // do we want to keep the directory structure?
            DialogResult drKeepStructure = MessageBox.Show("Keep directory structure?", "Path", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (drKeepStructure == DialogResult.Cancel) return;

            ExtractPackPatchArgs extractPatchArgs = new ExtractPackPatchArgs
            {
                KeepStructure = (drKeepStructure == DialogResult.Yes),
                PatchFiles = false,
                RootDir = folderBrowserDialog.SelectedPath,
                CheckedNodes = checkedNodes
            };

            ProgressForm progressForm = new ProgressForm(_DoExtractPatch, extractPatchArgs);
            progressForm.SetLoadingText(String.Format("Extracting file(s)... ({0})", checkedNodes.Count));
            progressForm.Show(this);
        }

        private void _ExtractPatchButton_Click(object sender, EventArgs e)
        {
            // make sure we have at least 1 checked file
            List<TreeNode> checkedNodes = new List<TreeNode>();
            if (_GetCheckedNodes(_files_fileTreeView.Nodes, checkedNodes) == 0)
            {
                MessageBox.Show("No files checked for extraction!", "Need Tables", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            DialogResult dialogResult = MessageBox.Show(
                "Extract & Patch out checked file's?",
                "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.No) return;

            ExtractPackPatchArgs extractPatchArgs = new ExtractPackPatchArgs
            {
                KeepStructure = true,
                PatchFiles = true,
                RootDir = Config.HglDir,
                CheckedNodes = checkedNodes
            };

            ProgressForm progressForm = new ProgressForm(_DoExtractPatch, extractPatchArgs);
            progressForm.SetLoadingText(String.Format("Extracting and Patching file(s)... ({0})", checkedNodes.Count));
            progressForm.Show(this);
        }

        private void _DoExtractPatch(ProgressForm progressForm, Object param)
        {
            ExtractPackPatchArgs extractPatchArgs = (ExtractPackPatchArgs)param;
            DialogResult overwrite = DialogResult.None;
            Hashtable indexToWrite = new Hashtable();

            const int progressStepRate = 50;
            progressForm.ConfigBar(1, extractPatchArgs.CheckedNodes.Count, progressStepRate);
            progressForm.SetCurrentItemText("Extracting file(s)...");

            if (!_fileManager.BeginAllDatReadAccess())
            {
                MessageBox.Show(
                    "Failed to open dat files for reading!\nEnsure no other programs are using them and try again.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int i = 0;
            foreach (TreeNode extractNode in extractPatchArgs.CheckedNodes)
            {
                if (i % 50 == 0)
                {
                    progressForm.SetCurrentItemText(extractNode.FullPath);
                }
                i++;

                if (_ExtractPatchFile(extractNode, ref overwrite, indexToWrite, extractPatchArgs)) continue;

                if (overwrite != DialogResult.Cancel)
                {
                    MessageBox.Show("Unexpected error, extraction process terminated!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                return;
            }
            _fileManager.EndAllDatAccess();

            // are we patching?
            if (!extractPatchArgs.PatchFiles)
            {
                MessageBox.Show("Extraction process completed sucessfully!", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (indexToWrite.Count == 0)
            {
                MessageBox.Show("Extraction process completed sucessfully!\nNo index files require modifications.", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            progressForm.SetCurrentItemText("Performing index modifications...");
            foreach (IndexFile idx in
                from DictionaryEntry indexDictionary in indexToWrite select (IndexFile)indexDictionary.Value)
            {
                byte[] idxData = idx.ToByteArray();
                Crypt.Encrypt(idxData);
                File.WriteAllBytes(idx.FilePath, idxData);
            }
            MessageBox.Show("Index modification process completed sucessfully!", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private bool _ExtractPatchFile(TreeNode treeNode, ref DialogResult overwrite, Hashtable indexToWrite, ExtractPackPatchArgs extractPatchArgs)
        {
            if (treeNode == null) return false;

            //if (treeNode.Text == "affixes.txt.cooked")
            //{
            //    int bp = 0;
            //}

            // loop through for folders, etc
            NodeObject nodeObject = (NodeObject)treeNode.Tag;
            if (nodeObject.IsFolder)
            {
                foreach (TreeNode childNode in treeNode.Nodes)
                {
                    if (!_ExtractPatchFile(childNode, ref overwrite, indexToWrite, extractPatchArgs)) return false;
                }

                return true;
            }


            // make sure we want to extract this file
            if (!treeNode.Checked || nodeObject.Index == null || nodeObject.FileEntry == null) return true;


            // get path
            IndexFile.FileEntry file = nodeObject.FileEntry;
            String filePath = extractPatchArgs.KeepStructure
                                  ? Path.Combine(extractPatchArgs.RootDir, treeNode.FullPath)
                                  : Path.Combine(extractPatchArgs.RootDir, file.FileNameString);


            // does it exist?
            bool fileExists = File.Exists(filePath);
            if (fileExists && overwrite == DialogResult.None)
            {
                overwrite = MessageBox.Show("An extract file already exists, do you wish to overwrite the file, and all following?\nTablele: " + filePath,
                    "Question", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (overwrite == DialogResult.Cancel) return false;
            }
            if (fileExists && overwrite == DialogResult.No) return true;


            // save file
            DialogResult extractDialogResult = DialogResult.Retry;
            while (extractDialogResult == DialogResult.Retry)
            {
                byte[] fileBytes = _fileManager.GetFileBytes(file);
                if (fileBytes == null)
                {
                    extractDialogResult = MessageBox.Show("Failed to read file from .dat! Try again?", "Error",
                                                   MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);

                    if (extractDialogResult == DialogResult.Abort)
                    {
                        overwrite = DialogResult.Cancel;
                        return false;
                    }

                    continue;
                }

                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                File.WriteAllBytes(filePath, fileBytes);
                break;
            }


            // are we patching?
            if (!extractPatchArgs.PatchFiles) return true;


            // don't patch out string files or music/movie files
            if (file.FileNameString.EndsWith(StringsFile.FileExtention) ||
                file.FileNameString.EndsWith(".ogg") ||
                file.FileNameString.EndsWith(".mp2") ||
                file.FileNameString.EndsWith(".bik")) return true;


            // if we're patching out the file, then change its bgColor and set its nodeObject state to backup
            treeNode.ForeColor = BackupColor;
            nodeObject.IsBackup = true;


            // is this file located else where? (i.e. does it have Siblings)
            String fileIndexKey;
            if (nodeObject.Siblings != null && nodeObject.Siblings.Count > 0)
            {
                // this file has siblings - loop through
                foreach (NodeObject siblingNodeObject in nodeObject.Siblings)
                {
                    IndexFile.FileEntry siblingFileEntry = siblingNodeObject.FileEntry;
                    IndexFile siblingIndex = null; // todo: rewrite siblingFileEntry.InIndex;

                    siblingIndex.PatchOutFile(siblingNodeObject.FileEntry);

                    fileIndexKey = siblingIndex.FileNameWithoutExtension;
                    if (!indexToWrite.ContainsKey(fileIndexKey))
                    {
                        indexToWrite.Add(fileIndexKey, siblingIndex);
                    }
                }
            }


            // now patch the curr file as well
            // only add index to list if it needs to be
            IndexFile fileIndex = nodeObject.Index;
            if (!fileIndex.PatchOutFile(file)) return true;


            // add index to indexToWrite list
            fileIndexKey = fileIndex.FileNameWithoutExtension;
            if (!indexToWrite.ContainsKey(fileIndexKey))
            {
                // todo: rewrite indexToWrite.Add(fileIndexKey, file.InIndex);
            }
            return true;
        }

        private void _PackPatchButton_Click(object sender, EventArgs e)
        {
            // make sure we have at least 1 checked file
            List<TreeNode> checkedNodes = new List<TreeNode>();
            if (_GetCheckedNodes(_files_fileTreeView.Nodes, checkedNodes) == 0)
            {
                MessageBox.Show("No files checked for packing!", "Need Tables", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            // get our custom index - or create if doesn't exist
            IndexFile packIndex = null; // todo: rewrite IndexFiles.FirstOrDefault(index => index.FileNameWithoutExtension == ReanimatorIndex);
            if (packIndex == null)
            {
                String indexPath = String.Format(@"data\{0}.idx", ReanimatorIndex);
                indexPath = Path.Combine(Config.HglDir, indexPath);
                try
                {
                    File.Create(indexPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to create custom index file!\n\n" + ex, "Error", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }

                // todo: rewrite packIndex = new Index(indexPath);
                // todo: rewrite IndexFiles.Add(packIndex);
            }

            ExtractPackPatchArgs extractPackPatchArgs = new ExtractPackPatchArgs
            {
                PackIndex = packIndex,
                RootDir = Config.HglDir,
                CheckedNodes = checkedNodes,
                PatchFiles = false
            };

            _files_fileTreeView.BeginUpdate();
            ProgressForm progressForm = new ProgressForm(_DoPackPatch, extractPackPatchArgs);
            progressForm.Disposed += delegate { _files_fileTreeView.EndUpdate(); };
            progressForm.SetLoadingText("Packing and Patching files...");
            progressForm.Show();
        }

        private void _DoPackPatch(ProgressForm progressForm, Object param)
        {
            ExtractPackPatchArgs args = (ExtractPackPatchArgs)param;

            // find which checked nodes actually have files we can pack
            StringWriter packResults = new StringWriter();

            String state = String.Format("Checking {0} file(s) for packing...", args.CheckedNodes.Count);
            const int packCheckStep = 200;
            progressForm.SetLoadingText(state);
            progressForm.ConfigBar(0, args.CheckedNodes.Count, packCheckStep);
            packResults.WriteLine(state);

            int i = 0;
            List<TreeNode> packNodes = new List<TreeNode>();
            foreach (TreeNode checkedNode in args.CheckedNodes)
            {
                String filePath = Path.Combine(args.RootDir, checkedNode.FullPath);

                if (i % packCheckStep == 0)
                {
                    progressForm.SetCurrentItemText(filePath);
                }
                i++;

                // ensure exists
                if (!File.Exists(filePath))
                {
                    packResults.WriteLine("{0} - Table Not Found", filePath);
                    continue;
                }

                // ensure it was once packed (need FilePathHash etc)
                // todo: implement Crypt.StringHash for FilePathHash and FolderPathHash
                NodeObject nodeObject = (NodeObject)checkedNode.Tag;
                if (nodeObject.FileEntry == null ||
                    nodeObject.FileEntry.FileStruct == null ||
                    nodeObject.FileEntry.FileStruct.FileNameSHA1Hash == 0)
                {
                    packResults.WriteLine("{0} - Table Has No Base Version", filePath);
                    continue;
                }

                packResults.WriteLine(filePath);
                packNodes.Add(checkedNode);
            }


            // write our error log if we have it
            const String packResultsFile = "packResults.log";
            if (packNodes.Count != args.CheckedNodes.Count)
            {
                try
                {
                    File.WriteAllText(packResultsFile, packResults.ToString());
                }
                catch (Exception e)
                {
                    MessageBox.Show("Failed to write to log file!\n\n" + e, "Warning", MessageBoxButtons.OK,
                                    MessageBoxIcon.Exclamation);
                }


                // can we pack any?
                if (packNodes.Count == 0)
                {
                    String errorMsg =
                        String.Format("None of the {0} files were able to be packed!\nSee {1} for more details.",
                                      args.CheckedNodes.Count, packResultsFile);
                    MessageBox.Show(errorMsg, "Error", MessageBoxButtons.OK,
                                    MessageBoxIcon.Exclamation);
                    return;
                }
                else
                {
                    String errorMsg =
                        String.Format("Of the {0} files checked, only {1} will be able to be packed.\nSee {2} for more details.\n\nContinue with packing and patching process?",
                                      args.CheckedNodes.Count, packNodes.Count, packResultsFile);
                    DialogResult continuePacking = MessageBox.Show(errorMsg, "Notice", MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Information);
                    if (continuePacking == DialogResult.No) return;
                }
            }


            // pack our files
            // todo: rewrite
            //if (!_BeginDatAccess(IndexFiles, true))
            //{
            //    MessageBox.Show(
            //        "Failed to open dat files for writing!\nEnsure no other programs are using them and try again.",
            //        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            state = String.Format("Packing {0} files...", packNodes.Count);
            progressForm.ConfigBar(0, packNodes.Count, packCheckStep);
            progressForm.SetLoadingText(state);
            packResults.WriteLine(state);

            i = 0;
            bool allNodesPacked = true;
            bool datNeedsCleaning = false;
            foreach (TreeNode packNode in packNodes)
            {
                NodeObject oldNodeObject = (NodeObject)packNode.Tag;

                if (i % packCheckStep == 0)
                {
                    progressForm.SetCurrentItemText(packNode.FullPath);
                }
                i++;

                // add to our custom index if not already present
                IndexFile.FileEntry fileEntry = null; // todo: rewrite args.PackIndex.GetFileFromIndex(packNode.FullPath);
                if (fileEntry == null)
                {
                    //fileEntry = args.PackIndex.AddFileToIndex(oldNodeObject.FileEntry);
                }
                else
                {
                    // file exists - we'll need to clean the dat afterwards and remove orphaned data bytes
                    datNeedsCleaning = true;
                }

                // update fileTime to now - ensures it will override older versions
                fileEntry.FileStruct.FileTime = DateTime.Now.ToFileTime();

                // read in file data
                String filePath = Path.Combine(Config.HglDir, packNode.FullPath);
                byte[] fileData;
                try
                {
                    fileData = File.ReadAllBytes(filePath);
                }
                catch (Exception)
                {
                    packResults.WriteLine("{0} - Failed to read file data", filePath);
                    allNodesPacked = false;
                    continue;
                }

                // append to dat file
                try
                {
                    // todo: rewite args.PackIndex.AddFileToDat(fileData, fileEntry);
                }
                catch (Exception)
                {
                    packResults.WriteLine("{0} - Failed to add to data file", filePath);
                    allNodesPacked = false;
                    continue;
                }

                packResults.WriteLine(filePath);

                // update our node object while we're here
                oldNodeObject.AddSibling(oldNodeObject);
                NodeObject newNodeObject = new NodeObject
                {
                    Siblings = oldNodeObject.Siblings,
                    CanEdit = oldNodeObject.CanEdit,
                    FileEntry = fileEntry,
                    Index = args.PackIndex,
                    IsBackup = false,
                    IsFolder = false
                };

                packNode.Tag = newNodeObject;
                packNode.ForeColor = BaseColor;
            }


            // were all files packed?
            if (!allNodesPacked)
            {
                try
                {
                    File.WriteAllText(packResultsFile, packResults.ToString());
                }
                catch (Exception e)
                {
                    MessageBox.Show("Failed to write to log file!\n\n" + e, "Warning", MessageBoxButtons.OK,
                                    MessageBoxIcon.Exclamation);
                }

                String warningMsg = String.Format("Not all files were packed!\nCheck {0} for more details.",
                                                packResultsFile);
                MessageBox.Show(warningMsg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


            // do we need to clean our dat?
            progressForm.SetLoadingText("Generating and saving files...");
            progressForm.SetStyle(ProgressBarStyle.Marquee);
            if (datNeedsCleaning)
            {
                progressForm.SetCurrentItemText("Removing orphan data...");
                // todo: rewrite args.PackIndex.RebuildDatFile();
            }
            _fileManager.EndAllDatAccess();


            // write updated index
            progressForm.SetCurrentItemText("Writing update dat index...");
            try
            {
                byte[] idxBytes = args.PackIndex.ToByteArray();
                Crypt.Encrypt(idxBytes);
                File.WriteAllBytes(args.PackIndex.FilePath, idxBytes);
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to write updated index file!\n\n" + e, "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Table packing and idx/dat writing completed!", "Success", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }

        private void _RevertRestoreButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Todo");
        }

        private void _FilesTreeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            NodeObject nodeObject = (NodeObject)e.Node.Tag;
            if (!nodeObject.IsFolder) return;

            e.Node.ImageIndex = (int)IconIndex.FolderOpen;
            e.Node.SelectedImageIndex = e.Node.ImageIndex;
        }

        private void _FilesTreeView_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            NodeObject nodeObject = (NodeObject)e.Node.Tag;
            if (!nodeObject.IsFolder) return;

            e.Node.ImageIndex = (int)IconIndex.Folder;
            e.Node.SelectedImageIndex = e.Node.ImageIndex;
        }

        private void FileExplorer_Shown(object sender, EventArgs e)
        {
            _files_fileTreeView.EndUpdate();
        }

        private void _FilesTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            _files_fileTreeView.AfterCheck -= _FilesTreeView_AfterCheck;
            //files_treeView.BeginUpdate();

            /* note: even after disabling the check event, the performance is still the same.
             * This is only left here for the meantime for demonstrative/debugging purposes.
             * >> Alex tested this and found it to be siginifcantly better with event removed - debug vs release compiles maybe?
             * 
             * After much testing - it isn't the looping/events/function calls causing the lag (at least, not the most siginificant)
             * it's the .Checked setter that seriously kills it (.Checked getter has no issues)
             * 
             * Also, Begin/End Update - while it might help on excessivly large amounts of check boxes,
             * on only small nodes it causes a huge noticable lag... (wtf?)
             * 
             * todo: possibly investigate reflection and private state member modification
             */
            _CheckChildNodes(e.Node);

            //files_treeView.EndUpdate();
            _files_fileTreeView.AfterCheck += _FilesTreeView_AfterCheck;
        }

        private static void _CheckChildNodes(TreeNode parentNode)
        {
            foreach (TreeNode childNode in parentNode.Nodes)
            {
                childNode.Checked = parentNode.Checked;
                if (childNode.Nodes.Count > 0)
                {
                    _CheckChildNodes(childNode);
                }
            }
        }

        private void _FilterApplyButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("todo");
            //_DoApplyFilter();

            //byte[] asdf = GetFileBytes(@"data\ai\carnagorpet.xml.cooked", true);
        }

        private void _FilterResetButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("todo");
            //filter_textBox.Text = "*.*";
            //_DoApplyFilter();


            //// ignore me - alex's testing stuffs ////

            //byte[] asdf = GetFileBytes(@"data\excel\strings\english\strings_affix.xls.uni.cooked", true);


            //Index idx = _indexFiles.FirstOrDefault(i => i.FileNameWithoutExtension == "hellgate000");
            //Index idx = _indexFiles.FirstOrDefault(i => i.FileNameWithoutExtension == ReanimatorIndex);
            //Debug.Assert(idx != null);
            //byte[] data = idx.GenerateIndexFile();
            //File.WriteAllBytes(idx.FilePath + ".decrypt", data);
            //Crypt.Encrypt(data);
            //File.WriteAllBytes(idx.FilePath, data);




            //idx = _indexFiles.FirstOrDefault(i => i.FileNameWithoutExtension == "sp_hellgate_1.10.180.3416_1.18074.70.4256");
            //Debug.Assert(idx != null);
            //data = idx.GenerateIndexFile();
            //File.WriteAllBytes(idx.FilePath + ".decrypt", data);
            //Crypt.Encrypt(data);
            //File.WriteAllBytes(idx.FilePath, data);
        }

        //TreeView filteredTreeView = new TreeView();
        //List<TreeNode> filterNodes = new List<TreeNode>();
        private void _DoApplyFilter()
        {
            // String filterText = ".*heal.*";
            // if (String.IsNullOrEmpty(filterText)) return;

            // foreach (TreeNode treeNode in files_treeView.Nodes)
            // {
            //     _ApplyFilter(treeNode, filterText);
            // }

            // if (filterNodes.Count <= 0) return;

            // ////files_treeView.BeginUpdate();
            // //foreach (TreeNode removeNode in filterNodes)
            // //{
            // //    removeNode.Remove();
            // //}
            //// files_treeView.EndUpdate();
        }

        private void _ApplyFilter(TreeNode treeNode, String filterText)
        {
            //NodeObject nodeObject = (NodeObject) treeNode.Tag;
            //if (nodeObject.IsFolder)
            //{
            //    foreach (TreeNode childNode in treeNode.Nodes)
            //    {
            //        _ApplyFilter(childNode, filterText);
            //    }

            //    return;
            //}

            //if (!Regex.IsMatch(treeNode.Text, filterText)) return;

            ////filterNodes.Add(treeNode);
        }

        /// <summary>
        /// Determines if the file HGL tries to load exists.<br />
        /// That is, checks .dat, if patched out, checks HGL data dir's.
        /// </summary>
        /// <param name="filePath">The path to the file - relative to HGL e.g. "data\colorsets.xml.cooked"</param>
        /// <returns>true for file exists, false otherwise.</returns>
        //public bool GetFileExists(String filePath)
        //{
        //    if (String.IsNullOrEmpty(filePath)) return false;

        //    if (filePath[0] == '\\')
        //    {
        //        filePath = filePath.Replace(@"\data", "data");
        //    }

        //    TreeNode treeNode = (TreeNode)_fileTable[filePath];
        //    if (treeNode == null) return false;

        //    // is not backup (in idx/dat)
        //    NodeObject nodeObject = (NodeObject)treeNode.Tag;
        //    if (!nodeObject.IsBackup) return true;

        //    // get full file path
        //    filePath = Path.Combine(Config.HglDir, treeNode.FullPath);
        //    return File.Exists(filePath);
        //}

        /// <summary>
        /// Reads in a file's bytes from where HGL would read it.<br />
        /// That is, from the .dat or data directorys if the file has been patched out.
        /// </summary>
        /// <param name="filePath">The path to the file - relative to HGL e.g. "data\colorsets.xml.cooked"</param>
        /// <returns>File byte data, or null if not found.</returns>
        //public byte[] GetFileBytes(String filePath)
        //{
        //    if (String.IsNullOrEmpty(filePath)) return null;

        //    if (filePath[0] == '\\')
        //    {
        //        filePath = filePath.Replace(@"\data", "data");
        //    }

        //    TreeNode treeNode = (TreeNode)_fileTable[filePath];
        //    if (treeNode == null) return null;

        //    NodeObject nodeObject = (NodeObject)treeNode.Tag;

        //    // are we loading from file or dat
        //    byte[] fileBytes;
        //    if (nodeObject.IsBackup)
        //    {
        //        filePath = Path.Combine(Config.HglDir, treeNode.FullPath);
        //        fileBytes = File.ReadAllBytes(filePath);
        //    }
        //    else
        //    {
        //        IndexFile idx = nodeObject.Index;
        //        Debug.Assert(idx != null);

        //        idx.BeginDatReading();
        //        // todo: rewrite fileBytes = idx.ReadDatFile(nodeObject.FileEntry);
        //        idx.EndDatAccess();
        //    }

        //    return null;// todo: rewrite  fileBytes;
        //}

        private void _UncookButton_Click(object sender, EventArgs e)
        {
            // make sure we have at least 1 checked file
            List<TreeNode> checkedNodes = new List<TreeNode>();
            if (_GetCheckedNodes(_files_fileTreeView.Nodes, checkedNodes) == 0)
            {
                MessageBox.Show("No files checked for extraction!", "Need Tables", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            // we're uncooking, so we want only uncook-able files
            List<TreeNode> uncookingNodes = (from treeNode in checkedNodes
                                             let nodeObject = (NodeObject)treeNode.Tag
                                             where nodeObject.CanCookWith && !nodeObject.IsUncookedVersion
                                             select treeNode).ToList();
            if (uncookingNodes.Count == 0)
            {
                MessageBox.Show("Unable to find any checked files that can be uncooked!", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
                return;
            }

            ProgressForm progressForm = new ProgressForm(_DoUnooking, uncookingNodes);
            progressForm.SetLoadingText(String.Format("Uncooking file(s)... ({0})", uncookingNodes.Count));
            progressForm.Show(this);
        }

        private void _DoUnooking(ProgressForm progressForm, Object param)
        {
            List<TreeNode> uncookingNodes = (List<TreeNode>)param;
            const int progressUpdateFreq = 20;
            if (progressForm != null)
            {
                progressForm.ConfigBar(1, uncookingNodes.Count, progressUpdateFreq);
            }

            int i = 0;
            foreach (String nodeFullPath in uncookingNodes.Select(treeNode => treeNode.FullPath))
            {
                if (i % progressUpdateFreq == 0 && progressForm != null)
                {
                    progressForm.SetCurrentItemText(nodeFullPath);
                }
                i++;

                if (!nodeFullPath.EndsWith(XmlCookedFile.FileExtention)) continue;

                byte[] fileBytes = null; // todo: rewrite GetFileBytes(nodeFullPath);
                Debug.Assert(fileBytes != null);

                //if (nodeFullPath.Contains("actor_ghost.xml.cooked"))
                //{
                //    int bp = 0;
                //}

                XmlCookedFile xmlCookedFile = new XmlCookedFile();

                DialogResult dr = DialogResult.Retry;
                bool uncooked = false;
                while (dr == DialogResult.Retry && !uncooked)
                {
                    try
                    {
                        xmlCookedFile.Uncook(fileBytes);
                        uncooked = true;
                    }
                    catch (Exception e)
                    {
                        ExceptionLogger.LogException(e, "_DoUnooking", true);

                        String errorMsg = String.Format("Failed to uncooked file!\n{0}\n\n{1}", nodeFullPath, e);
                        dr = MessageBox.Show(errorMsg, "Error",
                                             MessageBoxButtons.AbortRetryIgnore,
                                             MessageBoxIcon.Exclamation);
                        if (dr == DialogResult.Abort) return;
                        if (dr == DialogResult.Ignore) break;
                    }
                }

                if (!uncooked) continue;

                // todo: add newly uncooked file to file tree
                // note: assuming all cooked files end in .cooked - is this true anyways?
                String savePath = Path.Combine(Config.HglDir, nodeFullPath.Replace(".cooked", ""));
                xmlCookedFile.SaveXml(savePath);
            }
        }

        private void _CookButton_Click(object sender, EventArgs e)
        {
            // make sure we have at least 1 checked file
            List<TreeNode> checkedNodes = new List<TreeNode>();
            if (_GetCheckedNodes(_files_fileTreeView.Nodes, checkedNodes) == 0)
            {
                MessageBox.Show("No files checked for extraction!", "Need Tables", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            // we're uncooking, so we want only cook-able files
            List<TreeNode> cookNodes = (from treeNode in checkedNodes
                                        let nodeObject = (NodeObject)treeNode.Tag
                                        where nodeObject.CanCookWith && nodeObject.IsUncookedVersion
                                        select treeNode).ToList();
            if (cookNodes.Count == 0)
            {
                MessageBox.Show("Unable to find any checked files that can be cooked!", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
                return;
            }

            ProgressForm progressForm = new ProgressForm(_DoCooking, cookNodes);
            progressForm.SetLoadingText(String.Format("Uncooking file(s)... ({0})", cookNodes.Count));
            progressForm.Show(this);

        }

        private static void _DoCooking(ProgressForm progressForm, Object param)
        {
            List<TreeNode> cookNodes = (List<TreeNode>)param;
            const int progressUpdateFreq = 20;
            if (progressForm != null)
            {
                progressForm.ConfigBar(1, cookNodes.Count, progressUpdateFreq);
            }

            int i = 0;
            //foreach (String nodeFullPath in cookNodes.Select(treeNode => treeNode.FullPath))
            foreach (TreeNode treeNode in cookNodes)
            {
                TreeNode cookedNode = treeNode.Parent;
                String nodeFullPath = cookedNode.FullPath.Replace(".cooked", "");
                String filePath = Path.Combine(Config.HglDir, nodeFullPath);
                Debug.Assert(filePath.EndsWith(".xml"));

                if (i % progressUpdateFreq == 0 && progressForm != null)
                {
                    progressForm.SetCurrentItemText(filePath);
                }
                i++;

                //if (nodeFullPath.Contains("actor_ghost.xml"))
                //{
                //    int bp = 0;
                //}

                if (!File.Exists(filePath)) continue;
                XmlDocument xmlDocument = new XmlDocument();
                XmlCookedFile xmlCookedFile = new XmlCookedFile();

                DialogResult dr = DialogResult.Retry;
                byte[] cookedBytes = null;
                while (dr == DialogResult.Retry && cookedBytes == null)
                {
                    try
                    {
                        xmlDocument.Load(filePath);
                        cookedBytes = XmlCookedFile.CookXmlDocument(xmlDocument);
                    }
                    catch (Exception e)
                    {
                        ExceptionLogger.LogException(e, "_DoCooking", true);

                        String errorMsg = String.Format("Failed to cook file!\n{0}\n\n{1}", nodeFullPath, e);
                        dr = MessageBox.Show(errorMsg, "Error",
                                             MessageBoxButtons.AbortRetryIgnore,
                                             MessageBoxIcon.Exclamation);
                        if (dr == DialogResult.Abort) return;
                        if (dr == DialogResult.Ignore) break;
                    }
                }

                if (cookedBytes == null) continue;

                // todo: update newly cooked file to file tree
                String savePath = Path.Combine(Config.HglDir, filePath + ".cooked");
                File.WriteAllBytes(savePath, cookedBytes);

                // debug section
                //String savePath2 = Path.Combine(Config.HglDir, filePath + ".cooked2");
                //File.WriteAllBytes(savePath2, cookedBytes);
                //byte[] origBytes = File.ReadAllBytes(savePath);

                //if (cookedBytes.Length != origBytes.Length)
                //{
                //    int bp = 0;
                //}

                //if (!origBytes.SequenceEqual(cookedBytes))
                //{
                //    int bp = 0;
                //}
            }
        }
    }
}