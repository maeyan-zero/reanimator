using System;
namespace Reanimator.Forms
{
    partial class HeroEditor
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
            this.main_TabControl = new System.Windows.Forms.TabControl();
            this.general_TabPage = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.statPoints_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.skillPoints_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.palladium_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dead_CheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.level_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.mode_Label = new System.Windows.Forms.Label();
            this.hardcore_CheckBox = new System.Windows.Forms.CheckBox();
            this.elite_CheckBox = new System.Windows.Forms.CheckBox();
            this.level_Label = new System.Windows.Forms.Label();
            this.class_TextBox = new System.Windows.Forms.TextBox();
            this.class_Label = new System.Windows.Forms.Label();
            this.name_TextBox = new System.Windows.Forms.TextBox();
            this.name_Label = new System.Windows.Forms.Label();
            this.stats_TabPage = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.stats_GroupBox = new System.Windows.Forms.GroupBox();
            this.stats_ListBox = new System.Windows.Forms.ListBox();
            this.items_TabPage = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.items_ListBox = new System.Windows.Forms.ListBox();
            this.save_Button = new System.Windows.Forms.Button();
            this.currentlyEditing_Label = new System.Windows.Forms.Label();
            this.currentlyEditing_ComboBox = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.main_TabControl.SuspendLayout();
            this.general_TabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statPoints_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.skillPoints_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.palladium_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.level_NumericUpDown)).BeginInit();
            this.stats_TabPage.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.stats_GroupBox.SuspendLayout();
            this.items_TabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // main_TabControl
            // 
            this.main_TabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.main_TabControl.Controls.Add(this.general_TabPage);
            this.main_TabControl.Controls.Add(this.stats_TabPage);
            this.main_TabControl.Controls.Add(this.items_TabPage);
            this.main_TabControl.Location = new System.Drawing.Point(12, 33);
            this.main_TabControl.Name = "main_TabControl";
            this.main_TabControl.SelectedIndex = 0;
            this.main_TabControl.Size = new System.Drawing.Size(751, 535);
            this.main_TabControl.TabIndex = 0;
            // 
            // general_TabPage
            // 
            this.general_TabPage.Controls.Add(this.label6);
            this.general_TabPage.Controls.Add(this.statPoints_numericUpDown);
            this.general_TabPage.Controls.Add(this.label5);
            this.general_TabPage.Controls.Add(this.skillPoints_numericUpDown);
            this.general_TabPage.Controls.Add(this.palladium_numericUpDown);
            this.general_TabPage.Controls.Add(this.label4);
            this.general_TabPage.Controls.Add(this.button2);
            this.general_TabPage.Controls.Add(this.richTextBox1);
            this.general_TabPage.Controls.Add(this.label3);
            this.general_TabPage.Controls.Add(this.dead_CheckBox);
            this.general_TabPage.Controls.Add(this.label2);
            this.general_TabPage.Controls.Add(this.textBox1);
            this.general_TabPage.Controls.Add(this.level_NumericUpDown);
            this.general_TabPage.Controls.Add(this.mode_Label);
            this.general_TabPage.Controls.Add(this.hardcore_CheckBox);
            this.general_TabPage.Controls.Add(this.elite_CheckBox);
            this.general_TabPage.Controls.Add(this.level_Label);
            this.general_TabPage.Controls.Add(this.class_TextBox);
            this.general_TabPage.Controls.Add(this.class_Label);
            this.general_TabPage.Controls.Add(this.name_TextBox);
            this.general_TabPage.Controls.Add(this.name_Label);
            this.general_TabPage.Location = new System.Drawing.Point(4, 22);
            this.general_TabPage.Name = "general_TabPage";
            this.general_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.general_TabPage.Size = new System.Drawing.Size(743, 509);
            this.general_TabPage.TabIndex = 2;
            this.general_TabPage.Text = "General";
            this.general_TabPage.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(359, 106);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Statpoints:";
            // 
            // statPoints_numericUpDown
            // 
            this.statPoints_numericUpDown.Location = new System.Drawing.Point(422, 104);
            this.statPoints_numericUpDown.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.statPoints_numericUpDown.Name = "statPoints_numericUpDown";
            this.statPoints_numericUpDown.Size = new System.Drawing.Size(100, 20);
            this.statPoints_numericUpDown.TabIndex = 21;
            this.statPoints_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.statPoints_numericUpDown.ThousandsSeparator = true;
            this.statPoints_numericUpDown.ValueChanged += new System.EventHandler(this.statPoints_numericUpDown_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(359, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Skillpoints:";
            // 
            // skillPoints_numericUpDown
            // 
            this.skillPoints_numericUpDown.Location = new System.Drawing.Point(422, 78);
            this.skillPoints_numericUpDown.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.skillPoints_numericUpDown.Name = "skillPoints_numericUpDown";
            this.skillPoints_numericUpDown.Size = new System.Drawing.Size(100, 20);
            this.skillPoints_numericUpDown.TabIndex = 19;
            this.skillPoints_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.skillPoints_numericUpDown.ThousandsSeparator = true;
            this.skillPoints_numericUpDown.ValueChanged += new System.EventHandler(this.skillPoints_numericUpDown_ValueChanged);
            // 
            // palladium_numericUpDown
            // 
            this.palladium_numericUpDown.Location = new System.Drawing.Point(253, 57);
            this.palladium_numericUpDown.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.palladium_numericUpDown.Name = "palladium_numericUpDown";
            this.palladium_numericUpDown.Size = new System.Drawing.Size(100, 20);
            this.palladium_numericUpDown.TabIndex = 18;
            this.palladium_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.palladium_numericUpDown.ThousandsSeparator = true;
            this.palladium_numericUpDown.ValueChanged += new System.EventHandler(this.palladium_numericUpDown_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(192, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Palladium:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(195, 101);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(106, 23);
            this.button2.TabIndex = 15;
            this.button2.Text = "Update Flags";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(6, 130);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(305, 373);
            this.richTextBox1.TabIndex = 14;
            this.richTextBox1.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(359, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(213, 26);
            this.label3.TabIndex = 13;
            this.label3.Text = "=> JobClass variable != Character Class as\r\nit changes with the character appeara" +
                "nce...";
            // 
            // dead_CheckBox
            // 
            this.dead_CheckBox.AutoSize = true;
            this.dead_CheckBox.Enabled = false;
            this.dead_CheckBox.Location = new System.Drawing.Point(134, 107);
            this.dead_CheckBox.Name = "dead_CheckBox";
            this.dead_CheckBox.Size = new System.Drawing.Size(52, 17);
            this.dead_CheckBox.TabIndex = 12;
            this.dead_CheckBox.Text = "Dead";
            this.dead_CheckBox.UseVisualStyleBackColor = true;
            this.dead_CheckBox.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(192, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Job bits";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(253, 30);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 10;
            // 
            // level_NumericUpDown
            // 
            this.level_NumericUpDown.Location = new System.Drawing.Point(54, 56);
            this.level_NumericUpDown.Maximum = new decimal(new int[] {
            55,
            0,
            0,
            0});
            this.level_NumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.level_NumericUpDown.Name = "level_NumericUpDown";
            this.level_NumericUpDown.Size = new System.Drawing.Size(132, 20);
            this.level_NumericUpDown.TabIndex = 9;
            this.level_NumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.level_NumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.level_NumericUpDown.ValueChanged += new System.EventHandler(this.level_NumericUpDown_ValueChanged);
            // 
            // mode_Label
            // 
            this.mode_Label.AutoSize = true;
            this.mode_Label.Location = new System.Drawing.Point(7, 85);
            this.mode_Label.Name = "mode_Label";
            this.mode_Label.Size = new System.Drawing.Size(37, 13);
            this.mode_Label.TabIndex = 8;
            this.mode_Label.Text = "Mode:";
            // 
            // hardcore_CheckBox
            // 
            this.hardcore_CheckBox.AutoSize = true;
            this.hardcore_CheckBox.Location = new System.Drawing.Point(54, 107);
            this.hardcore_CheckBox.Name = "hardcore_CheckBox";
            this.hardcore_CheckBox.Size = new System.Drawing.Size(70, 17);
            this.hardcore_CheckBox.TabIndex = 7;
            this.hardcore_CheckBox.Text = "Hardcore";
            this.hardcore_CheckBox.UseVisualStyleBackColor = true;
            this.hardcore_CheckBox.CheckedChanged += new System.EventHandler(this.hardcore_CheckBox_CheckedChanged);
            // 
            // elite_CheckBox
            // 
            this.elite_CheckBox.AutoSize = true;
            this.elite_CheckBox.Location = new System.Drawing.Point(54, 84);
            this.elite_CheckBox.Name = "elite_CheckBox";
            this.elite_CheckBox.Size = new System.Drawing.Size(46, 17);
            this.elite_CheckBox.TabIndex = 6;
            this.elite_CheckBox.Text = "Elite";
            this.elite_CheckBox.UseVisualStyleBackColor = true;
            this.elite_CheckBox.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // level_Label
            // 
            this.level_Label.AutoSize = true;
            this.level_Label.Location = new System.Drawing.Point(7, 59);
            this.level_Label.Name = "level_Label";
            this.level_Label.Size = new System.Drawing.Size(39, 13);
            this.level_Label.TabIndex = 4;
            this.level_Label.Text = "Level: ";
            // 
            // class_TextBox
            // 
            this.class_TextBox.Location = new System.Drawing.Point(54, 30);
            this.class_TextBox.Name = "class_TextBox";
            this.class_TextBox.Size = new System.Drawing.Size(132, 20);
            this.class_TextBox.TabIndex = 3;
            // 
            // class_Label
            // 
            this.class_Label.AutoSize = true;
            this.class_Label.Location = new System.Drawing.Point(7, 33);
            this.class_Label.Name = "class_Label";
            this.class_Label.Size = new System.Drawing.Size(38, 13);
            this.class_Label.TabIndex = 2;
            this.class_Label.Text = "Class: ";
            // 
            // name_TextBox
            // 
            this.name_TextBox.Location = new System.Drawing.Point(54, 4);
            this.name_TextBox.Name = "name_TextBox";
            this.name_TextBox.Size = new System.Drawing.Size(132, 20);
            this.name_TextBox.TabIndex = 1;
            // 
            // name_Label
            // 
            this.name_Label.AutoSize = true;
            this.name_Label.Location = new System.Drawing.Point(7, 7);
            this.name_Label.Name = "name_Label";
            this.name_Label.Size = new System.Drawing.Size(41, 13);
            this.name_Label.TabIndex = 0;
            this.name_Label.Text = "Name: ";
            // 
            // stats_TabPage
            // 
            this.stats_TabPage.Controls.Add(this.groupBox2);
            this.stats_TabPage.Controls.Add(this.groupBox1);
            this.stats_TabPage.Controls.Add(this.stats_GroupBox);
            this.stats_TabPage.Location = new System.Drawing.Point(4, 22);
            this.stats_TabPage.Name = "stats_TabPage";
            this.stats_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.stats_TabPage.Size = new System.Drawing.Size(743, 509);
            this.stats_TabPage.TabIndex = 0;
            this.stats_TabPage.Text = "Stats";
            this.stats_TabPage.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Location = new System.Drawing.Point(201, 147);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(221, 354);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Stat Values";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Location = new System.Drawing.Point(7, 20);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(208, 328);
            this.panel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(200, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(535, 134);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Stat Attributes";
            // 
            // stats_GroupBox
            // 
            this.stats_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.stats_GroupBox.Controls.Add(this.stats_ListBox);
            this.stats_GroupBox.Location = new System.Drawing.Point(8, 6);
            this.stats_GroupBox.Name = "stats_GroupBox";
            this.stats_GroupBox.Size = new System.Drawing.Size(186, 495);
            this.stats_GroupBox.TabIndex = 2;
            this.stats_GroupBox.TabStop = false;
            this.stats_GroupBox.Text = "Stats";
            // 
            // stats_ListBox
            // 
            this.stats_ListBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.stats_ListBox.FormattingEnabled = true;
            this.stats_ListBox.Location = new System.Drawing.Point(3, 16);
            this.stats_ListBox.Name = "stats_ListBox";
            this.stats_ListBox.Size = new System.Drawing.Size(177, 472);
            this.stats_ListBox.TabIndex = 1;
            this.stats_ListBox.SelectedIndexChanged += new System.EventHandler(this.charStats_ListBox_SelectedIndexChanged);
            // 
            // items_TabPage
            // 
            this.items_TabPage.Controls.Add(this.label1);
            this.items_TabPage.Controls.Add(this.items_ListBox);
            this.items_TabPage.Location = new System.Drawing.Point(4, 22);
            this.items_TabPage.Name = "items_TabPage";
            this.items_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.items_TabPage.Size = new System.Drawing.Size(743, 509);
            this.items_TabPage.TabIndex = 1;
            this.items_TabPage.Text = "Items";
            this.items_TabPage.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(213, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(253, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Ignore this section for now. Select items from above.";
            // 
            // items_ListBox
            // 
            this.items_ListBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.items_ListBox.FormattingEnabled = true;
            this.items_ListBox.Location = new System.Drawing.Point(3, 3);
            this.items_ListBox.Name = "items_ListBox";
            this.items_ListBox.Size = new System.Drawing.Size(150, 498);
            this.items_ListBox.TabIndex = 0;
            this.items_ListBox.SelectedIndexChanged += new System.EventHandler(this.ListBox1SelectedIndexChanged);
            // 
            // save_Button
            // 
            this.save_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.save_Button.Location = new System.Drawing.Point(576, 4);
            this.save_Button.Name = "save_Button";
            this.save_Button.Size = new System.Drawing.Size(75, 23);
            this.save_Button.TabIndex = 3;
            this.save_Button.Text = "Save File";
            this.save_Button.UseVisualStyleBackColor = true;
            this.save_Button.Click += new System.EventHandler(this.saveCharButton_Click);
            // 
            // currentlyEditing_Label
            // 
            this.currentlyEditing_Label.AutoSize = true;
            this.currentlyEditing_Label.Location = new System.Drawing.Point(12, 9);
            this.currentlyEditing_Label.Name = "currentlyEditing_Label";
            this.currentlyEditing_Label.Size = new System.Drawing.Size(124, 13);
            this.currentlyEditing_Label.TabIndex = 1;
            this.currentlyEditing_Label.Text = "Select Characer or Item: ";
            // 
            // currentlyEditing_ComboBox
            // 
            this.currentlyEditing_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.currentlyEditing_ComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.currentlyEditing_ComboBox.FormattingEnabled = true;
            this.currentlyEditing_ComboBox.Location = new System.Drawing.Point(142, 6);
            this.currentlyEditing_ComboBox.Name = "currentlyEditing_ComboBox";
            this.currentlyEditing_ComboBox.Size = new System.Drawing.Size(337, 21);
            this.currentlyEditing_ComboBox.TabIndex = 2;
            this.currentlyEditing_ComboBox.SelectedIndexChanged += new System.EventHandler(this.currentlyEditing_ComboBox_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(657, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(106, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Launch Saved File";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // HeroEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 580);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.currentlyEditing_ComboBox);
            this.Controls.Add(this.currentlyEditing_Label);
            this.Controls.Add(this.main_TabControl);
            this.Controls.Add(this.save_Button);
            this.Name = "HeroEditor";
            this.Text = "HeroEditor";
            this.Load += new System.EventHandler(this.HeroEditor_Load);
            this.main_TabControl.ResumeLayout(false);
            this.general_TabPage.ResumeLayout(false);
            this.general_TabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statPoints_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.skillPoints_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.palladium_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.level_NumericUpDown)).EndInit();
            this.stats_TabPage.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.stats_GroupBox.ResumeLayout(false);
            this.items_TabPage.ResumeLayout(false);
            this.items_TabPage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl main_TabControl;
        private System.Windows.Forms.TabPage general_TabPage;
        private System.Windows.Forms.TabPage stats_TabPage;
        private System.Windows.Forms.TabPage items_TabPage;
        private System.Windows.Forms.ListBox items_ListBox;
        private System.Windows.Forms.GroupBox stats_GroupBox;
        private System.Windows.Forms.ListBox stats_ListBox;
        private System.Windows.Forms.Button save_Button;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label currentlyEditing_Label;
        private System.Windows.Forms.ComboBox currentlyEditing_ComboBox;
        private System.Windows.Forms.Label name_Label;
        private System.Windows.Forms.TextBox name_TextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label mode_Label;
        private System.Windows.Forms.CheckBox hardcore_CheckBox;
        private System.Windows.Forms.CheckBox elite_CheckBox;
        private System.Windows.Forms.Label level_Label;
        private System.Windows.Forms.TextBox class_TextBox;
        private System.Windows.Forms.Label class_Label;
        private System.Windows.Forms.NumericUpDown level_NumericUpDown;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox dead_CheckBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.NumericUpDown palladium_numericUpDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown skillPoints_numericUpDown;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown statPoints_numericUpDown;
    }
}