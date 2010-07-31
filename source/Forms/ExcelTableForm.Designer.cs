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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExcelTableForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tableDataPage = new System.Windows.Forms.TabPage();
            this.tableData_DataGridView = new System.Windows.Forms.DataGridView();
            this.indexArraysPage = new System.Windows.Forms.TabPage();
            this.indexArrays_DataGridView = new System.Windows.Forms.DataGridView();
            this.stringsPage = new System.Windows.Forms.TabPage();
            this.strings_ListBox = new System.Windows.Forms.ListBox();
            this.rowViewPage = new System.Windows.Forms.TabPage();
            this.rows_LayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.rows_ListBox = new System.Windows.Forms.ListBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.tabControl1.SuspendLayout();
            this.tableDataPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tableData_DataGridView)).BeginInit();
            this.indexArraysPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.indexArrays_DataGridView)).BeginInit();
            this.stringsPage.SuspendLayout();
            this.rowViewPage.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tableDataPage);
            this.tabControl1.Controls.Add(this.indexArraysPage);
            this.tabControl1.Controls.Add(this.stringsPage);
            this.tabControl1.Controls.Add(this.rowViewPage);
            this.tabControl1.Location = new System.Drawing.Point(0, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(752, 588);
            this.tabControl1.TabIndex = 4;
            // 
            // tableDataPage
            // 
            this.tableDataPage.Controls.Add(this.tableData_DataGridView);
            this.tableDataPage.Location = new System.Drawing.Point(4, 22);
            this.tableDataPage.Name = "tableDataPage";
            this.tableDataPage.Padding = new System.Windows.Forms.Padding(3);
            this.tableDataPage.Size = new System.Drawing.Size(744, 562);
            this.tableDataPage.TabIndex = 0;
            this.tableDataPage.Text = "Table Data";
            this.tableDataPage.UseVisualStyleBackColor = true;
            // 
            // tableData_DataGridView
            // 
            this.tableData_DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableData_DataGridView.Location = new System.Drawing.Point(3, 3);
            this.tableData_DataGridView.Name = "tableData_DataGridView";
            this.tableData_DataGridView.Size = new System.Drawing.Size(738, 556);
            this.tableData_DataGridView.TabIndex = 1;
            // 
            // indexArraysPage
            // 
            this.indexArraysPage.Controls.Add(this.indexArrays_DataGridView);
            this.indexArraysPage.Location = new System.Drawing.Point(4, 22);
            this.indexArraysPage.Name = "indexArraysPage";
            this.indexArraysPage.Padding = new System.Windows.Forms.Padding(3);
            this.indexArraysPage.Size = new System.Drawing.Size(744, 562);
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
            this.indexArrays_DataGridView.Location = new System.Drawing.Point(-4, 0);
            this.indexArrays_DataGridView.Name = "indexArrays_DataGridView";
            this.indexArrays_DataGridView.Size = new System.Drawing.Size(688, 569);
            this.indexArrays_DataGridView.TabIndex = 0;
            // 
            // stringsPage
            // 
            this.stringsPage.Controls.Add(this.strings_ListBox);
            this.stringsPage.Location = new System.Drawing.Point(4, 22);
            this.stringsPage.Name = "stringsPage";
            this.stringsPage.Padding = new System.Windows.Forms.Padding(3);
            this.stringsPage.Size = new System.Drawing.Size(744, 562);
            this.stringsPage.TabIndex = 2;
            this.stringsPage.Text = "Strings";
            this.stringsPage.UseVisualStyleBackColor = true;
            // 
            // strings_ListBox
            // 
            this.strings_ListBox.FormattingEnabled = true;
            this.strings_ListBox.Location = new System.Drawing.Point(6, 6);
            this.strings_ListBox.Name = "strings_ListBox";
            this.strings_ListBox.Size = new System.Drawing.Size(211, 563);
            this.strings_ListBox.TabIndex = 0;
            // 
            // rowViewPage
            // 
            this.rowViewPage.Controls.Add(this.rows_LayoutPanel);
            this.rowViewPage.Controls.Add(this.rows_ListBox);
            this.rowViewPage.Location = new System.Drawing.Point(4, 22);
            this.rowViewPage.Name = "rowViewPage";
            this.rowViewPage.Padding = new System.Windows.Forms.Padding(3);
            this.rowViewPage.Size = new System.Drawing.Size(744, 559);
            this.rowViewPage.TabIndex = 3;
            this.rowViewPage.Text = "Row View";
            this.rowViewPage.UseVisualStyleBackColor = true;
            // 
            // rows_LayoutPanel
            // 
            this.rows_LayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rows_LayoutPanel.AutoScroll = true;
            this.rows_LayoutPanel.ColumnCount = 2;
            this.rows_LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.rows_LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.rows_LayoutPanel.Location = new System.Drawing.Point(198, 6);
            this.rows_LayoutPanel.Name = "rows_LayoutPanel";
            this.rows_LayoutPanel.RowCount = 1;
            this.rows_LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.rows_LayoutPanel.Size = new System.Drawing.Size(516, 543);
            this.rows_LayoutPanel.TabIndex = 1;
            // 
            // rows_ListBox
            // 
            this.rows_ListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.rows_ListBox.FormattingEnabled = true;
            this.rows_ListBox.Location = new System.Drawing.Point(6, 6);
            this.rows_ListBox.Name = "rows_ListBox";
            this.rows_ListBox.Size = new System.Drawing.Size(186, 537);
            this.rows_ListBox.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 592);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStrip1.Size = new System.Drawing.Size(752, 25);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(70, 22);
            this.toolStripButton1.Text = "Regenerate";
            this.toolStripButton1.Click += new System.EventHandler(this.regenTable_Click);
            // 
            // ExcelTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 617);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.tabControl1);
            this.Name = "ExcelTableForm";
            this.Text = "ExcelTable";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExcelTableForm_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tableDataPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tableData_DataGridView)).EndInit();
            this.indexArraysPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.indexArrays_DataGridView)).EndInit();
            this.stringsPage.ResumeLayout(false);
            this.rowViewPage.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tableDataPage;
        private System.Windows.Forms.TabPage indexArraysPage;
        private System.Windows.Forms.DataGridView indexArrays_DataGridView;
        public System.Windows.Forms.DataGridView tableData_DataGridView;
        private System.Windows.Forms.TabPage stringsPage;
        private System.Windows.Forms.ListBox strings_ListBox;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TabPage rowViewPage;
        private System.Windows.Forms.ListBox rows_ListBox;
        private System.Windows.Forms.TableLayoutPanel rows_LayoutPanel;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
    }
}