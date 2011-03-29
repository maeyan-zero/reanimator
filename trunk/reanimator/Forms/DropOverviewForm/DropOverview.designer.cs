namespace Reanimator.Forms.DropOverviewForm
{
    partial class DropOverview
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
            this.cb_dropTables = new System.Windows.Forms.ComboBox();
            this.tv_drops = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_expandTree = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cb_dropTables
            // 
            this.cb_dropTables.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cb_dropTables.FormattingEnabled = true;
            this.cb_dropTables.Location = new System.Drawing.Point(12, 12);
            this.cb_dropTables.Name = "cb_dropTables";
            this.cb_dropTables.Size = new System.Drawing.Size(192, 21);
            this.cb_dropTables.TabIndex = 0;
            this.cb_dropTables.SelectedIndexChanged += new System.EventHandler(this.cb_dropTables_SelectedIndexChanged);
            // 
            // tv_drops
            // 
            this.tv_drops.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tv_drops.FullRowSelect = true;
            this.tv_drops.Location = new System.Drawing.Point(12, 52);
            this.tv_drops.Name = "tv_drops";
            this.tv_drops.Size = new System.Drawing.Size(260, 236);
            this.tv_drops.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(12, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(259, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "If an entry is red, only one of the items will be dropped";
            // 
            // cb_expandTree
            // 
            this.cb_expandTree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cb_expandTree.AutoSize = true;
            this.cb_expandTree.Location = new System.Drawing.Point(210, 14);
            this.cb_expandTree.Name = "cb_expandTree";
            this.cb_expandTree.Size = new System.Drawing.Size(62, 17);
            this.cb_expandTree.TabIndex = 3;
            this.cb_expandTree.Text = "Expand";
            this.cb_expandTree.UseVisualStyleBackColor = true;
            this.cb_expandTree.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // DropOverview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 300);
            this.Controls.Add(this.cb_expandTree);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tv_drops);
            this.Controls.Add(this.cb_dropTables);
            this.MinimumSize = new System.Drawing.Size(300, 100);
            this.Name = "DropOverview";
            this.ShowIcon = false;
            this.Text = "DropOverview";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cb_dropTables;
        private System.Windows.Forms.TreeView tv_drops;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cb_expandTree;
    }
}