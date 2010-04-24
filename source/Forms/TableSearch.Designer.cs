namespace Reanimator.Forms
{
    partial class TableSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TableSearch));
            this.dgv_foundTables = new System.Windows.Forms.DataGridView();
            this.b_prev = new System.Windows.Forms.Button();
            this.b_next = new System.Windows.Forms.Button();
            this.l_tableName = new System.Windows.Forms.Label();
            this.tsl_results = new System.Windows.Forms.ToolStripLabel();
            this.tst_searchString = new System.Windows.Forms.ToolStripTextBox();
            this.tsb_search = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.clb_tablesToSearch = new System.Windows.Forms.CheckedListBox();
            this.b_checkAll = new System.Windows.Forms.Button();
            this.b_uncheckAll = new System.Windows.Forms.Button();
            this.l_position = new System.Windows.Forms.Label();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_foundTables)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv_foundTables
            // 
            this.dgv_foundTables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_foundTables.Location = new System.Drawing.Point(218, 57);
            this.dgv_foundTables.Name = "dgv_foundTables";
            this.dgv_foundTables.Size = new System.Drawing.Size(394, 379);
            this.dgv_foundTables.TabIndex = 0;
            // 
            // b_prev
            // 
            this.b_prev.Location = new System.Drawing.Point(218, 28);
            this.b_prev.Name = "b_prev";
            this.b_prev.Size = new System.Drawing.Size(23, 23);
            this.b_prev.TabIndex = 3;
            this.b_prev.Text = "<";
            this.b_prev.UseVisualStyleBackColor = true;
            this.b_prev.Click += new System.EventHandler(this.b_prev_Click);
            // 
            // b_next
            // 
            this.b_next.Location = new System.Drawing.Point(301, 28);
            this.b_next.Name = "b_next";
            this.b_next.Size = new System.Drawing.Size(23, 23);
            this.b_next.TabIndex = 4;
            this.b_next.Text = ">";
            this.b_next.UseVisualStyleBackColor = true;
            this.b_next.Click += new System.EventHandler(this.b_next_Click);
            // 
            // l_tableName
            // 
            this.l_tableName.AutoSize = true;
            this.l_tableName.Location = new System.Drawing.Point(330, 33);
            this.l_tableName.Name = "l_tableName";
            this.l_tableName.Size = new System.Drawing.Size(53, 13);
            this.l_tableName.TabIndex = 5;
            this.l_tableName.Text = "Unknown";
            // 
            // tsl_results
            // 
            this.tsl_results.Name = "tsl_results";
            this.tsl_results.Size = new System.Drawing.Size(106, 22);
            this.tsl_results.Text = "No matches found";
            // 
            // tst_searchString
            // 
            this.tst_searchString.Name = "tst_searchString";
            this.tst_searchString.Size = new System.Drawing.Size(100, 25);
            // 
            // tsb_search
            // 
            this.tsb_search.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsb_search.Image = ((System.Drawing.Image)(resources.GetObject("tsb_search.Image")));
            this.tsb_search.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_search.Name = "tsb_search";
            this.tsb_search.Size = new System.Drawing.Size(46, 22);
            this.tsb_search.Text = "Search";
            this.tsb_search.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.tsb_search.Click += new System.EventHandler(this.tsb_search_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tst_searchString,
            this.tsb_search,
            this.toolStripSeparator2,
            this.toolStripLabel1,
            this.toolStripSeparator1,
            this.tsl_results});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(624, 25);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // clb_tablesToSearch
            // 
            this.clb_tablesToSearch.CheckOnClick = true;
            this.clb_tablesToSearch.FormattingEnabled = true;
            this.clb_tablesToSearch.Location = new System.Drawing.Point(12, 57);
            this.clb_tablesToSearch.Name = "clb_tablesToSearch";
            this.clb_tablesToSearch.Size = new System.Drawing.Size(200, 379);
            this.clb_tablesToSearch.TabIndex = 8;
            // 
            // b_checkAll
            // 
            this.b_checkAll.Location = new System.Drawing.Point(12, 28);
            this.b_checkAll.Name = "b_checkAll";
            this.b_checkAll.Size = new System.Drawing.Size(75, 23);
            this.b_checkAll.TabIndex = 9;
            this.b_checkAll.Text = "Check all";
            this.b_checkAll.UseVisualStyleBackColor = true;
            this.b_checkAll.Click += new System.EventHandler(this.b_checkAll_Click);
            // 
            // b_uncheckAll
            // 
            this.b_uncheckAll.Location = new System.Drawing.Point(93, 28);
            this.b_uncheckAll.Name = "b_uncheckAll";
            this.b_uncheckAll.Size = new System.Drawing.Size(75, 23);
            this.b_uncheckAll.TabIndex = 10;
            this.b_uncheckAll.Text = "Uncheck all";
            this.b_uncheckAll.UseVisualStyleBackColor = true;
            this.b_uncheckAll.Click += new System.EventHandler(this.b_uncheckAll_Click);
            // 
            // l_position
            // 
            this.l_position.AutoSize = true;
            this.l_position.Location = new System.Drawing.Point(247, 33);
            this.l_position.MaximumSize = new System.Drawing.Size(48, 13);
            this.l_position.MinimumSize = new System.Drawing.Size(48, 13);
            this.l_position.Name = "l_position";
            this.l_position.Size = new System.Drawing.Size(48, 13);
            this.l_position.TabIndex = 11;
            this.l_position.Text = "0/0";
            this.l_position.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(97, 22);
            this.toolStripLabel1.Text = "Wildcards: ?, %, *";
            // 
            // TableSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 444);
            this.Controls.Add(this.l_position);
            this.Controls.Add(this.b_uncheckAll);
            this.Controls.Add(this.b_checkAll);
            this.Controls.Add(this.clb_tablesToSearch);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.l_tableName);
            this.Controls.Add(this.b_next);
            this.Controls.Add(this.b_prev);
            this.Controls.Add(this.dgv_foundTables);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "TableSearch";
            this.Text = "TableSearch";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_foundTables)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_foundTables;
        private System.Windows.Forms.Button b_prev;
        private System.Windows.Forms.Button b_next;
        private System.Windows.Forms.Label l_tableName;
        private System.Windows.Forms.ToolStripLabel tsl_results;
        private System.Windows.Forms.ToolStripTextBox tst_searchString;
        private System.Windows.Forms.ToolStripButton tsb_search;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.CheckedListBox clb_tablesToSearch;
        private System.Windows.Forms.Button b_checkAll;
        private System.Windows.Forms.Button b_uncheckAll;
        private System.Windows.Forms.Label l_position;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    }
}