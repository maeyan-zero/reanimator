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
            this.charTab = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.saveCharButton = new System.Windows.Forms.Button();
            this.charStats_GroupBox = new System.Windows.Forms.GroupBox();
            this.charStats_ListBox = new System.Windows.Forms.ListBox();
            this.itemTab = new System.Windows.Forms.TabPage();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.items_ListBox = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.charStatValues_ListBox = new System.Windows.Forms.ListBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.main_TabControl.SuspendLayout();
            this.charTab.SuspendLayout();
            this.charStats_GroupBox.SuspendLayout();
            this.itemTab.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // main_TabControl
            // 
            this.main_TabControl.Controls.Add(this.charTab);
            this.main_TabControl.Controls.Add(this.itemTab);
            this.main_TabControl.Controls.Add(this.tabPage1);
            this.main_TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.main_TabControl.Location = new System.Drawing.Point(0, 0);
            this.main_TabControl.Name = "main_TabControl";
            this.main_TabControl.SelectedIndex = 0;
            this.main_TabControl.Size = new System.Drawing.Size(1021, 603);
            this.main_TabControl.TabIndex = 0;
            // 
            // charTab
            // 
            this.charTab.Controls.Add(this.charStatValues_ListBox);
            this.charTab.Controls.Add(this.groupBox2);
            this.charTab.Controls.Add(this.groupBox1);
            this.charTab.Controls.Add(this.saveCharButton);
            this.charTab.Controls.Add(this.charStats_GroupBox);
            this.charTab.Location = new System.Drawing.Point(4, 22);
            this.charTab.Name = "charTab";
            this.charTab.Padding = new System.Windows.Forms.Padding(3);
            this.charTab.Size = new System.Drawing.Size(1013, 577);
            this.charTab.TabIndex = 0;
            this.charTab.Text = "Character";
            this.charTab.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(200, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(805, 134);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Stat Attributes";
            // 
            // saveCharButton
            // 
            this.saveCharButton.Location = new System.Drawing.Point(689, 469);
            this.saveCharButton.Name = "saveCharButton";
            this.saveCharButton.Size = new System.Drawing.Size(75, 23);
            this.saveCharButton.TabIndex = 3;
            this.saveCharButton.Text = "Save";
            this.saveCharButton.UseVisualStyleBackColor = true;
            this.saveCharButton.Click += new System.EventHandler(this.saveCharButton_Click);
            // 
            // charStats_GroupBox
            // 
            this.charStats_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.charStats_GroupBox.Controls.Add(this.charStats_ListBox);
            this.charStats_GroupBox.Location = new System.Drawing.Point(8, 6);
            this.charStats_GroupBox.Name = "charStats_GroupBox";
            this.charStats_GroupBox.Size = new System.Drawing.Size(186, 563);
            this.charStats_GroupBox.TabIndex = 2;
            this.charStats_GroupBox.TabStop = false;
            this.charStats_GroupBox.Text = "Stats";
            // 
            // charStats_ListBox
            // 
            this.charStats_ListBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.charStats_ListBox.FormattingEnabled = true;
            this.charStats_ListBox.Location = new System.Drawing.Point(3, 16);
            this.charStats_ListBox.Name = "charStats_ListBox";
            this.charStats_ListBox.Size = new System.Drawing.Size(177, 537);
            this.charStats_ListBox.TabIndex = 1;
            this.charStats_ListBox.Resize += new System.EventHandler(this.charStats_ListBox_Resize);
            this.charStats_ListBox.SelectedIndexChanged += new System.EventHandler(this.charStats_ListBox_SelectedIndexChanged);
            // 
            // itemTab
            // 
            this.itemTab.Controls.Add(this.textBox5);
            this.itemTab.Controls.Add(this.textBox4);
            this.itemTab.Controls.Add(this.textBox3);
            this.itemTab.Controls.Add(this.textBox2);
            this.itemTab.Controls.Add(this.textBox1);
            this.itemTab.Controls.Add(this.items_ListBox);
            this.itemTab.Location = new System.Drawing.Point(4, 22);
            this.itemTab.Name = "itemTab";
            this.itemTab.Padding = new System.Windows.Forms.Padding(3);
            this.itemTab.Size = new System.Drawing.Size(616, 577);
            this.itemTab.TabIndex = 1;
            this.itemTab.Text = "Items";
            this.itemTab.UseVisualStyleBackColor = true;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(185, 129);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(100, 20);
            this.textBox5.TabIndex = 5;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(185, 103);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(100, 20);
            this.textBox4.TabIndex = 4;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(185, 77);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 20);
            this.textBox3.TabIndex = 3;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(185, 51);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 2;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(185, 25);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 1;
            // 
            // items_ListBox
            // 
            this.items_ListBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.items_ListBox.FormattingEnabled = true;
            this.items_ListBox.Location = new System.Drawing.Point(3, 3);
            this.items_ListBox.Name = "items_ListBox";
            this.items_ListBox.Size = new System.Drawing.Size(150, 563);
            this.items_ListBox.TabIndex = 0;
            this.items_ListBox.SelectedIndexChanged += new System.EventHandler(this.ListBox1SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Location = new System.Drawing.Point(201, 147);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(221, 422);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Stat Values";
            // 
            // charStatValues_ListBox
            // 
            this.charStatValues_ListBox.FormattingEnabled = true;
            this.charStatValues_ListBox.Location = new System.Drawing.Point(856, 198);
            this.charStatValues_ListBox.Name = "charStatValues_ListBox";
            this.charStatValues_ListBox.Size = new System.Drawing.Size(130, 342);
            this.charStatValues_ListBox.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(616, 577);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
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
            this.panel1.Size = new System.Drawing.Size(208, 396);
            this.panel1.TabIndex = 0;
            // 
            // HeroEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1021, 603);
            this.Controls.Add(this.main_TabControl);
            this.Name = "HeroEditor";
            this.Text = "HeroEditor";
            this.main_TabControl.ResumeLayout(false);
            this.charTab.ResumeLayout(false);
            this.charStats_GroupBox.ResumeLayout(false);
            this.itemTab.ResumeLayout(false);
            this.itemTab.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        
        #endregion

        private System.Windows.Forms.TabControl main_TabControl;
        private System.Windows.Forms.TabPage charTab;
        private System.Windows.Forms.TabPage itemTab;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListBox items_ListBox;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.GroupBox charStats_GroupBox;
        private System.Windows.Forms.ListBox charStats_ListBox;
        private System.Windows.Forms.Button saveCharButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox charStatValues_ListBox;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel1;
    }
}