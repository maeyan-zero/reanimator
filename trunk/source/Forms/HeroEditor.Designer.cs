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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.charTab = new System.Windows.Forms.TabPage();
            this.charListBox = new System.Windows.Forms.ListBox();
            this.itemTab = new System.Windows.Forms.TabPage();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.itemListBox = new System.Windows.Forms.ListBox();
            this.tabControl1.SuspendLayout();
            this.charTab.SuspendLayout();
            this.itemTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.charTab);
            this.tabControl1.Controls.Add(this.itemTab);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(387, 312);
            this.tabControl1.TabIndex = 0;
            // 
            // charTab
            // 
            this.charTab.Controls.Add(this.charListBox);
            this.charTab.Location = new System.Drawing.Point(4, 22);
            this.charTab.Name = "charTab";
            this.charTab.Padding = new System.Windows.Forms.Padding(3);
            this.charTab.Size = new System.Drawing.Size(379, 286);
            this.charTab.TabIndex = 0;
            this.charTab.Text = "Character";
            this.charTab.UseVisualStyleBackColor = true;
            // 
            // charListBox
            // 
            this.charListBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.charListBox.FormattingEnabled = true;
            this.charListBox.Location = new System.Drawing.Point(3, 3);
            this.charListBox.Name = "charListBox";
            this.charListBox.Size = new System.Drawing.Size(150, 277);
            this.charListBox.TabIndex = 0;
            // 
            // itemTab
            // 
            this.itemTab.Controls.Add(this.textBox5);
            this.itemTab.Controls.Add(this.textBox4);
            this.itemTab.Controls.Add(this.textBox3);
            this.itemTab.Controls.Add(this.textBox2);
            this.itemTab.Controls.Add(this.textBox1);
            this.itemTab.Controls.Add(this.itemListBox);
            this.itemTab.Location = new System.Drawing.Point(4, 22);
            this.itemTab.Name = "itemTab";
            this.itemTab.Padding = new System.Windows.Forms.Padding(3);
            this.itemTab.Size = new System.Drawing.Size(379, 286);
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
            // itemListBox
            // 
            this.itemListBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.itemListBox.FormattingEnabled = true;
            this.itemListBox.Location = new System.Drawing.Point(3, 3);
            this.itemListBox.Name = "itemListBox";
            this.itemListBox.Size = new System.Drawing.Size(150, 277);
            this.itemListBox.TabIndex = 0;
            this.itemListBox.SelectedIndexChanged += new System.EventHandler(this.ListBox1SelectedIndexChanged);
            // 
            // HeroEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 312);
            this.Controls.Add(this.tabControl1);
            this.Name = "HeroEditor";
            this.Text = "HeroEditor";
            this.tabControl1.ResumeLayout(false);
            this.charTab.ResumeLayout(false);
            this.itemTab.ResumeLayout(false);
            this.itemTab.PerformLayout();
            this.ResumeLayout(false);

        }
        
        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage charTab;
        private System.Windows.Forms.TabPage itemTab;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListBox itemListBox;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.ListBox charListBox;
    }
}