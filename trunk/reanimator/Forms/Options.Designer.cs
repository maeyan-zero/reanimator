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
            this.hglDir_Label = new System.Windows.Forms.Label();
            this.hglDir_Button = new System.Windows.Forms.Button();
            this.hglDir_TextBox = new System.Windows.Forms.TextBox();
            this.ok_Button = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.relationsCheck = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.intPtrTypeCombo = new System.Windows.Forms.ComboBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtEditor_Button = new System.Windows.Forms.Button();
            this.csvEditor_Button = new System.Windows.Forms.Button();
            this.xmlEditor_Button = new System.Windows.Forms.Button();
            this.xmlEditor_TextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtEditor_TextBox = new System.Windows.Forms.TextBox();
            this.csvEditor_TextBox = new System.Windows.Forms.TextBox();
            this.pathSettings_GroupBox.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
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
            this.pathSettings_GroupBox.Controls.Add(this.hglDir_Label);
            this.pathSettings_GroupBox.Controls.Add(this.hglDir_Button);
            this.pathSettings_GroupBox.Controls.Add(this.hglDir_TextBox);
            this.pathSettings_GroupBox.Location = new System.Drawing.Point(6, 6);
            this.pathSettings_GroupBox.Name = "pathSettings_GroupBox";
            this.pathSettings_GroupBox.Size = new System.Drawing.Size(419, 258);
            this.pathSettings_GroupBox.TabIndex = 0;
            this.pathSettings_GroupBox.TabStop = false;
            this.pathSettings_GroupBox.Text = "Path Settings";
            // 
            // scriptButton
            // 
            this.scriptButton.Location = new System.Drawing.Point(329, 140);
            this.scriptButton.Name = "scriptButton";
            this.scriptButton.Size = new System.Drawing.Size(75, 23);
            this.scriptButton.TabIndex = 13;
            this.scriptButton.Text = "Browse";
            this.scriptButton.UseVisualStyleBackColor = true;
            this.scriptButton.Click += new System.EventHandler(this._ScriptButton_Click);
            // 
            // scriptDirText
            // 
            this.scriptDirText.Location = new System.Drawing.Point(10, 143);
            this.scriptDirText.Name = "scriptDirText";
            this.scriptDirText.Size = new System.Drawing.Size(313, 20);
            this.scriptDirText.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 127);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Mod Path";
            // 
            // gameClientPath_Button
            // 
            this.gameClientPath_Button.Location = new System.Drawing.Point(329, 85);
            this.gameClientPath_Button.Name = "gameClientPath_Button";
            this.gameClientPath_Button.Size = new System.Drawing.Size(75, 23);
            this.gameClientPath_Button.TabIndex = 10;
            this.gameClientPath_Button.Text = "Browse";
            this.gameClientPath_Button.UseVisualStyleBackColor = true;
            this.gameClientPath_Button.Click += new System.EventHandler(this._GameClientPath_Button_Click);
            // 
            // gameClientPath_TextBox
            // 
            this.gameClientPath_TextBox.Location = new System.Drawing.Point(10, 88);
            this.gameClientPath_TextBox.Name = "gameClientPath_TextBox";
            this.gameClientPath_TextBox.Size = new System.Drawing.Size(313, 20);
            this.gameClientPath_TextBox.TabIndex = 9;
            this.gameClientPath_TextBox.Click += new System.EventHandler(this._GameClientPath_Button_Click);
            // 
            // gameClientPath_Label
            // 
            this.gameClientPath_Label.AutoSize = true;
            this.gameClientPath_Label.Location = new System.Drawing.Point(7, 72);
            this.gameClientPath_Label.Name = "gameClientPath_Label";
            this.gameClientPath_Label.Size = new System.Drawing.Size(58, 13);
            this.gameClientPath_Label.TabIndex = 8;
            this.gameClientPath_Label.Text = "Client Path";
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
            this.hglDir_Button.Click += new System.EventHandler(this._HglDirBrowse_Click);
            // 
            // hglDir_TextBox
            // 
            this.hglDir_TextBox.Location = new System.Drawing.Point(10, 36);
            this.hglDir_TextBox.Name = "hglDir_TextBox";
            this.hglDir_TextBox.Size = new System.Drawing.Size(313, 20);
            this.hglDir_TextBox.TabIndex = 1;
            this.hglDir_TextBox.Click += new System.EventHandler(this._HglDirBrowse_Click);
            // 
            // ok_Button
            // 
            this.ok_Button.Location = new System.Drawing.Point(351, 314);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(75, 23);
            this.ok_Button.TabIndex = 1;
            this.ok_Button.Text = "Ok";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this._OkButton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(444, 296);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pathSettings_GroupBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(436, 270);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Paths";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.relationsCheck);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.intPtrTypeCombo);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(436, 270);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Display";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // relationsCheck
            // 
            this.relationsCheck.AutoSize = true;
            this.relationsCheck.Location = new System.Drawing.Point(21, 47);
            this.relationsCheck.Name = "relationsCheck";
            this.relationsCheck.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.relationsCheck.Size = new System.Drawing.Size(117, 17);
            this.relationsCheck.TabIndex = 2;
            this.relationsCheck.Text = "Generate Relations";
            this.relationsCheck.UseVisualStyleBackColor = true;
            this.relationsCheck.CheckedChanged += new System.EventHandler(this._RelationsCheck_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Cast IntPtr data as";
            // 
            // intPtrTypeCombo
            // 
            this.intPtrTypeCombo.FormattingEnabled = true;
            this.intPtrTypeCombo.Items.AddRange(new object[] {
            "hex",
            "signed",
            "unsigned"});
            this.intPtrTypeCombo.Location = new System.Drawing.Point(124, 20);
            this.intPtrTypeCombo.Name = "intPtrTypeCombo";
            this.intPtrTypeCombo.Size = new System.Drawing.Size(121, 21);
            this.intPtrTypeCombo.TabIndex = 0;
            this.intPtrTypeCombo.SelectedIndexChanged += new System.EventHandler(this._IntPtrTypeCombo_SelectedIndexChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tableLayoutPanel1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(436, 270);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Default Programs";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.txtEditor_Button, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.csvEditor_Button, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.xmlEditor_Button, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.xmlEditor_TextBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtEditor_TextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.csvEditor_TextBox, 1, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(424, 87);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // txtEditor_Button
            // 
            this.txtEditor_Button.Location = new System.Drawing.Point(352, 3);
            this.txtEditor_Button.Name = "txtEditor_Button";
            this.txtEditor_Button.Size = new System.Drawing.Size(69, 23);
            this.txtEditor_Button.TabIndex = 10;
            this.txtEditor_Button.Text = "Browse";
            this.txtEditor_Button.UseVisualStyleBackColor = true;
            this.txtEditor_Button.Click += new System.EventHandler(this._TxtEditor_Button_Click);
            // 
            // csvEditor_Button
            // 
            this.csvEditor_Button.Location = new System.Drawing.Point(352, 61);
            this.csvEditor_Button.Name = "csvEditor_Button";
            this.csvEditor_Button.Size = new System.Drawing.Size(69, 23);
            this.csvEditor_Button.TabIndex = 9;
            this.csvEditor_Button.Text = "Browse";
            this.csvEditor_Button.UseVisualStyleBackColor = true;
            this.csvEditor_Button.Click += new System.EventHandler(this._CsvEditor_Button_Click);
            // 
            // xmlEditor_Button
            // 
            this.xmlEditor_Button.Location = new System.Drawing.Point(352, 32);
            this.xmlEditor_Button.Name = "xmlEditor_Button";
            this.xmlEditor_Button.Size = new System.Drawing.Size(69, 23);
            this.xmlEditor_Button.TabIndex = 8;
            this.xmlEditor_Button.Text = "Browse";
            this.xmlEditor_Button.UseVisualStyleBackColor = true;
            this.xmlEditor_Button.Click += new System.EventHandler(this._XmlEditor_Button_Click);
            // 
            // xmlEditor_TextBox
            // 
            this.xmlEditor_TextBox.Location = new System.Drawing.Point(68, 32);
            this.xmlEditor_TextBox.Name = "xmlEditor_TextBox";
            this.xmlEditor_TextBox.Size = new System.Drawing.Size(278, 20);
            this.xmlEditor_TextBox.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 29);
            this.label5.TabIndex = 3;
            this.label5.Text = "CSV Editor";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 29);
            this.label3.TabIndex = 1;
            this.label3.Text = "Text Editor";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 29);
            this.label4.TabIndex = 2;
            this.label4.Text = "XML Editor";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtEditor_TextBox
            // 
            this.txtEditor_TextBox.Location = new System.Drawing.Point(68, 3);
            this.txtEditor_TextBox.Name = "txtEditor_TextBox";
            this.txtEditor_TextBox.Size = new System.Drawing.Size(278, 20);
            this.txtEditor_TextBox.TabIndex = 4;
            // 
            // csvEditor_TextBox
            // 
            this.csvEditor_TextBox.Location = new System.Drawing.Point(68, 61);
            this.csvEditor_TextBox.Name = "csvEditor_TextBox";
            this.csvEditor_TextBox.Size = new System.Drawing.Size(278, 20);
            this.csvEditor_TextBox.TabIndex = 6;
            // 
            // Options
            // 
            this.AcceptButton = this.ok_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 344);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.ok_Button);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Options";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.Options_Load);
            this.pathSettings_GroupBox.ResumeLayout(false);
            this.pathSettings_GroupBox.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox pathSettings_GroupBox;

        private System.Windows.Forms.Label hglDir_Label;
        private System.Windows.Forms.Button hglDir_Button;
        private System.Windows.Forms.TextBox hglDir_TextBox;

        private System.Windows.Forms.Label gameClientPath_Label;
        private System.Windows.Forms.TextBox gameClientPath_TextBox;
        private System.Windows.Forms.Button gameClientPath_Button;

        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Button scriptButton;
        private System.Windows.Forms.TextBox scriptDirText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox intPtrTypeCombo;
        private System.Windows.Forms.CheckBox relationsCheck;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox csvEditor_TextBox;
        private System.Windows.Forms.TextBox xmlEditor_TextBox;
        private System.Windows.Forms.TextBox txtEditor_TextBox;
        private System.Windows.Forms.Button csvEditor_Button;
        private System.Windows.Forms.Button xmlEditor_Button;
        private System.Windows.Forms.Button txtEditor_Button;

    }
}