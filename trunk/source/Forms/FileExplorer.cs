using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Reanimator.Forms
{
    public partial class FileExplorer : Form
    {
        private static readonly Color BackupColor = Color.IndianRed;
        private static readonly Color NoEditColor = Color.DimGray;

        private static readonly int[] IndexOpen = { Index.Base000, Index.LatestPatch, Index.LatestPatchLocalized };
        private readonly List<Index> _indexFiles;
        private readonly Hashtable _fileTable;

        private class NodeObject
        {
            public Index Index;
            public Index.FileIndex FileIndex;
            public bool IsFolder;
            public bool IsBackup;
        }

        public FileExplorer()
        {
            InitializeComponent();

            _indexFiles = new List<Index>();
            _fileTable = new Hashtable();
            backupKey_label.ForeColor = BackupColor;
            noEditorKey_label.ForeColor = NoEditColor;
        }

        public void LoadIndexFiles(ProgressForm progressForm, Object param)
        {
            if (progressForm != null)
            {
                progressForm.SetLoadingText("Loading game file system...");
                progressForm.ConfigBar(1, IndexOpen.Length, 1);
            }

            files_treeView.BeginUpdate();

            foreach (int idxIndex in IndexOpen)
            {
                String indexPath = Path.Combine(Path.Combine(Config.HglDir, "data"), Index.FileNames[idxIndex] + ".idx");
                if (!File.Exists(indexPath)) continue;

                if (progressForm != null)
                {
                    progressForm.SetCurrentItemText("Loading " + Index.FileNames[idxIndex] + "...");
                }

                byte[] indexData = File.ReadAllBytes(indexPath);
                Index index = new Index();
                if (!index.ParseData(indexData, indexPath))
                {
                    MessageBox.Show("Failed to read index file:\n" + indexPath, "Warning", MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    continue;
                }
                _indexFiles.Add(index);

                foreach (Index.FileIndex file in index.FileTable)
                {
                    NodeObject nodeObject = new NodeObject {Index = index};
                    String[] nodeKeys = file.DirectoryString.Split('\\');
                    TreeNode treeNode = null;

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

                    if (treeNode.Parent != null)
                    {
                        if (treeNode.Parent.Tag == null)
                        {
                            treeNode.Parent.Tag = new NodeObject { IsFolder = true };
                        }
                    }

                    String key = file.DirectoryString + file.FileNameString;
                    if (_fileTable.Contains(key))
                    {
                        TreeNode fileNode = (TreeNode)_fileTable[key];
                        NodeObject nodeObj = (NodeObject)fileNode.Tag;

                        // is it the same index?
                        if (nodeObj.Index.FilePath == index.FilePath) continue;
                        
                        // from a newer index
                        treeNode.Nodes.Remove(fileNode);
                        _fileTable.Remove(key);
                    }

                    nodeObject.FileIndex = file;

                    TreeNode node = treeNode.Nodes.Add(key, file.FileNameString);
                    node.Tag = nodeObject;
                    node.ForeColor = NoEditColor;
                    _fileTable.Add(key, node);

                    if (nodeObject.IsBackup)
                    {
                        node.ForeColor = BackupColor;
                    }
                    else if (file.DirectoryString.Contains("skills") && file.FileNameString.Contains(".xml.cooked"))
                    {
                        node.ForeColor = Color.Black;
                    }
                    else if (file.DirectoryString.Contains("excel") && file.FileNameString.Contains(".txt.cooked"))
                    {
                        node.ForeColor = Color.Black;
                    }
                }
            }

            files_treeView.Sort();

            // expand highest nodes & select first for aesthetics
            foreach (TreeNode expandNode in files_treeView.Nodes)
            {
                if (expandNode.Index == 0)
                {
                    files_treeView.SelectedNode = expandNode;
                }

                expandNode.Expand();
            }

            files_treeView.EndUpdate();

            /*
             * treeView1.TreeViewNodeSorter = new NodeSorter();

            class NodeSorter : IComparer
            {
                public int Compare(object x, object y)
                {
                    TreeNode nodeX = (TreeNode)x;
                    TreeNode nodeY = (TreeNode)y;
                    return nodeX.Text.CompareTo(nodeY.Text);
                }
            }
             */
        }

        private void _FilesTreeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeView treeView = (TreeView)sender;
            TreeNode selectedNode = treeView.SelectedNode;
            NodeObject nodeObject = (NodeObject) selectedNode.Tag;

            textBox1.DataBindings.Clear();
            textBox2.DataBindings.Clear();
            textBox3.DataBindings.Clear();

            if (nodeObject.IsFolder) // if is a folder
            {
                textBox1.Text = selectedNode.Text;
                textBox2.Text = "NA";
                textBox3.Text = "NA";
                textBox4.Text = "NA";

                revertFile_button.Enabled = false;
                label5.Text = "Folder elements can't be backed-up/restored.";
                label6.Text = "Extract this folder and all files & folders within it.";
                label7.Text = "Extract this folder and all files & folders within it, and then patch the index to force the game to load the extracted files over the .dat.\n\nWarning: Not all files can be safely patched; use with caution.";

                return;
            }

            Index.FileIndex fileIndex = nodeObject.FileIndex;
            Debug.Assert(fileIndex != null);

            textBox1.DataBindings.Add("Text", fileIndex, "FileNameString");
            textBox2.DataBindings.Add("Text", fileIndex, "UncompressedSize");
            textBox3.DataBindings.Add("Text", fileIndex, "CompressedSize");

            if (nodeObject.IsBackup)
            {
                revertFile_button.Enabled = true;
                label5.Text = "Restore this file to its original state so the game loads from the .dat as originally.";
            }
            else
            {
                revertFile_button.Enabled = false;
                label5.Text = "This file is neither modified nor patched out.";
            }

            if (fileIndex.Modified)
            {
                String fileDataPath = Path.Combine(fileIndex.DirectoryString.Replace(Index.BackupPrefix, ""), fileIndex.FileNameString);
                String filePath = Config.HglDir + fileDataPath;
                if (File.Exists(filePath))
                {
                    textBox4.DataBindings.Clear();
                    textBox4.Text = filePath;
                }
                else
                {
                    textBox4.DataBindings.Clear();
                    textBox4.DataBindings.Add("Text", fileIndex, "InIndex");
                }
            }
            else
            {
                textBox4.DataBindings.Clear();
                textBox4.DataBindings.Add("Text", fileIndex, "InIndex");
            }
        }

        private void _FilesTreeViewDoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("Todo");
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
            MessageBox.Show("Todo");
        }
    }
}