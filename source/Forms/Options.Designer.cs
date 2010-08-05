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
            this.pathSettings_GroupBox = new System.Windows.Forms.GroupBox();
            this.scriptButton = new System.Windows.Forms.Button();
            this.scriptDirText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gameClientPath_Button = new System.Windows.Forms.Button();
            this.gameClientPath_TextBox = new System.Windows.Forms.TextBox();
            this.gameClientPath_Label = new System.Windows.Forms.Label();
            this.dataDir_Button = new System.Windows.Forms.Button();
            this.dataDir_TextBox = new System.Windows.Forms.TextBox();
            this.dataDir_CheckBox = new System.Windows.Forms.CheckBox();
            this.dataDir_Label = new System.Windows.Forms.Label();
            this.hglDir_Label = new System.Windows.Forms.Label();
            this.hglDir_Button = new System.Windows.Forms.Button();
            this.hglDir_TextBox = new System.Windows.Forms.TextBox();
            this.ok_Button = new System.Windows.Forms.Button();
            this.pathSettings_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // pathSettings_GroupBox
            // 
            this.pathSettings_GroupBox.Controls.Add(this.scriptButton);
            this.pathSettings_GroupBox.Controls.Add(this.scriptDirText);
            this.pathSettings_GroupBox.Controls.Add(this.label1);
            this.pathSettings_GroupBox.Controls.Add(this.gameClientPath_Button);
            this.pathSettings_GroupBox.Controls.Add(this.gameClientPath_TextBox);
            this.pathSettings_GroupBox.Controls.Add(this.gameClientPath_Label);
            this.pathSettings_GroupBox.Controls.Add(this.dataDir_Button);
            this.pathSettings_GroupBox.Controls.Add(this.dataDir_TextBox);
            this.pathSettings_GroupBox.Controls.Add(this.dataDir_CheckBox);
            this.pathSettings_GroupBox.Controls.Add(this.dataDir_Label);
            this.pathSettings_GroupBox.Controls.Add(this.hglDir_Label);
            this.pathSettings_GroupBox.Controls.Add(this.hglDir_Button);
            this.pathSettings_GroupBox.Controls.Add(this.hglDir_TextBox);
            this.pathSettings_GroupBox.Location = new System.Drawing.Point(12, 12);
            this.pathSettings_GroupBox.Name = "pathSettings_GroupBox";
            this.pathSettings_GroupBox.Size = new System.Drawing.Size(413, 250);
            this.pathSettings_GroupBox.TabIndex = 0;
            this.pathSettings_GroupBox.TabStop = false;
            this.pathSettings_GroupBox.Text = "Path Settings";
            // 
            // scriptButton
            // 
            this.scriptButton.Location = new System.Drawing.Point(329, 221);
            this.scriptButton.Name = "scriptButton";
            this.scriptButton.Size = new System.Drawing.Size(75, 23);
            this.scriptButton.TabIndex = 13;
            this.scriptButton.Text = "Browse";
            this.scriptButton.UseVisualStyleBackColor = true;
            this.scriptButton.Click += new System.EventHandler(this.scriptButton_Click);
            // 
            // scriptDirText
            // 
            this.scriptDirText.Location = new System.Drawing.Point(10, 224);
            this.scriptDirText.Name = "scriptDirText";
            this.scriptDirText.Size = new System.Drawing.Size(313, 20);
            this.scriptDirText.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 208);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Script Path";
            // 
            // gameClientPath_Button
            // 
            this.gameClientPath_Button.Location = new System.Drawing.Point(329, 166);
            this.gameClientPath_Button.Name = "gameClientPath_Button";
            this.gameClientPath_Button.Size = new System.Drawing.Size(75, 23);
            this.gameClientPath_Button.TabIndex = 10;
            this.gameClientPath_Button.Text = "Browse";
            this.gameClientPath_Button.UseVisualStyleBackColor = true;
            this.gameClientPath_Button.Click += new System.EventHandler(this.gameClientPath_Button_Click);
            // 
            // gameClientPath_TextBox
            // 
            this.gameClientPath_TextBox.Location = new System.Drawing.Point(10, 169);
            this.gameClientPath_TextBox.Name = "gameClientPath_TextBox";
            this.gameClientPath_TextBox.Size = new System.Drawing.Size(313, 20);
            this.gameClientPath_TextBox.TabIndex = 9;
            this.gameClientPath_TextBox.Click += new System.EventHandler(this.gameClientPath_Button_Click);
            // 
            // gameClientPath_Label
            // 
            this.gameClientPath_Label.AutoSize = true;
            this.gameClientPath_Label.Location = new System.Drawing.Point(7, 153);
            this.gameClientPath_Label.Name = "gameClientPath_Label";
            this.gameClientPath_Label.Size = new System.Drawing.Size(58, 13);
            this.gameClientPath_Label.TabIndex = 8;
            this.gameClientPath_Label.Text = "Client Path";
            // 
            // dataDir_Button
            // 
            this.dataDir_Button.Location = new System.Drawing.Point(329, 112);
            this.dataDir_Button.Name = "dataDir_Button";
            this.dataDir_Button.Size = new System.Drawing.Size(75, 23);
            this.dataDir_Button.TabIndex = 7;
            this.dataDir_Button.Text = "Browse";
            this.dataDir_Button.UseVisualStyleBackColor = true;
            this.dataDir_Button.Click += new System.EventHandler(this.dataDir_Button_Clicked);
            // 
            // dataDir_TextBox
            // 
            this.dataDir_TextBox.Location = new System.Drawing.Point(10, 115);
            this.dataDir_TextBox.Name = "dataDir_TextBox";
            this.dataDir_TextBox.Size = new System.Drawing.Size(313, 20);
            this.dataDir_TextBox.TabIndex = 6;
            this.dataDir_TextBox.Click += new System.EventHandler(this.dataDir_Button_Clicked);
            // 
            // dataDir_CheckBox
            // 
            this.dataDir_CheckBox.AutoSize = true;
            this.dataDir_CheckBox.Checked = true;
            this.dataDir_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dataDir_CheckBox.Location = new System.Drawing.Point(10, 92);
            this.dataDir_CheckBox.Name = "dataDir_CheckBox";
            this.dataDir_CheckBox.Size = new System.Drawing.Size(72, 17);
            this.dataDir_CheckBox.TabIndex = 5;
            this.dataDir_CheckBox.Text = "As Above";
            this.dataDir_CheckBox.UseVisualStyleBackColor = true;
            this.dataDir_CheckBox.CheckedChanged += new System.EventHandler(this.dataDirCheckBox_CheckedChanged);
            // 
            // dataDir_Label
            // 
            this.dataDir_Label.AutoSize = true;
            this.dataDir_Label.Location = new System.Drawing.Point(7, 75);
            this.dataDir_Label.Name = "dataDir_Label";
            this.dataDir_Label.Size = new System.Drawing.Size(374, 13);
            this.dataDir_Label.TabIndex = 4;
            this.dataDir_Label.Text = "Root Data Directory (this should be the dir that contains data && data_common)";
            // 
            // hglDir_Label
            // 
            this.hglDir_Label.AutoSize = true;
            this.hglDir_Label.Location = new System.Drawing.Point(7, 20);
            this.hglDir_Label.Name = "hglDir_Label";
            this.hglDir_Label.Size = new System.Drawing.Size(186, 13);
            this.hglDir_Label.TabIndex = 3;
            this.hglDir_Label.Text = "Hellgate: London Installation Directory";
            // 
            // hglDir_Button
            // 
            this.hglDir_Button.Location = new System.Drawing.Point(329, 33);
            this.hglDir_Button.Name = "hglDir_Button";
            this.hglDir_Button.Size = new System.Drawing.Size(75, 23);
            this.hglDir_Button.TabIndex = 2;
            this.hglDir_Button.Text = "Browse";
            this.hglDir_Button.UseVisualStyleBackColor = true;
            this.hglDir_Button.Click += new System.EventHandler(this.hglDirBrowse_Click);
            // 
            // hglDir_TextBox
            // 
            this.hglDir_TextBox.Location = new System.Drawing.Point(10, 36);
            this.hglDir_TextBox.Name = "hglDir_TextBox";
            this.hglDir_TextBox.Size = new System.Drawing.Size(313, 20);
            this.hglDir_TextBox.TabIndex = 1;
            this.hglDir_TextBox.Click += new System.EventHandler(this.hglDirBrowse_Click);
            // 
            // ok_Button
            // 
            this.ok_Button.Location = new System.Drawing.Point(341, 268);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(75, 23);
            this.ok_Button.TabIndex = 1;
            this.ok_Button.Text = "Ok";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.okButton_Click);
            // 
            // Options
            // 
            this.AcceptButton = this.ok_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 302);
            this.ControlBox = false;
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.pathSettings_GroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Options";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.Options_Load);
            this.pathSettings_GroupBox.ResumeLayout(false);
            this.pathSettings_GroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox pathSettings_GroupBox;

        private System.Windows.Forms.Label hglDir_Label;
        private System.Windows.Forms.Button hglDir_Button;
        private System.Windows.Forms.TextBox hglDir_TextBox;

        private System.Windows.Forms.CheckBox dataDir_CheckBox;
        private System.Windows.Forms.Label dataDir_Label;
        private System.Windows.Forms.Button dataDir_Button;
        private System.Windows.Forms.TextBox dataDir_TextBox;

        private System.Windows.Forms.Label gameClientPath_Label;
        private System.Windows.Forms.TextBox gameClientPath_TextBox;
        private System.Windows.Forms.Button gameClientPath_Button;

        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Button scriptButton;
        private System.Windows.Forms.TextBox scriptDirText;
        private System.Windows.Forms.Label label1;

    }
}