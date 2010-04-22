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
            this.dgv_foundTables = new System.Windows.Forms.DataGridView();
            this.b_search = new System.Windows.Forms.Button();
            this.tb_searchString = new System.Windows.Forms.TextBox();
            this.b_prev = new System.Windows.Forms.Button();
            this.b_next = new System.Windows.Forms.Button();
            this.l_tableName = new System.Windows.Forms.Label();
            this.l_results = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_foundTables)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_foundTables
            // 
            this.dgv_foundTables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_foundTables.Location = new System.Drawing.Point(12, 32);
            this.dgv_foundTables.Name = "dgv_foundTables";
            this.dgv_foundTables.Size = new System.Drawing.Size(600, 400);
            this.dgv_foundTables.TabIndex = 0;
            // 
            // b_search
            // 
            this.b_search.Location = new System.Drawing.Point(537, 3);
            this.b_search.Name = "b_search";
            this.b_search.Size = new System.Drawing.Size(75, 23);
            this.b_search.TabIndex = 1;
            this.b_search.Text = "Search";
            this.b_search.UseVisualStyleBackColor = true;
            this.b_search.Click += new System.EventHandler(this.b_search_Click);
            // 
            // tb_searchString
            // 
            this.tb_searchString.Location = new System.Drawing.Point(391, 5);
            this.tb_searchString.Name = "tb_searchString";
            this.tb_searchString.Size = new System.Drawing.Size(140, 20);
            this.tb_searchString.TabIndex = 2;
            // 
            // b_prev
            // 
            this.b_prev.Location = new System.Drawing.Point(12, 3);
            this.b_prev.Name = "b_prev";
            this.b_prev.Size = new System.Drawing.Size(23, 23);
            this.b_prev.TabIndex = 3;
            this.b_prev.Text = "<";
            this.b_prev.UseVisualStyleBackColor = true;
            this.b_prev.Click += new System.EventHandler(this.b_prev_Click);
            // 
            // b_next
            // 
            this.b_next.Location = new System.Drawing.Point(41, 3);
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
            this.l_tableName.Location = new System.Drawing.Point(70, 9);
            this.l_tableName.Name = "l_tableName";
            this.l_tableName.Size = new System.Drawing.Size(53, 13);
            this.l_tableName.TabIndex = 5;
            this.l_tableName.Text = "Unknown";
            // 
            // l_results
            // 
            this.l_results.AutoSize = true;
            this.l_results.Location = new System.Drawing.Point(291, 9);
            this.l_results.Name = "l_results";
            this.l_results.Size = new System.Drawing.Size(94, 13);
            this.l_results.TabIndex = 6;
            this.l_results.Text = "No matches found";
            // 
            // TableSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 444);
            this.Controls.Add(this.l_results);
            this.Controls.Add(this.l_tableName);
            this.Controls.Add(this.b_next);
            this.Controls.Add(this.b_prev);
            this.Controls.Add(this.tb_searchString);
            this.Controls.Add(this.b_search);
            this.Controls.Add(this.dgv_foundTables);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "TableSearch";
            this.Text = "TableSearch";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_foundTables)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_foundTables;
        private System.Windows.Forms.Button b_search;
        private System.Windows.Forms.TextBox tb_searchString;
        private System.Windows.Forms.Button b_prev;
        private System.Windows.Forms.Button b_next;
        private System.Windows.Forms.Label l_tableName;
        private System.Windows.Forms.Label l_results;
    }
}