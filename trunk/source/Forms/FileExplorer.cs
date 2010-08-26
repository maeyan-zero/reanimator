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
        private static readonly Icon[] Icons = { Resources.Generic_Document, Resources.folder, Resources.folder_open };
        private enum IconIndex
        {
            GenericDocument,
            Folder,
            FolderOpen
        }

        private static readonly Color BackupColor = Color.IndianRed;
        private static readonly Color NoEditColor = Color.DimGray;

        private static readonly String[] IndexQueryStrings = { "hellgate*.dat", "sp_hellgate*.dat" };
        private readonly List<Index> _indexFiles;
        private readonly Hashtable _fileTable;

        private class NodeObject
        {
            public Index Index;
            public Index.FileIndex FileIndex;
            public bool IsFolder;
            public bool IsBackup;
            public bool CanEdit;
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
            foreach (Index.FileIndex file in index.FileTable)
            {
                NodeObject nodeObject = new NodeObject { Index = index };
                String[] nodeKeys = file.DirectoryString.Split('\\');
                TreeNode treeNode = null;

                // can we edit the file via. Reanimator?
                if ((nodeKeys.Contains(XmlCookedSkill.RootFolder) && file.FileNameString.EndsWith(XmlCookedFile.FileExtention)) ||
                    file.FileNameString.EndsWith(ExcelFile.FileExtention) ||
                    file.FileNameString.EndsWith(StringsFile.FileExtention) ||
                    file.FileNameString.EndsWith("txt"))
                {
                    nodeObject.CanEdit = true;
                }


                // set up folders and get applicable root folder
                foreach (string nodeKey in nodeKeys.Where(nodeKey => !String.IsNullOrEmpty(nodeKey)))
                {
                    if (nodeKey == Index.BackupPrefix)
                    {
                        nodeObject.IsBackup = true;
                        continue;
                    }

                    if (treeNode == null)
                    {
                        treeNode = files_treeView.Nodes[nodeKey] ?? files_treeView.Nodes.Add(nodeKey, nodeKey);
                    }
                    else
                    {
                        treeNode = treeNode.Nodes[nodeKey] ?? treeNode.Nodes.Add(nodeKey, nodeKey);
                    }
                }
                Debug.Assert(treeNode != null);


                // have we already added the file? if so, remove it for updated version
                String key = file.DirectoryString.Replace(Index.BackupPrefix + @"\", "") + file.FileNameString;
                if (_fileTable.Contains(key))
                {
                    TreeNode fileNode = (TreeNode)_fileTable[key];
                    NodeObject nodeObj = (NodeObject)fileNode.Tag;

                    // is it a newer file?
                    if (nodeObj.FileIndex.FileStruct.FileTime > file.FileStruct.FileTime) continue;

                    // no, it's from a newer index
                    treeNode.Nodes.Remove(fileNode);
                    _fileTable.Remove(key);
                }


                // add file/node
                TreeNode node = treeNode.Nodes.Add(key, file.FileNameString);
                node.Tag = nodeObject;
                _fileTable.Add(key, node);


                // final nodeObject setups
                nodeObject.FileIndex = file;
                if (nodeObject.IsBackup)
                {
                    node.ForeColor = BackupColor;
                }
                else if (!nodeObject.CanEdit)
                {
                    node.ForeColor = NoEditColor;
                }
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

            Index.FileIndex fileIndex = nodeObject.FileIndex;
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

            Index.FileIndex fileIndex = nodeObject.FileIndex;

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
            Index.FileIndex file = nodeObject.FileIndex;
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

            // only add index to list if it needs to be
            if (!fileIndex.PatchOutFile(file)) return true;

            String fileIndexKey = fileIndex.FileNameWithoutExtension;
            if (!indexToWrite.ContainsKey(fileIndexKey))
            {
                indexToWrite.Add(fileIndexKey, file.InIndex);
            }
            return true;
        }

        private void _PackPatchButtonClick(object sender, EventArgs e)
        {
            MessageBox.Show("todo");
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
    }
}