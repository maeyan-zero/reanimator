namespace Reanimator.Forms
{
    partial class UpdateForm
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
            this.b_download = new System.Windows.Forms.Button();
            this.lb_availableUpdates = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.l_version = new System.Windows.Forms.Label();
            this.l_status = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // b_download
            // 
            this.b_download.Enabled = false;
            this.b_download.Location = new System.Drawing.Point(12, 261);
            this.b_download.Name = "b_download";
            this.b_download.Size = new System.Drawing.Size(75, 23);
            this.b_download.TabIndex = 1;
            this.b_download.Text = "Download";
            this.b_download.UseVisualStyleBackColor = true;
            this.b_download.Click += new System.EventHandler(this.b_download_Click);
            // 
            // lb_availableUpdates
            // 
            this.lb_availableUpdates.FormattingEnabled = true;
            this.lb_availableUpdates.Location = new System.Drawing.Point(12, 12);
            this.lb_availableUpdates.Name = "lb_availableUpdates";
            this.lb_availableUpdates.Size = new System.Drawing.Size(250, 238);
            this.lb_availableUpdates.TabIndex = 2;
            this.lb_availableUpdates.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lb_availableUpdates_MouseDoubleClick);
            this.lb_availableUpdates.SelectedIndexChanged += new System.EventHandler(this.lb_availableUpdates_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(93, 253);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Current version:";
            // 
            // l_version
            // 
            this.l_version.AutoSize = true;
            this.l_version.Location = new System.Drawing.Point(180, 253);
            this.l_version.Name = "l_version";
            this.l_version.Size = new System.Drawing.Size(37, 13);
            this.l_version.TabIndex = 4;
            this.l_version.Text = "0_0_0";
            // 
            // l_status
            // 
            this.l_status.AutoSize = true;
            this.l_status.Location = new System.Drawing.Point(93, 274);
            this.l_status.Name = "l_status";
            this.l_status.Size = new System.Drawing.Size(145, 13);
            this.l_status.TabIndex = 6;
            this.l_status.Text = "Not installed/downloaded yet";
            // 
            // UpdateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 296);
            this.Controls.Add(this.l_status);
            this.Controls.Add(this.l_version);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lb_availableUpdates);
            this.Controls.Add(this.b_download);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "UpdateForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UpdateForm";
            this.Load += new System.EventHandler(this.UpdateForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button b_download;
        private System.Windows.Forms.ListBox lb_availableUpdates;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label l_version;
        private System.Windows.Forms.Label l_status;
    }
}