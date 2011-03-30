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
using Config = Revival.Common.Config;
using ExceptionLogger = Revival.Common.ExceptionLogger;

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
            public PackFile Index;
            public PackFileEntry FileEntry;
            public bool IsFolder;
            public bool CanEdit;
            public bool CanCookWith;
            public bool IsUncookedVersion;
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
            public bool ExtractFiles;
            public bool KeepStructure;
            public bool PatchFiles;
            public String RootDir;
            public List<TreeNode> CheckedNodes;
            public IndexFile PackIndex;
        }

        public TreeNodeCollection GetDirectories(String directory)
        {
            directory = directory.Replace(Config.HglDir, ""); // remove absolute reference for tree node lookups
            String[] directoryKeys = directory.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

            TreeNodeCollection currDir = _files_fileTreeView.Nodes;
            foreach (String dir in directoryKeys)
            {
                if (currDir == null || currDir.Count == 0) return null;

                TreeNode currNode = currDir[dir];
                if (currNode == null) return null;

                currDir = currNode.Nodes;
            }

            return currDir;
        }

        /// <summary>
        /// Loop over all file entries and generate tree nodes.
        /// </summary>
        private void _GenerateFileTree()
        {
            foreach (PackFileEntry fileEntry in _fileManager.FileEntries.Values)
            {
                NodeObject nodeObject = new NodeObject { Index = fileEntry.Pack, FileEntry = fileEntry };
                String[] nodeKeys = fileEntry.Directory.Split('\\');
                TreeNode treeNode = null;

                //if (fileEntry.FileNameString == "ct_conna.mop")
                //{
                //    int bp = 0;
                //}

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
                if (fileEntry.Name.EndsWith(XmlCookedFile.Extension))
                    // todo: before we can do this, need to fix up the "sanity check" part just below
                    //fileEntry.FileNameString.EndsWith(RoomDefinitionFile.Extension) ||
                    //fileEntry.FileNameString.EndsWith(LevelRulesFile.Extension) ||
                    //fileEntry.FileNameString.EndsWith(MLIFile.Extension))
                {
                    nodeObject.CanEdit = true;
                    nodeObject.CanCookWith = true;
                }
                else if (fileEntry.Name.EndsWith(ExcelFile.Extension) ||
                         fileEntry.Name.EndsWith(StringsFile.Extention) ||
                         fileEntry.Name.EndsWith("txt"))
                {
                    nodeObject.CanEdit = true;
                }


                // our new node
                TreeNode node = treeNode.Nodes.Add(fileEntry.Path, fileEntry.Name);
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
                if (nodeObject.FileEntry.IsPatchedOut)
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
        /// Recursively sets the node colors to match the node object states.
        /// </summary>
        /// <param name="treeNodeCollection">The tree nodes to recursively check.</param>
        private static void _AssignColors(TreeNodeCollection treeNodeCollection)
        {
            foreach (TreeNode treeNode in treeNodeCollection)
            {
                if (treeNode.Nodes != null && treeNode.Nodes.Count > 1)
                {
                    _AssignColors(treeNode.Nodes);
                    continue;
                }

                //if (treeNode.Text == "ct_conna.mop")
                //{
                //    int bp = 0;
                //}

                NodeObject nodeObject = (NodeObject) treeNode.Tag;
                if (nodeObject.FileEntry != null && nodeObject.FileEntry.IsPatchedOut)
                {
                    treeNode.ForeColor = BackupColor;
                }
                else if (!nodeObject.CanEdit)
                {
                    treeNode.ForeColor = NoEditColor;
                }
            }
        }

        /// <summary>
        /// Sets the ImageIndex in a TreeNode depending on its FullPath "file extension".
        /// </summary>
        /// <param name="treeNode">The TreeNode to set icons to.</param>
        private static void _AssignIcons(TreeNode treeNode)
        {
            String nodePath = treeNode.FullPath;

            if (nodePath.EndsWith(XmlCookedFile.Extension) || nodePath.EndsWith(".xml"))
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

            try
            {
                _fileManager.BeginAllDatReadAccess();
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to open dat files for reading!\nEnsure no other programs are using them and try again.\n" + e, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                File.WriteAllBytes(idx.Path, idxData);
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

            PackFileEntry fileEntry = nodeObject.FileEntry;


            // are we extracting?
            if (extractPatchArgs.ExtractFiles)
            {
                // get path
                String filePath = extractPatchArgs.KeepStructure
                                      ? Path.Combine(extractPatchArgs.RootDir, treeNode.FullPath)
                                      : Path.Combine(extractPatchArgs.RootDir, fileEntry.Name);


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
                    byte[] fileBytes = _fileManager.GetFileBytes(fileEntry, extractPatchArgs.PatchFiles);
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
                    File.SetLastWriteTime(filePath, fileEntry.LastModified);
                    break;
                }
            }


            // are we patching?
            if (!extractPatchArgs.PatchFiles) return true;


            // don't patch out string files or sound/movie files
            if (IndexFile.NoPatchExt.Any(ext => fileEntry.Name.EndsWith(ext))) return true;


            // if we're patching out the file, then change its bgColor and set its nodeObject state to backup
            treeNode.ForeColor = BackupColor;


            // is this file located else where? (i.e. does it have Siblings)
            String indexFileKey;
            if (fileEntry.Siblings != null && fileEntry.Siblings.Count > 0)
            {
                // this file has siblings - loop through
                foreach (PackFileEntry siblingFileEntry in fileEntry.Siblings.Where(siblingFileEntry => !siblingFileEntry.IsPatchedOut))
                {
                    siblingFileEntry.IsPatchedOut = true;

                    indexFileKey = siblingFileEntry.Pack.NameWithoutExtension;
                    if (!indexToWrite.ContainsKey(indexFileKey))
                    {
                        indexToWrite.Add(indexFileKey, siblingFileEntry.Pack);
                    }
                }
            }


            // now patch the curr file as well
            // only add index to list if it needs to be
            PackFile indexFile = nodeObject.Index;
            if (fileEntry.IsPatchedOut) return true;

            fileEntry.IsPatchedOut = true;
            indexFileKey = indexFile.NameWithoutExtension;
            if (!indexToWrite.ContainsKey(indexFileKey))
            {
                indexToWrite.Add(indexFileKey, fileEntry.Pack);
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
            _AssignColors(_files_fileTreeView.Nodes);
            _files_fileTreeView.EndUpdate();

            // remove cloned tree - we no longer have a filter
            _clonedTreeView.Dispose();
            _clonedTreeView = null;
        }

        /// <summary>
        /// User-friendly uncooking of Tree Node list.
        /// </summary>
        /// <param name="progressForm">A progress form to update.</param>
        /// <param name="param">The Tree Node List.</param>
        private void _DoUnooking(ProgressForm progressForm, Object param)
        {
            List<TreeNode> uncookingNodes = (List<TreeNode>)param;
            const int progressUpdateFreq = 20;
            if (progressForm != null)
            {
                progressForm.ConfigBar(1, uncookingNodes.Count, progressUpdateFreq);
            }

            int i = 0;
            foreach (TreeNode treeNode in uncookingNodes)
            {
                NodeObject nodeObject = (NodeObject)treeNode.Tag;
                PackFileEntry fileEntry = nodeObject.FileEntry;


                // update progress if applicable
                if (i % progressUpdateFreq == 0 && progressForm != null)
                {
                    progressForm.SetCurrentItemText(fileEntry.Path);
                }
                i++;


                // get the file bytes
                String relativePath = fileEntry.Path;
                byte[] fileBytes;
                try
                {
                    fileBytes = _fileManager.GetFileBytes(fileEntry, true);
                    if (fileBytes == null) continue;
                }
                catch (Exception e)
                {
                    ExceptionLogger.LogException(e);
                    continue;
                }


                // determine file type
                HellgateFile hellgateFile;
                if (relativePath.EndsWith(XmlCookedFile.Extension))
                {
                    hellgateFile = new XmlCookedFile();
                }
                else if (relativePath.EndsWith(RoomDefinitionFile.Extension))
                {
                    hellgateFile = new RoomDefinitionFile();
                }
                else if (relativePath.EndsWith(MLIFile.Extension))
                {
                    hellgateFile = new MLIFile();
                }
                else
                {
                    Debug.Assert(false, "wtf");
                    continue;
                }


                // deserialise file
                DialogResult dr = DialogResult.Retry;
                bool uncooked = false;
                while (dr == DialogResult.Retry && !uncooked)
                {
                    try
                    {
                        hellgateFile.ParseFileBytes(fileBytes);
                        uncooked = true;
                    }
                    catch (Exception e)
                    {
                        ExceptionLogger.LogException(e, "_DoUnooking", true);

                        String errorMsg = String.Format("Failed to uncooked file!\n{0}\n\n{1}", relativePath, e);
                        dr = MessageBox.Show(errorMsg, "Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Exclamation);
                        if (dr == DialogResult.Abort) return;
                        if (dr == DialogResult.Ignore) break;
                    }
                }
                if (!uncooked) continue;


                // save file
                String relativeSavePath = relativePath.Replace(HellgateFile.Extension, HellgateFile.ExtensionDeserialised);
                String savePath = Path.Combine(Config.HglDir, relativeSavePath);

                dr = DialogResult.Retry;
                bool saved = false;
                byte[] documentBytes = null;
                while (dr == DialogResult.Retry && !saved)
                {
                    try
                    {
                        if (documentBytes == null) documentBytes = hellgateFile.ExportAsDocument();
                        File.WriteAllBytes(savePath, documentBytes);
                        saved = true;
                    }
                    catch (Exception e)
                    {
                        ExceptionLogger.LogException(e, "_DoUnooking", true);

                        String errorMsg = String.Format("Failed to save file!\n{0}\n\n{1}", relativePath, e);
                        dr = MessageBox.Show(errorMsg, "Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Exclamation);
                        if (dr == DialogResult.Abort) return;
                        if (dr == DialogResult.Ignore) break;
                    }
                }


                // update tree view
                TreeNode newTreeNode = new TreeNode();
                NodeObject newNodeObject = new NodeObject
                {
                    CanEdit = true,
                    IsUncookedVersion = true
                };
                newTreeNode.Tag = newNodeObject;
                treeNode.Nodes.Add(newTreeNode);
            }
        }

    }
}
