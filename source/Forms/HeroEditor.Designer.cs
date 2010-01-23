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
            this.saveCharButton = new System.Windows.Forms.Button();
            this.charStats_GroupBox = new System.Windows.Forms.GroupBox();
            this.charStats_ListBox = new System.Windows.Forms.ListBox();
            this.charStatValues_ListBox = new System.Windows.Forms.ListBox();
            this.itemTab = new System.Windows.Forms.TabPage();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.items_ListBox = new System.Windows.Forms.ListBox();
            this.main_TabControl.SuspendLayout();
            this.charTab.SuspendLayout();
            this.charStats_GroupBox.SuspendLayout();
            this.itemTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // main_TabControl
            // 
            this.main_TabControl.Controls.Add(this.charTab);
            this.main_TabControl.Controls.Add(this.itemTab);
            this.main_TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.main_TabControl.Location = new System.Drawing.Point(0, 0);
            this.main_TabControl.Name = "main_TabControl";
            this.main_TabControl.SelectedIndex = 0;
            this.main_TabControl.Size = new System.Drawing.Size(523, 404);
            this.main_TabControl.TabIndex = 0;
            // 
            // charTab
            // 
            this.charTab.Controls.Add(this.saveCharButton);
            this.charTab.Controls.Add(this.charStats_GroupBox);
            this.charTab.Location = new System.Drawing.Point(4, 22);
            this.charTab.Name = "charTab";
            this.charTab.Padding = new System.Windows.Forms.Padding(3);
            this.charTab.Size = new System.Drawing.Size(515, 378);
            this.charTab.TabIndex = 0;
            this.charTab.Text = "Character";
            this.charTab.UseVisualStyleBackColor = true;
            // 
            // saveCharButton
            // 
            this.saveCharButton.Location = new System.Drawing.Point(432, 347);
            this.saveCharButton.Name = "saveCharButton";
            this.saveCharButton.Size = new System.Drawing.Size(75, 23);
            this.saveCharButton.TabIndex = 3;
            this.saveCharButton.Text = "Save";
            this.saveCharButton.UseVisualStyleBackColor = true;
            this.saveCharButton.Click += new System.EventHandler(this.saveCharButton_Click);
            // 
            // charStats_GroupBox
            // 
            this.charStats_GroupBox.Controls.Add(this.charStats_ListBox);
            this.charStats_GroupBox.Controls.Add(this.charStatValues_ListBox);
            this.charStats_GroupBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.charStats_GroupBox.Location = new System.Drawing.Point(3, 3);
            this.charStats_GroupBox.Name = "charStats_GroupBox";
            this.charStats_GroupBox.Size = new System.Drawing.Size(319, 372);
            this.charStats_GroupBox.TabIndex = 2;
            this.charStats_GroupBox.TabStop = false;
            this.charStats_GroupBox.Text = "groupBox1";
            // 
            // charStats_ListBox
            // 
            this.charStats_ListBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.charStats_ListBox.FormattingEnabled = true;
            this.charStats_ListBox.Location = new System.Drawing.Point(3, 16);
            this.charStats_ListBox.Name = "charStats_ListBox";
            this.charStats_ListBox.Size = new System.Drawing.Size(170, 342);
            this.charStats_ListBox.TabIndex = 1;
            this.charStats_ListBox.Resize += new System.EventHandler(this.charStats_ListBox_Resize);
            this.charStats_ListBox.SelectedIndexChanged += new System.EventHandler(this.charStats_ListBox_SelectedIndexChanged);
            // 
            // charStatValues_ListBox
            // 
            this.charStatValues_ListBox.FormattingEnabled = true;
            this.charStatValues_ListBox.Location = new System.Drawing.Point(179, 16);
            this.charStatValues_ListBox.Name = "charStatValues_ListBox";
            this.charStatValues_ListBox.Size = new System.Drawing.Size(130, 342);
            this.charStatValues_ListBox.TabIndex = 2;
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
            this.itemTab.Size = new System.Drawing.Size(515, 378);
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
            this.items_ListBox.Size = new System.Drawing.Size(150, 368);
            this.items_ListBox.TabIndex = 0;
            this.items_ListBox.SelectedIndexChanged += new System.EventHandler(this.ListBox1SelectedIndexChanged);
            // 
            // HeroEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(523, 404);
            this.Controls.Add(this.main_TabControl);
            this.Name = "HeroEditor";
            this.Text = "HeroEditor";
            this.main_TabControl.ResumeLayout(false);
            this.charTab.ResumeLayout(false);
            this.charStats_GroupBox.ResumeLayout(false);
            this.itemTab.ResumeLayout(false);
            this.itemTab.PerformLayout();
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
        private System.Windows.Forms.ListBox charStatValues_ListBox;
        private System.Windows.Forms.Button saveCharButton;
    }
}