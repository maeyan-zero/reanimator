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
            this.toolsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.installModsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uninstallModsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.backupCharactersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.p_openHomePage = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.unleshHell = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(624, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolsToolStripMenuItem1
            // 
            this.toolsToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.installModsToolStripMenuItem,
            this.uninstallModsToolStripMenuItem});
            this.toolsToolStripMenuItem1.Name = "toolsToolStripMenuItem1";
            this.toolsToolStripMenuItem1.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem1.Text = "Tools";
            // 
            // installModsToolStripMenuItem
            // 
            this.installModsToolStripMenuItem.Name = "installModsToolStripMenuItem";
            this.installModsToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.installModsToolStripMenuItem.Text = "Install Mods...";
            this.installModsToolStripMenuItem.Click += new System.EventHandler(this.installToolStripMenuItem_Click);
            // 
            // uninstallModsToolStripMenuItem
            // 
            this.uninstallModsToolStripMenuItem.Name = "uninstallModsToolStripMenuItem";
            this.uninstallModsToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.uninstallModsToolStripMenuItem.Text = "Uninstall Mods...";
            this.uninstallModsToolStripMenuItem.Click += new System.EventHandler(this.revertToolStripMenuItem_Click);
            // 
            // programToolStripMenuItem
            // 
            this.programToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.programToolStripMenuItem.Name = "programToolStripMenuItem";
            this.programToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
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
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // installToolStripMenuItem
            // 
            this.installToolStripMenuItem.Name = "installToolStripMenuItem";
            this.installToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.installToolStripMenuItem.Text = "Install Modifications...";
            this.installToolStripMenuItem.Click += new System.EventHandler(this.installToolStripMenuItem_Click);
            // 
            // revertToolStripMenuItem
            // 
            this.revertToolStripMenuItem.Name = "revertToolStripMenuItem";
            this.revertToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.revertToolStripMenuItem.Text = "Uninstall Modifications...";
            this.revertToolStripMenuItem.Click += new System.EventHandler(this.revertToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(202, 6);
            // 
            // itemTransferToolStripMenuItem
            // 
            this.itemTransferToolStripMenuItem.Enabled = false;
            this.itemTransferToolStripMenuItem.Name = "itemTransferToolStripMenuItem";
            this.itemTransferToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.itemTransferToolStripMenuItem.Text = "Item Transfer...";
            this.itemTransferToolStripMenuItem.Click += new System.EventHandler(this.itemTransferToolStripMenuItem_Click);
            // 
            // enableHCCharacterToolStripMenuItem
            // 
            this.enableHCCharacterToolStripMenuItem.Enabled = false;
            this.enableHCCharacterToolStripMenuItem.Name = "enableHCCharacterToolStripMenuItem";
            this.enableHCCharacterToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.enableHCCharacterToolStripMenuItem.Text = "Enable Hardcore...";
            this.enableHCCharacterToolStripMenuItem.Click += new System.EventHandler(this.enableHCCharacterToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(202, 6);
            // 
            // backupCharactersToolStripMenuItem
            // 
            this.backupCharactersToolStripMenuItem.CheckOnClick = true;
            this.backupCharactersToolStripMenuItem.Enabled = false;
            this.backupCharactersToolStripMenuItem.Name = "backupCharactersToolStripMenuItem";
            this.backupCharactersToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.backupCharactersToolStripMenuItem.Text = "Backup Characters";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
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
            this.statusStrip1.Location = new System.Drawing.Point(0, 340);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(624, 22);
            this.statusStrip1.TabIndex = 17;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BackColor = System.Drawing.Color.Transparent;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // unleshHell
            // 
            this.unleshHell.AutoSize = true;
            this.unleshHell.BackColor = System.Drawing.Color.Transparent;
            this.unleshHell.Font = new System.Drawing.Font("Times New Roman", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.unleshHell.ForeColor = System.Drawing.Color.LightGoldenrodYellow;
            this.unleshHell.Location = new System.Drawing.Point(425, 288);
            this.unleshHell.Name = "unleshHell";
            this.unleshHell.Size = new System.Drawing.Size(178, 36);
            this.unleshHell.TabIndex = 18;
            this.unleshHell.Text = "Unleash Hell";
            this.unleshHell.MouseLeave += new System.EventHandler(this.unleshHell_MouseLeave);
            this.unleshHell.MouseEnter += new System.EventHandler(this.label1_MouseEnter);
            // 
            // Launcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(23)))), ((int)(((byte)(23)))));
            this.BackgroundImage = global::launcher.Properties.Resources.bg2;
            this.ClientSize = new System.Drawing.Size(624, 362);
            this.Controls.Add(this.unleshHell);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.p_openHomePage);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(640, 400);
            this.MinimumSize = new System.Drawing.Size(640, 400);
            this.Name = "Launcher";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hellgateaus.net Launcher";
            this.Load += new System.EventHandler(this.Launcher_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
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
        private System.Windows.Forms.Panel p_openHomePage;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem itemTransferToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem backupCharactersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem installModsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uninstallModsToolStripMenuItem;
        private System.Windows.Forms.Label unleshHell;

    }
}

