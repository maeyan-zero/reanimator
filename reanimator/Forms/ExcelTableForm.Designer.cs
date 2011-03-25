namespace Reanimator.Forms
{
    partial class ExcelTableForm
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
            this.stringsPage = new System.Windows.Forms.TabPage();
            this._strings_ListBox = new System.Windows.Forms.ListBox();
            this.indexArraysPage = new System.Windows.Forms.TabPage();
            this.indexArrays_DataGridView = new System.Windows.Forms.DataGridView();
            this.tableDataPage = new System.Windows.Forms.TabPage();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.duplicateRowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showRowViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._tableData_DataGridView = new System.Windows.Forms.DataGridView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.stringsPage.SuspendLayout();
            this.indexArraysPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.indexArrays_DataGridView)).BeginInit();
            this.tableDataPage.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._tableData_DataGridView)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // stringsPage
            // 
            this.stringsPage.Controls.Add(this._strings_ListBox);
            this.stringsPage.Location = new System.Drawing.Point(4, 24);
            this.stringsPage.Name = "stringsPage";
            this.stringsPage.Padding = new System.Windows.Forms.Padding(3);
            this.stringsPage.Size = new System.Drawing.Size(809, 590);
            this.stringsPage.TabIndex = 2;
            this.stringsPage.Text = "Strings";
            this.stringsPage.UseVisualStyleBackColor = true;
            // 
            // _strings_ListBox
            // 
            this._strings_ListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this._strings_ListBox.FormattingEnabled = true;
            this._strings_ListBox.ItemHeight = 15;
            this._strings_ListBox.Location = new System.Drawing.Point(7, 7);
            this._strings_ListBox.Name = "_strings_ListBox";
            this._strings_ListBox.Size = new System.Drawing.Size(245, 559);
            this._strings_ListBox.TabIndex = 0;
            // 
            // indexArraysPage
            // 
            this.indexArraysPage.Controls.Add(this.indexArrays_DataGridView);
            this.indexArraysPage.Location = new System.Drawing.Point(4, 24);
            this.indexArraysPage.Name = "indexArraysPage";
            this.indexArraysPage.Padding = new System.Windows.Forms.Padding(3);
            this.indexArraysPage.Size = new System.Drawing.Size(809, 590);
            this.indexArraysPage.TabIndex = 1;
            this.indexArraysPage.Text = "Index Arrays";
            this.indexArraysPage.UseVisualStyleBackColor = true;
            // 
            // indexArrays_DataGridView
            // 
            this.indexArrays_DataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.indexArrays_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.indexArrays_DataGridView.Location = new System.Drawing.Point(3, 3);
            this.indexArrays_DataGridView.Name = "indexArrays_DataGridView";
            this.indexArrays_DataGridView.Size = new System.Drawing.Size(803, 563);
            this.indexArrays_DataGridView.TabIndex = 0;
            // 
            // tableDataPage
            // 
            this.tableDataPage.Controls.Add(this.splitContainer1);
            this.tableDataPage.Controls.Add(this.menuStrip1);
            this.tableDataPage.Location = new System.Drawing.Point(4, 24);
            this.tableDataPage.Name = "tableDataPage";
            this.tableDataPage.Padding = new System.Windows.Forms.Padding(3);
            this.tableDataPage.Size = new System.Drawing.Size(809, 590);
            this.tableDataPage.TabIndex = 0;
            this.tableDataPage.Text = "Table Data";
            this.tableDataPage.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.AllowMerge = false;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(3, 3);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(803, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importToolStripMenuItem,
            this.exportToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.importToolStripMenuItem.Text = "&Import..";
            this.importToolStripMenuItem.Click += new System.EventHandler(this._ImportButton_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.exportToolStripMenuItem.Text = "&Export..";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this._ExportButton_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.duplicateRowsToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // duplicateRowsToolStripMenuItem
            // 
            this.duplicateRowsToolStripMenuItem.Name = "duplicateRowsToolStripMenuItem";
            this.duplicateRowsToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.duplicateRowsToolStripMenuItem.Text = "&Duplicate Rows";
            this.duplicateRowsToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showRowViewToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // showRowViewToolStripMenuItem
            // 
            this.showRowViewToolStripMenuItem.Checked = true;
            this.showRowViewToolStripMenuItem.CheckOnClick = true;
            this.showRowViewToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showRowViewToolStripMenuItem.Name = "showRowViewToolStripMenuItem";
            this.showRowViewToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.showRowViewToolStripMenuItem.Text = "&Rows";
            this.showRowViewToolStripMenuItem.Click += new System.EventHandler(this._ToggleRowView);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(3, 27);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this._tableData_DataGridView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(803, 560);
            this.splitContainer1.SplitterDistance = 481;
            this.splitContainer1.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(318, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(0, 560);
            this.panel1.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(318, 560);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // _tableData_DataGridView
            // 
            this._tableData_DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableData_DataGridView.Location = new System.Drawing.Point(0, 0);
            this._tableData_DataGridView.Name = "_tableData_DataGridView";
            this._tableData_DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._tableData_DataGridView.Size = new System.Drawing.Size(481, 560);
            this._tableData_DataGridView.TabIndex = 1;
            this._tableData_DataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this._TableData_DataGridView_CellDoubleClick);
            this._tableData_DataGridView.SelectionChanged += new System.EventHandler(this._Rows_ListBox_SelectedIndexChanged);
            this._tableData_DataGridView.KeyUp += new System.Windows.Forms.KeyEventHandler(this._TableData_DataGridView_KeyUp);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tableDataPage);
            this.tabControl1.Controls.Add(this.indexArraysPage);
            this.tabControl1.Controls.Add(this.stringsPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(817, 618);
            this.tabControl1.TabIndex = 4;
            // 
            // ExcelTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 618);
            this.Controls.Add(this.tabControl1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ExcelTableForm";
            this.Text = "ExcelTable";
            this.stringsPage.ResumeLayout(false);
            this.indexArraysPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.indexArrays_DataGridView)).EndInit();
            this.tableDataPage.ResumeLayout(false);
            this.tableDataPage.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._tableData_DataGridView)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage stringsPage;
        private System.Windows.Forms.ListBox _strings_ListBox;
        private System.Windows.Forms.TabPage indexArraysPage;
        private System.Windows.Forms.DataGridView indexArrays_DataGridView;
        private System.Windows.Forms.TabPage tableDataPage;
        private System.Windows.Forms.SplitContainer splitContainer1;
        public System.Windows.Forms.DataGridView _tableData_DataGridView;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem duplicateRowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showRowViewToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;

    }
}