namespace Reanimator.Forms.ItemTransfer
{
    partial class ItemTransferForm
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
            this.gb_characterName1 = new System.Windows.Forms.GroupBox();
            this.tc_character1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lb_characterEquipment1 = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lb_characterInventory1 = new System.Windows.Forms.ListBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.lb_characterStash1 = new System.Windows.Forms.ListBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.lb_characterCube1 = new System.Windows.Forms.ListBox();
            this.cb_selectCharacter1 = new System.Windows.Forms.ComboBox();
            this.gb_characterName2 = new System.Windows.Forms.GroupBox();
            this.tc_character2 = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.lb_characterEquipment2 = new System.Windows.Forms.ListBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.lb_characterInventory2 = new System.Windows.Forms.ListBox();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.lb_characterStash2 = new System.Windows.Forms.ListBox();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.lb_characterCube2 = new System.Windows.Forms.ListBox();
            this.b_transferToLeft = new System.Windows.Forms.Button();
            this.b_transferToRight = new System.Windows.Forms.Button();
            this.b_delete = new System.Windows.Forms.Button();
            this.b_loadCharacter1 = new System.Windows.Forms.Button();
            this.b_loadCharacter2 = new System.Windows.Forms.Button();
            this.cb_selectCharacter2 = new System.Windows.Forms.ComboBox();
            this.b_save = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.gb_characterName1.SuspendLayout();
            this.tc_character1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.gb_characterName2.SuspendLayout();
            this.tc_character2.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.SuspendLayout();
            // 
            // gb_characterName1
            // 
            this.gb_characterName1.Controls.Add(this.tc_character1);
            this.gb_characterName1.Location = new System.Drawing.Point(12, 39);
            this.gb_characterName1.Name = "gb_characterName1";
            this.gb_characterName1.Size = new System.Drawing.Size(280, 393);
            this.gb_characterName1.TabIndex = 0;
            this.gb_characterName1.TabStop = false;
            this.gb_characterName1.Text = "Character 1";
            // 
            // tc_character1
            // 
            this.tc_character1.Controls.Add(this.tabPage1);
            this.tc_character1.Controls.Add(this.tabPage2);
            this.tc_character1.Controls.Add(this.tabPage3);
            this.tc_character1.Controls.Add(this.tabPage4);
            this.tc_character1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tc_character1.Location = new System.Drawing.Point(3, 16);
            this.tc_character1.Multiline = true;
            this.tc_character1.Name = "tc_character1";
            this.tc_character1.SelectedIndex = 0;
            this.tc_character1.Size = new System.Drawing.Size(274, 374);
            this.tc_character1.TabIndex = 0;
            this.tc_character1.SelectedIndexChanged += new System.EventHandler(this.tc_character1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lb_characterEquipment1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(266, 348);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Equipped";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lb_characterEquipment1
            // 
            this.lb_characterEquipment1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lb_characterEquipment1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_characterEquipment1.FormattingEnabled = true;
            this.lb_characterEquipment1.Location = new System.Drawing.Point(3, 3);
            this.lb_characterEquipment1.Name = "lb_characterEquipment1";
            this.lb_characterEquipment1.Size = new System.Drawing.Size(260, 338);
            this.lb_characterEquipment1.TabIndex = 0;
            this.lb_characterEquipment1.SelectedIndexChanged += new System.EventHandler(this.lb_character1_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lb_characterInventory1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(266, 348);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Inventory";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lb_characterInventory1
            // 
            this.lb_characterInventory1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lb_characterInventory1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_characterInventory1.FormattingEnabled = true;
            this.lb_characterInventory1.Location = new System.Drawing.Point(3, 3);
            this.lb_characterInventory1.Name = "lb_characterInventory1";
            this.lb_characterInventory1.Size = new System.Drawing.Size(260, 338);
            this.lb_characterInventory1.TabIndex = 1;
            this.lb_characterInventory1.SelectedIndexChanged += new System.EventHandler(this.lb_character1_SelectedIndexChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.lb_characterStash1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(266, 348);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Stash";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // lb_characterStash1
            // 
            this.lb_characterStash1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lb_characterStash1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_characterStash1.FormattingEnabled = true;
            this.lb_characterStash1.Location = new System.Drawing.Point(3, 3);
            this.lb_characterStash1.Name = "lb_characterStash1";
            this.lb_characterStash1.Size = new System.Drawing.Size(260, 338);
            this.lb_characterStash1.TabIndex = 1;
            this.lb_characterStash1.SelectedIndexChanged += new System.EventHandler(this.lb_character1_SelectedIndexChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.lb_characterCube1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(266, 348);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Cube";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // lb_characterCube1
            // 
            this.lb_characterCube1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lb_characterCube1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_characterCube1.FormattingEnabled = true;
            this.lb_characterCube1.Location = new System.Drawing.Point(3, 3);
            this.lb_characterCube1.Name = "lb_characterCube1";
            this.lb_characterCube1.Size = new System.Drawing.Size(260, 338);
            this.lb_characterCube1.TabIndex = 1;
            this.lb_characterCube1.SelectedIndexChanged += new System.EventHandler(this.lb_character1_SelectedIndexChanged);
            // 
            // cb_selectCharacter1
            // 
            this.cb_selectCharacter1.FormattingEnabled = true;
            this.cb_selectCharacter1.Location = new System.Drawing.Point(115, 12);
            this.cb_selectCharacter1.Name = "cb_selectCharacter1";
            this.cb_selectCharacter1.Size = new System.Drawing.Size(107, 21);
            this.cb_selectCharacter1.TabIndex = 1;
            // 
            // gb_characterName2
            // 
            this.gb_characterName2.Controls.Add(this.tc_character2);
            this.gb_characterName2.Location = new System.Drawing.Point(332, 39);
            this.gb_characterName2.Name = "gb_characterName2";
            this.gb_characterName2.Size = new System.Drawing.Size(280, 393);
            this.gb_characterName2.TabIndex = 2;
            this.gb_characterName2.TabStop = false;
            this.gb_characterName2.Text = "Character 2";
            // 
            // tc_character2
            // 
            this.tc_character2.Controls.Add(this.tabPage5);
            this.tc_character2.Controls.Add(this.tabPage6);
            this.tc_character2.Controls.Add(this.tabPage7);
            this.tc_character2.Controls.Add(this.tabPage8);
            this.tc_character2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tc_character2.Location = new System.Drawing.Point(3, 16);
            this.tc_character2.Multiline = true;
            this.tc_character2.Name = "tc_character2";
            this.tc_character2.SelectedIndex = 0;
            this.tc_character2.Size = new System.Drawing.Size(274, 374);
            this.tc_character2.TabIndex = 0;
            this.tc_character2.SelectedIndexChanged += new System.EventHandler(this.tc_character2_SelectedIndexChanged);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.lb_characterEquipment2);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(266, 348);
            this.tabPage5.TabIndex = 0;
            this.tabPage5.Text = "Equipped";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // lb_characterEquipment2
            // 
            this.lb_characterEquipment2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lb_characterEquipment2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_characterEquipment2.FormattingEnabled = true;
            this.lb_characterEquipment2.Location = new System.Drawing.Point(3, 3);
            this.lb_characterEquipment2.Name = "lb_characterEquipment2";
            this.lb_characterEquipment2.Size = new System.Drawing.Size(260, 338);
            this.lb_characterEquipment2.TabIndex = 1;
            this.lb_characterEquipment2.SelectedIndexChanged += new System.EventHandler(this.lb_character2_SelectedIndexChanged);
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.lb_characterInventory2);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(266, 348);
            this.tabPage6.TabIndex = 1;
            this.tabPage6.Text = "Inventory";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // lb_characterInventory2
            // 
            this.lb_characterInventory2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lb_characterInventory2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_characterInventory2.FormattingEnabled = true;
            this.lb_characterInventory2.Location = new System.Drawing.Point(3, 3);
            this.lb_characterInventory2.Name = "lb_characterInventory2";
            this.lb_characterInventory2.Size = new System.Drawing.Size(260, 338);
            this.lb_characterInventory2.TabIndex = 1;
            this.lb_characterInventory2.SelectedIndexChanged += new System.EventHandler(this.lb_character2_SelectedIndexChanged);
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.lb_characterStash2);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(266, 348);
            this.tabPage7.TabIndex = 2;
            this.tabPage7.Text = "Stash";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // lb_characterStash2
            // 
            this.lb_characterStash2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lb_characterStash2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_characterStash2.FormattingEnabled = true;
            this.lb_characterStash2.Location = new System.Drawing.Point(3, 3);
            this.lb_characterStash2.Name = "lb_characterStash2";
            this.lb_characterStash2.Size = new System.Drawing.Size(260, 338);
            this.lb_characterStash2.TabIndex = 1;
            this.lb_characterStash2.SelectedIndexChanged += new System.EventHandler(this.lb_character2_SelectedIndexChanged);
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.lb_characterCube2);
            this.tabPage8.Location = new System.Drawing.Point(4, 22);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage8.Size = new System.Drawing.Size(266, 348);
            this.tabPage8.TabIndex = 3;
            this.tabPage8.Text = "Cube";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // lb_characterCube2
            // 
            this.lb_characterCube2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lb_characterCube2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_characterCube2.FormattingEnabled = true;
            this.lb_characterCube2.Location = new System.Drawing.Point(3, 3);
            this.lb_characterCube2.Name = "lb_characterCube2";
            this.lb_characterCube2.Size = new System.Drawing.Size(260, 338);
            this.lb_characterCube2.TabIndex = 1;
            this.lb_characterCube2.SelectedIndexChanged += new System.EventHandler(this.lb_character2_SelectedIndexChanged);
            // 
            // b_transferToLeft
            // 
            this.b_transferToLeft.Location = new System.Drawing.Point(298, 39);
            this.b_transferToLeft.Name = "b_transferToLeft";
            this.b_transferToLeft.Size = new System.Drawing.Size(28, 28);
            this.b_transferToLeft.TabIndex = 4;
            this.b_transferToLeft.Text = "<=";
            this.b_transferToLeft.UseVisualStyleBackColor = true;
            this.b_transferToLeft.Click += new System.EventHandler(this.b_transferToLeft_Click);
            // 
            // b_transferToRight
            // 
            this.b_transferToRight.Location = new System.Drawing.Point(298, 73);
            this.b_transferToRight.Name = "b_transferToRight";
            this.b_transferToRight.Size = new System.Drawing.Size(28, 28);
            this.b_transferToRight.TabIndex = 5;
            this.b_transferToRight.Text = "=>";
            this.b_transferToRight.UseVisualStyleBackColor = true;
            this.b_transferToRight.Click += new System.EventHandler(this.b_transferToRight_Click);
            // 
            // b_delete
            // 
            this.b_delete.Location = new System.Drawing.Point(298, 107);
            this.b_delete.Name = "b_delete";
            this.b_delete.Size = new System.Drawing.Size(28, 28);
            this.b_delete.TabIndex = 6;
            this.b_delete.Text = "[_]";
            this.b_delete.UseVisualStyleBackColor = true;
            this.b_delete.Click += new System.EventHandler(this.b_delete_Click);
            // 
            // b_loadCharacter1
            // 
            this.b_loadCharacter1.Location = new System.Drawing.Point(228, 12);
            this.b_loadCharacter1.Name = "b_loadCharacter1";
            this.b_loadCharacter1.Size = new System.Drawing.Size(64, 23);
            this.b_loadCharacter1.TabIndex = 7;
            this.b_loadCharacter1.Text = "load";
            this.b_loadCharacter1.UseVisualStyleBackColor = true;
            this.b_loadCharacter1.Click += new System.EventHandler(this.b_loadCharacter1_Click);
            // 
            // b_loadCharacter2
            // 
            this.b_loadCharacter2.Location = new System.Drawing.Point(548, 12);
            this.b_loadCharacter2.Name = "b_loadCharacter2";
            this.b_loadCharacter2.Size = new System.Drawing.Size(64, 23);
            this.b_loadCharacter2.TabIndex = 9;
            this.b_loadCharacter2.Text = "load";
            this.b_loadCharacter2.UseVisualStyleBackColor = true;
            this.b_loadCharacter2.Click += new System.EventHandler(this.b_loadCharacter2_Click);
            // 
            // cb_selectCharacter2
            // 
            this.cb_selectCharacter2.FormattingEnabled = true;
            this.cb_selectCharacter2.Location = new System.Drawing.Point(435, 12);
            this.cb_selectCharacter2.Name = "cb_selectCharacter2";
            this.cb_selectCharacter2.Size = new System.Drawing.Size(107, 21);
            this.cb_selectCharacter2.TabIndex = 8;
            // 
            // b_save
            // 
            this.b_save.Location = new System.Drawing.Point(298, 404);
            this.b_save.Name = "b_save";
            this.b_save.Size = new System.Drawing.Size(28, 28);
            this.b_save.TabIndex = 10;
            this.b_save.Text = "!";
            this.b_save.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Select a character:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(332, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Select a character:";
            // 
            // ItemTransferForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 444);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.b_save);
            this.Controls.Add(this.b_loadCharacter2);
            this.Controls.Add(this.cb_selectCharacter2);
            this.Controls.Add(this.b_loadCharacter1);
            this.Controls.Add(this.b_delete);
            this.Controls.Add(this.b_transferToRight);
            this.Controls.Add(this.b_transferToLeft);
            this.Controls.Add(this.gb_characterName2);
            this.Controls.Add(this.cb_selectCharacter1);
            this.Controls.Add(this.gb_characterName1);
            this.Name = "ItemTransferForm";
            this.Text = "Trade Items";
            this.gb_characterName1.ResumeLayout(false);
            this.tc_character1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.gb_characterName2.ResumeLayout(false);
            this.tc_character2.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabPage8.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_characterName1;
        private System.Windows.Forms.ComboBox cb_selectCharacter1;
        private System.Windows.Forms.TabControl tc_character1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.GroupBox gb_characterName2;
        private System.Windows.Forms.TabControl tc_character2;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.Button b_transferToLeft;
        private System.Windows.Forms.Button b_transferToRight;
        private System.Windows.Forms.Button b_delete;
        private System.Windows.Forms.ListBox lb_characterEquipment1;
        private System.Windows.Forms.ListBox lb_characterInventory1;
        private System.Windows.Forms.ListBox lb_characterStash1;
        private System.Windows.Forms.ListBox lb_characterCube1;
        private System.Windows.Forms.ListBox lb_characterEquipment2;
        private System.Windows.Forms.ListBox lb_characterInventory2;
        private System.Windows.Forms.ListBox lb_characterStash2;
        private System.Windows.Forms.ListBox lb_characterCube2;
        private System.Windows.Forms.Button b_loadCharacter1;
        private System.Windows.Forms.Button b_loadCharacter2;
        private System.Windows.Forms.ComboBox cb_selectCharacter2;
        private System.Windows.Forms.Button b_save;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}