using System;
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
        private TreeView _clonedTreeView;

        /// <summary>
        /// Main constructor. Initialises the file tree system from a valid FileManager.
        /// </summary>
        /// <param name="fileManager">The FileManager to base the explorer tree on.</param>
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

        /// <summary>
        /// Attempt to decrease user wait time by hiding until completely filled.
        /// </summary>
        /// <param name="sender">The TreeView clicked.</param>
        /// <param name="e">The Shown event args.</param>
        private void FileExplorer_Shown(object sender, EventArgs e)
        {
            _files_fileTreeView.EndUpdate();
        }

        /// <summary>
        /// Event Function for Tree View - After Select.
        /// Will update the file details based upon selection.
        /// </summary>
        /// <param name="sender">The TreeView clicked.</param>
        /// <param name="e">The After Select event args.</param>
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
                String fileDataPath = parentNode.Name.Replace(".cooked", "");
                String filePath = Path.Combine(Config.HglDir, fileDataPath);

                FileInfo fileInfo;
                try
                {
                    fileInfo = new FileInfo(filePath);
                }
                catch (Exception)
                {
                    // todo remove file from tree as it has been moved(?)
                    return;
                }

                fileName_textBox.Text = fileInfo.Name;
                fileName_textBox.ReadOnly = true;
                fileSize_textBox.Text = fileInfo.Length.ToString();
                fileCompressed_textBox.Text = String.Empty;
                fileTime_textBox.Text = fileInfo.CreationTime.ToString();
                loadingLocation_textBox.Text = fileDataPath;

                return;
            }


            fileName_textBox.ReadOnly = false;
            fileName_textBox.DataBindings.Add("Text", fileIndex, "FileNameString");
            fileSize_textBox.DataBindings.Add("Text", fileIndex, "UncompressedSize");
            fileCompressed_textBox.DataBindings.Add("Text", fileIndex, "CompressedSize");
            fileTime_textBox.Text = (DateTime.FromFileTime(fileIndex.FileStruct.FileTime)).ToString();

            if (fileIndex.IsPatchedOut)
            {
                String fileDataPath = selectedNode.FullPath;
                String filePath = Path.Combine(Config.HglDir, fileDataPath);
                if (File.Exists(filePath))
                {
                    loadingLocation_textBox.Text = fileDataPath;
                }
                else
                {
                    loadingLocation_textBox.DataBindings.Add("Text", fileIndex, "Index");
                }
            }
            else
            {
                loadingLocation_textBox.DataBindings.Add("Text", fileIndex, "Index");
            }
        }

        // todo: update/fix me
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

        /// <summary>
        /// Event Function for "Extract to..." Button - Click.
        /// Checks and extracts files to prompted location.
        /// </summary>
        /// <param name="sender">The button clicked.</param>
        /// <param name="e">The Click event args.</param>
        private void _ExtractButton_Click(object sender, EventArgs e)
        {
            // make sure we have at least 1 checked file
            List<TreeNode> checkedNodes = new List<TreeNode>();
            if (_GetCheckedNodes(_files_fileTreeView.Nodes, checkedNodes) == 0)
            {
                MessageBox.Show("No files checked for extraction!", "Need Files", MessageBoxButtons.OK,
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

        /// <summary>
        /// Event Function for "Extract and Patch Index" Button -  Click.
        /// Checks and extracts files to HGL data locations, then patches out files and saves updated index files.
        /// </summary>
        /// <param name="sender">The button clicked.</param>
        /// <param name="e">The Click event args.</param>
        private void _ExtractPatchButton_Click(object sender, EventArgs e)
        {
            // make sure we have at least 1 checked file
            List<TreeNode> checkedNodes = new List<TreeNode>();
            if (_GetCheckedNodes(_files_fileTreeView.Nodes, checkedNodes) == 0)
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

            _files_fileTreeView.BeginUpdate();
            ProgressForm progressForm = new ProgressForm(_DoExtractPatch, extractPatchArgs);
            progressForm.SetLoadingText(String.Format("Extracting and Patching file(s)... ({0})", checkedNodes.Count));
            progressForm.Disposed += delegate { _files_fileTreeView.EndUpdate(); };
            progressForm.Show(this);
        }

        // todo: rewrite me
        private void _PackPatchButton_Click(object sender, EventArgs e)
        {
            // make sure we have at least 1 checked file
            List<TreeNode> checkedNodes = new List<TreeNode>();
            if (_GetCheckedNodes(_files_fileTreeView.Nodes, checkedNodes) == 0)
            {
                MessageBox.Show("No files checked for packing!", "Need Files", MessageBoxButtons.OK,
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
                    packResults.WriteLine("{0} - File Not Found", filePath);
                    continue;
                }

                // ensure it was once packed (need FilePathHash etc)
                // todo: implement Crypt.StringHash for FilePathHash and FolderPathHash
                NodeObject nodeObject = (NodeObject)checkedNode.Tag;
                if (nodeObject.FileEntry == null ||
                    nodeObject.FileEntry.FileStruct == null ||
                    nodeObject.FileEntry.FileStruct.FileNameSHA1Hash == 0)
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
                //oldNodeObject.AddSibling(oldNodeObject);
                //NodeObject newNodeObject = new NodeObject
                //{
                //    Siblings = oldNodeObject.Siblings,
                //    CanEdit = oldNodeObject.CanEdit,
                //    FileEntry = fileEntry,
                //    Index = args.PackIndex,
                //    IsFolder = false
                //};

                //packNode.Tag = newNodeObject;
                //packNode.ForeColor = BaseColor;
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

            MessageBox.Show("File packing and idx/dat writing completed!", "Success", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }

        /// <summary>
        /// todo: finish me
        /// </summary>
        /// <param name="sender">The button clicked.</param>
        /// <param name="e">The Click event args.</param>
        private void _RevertRestoreButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Todo");
        }

        /// <summary>
        /// Changes the folder icon in the TreeView to reflect tree expansion status.
        /// </summary>
        /// <param name="sender">The TreeView clicked.</param>
        /// <param name="e">The After Expand event args.</param>
        private void _FilesTreeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            NodeObject nodeObject = (NodeObject)e.Node.Tag;
            if (!nodeObject.IsFolder) return;

            e.Node.ImageIndex = (int)IconIndex.FolderOpen;
            e.Node.SelectedImageIndex = e.Node.ImageIndex;
        }

        /// <summary>
        /// Changes the folder icon in the TreeView to reflect tree expansion status.
        /// </summary>
        /// <param name="sender">The TreeView clicked.</param>
        /// <param name="e">The After Expand event args.</param>
        private void _FilesTreeView_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            NodeObject nodeObject = (NodeObject)e.Node.Tag;
            if (!nodeObject.IsFolder) return;

            e.Node.ImageIndex = (int)IconIndex.Folder;
            e.Node.SelectedImageIndex = e.Node.ImageIndex;
        }

        /// <summary>
        /// Function to recursivly check the checked node children.
        /// </summary>
        /// <param name="sender">The TreeView clicked.</param>
        /// <param name="e">The After Check event args.</param>
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

        /// <summary>
        /// Event function to apply a filter to the TreeView for button "Apply"-filter.
        /// </summary>
        /// <param name="sender">The Button clicked.</param>
        /// <param name="e">The ButtonClick event args.</param>
        private void _FilterApplyButton_Click(object sender, EventArgs e)
        {
            String filterText = filter_textBox.Text;
            if (String.IsNullOrEmpty(filterText)) return;


            // if "reset"
            if (filterText == "*.*")
            {
                _ResetFilter();
                return;
            }


            _files_fileTreeView.BeginUpdate();
            // clone tree for filtering
            if (_clonedTreeView == null)
            {
                _clonedTreeView = new TreeView();
                foreach (TreeNode treeNode in _files_fileTreeView.Nodes)
                {
                    _clonedTreeView.Nodes.Add((TreeNode)treeNode.Clone());
                }
            }
            else // if not null, then we need to "reset" the current view to original
            {
                _files_fileTreeView.Nodes.Clear();
                foreach (TreeNode treeNode in _clonedTreeView.Nodes)
                {
                    _files_fileTreeView.Nodes.Add((TreeNode)treeNode.Clone());
                }
                _AssignColors(_files_fileTreeView.Nodes);
            }


            // apply filter
            int nodeCount = _files_fileTreeView.Nodes.Count;
            for (int i = 0; i < nodeCount; i++)
            {
                if (!_ApplyFilter(_files_fileTreeView.Nodes[i], filterText)) continue;

                i--;
                nodeCount--;
            }

            // some aesthetics
            foreach (TreeNode treeNode in _files_fileTreeView.Nodes)
            {
                if (treeNode.Index == 0)
                {
                    _files_fileTreeView.SelectedNode = treeNode;
                }

                treeNode.Expand();
            }
            _files_fileTreeView.EndUpdate();
        }

        /// <summary>
        /// Event Function for "Reset"-filter Button - Click.
        /// Calls ResetFilter() function to reset the TreeView filter.
        /// </summary>
        /// <param name="sender">The Button clicked.</param>
        /// <param name="e">The ButtonClick event args.</param>
        private void _FilterResetButton_Click(object sender, EventArgs e)
        {
            filter_textBox.Text = "*.*";
            _ResetFilter();
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

        /// <summary>
        /// Event Function for "Uncook" Button -  Click.
        /// Uncooks checked files to their respective HGL file system location.
        /// </summary>
        /// <param name="sender">The button clicked.</param>
        /// <param name="e">The Click event args.</param>
        private void _UncookButton_Click(object sender, EventArgs e)
        {
            // make sure we have at least 1 checked file
            List<TreeNode> checkedNodes = new List<TreeNode>();
            if (_GetCheckedNodes(_files_fileTreeView.Nodes, checkedNodes) == 0)
            {
                MessageBox.Show("No files checked for extraction!", "Need Files", MessageBoxButtons.OK,
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

                byte[] fileBytes = null;// _fileManager.GetFileBytes(nodeFullPath);
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
                MessageBox.Show("No files checked for extraction!", "Need Files", MessageBoxButtons.OK,
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
                XmlCookedFile cookedXmlFile = new XmlCookedFile();

                DialogResult dr = DialogResult.Retry;
                byte[] cookedBytes = null;
                while (dr == DialogResult.Retry && cookedBytes == null)
                {
                    try
                    {
                        xmlDocument.Load(filePath);
                        cookedBytes = cookedXmlFile.CookXmlDocument(xmlDocument);
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

        private void _QuickXmlButton_Click(object sender, EventArgs e)
        {
            // where do we want to save it
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog { SelectedPath = Config.HglDir };
            if (folderBrowserDialog.ShowDialog(this) != DialogResult.OK) return;

            ExtractPackPatchArgs extractPatchArgs = new ExtractPackPatchArgs
            {
                RootDir = folderBrowserDialog.SelectedPath
            };

            ProgressForm progressForm = new ProgressForm(_QuickXmlWorker, extractPatchArgs);
            progressForm.SetLoadingText("Extracting an Uncooking .xml.cooked files...");
            progressForm.Show(this);
        }

        private void _QuickXmlWorker(ProgressForm progressForm, Object param)
        {
            const int progressStepRate = 50;
            const String outputResultsName = "uncook_results.txt";
            ExtractPackPatchArgs extractPatchArgs = (ExtractPackPatchArgs)param;

            TextWriter consoleOut = Console.Out;
            TextWriter textWriter = new StreamWriter(outputResultsName);
            Console.SetOut(textWriter);
            Console.WriteLine("Results of most recent uncooking of .xml.cooked files. Please scroll to end for tallied results.");


            // get all .xml.cooked
            IEnumerable<IndexFile.FileEntry> xmlCookedFiles =
                _fileManager.FileEntries.Values.Where(fileEntry => fileEntry.FileNameString.EndsWith(XmlCookedFile.FileExtention));


            // loop through file entries
            int count = xmlCookedFiles.Count();
            progressForm.ConfigBar(1, count, progressStepRate);
            progressForm.SetCurrentItemText("Extracting and Uncooking .xml.cooked files... (" + count + ")");
            int i = 0;
            int uncooked = 0;
            int readFailed = 0;
            int uncookFailed = 0;
            int tcv4Warnings = 0;
            int excelWarnings = 0;
            foreach (IndexFile.FileEntry fileEntry in xmlCookedFiles)
            {
                // update progress
                if (i % progressStepRate == 0)
                {
                    progressForm.SetCurrentItemText(fileEntry.RelativeFullPathWithoutPatch);
                }
                i++;

                // get file and uncook
                Console.WriteLine(fileEntry.RelativeFullPathWithoutPatch);
                byte[] fileBytes;
                XmlCookedFile xmlCookedFile = new XmlCookedFile();

                try
                {
                    fileBytes = _fileManager.GetFileBytes(fileEntry);
                }
                catch (Exception)
                {
                    Console.WriteLine("Error: FileManager failed to read file!\n");
                    readFailed++;
                    continue;
                }

                try
                {
                    xmlCookedFile.Uncook(fileBytes);
                }
                catch (Exception)
                {
                    Console.WriteLine("Warning: Failed to uncook file: " + fileEntry.FileNameString + "\n");
                    uncookFailed++;
                    continue;
                }

                // did we have any uncooking issues?
                bool hadWarning = false;
                if (xmlCookedFile.HasTCv4Elements)
                {
                    Console.WriteLine("Warning: File has TCv4-specific elements.");
                    hadWarning = true;
                    tcv4Warnings++;
                }
                if (xmlCookedFile.HasExcelStringsMissing)
                {
                    Console.WriteLine("Warning: File has " + xmlCookedFile.ExcelStringsMissing.Count + " unknown excel strings: ");
                    foreach (String str in xmlCookedFile.ExcelStringsMissing) Console.WriteLine("\t- \"" + str + "\"");
                    hadWarning = true;
                    excelWarnings++;
                }

                // save file
                String savePath = Path.Combine(extractPatchArgs.RootDir, fileEntry.RelativeFullPathWithoutPatch);
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));

                try
                {
                    xmlCookedFile.SaveXml(savePath.Replace(".cooked", ""));
                }
                catch (Exception)
                {
                    Console.WriteLine("Warning: Failed to save XML file: " + savePath + "\n");
                    continue;
                }

                if (hadWarning) Console.WriteLine();
                uncooked++;
            }


            // output final results
            Console.WriteLine("\nXML Files Uncooked: " + uncooked);
            if (readFailed > 0) Console.WriteLine(readFailed + " file(s) could not be read from the data files.");
            if (uncookFailed > 0) Console.WriteLine(uncookFailed + " file(s) failed to uncook at all.");
            if (tcv4Warnings > 0) Console.WriteLine(tcv4Warnings + " file(s) had TCv4-specific XML elements which wont be included when recooked.");
            if (excelWarnings > 0) Console.WriteLine(excelWarnings + " file(s) had excel warnings and cannot be safely recooked.");
            textWriter.Close();
            Console.SetOut(consoleOut);

            try
            {
                Process process = new Process { StartInfo = { FileName = Config.TxtEditor, Arguments = outputResultsName } };
                process.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    "Failed to open results!\nThe " + outputResultsName + " can be found in your Reanimator folder.\n" +
                    e, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}