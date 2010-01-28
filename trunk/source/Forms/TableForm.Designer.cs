namespace Reanimator
{
    partial class TableForm
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
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.IndexFileCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.b_search = new System.Windows.Forms.Button();
            this.tb_searchString = new System.Windows.Forms.TextBox();
            this.infoText_Label = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.extractSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractCheckedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.b_checkAll = new System.Windows.Forms.Button();
            this.b_selectAll = new System.Windows.Forms.Button();
            this.b_unCheckAll = new System.Windows.Forms.Button();
            this.b_unSelectAll = new System.Windows.Forms.Button();
            this.searchResults_Label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitter1
            // 
            this.splitter1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(700, 40);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IndexFileCheckBoxColumn});
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 40);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(700, 380);
            this.dataGridView.TabIndex = 2;
            this.dataGridView.DataSourceChanged += new System.EventHandler(this.dataGridView_DataSourceChanged);
            // 
            // IndexFileCheckBoxColumn
            // 
            this.IndexFileCheckBoxColumn.HeaderText = "";
            this.IndexFileCheckBoxColumn.Name = "IndexFileCheckBoxColumn";
            // 
            // b_search
            // 
            this.b_search.Location = new System.Drawing.Point(106, 20);
            this.b_search.Name = "b_search";
            this.b_search.Size = new System.Drawing.Size(75, 20);
            this.b_search.TabIndex = 3;
            this.b_search.Text = "Search";
            this.b_search.UseVisualStyleBackColor = true;
            this.b_search.Click += new System.EventHandler(this.Search_Click);
            // 
            // tb_searchString
            // 
            this.tb_searchString.Location = new System.Drawing.Point(0, 20);
            this.tb_searchString.Name = "tb_searchString";
            this.tb_searchString.Size = new System.Drawing.Size(100, 20);
            this.tb_searchString.TabIndex = 4;
            // 
            // infoText_Label
            // 
            this.infoText_Label.AutoSize = true;
            this.infoText_Label.Location = new System.Drawing.Point(12, 4);
            this.infoText_Label.Name = "infoText_Label";
            this.infoText_Label.Size = new System.Drawing.Size(46, 13);
            this.infoText_Label.TabIndex = 5;
            this.infoText_Label.Text = "InfoText";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extractSelectedToolStripMenuItem,
            this.extractCheckedToolStripMenuItem,
            this.extractAllToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(168, 70);
            // 
            // extractSelectedToolStripMenuItem
            // 
            this.extractSelectedToolStripMenuItem.Name = "extractSelectedToolStripMenuItem";
            this.extractSelectedToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.extractSelectedToolStripMenuItem.Text = "Extract Selected...";
            this.extractSelectedToolStripMenuItem.Click += new System.EventHandler(this.extractSelectedToolStripMenuItem_Click);
            // 
            // extractCheckedToolStripMenuItem
            // 
            this.extractCheckedToolStripMenuItem.Name = "extractCheckedToolStripMenuItem";
            this.extractCheckedToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.extractCheckedToolStripMenuItem.Text = "Extract Checked...";
            this.extractCheckedToolStripMenuItem.Click += new System.EventHandler(this.extractCheckedToolStripMenuItem_Click);
            // 
            // extractAllToolStripMenuItem
            // 
            this.extractAllToolStripMenuItem.Name = "extractAllToolStripMenuItem";
            this.extractAllToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.extractAllToolStripMenuItem.Text = "Extract All...";
            this.extractAllToolStripMenuItem.Click += new System.EventHandler(this.extractAllToolStripMenuItem_Click);
            // 
            // b_checkAll
            // 
            this.b_checkAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.b_checkAll.Location = new System.Drawing.Point(554, 0);
            this.b_checkAll.Name = "b_checkAll";
            this.b_checkAll.Size = new System.Drawing.Size(70, 20);
            this.b_checkAll.TabIndex = 6;
            this.b_checkAll.Text = "Check";
            this.b_checkAll.UseVisualStyleBackColor = true;
            this.b_checkAll.Click += new System.EventHandler(this.CheckAll_Click);
            // 
            // b_selectAll
            // 
            this.b_selectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.b_selectAll.Location = new System.Drawing.Point(630, 0);
            this.b_selectAll.Name = "b_selectAll";
            this.b_selectAll.Size = new System.Drawing.Size(70, 20);
            this.b_selectAll.TabIndex = 7;
            this.b_selectAll.Text = "Select";
            this.b_selectAll.UseVisualStyleBackColor = true;
            this.b_selectAll.Click += new System.EventHandler(this.SelectAll_Click);
            // 
            // b_unCheckAll
            // 
            this.b_unCheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.b_unCheckAll.Location = new System.Drawing.Point(554, 20);
            this.b_unCheckAll.Name = "b_unCheckAll";
            this.b_unCheckAll.Size = new System.Drawing.Size(70, 20);
            this.b_unCheckAll.TabIndex = 8;
            this.b_unCheckAll.Text = "Uncheck";
            this.b_unCheckAll.UseVisualStyleBackColor = true;
            this.b_unCheckAll.Click += new System.EventHandler(this.UnCheckAll_Click);
            // 
            // b_unSelectAll
            // 
            this.b_unSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.b_unSelectAll.Location = new System.Drawing.Point(630, 20);
            this.b_unSelectAll.Name = "b_unSelectAll";
            this.b_unSelectAll.Size = new System.Drawing.Size(70, 20);
            this.b_unSelectAll.TabIndex = 9;
            this.b_unSelectAll.Text = "Unselect";
            this.b_unSelectAll.UseVisualStyleBackColor = true;
            this.b_unSelectAll.Click += new System.EventHandler(this.UnSelectAll_Click);
            // 
            // searchResults_Label
            // 
            this.searchResults_Label.AutoSize = true;
            this.searchResults_Label.Location = new System.Drawing.Point(187, 24);
            this.searchResults_Label.Name = "searchResults_Label";
            this.searchResults_Label.Size = new System.Drawing.Size(79, 13);
            this.searchResults_Label.TabIndex = 10;
            this.searchResults_Label.Text = "Search Results";
            // 
            // TableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 420);
            this.Controls.Add(this.searchResults_Label);
            this.Controls.Add(this.b_unSelectAll);
            this.Controls.Add(this.b_unCheckAll);
            this.Controls.Add(this.b_selectAll);
            this.Controls.Add(this.b_checkAll);
            this.Controls.Add(this.tb_searchString);
            this.Controls.Add(this.infoText_Label);
            this.Controls.Add(this.b_search);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.splitter1);
            this.Name = "TableForm";
            this.Text = "Table Form";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Splitter splitter1;
        public System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IndexFileCheckBoxColumn;
        private System.Windows.Forms.Button b_search;
        private System.Windows.Forms.TextBox tb_searchString;
        private System.Windows.Forms.Label infoText_Label;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem extractSelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extractCheckedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extractAllToolStripMenuItem;
        private System.Windows.Forms.Button b_checkAll;
        private System.Windows.Forms.Button b_selectAll;
        private System.Windows.Forms.Button b_unCheckAll;
        private System.Windows.Forms.Button b_unSelectAll;
        private System.Windows.Forms.Label searchResults_Label;
    }
}