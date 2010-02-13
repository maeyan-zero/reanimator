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
            this.statAttribute3_GroupBox = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.statAttribute3_resource_TextBox = new System.Windows.Forms.TextBox();
            this.statAttribute3_unknown1_1_TextBox = new System.Windows.Forms.TextBox();
            this.statAttribute3_unknown1_TextBox = new System.Windows.Forms.TextBox();
            this.statAttribute3_bitCount_TextBox = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.statAttribute2_GroupBox = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.statAttribute2_resource_TextBox = new System.Windows.Forms.TextBox();
            this.statAttribute2_unknown1_1_TextBox = new System.Windows.Forms.TextBox();
            this.statAttribute2_unknown1_TextBox = new System.Windows.Forms.TextBox();
            this.statAttribute2_bitCount_TextBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.statAttribute1_GroupBox = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.statAttribute1_resource_TextBox = new System.Windows.Forms.TextBox();
            this.statAttribute1_unknown1_1_TextBox = new System.Windows.Forms.TextBox();
            this.statAttribute1_unknown1_TextBox = new System.Windows.Forms.TextBox();
            this.statAttribute1_bitCount_TextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.stats_GroupBox = new System.Windows.Forms.GroupBox();
            this.stats_ListBox = new System.Windows.Forms.ListBox();
            this.items_TabPage = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.items_ListBox = new System.Windows.Forms.ListBox();
            this.minigame_TabPage = new System.Windows.Forms.TabPage();
            this.save_Button = new System.Windows.Forms.Button();
            this.currentlyEditing_Label = new System.Windows.Forms.Label();
            this.currentlyEditing_ComboBox = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tp_test = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.p_wpNormal = new System.Windows.Forms.Panel();
            this.p_wpNightmare = new System.Windows.Forms.Panel();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.p_miniGame = new System.Windows.Forms.Panel();
            this.main_TabControl.SuspendLayout();
            this.general_TabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statPoints_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.skillPoints_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.palladium_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.level_NumericUpDown)).BeginInit();
            this.stats_TabPage.SuspendLayout();
            this.statAttribute3_GroupBox.SuspendLayout();
            this.statAttribute2_GroupBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statAttribute1_GroupBox.SuspendLayout();
            this.stats_GroupBox.SuspendLayout();
            this.items_TabPage.SuspendLayout();
            this.minigame_TabPage.SuspendLayout();
            this.tp_test.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
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
            this.main_TabControl.Controls.Add(this.minigame_TabPage);
            this.main_TabControl.Controls.Add(this.tp_test);
            this.main_TabControl.Location = new System.Drawing.Point(12, 33);
            this.main_TabControl.Name = "main_TabControl";
            this.main_TabControl.SelectedIndex = 0;
            this.main_TabControl.Size = new System.Drawing.Size(657, 535);
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
            this.general_TabPage.Size = new System.Drawing.Size(649, 509);
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
            this.stats_TabPage.Controls.Add(this.statAttribute3_GroupBox);
            this.stats_TabPage.Controls.Add(this.statAttribute2_GroupBox);
            this.stats_TabPage.Controls.Add(this.groupBox2);
            this.stats_TabPage.Controls.Add(this.statAttribute1_GroupBox);
            this.stats_TabPage.Controls.Add(this.stats_GroupBox);
            this.stats_TabPage.Location = new System.Drawing.Point(4, 22);
            this.stats_TabPage.Name = "stats_TabPage";
            this.stats_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.stats_TabPage.Size = new System.Drawing.Size(649, 509);
            this.stats_TabPage.TabIndex = 0;
            this.stats_TabPage.Text = "Stats";
            this.stats_TabPage.UseVisualStyleBackColor = true;
            // 
            // statAttribute3_GroupBox
            // 
            this.statAttribute3_GroupBox.Controls.Add(this.label15);
            this.statAttribute3_GroupBox.Controls.Add(this.label16);
            this.statAttribute3_GroupBox.Controls.Add(this.statAttribute3_resource_TextBox);
            this.statAttribute3_GroupBox.Controls.Add(this.statAttribute3_unknown1_1_TextBox);
            this.statAttribute3_GroupBox.Controls.Add(this.statAttribute3_unknown1_TextBox);
            this.statAttribute3_GroupBox.Controls.Add(this.statAttribute3_bitCount_TextBox);
            this.statAttribute3_GroupBox.Controls.Add(this.label17);
            this.statAttribute3_GroupBox.Controls.Add(this.label18);
            this.statAttribute3_GroupBox.Location = new System.Drawing.Point(200, 286);
            this.statAttribute3_GroupBox.Name = "statAttribute3_GroupBox";
            this.statAttribute3_GroupBox.Size = new System.Drawing.Size(216, 134);
            this.statAttribute3_GroupBox.TabIndex = 9;
            this.statAttribute3_GroupBox.TabStop = false;
            this.statAttribute3_GroupBox.Text = "Stat Attribute 3";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(0, 102);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(53, 13);
            this.label15.TabIndex = 7;
            this.label15.Text = "Resource";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(0, 80);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(71, 13);
            this.label16.TabIndex = 6;
            this.label16.Text = "Unknown1_1";
            // 
            // statAttribute3_resource_TextBox
            // 
            this.statAttribute3_resource_TextBox.Location = new System.Drawing.Point(77, 99);
            this.statAttribute3_resource_TextBox.Name = "statAttribute3_resource_TextBox";
            this.statAttribute3_resource_TextBox.Size = new System.Drawing.Size(133, 20);
            this.statAttribute3_resource_TextBox.TabIndex = 5;
            // 
            // statAttribute3_unknown1_1_TextBox
            // 
            this.statAttribute3_unknown1_1_TextBox.Location = new System.Drawing.Point(77, 73);
            this.statAttribute3_unknown1_1_TextBox.Name = "statAttribute3_unknown1_1_TextBox";
            this.statAttribute3_unknown1_1_TextBox.Size = new System.Drawing.Size(133, 20);
            this.statAttribute3_unknown1_1_TextBox.TabIndex = 4;
            // 
            // statAttribute3_unknown1_TextBox
            // 
            this.statAttribute3_unknown1_TextBox.Location = new System.Drawing.Point(77, 47);
            this.statAttribute3_unknown1_TextBox.Name = "statAttribute3_unknown1_TextBox";
            this.statAttribute3_unknown1_TextBox.Size = new System.Drawing.Size(133, 20);
            this.statAttribute3_unknown1_TextBox.TabIndex = 3;
            // 
            // statAttribute3_bitCount_TextBox
            // 
            this.statAttribute3_bitCount_TextBox.Location = new System.Drawing.Point(77, 20);
            this.statAttribute3_bitCount_TextBox.Name = "statAttribute3_bitCount_TextBox";
            this.statAttribute3_bitCount_TextBox.Size = new System.Drawing.Size(133, 20);
            this.statAttribute3_bitCount_TextBox.TabIndex = 2;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(0, 50);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(59, 13);
            this.label17.TabIndex = 1;
            this.label17.Text = "Unknown1";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(0, 23);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(50, 13);
            this.label18.TabIndex = 0;
            this.label18.Text = "Bit Count";
            // 
            // statAttribute2_GroupBox
            // 
            this.statAttribute2_GroupBox.Controls.Add(this.label11);
            this.statAttribute2_GroupBox.Controls.Add(this.label12);
            this.statAttribute2_GroupBox.Controls.Add(this.statAttribute2_resource_TextBox);
            this.statAttribute2_GroupBox.Controls.Add(this.statAttribute2_unknown1_1_TextBox);
            this.statAttribute2_GroupBox.Controls.Add(this.statAttribute2_unknown1_TextBox);
            this.statAttribute2_GroupBox.Controls.Add(this.statAttribute2_bitCount_TextBox);
            this.statAttribute2_GroupBox.Controls.Add(this.label13);
            this.statAttribute2_GroupBox.Controls.Add(this.label14);
            this.statAttribute2_GroupBox.Location = new System.Drawing.Point(200, 146);
            this.statAttribute2_GroupBox.Name = "statAttribute2_GroupBox";
            this.statAttribute2_GroupBox.Size = new System.Drawing.Size(216, 134);
            this.statAttribute2_GroupBox.TabIndex = 8;
            this.statAttribute2_GroupBox.TabStop = false;
            this.statAttribute2_GroupBox.Text = "Stat Attribute 2";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(0, 102);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 13);
            this.label11.TabIndex = 7;
            this.label11.Text = "Resource";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(0, 80);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(71, 13);
            this.label12.TabIndex = 6;
            this.label12.Text = "Unknown1_1";
            // 
            // statAttribute2_resource_TextBox
            // 
            this.statAttribute2_resource_TextBox.Location = new System.Drawing.Point(77, 99);
            this.statAttribute2_resource_TextBox.Name = "statAttribute2_resource_TextBox";
            this.statAttribute2_resource_TextBox.Size = new System.Drawing.Size(133, 20);
            this.statAttribute2_resource_TextBox.TabIndex = 5;
            // 
            // statAttribute2_unknown1_1_TextBox
            // 
            this.statAttribute2_unknown1_1_TextBox.Location = new System.Drawing.Point(77, 73);
            this.statAttribute2_unknown1_1_TextBox.Name = "statAttribute2_unknown1_1_TextBox";
            this.statAttribute2_unknown1_1_TextBox.Size = new System.Drawing.Size(133, 20);
            this.statAttribute2_unknown1_1_TextBox.TabIndex = 4;
            // 
            // statAttribute2_unknown1_TextBox
            // 
            this.statAttribute2_unknown1_TextBox.Location = new System.Drawing.Point(77, 47);
            this.statAttribute2_unknown1_TextBox.Name = "statAttribute2_unknown1_TextBox";
            this.statAttribute2_unknown1_TextBox.Size = new System.Drawing.Size(133, 20);
            this.statAttribute2_unknown1_TextBox.TabIndex = 3;
            // 
            // statAttribute2_bitCount_TextBox
            // 
            this.statAttribute2_bitCount_TextBox.Location = new System.Drawing.Point(77, 20);
            this.statAttribute2_bitCount_TextBox.Name = "statAttribute2_bitCount_TextBox";
            this.statAttribute2_bitCount_TextBox.Size = new System.Drawing.Size(133, 20);
            this.statAttribute2_bitCount_TextBox.TabIndex = 2;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(0, 50);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(59, 13);
            this.label13.TabIndex = 1;
            this.label13.Text = "Unknown1";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(0, 23);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(50, 13);
            this.label14.TabIndex = 0;
            this.label14.Text = "Bit Count";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Location = new System.Drawing.Point(422, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(221, 500);
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
            this.panel1.Size = new System.Drawing.Size(208, 474);
            this.panel1.TabIndex = 0;
            // 
            // statAttribute1_GroupBox
            // 
            this.statAttribute1_GroupBox.Controls.Add(this.label10);
            this.statAttribute1_GroupBox.Controls.Add(this.label9);
            this.statAttribute1_GroupBox.Controls.Add(this.statAttribute1_resource_TextBox);
            this.statAttribute1_GroupBox.Controls.Add(this.statAttribute1_unknown1_1_TextBox);
            this.statAttribute1_GroupBox.Controls.Add(this.statAttribute1_unknown1_TextBox);
            this.statAttribute1_GroupBox.Controls.Add(this.statAttribute1_bitCount_TextBox);
            this.statAttribute1_GroupBox.Controls.Add(this.label8);
            this.statAttribute1_GroupBox.Controls.Add(this.label7);
            this.statAttribute1_GroupBox.Location = new System.Drawing.Point(200, 6);
            this.statAttribute1_GroupBox.Name = "statAttribute1_GroupBox";
            this.statAttribute1_GroupBox.Size = new System.Drawing.Size(216, 134);
            this.statAttribute1_GroupBox.TabIndex = 4;
            this.statAttribute1_GroupBox.TabStop = false;
            this.statAttribute1_GroupBox.Text = "Stat Attribute 1";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(0, 102);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 13);
            this.label10.TabIndex = 7;
            this.label10.Text = "Resource";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(0, 80);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Unknown1_1";
            // 
            // statAttribute1_resource_TextBox
            // 
            this.statAttribute1_resource_TextBox.Location = new System.Drawing.Point(77, 99);
            this.statAttribute1_resource_TextBox.Name = "statAttribute1_resource_TextBox";
            this.statAttribute1_resource_TextBox.Size = new System.Drawing.Size(133, 20);
            this.statAttribute1_resource_TextBox.TabIndex = 5;
            // 
            // statAttribute1_unknown1_1_TextBox
            // 
            this.statAttribute1_unknown1_1_TextBox.Location = new System.Drawing.Point(77, 73);
            this.statAttribute1_unknown1_1_TextBox.Name = "statAttribute1_unknown1_1_TextBox";
            this.statAttribute1_unknown1_1_TextBox.Size = new System.Drawing.Size(133, 20);
            this.statAttribute1_unknown1_1_TextBox.TabIndex = 4;
            // 
            // statAttribute1_unknown1_TextBox
            // 
            this.statAttribute1_unknown1_TextBox.Location = new System.Drawing.Point(77, 47);
            this.statAttribute1_unknown1_TextBox.Name = "statAttribute1_unknown1_TextBox";
            this.statAttribute1_unknown1_TextBox.Size = new System.Drawing.Size(133, 20);
            this.statAttribute1_unknown1_TextBox.TabIndex = 3;
            // 
            // statAttribute1_bitCount_TextBox
            // 
            this.statAttribute1_bitCount_TextBox.Location = new System.Drawing.Point(77, 20);
            this.statAttribute1_bitCount_TextBox.Name = "statAttribute1_bitCount_TextBox";
            this.statAttribute1_bitCount_TextBox.Size = new System.Drawing.Size(133, 20);
            this.statAttribute1_bitCount_TextBox.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(0, 50);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Unknown1";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(0, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Bit Count";
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
            this.items_TabPage.Size = new System.Drawing.Size(649, 509);
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
            // minigame_TabPage
            // 
            this.minigame_TabPage.Controls.Add(this.groupBox3);
            this.minigame_TabPage.Location = new System.Drawing.Point(4, 22);
            this.minigame_TabPage.Name = "minigame_TabPage";
            this.minigame_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.minigame_TabPage.Size = new System.Drawing.Size(649, 509);
            this.minigame_TabPage.TabIndex = 3;
            this.minigame_TabPage.Text = "Minigame";
            this.minigame_TabPage.UseVisualStyleBackColor = true;
            // 
            // save_Button
            // 
            this.save_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.save_Button.Location = new System.Drawing.Point(482, 4);
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
            this.button1.Location = new System.Drawing.Point(563, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(106, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Launch Saved File";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tp_test
            // 
            this.tp_test.Controls.Add(this.groupBox1);
            this.tp_test.Location = new System.Drawing.Point(4, 22);
            this.tp_test.Name = "tp_test";
            this.tp_test.Padding = new System.Windows.Forms.Padding(3);
            this.tp_test.Size = new System.Drawing.Size(649, 509);
            this.tp_test.TabIndex = 4;
            this.tp_test.Text = "Test";
            this.tp_test.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.p_wpNightmare);
            this.groupBox1.Controls.Add(this.p_wpNormal);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(360, 210);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Warp Terminal Locations";
            // 
            // p_wpNormal
            // 
            this.p_wpNormal.Location = new System.Drawing.Point(6, 32);
            this.p_wpNormal.Name = "p_wpNormal";
            this.p_wpNormal.Size = new System.Drawing.Size(170, 170);
            this.p_wpNormal.TabIndex = 0;
            // 
            // p_wpNightmare
            // 
            this.p_wpNightmare.Location = new System.Drawing.Point(182, 32);
            this.p_wpNightmare.Name = "p_wpNightmare";
            this.p_wpNightmare.Size = new System.Drawing.Size(170, 170);
            this.p_wpNightmare.TabIndex = 1;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(3, 16);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(72, 13);
            this.label19.TabIndex = 2;
            this.label19.Text = "Normal mode:";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(179, 16);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(87, 13);
            this.label20.TabIndex = 3;
            this.label20.Text = "Nightmare mode:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.p_miniGame);
            this.groupBox3.Location = new System.Drawing.Point(6, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(300, 200);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Minigame";
            // 
            // p_miniGame
            // 
            this.p_miniGame.Location = new System.Drawing.Point(6, 19);
            this.p_miniGame.Name = "p_miniGame";
            this.p_miniGame.Size = new System.Drawing.Size(286, 176);
            this.p_miniGame.TabIndex = 0;
            // 
            // HeroEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 580);
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
            this.statAttribute3_GroupBox.ResumeLayout(false);
            this.statAttribute3_GroupBox.PerformLayout();
            this.statAttribute2_GroupBox.ResumeLayout(false);
            this.statAttribute2_GroupBox.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.statAttribute1_GroupBox.ResumeLayout(false);
            this.statAttribute1_GroupBox.PerformLayout();
            this.stats_GroupBox.ResumeLayout(false);
            this.items_TabPage.ResumeLayout(false);
            this.items_TabPage.PerformLayout();
            this.minigame_TabPage.ResumeLayout(false);
            this.tp_test.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
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
        private System.Windows.Forms.GroupBox statAttribute1_GroupBox;
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
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox statAttribute1_bitCount_TextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox statAttribute1_unknown1_TextBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox statAttribute1_resource_TextBox;
        private System.Windows.Forms.TextBox statAttribute1_unknown1_1_TextBox;
        private System.Windows.Forms.GroupBox statAttribute3_GroupBox;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox statAttribute3_resource_TextBox;
        private System.Windows.Forms.TextBox statAttribute3_unknown1_1_TextBox;
        private System.Windows.Forms.TextBox statAttribute3_unknown1_TextBox;
        private System.Windows.Forms.TextBox statAttribute3_bitCount_TextBox;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.GroupBox statAttribute2_GroupBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox statAttribute2_resource_TextBox;
        private System.Windows.Forms.TextBox statAttribute2_unknown1_1_TextBox;
        private System.Windows.Forms.TextBox statAttribute2_unknown1_TextBox;
        private System.Windows.Forms.TextBox statAttribute2_bitCount_TextBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TabPage minigame_TabPage;
        private System.Windows.Forms.TabPage tp_test;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Panel p_wpNightmare;
        private System.Windows.Forms.Panel p_wpNormal;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Panel p_miniGame;
    }
}