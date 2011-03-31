namespace Reanimator.Forms
{
    partial class SavegameOverviewForm
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
            this.treeViewSkills = new System.Windows.Forms.TreeView();
            this.savegameOverviewControl1 = new SavegameOverviewControl();
            this.SuspendLayout();
            // 
            // treeViewSkills
            // 
            this.treeViewSkills.Location = new System.Drawing.Point(12, 181);
            this.treeViewSkills.Name = "treeViewSkills";
            this.treeViewSkills.Size = new System.Drawing.Size(441, 251);
            this.treeViewSkills.TabIndex = 2;
            // 
            // savegameOverviewControl1
            // 
            this.savegameOverviewControl1.AutoScroll = true;
            this.savegameOverviewControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.savegameOverviewControl1.Location = new System.Drawing.Point(12, 12);
            this.savegameOverviewControl1.Name = "savegameOverviewControl1";
            this.savegameOverviewControl1.Size = new System.Drawing.Size(440, 163);
            this.savegameOverviewControl1.TabIndex = 0;
            // 
            // SavegameOverviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 444);
            this.Controls.Add(this.treeViewSkills);
            this.Controls.Add(this.savegameOverviewControl1);
            this.Name = "SavegameOverviewForm";
            this.Text = "SavegameOverviewForm";
            this.ResumeLayout(false);

        }

        #endregion

        private SavegameOverviewControl savegameOverviewControl1;
        private System.Windows.Forms.TreeView treeViewSkills;
    }
}