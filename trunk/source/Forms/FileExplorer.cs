using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Reanimator.Properties;

namespace Reanimator.Forms
{
    public partial class FileExplorer : Form
    {
        private const String ReanimatorIndex = "sp_hellgate_reanimator";

        private static readonly Icon[] Icons = { Resources.GenericDocument, Resources.Folder, Resources.FolderOpen, Resources.XMLFile, Resources.AudioFile, Resources.MediaFile };
        private enum IconIndex
        {
            GenericDocument,
            Folder,
            FolderOpen,
            XMLFile,
            AudioFile,
            MediaFile
        }

        private static readonly Color BackupColor = Color.IndianRed;
        private static readonly Color NoEditColor = Color.DimGray;

        private static readonly String[] IndexQueryStrings = { "hellgate*.dat", "sp_hellgate*.dat" };
        private readonly List<Index> _indexFiles;
        private readonly Hashtable _fileTable;

        private class NodeObject
        {
            public Index Index;
            public Index.FileEntry FileIndex;
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

        public FileExplorer(List<Index> indexFiles)
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
            foreach (Index.FileEntry currFile in index.FileTable)
            {
                NodeObject currNodeObject = new NodeObject { Index = index, FileIndex = currFile };
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
                        origNodeObject.FileIndex.FileStruct.FileTime < currFile.FileStruct.FileTime)
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
                    if (origNodeObject.FileIndex.FileStruct.FileTime >= currFile.FileStruct.FileTime)
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
                    node.ImageIndex = (int)IconIndex.XMLFile;

                    // we can only edit skills xml.cooked at the moment
                    if (nodeKeys.Contains(XmlCookedSkill.RootFolder))
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

            Index.FileEntry fileIndex = nodeObject.FileIndex;
            Debug.Assert(fileIndex != null);

            fileName_textBox.DataBindings.Add("Text", fileIndex, "FileNameString");
            fileSize_textBox.DataBindings.Add("Text", fileIndex, "UncompressedSize");
            fileCompressed_textBox.DataBindings.Add("Text", fileIndex, "CompressedSize");
            fileTime_textBox.Text = (DateTime.FromFileTime((long)fileIndex.FileStruct.FileTime)).ToString();

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

            Index.FileEntry fileIndex = nodeObject.FileIndex;

            if (fileIndex.FileNameString.EndsWith(ExcelFile.FileExtention) || fileIndex.FileNameString.EndsWith(StringsFile.FileExtention))
            {
                MessageBox.Show("todo");
            }
            else if (fileIndex.FileNameString.EndsWith(XmlCookedFile.FileExtention))
            {
                MessageBox.Show("todo");
            }
            else if (fileIndex.FileNameString.EndsWith(".txt"))
            {
                MessageBox.Show("todo");
            }
            else
            {
                MessageBox.Show("Unexpected editable file!\n(this shouldn't happen - please report this)", "Warning",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private class ExtractPatchArgs
        {
            public bool KeepPath;
            public bool PatchFiles;
            public String ExtractRoot;
        }

        private void _ExtractButtonClick(object sender, EventArgs e)
        {
            // where do we want to save it
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog {SelectedPath = Config.HglDir};
            if (folderBrowserDialog.ShowDialog(this) != DialogResult.OK) return;

            // do we want to keep the directory structure?
            bool keepPath = false;
            DialogResult dr = MessageBox.Show("Keep directory structure?", "Path", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.Cancel) return;

            if (dr == DialogResult.Yes)
            {
                keepPath = true;
            }

            ExtractPatchArgs extractPatchArgs = new ExtractPatchArgs
                                                    {
                                                        KeepPath = keepPath,
                                                        PatchFiles = false,
                                                        ExtractRoot = folderBrowserDialog.SelectedPath
                                                    };
            ProgressForm progressForm = new ProgressForm(_DoExtractPatch, extractPatchArgs);
            progressForm.SetLoadingText("Patching and Extracting files...");
            progressForm.SetStyle(ProgressBarStyle.Marquee);
            progressForm.Show();
        }

        private void _ExtractPatchButtonClick(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(
                "Extract & Patch out the selected file's?",
                "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.No) return;

            ExtractPatchArgs extractPatchArgs = new ExtractPatchArgs
            {
                KeepPath = true,
                PatchFiles = true,
                ExtractRoot = Config.HglDir
            };
            ProgressForm progressForm = new ProgressForm(_DoExtractPatch, extractPatchArgs);
            progressForm.SetLoadingText("Patching and Extracting files...");
            progressForm.SetStyle(ProgressBarStyle.Marquee);
            progressForm.Show();
        }

        private void _DoExtractPatch(ProgressForm progressForm, Object param)
        {
            // todo: add proper progress counter, etc?
            progressForm.SetCurrentItemText("Extracting file(s)...");
            ExtractPatchArgs extractPatchArgs = (ExtractPatchArgs) param;
            DialogResult overwrite = DialogResult.None;
            Hashtable indexToWrite = new Hashtable();

            // might as well just loop all nodes - slower finding and cloning them; using the .Checked setter is the killer
            if (files_treeView.Nodes.Cast<TreeNode>().Any(node => !_ExtractPatchFile(node, ref overwrite, indexToWrite, extractPatchArgs)))
            {
                if (overwrite != DialogResult.Cancel)
                {
                    MessageBox.Show("Unexpected error, extraction process terminated!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                return;
            }

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

            DialogResult writeIdx = MessageBox.Show("Files extracted sucessfully!\nSave modified index file(s)?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (writeIdx == DialogResult.No) return;

            progressForm.SetCurrentItemText("Saving modified index file(s)...");
            foreach (Index idx in
                from DictionaryEntry indexDictionary in indexToWrite select (Index)indexDictionary.Value)
            {
                File.WriteAllBytes(idx.FilePath, idx.GenerateIndexFile());
            }
            MessageBox.Show("Modified index file(s) saved sucessfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);            
        }

        private static bool _ExtractPatchFile(TreeNode treeNode, ref DialogResult overwrite, Hashtable indexToWrite, ExtractPatchArgs extractPatchArgs)
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
            Index.FileEntry file = nodeObject.FileIndex;
            String filePath = extractPatchArgs.KeepPath
                                  ? Path.Combine(extractPatchArgs.ExtractRoot, treeNode.FullPath)
                                  : Path.Combine(extractPatchArgs.ExtractRoot, file.FileNameString);


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
                byte[] fileBytes = nodeObject.Index.ReadDataFile(file);
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


            // is this file located else where? (i.e. does it have Siblings)
            String fileIndexKey;
            if (nodeObject.Siblings != null && nodeObject.Siblings.Count > 0)
            {
                // this file has siblings - loop through
                foreach (Index.FileEntry siblingFileIndex in
                    nodeObject.Siblings.Select(siblingNodeObject => siblingNodeObject.FileIndex).Where(fileIndex.PatchOutFile))
                {
                    fileIndexKey = siblingFileIndex.InIndex.FileNameWithoutExtension;
                    if (!indexToWrite.ContainsKey(fileIndexKey))
                    {
                        indexToWrite.Add(fileIndexKey, siblingFileIndex.InIndex);
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
            //// get our custom index - or create if doesn't exist
            //Index packIndex = _indexFiles.FirstOrDefault(index => index.FileNameWithoutExtension == "sp_hellgate_reanimator");
            //if (packIndex == null)
            //{
            //    String indexPath = String.Format(@"data\{0}.idx", ReanimatorIndex);
            //    indexPath = Path.Combine(Config.HglDir, indexPath);
            //    try
            //    {
            //        File.Create(indexPath);
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("Failed to create custom index file!\n\n" + ex, "Error", MessageBoxButtons.OK,
            //                        MessageBoxIcon.Error);
            //        return;
            //    }

            //    packIndex = new Index(indexPath);
            //}

            //ProgressForm progressForm = new ProgressForm(_DoPackPatch, packIndex);
            //progressForm.SetLoadingText("Packing and Patching files...");
            //progressForm.SetStyle(ProgressBarStyle.Marquee);
            //progressForm.Show();
        }
        
        private void _DoPackPatch(ProgressForm progressForm, Object param)
        {
            //Index packIndex = (Index)param;
            //Debug.Assert(packIndex != null);


            //// find/check which files exist
            //progressForm.SetCurrentItemText("Finding file(s)...");
            //List<TreeNode> packFiles = new List<TreeNode>();
            //if (files_treeView.Nodes.Cast<TreeNode>().Any(treeNode => !_PatchPackCheckFiles(treeNode, packFiles)))
            //{
            //    MessageBox.Show("Pack and Patching process aborted!", "Notice", MessageBoxButtons.OK,
            //                    MessageBoxIcon.Information);
            //    return;
            //}

            //if (packFiles.Count <= 0)
            //{
            //    MessageBox.Show("No files found for packing!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return;
            //}


            //// update pack index
            //progressForm.SetCurrentItemText("Updating index and packing file(s)...");
            //if (!packIndex.BeginDatWriting())
            //{
            //    MessageBox.Show("Failed to open .dat file for writing!", "Error", MessageBoxButtons.OK,
            //                    MessageBoxIcon.Error);
            //    return;
            //}

            //// todo: this needs to be rewritten looks/feels gross shuffling the fileEntry around like this
            //foreach (TreeNode packNode in packFiles)
            //{
            //    NodeObject packObject = (NodeObject) packNode.Tag;

            //    // does the file exist in the packIndex - if not, add
            //    Index.FileEntry fileEntry = packIndex.GetFileFromIndex(packNode.FullPath);
            //    if (fileEntry == null)
            //    {
            //        if (packObject.FileIndex == null)
            //        {
            //            // todo: add yes/no continue/cancel etc
            //            MessageBox.Show("Unable to add file to index; file has no base.\n" + packNode.FullPath, "Error",
            //                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //            continue;
            //        }

            //        fileEntry = packIndex.AddFileToIndex(packObject.FileIndex);
            //    }

            //    // update fileTime to now - ensures it will override older versions
            //    fileEntry.FileStruct.FileTime = DateTime.Now.ToFileTime();

            //    String filePath = Path.Combine(Config.HglDir, packNode.FullPath);
            //    byte[] fileData;
            //    try
            //    {
            //        fileData = File.ReadAllBytes(filePath);
            //    }
            //    catch (Exception ex)
            //    {
            //        // todo: add yes/no continue/cancel etc
            //        MessageBox.Show("Failed to read in file for packing!\n\n" + ex, "Error", MessageBoxButtons.OK,
            //                        MessageBoxIcon.Error);
            //        continue;
            //    }

            //    if (!packIndex.AddFileToDat(fileData, fileEntry, true))
            //    {
            //        MessageBox.Show("Failed pack file!\n" + packNode.FullPath, "Error", MessageBoxButtons.OK,
            //                        MessageBoxIcon.Exclamation);
            //    }
            //}

            //packIndex.EndDatAccess();
            
            
            //// write index
            //progressForm.SetCurrentItemText("Writing index...");
            //try
            //{
            //    File.WriteAllBytes(packIndex.FilePath, packIndex.GenerateIndexFile());
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Failed to write updated index file!\n\n" + ex, "Error", MessageBoxButtons.OK,
            //                    MessageBoxIcon.Error);
            //    return;
            //}

            //MessageBox.Show("File packing and index/dat writing completed!", "Sucess", MessageBoxButtons.OK,
            //                MessageBoxIcon.Information);
        }

        private static bool _PatchPackCheckFiles(TreeNode treeNode, List<TreeNode> packFiles)
        {
            //NodeObject nodeObject = (NodeObject) treeNode.Tag;

            //if (nodeObject.IsFolder)
            //{
            //    return treeNode.Nodes.Cast<TreeNode>().All(childNode => _PatchPackCheckFiles(childNode, packFiles));
            //}

            //if (!treeNode.Checked) return true;

            //String filePath = Path.Combine(Config.HglDir, treeNode.FullPath);
            //if (!File.Exists(filePath))
            //{
            //    DialogResult dialogResult = MessageBox.Show("A checked file was not found for packing:\n" + filePath + "\n\nIgnore missing checked files and continue?", "Warning",
            //                                                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //    return dialogResult == DialogResult.Yes;
            //}

            //packFiles.Add(treeNode);
            return true;
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
           // MessageBox.Show("todo");
            //_DoApplyFilter();

            //byte[] asdf = GetFileBytes(@"data\ai\carnagorpet.xml.cooked");
        }

        private void _FilterResetButtonClick(object sender, EventArgs e)
        {
            //MessageBox.Show("todo");
            //filter_textBox.Text = "*.*";
            //_DoApplyFilter();



            //Index idx = _indexFiles.FirstOrDefault(i => i.FileNameWithoutExtension == "hellgate000");
            //Debug.Assert(idx != null);

            //File.WriteAllBytes(idx.FilePath, idx.GenerateIndexFile());
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

        public byte[] GetFileBytes(String filePath)
        {
            TreeNode treeNode = (TreeNode) _fileTable[filePath];
            if (treeNode == null) return null;

            NodeObject nodeObject = (NodeObject) treeNode.Tag;

            return nodeObject.Index.ReadDataFile(nodeObject.FileIndex);
        }
    }
}