namespace Reanimator.Controls
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

        #region Component Designer generated code

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
            this.loadedTables_ListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loadedTables_ListBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadedTables_ListBox.FormattingEnabled = true;
            this.loadedTables_ListBox.ItemHeight = 15;
            this.loadedTables_ListBox.Location = new System.Drawing.Point(0, 0);
            this.loadedTables_ListBox.Name = "loadedTables_ListBox";
            this.loadedTables_ListBox.Size = new System.Drawing.Size(150, 150);
            this.loadedTables_ListBox.TabIndex = 2;
            // 
            // TablesLoaded
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.loadedTables_ListBox);
            this.Name = "TablesLoaded";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox loadedTables_ListBox;
    }
}
