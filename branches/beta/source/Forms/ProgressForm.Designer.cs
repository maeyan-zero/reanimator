namespace Reanimator.Forms
{
    partial class ProgressForm
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
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.loadingTextLabel = new System.Windows.Forms.Label();
            this.currentItemLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 50);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(364, 23);
            this.progressBar.TabIndex = 0;
            this.progressBar.UseWaitCursor = true;
            // 
            // loadingTextLabel
            // 
            this.loadingTextLabel.AutoSize = true;
            this.loadingTextLabel.Location = new System.Drawing.Point(12, 12);
            this.loadingTextLabel.Margin = new System.Windows.Forms.Padding(3);
            this.loadingTextLabel.Name = "loadingTextLabel";
            this.loadingTextLabel.Size = new System.Drawing.Size(61, 13);
            this.loadingTextLabel.TabIndex = 1;
            this.loadingTextLabel.Text = "loading text";
            this.loadingTextLabel.UseWaitCursor = true;
            // 
            // currentItemLabel
            // 
            this.currentItemLabel.AutoSize = true;
            this.currentItemLabel.Location = new System.Drawing.Point(12, 31);
            this.currentItemLabel.Margin = new System.Windows.Forms.Padding(3);
            this.currentItemLabel.Name = "currentItemLabel";
            this.currentItemLabel.Size = new System.Drawing.Size(62, 13);
            this.currentItemLabel.TabIndex = 2;
            this.currentItemLabel.Text = "current item";
            this.currentItemLabel.UseWaitCursor = true;
            this.currentItemLabel.TextChanged += new System.EventHandler(this.currentItemLabel_TextChanged);
            // 
            // ProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 80);
            this.ControlBox = false;
            this.Controls.Add(this.currentItemLabel);
            this.Controls.Add(this.loadingTextLabel);
            this.Controls.Add(this.progressBar);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Loading...";
            this.UseWaitCursor = true;
            this.Shown += new System.EventHandler(this.Progress_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label loadingTextLabel;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label currentItemLabel;
    }
}