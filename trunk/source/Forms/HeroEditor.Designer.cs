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
            this.stats_TabPage = new System.Windows.Forms.TabPage();
            this.charStatValues_ListBox = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.save_Button = new System.Windows.Forms.Button();
            this.stats_GroupBox = new System.Windows.Forms.GroupBox();
            this.stats_ListBox = new System.Windows.Forms.ListBox();
            this.items_TabPage = new System.Windows.Forms.TabPage();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.items_ListBox = new System.Windows.Forms.ListBox();
            this.general_TabPage = new System.Windows.Forms.TabPage();
            this.currentlyEditing_Label = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.main_TabControl.SuspendLayout();
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
            // charTab
            // 
            this.stats_TabPage.Controls.Add(this.charStatValues_ListBox);
            this.stats_TabPage.Controls.Add(this.groupBox2);
            this.stats_TabPage.Controls.Add(this.groupBox1);
            this.stats_TabPage.Controls.Add(this.stats_GroupBox);
            this.stats_TabPage.Location = new System.Drawing.Point(4, 22);
            this.stats_TabPage.Name = "charTab";
            this.stats_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.stats_TabPage.Size = new System.Drawing.Size(743, 509);
            this.stats_TabPage.TabIndex = 0;
            this.stats_TabPage.Text = "Stats";
            this.stats_TabPage.UseVisualStyleBackColor = true;
            // 
            // charStatValues_ListBox
            // 
            this.charStatValues_ListBox.FormattingEnabled = true;
            this.charStatValues_ListBox.Location = new System.Drawing.Point(448, 185);
            this.charStatValues_ListBox.Name = "charStatValues_ListBox";
            this.charStatValues_ListBox.Size = new System.Drawing.Size(130, 225);
            this.charStatValues_ListBox.TabIndex = 3;
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
            // save_Button
            // 
            this.save_Button.Location = new System.Drawing.Point(450, 4);
            this.save_Button.Name = "saveCharButton";
            this.save_Button.Size = new System.Drawing.Size(75, 23);
            this.save_Button.TabIndex = 3;
            this.save_Button.Text = "Save";
            this.save_Button.UseVisualStyleBackColor = true;
            this.save_Button.Click += new System.EventHandler(this.saveCharButton_Click);
            // 
            // stats_GroupBox
            // 
            this.stats_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.stats_GroupBox.Controls.Add(this.stats_ListBox);
            this.stats_GroupBox.Location = new System.Drawing.Point(8, 6);
            this.stats_GroupBox.Name = "charStats_GroupBox";
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
            this.stats_ListBox.Name = "charStats_ListBox";
            this.stats_ListBox.Size = new System.Drawing.Size(177, 472);
            this.stats_ListBox.TabIndex = 1;
            this.stats_ListBox.Resize += new System.EventHandler(this.charStats_ListBox_Resize);
            this.stats_ListBox.SelectedIndexChanged += new System.EventHandler(this.charStats_ListBox_SelectedIndexChanged);
            // 
            // items_TabPage
            // 
            this.items_TabPage.Controls.Add(this.textBox5);
            this.items_TabPage.Controls.Add(this.textBox4);
            this.items_TabPage.Controls.Add(this.textBox3);
            this.items_TabPage.Controls.Add(this.textBox2);
            this.items_TabPage.Controls.Add(this.textBox1);
            this.items_TabPage.Controls.Add(this.items_ListBox);
            this.items_TabPage.Location = new System.Drawing.Point(4, 22);
            this.items_TabPage.Name = "itemTab";
            this.items_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.items_TabPage.Size = new System.Drawing.Size(1009, 558);
            this.items_TabPage.TabIndex = 1;
            this.items_TabPage.Text = "Items";
            this.items_TabPage.UseVisualStyleBackColor = true;
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
            this.items_ListBox.Size = new System.Drawing.Size(150, 550);
            this.items_ListBox.TabIndex = 0;
            this.items_ListBox.SelectedIndexChanged += new System.EventHandler(this.ListBox1SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.general_TabPage.Location = new System.Drawing.Point(4, 22);
            this.general_TabPage.Name = "tabPage1";
            this.general_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.general_TabPage.Size = new System.Drawing.Size(1009, 558);
            this.general_TabPage.TabIndex = 2;
            this.general_TabPage.Text = "General";
            this.general_TabPage.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.currentlyEditing_Label.AutoSize = true;
            this.currentlyEditing_Label.Location = new System.Drawing.Point(12, 9);
            this.currentlyEditing_Label.Name = "label1";
            this.currentlyEditing_Label.Size = new System.Drawing.Size(89, 13);
            this.currentlyEditing_Label.TabIndex = 1;
            this.currentlyEditing_Label.Text = "Currently Editing: ";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(107, 6);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(337, 21);
            this.comboBox1.TabIndex = 2;
            // 
            // HeroEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 580);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.currentlyEditing_Label);
            this.Controls.Add(this.main_TabControl);
            this.Controls.Add(this.save_Button);
            this.Name = "HeroEditor";
            this.Text = "HeroEditor";
            this.main_TabControl.ResumeLayout(false);
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
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListBox items_ListBox;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.GroupBox stats_GroupBox;
        private System.Windows.Forms.ListBox stats_ListBox;
        private System.Windows.Forms.Button save_Button;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox charStatValues_ListBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label currentlyEditing_Label;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}