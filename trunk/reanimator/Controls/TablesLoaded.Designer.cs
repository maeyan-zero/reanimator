namespace Reanimator.Controls
{
    partial class TablesLoaded
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void _InitializeComponent()
        {
            this._loadedTables_ListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // loadedTables_ListBox
            // 
            this._loadedTables_ListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._loadedTables_ListBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._loadedTables_ListBox.FormattingEnabled = true;
            this._loadedTables_ListBox.ItemHeight = 15;
            this._loadedTables_ListBox.Location = new System.Drawing.Point(0, 0);
            this._loadedTables_ListBox.Name = "loadedTables_ListBox";
            this._loadedTables_ListBox.Size = new System.Drawing.Size(150, 150);
            this._loadedTables_ListBox.TabIndex = 2;
            // 
            // TablesLoaded
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._loadedTables_ListBox);
            this.Name = "TablesLoaded";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox _loadedTables_ListBox;
    }
}
