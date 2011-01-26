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
            this._pathSettings_GroupBox = new System.Windows.Forms.GroupBox();
            this.scriptButton = new System.Windows.Forms.Button();
            this.scriptDirText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gameClientPath_Button = new System.Windows.Forms.Button();
            this.gameClientPath_TextBox = new System.Windows.Forms.TextBox();
            this.gameClientPath_Label = new System.Windows.Forms.Label();
            this.hglDir_Label = new System.Windows.Forms.Label();
            this.hglDir_Button = new System.Windows.Forms.Button();
            this.hglDir_TextBox = new System.Windows.Forms.TextBox();
            this._ok_button = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._tcv4_CheckBox = new System.Windows.Forms.CheckBox();
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
            this._stringsLang_comboBox = new System.Windows.Forms.ComboBox();
            this._pathSettings_GroupBox.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _pathSettings_GroupBox
            // 
            this._pathSettings_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._pathSettings_GroupBox.Controls.Add(this.scriptButton);
            this._pathSettings_GroupBox.Controls.Add(this.scriptDirText);
            this._pathSettings_GroupBox.Controls.Add(this.label1);
            this._pathSettings_GroupBox.Controls.Add(this.gameClientPath_Button);
            this._pathSettings_GroupBox.Controls.Add(this.gameClientPath_TextBox);
            this._pathSettings_GroupBox.Controls.Add(this.gameClientPath_Label);
            this._pathSettings_GroupBox.Controls.Add(this.hglDir_Label);
            this._pathSettings_GroupBox.Controls.Add(this.hglDir_Button);
            this._pathSettings_GroupBox.Controls.Add(this.hglDir_TextBox);
            this._pathSettings_GroupBox.Location = new System.Drawing.Point(7, 7);
            this._pathSettings_GroupBox.Name = "_pathSettings_GroupBox";
            this._pathSettings_GroupBox.Size = new System.Drawing.Size(519, 198);
            this._pathSettings_GroupBox.TabIndex = 0;
            this._pathSettings_GroupBox.TabStop = false;
            this._pathSettings_GroupBox.Text = "Path Settings";
            // 
            // scriptButton
            // 
            this.scriptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.scriptButton.Location = new System.Drawing.Point(426, 165);
            this.scriptButton.Name = "scriptButton";
            this.scriptButton.Size = new System.Drawing.Size(87, 27);
            this.scriptButton.TabIndex = 13;
            this.scriptButton.Text = "Browse";
            this.scriptButton.UseVisualStyleBackColor = true;
            this.scriptButton.Click += new System.EventHandler(this._ScriptButton_Click);
            // 
            // scriptDirText
            // 
            this.scriptDirText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.scriptDirText.Location = new System.Drawing.Point(12, 165);
            this.scriptDirText.Name = "scriptDirText";
            this.scriptDirText.Size = new System.Drawing.Size(408, 23);
            this.scriptDirText.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 147);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 15);
            this.label1.TabIndex = 11;
            this.label1.Text = "Mod Path";
            // 
            // gameClientPath_Button
            // 
            this.gameClientPath_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gameClientPath_Button.Location = new System.Drawing.Point(426, 99);
            this.gameClientPath_Button.Name = "gameClientPath_Button";
            this.gameClientPath_Button.Size = new System.Drawing.Size(87, 27);
            this.gameClientPath_Button.TabIndex = 10;
            this.gameClientPath_Button.Text = "Browse";
            this.gameClientPath_Button.UseVisualStyleBackColor = true;
            this.gameClientPath_Button.Click += new System.EventHandler(this._GameClientPath_Button_Click);
            // 
            // gameClientPath_TextBox
            // 
            this.gameClientPath_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gameClientPath_TextBox.Location = new System.Drawing.Point(12, 102);
            this.gameClientPath_TextBox.Name = "gameClientPath_TextBox";
            this.gameClientPath_TextBox.Size = new System.Drawing.Size(408, 23);
            this.gameClientPath_TextBox.TabIndex = 9;
            this.gameClientPath_TextBox.Click += new System.EventHandler(this._GameClientPath_Button_Click);
            // 
            // gameClientPath_Label
            // 
            this.gameClientPath_Label.AutoSize = true;
            this.gameClientPath_Label.Location = new System.Drawing.Point(8, 83);
            this.gameClientPath_Label.Name = "gameClientPath_Label";
            this.gameClientPath_Label.Size = new System.Drawing.Size(65, 15);
            this.gameClientPath_Label.TabIndex = 8;
            this.gameClientPath_Label.Text = "Client Path";
            // 
            // hglDir_Label
            // 
            this.hglDir_Label.AutoSize = true;
            this.hglDir_Label.Location = new System.Drawing.Point(8, 23);
            this.hglDir_Label.Name = "hglDir_Label";
            this.hglDir_Label.Size = new System.Drawing.Size(324, 15);
            this.hglDir_Label.TabIndex = 3;
            this.hglDir_Label.Text = "Hellgate Installation Directory (requires restart to take effect)";
            // 
            // hglDir_Button
            // 
            this.hglDir_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.hglDir_Button.Location = new System.Drawing.Point(426, 38);
            this.hglDir_Button.Name = "hglDir_Button";
            this.hglDir_Button.Size = new System.Drawing.Size(87, 27);
            this.hglDir_Button.TabIndex = 2;
            this.hglDir_Button.Text = "Browse";
            this.hglDir_Button.UseVisualStyleBackColor = true;
            this.hglDir_Button.Click += new System.EventHandler(this._HglDirBrowse_Click);
            // 
            // hglDir_TextBox
            // 
            this.hglDir_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.hglDir_TextBox.Location = new System.Drawing.Point(12, 42);
            this.hglDir_TextBox.Name = "hglDir_TextBox";
            this.hglDir_TextBox.Size = new System.Drawing.Size(408, 23);
            this.hglDir_TextBox.TabIndex = 1;
            this.hglDir_TextBox.Click += new System.EventHandler(this._HglDirBrowse_Click);
            // 
            // _ok_button
            // 
            this._ok_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._ok_button.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ok_button.Location = new System.Drawing.Point(441, 442);
            this._ok_button.Name = "_ok_button";
            this._ok_button.Size = new System.Drawing.Size(87, 27);
            this._ok_button.TabIndex = 1;
            this._ok_button.Text = "Ok";
            this._ok_button.UseVisualStyleBackColor = true;
            this._ok_button.Click += new System.EventHandler(this._OkButton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(540, 423);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this._pathSettings_GroupBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(532, 395);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this._stringsLang_comboBox);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(7, 311);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(519, 78);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Language Files";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(211, 37);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(225, 30);
            this.label7.TabIndex = 2;
            this.label7.Text = "(language availablity is dependant on\r\nhellgate path and data files being loaded)" +
                "";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(336, 15);
            this.label6.TabIndex = 0;
            this.label6.Text = "Strings Files Language Selection (requires restart to take effect)";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this._tcv4_CheckBox);
            this.groupBox1.Location = new System.Drawing.Point(5, 213);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(521, 91);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Advanced";
            // 
            // _tcv4_CheckBox
            // 
            this._tcv4_CheckBox.AutoSize = true;
            this._tcv4_CheckBox.Location = new System.Drawing.Point(8, 23);
            this._tcv4_CheckBox.Name = "_tcv4_CheckBox";
            this._tcv4_CheckBox.Size = new System.Drawing.Size(361, 19);
            this._tcv4_CheckBox.TabIndex = 1;
            this._tcv4_CheckBox.Text = "Load TCv4 Excel and Strings files. (requires restart to take effect)\r\n";
            this._tcv4_CheckBox.UseVisualStyleBackColor = true;
            this._tcv4_CheckBox.CheckedChanged += new System.EventHandler(this._TCv4_CheckBox_CheckedChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.relationsCheck);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.intPtrTypeCombo);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(532, 395);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Display";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // relationsCheck
            // 
            this.relationsCheck.AutoSize = true;
            this.relationsCheck.Location = new System.Drawing.Point(24, 54);
            this.relationsCheck.Name = "relationsCheck";
            this.relationsCheck.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.relationsCheck.Size = new System.Drawing.Size(124, 19);
            this.relationsCheck.TabIndex = 2;
            this.relationsCheck.Text = "Generate Relations";
            this.relationsCheck.UseVisualStyleBackColor = true;
            this.relationsCheck.CheckedChanged += new System.EventHandler(this._RelationsCheck_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 15);
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
            this.intPtrTypeCombo.Location = new System.Drawing.Point(145, 23);
            this.intPtrTypeCombo.Name = "intPtrTypeCombo";
            this.intPtrTypeCombo.Size = new System.Drawing.Size(140, 23);
            this.intPtrTypeCombo.TabIndex = 0;
            this.intPtrTypeCombo.SelectedIndexChanged += new System.EventHandler(this._IntPtrTypeCombo_SelectedIndexChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tableLayoutPanel1);
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(532, 395);
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
            this.tableLayoutPanel1.Location = new System.Drawing.Point(7, 7);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(495, 100);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // txtEditor_Button
            // 
            this.txtEditor_Button.Location = new System.Drawing.Point(404, 3);
            this.txtEditor_Button.Name = "txtEditor_Button";
            this.txtEditor_Button.Size = new System.Drawing.Size(80, 27);
            this.txtEditor_Button.TabIndex = 10;
            this.txtEditor_Button.Text = "Browse";
            this.txtEditor_Button.UseVisualStyleBackColor = true;
            this.txtEditor_Button.Click += new System.EventHandler(this._TxtEditor_Button_Click);
            // 
            // csvEditor_Button
            // 
            this.csvEditor_Button.Location = new System.Drawing.Point(404, 69);
            this.csvEditor_Button.Name = "csvEditor_Button";
            this.csvEditor_Button.Size = new System.Drawing.Size(80, 27);
            this.csvEditor_Button.TabIndex = 9;
            this.csvEditor_Button.Text = "Browse";
            this.csvEditor_Button.UseVisualStyleBackColor = true;
            this.csvEditor_Button.Click += new System.EventHandler(this._CsvEditor_Button_Click);
            // 
            // xmlEditor_Button
            // 
            this.xmlEditor_Button.Location = new System.Drawing.Point(404, 36);
            this.xmlEditor_Button.Name = "xmlEditor_Button";
            this.xmlEditor_Button.Size = new System.Drawing.Size(80, 27);
            this.xmlEditor_Button.TabIndex = 8;
            this.xmlEditor_Button.Text = "Browse";
            this.xmlEditor_Button.UseVisualStyleBackColor = true;
            this.xmlEditor_Button.Click += new System.EventHandler(this._XmlEditor_Button_Click);
            // 
            // xmlEditor_TextBox
            // 
            this.xmlEditor_TextBox.Location = new System.Drawing.Point(74, 36);
            this.xmlEditor_TextBox.Name = "xmlEditor_TextBox";
            this.xmlEditor_TextBox.Size = new System.Drawing.Size(324, 23);
            this.xmlEditor_TextBox.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 34);
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
            this.label3.Size = new System.Drawing.Size(65, 33);
            this.label3.TabIndex = 1;
            this.label3.Text = "Text Editor";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 33);
            this.label4.TabIndex = 2;
            this.label4.Text = "XML Editor";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtEditor_TextBox
            // 
            this.txtEditor_TextBox.Location = new System.Drawing.Point(74, 3);
            this.txtEditor_TextBox.Name = "txtEditor_TextBox";
            this.txtEditor_TextBox.Size = new System.Drawing.Size(324, 23);
            this.txtEditor_TextBox.TabIndex = 4;
            // 
            // csvEditor_TextBox
            // 
            this.csvEditor_TextBox.Location = new System.Drawing.Point(74, 69);
            this.csvEditor_TextBox.Name = "csvEditor_TextBox";
            this.csvEditor_TextBox.Size = new System.Drawing.Size(324, 23);
            this.csvEditor_TextBox.TabIndex = 6;
            // 
            // _stringsLang_comboBox
            // 
            this._stringsLang_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._stringsLang_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._stringsLang_comboBox.FormattingEnabled = true;
            this._stringsLang_comboBox.Location = new System.Drawing.Point(6, 38);
            this._stringsLang_comboBox.Name = "_stringsLang_comboBox";
            this._stringsLang_comboBox.Size = new System.Drawing.Size(199, 23);
            this._stringsLang_comboBox.TabIndex = 3;
            this._stringsLang_comboBox.SelectedIndexChanged += new System.EventHandler(this._StringsLang_ComboBox_SelectedIndexChanged);
            // 
            // Options
            // 
            this.AcceptButton = this._ok_button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 481);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this._ok_button);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Options";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.Options_Load);
            this._pathSettings_GroupBox.ResumeLayout(false);
            this._pathSettings_GroupBox.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox _pathSettings_GroupBox;

        private System.Windows.Forms.Label hglDir_Label;
        private System.Windows.Forms.Button hglDir_Button;
        private System.Windows.Forms.TextBox hglDir_TextBox;

        private System.Windows.Forms.Label gameClientPath_Label;
        private System.Windows.Forms.TextBox gameClientPath_TextBox;
        private System.Windows.Forms.Button gameClientPath_Button;

        private System.Windows.Forms.Button _ok_button;
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
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox _tcv4_CheckBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox _stringsLang_comboBox;

    }
}