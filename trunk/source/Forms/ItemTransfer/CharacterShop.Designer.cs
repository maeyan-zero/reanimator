namespace Reanimator.Forms.ItemTransfer
{
    partial class CharacterShop
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Mod Slots", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Affixes", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Ammo",
            "1",
            "0"}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "Battery",
            "1",
            "0"}, -1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "Fuel",
            "1",
            "0"}, -1);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "Relic",
            "1",
            "0"}, -1);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] {
            "Rocket",
            "1",
            "0"}, -1);
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] {
            "Test",
            "1",
            "0"}, -1);
            this.l_status1 = new System.Windows.Forms.Label();
            this.p_status1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.b_loadCharacter1 = new System.Windows.Forms.Button();
            this.cb_selectCharacter1 = new System.Windows.Forms.ComboBox();
            this.gb_characterName1 = new System.Windows.Forms.GroupBox();
            this.p_inventory1 = new System.Windows.Forms.Panel();
            this.tc_characterShop = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.b_undoTransfer = new System.Windows.Forms.Button();
            this.b_transfer = new System.Windows.Forms.Button();
            this.l_selectedItem = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.b_save = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.gb_characterName1.SuspendLayout();
            this.tc_characterShop.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // l_status1
            // 
            this.l_status1.AutoSize = true;
            this.l_status1.Location = new System.Drawing.Point(45, 49);
            this.l_status1.Name = "l_status1";
            this.l_status1.Size = new System.Drawing.Size(104, 13);
            this.l_status1.TabIndex = 20;
            this.l_status1.Text = "No character loaded";
            // 
            // p_status1
            // 
            this.p_status1.BackColor = System.Drawing.Color.Silver;
            this.p_status1.Location = new System.Drawing.Point(15, 49);
            this.p_status1.Name = "p_status1";
            this.p_status1.Size = new System.Drawing.Size(24, 24);
            this.p_status1.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Select a character:";
            // 
            // b_loadCharacter1
            // 
            this.b_loadCharacter1.Location = new System.Drawing.Point(228, 12);
            this.b_loadCharacter1.Name = "b_loadCharacter1";
            this.b_loadCharacter1.Size = new System.Drawing.Size(64, 23);
            this.b_loadCharacter1.TabIndex = 17;
            this.b_loadCharacter1.Text = "load";
            this.b_loadCharacter1.UseVisualStyleBackColor = true;
            this.b_loadCharacter1.Click += new System.EventHandler(this.b_loadCharacter_Click);
            // 
            // cb_selectCharacter1
            // 
            this.cb_selectCharacter1.FormattingEnabled = true;
            this.cb_selectCharacter1.Location = new System.Drawing.Point(115, 12);
            this.cb_selectCharacter1.Name = "cb_selectCharacter1";
            this.cb_selectCharacter1.Size = new System.Drawing.Size(107, 21);
            this.cb_selectCharacter1.TabIndex = 16;
            // 
            // gb_characterName1
            // 
            this.gb_characterName1.Controls.Add(this.p_inventory1);
            this.gb_characterName1.Location = new System.Drawing.Point(12, 79);
            this.gb_characterName1.Name = "gb_characterName1";
            this.gb_characterName1.Size = new System.Drawing.Size(280, 296);
            this.gb_characterName1.TabIndex = 15;
            this.gb_characterName1.TabStop = false;
            this.gb_characterName1.Text = "Character";
            // 
            // p_inventory1
            // 
            this.p_inventory1.AutoScroll = true;
            this.p_inventory1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.p_inventory1.Location = new System.Drawing.Point(3, 16);
            this.p_inventory1.Name = "p_inventory1";
            this.p_inventory1.Size = new System.Drawing.Size(274, 277);
            this.p_inventory1.TabIndex = 0;
            // 
            // tc_characterShop
            // 
            this.tc_characterShop.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tc_characterShop.Controls.Add(this.tabPage1);
            this.tc_characterShop.Controls.Add(this.tabPage2);
            this.tc_characterShop.Location = new System.Drawing.Point(332, 12);
            this.tc_characterShop.Name = "tc_characterShop";
            this.tc_characterShop.SelectedIndex = 0;
            this.tc_characterShop.Size = new System.Drawing.Size(280, 363);
            this.tc_characterShop.TabIndex = 21;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.listView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(272, 334);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Shop Inventory";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(272, 334);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.b_undoTransfer);
            this.groupBox1.Controls.Add(this.b_transfer);
            this.groupBox1.Controls.Add(this.l_selectedItem);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.b_save);
            this.groupBox1.Location = new System.Drawing.Point(12, 381);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(600, 91);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options:";
            // 
            // b_undoTransfer
            // 
            this.b_undoTransfer.Location = new System.Drawing.Point(466, 15);
            this.b_undoTransfer.Name = "b_undoTransfer";
            this.b_undoTransfer.Size = new System.Drawing.Size(128, 32);
            this.b_undoTransfer.TabIndex = 17;
            this.b_undoTransfer.Text = "Undo transfer/Reload";
            this.b_undoTransfer.UseVisualStyleBackColor = true;
            // 
            // b_transfer
            // 
            this.b_transfer.Location = new System.Drawing.Point(6, 53);
            this.b_transfer.Name = "b_transfer";
            this.b_transfer.Size = new System.Drawing.Size(128, 32);
            this.b_transfer.TabIndex = 4;
            this.b_transfer.Text = "Add selected affix";
            this.b_transfer.UseVisualStyleBackColor = true;
            // 
            // l_selectedItem
            // 
            this.l_selectedItem.AutoSize = true;
            this.l_selectedItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_selectedItem.Location = new System.Drawing.Point(141, 17);
            this.l_selectedItem.Name = "l_selectedItem";
            this.l_selectedItem.Size = new System.Drawing.Size(54, 24);
            this.l_selectedItem.TabIndex = 16;
            this.l_selectedItem.Text = "none";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 24);
            this.label3.TabIndex = 15;
            this.label3.Text = "Selected item:";
            // 
            // b_save
            // 
            this.b_save.Location = new System.Drawing.Point(466, 53);
            this.b_save.Name = "b_save";
            this.b_save.Size = new System.Drawing.Size(128, 32);
            this.b_save.TabIndex = 10;
            this.b_save.Text = "Save traded items";
            this.b_save.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            listViewGroup1.Header = "Mod Slots";
            listViewGroup1.Name = "listViewGroup1";
            listViewGroup2.Header = "Affixes";
            listViewGroup2.Name = "listViewGroup2";
            this.listView1.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            listViewItem1.Group = listViewGroup1;
            listViewItem2.Group = listViewGroup1;
            listViewItem3.Group = listViewGroup1;
            listViewItem4.Group = listViewGroup1;
            listViewItem5.Group = listViewGroup1;
            listViewItem6.Group = listViewGroup2;
            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5,
            listViewItem6});
            this.listView1.Location = new System.Drawing.Point(3, 3);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(266, 328);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Quantity";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Price";
            // 
            // CharacterShop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 484);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tc_characterShop);
            this.Controls.Add(this.l_status1);
            this.Controls.Add(this.p_status1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.b_loadCharacter1);
            this.Controls.Add(this.cb_selectCharacter1);
            this.Controls.Add(this.gb_characterName1);
            this.Name = "CharacterShop";
            this.ShowIcon = false;
            this.Text = "CharacterShop";
            this.gb_characterName1.ResumeLayout(false);
            this.tc_characterShop.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label l_status1;
        private System.Windows.Forms.Panel p_status1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button b_loadCharacter1;
        private System.Windows.Forms.ComboBox cb_selectCharacter1;
        private System.Windows.Forms.GroupBox gb_characterName1;
        private System.Windows.Forms.Panel p_inventory1;
        private System.Windows.Forms.TabControl tc_characterShop;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button b_undoTransfer;
        private System.Windows.Forms.Button b_transfer;
        private System.Windows.Forms.Label l_selectedItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button b_save;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}