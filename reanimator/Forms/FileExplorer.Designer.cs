namespace Reanimator.Forms
{
    partial class FileExplorer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._fileEntry_contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.fileExplorer_tabPage = new System.Windows.Forms.TabPage();
            this.fileExplorer_splitContainer = new System.Windows.Forms.SplitContainer();
            this.filterReset_button = new System.Windows.Forms.Button();
            this.filterApply_button = new System.Windows.Forms.Button();
            this.filter_textBox = new System.Windows.Forms.TextBox();
            this.filter_label = new System.Windows.Forms.Label();
            this.legend_tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.noEditorKey_label = new System.Windows.Forms.Label();
            this.backupKey_label = new System.Windows.Forms.Label();
            this._files_fileTreeView = new System.Windows.Forms.TreeView();
            this._selectedFile_groupBox = new System.Windows.Forms.GroupBox();
            this._files_listView = new System.Windows.Forms.ListView();
            this._fileName_columnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._size_columnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._compressed_columnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._date_columnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._location_columnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._fileActions_groupBox = new System.Windows.Forms.GroupBox();
            this._indexFiles_label = new System.Windows.Forms.Label();
            this._indexFiles_listBox = new System.Windows.Forms.ListBox();
            this._startProcess_button = new System.Windows.Forms.Button();
            this._patchNote_label = new System.Windows.Forms.Label();
            this._fileActionsBrowse_button = new System.Windows.Forms.Button();
            this._fileActionsPath_textBox = new System.Windows.Forms.TextBox();
            this._fileActionsExtract_checkBox = new System.Windows.Forms.CheckBox();
            this._fileActionsPatch_checkBox = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.uncook_button = new System.Windows.Forms.Button();
            this.cook_label = new System.Windows.Forms.Label();
            this.cook_button = new System.Windows.Forms.Button();
            this.uncook_label = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.options_tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.revertFile_label = new System.Windows.Forms.Label();
            this.extractPatch_label = new System.Windows.Forms.Label();
            this.extract_label = new System.Windows.Forms.Label();
            this.extract_button = new System.Windows.Forms.Button();
            this.extractPatch_button = new System.Windows.Forms.Button();
            this.packPatch_button = new System.Windows.Forms.Button();
            this.packPatch_label = new System.Windows.Forms.Label();
            this.revertFile_button = new System.Windows.Forms.Button();
            this.advancedCommands_tabPage = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.extractUncook_groupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.quickXml_button = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this._quickExcel_button = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this._quickExcelTCv4_checkBox = new System.Windows.Forms.CheckBox();
            this._quickExcelBrowse_button = new System.Windows.Forms.Button();
            this._quickExcelDir_textBox = new System.Windows.Forms.TextBox();
            this._quckExcel_label = new System.Windows.Forms.Label();
            this._fileEntry_contextMenu.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.fileExplorer_tabPage.SuspendLayout();
            this.fileExplorer_splitContainer.Panel1.SuspendLayout();
            this.fileExplorer_splitContainer.Panel2.SuspendLayout();
            this.fileExplorer_splitContainer.SuspendLayout();
            this.legend_tableLayoutPanel.SuspendLayout();
            this._selectedFile_groupBox.SuspendLayout();
            this._fileActions_groupBox.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.options_tableLayoutPanel.SuspendLayout();
            this.advancedCommands_tabPage.SuspendLayout();
            this.extractUncook_groupBox.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // _fileEntry_contextMenu
            // 
            this._fileEntry_contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.toolStripSeparator1,
            this.toolStripMenuItem1});
            this._fileEntry_contextMenu.Name = "contextMenuStrip1";
            this._fileEntry_contextMenu.Size = new System.Drawing.Size(133, 54);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(129, 6);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(132, 22);
            this.toolStripMenuItem1.Text = "Extract to...";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.fileExplorer_tabPage);
            this.tabControl1.Controls.Add(this.advancedCommands_tabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(872, 768);
            this.tabControl1.TabIndex = 3;
            // 
            // fileExplorer_tabPage
            // 
            this.fileExplorer_tabPage.BackColor = System.Drawing.SystemColors.Menu;
            this.fileExplorer_tabPage.Controls.Add(this.fileExplorer_splitContainer);
            this.fileExplorer_tabPage.Location = new System.Drawing.Point(4, 24);
            this.fileExplorer_tabPage.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.fileExplorer_tabPage.Name = "fileExplorer_tabPage";
            this.fileExplorer_tabPage.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.fileExplorer_tabPage.Size = new System.Drawing.Size(864, 740);
            this.fileExplorer_tabPage.TabIndex = 0;
            this.fileExplorer_tabPage.Text = "File Explorer";
            // 
            // fileExplorer_splitContainer
            // 
            this.fileExplorer_splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fileExplorer_splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileExplorer_splitContainer.Location = new System.Drawing.Point(2, 3);
            this.fileExplorer_splitContainer.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.fileExplorer_splitContainer.Name = "fileExplorer_splitContainer";
            // 
            // fileExplorer_splitContainer.Panel1
            // 
            this.fileExplorer_splitContainer.Panel1.Controls.Add(this.filterReset_button);
            this.fileExplorer_splitContainer.Panel1.Controls.Add(this.filterApply_button);
            this.fileExplorer_splitContainer.Panel1.Controls.Add(this.filter_textBox);
            this.fileExplorer_splitContainer.Panel1.Controls.Add(this.filter_label);
            this.fileExplorer_splitContainer.Panel1.Controls.Add(this.legend_tableLayoutPanel);
            this.fileExplorer_splitContainer.Panel1.Controls.Add(this._files_fileTreeView);
            // 
            // fileExplorer_splitContainer.Panel2
            // 
            this.fileExplorer_splitContainer.Panel2.Controls.Add(this._selectedFile_groupBox);
            this.fileExplorer_splitContainer.Panel2.Controls.Add(this._fileActions_groupBox);
            this.fileExplorer_splitContainer.Panel2.Controls.Add(this.groupBox3);
            this.fileExplorer_splitContainer.Panel2.Controls.Add(this.groupBox2);
            this.fileExplorer_splitContainer.Size = new System.Drawing.Size(860, 734);
            this.fileExplorer_splitContainer.SplitterDistance = 313;
            this.fileExplorer_splitContainer.SplitterWidth = 5;
            this.fileExplorer_splitContainer.TabIndex = 3;
            // 
            // filterReset_button
            // 
            this.filterReset_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.filterReset_button.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filterReset_button.Location = new System.Drawing.Point(251, 3);
            this.filterReset_button.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.filterReset_button.Name = "filterReset_button";
            this.filterReset_button.Size = new System.Drawing.Size(58, 27);
            this.filterReset_button.TabIndex = 7;
            this.filterReset_button.Text = "Reset";
            this.filterReset_button.UseVisualStyleBackColor = true;
            this.filterReset_button.Click += new System.EventHandler(this._FilterResetButton_Click);
            // 
            // filterApply_button
            // 
            this.filterApply_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.filterApply_button.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filterApply_button.Location = new System.Drawing.Point(189, 3);
            this.filterApply_button.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.filterApply_button.Name = "filterApply_button";
            this.filterApply_button.Size = new System.Drawing.Size(58, 27);
            this.filterApply_button.TabIndex = 6;
            this.filterApply_button.Text = "Apply";
            this.filterApply_button.UseVisualStyleBackColor = true;
            this.filterApply_button.Click += new System.EventHandler(this._FilterApplyButton_Click);
            // 
            // filter_textBox
            // 
            this.filter_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.filter_textBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filter_textBox.Location = new System.Drawing.Point(44, 6);
            this.filter_textBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.filter_textBox.Name = "filter_textBox";
            this.filter_textBox.Size = new System.Drawing.Size(141, 23);
            this.filter_textBox.TabIndex = 5;
            this.filter_textBox.Text = "*.*";
            this.filter_textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this._Filter_TextBox_KeyDown);
            // 
            // filter_label
            // 
            this.filter_label.AutoSize = true;
            this.filter_label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filter_label.Location = new System.Drawing.Point(2, 9);
            this.filter_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.filter_label.Name = "filter_label";
            this.filter_label.Size = new System.Drawing.Size(33, 15);
            this.filter_label.TabIndex = 4;
            this.filter_label.Text = "Filter";
            // 
            // legend_tableLayoutPanel
            // 
            this.legend_tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.legend_tableLayoutPanel.ColumnCount = 1;
            this.legend_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.legend_tableLayoutPanel.Controls.Add(this.noEditorKey_label, 0, 0);
            this.legend_tableLayoutPanel.Controls.Add(this.backupKey_label, 0, 1);
            this.legend_tableLayoutPanel.Location = new System.Drawing.Point(2, 676);
            this.legend_tableLayoutPanel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.legend_tableLayoutPanel.Name = "legend_tableLayoutPanel";
            this.legend_tableLayoutPanel.RowCount = 2;
            this.legend_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.legend_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.legend_tableLayoutPanel.Size = new System.Drawing.Size(304, 52);
            this.legend_tableLayoutPanel.TabIndex = 3;
            // 
            // noEditorKey_label
            // 
            this.noEditorKey_label.AutoSize = true;
            this.noEditorKey_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.noEditorKey_label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noEditorKey_label.Location = new System.Drawing.Point(2, 0);
            this.noEditorKey_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.noEditorKey_label.Name = "noEditorKey_label";
            this.noEditorKey_label.Size = new System.Drawing.Size(300, 26);
            this.noEditorKey_label.TabIndex = 4;
            this.noEditorKey_label.Text = "File has no editor";
            this.noEditorKey_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // backupKey_label
            // 
            this.backupKey_label.AutoSize = true;
            this.backupKey_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backupKey_label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.backupKey_label.Location = new System.Drawing.Point(2, 26);
            this.backupKey_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.backupKey_label.Name = "backupKey_label";
            this.backupKey_label.Size = new System.Drawing.Size(300, 26);
            this.backupKey_label.TabIndex = 0;
            this.backupKey_label.Text = "File is patched out";
            this.backupKey_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _files_fileTreeView
            // 
            this._files_fileTreeView.AllowDrop = true;
            this._files_fileTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._files_fileTreeView.CheckBoxes = true;
            this._files_fileTreeView.ContextMenuStrip = this._fileEntry_contextMenu;
            this._files_fileTreeView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._files_fileTreeView.Location = new System.Drawing.Point(2, 36);
            this._files_fileTreeView.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._files_fileTreeView.Name = "_files_fileTreeView";
            this._files_fileTreeView.Size = new System.Drawing.Size(303, 637);
            this._files_fileTreeView.TabIndex = 1;
            this._files_fileTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this._FilesTreeView_AfterCheck);
            this._files_fileTreeView.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this._FilesTreeView_AfterCollapse);
            this._files_fileTreeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this._FilesTreeView_AfterExpand);
            this._files_fileTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._FilesTreeView_AfterSelect);
            this._files_fileTreeView.DoubleClick += new System.EventHandler(this._FilesTreeView_DoubleClick);
            // 
            // _selectedFile_groupBox
            // 
            this._selectedFile_groupBox.Controls.Add(this._files_listView);
            this._selectedFile_groupBox.Location = new System.Drawing.Point(4, 3);
            this._selectedFile_groupBox.Name = "_selectedFile_groupBox";
            this._selectedFile_groupBox.Size = new System.Drawing.Size(531, 157);
            this._selectedFile_groupBox.TabIndex = 6;
            this._selectedFile_groupBox.TabStop = false;
            this._selectedFile_groupBox.Text = "Selected File Details (Right Click for Options (TODO))";
            // 
            // _files_listView
            // 
            this._files_listView.AllowColumnReorder = true;
            this._files_listView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._files_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._fileName_columnHeader,
            this._size_columnHeader,
            this._compressed_columnHeader,
            this._date_columnHeader,
            this._location_columnHeader});
            this._files_listView.ContextMenuStrip = this._fileEntry_contextMenu;
            this._files_listView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._files_listView.FullRowSelect = true;
            this._files_listView.GridLines = true;
            this._files_listView.Location = new System.Drawing.Point(5, 22);
            this._files_listView.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._files_listView.Name = "_files_listView";
            this._files_listView.Size = new System.Drawing.Size(521, 129);
            this._files_listView.TabIndex = 5;
            this._files_listView.UseCompatibleStateImageBehavior = false;
            this._files_listView.View = System.Windows.Forms.View.Details;
            // 
            // _fileName_columnHeader
            // 
            this._fileName_columnHeader.Text = "File";
            this._fileName_columnHeader.Width = 127;
            // 
            // _size_columnHeader
            // 
            this._size_columnHeader.Text = "Size";
            this._size_columnHeader.Width = 44;
            // 
            // _compressed_columnHeader
            // 
            this._compressed_columnHeader.Text = "Compressed";
            this._compressed_columnHeader.Width = 61;
            // 
            // _date_columnHeader
            // 
            this._date_columnHeader.Text = "Date";
            this._date_columnHeader.Width = 126;
            // 
            // _location_columnHeader
            // 
            this._location_columnHeader.Text = "Location";
            this._location_columnHeader.Width = 132;
            // 
            // _fileActions_groupBox
            // 
            this._fileActions_groupBox.Controls.Add(this._indexFiles_label);
            this._fileActions_groupBox.Controls.Add(this._indexFiles_listBox);
            this._fileActions_groupBox.Controls.Add(this._startProcess_button);
            this._fileActions_groupBox.Controls.Add(this._patchNote_label);
            this._fileActions_groupBox.Controls.Add(this._fileActionsBrowse_button);
            this._fileActions_groupBox.Controls.Add(this._fileActionsPath_textBox);
            this._fileActions_groupBox.Controls.Add(this._fileActionsExtract_checkBox);
            this._fileActions_groupBox.Controls.Add(this._fileActionsPatch_checkBox);
            this._fileActions_groupBox.Location = new System.Drawing.Point(4, 166);
            this._fileActions_groupBox.Name = "_fileActions_groupBox";
            this._fileActions_groupBox.Size = new System.Drawing.Size(531, 289);
            this._fileActions_groupBox.TabIndex = 5;
            this._fileActions_groupBox.TabStop = false;
            this._fileActions_groupBox.Text = "Checked File Actions";
            // 
            // _indexFiles_label
            // 
            this._indexFiles_label.AutoSize = true;
            this._indexFiles_label.Location = new System.Drawing.Point(2, 147);
            this._indexFiles_label.Name = "_indexFiles_label";
            this._indexFiles_label.Size = new System.Drawing.Size(371, 15);
            this._indexFiles_label.TabIndex = 10;
            this._indexFiles_label.Text = "Using Selected Index Files (Note: Not Implemented - uses all for now)";
            // 
            // _indexFiles_listBox
            // 
            this._indexFiles_listBox.FormattingEnabled = true;
            this._indexFiles_listBox.ItemHeight = 15;
            this._indexFiles_listBox.Location = new System.Drawing.Point(7, 165);
            this._indexFiles_listBox.Name = "_indexFiles_listBox";
            this._indexFiles_listBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this._indexFiles_listBox.Size = new System.Drawing.Size(316, 109);
            this._indexFiles_listBox.TabIndex = 9;
            // 
            // _startProcess_button
            // 
            this._startProcess_button.Location = new System.Drawing.Point(329, 260);
            this._startProcess_button.Name = "_startProcess_button";
            this._startProcess_button.Size = new System.Drawing.Size(75, 23);
            this._startProcess_button.TabIndex = 8;
            this._startProcess_button.Text = "Start";
            this._startProcess_button.UseVisualStyleBackColor = true;
            this._startProcess_button.Click += new System.EventHandler(this._StartProcess_Button_Click);
            // 
            // _patchNote_label
            // 
            this._patchNote_label.AutoSize = true;
            this._patchNote_label.Location = new System.Drawing.Point(3, 113);
            this._patchNote_label.Name = "_patchNote_label";
            this._patchNote_label.Size = new System.Drawing.Size(392, 15);
            this._patchNote_label.TabIndex = 7;
            this._patchNote_label.Text = "Note: Non-patchable files (e.g. sounds) wont be patch out automatically.";
            // 
            // _fileActionsBrowse_button
            // 
            this._fileActionsBrowse_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._fileActionsBrowse_button.Location = new System.Drawing.Point(450, 49);
            this._fileActionsBrowse_button.Name = "_fileActionsBrowse_button";
            this._fileActionsBrowse_button.Size = new System.Drawing.Size(75, 23);
            this._fileActionsBrowse_button.TabIndex = 3;
            this._fileActionsBrowse_button.Text = "Browse";
            this._fileActionsBrowse_button.UseVisualStyleBackColor = true;
            this._fileActionsBrowse_button.Click += new System.EventHandler(this._FileActionsBrowse_Button_Click);
            // 
            // _fileActionsPath_textBox
            // 
            this._fileActionsPath_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._fileActionsPath_textBox.Location = new System.Drawing.Point(7, 49);
            this._fileActionsPath_textBox.Name = "_fileActionsPath_textBox";
            this._fileActionsPath_textBox.Size = new System.Drawing.Size(437, 23);
            this._fileActionsPath_textBox.TabIndex = 2;
            // 
            // _fileActionsExtract_checkBox
            // 
            this._fileActionsExtract_checkBox.AutoSize = true;
            this._fileActionsExtract_checkBox.Location = new System.Drawing.Point(7, 23);
            this._fileActionsExtract_checkBox.Name = "_fileActionsExtract_checkBox";
            this._fileActionsExtract_checkBox.Size = new System.Drawing.Size(61, 19);
            this._fileActionsExtract_checkBox.TabIndex = 1;
            this._fileActionsExtract_checkBox.Text = "Extract";
            this._fileActionsExtract_checkBox.UseVisualStyleBackColor = true;
            // 
            // _fileActionsPatch_checkBox
            // 
            this._fileActionsPatch_checkBox.AutoSize = true;
            this._fileActionsPatch_checkBox.Location = new System.Drawing.Point(6, 91);
            this._fileActionsPatch_checkBox.Name = "_fileActionsPatch_checkBox";
            this._fileActionsPatch_checkBox.Size = new System.Drawing.Size(317, 19);
            this._fileActionsPatch_checkBox.TabIndex = 0;
            this._fileActionsPatch_checkBox.Text = "Patch Files; forcing the game to use alternate locations.";
            this._fileActionsPatch_checkBox.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.tableLayoutPanel4);
            this.groupBox3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(5, 621);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox3.Size = new System.Drawing.Size(533, 98);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Cooking";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.93525F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 69.06475F));
            this.tableLayoutPanel4.Controls.Add(this.uncook_button, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.cook_label, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.cook_button, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.uncook_label, 1, 1);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(6, 22);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(538, 70);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // uncook_button
            // 
            this.uncook_button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uncook_button.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uncook_button.Location = new System.Drawing.Point(2, 3);
            this.uncook_button.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.uncook_button.Name = "uncook_button";
            this.uncook_button.Size = new System.Drawing.Size(162, 29);
            this.uncook_button.TabIndex = 0;
            this.uncook_button.Text = "Uncook";
            this.uncook_button.UseVisualStyleBackColor = true;
            this.uncook_button.Click += new System.EventHandler(this._UncookButton_Click);
            // 
            // cook_label
            // 
            this.cook_label.AutoSize = true;
            this.cook_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cook_label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cook_label.Location = new System.Drawing.Point(168, 0);
            this.cook_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.cook_label.Name = "cook_label";
            this.cook_label.Size = new System.Drawing.Size(368, 35);
            this.cook_label.TabIndex = 1;
            this.cook_label.Text = "Uncook checked file/folders that can be uncooked.\r\nThey will be placed in their a" +
                "pplicable \\data\\ location.";
            this.cook_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cook_button
            // 
            this.cook_button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cook_button.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cook_button.Location = new System.Drawing.Point(2, 38);
            this.cook_button.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cook_button.Name = "cook_button";
            this.cook_button.Size = new System.Drawing.Size(162, 29);
            this.cook_button.TabIndex = 2;
            this.cook_button.Text = "Cook";
            this.cook_button.UseVisualStyleBackColor = true;
            this.cook_button.Click += new System.EventHandler(this._CookButton_Click);
            // 
            // uncook_label
            // 
            this.uncook_label.AutoSize = true;
            this.uncook_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uncook_label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uncook_label.Location = new System.Drawing.Point(168, 35);
            this.uncook_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.uncook_label.Name = "uncook_label";
            this.uncook_label.Size = new System.Drawing.Size(368, 35);
            this.uncook_label.TabIndex = 3;
            this.uncook_label.Text = "Cook checked files/folders that can be cooked.\r\nWARNING: Cooked files will be pla" +
                "ced in \\data\\ location overwriting previous versions.\r\n(files in .dat will be un" +
                "touched)";
            this.uncook_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.options_tableLayoutPanel);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(2, 461);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox2.Size = new System.Drawing.Size(534, 154);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Options";
            // 
            // options_tableLayoutPanel
            // 
            this.options_tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.options_tableLayoutPanel.ColumnCount = 2;
            this.options_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.99548F));
            this.options_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 69.00452F));
            this.options_tableLayoutPanel.Controls.Add(this.revertFile_label, 1, 3);
            this.options_tableLayoutPanel.Controls.Add(this.extractPatch_label, 1, 1);
            this.options_tableLayoutPanel.Controls.Add(this.extract_label, 1, 0);
            this.options_tableLayoutPanel.Controls.Add(this.extract_button, 0, 0);
            this.options_tableLayoutPanel.Controls.Add(this.extractPatch_button, 0, 1);
            this.options_tableLayoutPanel.Controls.Add(this.packPatch_button, 0, 2);
            this.options_tableLayoutPanel.Controls.Add(this.packPatch_label, 1, 2);
            this.options_tableLayoutPanel.Controls.Add(this.revertFile_button, 0, 3);
            this.options_tableLayoutPanel.Location = new System.Drawing.Point(7, 22);
            this.options_tableLayoutPanel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.options_tableLayoutPanel.Name = "options_tableLayoutPanel";
            this.options_tableLayoutPanel.RowCount = 4;
            this.options_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.options_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.options_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.options_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.options_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.options_tableLayoutPanel.Size = new System.Drawing.Size(535, 125);
            this.options_tableLayoutPanel.TabIndex = 1;
            // 
            // revertFile_label
            // 
            this.revertFile_label.AutoSize = true;
            this.revertFile_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.revertFile_label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.revertFile_label.Location = new System.Drawing.Point(167, 93);
            this.revertFile_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.revertFile_label.Name = "revertFile_label";
            this.revertFile_label.Size = new System.Drawing.Size(366, 32);
            this.revertFile_label.TabIndex = 9;
            this.revertFile_label.Text = "Re-Patch necessary index files to have the game to load original unmodified check" +
                "ed files/folders.";
            this.revertFile_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // extractPatch_label
            // 
            this.extractPatch_label.AutoSize = true;
            this.extractPatch_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extractPatch_label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.extractPatch_label.Location = new System.Drawing.Point(167, 31);
            this.extractPatch_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.extractPatch_label.Name = "extractPatch_label";
            this.extractPatch_label.Size = new System.Drawing.Size(366, 31);
            this.extractPatch_label.TabIndex = 5;
            this.extractPatch_label.Text = "Extract checked files/folders to game data location, then patch necessary index f" +
                "iles forcing the game to load extracted files.\r\nNote: Non-patchable files (e.g. " +
                "sounds) wont be patch out automaticlly.";
            this.extractPatch_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // extract_label
            // 
            this.extract_label.AutoSize = true;
            this.extract_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extract_label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.extract_label.Location = new System.Drawing.Point(167, 0);
            this.extract_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.extract_label.Name = "extract_label";
            this.extract_label.Size = new System.Drawing.Size(366, 31);
            this.extract_label.TabIndex = 4;
            this.extract_label.Text = "Extract checked files/folders to a selected location.";
            this.extract_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // extract_button
            // 
            this.extract_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.extract_button.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.extract_button.Location = new System.Drawing.Point(2, 3);
            this.extract_button.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.extract_button.Name = "extract_button";
            this.extract_button.Size = new System.Drawing.Size(161, 25);
            this.extract_button.TabIndex = 2;
            this.extract_button.Text = "Extract to...";
            this.extract_button.UseVisualStyleBackColor = true;
            this.extract_button.Click += new System.EventHandler(this._ExtractButton_Click);
            // 
            // extractPatch_button
            // 
            this.extractPatch_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.extractPatch_button.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.extractPatch_button.Location = new System.Drawing.Point(2, 34);
            this.extractPatch_button.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.extractPatch_button.Name = "extractPatch_button";
            this.extractPatch_button.Size = new System.Drawing.Size(161, 25);
            this.extractPatch_button.TabIndex = 1;
            this.extractPatch_button.Text = "Extract and Patch Index";
            this.extractPatch_button.UseVisualStyleBackColor = true;
            this.extractPatch_button.Click += new System.EventHandler(this._ExtractPatchButton_Click);
            // 
            // packPatch_button
            // 
            this.packPatch_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.packPatch_button.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.packPatch_button.Location = new System.Drawing.Point(2, 65);
            this.packPatch_button.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.packPatch_button.Name = "packPatch_button";
            this.packPatch_button.Size = new System.Drawing.Size(161, 25);
            this.packPatch_button.TabIndex = 6;
            this.packPatch_button.Text = "Pack and Patch Custom Dat";
            this.packPatch_button.UseVisualStyleBackColor = true;
            // 
            // packPatch_label
            // 
            this.packPatch_label.AutoSize = true;
            this.packPatch_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.packPatch_label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.packPatch_label.Location = new System.Drawing.Point(167, 62);
            this.packPatch_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.packPatch_label.Name = "packPatch_label";
            this.packPatch_label.Size = new System.Drawing.Size(366, 31);
            this.packPatch_label.TabIndex = 7;
            this.packPatch_label.Text = "Pack checked files/folders into a custom dat/idx to have the game load the files " +
                "from an isolated .dat.";
            this.packPatch_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // revertFile_button
            // 
            this.revertFile_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.revertFile_button.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.revertFile_button.Location = new System.Drawing.Point(2, 96);
            this.revertFile_button.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.revertFile_button.Name = "revertFile_button";
            this.revertFile_button.Size = new System.Drawing.Size(161, 26);
            this.revertFile_button.TabIndex = 8;
            this.revertFile_button.Text = "Revert and Restore";
            this.revertFile_button.UseVisualStyleBackColor = true;
            this.revertFile_button.Click += new System.EventHandler(this._RevertRestoreButton_Click);
            // 
            // advancedCommands_tabPage
            // 
            this.advancedCommands_tabPage.BackColor = System.Drawing.SystemColors.Menu;
            this.advancedCommands_tabPage.Controls.Add(this.button1);
            this.advancedCommands_tabPage.Controls.Add(this.extractUncook_groupBox);
            this.advancedCommands_tabPage.Location = new System.Drawing.Point(4, 24);
            this.advancedCommands_tabPage.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.advancedCommands_tabPage.Name = "advancedCommands_tabPage";
            this.advancedCommands_tabPage.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.advancedCommands_tabPage.Size = new System.Drawing.Size(864, 740);
            this.advancedCommands_tabPage.TabIndex = 1;
            this.advancedCommands_tabPage.Text = "Advanced Commands";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 537);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(146, 62);
            this.button1.TabIndex = 1;
            this.button1.Text = "Patch out all files";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // extractUncook_groupBox
            // 
            this.extractUncook_groupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.extractUncook_groupBox.Controls.Add(this.tableLayoutPanel1);
            this.extractUncook_groupBox.Location = new System.Drawing.Point(9, 7);
            this.extractUncook_groupBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.extractUncook_groupBox.Name = "extractUncook_groupBox";
            this.extractUncook_groupBox.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.extractUncook_groupBox.Size = new System.Drawing.Size(844, 194);
            this.extractUncook_groupBox.TabIndex = 0;
            this.extractUncook_groupBox.TabStop = false;
            this.extractUncook_groupBox.Text = "Quick Extract && Uncook";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.77199F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 88.228F));
            this.tableLayoutPanel1.Controls.Add(this.quickXml_button, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label5, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this._quickExcel_button, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 19);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(840, 172);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // quickXml_button
            // 
            this.quickXml_button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quickXml_button.Location = new System.Drawing.Point(2, 3);
            this.quickXml_button.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.quickXml_button.Name = "quickXml_button";
            this.quickXml_button.Size = new System.Drawing.Size(94, 80);
            this.quickXml_button.TabIndex = 0;
            this.quickXml_button.Text = "XML Files";
            this.quickXml_button.UseVisualStyleBackColor = true;
            this.quickXml_button.Click += new System.EventHandler(this._QuickXmlButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(100, 0);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(738, 86);
            this.label5.TabIndex = 1;
            this.label5.Text = "Will uncook all .xml.cooked files from entire HGL filesystem to a user specified " +
                "location.\r\n(requires ~800 MB free space)";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _quickExcel_button
            // 
            this._quickExcel_button.Dock = System.Windows.Forms.DockStyle.Fill;
            this._quickExcel_button.Location = new System.Drawing.Point(2, 89);
            this._quickExcel_button.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._quickExcel_button.Name = "_quickExcel_button";
            this._quickExcel_button.Size = new System.Drawing.Size(94, 80);
            this._quickExcel_button.TabIndex = 2;
            this._quickExcel_button.Text = "Excel Files";
            this._quickExcel_button.UseVisualStyleBackColor = true;
            this._quickExcel_button.Click += new System.EventHandler(this._QuickExcel_Button_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this._quckExcel_label, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(100, 89);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(738, 80);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.94294F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 103F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.05706F));
            this.tableLayoutPanel3.Controls.Add(this._quickExcelTCv4_checkBox, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this._quickExcelBrowse_button, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this._quickExcelDir_textBox, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(2, 43);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(734, 34);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // _quickExcelTCv4_checkBox
            // 
            this._quickExcelTCv4_checkBox.AutoSize = true;
            this._quickExcelTCv4_checkBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._quickExcelTCv4_checkBox.Location = new System.Drawing.Point(357, 3);
            this._quickExcelTCv4_checkBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._quickExcelTCv4_checkBox.Name = "_quickExcelTCv4_checkBox";
            this._quickExcelTCv4_checkBox.Size = new System.Drawing.Size(375, 28);
            this._quickExcelTCv4_checkBox.TabIndex = 7;
            this._quickExcelTCv4_checkBox.Text = "Include TCv4 Files (requires TCv4 loading option enabled)";
            this._quickExcelTCv4_checkBox.UseVisualStyleBackColor = true;
            // 
            // _quickExcelBrowse_button
            // 
            this._quickExcelBrowse_button.Location = new System.Drawing.Point(254, 3);
            this._quickExcelBrowse_button.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._quickExcelBrowse_button.Name = "_quickExcelBrowse_button";
            this._quickExcelBrowse_button.Size = new System.Drawing.Size(86, 25);
            this._quickExcelBrowse_button.TabIndex = 9;
            this._quickExcelBrowse_button.Text = "Browse";
            this._quickExcelBrowse_button.UseVisualStyleBackColor = true;
            this._quickExcelBrowse_button.Click += new System.EventHandler(this._QuickExcelBrowse_Button_Click);
            // 
            // _quickExcelDir_textBox
            // 
            this._quickExcelDir_textBox.Location = new System.Drawing.Point(2, 3);
            this._quickExcelDir_textBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._quickExcelDir_textBox.Name = "_quickExcelDir_textBox";
            this._quickExcelDir_textBox.Size = new System.Drawing.Size(248, 23);
            this._quickExcelDir_textBox.TabIndex = 0;
            this._quickExcelDir_textBox.Text = "C:\\test_mod";
            // 
            // _quckExcel_label
            // 
            this._quckExcel_label.AutoSize = true;
            this._quckExcel_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this._quckExcel_label.Location = new System.Drawing.Point(2, 0);
            this._quckExcel_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this._quckExcel_label.Name = "_quckExcel_label";
            this._quckExcel_label.Size = new System.Drawing.Size(734, 40);
            this._quckExcel_label.TabIndex = 1;
            this._quckExcel_label.Text = "Uncook all excel files to the specified location, maintinaing their directory str" +
                "ucture.\r\nIf applicable, the TCv4 files will be placed in an initial directory /T" +
                "Cv4/";
            this._quckExcel_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FileExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 768);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "FileExplorer";
            this.Text = "Hellgate File Explorer";
            this.Shown += new System.EventHandler(this.FileExplorer_Shown);
            this._fileEntry_contextMenu.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.fileExplorer_tabPage.ResumeLayout(false);
            this.fileExplorer_splitContainer.Panel1.ResumeLayout(false);
            this.fileExplorer_splitContainer.Panel1.PerformLayout();
            this.fileExplorer_splitContainer.Panel2.ResumeLayout(false);
            this.fileExplorer_splitContainer.ResumeLayout(false);
            this.legend_tableLayoutPanel.ResumeLayout(false);
            this.legend_tableLayoutPanel.PerformLayout();
            this._selectedFile_groupBox.ResumeLayout(false);
            this._fileActions_groupBox.ResumeLayout(false);
            this._fileActions_groupBox.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.options_tableLayoutPanel.ResumeLayout(false);
            this.options_tableLayoutPanel.PerformLayout();
            this.advancedCommands_tabPage.ResumeLayout(false);
            this.extractUncook_groupBox.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip _fileEntry_contextMenu;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage fileExplorer_tabPage;
        private System.Windows.Forms.SplitContainer fileExplorer_splitContainer;
        private System.Windows.Forms.Button filterReset_button;
        private System.Windows.Forms.Button filterApply_button;
        private System.Windows.Forms.TextBox filter_textBox;
        private System.Windows.Forms.Label filter_label;
        private System.Windows.Forms.TableLayoutPanel legend_tableLayoutPanel;
        private System.Windows.Forms.Label noEditorKey_label;
        private System.Windows.Forms.Label backupKey_label;
        private System.Windows.Forms.TreeView _files_fileTreeView;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button uncook_button;
        private System.Windows.Forms.Label cook_label;
        private System.Windows.Forms.Button cook_button;
        private System.Windows.Forms.Label uncook_label;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel options_tableLayoutPanel;
        private System.Windows.Forms.Label revertFile_label;
        private System.Windows.Forms.Label extractPatch_label;
        private System.Windows.Forms.Label extract_label;
        private System.Windows.Forms.Button extract_button;
        private System.Windows.Forms.Button extractPatch_button;
        private System.Windows.Forms.Button packPatch_button;
        private System.Windows.Forms.Label packPatch_label;
        private System.Windows.Forms.Button revertFile_button;
        private System.Windows.Forms.TabPage advancedCommands_tabPage;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox extractUncook_groupBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button quickXml_button;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button _quickExcel_button;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.CheckBox _quickExcelTCv4_checkBox;
        private System.Windows.Forms.Button _quickExcelBrowse_button;
        private System.Windows.Forms.TextBox _quickExcelDir_textBox;
        private System.Windows.Forms.Label _quckExcel_label;
        private System.Windows.Forms.GroupBox _fileActions_groupBox;
        private System.Windows.Forms.GroupBox _selectedFile_groupBox;
        private System.Windows.Forms.ListView _files_listView;
        private System.Windows.Forms.ColumnHeader _fileName_columnHeader;
        private System.Windows.Forms.ColumnHeader _size_columnHeader;
        private System.Windows.Forms.ColumnHeader _compressed_columnHeader;
        private System.Windows.Forms.ColumnHeader _date_columnHeader;
        private System.Windows.Forms.ColumnHeader _location_columnHeader;
        private System.Windows.Forms.CheckBox _fileActionsPatch_checkBox;
        private System.Windows.Forms.Label _patchNote_label;
        private System.Windows.Forms.Button _fileActionsBrowse_button;
        private System.Windows.Forms.TextBox _fileActionsPath_textBox;
        private System.Windows.Forms.CheckBox _fileActionsExtract_checkBox;
        private System.Windows.Forms.Button _startProcess_button;
        private System.Windows.Forms.Label _indexFiles_label;
        private System.Windows.Forms.ListBox _indexFiles_listBox;

    }
}