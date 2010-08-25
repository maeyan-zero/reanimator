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

        private static readonly int[] IndexOpen = { Index.Base000, Index.LatestPatch, Index.LatestPatchLocalized };
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

        public FileExplorer()
        {
            InitializeComponent();
            files_treeView.DoubleBuffered(true);

            _indexFiles = new List<Index>();
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
                progressForm.ConfigBar(1, IndexOpen.Length, 1);
            }

            String idxFilesRoot = Path.Combine(Config.HglDir, "data");
            foreach (String queryString in IndexQueryStrings)
            {
                String[] datFiles = Directory.GetFiles(idxFilesRoot, queryString);
                if (datFiles.Length == 0) continue;

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
                fileSize_textBox.Text = "NA";
                fileCompressed_textBox.Text = "NA";
                loadingLocation_textBox.Text = "NA";
                fileTime_textBox.Text = "NA";

                revertFile_button.Enabled = false;
                revertFile_label.Text = "Folder elements can't be backed-up/restored.";
                extract_label.Text = "Extract this folder and all files & folders within it.";
                extractPatch_label.Text = "Extract this folder and all files & folders within it, and then patch the index to force the game to load the extracted files over the .dat.\nNote: Non-patchable files (e.g. sounds) wont be patched out and will only be extracted.";

                return;
            }

            Index.FileIndex fileIndex = nodeObject.FileIndex;
            Debug.Assert(fileIndex != null);

            fileName_textBox.DataBindings.Add("Text", fileIndex, "FileNameString");
            fileSize_textBox.DataBindings.Add("Text", fileIndex, "UncompressedSize");
            fileCompressed_textBox.DataBindings.Add("Text", fileIndex, "CompressedSize");
            fileTime_textBox.Text = (DateTime.FromFileTime((long)fileIndex.FileStruct.FileTime)).ToString();

            if (nodeObject.IsBackup)
            {
                revertFile_button.Enabled = true;
                revertFile_label.Text = "Restore this file to its original state so the game loads from the .dat as originally.";
            }
            else
            {
                revertFile_button.Enabled = false;
                revertFile_label.Text = "This file is neither modified nor patched out.";
            }

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

            }
            else if (fileIndex.FileNameString.EndsWith(XmlCookedFile.FileExtention))
            {

            }
            else if (fileIndex.FileNameString.EndsWith(".txt"))
            {

            }
            else
            {
                MessageBox.Show("Unexpected editable file!\n(this shouldn't happen - please report this)", "Warning",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void _RevertFileButtonClick(object sender, EventArgs e)
        {
            MessageBox.Show("Todo");
        }

        private void _ExtractButtonClick(object sender, EventArgs e)
        {
            MessageBox.Show("Todo");
        }

        private void _ExtractPatchButtonClick(object sender, EventArgs e)
        {
            TreeNode selectedNodes = new TreeNode();
            TreeNodeCollection nodes = files_treeView.Nodes;
            foreach (TreeNode node in nodes)
            {
                _AddCheckedNodes(node, selectedNodes);
            }

            if (selectedNodes.Nodes.Count == 0) return;

            DialogResult dialogResult = MessageBox.Show(
                "Extract & Patch out the selected file/s?",
                "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.No) return;

            ProgressForm progressForm = new ProgressForm(_DoExtractPatch, selectedNodes);
            progressForm.SetLoadingText("Patching and Extracting files...");
            progressForm.SetStyle(ProgressBarStyle.Marquee);
            progressForm.Show();
        }

        private void _AddCheckedNodes(TreeNode caseNode, TreeNode nodeCollection)
        {
            foreach (TreeNode node in caseNode.Nodes)
            {
                NodeObject nodeObject = node.Tag as NodeObject;
                if (node.Checked && !nodeObject.IsFolder)
                {
                    TreeNode nodeClone = (TreeNode)node.Clone();
                    nodeCollection.Nodes.Add(nodeClone);
                }
                //recursive call
                _AddCheckedNodes(node, nodeCollection);
            }
        }

        private static void _DoExtractPatch(ProgressForm progressForm, Object param)
        {
            // todo: add proper progress counter, etc?
            progressForm.SetCurrentItemText("Extracting file(s)...");
            TreeNode selectedNodes = (TreeNode) param;
            DialogResult overwrite = DialogResult.None;
            Hashtable indexToWrite = new Hashtable();

            foreach (TreeNode node in selectedNodes.Nodes)
            {
                if (!_ExtractFiles(node, ref overwrite, indexToWrite)) return;
            }

            if (indexToWrite.Count == 0)
            {
                MessageBox.Show("File(s) extracted sucessfully!\nNo index files require modifications.", "Complete",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DialogResult writeIdx = MessageBox.Show("Files extracted sucessfully!\nSave modified index file(s)?", "Question", MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Question);
            if (writeIdx == DialogResult.No) return;

            progressForm.SetCurrentItemText("Saving modified index file(s)...");
            foreach (Index idx in
                from DictionaryEntry indexDictionary in indexToWrite select (Index)indexDictionary.Value)
            {
                File.WriteAllBytes(idx.FilePath, idx.GenerateIndexFile());
            }
            MessageBox.Show("Modified index file(s) saved sucessfully!", "Saved", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);            
        }

        private static bool _ExtractFiles(TreeNode treeNode, ref DialogResult overwrite, Hashtable indexToWrite)
        {
            if (treeNode == null) return false;

            NodeObject nodeObject = treeNode.Tag as NodeObject;
            Index.FileIndex fileIndex = nodeObject.FileIndex;
            String hglPath = Path.Combine(fileIndex.DirectoryString.Replace(Index.BackupPrefix + @"\", ""),
                                         fileIndex.FileNameString);
            String filePath = Path.Combine(Config.HglDir, hglPath);
            bool fileExists = File.Exists(filePath);
            if (fileExists && overwrite == DialogResult.None)
            {
                overwrite = MessageBox.Show("An extract file already exists, do you wish to overwrite the file, and all following?\n\nFile: " + filePath,
                    "Question", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (overwrite == DialogResult.Cancel) return false;
            }

            if (fileExists && overwrite == DialogResult.No) return true;

            DialogResult extractDialogResult = DialogResult.Retry;
            while (extractDialogResult == DialogResult.Retry)
            {
                byte[] fileBytes = fileIndex.InIndex.ReadDataFile(fileIndex);
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

            // don't patch out string files or music/movie files
            if (fileIndex.FileNameString.EndsWith(StringsFile.FileExtention) ||
                fileIndex.FileNameString.EndsWith(".ogg") ||
                fileIndex.FileNameString.EndsWith(".mp2") ||
                fileIndex.FileNameString.EndsWith(".bik")) return true;

            // only add index to list if it needs to be
            if (!fileIndex.InIndex.PatchOutFile(fileIndex)) return true;

            String fileIndexKey = fileIndex.InIndex.FileNameWithoutExtension;
            if (!indexToWrite.ContainsKey(fileIndexKey))
            {
                indexToWrite.Add(fileIndexKey, fileIndex.InIndex);
            }
            return true;
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

        private void files_treeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            files_treeView.AfterCheck -= files_treeView_AfterCheck;
            // NOTE: even after disabling the check event, the performance is exactly the same.
            // This is only left here for the meantime for demonstrative/debugging purposes.
            _CheckChildNodes(e.Node);
            files_treeView.AfterCheck += files_treeView_AfterCheck;
        }

        private void _CheckChildNodes(TreeNode parentNode)
        {
            foreach (TreeNode childNode in parentNode.Nodes)
            {
                childNode.Checked = parentNode.Checked;
                _CheckChildNodes(childNode);
            }
        }
    }
}