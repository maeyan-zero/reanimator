using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Reanimator.Forms;
using Hellgate;
using FileEntry = Hellgate.IndexFile.FileEntry;

namespace Reanimator
{
    public partial class TableForm : ThreadedFormBase, IDisposable, IMdiChildBase
    {
        
        Hellgate.IndexFile IndexFile;
        Hellgate.StringsFile StringsFileData;
        List<int> foundIndices;
        int currentSelection;
        public string FilePath { get { return (!(IndexFile == null)) ? IndexFile.FilePath : string.Empty; } }

        public bool IsIndexFile
        {
            get { return IndexFile == null ? false : true; }
        }

        public TableForm(Hellgate.IndexFile indexFile)
        {
            IndexFile = indexFile;
            Text += ": " + FilePath;
            TableFormInit();
            if (!(IndexFile.OpenDat(FileAccess.Read)))
            {
                string caption = "Warning";
                string message = "Unable to open accompanying data file: \n" + IndexFile.FileNameWithoutExtension + ".dat\nYou will be unable to extract any files.";
                MessageBox(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public TableForm(Hellgate.StringsFile stringsFile)
        {
            StringsFileData = stringsFile;
            TableFormInit();
        }

        private void TableFormInit()
        {
            InitializeComponent();
            dataGridView.CellContextMenuStripNeeded += dataGridView_CellContextMenuStripNeeded;
            dataGridView.RowHeadersVisible = false;
            dataGridView.AllowUserToOrderColumns = true;

            //Initialize the DataGridViewColumn control
            IndexFileCheckBoxColumn.DefaultCellStyle.DataSourceNullValue = false;
            IndexFileCheckBoxColumn.Frozen = false;
            IndexFileCheckBoxColumn.TrueValue = true;
            IndexFileCheckBoxColumn.FalseValue = false;
            IndexFileCheckBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            IndexFileCheckBoxColumn.Width = 24;
            IndexFileCheckBoxColumn.Name = "IndexFileCheckBoxColumn";
            
            foundIndices = new List<int>();
            currentSelection = 0;

            if (!(IndexFile == null))
                dataGridView.DataSource = IndexFile.Files.ToArray();

            if (!(StringsFileData == null))
                dataGridView.DataSource = StringsFileData.Rows.ToArray();
        }

        /// <summary>
        /// Returns a list of items that were checked in the DataGridView
        /// </summary>
        /// <returns>A list of all checked items</returns>
        public FileEntry[] GetCheckedFiles()
        {
            int counter = 0;
            //A list of checked files
            List<FileEntry> fileIndex = new List<FileEntry>();

            //Get the data bound to the DataGridView
            FileEntry[] index = (FileEntry[])this.dataGridView.DataSource;

            //Iterate through the Rows and check if the checkBoxes were checked 
            foreach (DataGridViewRow row in this.dataGridView.Rows)
            {
                //Get the checkBox of the current row
                DataGridViewCell cell = row.Cells["IndexFileCheckBoxColumn"];

                //As the checkBox.Value doesn't seem to get initialized even though the default value is set to "false",
                //check if the value is not null (right now they get initialized after being checked or unchecked once)
                if (cell.Value != null)
                {
                    //if the value is not null, it is a bool value. If it is checked, add the file to the new file list
                    if ((bool)cell.Value)
                    {
                        //adds the current row (FileIndex) to the new list (assues, that the original sequence was not
                        //modified/changed by sorting/rearranging)
                        fileIndex.Add(index[counter]);
                    }
                }
                counter++;
            }

            return fileIndex.ToArray();
        }

        public FileEntry[] GetSelectedFiles()
        {
            int counter = 0;
            List<FileEntry> fileIndex = new List<FileEntry>();
            FileEntry[] index = (FileEntry[])this.dataGridView.DataSource;

            foreach (DataGridViewRow row in this.dataGridView.Rows)
            {
                if (row.Selected)
                {
                    fileIndex.Add(index[counter]);
                }

                counter++;
            }

            return fileIndex.ToArray();
        }

        private void dataGridView_CellContextMenuStripNeeded(object sender, DataGridViewCellEventArgs e)
        {
            if (!this.IsIndexFile)
            {
                return;
            }
            if (!IndexFile.DatFileOpen)
            {
                return;
            }

            Point pt = dataGridView.PointToClient(MousePosition);
            DataGridView.HitTestInfo hti = dataGridView.HitTest(pt.X, pt.Y);

            if (hti.Type == DataGridViewHitTestType.Cell)
            {
                /*
                if (dataGridView.SelectedRows.Count == 1)
                {
                    Index.FileIndex[] index = (Index.FileIndex[])this.dataGridView.DataSource;
                    Index.FileIndex file = index[hti.RowIndex];
                    if (file.FilenameString.EndsWith("cooked"))
                    {
                        System.Windows.Forms.ToolStripMenuItem viewCookedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                        viewCookedToolStripMenuItem.Name = "viewCookedToolStripMenuItem";
                        viewCookedToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
                        viewCookedToolStripMenuItem.Text = "View as Text...";
                        viewCookedToolStripMenuItem.Click += new System.EventHandler(viewCookedToolStripMenuItem_Click);
                        contextMenuStrip1.Items.Add(viewCookedToolStripMenuItem);
                    }
                }
                */

                contextMenuStrip1.Show(MousePosition);
            }
        }

        private void viewCookedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
            Index.FileIndex file = GetSelectedFiles()[0];
            byte[] data = ReadDataFile(file);

            FileStream fileOut = new FileStream("blah.txt", FileMode.CreateNew);
             * //  ColorSets colorSets = new ColorSets(data);
             * */
        }

        private void extractSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExtractFiles(GetSelectedFiles());
        }

        private void extractCheckedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExtractFiles(GetCheckedFiles());
        }

        private void extractAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ExtractFiles(index.FileTable);
        }

        private void ExtractFiles(FileEntry[] files)
        {
            new ProgressForm(DoExtractFiles, files).ShowDialog(this);
        }

        private void DoExtractFiles(ProgressForm progressBar, Object param)
        {
           FileEntry[] files = param as FileEntry[];
            if (param == null)
            {
                return;
            }
            if (!IndexFile.DatFileOpen)
            {
                return;
            }

            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = IndexFile.FileDirectory;
            if (this.ShowDialog(folderBrowserDialog) != DialogResult.OK)
            {
                return;
            }

            String extractToPath = folderBrowserDialog.SelectedPath;
            bool keepPath = false;
            DialogResult dr = MessageBox("Keep directory structure?", "Path", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.Cancel)
            {
                return;
            }
            else if (dr == DialogResult.Yes)
            {
                keepPath = true;
                extractToPath += @"\" + IndexFile.FileNameWithoutExtension;
            }

            progressBar.ConfigBar(0, files.Length, 1);
            progressBar.SetLoadingText("Extracting files to... " + extractToPath);

            int filesSaved = 0;
            foreach (FileEntry file in files)
            {
                while (true)
                {
                    try
                    {
                        byte[] buffer = IndexFile.GetFileBytes(file);

                        string keepPathString = "\\";
                        if (keepPath)
                        {
                            keepPathString += file.DirectoryStringWithoutPatch;
                            Directory.CreateDirectory(extractToPath + keepPathString);
                        }

                        progressBar.SetCurrentItemText(file.FileNameString);
                        FileStream fileOut = new FileStream(extractToPath + keepPathString + file.FileNameString, FileMode.Create);
                        fileOut.Write(buffer, 0, buffer.Length);
                        fileOut.Close();
                        filesSaved++;
                        break;
                    }
                    catch (Exception e)
                    {
                        DialogResult failedDr = MessageBox("Failed to extract file from dat!\nFile: " + file.FileNameString + "\n\n" + e.ToString(), "Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                        if (failedDr == DialogResult.Ignore)
                        {
                            break;
                        }
                        else if (failedDr == DialogResult.Abort)
                        {
                            return;
                        }
                    }
                }
            }

            MessageBox(filesSaved + " file(s) saved!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dataGridView_DataSourceChanged(object sender, EventArgs e)
        {
            infoText_Label.Text = "Contains " + ((Array)dataGridView.DataSource).Length + " entries.";
        }

        #region debug help
        //When a new DataSource is loaded display some information


        //Just for debugging purposes... uncomment this section and the event for final use
        //private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        //{

        //  Index.FileIndex[] index = GetSelectedFiles();

        //  string files = string.Empty;
        //  for (int counter = 0; counter < index.Length; counter++)
        //  {
        //    files += index[counter].FilenameString + "\n";
        //  }

        //  MessageBox.Show("The following items were checked:\n\n" + files, "Checked files");
        //}
        #endregion

        #region IDisposable Members

        new public void Dispose()
        {
            if (IndexFile != null)
            {
                IndexFile.Dispose();
            }

            Dispose(true);
        }

        #endregion

        //is doing the job, but iterating over every single field is slow... needs improvement
        //DataView features REAL search functionality, but I couldn't figure out how to use it
        /// <summary>
        /// Marks all rows that contain the entered keyword
        /// </summary>
        /// <param name="sender">The control that fired the event</param>
        /// <param name="e">Parameters</param>
        private void Search_Click(object sender, EventArgs e)
        {
            foundIndices.Clear();
            int counter = 0;

            this.dataGridView.SuspendLayout();
            foreach (DataGridViewRow row in this.dataGridView.Rows)
            {
                row.Selected = false;

                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().ToLower().Contains(tb_searchString.Text.ToLower()))
                    {
                        foundIndices.Add(row.Index);
                        cell.Selected = true;
                        counter++;
                    }
                }
            }

            searchResults_Label.Text = counter + " matching entries found";

            if (foundIndices.Count > 0)
            {
                ScrollToPosition(foundIndices[0]);
            }

            this.dataGridView.ResumeLayout();
        }

        private void SelectAllEntries(bool selected)
        {
            if (selected)
            {
                dataGridView.SelectAll();
            }
            else
            {
                dataGridView.ClearSelection();
            }
        }

        private void CheckAllEntries(bool check)
        {
            dataGridView.SuspendLayout();
            foreach (DataGridViewRow row in this.dataGridView.Rows)
            {
                DataGridViewCell cell = row.Cells["IndexFileCheckBoxColumn"];
                cell.Value = check;
            }
            dataGridView.ResumeLayout();
        }

        private void CheckAll_Click(object sender, EventArgs e)
        {
            CheckAllEntries(true);
        }

        private void SelectAll_Click(object sender, EventArgs e)
        {
            SelectAllEntries(true);
        }

        private void UnCheckAll_Click(object sender, EventArgs e)
        {
            CheckAllEntries(false);
        }

        private void UnSelectAll_Click(object sender, EventArgs e)
        {
            SelectAllEntries(false);
        }

        public void SaveButton()
        {
            byte[] saveData = IndexFile.ToByteArray();
            Crypt.Encrypt(saveData);
            FileStream fOut = new FileStream(IndexFile.FileDirectory + IndexFile.FileNameWithoutExtension + ".new.idx", FileMode.Create);
            fOut.Write(saveData, 0, saveData.Length);
            fOut.Dispose();
        }

        private void b_prev_Click(object sender, EventArgs e)
        {
            MoveToNextFoundItem(-1);
        }

        private void b_next_Click(object sender, EventArgs e)
        {
            MoveToNextFoundItem(1);
        }

        private void MoveToNextFoundItem(int direction)
        {
            if (foundIndices.Count > 0)
            {
                currentSelection += direction;

                if (currentSelection < 0)
                {
                    currentSelection = 0;
                }
                else if (currentSelection > foundIndices.Count - 1)
                {
                    currentSelection = foundIndices.Count - 1;
                }

                ScrollToPosition(foundIndices[currentSelection]);
            }
        }

        private void ScrollToPosition(int position)
        {
            dataGridView.FirstDisplayedScrollingRowIndex = position;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsIndexFile) return;

            IndexFile.FileEntry[] filesIndex = dataGridView.DataSource as IndexFile.FileEntry[];
            DataGridViewSelectedRowCollection selectedRows = dataGridView.SelectedRows;

            foreach (DataGridViewRow row in selectedRows)
            {
                if (!IndexFile.PatchOutFile(row.Index))
                {
                    MessageBox("Failed to patch out file!");
                }
            }
        }

        private void replaceSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReplaceFiles(GetSelectedFiles());
        }

        private void replaceCheckedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReplaceFiles(GetCheckedFiles());
        }

        void ReplaceFiles(FileEntry[] files)
        {
            //if (!index.BeginDatWriting())
            //{
            //    MessageBox("Failed to open accompanying dat file!\n" + index.FileNameWithoutExtension, "Error",
            //                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            //foreach (Index.FileEntry file in files)
            //{
            //    OpenFileDialog fileDialog = new OpenFileDialog();
            //    fileDialog.ShowDialog();

            //    if (fileDialog.FileName == "") continue;

            //    using (FileStream buffer = new FileStream(fileDialog.FileName, FileMode.Open))
            //    {
            //        try
            //        {
            //            byte[] byteBuffer = new byte[buffer.Length];
            //            buffer.Read(byteBuffer, 0, (int)buffer.Length);
            //            index.AppendToDat(byteBuffer, true, file, true);
            //        }
            //        catch
            //        {
            //            Console.WriteLine("Caught a problem replacing a file.");
            //        }
            //    }
            //}
            //index.EndDatAccess();
        }
    }
}
