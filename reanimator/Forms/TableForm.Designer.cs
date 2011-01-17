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
            this.replaceSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceCheckedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkAll_Button = new System.Windows.Forms.Button();
            this.selectAll_Button = new System.Windows.Forms.Button();
            this.uncheckAll_Button = new System.Windows.Forms.Button();
            this.unselectAll_Button = new System.Windows.Forms.Button();
            this.searchResults_Label = new System.Windows.Forms.Label();
            this.b_prev = new System.Windows.Forms.Button();
            this.b_next = new System.Windows.Forms.Button();
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
            this.dataGridView.DataSourceChanged += new System.EventHandler(this._DataGridView_DataSourceChanged);
            // 
            // IndexFileCheckBoxColumn
            // 
            this.IndexFileCheckBoxColumn.HeaderText = "";
            this.IndexFileCheckBoxColumn.Name = "IndexFileCheckBoxColumn";
            // 
            // b_search
            // 
            this.b_search.Location = new System.Drawing.Point(140, 20);
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
            this.extractAllToolStripMenuItem,
            this.replaceSelectedToolStripMenuItem,
            this.replaceCheckedToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(174, 158);
            // 
            // extractSelectedToolStripMenuItem
            // 
            this.extractSelectedToolStripMenuItem.Name = "extractSelectedToolStripMenuItem";
            this.extractSelectedToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.extractSelectedToolStripMenuItem.Text = "Extract Selected...";
            this.extractSelectedToolStripMenuItem.Click += new System.EventHandler(this.extractSelectedToolStripMenuItem_Click);
            // 
            // extractCheckedToolStripMenuItem
            // 
            this.extractCheckedToolStripMenuItem.Name = "extractCheckedToolStripMenuItem";
            this.extractCheckedToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.extractCheckedToolStripMenuItem.Text = "Extract Checked...";
            this.extractCheckedToolStripMenuItem.Click += new System.EventHandler(this.extractCheckedToolStripMenuItem_Click);
            // 
            // extractAllToolStripMenuItem
            // 
            this.extractAllToolStripMenuItem.Name = "extractAllToolStripMenuItem";
            this.extractAllToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.extractAllToolStripMenuItem.Text = "Extract All...";
            this.extractAllToolStripMenuItem.Click += new System.EventHandler(this.extractAllToolStripMenuItem_Click);
            // 
            // replaceSelectedToolStripMenuItem
            // 
            this.replaceSelectedToolStripMenuItem.Name = "replaceSelectedToolStripMenuItem";
            this.replaceSelectedToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.replaceSelectedToolStripMenuItem.Text = "Replace Selected...";
            this.replaceSelectedToolStripMenuItem.Click += new System.EventHandler(this.replaceSelectedToolStripMenuItem_Click);
            // 
            // replaceCheckedToolStripMenuItem
            // 
            this.replaceCheckedToolStripMenuItem.Name = "replaceCheckedToolStripMenuItem";
            this.replaceCheckedToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.replaceCheckedToolStripMenuItem.Text = "Replace Checked...";
            this.replaceCheckedToolStripMenuItem.Click += new System.EventHandler(this.replaceCheckedToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // checkAll_Button
            // 
            this.checkAll_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkAll_Button.Location = new System.Drawing.Point(538, 0);
            this.checkAll_Button.Name = "checkAll_Button";
            this.checkAll_Button.Size = new System.Drawing.Size(75, 20);
            this.checkAll_Button.TabIndex = 6;
            this.checkAll_Button.Text = "Check All";
            this.checkAll_Button.UseVisualStyleBackColor = true;
            this.checkAll_Button.Click += new System.EventHandler(this.CheckAll_Click);
            // 
            // selectAll_Button
            // 
            this.selectAll_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectAll_Button.Location = new System.Drawing.Point(619, 0);
            this.selectAll_Button.Name = "selectAll_Button";
            this.selectAll_Button.Size = new System.Drawing.Size(81, 20);
            this.selectAll_Button.TabIndex = 7;
            this.selectAll_Button.Text = "Select All";
            this.selectAll_Button.UseVisualStyleBackColor = true;
            this.selectAll_Button.Click += new System.EventHandler(this.SelectAll_Click);
            // 
            // uncheckAll_Button
            // 
            this.uncheckAll_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uncheckAll_Button.Location = new System.Drawing.Point(538, 20);
            this.uncheckAll_Button.Name = "uncheckAll_Button";
            this.uncheckAll_Button.Size = new System.Drawing.Size(75, 20);
            this.uncheckAll_Button.TabIndex = 8;
            this.uncheckAll_Button.Text = "Uncheck All";
            this.uncheckAll_Button.UseVisualStyleBackColor = true;
            this.uncheckAll_Button.Click += new System.EventHandler(this.UnCheckAll_Click);
            // 
            // unselectAll_Button
            // 
            this.unselectAll_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.unselectAll_Button.Location = new System.Drawing.Point(619, 20);
            this.unselectAll_Button.Name = "unselectAll_Button";
            this.unselectAll_Button.Size = new System.Drawing.Size(81, 20);
            this.unselectAll_Button.TabIndex = 9;
            this.unselectAll_Button.Text = "Unselect All";
            this.unselectAll_Button.UseVisualStyleBackColor = true;
            this.unselectAll_Button.Click += new System.EventHandler(this.UnSelectAll_Click);
            // 
            // searchResults_Label
            // 
            this.searchResults_Label.AutoSize = true;
            this.searchResults_Label.Location = new System.Drawing.Point(255, 23);
            this.searchResults_Label.Name = "searchResults_Label";
            this.searchResults_Label.Size = new System.Drawing.Size(79, 13);
            this.searchResults_Label.TabIndex = 10;
            this.searchResults_Label.Text = "Search Results";
            // 
            // b_prev
            // 
            this.b_prev.Location = new System.Drawing.Point(106, 20);
            this.b_prev.Name = "b_prev";
            this.b_prev.Size = new System.Drawing.Size(28, 20);
            this.b_prev.TabIndex = 11;
            this.b_prev.Text = "<<";
            this.b_prev.UseVisualStyleBackColor = true;
            this.b_prev.Click += new System.EventHandler(this.b_prev_Click);
            // 
            // b_next
            // 
            this.b_next.Location = new System.Drawing.Point(221, 20);
            this.b_next.Name = "b_next";
            this.b_next.Size = new System.Drawing.Size(28, 20);
            this.b_next.TabIndex = 12;
            this.b_next.Text = ">>";
            this.b_next.UseVisualStyleBackColor = true;
            this.b_next.Click += new System.EventHandler(this.b_next_Click);
            // 
            // TableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 420);
            this.Controls.Add(this.b_next);
            this.Controls.Add(this.b_prev);
            this.Controls.Add(this.searchResults_Label);
            this.Controls.Add(this.unselectAll_Button);
            this.Controls.Add(this.selectAll_Button);
            this.Controls.Add(this.tb_searchString);
            this.Controls.Add(this.uncheckAll_Button);
            this.Controls.Add(this.b_search);
            this.Controls.Add(this.infoText_Label);
            this.Controls.Add(this.checkAll_Button);
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
        private System.Windows.Forms.Button checkAll_Button;
        private System.Windows.Forms.Button selectAll_Button;
        private System.Windows.Forms.Button uncheckAll_Button;
        private System.Windows.Forms.Button unselectAll_Button;
        private System.Windows.Forms.Label searchResults_Label;
        private System.Windows.Forms.Button b_prev;
        private System.Windows.Forms.Button b_next;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceSelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceCheckedToolStripMenuItem;
    }
}