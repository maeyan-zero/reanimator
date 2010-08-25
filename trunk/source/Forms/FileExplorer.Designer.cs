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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.noEditorKey_label = new System.Windows.Forms.Label();
            this.backupKey_label = new System.Windows.Forms.Label();
            this.files_treeView = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.extractPatch_label = new System.Windows.Forms.Label();
            this.extract_label = new System.Windows.Forms.Label();
            this.revertFile_button = new System.Windows.Forms.Button();
            this.revertFile_label = new System.Windows.Forms.Label();
            this.extract_button = new System.Windows.Forms.Button();
            this.extractPatch_button = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
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
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel3);
            this.splitContainer1.Panel1.Controls.Add(this.files_treeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(729, 653);
            this.splitContainer1.SplitterDistance = 260;
            this.splitContainer1.TabIndex = 2;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.noEditorKey_label, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.backupKey_label, 0, 1);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 601);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(250, 45);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // noEditorKey_label
            // 
            this.noEditorKey_label.AutoSize = true;
            this.noEditorKey_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.noEditorKey_label.Location = new System.Drawing.Point(3, 0);
            this.noEditorKey_label.Name = "noEditorKey_label";
            this.noEditorKey_label.Size = new System.Drawing.Size(244, 22);
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
            this.backupKey_label.Size = new System.Drawing.Size(244, 23);
            this.backupKey_label.TabIndex = 0;
            this.backupKey_label.Text = "File is backed up";
            this.backupKey_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // files_treeView
            // 
            this.files_treeView.AllowDrop = true;
            this.files_treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.files_treeView.CheckBoxes = true;
            this.files_treeView.ContextMenuStrip = this.contextMenuStrip1;
            this.files_treeView.Location = new System.Drawing.Point(3, 3);
            this.files_treeView.Name = "files_treeView";
            this.files_treeView.Size = new System.Drawing.Size(250, 592);
            this.files_treeView.TabIndex = 1;
            this.files_treeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.files_treeView_AfterCheck);
            this.files_treeView.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this._FilesTreeViewAfterCollapse);
            this.files_treeView.DoubleClick += new System.EventHandler(this._FilesTreeViewDoubleClick);
            this.files_treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._FilesTreeViewAfterSelect);
            this.files_treeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this._FilesTreeViewAfterExpand);
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
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.tableLayoutPanel1);
            this.groupBox2.Location = new System.Drawing.Point(3, 157);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(454, 245);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Options";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.99548F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 69.00452F));
            this.tableLayoutPanel1.Controls.Add(this.extractPatch_label, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.extract_label, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.revertFile_button, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.revertFile_label, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.extract_button, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.extractPatch_button, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 19);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(442, 220);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // extractPatch_label
            // 
            this.extractPatch_label.AutoSize = true;
            this.extractPatch_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extractPatch_label.Location = new System.Drawing.Point(140, 146);
            this.extractPatch_label.Name = "extractPatch_label";
            this.extractPatch_label.Size = new System.Drawing.Size(299, 74);
            this.extractPatch_label.TabIndex = 5;
            this.extractPatch_label.Text = "label7";
            this.extractPatch_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // extract_label
            // 
            this.extract_label.AutoSize = true;
            this.extract_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extract_label.Location = new System.Drawing.Point(140, 73);
            this.extract_label.Name = "extract_label";
            this.extract_label.Size = new System.Drawing.Size(299, 73);
            this.extract_label.TabIndex = 4;
            this.extract_label.Text = "label6";
            this.extract_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // revertFile_button
            // 
            this.revertFile_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.revertFile_button.Location = new System.Drawing.Point(3, 3);
            this.revertFile_button.Name = "revertFile_button";
            this.revertFile_button.Size = new System.Drawing.Size(131, 67);
            this.revertFile_button.TabIndex = 0;
            this.revertFile_button.Text = "Revert File State";
            this.revertFile_button.UseVisualStyleBackColor = true;
            this.revertFile_button.Click += new System.EventHandler(this._RevertFileButtonClick);
            // 
            // revertFile_label
            // 
            this.revertFile_label.AutoSize = true;
            this.revertFile_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.revertFile_label.Location = new System.Drawing.Point(140, 0);
            this.revertFile_label.Name = "revertFile_label";
            this.revertFile_label.Size = new System.Drawing.Size(299, 73);
            this.revertFile_label.TabIndex = 3;
            this.revertFile_label.Text = "label5";
            this.revertFile_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // extract_button
            // 
            this.extract_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.extract_button.Location = new System.Drawing.Point(3, 76);
            this.extract_button.Name = "extract_button";
            this.extract_button.Size = new System.Drawing.Size(131, 67);
            this.extract_button.TabIndex = 2;
            this.extract_button.Text = "Extract";
            this.extract_button.UseVisualStyleBackColor = true;
            this.extract_button.Click += new System.EventHandler(this._ExtractButtonClick);
            // 
            // extractPatch_button
            // 
            this.extractPatch_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.extractPatch_button.Location = new System.Drawing.Point(3, 149);
            this.extractPatch_button.Name = "extractPatch_button";
            this.extractPatch_button.Size = new System.Drawing.Size(131, 68);
            this.extractPatch_button.TabIndex = 1;
            this.extractPatch_button.Text = "Extract and Patch Index";
            this.extractPatch_button.UseVisualStyleBackColor = true;
            this.extractPatch_button.Click += new System.EventHandler(this._ExtractPatchButtonClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(455, 148);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Details";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.90045F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.09955F));
            this.tableLayoutPanel2.Controls.Add(this.fileTime_textBox, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.fileSize_textBox, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.fileName_textBox, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.fileCompressed_textBox, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.loadingLocation_textBox, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.label8, 0, 4);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(7, 20);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(442, 122);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // fileTime_textBox
            // 
            this.fileTime_textBox.Location = new System.Drawing.Point(143, 99);
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
            this.label3.Size = new System.Drawing.Size(134, 24);
            this.label3.TabIndex = 8;
            this.label3.Text = "Compressed Size (bytes)";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // fileSize_textBox
            // 
            this.fileSize_textBox.Location = new System.Drawing.Point(143, 27);
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
            this.label2.Size = new System.Drawing.Size(134, 24);
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
            this.label1.Size = new System.Drawing.Size(134, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "File";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // fileName_textBox
            // 
            this.fileName_textBox.Location = new System.Drawing.Point(143, 3);
            this.fileName_textBox.Name = "fileName_textBox";
            this.fileName_textBox.Size = new System.Drawing.Size(296, 20);
            this.fileName_textBox.TabIndex = 2;
            // 
            // fileCompressed_textBox
            // 
            this.fileCompressed_textBox.Location = new System.Drawing.Point(143, 51);
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
            this.label4.Size = new System.Drawing.Size(134, 24);
            this.label4.TabIndex = 9;
            this.label4.Text = "Loading Location";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // loadingLocation_textBox
            // 
            this.loadingLocation_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.loadingLocation_textBox.Location = new System.Drawing.Point(143, 75);
            this.loadingLocation_textBox.Name = "loadingLocation_textBox";
            this.loadingLocation_textBox.ReadOnly = true;
            this.loadingLocation_textBox.Size = new System.Drawing.Size(296, 20);
            this.loadingLocation_textBox.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(3, 96);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(134, 26);
            this.label8.TabIndex = 11;
            this.label8.Text = "Index File Time";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FileExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 677);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Name = "FileExplorer";
            this.Text = "File Explorer";
            this.Shown += new System.EventHandler(this.FileExplorer_Shown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView files_treeView;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox fileName_textBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox fileSize_textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox fileCompressed_textBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox loadingLocation_textBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button revertFile_button;
        private System.Windows.Forms.Button extractPatch_button;
        private System.Windows.Forms.Button extract_button;
        private System.Windows.Forms.Label revertFile_label;
        private System.Windows.Forms.Label extractPatch_label;
        private System.Windows.Forms.Label extract_label;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label backupKey_label;
        private System.Windows.Forms.Label noEditorKey_label;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox fileTime_textBox;

    }
}