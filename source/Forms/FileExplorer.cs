using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Reanimator.Properties;

namespace Reanimator.Forms
{
    public partial class FileExplorer : Form
    {
        private const String ReanimatorIndex = "sp_hellgate_1337";

        private static readonly Icon[] Icons = { Resources.GenericDocument, Resources.Folder, Resources.FolderOpen, Resources.XMLFile, Resources.AudioFile, Resources.MediaFile };
        private enum IconIndex
        {
            GenericDocument,
            Folder,
            FolderOpen,
            XmlFile,
            AudioFile,
            MediaFile
        }

        private static readonly Color BackupColor = Color.IndianRed;
        private static readonly Color NoEditColor = Color.DimGray;
        private static readonly Color BaseColor = Color.Black;

        private static readonly String[] IndexQueryStrings = { "hellgate*.dat", "sp_hellgate*.dat" };
        private readonly List<Index> _indexFiles;
        private readonly Hashtable _fileTable;

        private class NodeObject
        {
            public Index Index;
            public Index.FileEntry FileEntry;
            public bool IsFolder;
            public bool IsBackup;
            public bool CanEdit;
            public List<NodeObject> Siblings;

            public void AddSibling(NodeObject siblingNodeObject)
            {
                if (Siblings == null)
                {
                    Siblings = new List<NodeObject>();
                }

                Siblings.Add(siblingNodeObject);
            }

            public NodeObject GetYoungestChild()
            {
                if (Siblings == null || Siblings.Count == 0 || FileEntry == null)
                {
                    return this;
                }

                NodeObject returnObject = this;
                foreach (NodeObject siblingObject in Siblings)
                {
                    // if sibiling.FileTime > this.FileTime - i.e. sibling has bigger date; is newer
                    if (siblingObject.FileEntry != null && siblingObject.FileEntry.FileStruct.FileTime > FileEntry.FileStruct.FileTime)
                    {
                        returnObject = siblingObject;
                    }
                }

                return returnObject;
            }
        }

        private class NodeSorter : IComparer
        {
            public int Compare(Object objX, Object objY)
            {
                TreeNode treeNodeX = (TreeNode)objX;
                TreeNode treeNodeY = (TreeNode)objY;

                NodeObject nodeObjectX = (NodeObject)treeNodeX.Tag;
                NodeObject nodeObjectY = (NodeObject)treeNodeY.Tag;

                if (nodeObjectX != null && nodeObjectY != null)
                {
                    if (nodeObjectX.IsFolder && !nodeObjectY.IsFolder) return -1;
                    if (!nodeObjectX.IsFolder && nodeObjectY.IsFolder) return 1;
                }

                return treeNodeX.Text.CompareTo(treeNodeY.Text);
            }
        }

        public FileExplorer(ref List<Index> indexFiles)
        {
            InitializeComponent();
            files_treeView.DoubleBuffered(true);

            _indexFiles = indexFiles;
            _fileTable = new Hashtable();
            backupKey_label.ForeColor = BackupColor;
            noEditorKey_label.ForeColor = NoEditColor;

            // load icons
            ImageList imageList = new ImageList { ColorDepth = ColorDepth.Depth32Bit };
            foreach (Icon icon in Icons)
            {
                imageList.Images.Add(icon);
            }
            files_treeView.ImageList = imageList;
        }

        public void LoadIndexFiles(ProgressForm progressForm, Object param)
        {
            if (progressForm != null)
            {
                progressForm.SetLoadingText("Loading game file system...");
            }

            String idxFilesRoot = Path.Combine(Config.HglDir, "data");
            foreach (String queryString in IndexQueryStrings)
            {
                String[] datFiles = Directory.GetFiles(idxFilesRoot, queryString);
                if (datFiles.Length == 0) continue;
                if (progressForm != null)
                {
                    progressForm.ConfigBar(1, datFiles.Length, 1);
                }

                foreach (String idxFile in datFiles.Select(datFile => datFile.Replace(".dat", ".idx")))
                {
                    // update progress
                    if (progressForm != null)
                    {
                        progressForm.SetCurrentItemText("Loading " + Path.GetFileName(idxFile) + "...");
                    }

                    byte[] indexData = File.ReadAllBytes(idxFile);

                    Index index = new Index();
                    if (!index.ParseData(indexData, idxFile))
                    {
                        MessageBox.Show("Failed to parse index file!", "Error", MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        continue;
                    }
                    _indexFiles.Add(index);
                }
            }

            files_treeView.BeginUpdate();
            foreach (Index index in _indexFiles)
            {
                _ParseIndexFile(index);
            }
            files_treeView.TreeViewNodeSorter = new NodeSorter();
        }

        private void _ParseIndexFile(Index index)
        {
            // loop files
            foreach (Index.FileEntry currFile in index.Files)
            {
                NodeObject currNodeObject = new NodeObject { Index = index, FileEntry = currFile };
                String[] nodeKeys = currFile.DirectoryString.Split('\\');
                TreeNode currTreeNode = null;


                //if (currFile.FileNameString == "affixes.txt.cooked")
                //{
                //    int bp = 0;
                //}


                // set up folders and get applicable root folder
                foreach (string nodeKey in nodeKeys.Where(nodeKey => !String.IsNullOrEmpty(nodeKey)))
                {
                    if (nodeKey == Index.BackupPrefix)
                    {
                        currNodeObject.IsBackup = true;
                        continue;
                    }

                    if (currTreeNode == null)
                    {
                        currTreeNode = files_treeView.Nodes[nodeKey] ?? files_treeView.Nodes.Add(nodeKey, nodeKey);
                    }
                    else
                    {
                        currTreeNode = currTreeNode.Nodes[nodeKey] ?? currTreeNode.Nodes.Add(nodeKey, nodeKey);
                    }
                }
                Debug.Assert(currTreeNode != null);


                // need to have canEdit check before we update the node or it'll be false for newer versions
                // todo: fix me/merge me some how with stuff below - sucks doing multiple checks like this
                if (currFile.FileNameString.EndsWith(XmlCookedFile.FileExtention))
                {
                    // we can only edit skills xml.cooked at the moment
                    if (nodeKeys.Contains("skills") || nodeKeys.Contains("ai") || nodeKeys.Contains("states") || nodeKeys.Contains("effects") ||
                        (nodeKeys.Contains("background") && (currFile.FileNameString.Contains("layout") || currFile.FileNameString.Contains("path"))) ||
                        nodeKeys.Contains("materials"))
                    {
                        currNodeObject.CanEdit = true;
                    }
                }
                else if (currFile.FileNameString.EndsWith(ExcelFile.FileExtention) ||
                    currFile.FileNameString.EndsWith(StringsFile.FileExtention) ||
                    currFile.FileNameString.EndsWith("txt"))
                {
                    currNodeObject.CanEdit = true;
                }


                // have we already added the file? if so, check file time etc
                String key = currFile.DirectoryString.Replace(Index.BackupPrefix + @"\", "") + currFile.FileNameString;
                if (_fileTable.Contains(key))
                {
                    // get the already added/original node
                    TreeNode origTreeNode = (TreeNode)_fileTable[key];
                    NodeObject origNodeObject = (NodeObject)origTreeNode.Tag;

                    // do backup checks first as they'll "override" the FileTime values (i.e. file not found causes game to go to older version)
                    // if currFile IS a backup, and orig is NOT, then add to Siblings as game will be loading orig over "backup" anyways
                    if (currNodeObject.IsBackup && !origNodeObject.IsBackup)
                    {
                        origNodeObject.AddSibling(currNodeObject);
                        continue;
                    }

                    // if curr is NOT a backup, but orig IS, then we want to update (i.e. don't care about FileTime; as above)
                    // OR if orig is older than curr, we also want to update/re-arrange NodeObjects, etc
                    if (!currNodeObject.IsBackup && origNodeObject.IsBackup ||
                        origNodeObject.FileEntry.FileStruct.FileTime < currFile.FileStruct.FileTime)
                    {
                        // set the Siblings list to the updated NodeObject and null out other
                        if (origNodeObject.Siblings != null)
                        {
                            currNodeObject.Siblings = origNodeObject.Siblings;
                            origNodeObject.Siblings = null;
                        }
                        // add the "orig" (now old) to the curr NodeObject.Siblings list
                        currNodeObject.AddSibling(origNodeObject);
                        // update TreeNode.Tag to point to new (curr) NodeObject
                        origTreeNode.Tag = currNodeObject;

                        continue;
                    }

                    // if curr is older (or equal to; hellgate000 has duplicates) than the orig, then add this to the Siblings list (i.e. orig is newer)
                    if (origNodeObject.FileEntry.FileStruct.FileTime >= currFile.FileStruct.FileTime)
                    {
                        origNodeObject.AddSibling(currNodeObject);
                        continue;
                    }

                    Debug.Assert(false, "End of 'if (_fileTable.Contains(key))'", "wtf??\n\nThis shouldn't happen, please report this.");
                }


                // add file/node
                TreeNode node = currTreeNode.Nodes.Add(key, currFile.FileNameString);


                // do some pretty icon stuffs and other file-type checks
                if (currFile.FileNameString.EndsWith(XmlCookedFile.FileExtention))
                {
                    node.ImageIndex = (int)IconIndex.XmlFile;

                    // we can only edit skills xml.cooked at the moment
                    if (nodeKeys.Contains("skills"))
                    {
                        currNodeObject.CanEdit = true;
                    }
                }
                else if (currFile.FileNameString.EndsWith(ExcelFile.FileExtention) ||
                    currFile.FileNameString.EndsWith(StringsFile.FileExtention) ||
                    currFile.FileNameString.EndsWith("txt"))
                {
                    currNodeObject.CanEdit = true;
                }
                else if (currFile.FileNameString.EndsWith("mp2") || currFile.FileNameString.EndsWith("ogg"))
                {
                    node.ImageIndex = (int)IconIndex.AudioFile;
                }
                else if (currFile.FileNameString.EndsWith("bik"))
                {
                    node.ImageIndex = (int)IconIndex.MediaFile;
                }
                node.SelectedImageIndex = node.ImageIndex;


                // final nodeObject setups
                if (currNodeObject.IsBackup)
                {
                    node.ForeColor = BackupColor;
                }
                else if (!currNodeObject.CanEdit)
                {
                    node.ForeColor = NoEditColor;
                }

                node.Tag = currNodeObject;
                _fileTable.Add(key, node);
            }


            // aesthetics etc
            foreach (TreeNode treeNode in files_treeView.Nodes)
            {
                if (treeNode.Index == 0)
                {
                    files_treeView.SelectedNode = treeNode;
                }

                treeNode.Expand();
                _FlagFolderNodes(treeNode);
            }
        }

        private static void _FlagFolderNodes(TreeNode treeNode)
        {
            if (treeNode.Nodes.Count <= 0) return;

            if (treeNode.Tag == null)
            {
                treeNode.Tag = new NodeObject { IsFolder = true };
                treeNode.ImageIndex = (int)IconIndex.Folder;
                treeNode.SelectedImageIndex = treeNode.ImageIndex;
            }

            foreach (TreeNode childNode in treeNode.Nodes)
            {
                _FlagFolderNodes(childNode);
            }
        }

        private void _FilesTreeViewAfterSelect(Object sender, TreeViewEventArgs e)
        {
            TreeView treeView = (TreeView)sender;
            TreeNode selectedNode = treeView.SelectedNode;
            NodeObject nodeObject = (NodeObject)selectedNode.Tag;

            fileName_textBox.DataBindings.Clear();
            fileSize_textBox.DataBindings.Clear();
            fileCompressed_textBox.DataBindings.Clear();
            loadingLocation_textBox.DataBindings.Clear();

            if (nodeObject.IsFolder) // if is a folder
            {
                fileName_textBox.Text = selectedNode.Text;
                fileSize_textBox.Text = String.Empty;
                fileCompressed_textBox.Text = String.Empty;
                loadingLocation_textBox.Text = String.Empty;
                fileTime_textBox.Text = String.Empty;

                return;
            }

            Index.FileEntry fileIndex = nodeObject.FileEntry;
            Debug.Assert(fileIndex != null);

            fileName_textBox.DataBindings.Add("Text", fileIndex, "FileNameString");
            fileSize_textBox.DataBindings.Add("Text", fileIndex, "UncompressedSize");
            fileCompressed_textBox.DataBindings.Add("Text", fileIndex, "CompressedSize");
            fileTime_textBox.Text = (DateTime.FromFileTime(fileIndex.FileStruct.FileTime)).ToString();

            if (fileIndex.Modified)
            {
                String fileDataPath = Path.Combine(fileIndex.DirectoryString.Replace(Index.BackupPrefix, ""), fileIndex.FileNameString);
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

        private void _FilesTreeViewDoubleClick(Object sender, EventArgs e)
        {
            TreeView treeView = (TreeView)sender;
            TreeNode selectedNode = treeView.SelectedNode;
            NodeObject nodeObject = (NodeObject)selectedNode.Tag;
            Debug.Assert(nodeObject != null);

            if (nodeObject.IsFolder || !nodeObject.CanEdit) return;

            Index.FileEntry fileIndex = nodeObject.FileEntry;

            // todo: this section needs a good cleaning
            // todo: implementation of choosing default program in the options menu
            // todo: current implementation overwites already extracted/uncooked file without asking - open it instead or ask?
            if (fileIndex.FileNameString.EndsWith(ExcelFile.FileExtention) || fileIndex.FileNameString.EndsWith(StringsFile.FileExtention))
            {
                MessageBox.Show("todo");
            }
            else if (fileIndex.FileNameString.EndsWith(XmlCookedFile.FileExtention))
            {
                String xmlDataPath = Path.Combine(Config.HglDir, fileIndex.FullPath.Replace(".cooked", ""));

                byte[] fileData = GetFileBytes(fileIndex.FullPath);
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

                DialogResult drOpen = MessageBox.Show("Uncooked XML file saved at " + xmlDataPath + "\n\nOpen with notepad (notepad++ if available)?", "Success",
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (drOpen == DialogResult.Yes)
                {
                    const String fileName = "notepad++.exe";

                    // this is a bit dodgy using exceptions as if-else, but meh
                    // todo: add check for file name existence etc
                    try
                    {
                        Process notePad = new Process { StartInfo = { FileName = fileName, Arguments = xmlDataPath } };
                        notePad.Start();
                    }
                    catch(Exception)
                    {
                        Process notePad = new Process { StartInfo = { FileName = "notepad.exe", Arguments = xmlDataPath } };
                        notePad.Start();
                    }
                }
            }
            else if (fileIndex.FileNameString.EndsWith(".txt"))
            {
                // todo: fix me (copy-pasted from above)
                String xmlDataPath = Path.Combine(Config.HglDir, fileIndex.FullPath);
                const String fileName = "notepad++.exe";

                // this is a bit dodgy using exceptions as if-else, but meh
                // todo: add check for file name existence etc
                try
                {
                    Process notePad = new Process { StartInfo = { FileName = fileName, Arguments = xmlDataPath } };
                    notePad.Start();
                }
                catch (Exception)
                {
                    Process notePad = new Process { StartInfo = { FileName = "notepad.exe", Arguments = xmlDataPath } };
                    notePad.Start();
                }
            }
            else
            {
                MessageBox.Show("Unexpected editable file!\n(this shouldn't happen - please report this)", "Warning",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private class ExtractPackPatchArgs
        {
            public bool KeepStructure;
            public bool PatchFiles;
            public String RootDir;
            public List<TreeNode> CheckedNodes;
            public Index PackIndex;
        }

        private void _ExtractButtonClick(object sender, EventArgs e)
        {
            // make sure we have at least 1 checked file
            List<TreeNode> checkedNodes = new List<TreeNode>();
            if (_GetCheckedNodes(files_treeView.Nodes, checkedNodes) == 0)
            {
                MessageBox.Show("No files checked for extraction!", "Need Files", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            // where do we want to save it
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog {SelectedPath = Config.HglDir};
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
            progressForm.SetLoadingText(String.Format("Extracting files... ({0})", checkedNodes.Count));
            progressForm.Show();
        }

        private void _ExtractPatchButtonClick(object sender, EventArgs e)
        {
            // make sure we have at least 1 checked file
            List<TreeNode> checkedNodes = new List<TreeNode>();
            if (_GetCheckedNodes(files_treeView.Nodes, checkedNodes) == 0)
            {
                MessageBox.Show("No files checked for extraction!", "Need Files", MessageBoxButtons.OK,
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
            progressForm.SetLoadingText(String.Format("Extracting and Patching files... ({0})", checkedNodes.Count));
            progressForm.Show();
        }

        private static int _GetCheckedNodes(TreeNodeCollection nodes, ICollection<TreeNode> checkedNodes)
        {
            Debug.Assert(checkedNodes != null);

            foreach (TreeNode childNode in nodes)
            {
                // if folder, check children
                if (childNode.Nodes.Count > 0)
                {
                    _GetCheckedNodes(childNode.Nodes, checkedNodes);
                    continue;
                }

                if (!childNode.Checked) continue;

                checkedNodes.Add(childNode);
            }

            return checkedNodes.Count;
        }

        private void _DoExtractPatch(ProgressForm progressForm, Object param)
        {
            ExtractPackPatchArgs extractPatchArgs = (ExtractPackPatchArgs) param;
            DialogResult overwrite = DialogResult.None;
            Hashtable indexToWrite = new Hashtable();

            const int progressStepRate = 50;
            progressForm.ConfigBar(1, extractPatchArgs.CheckedNodes.Count, progressStepRate);
            progressForm.SetCurrentItemText("Extracting file(s)...");

            // might as well just loop all nodes - slower finding and cloning them; using the .Checked setter is the killer
            if (!_BeginDatAccess(_indexFiles, false))
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
                    progressForm.SetCurrentItemText(String.Format("Extracting file... {0}", extractNode.FullPath));
                }
                i++;

                if (_ExtractPatchFile(extractNode, ref overwrite, indexToWrite, extractPatchArgs)) continue;

                if (overwrite != DialogResult.Cancel)
                {
                    MessageBox.Show("Unexpected error, extraction process terminated!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                return;
            }
            _EndDatAccess(_indexFiles);

            // are we patching?
            if (!extractPatchArgs.PatchFiles)
            {
                MessageBox.Show("File(s) extracted sucessfully!", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (indexToWrite.Count == 0)
            {
                MessageBox.Show("File(s) extracted sucessfully!\nNo index files require modifications.", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // do we really need to ask if we've hit "Patch and Extract"?
            //DialogResult writeIdx = MessageBox.Show("Files extracted sucessfully!\nSave modified index file(s)?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (writeIdx == DialogResult.No) return;

            progressForm.SetCurrentItemText("Saving modified index file(s)...");
            foreach (Index idx in
                from DictionaryEntry indexDictionary in indexToWrite select (Index)indexDictionary.Value)
            {
                byte[] idxData = idx.GenerateIndexFile();
                Crypt.Encrypt(idxData);
                File.WriteAllBytes(idx.FilePath, idxData);
            }
            MessageBox.Show("Modified index file(s) saved sucessfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private static bool _BeginDatAccess(IEnumerable<Index> indexList, bool write)
        {
            Debug.Assert(indexList != null);
            return indexList.Select(index => write ? index.BeginDatWriting() : index.BeginDatReading()).All(result => result);
        }

        private static void _EndDatAccess(IEnumerable<Index> indexList)
        {
            Debug.Assert(indexList != null);

            foreach (Index index in indexList)
            {
                index.EndDatAccess();
            }
        }

        private static bool _ExtractPatchFile(TreeNode treeNode, ref DialogResult overwrite, Hashtable indexToWrite, ExtractPackPatchArgs extractPatchArgs)
        {
            if (treeNode == null) return false;

            //if (treeNode.Text == "affixes.txt.cooked")
            //{
            //    int bp = 0;
            //}

            // loop through for folders, etc
            NodeObject nodeObject = (NodeObject) treeNode.Tag;
            if (nodeObject.IsFolder)
            {
                foreach (TreeNode childNode in treeNode.Nodes)
                {
                    if (!_ExtractPatchFile(childNode, ref overwrite, indexToWrite, extractPatchArgs)) return false;
                }

                return true;
            }


            // make sure we want to extract this file
            if (!treeNode.Checked) return true;


            // get path
            Index fileIndex = nodeObject.Index;
            Index.FileEntry file = nodeObject.FileEntry;
            String filePath = extractPatchArgs.KeepStructure
                                  ? Path.Combine(extractPatchArgs.RootDir, treeNode.FullPath)
                                  : Path.Combine(extractPatchArgs.RootDir, file.FileNameString);


            // does it exist?
            bool fileExists = File.Exists(filePath);
            if (fileExists && overwrite == DialogResult.None)
            {
                overwrite = MessageBox.Show("An extract file already exists, do you wish to overwrite the file, and all following?\n\nFile: " + filePath,
                    "Question", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (overwrite == DialogResult.Cancel) return false;
            }
            if (fileExists && overwrite == DialogResult.No) return true;


            // save file
            DialogResult extractDialogResult = DialogResult.Retry;
            while (extractDialogResult == DialogResult.Retry)
            {
                byte[] fileBytes = nodeObject.Index.ReadDatFile(file);
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
                    Index.FileEntry siblingFileEntry = siblingNodeObject.FileEntry;
                    Index siblingIndex = siblingFileEntry.InIndex;

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
            if (!fileIndex.PatchOutFile(file)) return true;


            // add index to indexToWrite list
            fileIndexKey = fileIndex.FileNameWithoutExtension;
            if (!indexToWrite.ContainsKey(fileIndexKey))
            {
                indexToWrite.Add(fileIndexKey, file.InIndex);
            }
            return true;
        }

        private void _PackPatchButtonClick(object sender, EventArgs e)
        {
            // make sure we have at least 1 checked file
            List<TreeNode> checkedNodes = new List<TreeNode>();
            if (_GetCheckedNodes(files_treeView.Nodes, checkedNodes) == 0)
            {
                MessageBox.Show("No files checked for packing!", "Need Files", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            // get our custom index - or create if doesn't exist
            Index packIndex = _indexFiles.FirstOrDefault(index => index.FileNameWithoutExtension == ReanimatorIndex);
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

                packIndex = new Index(indexPath);
                _indexFiles.Add(packIndex);
            }

            ExtractPackPatchArgs extractPackPatchArgs = new ExtractPackPatchArgs
            {
                PackIndex = packIndex,
                RootDir = Config.HglDir,
                CheckedNodes = checkedNodes,
                PatchFiles = false
            };

            files_treeView.BeginUpdate();
            ProgressForm progressForm = new ProgressForm(_DoPackPatch, extractPackPatchArgs);
            progressForm.Disposed += delegate { files_treeView.EndUpdate(); };
            progressForm.SetLoadingText("Packing and Patching files...");
            progressForm.Show();
        }
        
        private void _DoPackPatch(ProgressForm progressForm, Object param)
        {
            ExtractPackPatchArgs args = (ExtractPackPatchArgs) param;

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
                    packResults.WriteLine("{0} - File Not Found", filePath);
                    continue;
                }

                // ensure it was once packed (need FilePathHash etc)
                NodeObject nodeObject = (NodeObject) checkedNode.Tag;
                if (nodeObject.FileEntry == null ||
                    nodeObject.FileEntry.FileStruct == null ||
                    nodeObject.FileEntry.FileStruct.FileNameHash == 0)
                {
                    packResults.WriteLine("{0} - File Has No Base Version", filePath);
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
            if (!_BeginDatAccess(_indexFiles, true))
            {
                MessageBox.Show(
                    "Failed to open dat files for writing!\nEnsure no other programs are using them and try again.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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
                Index.FileEntry fileEntry = args.PackIndex.GetFileFromIndex(packNode.FullPath);
                if (fileEntry == null)
                {
                    fileEntry = args.PackIndex.AddFileToIndex(oldNodeObject.FileEntry);
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
                    args.PackIndex.AddFileToDat(fileData, fileEntry);
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
                    CanEdit =  oldNodeObject.CanEdit,
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
                args.PackIndex.RebuildDatFile();
            }
            _EndDatAccess(_indexFiles);
            
            
            // write updated index
            progressForm.SetCurrentItemText("Writing update dat index...");
            try
            {
                byte[] idxBytes = args.PackIndex.GenerateIndexFile();
                Crypt.Encrypt(idxBytes);
                File.WriteAllBytes(args.PackIndex.FilePath, idxBytes);
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to write updated index file!\n\n" + e, "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("File packing and idx/dat writing completed!", "Success", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }

        private void _RevertRestoreButtonClick(object sender, EventArgs e)
        {
            MessageBox.Show("Todo");
        }

        private void _FilesTreeViewAfterExpand(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = (int)IconIndex.FolderOpen;
            e.Node.SelectedImageIndex = e.Node.ImageIndex;
        }

        private void _FilesTreeViewAfterCollapse(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = (int)IconIndex.Folder;
            e.Node.SelectedImageIndex = e.Node.ImageIndex;
        }

        private void FileExplorer_Shown(object sender, EventArgs e)
        {
            files_treeView.EndUpdate();
        }

        private void _FilesTreeViewAfterCheck(object sender, TreeViewEventArgs e)
        {
            files_treeView.AfterCheck -= _FilesTreeViewAfterCheck;
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
            files_treeView.AfterCheck += _FilesTreeViewAfterCheck;
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

        private void _FilterApplyButtonClick(object sender, EventArgs e)
        {
            MessageBox.Show("todo");
            //_DoApplyFilter();

            //byte[] asdf = GetFileBytes(@"data\ai\carnagorpet.xml.cooked", true);
        }

        private void _FilterResetButtonClick(object sender, EventArgs e)
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

        public bool GetFileExists(String filePath)
        {
            if (String.IsNullOrEmpty(filePath)) return false;

            if (filePath[0] == '\\')
            {
                filePath = filePath.Replace(@"\data", "data");
            }

            TreeNode treeNode = (TreeNode)_fileTable[filePath];
            if (treeNode == null) return false;

            // is not backup (in idx/dat)
            NodeObject nodeObject = (NodeObject) treeNode.Tag;
            if (!nodeObject.IsBackup) return true;

            // get full file path
            filePath = Path.Combine(Config.HglDir, treeNode.FullPath);
            return File.Exists(filePath);
        }

        public byte[] GetFileBytes(String filePath)
        {
            if (String.IsNullOrEmpty(filePath)) return null;

            if (filePath[0] == '\\')
            {
                filePath = filePath.Replace(@"\data", "data");
            }

            TreeNode treeNode = (TreeNode) _fileTable[filePath];
            if (treeNode == null) return null;

            NodeObject nodeObject = (NodeObject) treeNode.Tag;
            // not entirly sure why I even had this...
            //if (getLatestVersion)
            //{
            //    nodeObject = nodeObject.GetYoungestChild();
            //}

            // are we loading from file or dat
            byte[] fileBytes;
            if (nodeObject.IsBackup)
            {
                filePath = Path.Combine(Config.HglDir, treeNode.FullPath);
                fileBytes = File.ReadAllBytes(filePath);
            }
            else
            {
                Index idx = nodeObject.Index;
                Debug.Assert(idx != null);

                idx.BeginDatReading();
                fileBytes = idx.ReadDatFile(nodeObject.FileEntry);
                idx.EndDatAccess();
            }

            return fileBytes;
        }
    }
}