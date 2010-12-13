using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Hellgate;
using Reanimator.Properties;

namespace Reanimator.Forms
{
    partial class FileExplorer
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

        private class NodeObject
        {
            public IndexFile Index;
            public IndexFile.FileEntry FileEntry;
            public bool IsFolder;
            public bool IsBackup;
            public bool CanEdit;
            public bool CanCookWith;
            public bool IsUncookedVersion;
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

        private class ExtractPackPatchArgs
        {
            public bool KeepStructure;
            public bool PatchFiles;
            public String RootDir;
            public List<TreeNode> CheckedNodes;
            public IndexFile PackIndex;
        }

        /// <summary>
        /// Loop over all file entries and generate tree nodes.
        /// </summary>
        private void _GenerateFileTree()
        {
            foreach (IndexFile.FileEntry fileEntry in _fileManager.FileEntries.Values)
            {
                NodeObject nodeObject = new NodeObject { Index = fileEntry.Index, FileEntry = fileEntry };
                String[] nodeKeys = fileEntry.DirectoryStringWithoutBackup.Split('\\');
                TreeNode treeNode = null;


                // set up folders and get applicable root folder
                foreach (String nodeKey in nodeKeys.Where(nodeKey => !String.IsNullOrEmpty(nodeKey)))
                {
                    if (treeNode == null)
                    {
                        treeNode = _files_fileTreeView.Nodes[nodeKey] ?? _files_fileTreeView.Nodes.Add(nodeKey, nodeKey);
                    }
                    else
                    {
                        treeNode = treeNode.Nodes[nodeKey] ?? treeNode.Nodes.Add(nodeKey, nodeKey);
                    }
                }
                Debug.Assert(treeNode != null);


                // need to have canEdit check before we update the node below or it'll be false for newer versions);
                if (fileEntry.FileNameString.EndsWith(XmlCookedFile.FileExtention))
                {
                    nodeObject.CanEdit = true;
                    nodeObject.CanCookWith = true;
                }
                else if (fileEntry.FileNameString.EndsWith(ExcelFile.FileExtention) ||
                         fileEntry.FileNameString.EndsWith(StringsFile.FileExtention) ||
                         fileEntry.FileNameString.EndsWith("txt"))
                {
                    nodeObject.CanEdit = true;
                }


                // our new node
                TreeNode node = treeNode.Nodes.Add(fileEntry.RelativeFullPathWithoutBackup, fileEntry.FileNameString);
                _AssignIcons(node);


                // if we can cook with it, then check if the uncooked version is present
                if (nodeObject.CanCookWith)
                {
                    // sanity check
                    String nodeFullPath = node.FullPath;
                    Debug.Assert(nodeFullPath.EndsWith(".cooked"));

                    String uncookedDataPath = nodeFullPath.Replace(".cooked", "");
                    String uncookedFilePath = Path.Combine(Config.HglDir, uncookedDataPath);
                    if (File.Exists(uncookedFilePath))
                    {
                        String uncookedFileName = Path.GetFileName(uncookedFilePath);
                        TreeNode uncookedNode = node.Nodes.Add(uncookedDataPath, uncookedFileName);
                        _AssignIcons(uncookedNode);

                        NodeObject uncookedNodeObject = new NodeObject
                        {
                            IsUncookedVersion = true,
                            CanCookWith = true,
                            CanEdit = true
                        };
                        uncookedNode.Tag = uncookedNodeObject;
                    }
                }


                // final nodeObject setups
                if (nodeObject.IsBackup)
                {
                    node.ForeColor = BackupColor;
                }
                else if (!nodeObject.CanEdit)
                {
                    node.ForeColor = NoEditColor;
                }
                node.Tag = nodeObject;
            }


            // aesthetics etc
            foreach (TreeNode treeNode in _files_fileTreeView.Nodes)
            {
                if (treeNode.Index == 0)
                {
                    _files_fileTreeView.SelectedNode = treeNode;
                }

                treeNode.Expand();
                _FlagFolderNodes(treeNode);
            }
        }

        /// <summary>
        /// Sets the ImageIndex in a TreeNode depending on its FullPath "file extension".
        /// </summary>
        /// <param name="treeNode">The TreeNode to set icons to.</param>
        private static void _AssignIcons(TreeNode treeNode)
        {
            String nodePath = treeNode.FullPath;

            if (nodePath.EndsWith(XmlCookedFile.FileExtention) || nodePath.EndsWith(".xml"))
            {
                treeNode.ImageIndex = (int)IconIndex.XmlFile;
            }
            else if (nodePath.EndsWith("mp2") || nodePath.EndsWith("ogg"))
            {
                treeNode.ImageIndex = (int)IconIndex.AudioFile;
            }
            else if (nodePath.EndsWith("bik"))
            {
                treeNode.ImageIndex = (int)IconIndex.MediaFile;
            }

            treeNode.SelectedImageIndex = treeNode.ImageIndex;
        }

        /// <summary>
        /// Finds all folder nodes by recursivly searching for nodes <i>without an associated NodeObject</i>
        /// and adds a default NodeObject flagged as a folder.
        /// </summary>
        /// <param name="treeNode">Root Tree Node.</param>
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

        /// <summary>
        /// Recursivly checks a specified node and its children are checked.
        /// </summary>
        /// <param name="parentNode">The TreeNode to recursivly check.</param>
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

        /// <summary>
        /// Adds all checked nodes from a TreeNodeCollection to a Collection of TreeNodes.<br />
        /// If the node is a folder, it is not added; instead its children are checked.
        /// </summary>
        /// <param name="nodes">Root TreeNodeCollection to recursivly search.</param>
        /// <param name="checkedNodes">The Collection to add checked nodes to.</param>
        /// <returns>The total number of checked nodes.</returns>
        private static int _GetCheckedNodes(TreeNodeCollection nodes, ICollection<TreeNode> checkedNodes)
        {
            Debug.Assert(checkedNodes != null);

            foreach (TreeNode childNode in nodes)
            {
                // check children
                if (childNode.Nodes.Count > 0)
                {
                    _GetCheckedNodes(childNode.Nodes, checkedNodes);

                    // don't want folders
                    NodeObject nodeObject = (NodeObject)childNode.Tag;
                    if (nodeObject.IsFolder) continue;
                }

                if (!childNode.Checked) continue;

                checkedNodes.Add(childNode);
            }

            return checkedNodes.Count;
        }

        /// <summary>
        /// Shared threaded function for extracting and/or patching out files.
        /// </summary>
        /// <param name="progressForm">A valid user progress display form.</param>
        /// <param name="param">The operation arguments to perform.</param>
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
                if (i % progressStepRate == 0)
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

        /// <summary>
        /// Singlular file extract and patch worker function.
        /// </summary>
        /// <param name="treeNode">The current file/folder tree node.</param>
        /// <param name="overwrite">A referenced and static file overwrite option.</param>
        /// <param name="indexToWrite">A Hashtable of IndexFiles that require writing due to patching.</param>
        /// <param name="extractPatchArgs">The operation arguments to perform.</param>
        /// <returns>True upon successful extraction and/or patching of the file.</returns>
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
                overwrite = MessageBox.Show("An extract file already exists, do you wish to overwrite the file, and all following?\nFile: " + filePath,
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
            String indexFileKey;
            if (nodeObject.Siblings != null && nodeObject.Siblings.Count > 0)
            {
                // this file has siblings - loop through
                foreach (NodeObject siblingNodeObject in nodeObject.Siblings)
                {
                    IndexFile.FileEntry siblingFileEntry = siblingNodeObject.FileEntry;
                    IndexFile siblingIndex = siblingFileEntry.Index;

                    siblingIndex.PatchOutFile(siblingNodeObject.FileEntry);

                    indexFileKey = siblingIndex.FileNameWithoutExtension;
                    if (!indexToWrite.ContainsKey(indexFileKey))
                    {
                        indexToWrite.Add(indexFileKey, siblingIndex);
                    }
                }
            }


            // now patch the curr file as well
            // only add index to list if it needs to be
            IndexFile indexFile = nodeObject.Index;
            if (!indexFile.PatchOutFile(file)) return true;


            // add index to indexToWrite list
            indexFileKey = indexFile.FileNameWithoutExtension;
            if (!indexToWrite.ContainsKey(indexFileKey))
            {
                indexToWrite.Add(indexFileKey, file.Index);
            }
            return true;
        }

        /// <summary>
        /// Recursivly compares and removes nodes not satisfying the filter text.
        /// </summary>
        /// <param name="treeNode">The tree node and siblings to recursivly check.</param>
        /// <param name="filterText">The text to compare the node text to.</param>
        /// <returns>True if the node was removed from tree, false otherwise.</returns>
        private static bool _ApplyFilter(TreeNode treeNode, String filterText)
        {
            NodeObject nodeObject = (NodeObject)treeNode.Tag;
            if (nodeObject.IsFolder)
            {
                int nodeCount = treeNode.Nodes.Count;
                for (int i = 0; i < nodeCount; i++)
                {
                    if (!_ApplyFilter(treeNode.Nodes[i], filterText)) continue;

                    i--;
                    nodeCount--;
                }

                if (treeNode.Nodes.Count != 0) return false;

                treeNode.Remove();
                return true;
            }

            if (PathMatchSpec(treeNode.Text, filterText)) return false;

            treeNode.Remove();
            return true;
        }

        /// <summary>
        /// Escapes any path special characters and performs a Regex match against the compare string to the supplied filter string.
        /// </summary>
        /// <param name="compare">The string to be checked.</param>
        /// <param name="filter">The filter string to use to compare.</param>
        /// <returns>True if the path contains the filter text.</returns>
        private static bool PathMatchSpec(String compare, String filter)
        {
            String specAsRegex = Regex.Escape(filter).Replace("\\*", ".*").Replace("\\?", ".") + "$";
            return Regex.IsMatch(compare, specAsRegex);
        }

        /// <summary>
        /// Resets the TreeView filter.
        /// </summary>
        private void _ResetFilter()
        {
            // make sure we even have a filter
            if (_clonedTreeView == null) return;

            // clear current tree and clone original nodes back
            _files_fileTreeView.BeginUpdate();
            _files_fileTreeView.Nodes.Clear();
            foreach (TreeNode treeNode in _clonedTreeView.Nodes)
            {
                _files_fileTreeView.Nodes.Add((TreeNode)treeNode.Clone());

                // some aesthetics
                int nodeIndex = _files_fileTreeView.Nodes.Count - 1;
                if (nodeIndex == 0)
                {
                    _files_fileTreeView.SelectedNode = _files_fileTreeView.Nodes[0];
                }
                _files_fileTreeView.Nodes[nodeIndex].Expand();
            }
            _files_fileTreeView.EndUpdate();

            // remove cloned tree - we no longer have a filter
            _clonedTreeView.Dispose();
            _clonedTreeView = null;
        }
    }
}
