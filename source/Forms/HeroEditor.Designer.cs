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
            this.name_TextBox = new System.Windows.Forms.TextBox();
            this.name_Label = new System.Windows.Forms.Label();
            this.stats_TabPage = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.stats_GroupBox = new System.Windows.Forms.GroupBox();
            this.stats_ListBox = new System.Windows.Forms.ListBox();
            this.items_TabPage = new System.Windows.Forms.TabPage();
            this.items_ListBox = new System.Windows.Forms.ListBox();
            this.save_Button = new System.Windows.Forms.Button();
            this.currentlyEditing_Label = new System.Windows.Forms.Label();
            this.currentlyEditing_ComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.main_TabControl.SuspendLayout();
            this.general_TabPage.SuspendLayout();
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
            this.save_Button.Location = new System.Drawing.Point(599, 6);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(213, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(253, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Ignore this section for now. Select items from above.";
            // 
            // HeroEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 580);
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
    }
}