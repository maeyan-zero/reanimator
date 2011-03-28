namespace Reanimator.Forms
{
    partial class TableEditorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer _components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (_components != null))
            {
                _components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void _InitializeComponent()
        {
            this._tabControl = new System.Windows.Forms.TabControl();
            this._splitContainer1 = new System.Windows.Forms.SplitContainer();
            this._splitContainer2 = new System.Windows.Forms.SplitContainer();
            this._reloadTableButton = new System.Windows.Forms.Button();
            this._closeTabButton = new System.Windows.Forms.Button();
            this._tableViewToggleButton = new System.Windows.Forms.Button();
            this._splitContainer1.Panel1.SuspendLayout();
            this._splitContainer1.Panel2.SuspendLayout();
            this._splitContainer1.SuspendLayout();
            this._splitContainer2.Panel2.SuspendLayout();
            this._splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this._tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabControl.Location = new System.Drawing.Point(0, 0);
            this._tabControl.Name = "tabControl";
            this._tabControl.SelectedIndex = 0;
            this._tabControl.Size = new System.Drawing.Size(486, 442);
            this._tabControl.TabIndex = 4;
            // 
            // splitContainer1
            // 
            this._splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this._splitContainer1.Location = new System.Drawing.Point(0, 0);
            this._splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this._splitContainer1.Panel1.Controls.Add(this._splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this._splitContainer1.Panel2.Controls.Add(this._tableViewToggleButton);
            this._splitContainer1.Panel2.Controls.Add(this._tabControl);
            this._splitContainer1.Size = new System.Drawing.Size(624, 442);
            this._splitContainer1.SplitterDistance = 134;
            this._splitContainer1.TabIndex = 5;
            // 
            // splitContainer2
            // 
            this._splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this._splitContainer2.Location = new System.Drawing.Point(0, 0);
            this._splitContainer2.Name = "splitContainer2";
            this._splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel2
            // 
            this._splitContainer2.Panel2.Controls.Add(this._reloadTableButton);
            this._splitContainer2.Panel2.Controls.Add(this._closeTabButton);
            this._splitContainer2.Size = new System.Drawing.Size(134, 442);
            this._splitContainer2.SplitterDistance = 363;
            this._splitContainer2.TabIndex = 1;
            // 
            // reloadTableButton
            // 
            this._reloadTableButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._reloadTableButton.Location = new System.Drawing.Point(28, 11);
            this._reloadTableButton.Name = "reloadTableButton";
            this._reloadTableButton.Size = new System.Drawing.Size(75, 23);
            this._reloadTableButton.TabIndex = 1;
            this._reloadTableButton.Text = "Reload";
            this._reloadTableButton.UseVisualStyleBackColor = true;
            this._reloadTableButton.Click += new System.EventHandler(this._reloadTableButton_Click);
            // 
            // closeTabButton
            // 
            this._closeTabButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._closeTabButton.Location = new System.Drawing.Point(28, 40);
            this._closeTabButton.Name = "closeTabButton";
            this._closeTabButton.Size = new System.Drawing.Size(75, 23);
            this._closeTabButton.TabIndex = 0;
            this._closeTabButton.Text = "Close Tab";
            this._closeTabButton.UseVisualStyleBackColor = true;
            this._closeTabButton.Click += new System.EventHandler(this._closeTabButton_Click);
            // 
            // tableViewToggleButton
            // 
            this._tableViewToggleButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this._tableViewToggleButton.Cursor = System.Windows.Forms.Cursors.Default;
            this._tableViewToggleButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this._tableViewToggleButton.Location = new System.Drawing.Point(4, 206);
            this._tableViewToggleButton.Name = "tableViewToggleButton";
            this._tableViewToggleButton.Size = new System.Drawing.Size(10, 50);
            this._tableViewToggleButton.TabIndex = 6;
            this._tableViewToggleButton.UseVisualStyleBackColor = true;
            this._tableViewToggleButton.Click += new System.EventHandler(this._tableViewToggleButton_Click);
            // 
            // ExcelTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 442);
            this.Controls.Add(this._splitContainer1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ExcelTableForm";
            this.Text = "Table Editor";
            this._splitContainer1.Panel1.ResumeLayout(false);
            this._splitContainer1.Panel2.ResumeLayout(false);
            this._splitContainer1.ResumeLayout(false);
            this._splitContainer2.Panel2.ResumeLayout(false);
            this._splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl _tabControl;
        private System.Windows.Forms.SplitContainer _splitContainer1;
        private System.Windows.Forms.Button _closeTabButton;
        private System.Windows.Forms.SplitContainer _splitContainer2;
        private System.Windows.Forms.Button _tableViewToggleButton;
        private System.Windows.Forms.Button _reloadTableButton;

    }
}