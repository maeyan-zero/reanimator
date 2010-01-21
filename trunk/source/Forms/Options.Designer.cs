namespace Reanimator.Forms
{
    partial class Options
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataDirBrowse = new System.Windows.Forms.Button();
            this.dataDirTextBox = new System.Windows.Forms.TextBox();
            this.dataDirCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.hglDirBrowse = new System.Windows.Forms.Button();
            this.hglDir = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataDirBrowse);
            this.groupBox1.Controls.Add(this.dataDirTextBox);
            this.groupBox1.Controls.Add(this.dataDirCheckBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.hglDirBrowse);
            this.groupBox1.Controls.Add(this.hglDir);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(412, 145);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Directoy Settings";
            // 
            // dataDirBrowse
            // 
            this.dataDirBrowse.Location = new System.Drawing.Point(329, 112);
            this.dataDirBrowse.Name = "dataDirBrowse";
            this.dataDirBrowse.Size = new System.Drawing.Size(75, 23);
            this.dataDirBrowse.TabIndex = 7;
            this.dataDirBrowse.Text = "Browse";
            this.dataDirBrowse.UseVisualStyleBackColor = true;
            this.dataDirBrowse.Click += new System.EventHandler(this.dataDirBrowse_Click);
            // 
            // dataDirTextBox
            // 
            this.dataDirTextBox.Location = new System.Drawing.Point(10, 115);
            this.dataDirTextBox.Name = "dataDirTextBox";
            this.dataDirTextBox.Size = new System.Drawing.Size(313, 20);
            this.dataDirTextBox.TabIndex = 6;
            // 
            // dataDirCheckBox
            // 
            this.dataDirCheckBox.AutoSize = true;
            this.dataDirCheckBox.Checked = true;
            this.dataDirCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dataDirCheckBox.Location = new System.Drawing.Point(7, 92);
            this.dataDirCheckBox.Name = "dataDirCheckBox";
            this.dataDirCheckBox.Size = new System.Drawing.Size(72, 17);
            this.dataDirCheckBox.TabIndex = 5;
            this.dataDirCheckBox.Text = "As Above";
            this.dataDirCheckBox.UseVisualStyleBackColor = true;
            this.dataDirCheckBox.CheckedChanged += new System.EventHandler(this.dataDirCheckBox_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(371, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Root Data Directory (this should to the dir that contains data && data_common)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(186, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Hellgate: London Installation Directory";
            // 
            // hglDirBrowse
            // 
            this.hglDirBrowse.Location = new System.Drawing.Point(329, 33);
            this.hglDirBrowse.Name = "hglDirBrowse";
            this.hglDirBrowse.Size = new System.Drawing.Size(75, 23);
            this.hglDirBrowse.TabIndex = 2;
            this.hglDirBrowse.Text = "Browse";
            this.hglDirBrowse.UseVisualStyleBackColor = true;
            this.hglDirBrowse.Click += new System.EventHandler(this.hglDirBrowse_Click);
            // 
            // hglDir
            // 
            this.hglDir.Location = new System.Drawing.Point(10, 36);
            this.hglDir.Name = "hglDir";
            this.hglDir.Size = new System.Drawing.Size(313, 20);
            this.hglDir.TabIndex = 1;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(353, 163);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 192);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.groupBox1);
            this.Name = "Options";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Reanimator Options";
            this.Load += new System.EventHandler(this.Options_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button hglDirBrowse;
        private System.Windows.Forms.TextBox hglDir;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button dataDirBrowse;
        private System.Windows.Forms.TextBox dataDirTextBox;
        private System.Windows.Forms.CheckBox dataDirCheckBox;
        private System.Windows.Forms.Label label2;


    }
}