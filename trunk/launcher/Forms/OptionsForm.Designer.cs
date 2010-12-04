namespace Launcher.Forms
{
    partial class OptionsForm
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.hellgateLabel = new System.Windows.Forms.Label();
            this.hellgateTextBox = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.hellgateButton = new System.Windows.Forms.Button();
            this.savePath = new System.Windows.Forms.Label();
            this.saveTextBox = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.clientLabel = new System.Windows.Forms.Label();
            this.clientButton = new System.Windows.Forms.Button();
            this.clientTextBox = new System.Windows.Forms.TextBox();
            this.backupPath = new System.Windows.Forms.Label();
            this.backupTextBox = new System.Windows.Forms.TextBox();
            this.backupButton = new System.Windows.Forms.Button();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel.SuspendLayout();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 6;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel.Controls.Add(this.hellgateLabel, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.hellgateTextBox, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.cancelButton, 3, 5);
            this.tableLayoutPanel.Controls.Add(this.acceptButton, 2, 5);
            this.tableLayoutPanel.Controls.Add(this.hellgateButton, 4, 1);
            this.tableLayoutPanel.Controls.Add(this.savePath, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.saveTextBox, 2, 3);
            this.tableLayoutPanel.Controls.Add(this.saveButton, 4, 3);
            this.tableLayoutPanel.Controls.Add(this.clientLabel, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.clientButton, 4, 2);
            this.tableLayoutPanel.Controls.Add(this.clientTextBox, 2, 2);
            this.tableLayoutPanel.Controls.Add(this.backupPath, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.backupTextBox, 2, 4);
            this.tableLayoutPanel.Controls.Add(this.backupButton, 4, 4);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 7;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(529, 189);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // hellgateLabel
            // 
            this.hellgateLabel.AutoSize = true;
            this.hellgateLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hellgateLabel.Location = new System.Drawing.Point(25, 20);
            this.hellgateLabel.Name = "hellgateLabel";
            this.hellgateLabel.Size = new System.Drawing.Size(85, 30);
            this.hellgateLabel.TabIndex = 0;
            this.hellgateLabel.Text = "Hellgate London";
            this.hellgateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hellgateTextBox
            // 
            this.tableLayoutPanel.SetColumnSpan(this.hellgateTextBox, 2);
            this.hellgateTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hellgateTextBox.Location = new System.Drawing.Point(116, 23);
            this.hellgateTextBox.Name = "hellgateTextBox";
            this.hellgateTextBox.Size = new System.Drawing.Size(318, 20);
            this.hellgateTextBox.TabIndex = 1;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cancelButton.Location = new System.Drawing.Point(278, 143);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(156, 24);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // acceptButton
            // 
            this.acceptButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.acceptButton.Location = new System.Drawing.Point(116, 143);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(156, 24);
            this.acceptButton.TabIndex = 4;
            this.acceptButton.Text = "Accept";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // hellgateButton
            // 
            this.hellgateButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hellgateButton.Location = new System.Drawing.Point(440, 23);
            this.hellgateButton.Name = "hellgateButton";
            this.hellgateButton.Size = new System.Drawing.Size(64, 24);
            this.hellgateButton.TabIndex = 5;
            this.hellgateButton.Text = "Browse";
            this.hellgateButton.UseVisualStyleBackColor = true;
            this.hellgateButton.Click += new System.EventHandler(this.PathBrowseButton_Click);
            // 
            // savePath
            // 
            this.savePath.AutoSize = true;
            this.savePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.savePath.Location = new System.Drawing.Point(25, 80);
            this.savePath.Name = "savePath";
            this.savePath.Size = new System.Drawing.Size(85, 30);
            this.savePath.TabIndex = 10;
            this.savePath.Text = "Save Files";
            this.savePath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // saveTextBox
            // 
            this.tableLayoutPanel.SetColumnSpan(this.saveTextBox, 2);
            this.saveTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.saveTextBox.Location = new System.Drawing.Point(116, 83);
            this.saveTextBox.Name = "saveTextBox";
            this.saveTextBox.Size = new System.Drawing.Size(318, 20);
            this.saveTextBox.TabIndex = 11;
            // 
            // saveButton
            // 
            this.saveButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.saveButton.Location = new System.Drawing.Point(440, 83);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(64, 24);
            this.saveButton.TabIndex = 12;
            this.saveButton.Text = "Browse";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.PathBrowseButton_Click);
            // 
            // clientLabel
            // 
            this.clientLabel.AutoSize = true;
            this.clientLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clientLabel.Location = new System.Drawing.Point(25, 50);
            this.clientLabel.Name = "clientLabel";
            this.clientLabel.Size = new System.Drawing.Size(85, 30);
            this.clientLabel.TabIndex = 13;
            this.clientLabel.Text = "Default Client";
            this.clientLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // clientButton
            // 
            this.clientButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clientButton.Location = new System.Drawing.Point(440, 53);
            this.clientButton.Name = "clientButton";
            this.clientButton.Size = new System.Drawing.Size(64, 24);
            this.clientButton.TabIndex = 14;
            this.clientButton.Text = "Browse";
            this.clientButton.UseVisualStyleBackColor = true;
            this.clientButton.Click += new System.EventHandler(this.FileBrowseButton_Click);
            // 
            // clientTextBox
            // 
            this.tableLayoutPanel.SetColumnSpan(this.clientTextBox, 2);
            this.clientTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clientTextBox.Location = new System.Drawing.Point(116, 53);
            this.clientTextBox.Name = "clientTextBox";
            this.clientTextBox.Size = new System.Drawing.Size(318, 20);
            this.clientTextBox.TabIndex = 15;
            // 
            // backupPath
            // 
            this.backupPath.AutoSize = true;
            this.backupPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backupPath.Location = new System.Drawing.Point(25, 110);
            this.backupPath.Name = "backupPath";
            this.backupPath.Size = new System.Drawing.Size(85, 30);
            this.backupPath.TabIndex = 18;
            this.backupPath.Text = "Backups";
            this.backupPath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // backupTextBox
            // 
            this.tableLayoutPanel.SetColumnSpan(this.backupTextBox, 2);
            this.backupTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backupTextBox.Location = new System.Drawing.Point(116, 113);
            this.backupTextBox.Name = "backupTextBox";
            this.backupTextBox.Size = new System.Drawing.Size(318, 20);
            this.backupTextBox.TabIndex = 19;
            // 
            // backupButton
            // 
            this.backupButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backupButton.Location = new System.Drawing.Point(440, 113);
            this.backupButton.Name = "backupButton";
            this.backupButton.Size = new System.Drawing.Size(64, 24);
            this.backupButton.TabIndex = 20;
            this.backupButton.Text = "Browse";
            this.backupButton.UseVisualStyleBackColor = true;
            this.backupButton.Click += new System.EventHandler(this.PathBrowseButton_Click);
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.tableLayoutPanel);
            this.groupBox.Location = new System.Drawing.Point(12, 12);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(535, 208);
            this.groupBox.TabIndex = 1;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Paths";
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(559, 229);
            this.Controls.Add(this.groupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.groupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label hellgateLabel;
        private System.Windows.Forms.TextBox hellgateTextBox;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.Button hellgateButton;
        private System.Windows.Forms.Label savePath;
        private System.Windows.Forms.TextBox saveTextBox;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Label clientLabel;
        private System.Windows.Forms.Button clientButton;
        private System.Windows.Forms.TextBox clientTextBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Label backupPath;
        private System.Windows.Forms.TextBox backupTextBox;
        private System.Windows.Forms.Button backupButton;
    }
}