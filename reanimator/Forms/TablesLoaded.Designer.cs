﻿namespace Reanimator.Forms
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
            this.loadedTables_ListBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadedTables_ListBox.FormattingEnabled = true;
            this.loadedTables_ListBox.ItemHeight = 15;
            this.loadedTables_ListBox.Location = new System.Drawing.Point(12, 14);
            this.loadedTables_ListBox.Name = "loadedTables_ListBox";
            this.loadedTables_ListBox.Size = new System.Drawing.Size(299, 319);
            this.loadedTables_ListBox.TabIndex = 1;
            this.loadedTables_ListBox.Format += new System.Windows.Forms.ListControlConvertEventHandler(this._LoadedTables_ListBox_Format);
            this.loadedTables_ListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this._ListBox1_MouseDoubleClick);
            // 
            // TablesLoaded
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 346);
            this.ControlBox = false;
            this.Controls.Add(this.loadedTables_ListBox);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TablesLoaded";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Hellgate Tables Loaded";
            this.LocationChanged += new System.EventHandler(this.TablesLoaded_LocationChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox loadedTables_ListBox;
    }
}