namespace Reanimator.Forms
{
    partial class TablesLoaded
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
            this.loadedTables_ListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // loadedTables_ListBox
            // 
            this.loadedTables_ListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.loadedTables_ListBox.FormattingEnabled = true;
            this.loadedTables_ListBox.Location = new System.Drawing.Point(12, 12);
            this.loadedTables_ListBox.Name = "loadedTables_ListBox";
            this.loadedTables_ListBox.Size = new System.Drawing.Size(270, 303);
            this.loadedTables_ListBox.TabIndex = 1;
            this.loadedTables_ListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDoubleClick);
            // 
            // TablesLoaded
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 326);
            this.ControlBox = false;
            this.Controls.Add(this.loadedTables_ListBox);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TablesLoaded";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ExcelTablesLoaded";
            this.LocationChanged += new System.EventHandler(this.TablesLoaded_LocationChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox loadedTables_ListBox;
    }
}