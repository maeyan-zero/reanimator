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
            this.p_inventory1 = new System.Windows.Forms.Panel();
            this.cb_selectCharacter1 = new System.Windows.Forms.ComboBox();
            this.gb_characterName2 = new System.Windows.Forms.GroupBox();
            this.p_inventory2 = new System.Windows.Forms.Panel();
            this.b_transfer = new System.Windows.Forms.Button();
            this.b_transferAll = new System.Windows.Forms.Button();
            this.b_loadCharacter1 = new System.Windows.Forms.Button();
            this.b_loadCharacter2 = new System.Windows.Forms.Button();
            this.cb_selectCharacter2 = new System.Windows.Forms.ComboBox();
            this.b_save = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.b_delete = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.l_selectedItem = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.b_undoTransfer = new System.Windows.Forms.Button();
            this.p_status1 = new System.Windows.Forms.Panel();
            this.l_status1 = new System.Windows.Forms.Label();
            this.l_status2 = new System.Windows.Forms.Label();
            this.p_status2 = new System.Windows.Forms.Panel();
            this.gb_characterName1.SuspendLayout();
            this.gb_characterName2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gb_characterName1
            // 
            this.gb_characterName1.Controls.Add(this.p_inventory1);
            this.gb_characterName1.Location = new System.Drawing.Point(12, 79);
            this.gb_characterName1.Name = "gb_characterName1";
            this.gb_characterName1.Size = new System.Drawing.Size(280, 296);
            this.gb_characterName1.TabIndex = 0;
            this.gb_characterName1.TabStop = false;
            this.gb_characterName1.Text = "Character 1";
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
            this.gb_characterName2.Controls.Add(this.p_inventory2);
            this.gb_characterName2.Location = new System.Drawing.Point(332, 79);
            this.gb_characterName2.Name = "gb_characterName2";
            this.gb_characterName2.Size = new System.Drawing.Size(280, 296);
            this.gb_characterName2.TabIndex = 2;
            this.gb_characterName2.TabStop = false;
            this.gb_characterName2.Text = "Character 2";
            // 
            // p_inventory2
            // 
            this.p_inventory2.AutoScroll = true;
            this.p_inventory2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.p_inventory2.Location = new System.Drawing.Point(3, 16);
            this.p_inventory2.Name = "p_inventory2";
            this.p_inventory2.Size = new System.Drawing.Size(274, 277);
            this.p_inventory2.TabIndex = 0;
            // 
            // b_transfer
            // 
            this.b_transfer.Location = new System.Drawing.Point(6, 53);
            this.b_transfer.Name = "b_transfer";
            this.b_transfer.Size = new System.Drawing.Size(128, 32);
            this.b_transfer.TabIndex = 4;
            this.b_transfer.Text = "Trade selected item";
            this.b_transfer.UseVisualStyleBackColor = true;
            this.b_transfer.Click += new System.EventHandler(this.b_transfer_Click);
            // 
            // b_transferAll
            // 
            this.b_transferAll.Location = new System.Drawing.Point(140, 53);
            this.b_transferAll.Name = "b_transferAll";
            this.b_transferAll.Size = new System.Drawing.Size(128, 32);
            this.b_transferAll.TabIndex = 5;
            this.b_transferAll.Text = "Exchange items";
            this.b_transferAll.UseVisualStyleBackColor = true;
            this.b_transferAll.Click += new System.EventHandler(this.b_transferAll_Click);
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
            this.b_save.Location = new System.Drawing.Point(466, 53);
            this.b_save.Name = "b_save";
            this.b_save.Size = new System.Drawing.Size(128, 32);
            this.b_save.TabIndex = 10;
            this.b_save.Text = "Save traded items";
            this.b_save.UseVisualStyleBackColor = true;
            this.b_save.Click += new System.EventHandler(this.b_save_Click);
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
            // b_delete
            // 
            this.b_delete.Location = new System.Drawing.Point(274, 53);
            this.b_delete.Name = "b_delete";
            this.b_delete.Size = new System.Drawing.Size(128, 32);
            this.b_delete.TabIndex = 13;
            this.b_delete.Text = "Delete Item";
            this.b_delete.UseVisualStyleBackColor = true;
            this.b_delete.Click += new System.EventHandler(this.b_delete_Click);
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.b_undoTransfer);
            this.groupBox1.Controls.Add(this.b_transfer);
            this.groupBox1.Controls.Add(this.l_selectedItem);
            this.groupBox1.Controls.Add(this.b_transferAll);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.b_delete);
            this.groupBox1.Controls.Add(this.b_save);
            this.groupBox1.Location = new System.Drawing.Point(12, 381);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(600, 91);
            this.groupBox1.TabIndex = 0;
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
            this.b_undoTransfer.Click += new System.EventHandler(this.b_undoTransfer_Click);
            // 
            // p_status1
            // 
            this.p_status1.BackColor = System.Drawing.Color.Silver;
            this.p_status1.Location = new System.Drawing.Point(15, 49);
            this.p_status1.Name = "p_status1";
            this.p_status1.Size = new System.Drawing.Size(24, 24);
            this.p_status1.TabIndex = 13;
            // 
            // l_status1
            // 
            this.l_status1.AutoSize = true;
            this.l_status1.Location = new System.Drawing.Point(45, 49);
            this.l_status1.Name = "l_status1";
            this.l_status1.Size = new System.Drawing.Size(104, 13);
            this.l_status1.TabIndex = 14;
            this.l_status1.Text = "No character loaded";
            // 
            // l_status2
            // 
            this.l_status2.AutoSize = true;
            this.l_status2.Location = new System.Drawing.Point(365, 49);
            this.l_status2.Name = "l_status2";
            this.l_status2.Size = new System.Drawing.Size(104, 13);
            this.l_status2.TabIndex = 16;
            this.l_status2.Text = "No character loaded";
            // 
            // p_status2
            // 
            this.p_status2.BackColor = System.Drawing.Color.Silver;
            this.p_status2.Location = new System.Drawing.Point(335, 49);
            this.p_status2.Name = "p_status2";
            this.p_status2.Size = new System.Drawing.Size(24, 24);
            this.p_status2.TabIndex = 15;
            // 
            // ItemTransferForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(624, 484);
            this.Controls.Add(this.l_status2);
            this.Controls.Add(this.l_status1);
            this.Controls.Add(this.p_status2);
            this.Controls.Add(this.p_status1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.b_loadCharacter2);
            this.Controls.Add(this.cb_selectCharacter2);
            this.Controls.Add(this.b_loadCharacter1);
            this.Controls.Add(this.gb_characterName2);
            this.Controls.Add(this.cb_selectCharacter1);
            this.Controls.Add(this.gb_characterName1);
            this.DoubleBuffered = true;
            this.Name = "ItemTransferForm";
            this.ShowIcon = false;
            this.Text = "Trade Items";
            this.gb_characterName1.ResumeLayout(false);
            this.gb_characterName2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_characterName1;
        private System.Windows.Forms.ComboBox cb_selectCharacter1;
        private System.Windows.Forms.GroupBox gb_characterName2;
        private System.Windows.Forms.Button b_transfer;
        private System.Windows.Forms.Button b_transferAll;
        private System.Windows.Forms.Button b_loadCharacter1;
        private System.Windows.Forms.Button b_loadCharacter2;
        private System.Windows.Forms.ComboBox cb_selectCharacter2;
        private System.Windows.Forms.Button b_save;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button b_delete;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label l_selectedItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel p_inventory1;
        private System.Windows.Forms.Panel p_inventory2;
        private System.Windows.Forms.Button b_undoTransfer;
        private System.Windows.Forms.Panel p_status1;
        private System.Windows.Forms.Label l_status1;
        private System.Windows.Forms.Label l_status2;
        private System.Windows.Forms.Panel p_status2;
    }
}