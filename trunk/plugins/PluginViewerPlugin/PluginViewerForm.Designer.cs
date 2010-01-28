namespace PluginViewer
{
  partial class PluginViewerForm
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
      this.lv_pluginList = new System.Windows.Forms.ListView();
      this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
      this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
      this.SuspendLayout();
      // 
      // lv_pluginList
      // 
      this.lv_pluginList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
      this.lv_pluginList.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lv_pluginList.FullRowSelect = true;
      this.lv_pluginList.Location = new System.Drawing.Point(0, 0);
      this.lv_pluginList.Name = "lv_pluginList";
      this.lv_pluginList.Size = new System.Drawing.Size(464, 204);
      this.lv_pluginList.TabIndex = 0;
      this.lv_pluginList.UseCompatibleStateImageBehavior = false;
      this.lv_pluginList.View = System.Windows.Forms.View.Details;
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Plugin";
      this.columnHeader1.Width = 80;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Description";
      this.columnHeader2.Width = 360;
      // 
      // PluginViewerForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(464, 204);
      this.Controls.Add(this.lv_pluginList);
      this.Name = "PluginViewerForm";
      this.ShowIcon = false;
      this.Text = "Loaded Plugins";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView lv_pluginList;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader2;

  }
}