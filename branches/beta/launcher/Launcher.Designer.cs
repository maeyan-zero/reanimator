namespace launcher
{
    partial class Launcher
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.programToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.installToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.revertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.itemTransferToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableHCCharacterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.characterCombo = new System.Windows.Forms.ComboBox();
            this.p_start = new System.Windows.Forms.PictureBox();
            this.p_openHomePage = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.backupCharactersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.p_start)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.programToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(632, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // programToolStripMenuItem
            // 
            this.programToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.programToolStripMenuItem.Name = "programToolStripMenuItem";
            this.programToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.programToolStripMenuItem.Text = "Program";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.settingsToolStripMenuItem.Text = "Settings...";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(122, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.installToolStripMenuItem,
            this.revertToolStripMenuItem,
            this.toolStripSeparator1,
            this.itemTransferToolStripMenuItem,
            this.enableHCCharacterToolStripMenuItem,
            this.toolStripSeparator3,
            this.backupCharactersToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // installToolStripMenuItem
            // 
            this.installToolStripMenuItem.Name = "installToolStripMenuItem";
            this.installToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.installToolStripMenuItem.Text = "Install Modifications...";
            this.installToolStripMenuItem.Click += new System.EventHandler(this.installToolStripMenuItem_Click);
            // 
            // revertToolStripMenuItem
            // 
            this.revertToolStripMenuItem.Name = "revertToolStripMenuItem";
            this.revertToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.revertToolStripMenuItem.Text = "Uninstall Modifications...";
            this.revertToolStripMenuItem.Click += new System.EventHandler(this.revertToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(188, 6);
            // 
            // itemTransferToolStripMenuItem
            // 
            this.itemTransferToolStripMenuItem.Name = "itemTransferToolStripMenuItem";
            this.itemTransferToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.itemTransferToolStripMenuItem.Text = "Item Transfer...";
            this.itemTransferToolStripMenuItem.Click += new System.EventHandler(this.itemTransferToolStripMenuItem_Click);
            // 
            // enableHCCharacterToolStripMenuItem
            // 
            this.enableHCCharacterToolStripMenuItem.Name = "enableHCCharacterToolStripMenuItem";
            this.enableHCCharacterToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.enableHCCharacterToolStripMenuItem.Text = "Enable Hardcore...";
            this.enableHCCharacterToolStripMenuItem.Click += new System.EventHandler(this.enableHCCharacterToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // characterCombo
            // 
            this.characterCombo.BackColor = System.Drawing.Color.Black;
            this.characterCombo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.characterCombo.ForeColor = System.Drawing.Color.DarkOrange;
            this.characterCombo.FormattingEnabled = true;
            this.characterCombo.Location = new System.Drawing.Point(472, 390);
            this.characterCombo.Name = "characterCombo";
            this.characterCombo.Size = new System.Drawing.Size(146, 21);
            this.characterCombo.TabIndex = 7;
            this.characterCombo.Text = "Select a character";
            // 
            // p_start
            // 
            this.p_start.BackColor = System.Drawing.Color.Transparent;
            this.p_start.BackgroundImage = global::launcher.Properties.Resources.templar_normal;
            this.p_start.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.p_start.Location = new System.Drawing.Point(468, 214);
            this.p_start.Name = "p_start";
            this.p_start.Size = new System.Drawing.Size(146, 180);
            this.p_start.TabIndex = 13;
            this.p_start.TabStop = false;
            this.p_start.MouseLeave += new System.EventHandler(this.p_start_MouseLeave);
            this.p_start.Click += new System.EventHandler(this.p_start_Click);
            this.p_start.MouseDown += new System.Windows.Forms.MouseEventHandler(this.p_start_MouseDown);
            this.p_start.MouseUp += new System.Windows.Forms.MouseEventHandler(this.p_start_MouseUp);
            this.p_start.MouseEnter += new System.EventHandler(this.p_start_MouseEnter);
            // 
            // p_openHomePage
            // 
            this.p_openHomePage.BackColor = System.Drawing.Color.Transparent;
            this.p_openHomePage.Location = new System.Drawing.Point(106, 376);
            this.p_openHomePage.Name = "p_openHomePage";
            this.p_openHomePage.Size = new System.Drawing.Size(223, 28);
            this.p_openHomePage.TabIndex = 16;
            this.p_openHomePage.MouseLeave += new System.EventHandler(this.p_openHomePage_MouseLeave);
            this.p_openHomePage.Click += new System.EventHandler(this.p_homePageLink_Click);
            this.p_openHomePage.MouseEnter += new System.EventHandler(this.p_openHomePage_MouseEnter);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 431);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(632, 22);
            this.statusStrip1.TabIndex = 17;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BackColor = System.Drawing.Color.Transparent;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.DarkOrange;
            this.label1.Location = new System.Drawing.Point(476, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "RSS Feed";
            // 
            // backupCharactersToolStripMenuItem
            // 
            this.backupCharactersToolStripMenuItem.CheckOnClick = true;
            this.backupCharactersToolStripMenuItem.Name = "backupCharactersToolStripMenuItem";
            this.backupCharactersToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.backupCharactersToolStripMenuItem.Text = "Backup Characters";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(188, 6);
            // 
            // Launcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(23)))), ((int)(((byte)(23)))));
            this.BackgroundImage = global::launcher.Properties.Resources.background4;
            this.ClientSize = new System.Drawing.Size(632, 453);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.p_openHomePage);
            this.Controls.Add(this.characterCombo);
            this.Controls.Add(this.p_start);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(640, 480);
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "Launcher";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HellgateAus.net Launcher 2038";
            this.Load += new System.EventHandler(this.Launcher_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.p_start)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem programToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem installToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem revertToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem enableHCCharacterToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ComboBox characterCombo;
        private System.Windows.Forms.PictureBox p_start;
        private System.Windows.Forms.Panel p_openHomePage;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem itemTransferToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem backupCharactersToolStripMenuItem;

    }
}

