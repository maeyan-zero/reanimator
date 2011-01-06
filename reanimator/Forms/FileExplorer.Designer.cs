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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.details_tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.fileTime_textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.fileSize_textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.fileName_textBox = new System.Windows.Forms.TextBox();
            this.fileCompressed_textBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.loadingLocation_textBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
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
            this.contextMenuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.fileExplorer_tabPage.SuspendLayout();
            this.fileExplorer_splitContainer.Panel1.SuspendLayout();
            this.fileExplorer_splitContainer.Panel2.SuspendLayout();
            this.fileExplorer_splitContainer.SuspendLayout();
            this.legend_tableLayoutPanel.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.options_tableLayoutPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.details_tableLayoutPanel.SuspendLayout();
            this.advancedCommands_tabPage.SuspendLayout();
            this.extractUncook_groupBox.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.toolStripSeparator1,
            this.toolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(133, 54);
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
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(837, 675);
            this.tabControl1.TabIndex = 3;
            // 
            // fileExplorer_tabPage
            // 
            this.fileExplorer_tabPage.BackColor = System.Drawing.SystemColors.Menu;
            this.fileExplorer_tabPage.Controls.Add(this.fileExplorer_splitContainer);
            this.fileExplorer_tabPage.Location = new System.Drawing.Point(4, 22);
            this.fileExplorer_tabPage.Name = "fileExplorer_tabPage";
            this.fileExplorer_tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.fileExplorer_tabPage.Size = new System.Drawing.Size(829, 649);
            this.fileExplorer_tabPage.TabIndex = 0;
            this.fileExplorer_tabPage.Text = "File Explorer";
            // 
            // fileExplorer_splitContainer
            // 
            this.fileExplorer_splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fileExplorer_splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileExplorer_splitContainer.Location = new System.Drawing.Point(3, 3);
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
            this.fileExplorer_splitContainer.Panel2.Controls.Add(this.groupBox3);
            this.fileExplorer_splitContainer.Panel2.Controls.Add(this.groupBox2);
            this.fileExplorer_splitContainer.Panel2.Controls.Add(this.groupBox1);
            this.fileExplorer_splitContainer.Size = new System.Drawing.Size(823, 643);
            this.fileExplorer_splitContainer.SplitterDistance = 300;
            this.fileExplorer_splitContainer.TabIndex = 3;
            // 
            // filterReset_button
            // 
            this.filterReset_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.filterReset_button.Location = new System.Drawing.Point(222, 3);
            this.filterReset_button.Name = "filterReset_button";
            this.filterReset_button.Size = new System.Drawing.Size(50, 23);
            this.filterReset_button.TabIndex = 7;
            this.filterReset_button.Text = "Reset";
            this.filterReset_button.UseVisualStyleBackColor = true;
            this.filterReset_button.Click += new System.EventHandler(this._FilterResetButton_Click);
            // 
            // filterApply_button
            // 
            this.filterApply_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.filterApply_button.Location = new System.Drawing.Point(166, 3);
            this.filterApply_button.Name = "filterApply_button";
            this.filterApply_button.Size = new System.Drawing.Size(50, 23);
            this.filterApply_button.TabIndex = 6;
            this.filterApply_button.Text = "Apply";
            this.filterApply_button.UseVisualStyleBackColor = true;
            this.filterApply_button.Click += new System.EventHandler(this._FilterApplyButton_Click);
            // 
            // filter_textBox
            // 
            this.filter_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.filter_textBox.Location = new System.Drawing.Point(38, 5);
            this.filter_textBox.Name = "filter_textBox";
            this.filter_textBox.Size = new System.Drawing.Size(122, 20);
            this.filter_textBox.TabIndex = 5;
            this.filter_textBox.Text = "*.*";
            this.filter_textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this._Filter_TextBox_KeyDown);
            // 
            // filter_label
            // 
            this.filter_label.AutoSize = true;
            this.filter_label.Location = new System.Drawing.Point(3, 8);
            this.filter_label.Name = "filter_label";
            this.filter_label.Size = new System.Drawing.Size(29, 13);
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
            this.legend_tableLayoutPanel.Location = new System.Drawing.Point(3, 593);
            this.legend_tableLayoutPanel.Name = "legend_tableLayoutPanel";
            this.legend_tableLayoutPanel.RowCount = 2;
            this.legend_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.legend_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.legend_tableLayoutPanel.Size = new System.Drawing.Size(269, 45);
            this.legend_tableLayoutPanel.TabIndex = 3;
            // 
            // noEditorKey_label
            // 
            this.noEditorKey_label.AutoSize = true;
            this.noEditorKey_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.noEditorKey_label.Location = new System.Drawing.Point(3, 0);
            this.noEditorKey_label.Name = "noEditorKey_label";
            this.noEditorKey_label.Size = new System.Drawing.Size(263, 22);
            this.noEditorKey_label.TabIndex = 4;
            this.noEditorKey_label.Text = "File has no editor";
            this.noEditorKey_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // backupKey_label
            // 
            this.backupKey_label.AutoSize = true;
            this.backupKey_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backupKey_label.Location = new System.Drawing.Point(3, 22);
            this.backupKey_label.Name = "backupKey_label";
            this.backupKey_label.Size = new System.Drawing.Size(263, 23);
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
            this._files_fileTreeView.ContextMenuStrip = this.contextMenuStrip1;
            this._files_fileTreeView.Location = new System.Drawing.Point(3, 31);
            this._files_fileTreeView.Name = "_files_fileTreeView";
            this._files_fileTreeView.Size = new System.Drawing.Size(269, 556);
            this._files_fileTreeView.TabIndex = 1;
            this._files_fileTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this._FilesTreeView_AfterCheck);
            this._files_fileTreeView.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this._FilesTreeView_AfterCollapse);
            this._files_fileTreeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this._FilesTreeView_AfterExpand);
            this._files_fileTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._FilesTreeView_AfterSelect);
            this._files_fileTreeView.DoubleClick += new System.EventHandler(this._FilesTreeView_DoubleClick);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.tableLayoutPanel4);
            this.groupBox3.Location = new System.Drawing.Point(4, 489);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(474, 134);
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
            this.tableLayoutPanel4.Location = new System.Drawing.Point(5, 19);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(478, 110);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // uncook_button
            // 
            this.uncook_button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uncook_button.Location = new System.Drawing.Point(3, 3);
            this.uncook_button.Name = "uncook_button";
            this.uncook_button.Size = new System.Drawing.Size(141, 49);
            this.uncook_button.TabIndex = 0;
            this.uncook_button.Text = "Uncook";
            this.uncook_button.UseVisualStyleBackColor = true;
            this.uncook_button.Click += new System.EventHandler(this._UncookButton_Click);
            // 
            // cook_label
            // 
            this.cook_label.AutoSize = true;
            this.cook_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cook_label.Location = new System.Drawing.Point(150, 0);
            this.cook_label.Name = "cook_label";
            this.cook_label.Size = new System.Drawing.Size(325, 55);
            this.cook_label.TabIndex = 1;
            this.cook_label.Text = "Uncook checked file/folders that can be uncooked.\r\nThey will be placed in their a" +
                "pplicable \\data\\ location.";
            this.cook_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cook_button
            // 
            this.cook_button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cook_button.Location = new System.Drawing.Point(3, 58);
            this.cook_button.Name = "cook_button";
            this.cook_button.Size = new System.Drawing.Size(141, 49);
            this.cook_button.TabIndex = 2;
            this.cook_button.Text = "Cook";
            this.cook_button.UseVisualStyleBackColor = true;
            this.cook_button.Click += new System.EventHandler(this._CookButton_Click);
            // 
            // uncook_label
            // 
            this.uncook_label.AutoSize = true;
            this.uncook_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uncook_label.Location = new System.Drawing.Point(150, 55);
            this.uncook_label.Name = "uncook_label";
            this.uncook_label.Size = new System.Drawing.Size(325, 55);
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
            this.groupBox2.Location = new System.Drawing.Point(3, 157);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(474, 326);
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
            this.options_tableLayoutPanel.Location = new System.Drawing.Point(6, 19);
            this.options_tableLayoutPanel.Name = "options_tableLayoutPanel";
            this.options_tableLayoutPanel.RowCount = 4;
            this.options_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.options_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.options_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.options_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.options_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.options_tableLayoutPanel.Size = new System.Drawing.Size(477, 301);
            this.options_tableLayoutPanel.TabIndex = 1;
            // 
            // revertFile_label
            // 
            this.revertFile_label.AutoSize = true;
            this.revertFile_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.revertFile_label.Location = new System.Drawing.Point(150, 225);
            this.revertFile_label.Name = "revertFile_label";
            this.revertFile_label.Size = new System.Drawing.Size(324, 76);
            this.revertFile_label.TabIndex = 9;
            this.revertFile_label.Text = "Re-Patch necessary index files to have the game to load original unmodified check" +
                "ed files/folders.";
            this.revertFile_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // extractPatch_label
            // 
            this.extractPatch_label.AutoSize = true;
            this.extractPatch_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extractPatch_label.Location = new System.Drawing.Point(150, 75);
            this.extractPatch_label.Name = "extractPatch_label";
            this.extractPatch_label.Size = new System.Drawing.Size(324, 75);
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
            this.extract_label.Location = new System.Drawing.Point(150, 0);
            this.extract_label.Name = "extract_label";
            this.extract_label.Size = new System.Drawing.Size(324, 75);
            this.extract_label.TabIndex = 4;
            this.extract_label.Text = "Extract checked files/folders to a selected location.";
            this.extract_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // extract_button
            // 
            this.extract_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.extract_button.Location = new System.Drawing.Point(3, 3);
            this.extract_button.Name = "extract_button";
            this.extract_button.Size = new System.Drawing.Size(141, 69);
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
            this.extractPatch_button.Location = new System.Drawing.Point(3, 78);
            this.extractPatch_button.Name = "extractPatch_button";
            this.extractPatch_button.Size = new System.Drawing.Size(141, 69);
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
            this.packPatch_button.Location = new System.Drawing.Point(3, 153);
            this.packPatch_button.Name = "packPatch_button";
            this.packPatch_button.Size = new System.Drawing.Size(141, 69);
            this.packPatch_button.TabIndex = 6;
            this.packPatch_button.Text = "Pack and Patch Custom Dat";
            this.packPatch_button.UseVisualStyleBackColor = true;
            // 
            // packPatch_label
            // 
            this.packPatch_label.AutoSize = true;
            this.packPatch_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.packPatch_label.Location = new System.Drawing.Point(150, 150);
            this.packPatch_label.Name = "packPatch_label";
            this.packPatch_label.Size = new System.Drawing.Size(324, 75);
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
            this.revertFile_button.Location = new System.Drawing.Point(3, 228);
            this.revertFile_button.Name = "revertFile_button";
            this.revertFile_button.Size = new System.Drawing.Size(141, 70);
            this.revertFile_button.TabIndex = 8;
            this.revertFile_button.Text = "Revert and Restore";
            this.revertFile_button.UseVisualStyleBackColor = true;
            this.revertFile_button.Click += new System.EventHandler(this._RevertRestoreButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.details_tableLayoutPanel);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(475, 148);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Details";
            // 
            // details_tableLayoutPanel
            // 
            this.details_tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.details_tableLayoutPanel.ColumnCount = 2;
            this.details_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.90045F));
            this.details_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.09955F));
            this.details_tableLayoutPanel.Controls.Add(this.fileTime_textBox, 1, 4);
            this.details_tableLayoutPanel.Controls.Add(this.label3, 0, 2);
            this.details_tableLayoutPanel.Controls.Add(this.fileSize_textBox, 1, 1);
            this.details_tableLayoutPanel.Controls.Add(this.label2, 0, 1);
            this.details_tableLayoutPanel.Controls.Add(this.label1, 0, 0);
            this.details_tableLayoutPanel.Controls.Add(this.fileName_textBox, 1, 0);
            this.details_tableLayoutPanel.Controls.Add(this.fileCompressed_textBox, 1, 2);
            this.details_tableLayoutPanel.Controls.Add(this.label4, 0, 3);
            this.details_tableLayoutPanel.Controls.Add(this.loadingLocation_textBox, 1, 3);
            this.details_tableLayoutPanel.Controls.Add(this.label8, 0, 4);
            this.details_tableLayoutPanel.Location = new System.Drawing.Point(7, 20);
            this.details_tableLayoutPanel.Name = "details_tableLayoutPanel";
            this.details_tableLayoutPanel.RowCount = 5;
            this.details_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.details_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.details_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.details_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.details_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.details_tableLayoutPanel.Size = new System.Drawing.Size(477, 122);
            this.details_tableLayoutPanel.TabIndex = 0;
            // 
            // fileTime_textBox
            // 
            this.fileTime_textBox.Location = new System.Drawing.Point(155, 99);
            this.fileTime_textBox.Name = "fileTime_textBox";
            this.fileTime_textBox.ReadOnly = true;
            this.fileTime_textBox.Size = new System.Drawing.Size(200, 20);
            this.fileTime_textBox.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(146, 24);
            this.label3.TabIndex = 8;
            this.label3.Text = "Compressed Size (bytes)";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // fileSize_textBox
            // 
            this.fileSize_textBox.Location = new System.Drawing.Point(155, 27);
            this.fileSize_textBox.Name = "fileSize_textBox";
            this.fileSize_textBox.ReadOnly = true;
            this.fileSize_textBox.Size = new System.Drawing.Size(100, 20);
            this.fileSize_textBox.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(146, 24);
            this.label2.TabIndex = 4;
            this.label2.Text = "Size (bytes)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "File Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // fileName_textBox
            // 
            this.fileName_textBox.Location = new System.Drawing.Point(155, 3);
            this.fileName_textBox.Name = "fileName_textBox";
            this.fileName_textBox.Size = new System.Drawing.Size(278, 20);
            this.fileName_textBox.TabIndex = 2;
            // 
            // fileCompressed_textBox
            // 
            this.fileCompressed_textBox.Location = new System.Drawing.Point(155, 51);
            this.fileCompressed_textBox.Name = "fileCompressed_textBox";
            this.fileCompressed_textBox.ReadOnly = true;
            this.fileCompressed_textBox.Size = new System.Drawing.Size(100, 20);
            this.fileCompressed_textBox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(146, 24);
            this.label4.TabIndex = 9;
            this.label4.Text = "Loading Location";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // loadingLocation_textBox
            // 
            this.loadingLocation_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.loadingLocation_textBox.Location = new System.Drawing.Point(155, 75);
            this.loadingLocation_textBox.Name = "loadingLocation_textBox";
            this.loadingLocation_textBox.ReadOnly = true;
            this.loadingLocation_textBox.Size = new System.Drawing.Size(319, 20);
            this.loadingLocation_textBox.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(3, 96);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(146, 26);
            this.label8.TabIndex = 11;
            this.label8.Text = "Index Table Time";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // advancedCommands_tabPage
            // 
            this.advancedCommands_tabPage.BackColor = System.Drawing.SystemColors.Menu;
            this.advancedCommands_tabPage.Controls.Add(this.button1);
            this.advancedCommands_tabPage.Controls.Add(this.extractUncook_groupBox);
            this.advancedCommands_tabPage.Location = new System.Drawing.Point(4, 22);
            this.advancedCommands_tabPage.Name = "advancedCommands_tabPage";
            this.advancedCommands_tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.advancedCommands_tabPage.Size = new System.Drawing.Size(829, 649);
            this.advancedCommands_tabPage.TabIndex = 1;
            this.advancedCommands_tabPage.Text = "Advanced Commands";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(11, 465);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(125, 54);
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
            this.extractUncook_groupBox.Location = new System.Drawing.Point(8, 6);
            this.extractUncook_groupBox.Name = "extractUncook_groupBox";
            this.extractUncook_groupBox.Size = new System.Drawing.Size(813, 168);
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
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(807, 149);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // quickXml_button
            // 
            this.quickXml_button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quickXml_button.Location = new System.Drawing.Point(3, 3);
            this.quickXml_button.Name = "quickXml_button";
            this.quickXml_button.Size = new System.Drawing.Size(88, 68);
            this.quickXml_button.TabIndex = 0;
            this.quickXml_button.Text = "XML Files";
            this.quickXml_button.UseVisualStyleBackColor = true;
            this.quickXml_button.Click += new System.EventHandler(this._QuickXmlButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(97, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(707, 74);
            this.label5.TabIndex = 1;
            this.label5.Text = "Will uncook all .xml.cooked files from entire HGL filesystem to a user specified " +
                "location.\r\n(requires ~800 MB free space)";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _quickExcel_button
            // 
            this._quickExcel_button.Dock = System.Windows.Forms.DockStyle.Fill;
            this._quickExcel_button.Location = new System.Drawing.Point(3, 77);
            this._quickExcel_button.Name = "_quickExcel_button";
            this._quickExcel_button.Size = new System.Drawing.Size(88, 69);
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
            this.tableLayoutPanel2.Location = new System.Drawing.Point(97, 77);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(707, 69);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.94294F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.05706F));
            this.tableLayoutPanel3.Controls.Add(this._quickExcelTCv4_checkBox, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this._quickExcelBrowse_button, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this._quickExcelDir_textBox, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 37);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(701, 29);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // _quickExcelTCv4_checkBox
            // 
            this._quickExcelTCv4_checkBox.AutoSize = true;
            this._quickExcelTCv4_checkBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._quickExcelTCv4_checkBox.Location = new System.Drawing.Point(335, 3);
            this._quickExcelTCv4_checkBox.Name = "_quickExcelTCv4_checkBox";
            this._quickExcelTCv4_checkBox.Size = new System.Drawing.Size(363, 23);
            this._quickExcelTCv4_checkBox.TabIndex = 7;
            this._quickExcelTCv4_checkBox.Text = "Include TCv4 Files (requires TCv4 loading option enabled)";
            this._quickExcelTCv4_checkBox.UseVisualStyleBackColor = true;
            // 
            // _quickExcelBrowse_button
            // 
            this._quickExcelBrowse_button.Location = new System.Drawing.Point(247, 3);
            this._quickExcelBrowse_button.Name = "_quickExcelBrowse_button";
            this._quickExcelBrowse_button.Size = new System.Drawing.Size(75, 23);
            this._quickExcelBrowse_button.TabIndex = 9;
            this._quickExcelBrowse_button.Text = "Browse";
            this._quickExcelBrowse_button.UseVisualStyleBackColor = true;
            this._quickExcelBrowse_button.Click += new System.EventHandler(this._QuickExcelBrowse_Button_Click);
            // 
            // _quickExcelDir_textBox
            // 
            this._quickExcelDir_textBox.Location = new System.Drawing.Point(3, 3);
            this._quickExcelDir_textBox.Name = "_quickExcelDir_textBox";
            this._quickExcelDir_textBox.Size = new System.Drawing.Size(238, 20);
            this._quickExcelDir_textBox.TabIndex = 0;
            this._quickExcelDir_textBox.Text = "C:\\test_mod";
            // 
            // _quckExcel_label
            // 
            this._quckExcel_label.AutoSize = true;
            this._quckExcel_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this._quckExcel_label.Location = new System.Drawing.Point(3, 0);
            this._quckExcel_label.Name = "_quckExcel_label";
            this._quckExcel_label.Size = new System.Drawing.Size(701, 34);
            this._quckExcel_label.TabIndex = 1;
            this._quckExcel_label.Text = "Uncook all excel files to the specified location, maintinaing their directory str" +
                "ucture.\r\nIf applicable, the TCv4 files will be placed in an initial directory /T" +
                "Cv4/";
            this._quckExcel_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FileExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(837, 675);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.DoubleBuffered = true;
            this.Name = "FileExplorer";
            this.Text = "Hellgate File Explorer";
            this.Shown += new System.EventHandler(this.FileExplorer_Shown);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.fileExplorer_tabPage.ResumeLayout(false);
            this.fileExplorer_splitContainer.Panel1.ResumeLayout(false);
            this.fileExplorer_splitContainer.Panel1.PerformLayout();
            this.fileExplorer_splitContainer.Panel2.ResumeLayout(false);
            this.fileExplorer_splitContainer.ResumeLayout(false);
            this.legend_tableLayoutPanel.ResumeLayout(false);
            this.legend_tableLayoutPanel.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.options_tableLayoutPanel.ResumeLayout(false);
            this.options_tableLayoutPanel.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.details_tableLayoutPanel.ResumeLayout(false);
            this.details_tableLayoutPanel.PerformLayout();
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

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
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
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel details_tableLayoutPanel;
        private System.Windows.Forms.TextBox fileTime_textBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox fileSize_textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox fileName_textBox;
        private System.Windows.Forms.TextBox fileCompressed_textBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox loadingLocation_textBox;
        private System.Windows.Forms.Label label8;
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

    }
}