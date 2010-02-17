namespace Reanimator.Forms
{
    partial class CacheInfo
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
            this.b_update = new System.Windows.Forms.Button();
            this.lb_cachedFiles = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.l_fileNumber = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.l_totalSize = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // b_update
            // 
            this.b_update.Location = new System.Drawing.Point(167, 229);
            this.b_update.Name = "b_update";
            this.b_update.Size = new System.Drawing.Size(75, 23);
            this.b_update.TabIndex = 0;
            this.b_update.Text = "Update";
            this.b_update.UseVisualStyleBackColor = true;
            this.b_update.Click += new System.EventHandler(this.b_update_Click);
            // 
            // lb_cachedFiles
            // 
            this.lb_cachedFiles.FormattingEnabled = true;
            this.lb_cachedFiles.Location = new System.Drawing.Point(12, 12);
            this.lb_cachedFiles.Name = "lb_cachedFiles";
            this.lb_cachedFiles.Size = new System.Drawing.Size(230, 212);
            this.lb_cachedFiles.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 234);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Cached files:";
            // 
            // l_fileNumber
            // 
            this.l_fileNumber.AutoSize = true;
            this.l_fileNumber.Location = new System.Drawing.Point(129, 234);
            this.l_fileNumber.MaximumSize = new System.Drawing.Size(32, 13);
            this.l_fileNumber.MinimumSize = new System.Drawing.Size(32, 13);
            this.l_fileNumber.Name = "l_fileNumber";
            this.l_fileNumber.Size = new System.Drawing.Size(32, 13);
            this.l_fileNumber.TabIndex = 3;
            this.l_fileNumber.Text = "0";
            this.l_fileNumber.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 254);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Total size:";
            // 
            // l_totalSize
            // 
            this.l_totalSize.AutoSize = true;
            this.l_totalSize.Location = new System.Drawing.Point(97, 254);
            this.l_totalSize.MaximumSize = new System.Drawing.Size(64, 13);
            this.l_totalSize.MinimumSize = new System.Drawing.Size(64, 13);
            this.l_totalSize.Name = "l_totalSize";
            this.l_totalSize.Size = new System.Drawing.Size(64, 13);
            this.l_totalSize.TabIndex = 6;
            this.l_totalSize.Text = "0 KB";
            this.l_totalSize.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // CacheInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 276);
            this.Controls.Add(this.l_totalSize);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.l_fileNumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lb_cachedFiles);
            this.Controls.Add(this.b_update);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "CacheInfo";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Cache Info";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button b_update;
        private System.Windows.Forms.ListBox lb_cachedFiles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label l_fileNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label l_totalSize;
    }
}