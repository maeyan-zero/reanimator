using System;
using System.Resources;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Hellgate;
using ManagedIndexType = Reanimator.FileManager.Managed;
using FileEntry = Hellgate.IndexFile.FileDefinition;

namespace Reanimator.Forms
{
    public partial class ExplorerForm : Form
    {
        private FileManager FileManager { get; set; }
        private List<Form> LoadedTables { get; set; }
        private List<TreeNode> CheckedNodes { get; set; }
        private Boolean RetainDirectories { get; set; }
        private String ExtractLocation { get; set; }

        private ExplorerForm()
        {
            InitializeComponent();
        }

        private class NodeObject
        {
            public Boolean IsFolder { get; set; }
            public Boolean IsExcelFile { get; set; }
            public ManagedIndexType File { get; set; }
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

        public ExplorerForm(FileManager fileManager)
            : this()
        {
            FileManager = fileManager;

            /**
             * Assign the tree images.
             * */
            treeView.ImageList = new ImageList();
            treeView.ImageList.Images.Add(Properties.Resources.FolderClosed);
            treeView.ImageList.Images.Add(Properties.Resources.FolderOpen);
            treeView.ImageList.Images.Add(Properties.Resources.Document);

            /**
             * Add every files to the tree as a node.
             * */
            foreach (DictionaryEntry entry in FileManager.HellgateFiles)
            {
                ManagedIndexType managedIndex = (ManagedIndexType)entry.Value;
                String fileName = managedIndex.FileName;
                String directory = managedIndex.Directory;
                
                String[] nodeKeys = managedIndex.Directory.Split('\\');
                TreeNode currTreeNode = null;
                NodeObject nodeObject = new NodeObject() { IsFolder = false };
                nodeObject.File = managedIndex;

                foreach (string nodeKey in nodeKeys.Where(nodeKey => !String.IsNullOrEmpty(nodeKey)))
                {
                    if (currTreeNode == null)
                    {
                        currTreeNode = treeView.Nodes[nodeKey] ?? treeView.Nodes.Add(nodeKey, nodeKey);
                    }
                    else
                    {
                        currTreeNode = currTreeNode.Nodes[nodeKey] ?? currTreeNode.Nodes.Add(nodeKey, nodeKey);
                    }
                }

                if (fileName.EndsWith(".txt.cooked"))
                {
                    nodeObject.IsExcelFile = true;
                }
                
                String key = managedIndex.Entry.Hash.ToString();
                String text = managedIndex.FileName;

                TreeNode node = currTreeNode.Nodes.Add(key, text);
                node.Tag = nodeObject;
            }

            // aesthetics etc
            foreach (TreeNode treeNode in treeView.Nodes)
            {
                treeNode.Expand();
                FlagFolderNodes(treeNode);
            }

            /**
             * Assign the correct icons.
             * */
            foreach (TreeNode node in treeView.Nodes)
            {
                AssignIcons(node);
            }

            treeView.TreeViewNodeSorter = new NodeSorter();
        }

        private void AssignIcons(TreeNode treeNode)
        {
            treeNode.ImageIndex = ((NodeObject)treeNode.Tag).IsFolder ? 0 : 2;

            foreach (TreeNode node in treeNode.Nodes)
            {
                AssignIcons(node);
            }
        }

        /// <summary>
        /// Finds all folder nodes by recursivly searching for nodes <i>without an associated NodeObject</i>
        /// and adds a default NodeObject flagged as a folder.
        /// </summary>
        /// <param name="treeNode">Root Tree Node.</param>
        private void FlagFolderNodes(TreeNode treeNode)
        {
            if (treeNode.Nodes.Count <= 0) return;

            if (treeNode.Tag == null)
            {
                treeNode.Tag = new NodeObject { IsFolder = true };
            }

            foreach (TreeNode childNode in treeNode.Nodes)
            {
                FlagFolderNodes(childNode);
            }

           
        }

        private void treeView_DoubleClick(object sender, EventArgs e)
        {
            TreeView treeView = sender as TreeView;
            if (treeView == null) return;

            TreeNode treeNode = treeView.SelectedNode;
            NodeObject nodeObject = (NodeObject)treeNode.Tag;

            if (nodeObject.IsExcelFile)
            {
                if (LoadedTables == null)
                    LoadedTables = new List<Form>();

                String stringID = treeNode.Text.Remove(treeNode.Text.IndexOf('.')).ToUpper(); ;
                IEnumerable<Form> formVar = LoadedTables.Where(f => f.Text.Contains(stringID));
                if (formVar.Count() > 0)
                {
                    formVar.First().Show();
                    return;
                }

                DataTable dataTable = FileManager.GetDataTable(stringID);
                if (dataTable == null) return;
                ExcelForm excelForm = new ExcelForm(dataTable)
                {
                    MdiParent = this.MdiParent
                };

                if (Config.EnableExcelRelations)
                {
                    FileManager.GenerateRelations(stringID);
                }
                excelForm.Show();
                LoadedTables.Add(excelForm);
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeView treeView = sender as TreeView;
            if (treeView == null) return;

            TreeNode treeNode = treeView.SelectedNode;
            NodeObject nodeObject = (NodeObject)treeNode.Tag;
            if (nodeObject.IsFolder)
            {
                parentTextBox.Text = String.Empty;
                timestampTextBox.Text = String.Empty;
                compressedTextBox.Text = String.Empty;
                uncompressedTextBox.Text = String.Empty;
            }
            else
            {
                parentTextBox.Text = Path.GetFileName(nodeObject.File.Parent.FilePath);
                timestampTextBox.Text = DateTime.FromFileTime(nodeObject.File.Entry.FileStruct.FileTime).ToString();
                compressedTextBox.Text = String.Format("{0:n0}", nodeObject.File.Entry.FileStruct.CompressedSize);
                uncompressedTextBox.Text = String.Format("{0:n0}", nodeObject.File.Entry.FileStruct.UncompressedSize);
            }
        }

        private void treeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TreeView treeView = sender as TreeView;
            if (treeView == null) return;

            treeView.AfterCheck -= treeView_AfterCheck;
            ToggleChildNodes(e.Node);
            treeView.AfterCheck += treeView_AfterCheck;
        }

        private void ToggleChildNodes(TreeNode parentNode)
        {
            foreach (TreeNode childNode in parentNode.Nodes)
            {
                childNode.Checked = parentNode.Checked;
                if (childNode.Nodes.Count > 0)
                {
                    ToggleChildNodes(childNode);
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CheckedNodes = new List<TreeNode>();
            if (GetCheckedNodes(treeView.Nodes, CheckedNodes) == 0)
            {
                MessageBox.Show("No items selected to extract.");
                return;
            }

            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.ShowNewFolderButton = true;
            if (!(folderBrowser.ShowDialog() == DialogResult.OK)) return;
            ExtractLocation = folderBrowser.SelectedPath;

            DialogResult result = MessageBox.Show("Retain directory structure?", "Question", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Cancel) return;
            RetainDirectories = result == DialogResult.Yes ? true : false;
 

            Reanimator reanimatorForm = (Reanimator)MdiParent;
            extractBackgroundWorker.DoWork += new DoWorkEventHandler(Extract_DoWork);
            extractBackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(reanimatorForm.Status_ProgressChanged);
            extractBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(reanimatorForm.Status_RunWorkerCompleted);
            extractBackgroundWorker.RunWorkerAsync();

            reanimatorForm.EnableCancelButton();
        }

        private void Extract_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Int32 total = CheckedNodes.Count;
            Int32 i = 1;

            foreach (TreeNode node in CheckedNodes)
            {
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }

                Int32 progress = ((i * 100) / total);
                NodeObject nodeObject = (NodeObject)node.Tag;
                String fileName = nodeObject.File.FileName;
                worker.ReportProgress(progress, "Extracting " + fileName + "..");
                worker.ReportProgress(progress);
                String extractPath = RetainDirectories ? Path.Combine(ExtractLocation, nodeObject.File.Directory) : ExtractLocation;
                Byte[] buffer = FileManager.GetFileBytes(nodeObject.File);
                // check there was no error
                if (!(buffer == null))
                {
                    if (!(Directory.Exists(extractPath))) Directory.CreateDirectory(extractPath);
                    String extractFilePath = Path.Combine(extractPath, nodeObject.File.FileName);
                    File.WriteAllBytes(extractFilePath, buffer);
                }
                i++;
            }
        }

        private int GetCheckedNodes(TreeNodeCollection nodes, List<TreeNode> checkedNodes)
        {
            foreach (TreeNode childNode in nodes)
            {
                // check children
                if (childNode.Nodes.Count > 0)
                {
                    GetCheckedNodes(childNode.Nodes, checkedNodes);

                    // don't want folders
                    NodeObject nodeObject = (NodeObject)childNode.Tag;
                    if (nodeObject.IsFolder) continue;
                }

                if (!childNode.Checked) continue;
                checkedNodes.Add(childNode);
            }

            return checkedNodes.Count;
        }

        private void ExplorerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (extractBackgroundWorker.IsBusy) extractBackgroundWorker.CancelAsync();
        }

        public void CancelExtraction()
        {
            extractBackgroundWorker.CancelAsync();
        }
    }
}
