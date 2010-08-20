namespace Reanimator.Forms.ItemTransfer
{
    partial class BasicItemTransferForm
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
            this.b_transfer = new System.Windows.Forms.Button();
            this.p_inventory1 = new System.Windows.Forms.Panel();
            this.b_undoTransfer = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.l_selectedItem = new System.Windows.Forms.Label();
            this.b_transferAll = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.b_delete = new System.Windows.Forms.Button();
            this.b_save = new System.Windows.Forms.Button();
            this.l_status2 = new System.Windows.Forms.Label();
            this.l_status1 = new System.Windows.Forms.Label();
            this.p_status1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.p_status2 = new System.Windows.Forms.Panel();
            this.p_inventory2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.b_loadCharacter2 = new System.Windows.Forms.Button();
            this.cb_selectCharacter2 = new System.Windows.Forms.ComboBox();
            this.b_loadCharacter1 = new System.Windows.Forms.Button();
            this.gb_characterName2 = new System.Windows.Forms.GroupBox();
            this.cb_selectCharacter1 = new System.Windows.Forms.ComboBox();
            this.gb_characterName1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.nud_palladium = new System.Windows.Forms.NumericUpDown();
            this.b_tradeFrom1To2 = new System.Windows.Forms.Button();
            this.b_tradeFrom2To1 = new System.Windows.Forms.Button();
            this.l_palladium1 = new System.Windows.Forms.Label();
            this.l_palladium2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.gb_characterName2.SuspendLayout();
            this.gb_characterName1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_palladium)).BeginInit();
            this.SuspendLayout();
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
            // p_inventory1
            // 
            this.p_inventory1.AutoScroll = true;
            this.p_inventory1.BackColor = System.Drawing.Color.Black;
            this.p_inventory1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.p_inventory1.Location = new System.Drawing.Point(3, 16);
            this.p_inventory1.Name = "p_inventory1";
            this.p_inventory1.Size = new System.Drawing.Size(274, 277);
            this.p_inventory1.TabIndex = 0;
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.b_undoTransfer);
            this.groupBox1.Controls.Add(this.b_transfer);
            this.groupBox1.Controls.Add(this.l_selectedItem);
            this.groupBox1.Controls.Add(this.b_transferAll);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.b_delete);
            this.groupBox1.Controls.Add(this.b_save);
            this.groupBox1.Location = new System.Drawing.Point(12, 401);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(610, 101);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options:";
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
            // l_status2
            // 
            this.l_status2.AutoSize = true;
            this.l_status2.Location = new System.Drawing.Point(375, 49);
            this.l_status2.Name = "l_status2";
            this.l_status2.Size = new System.Drawing.Size(104, 13);
            this.l_status2.TabIndex = 29;
            this.l_status2.Text = "No character loaded";
            // 
            // l_status1
            // 
            this.l_status1.AutoSize = true;
            this.l_status1.Location = new System.Drawing.Point(45, 49);
            this.l_status1.Name = "l_status1";
            this.l_status1.Size = new System.Drawing.Size(104, 13);
            this.l_status1.TabIndex = 27;
            this.l_status1.Text = "No character loaded";
            // 
            // p_status1
            // 
            this.p_status1.BackColor = System.Drawing.Color.Silver;
            this.p_status1.Location = new System.Drawing.Point(15, 49);
            this.p_status1.Name = "p_status1";
            this.p_status1.Size = new System.Drawing.Size(24, 24);
            this.p_status1.TabIndex = 26;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(342, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "Select a character:";
            // 
            // p_status2
            // 
            this.p_status2.BackColor = System.Drawing.Color.Silver;
            this.p_status2.Location = new System.Drawing.Point(345, 49);
            this.p_status2.Name = "p_status2";
            this.p_status2.Size = new System.Drawing.Size(24, 24);
            this.p_status2.TabIndex = 28;
            // 
            // p_inventory2
            // 
            this.p_inventory2.AutoScroll = true;
            this.p_inventory2.BackColor = System.Drawing.Color.Black;
            this.p_inventory2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.p_inventory2.Location = new System.Drawing.Point(3, 16);
            this.p_inventory2.Name = "p_inventory2";
            this.p_inventory2.Size = new System.Drawing.Size(274, 277);
            this.p_inventory2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Select a character:";
            // 
            // b_loadCharacter2
            // 
            this.b_loadCharacter2.Location = new System.Drawing.Point(558, 12);
            this.b_loadCharacter2.Name = "b_loadCharacter2";
            this.b_loadCharacter2.Size = new System.Drawing.Size(64, 23);
            this.b_loadCharacter2.TabIndex = 23;
            this.b_loadCharacter2.Text = "load";
            this.b_loadCharacter2.UseVisualStyleBackColor = true;
            this.b_loadCharacter2.Click += new System.EventHandler(this.b_loadCharacter2_Click);
            // 
            // cb_selectCharacter2
            // 
            this.cb_selectCharacter2.FormattingEnabled = true;
            this.cb_selectCharacter2.Location = new System.Drawing.Point(445, 12);
            this.cb_selectCharacter2.Name = "cb_selectCharacter2";
            this.cb_selectCharacter2.Size = new System.Drawing.Size(107, 21);
            this.cb_selectCharacter2.TabIndex = 22;
            this.cb_selectCharacter2.SelectedIndexChanged += new System.EventHandler(this.cb_selectCharacter2_SelectedIndexChanged);
            // 
            // b_loadCharacter1
            // 
            this.b_loadCharacter1.Location = new System.Drawing.Point(228, 12);
            this.b_loadCharacter1.Name = "b_loadCharacter1";
            this.b_loadCharacter1.Size = new System.Drawing.Size(64, 23);
            this.b_loadCharacter1.TabIndex = 21;
            this.b_loadCharacter1.Text = "load";
            this.b_loadCharacter1.UseVisualStyleBackColor = true;
            this.b_loadCharacter1.Click += new System.EventHandler(this.b_loadCharacter1_Click);
            // 
            // gb_characterName2
            // 
            this.gb_characterName2.Controls.Add(this.p_inventory2);
            this.gb_characterName2.Location = new System.Drawing.Point(342, 79);
            this.gb_characterName2.Name = "gb_characterName2";
            this.gb_characterName2.Size = new System.Drawing.Size(280, 296);
            this.gb_characterName2.TabIndex = 20;
            this.gb_characterName2.TabStop = false;
            this.gb_characterName2.Text = "Character 2";
            // 
            // cb_selectCharacter1
            // 
            this.cb_selectCharacter1.FormattingEnabled = true;
            this.cb_selectCharacter1.Location = new System.Drawing.Point(115, 12);
            this.cb_selectCharacter1.Name = "cb_selectCharacter1";
            this.cb_selectCharacter1.Size = new System.Drawing.Size(107, 21);
            this.cb_selectCharacter1.TabIndex = 19;
            this.cb_selectCharacter1.SelectedIndexChanged += new System.EventHandler(this.cb_selectCharacter1_SelectedIndexChanged);
            // 
            // gb_characterName1
            // 
            this.gb_characterName1.Controls.Add(this.p_inventory1);
            this.gb_characterName1.Location = new System.Drawing.Point(12, 79);
            this.gb_characterName1.Name = "gb_characterName1";
            this.gb_characterName1.Size = new System.Drawing.Size(280, 296);
            this.gb_characterName1.TabIndex = 17;
            this.gb_characterName1.TabStop = false;
            this.gb_characterName1.Text = "Character 1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 380);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 17);
            this.label4.TabIndex = 30;
            this.label4.Text = "Palladium";
            // 
            // nud_palladium
            // 
            this.nud_palladium.Enabled = false;
            this.nud_palladium.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nud_palladium.Increment = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nud_palladium.Location = new System.Drawing.Point(277, 378);
            this.nud_palladium.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.nud_palladium.Name = "nud_palladium";
            this.nud_palladium.Size = new System.Drawing.Size(80, 23);
            this.nud_palladium.TabIndex = 32;
            this.nud_palladium.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_palladium.ThousandsSeparator = true;
            this.nud_palladium.Value = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            // 
            // b_tradeFrom1To2
            // 
            this.b_tradeFrom1To2.Enabled = false;
            this.b_tradeFrom1To2.Location = new System.Drawing.Point(363, 378);
            this.b_tradeFrom1To2.Name = "b_tradeFrom1To2";
            this.b_tradeFrom1To2.Size = new System.Drawing.Size(28, 23);
            this.b_tradeFrom1To2.TabIndex = 34;
            this.b_tradeFrom1To2.Text = ">>";
            this.b_tradeFrom1To2.UseVisualStyleBackColor = true;
            // 
            // b_tradeFrom2To1
            // 
            this.b_tradeFrom2To1.Enabled = false;
            this.b_tradeFrom2To1.Location = new System.Drawing.Point(243, 378);
            this.b_tradeFrom2To1.Name = "b_tradeFrom2To1";
            this.b_tradeFrom2To1.Size = new System.Drawing.Size(28, 23);
            this.b_tradeFrom2To1.TabIndex = 35;
            this.b_tradeFrom2To1.Text = "<<";
            this.b_tradeFrom2To1.UseVisualStyleBackColor = true;
            this.b_tradeFrom2To1.Click += new System.EventHandler(this.b_tradeFrom2To1_Click);
            // 
            // l_palladium1
            // 
            this.l_palladium1.AutoSize = true;
            this.l_palladium1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_palladium1.Location = new System.Drawing.Point(87, 380);
            this.l_palladium1.MaximumSize = new System.Drawing.Size(150, 17);
            this.l_palladium1.MinimumSize = new System.Drawing.Size(150, 17);
            this.l_palladium1.Name = "l_palladium1";
            this.l_palladium1.Size = new System.Drawing.Size(150, 17);
            this.l_palladium1.TabIndex = 36;
            this.l_palladium1.Text = "-";
            this.l_palladium1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // l_palladium2
            // 
            this.l_palladium2.AutoSize = true;
            this.l_palladium2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_palladium2.Location = new System.Drawing.Point(397, 380);
            this.l_palladium2.MaximumSize = new System.Drawing.Size(150, 17);
            this.l_palladium2.MinimumSize = new System.Drawing.Size(150, 17);
            this.l_palladium2.Name = "l_palladium2";
            this.l_palladium2.Size = new System.Drawing.Size(150, 17);
            this.l_palladium2.TabIndex = 37;
            this.l_palladium2.Text = "-";
            this.l_palladium2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(553, 380);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 17);
            this.label5.TabIndex = 38;
            this.label5.Text = "Palladium";
            // 
            // BasicItemTransferForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 514);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.l_palladium2);
            this.Controls.Add(this.l_palladium1);
            this.Controls.Add(this.b_tradeFrom2To1);
            this.Controls.Add(this.b_tradeFrom1To2);
            this.Controls.Add(this.nud_palladium);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.l_status2);
            this.Controls.Add(this.l_status1);
            this.Controls.Add(this.p_status1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.p_status2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.b_loadCharacter2);
            this.Controls.Add(this.cb_selectCharacter2);
            this.Controls.Add(this.b_loadCharacter1);
            this.Controls.Add(this.gb_characterName2);
            this.Controls.Add(this.cb_selectCharacter1);
            this.Controls.Add(this.gb_characterName1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "BasicItemTransferForm";
            this.ShowIcon = false;
            this.Text = "Trade Items";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gb_characterName2.ResumeLayout(false);
            this.gb_characterName1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nud_palladium)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Button b_transfer;
        protected System.Windows.Forms.Panel p_inventory1;
        protected System.Windows.Forms.Button b_undoTransfer;
        protected System.Windows.Forms.GroupBox groupBox1;
        protected System.Windows.Forms.Label l_selectedItem;
        protected System.Windows.Forms.Button b_transferAll;
        protected System.Windows.Forms.Label label3;
        protected System.Windows.Forms.Button b_delete;
        protected System.Windows.Forms.Button b_save;
        protected System.Windows.Forms.Label l_status2;
        protected System.Windows.Forms.Label l_status1;
        protected System.Windows.Forms.Panel p_status1;
        protected System.Windows.Forms.Label label2;
        protected System.Windows.Forms.Panel p_status2;
        protected System.Windows.Forms.Panel p_inventory2;
        protected System.Windows.Forms.Label label1;
        protected System.Windows.Forms.Button b_loadCharacter2;
        protected System.Windows.Forms.ComboBox cb_selectCharacter2;
        protected System.Windows.Forms.Button b_loadCharacter1;
        protected System.Windows.Forms.GroupBox gb_characterName2;
        protected System.Windows.Forms.ComboBox cb_selectCharacter1;
        protected System.Windows.Forms.GroupBox gb_characterName1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nud_palladium;
        private System.Windows.Forms.Button b_tradeFrom1To2;
        private System.Windows.Forms.Button b_tradeFrom2To1;
        private System.Windows.Forms.Label l_palladium1;
        private System.Windows.Forms.Label l_palladium2;
        private System.Windows.Forms.Label label5;

    }
}