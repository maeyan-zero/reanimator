namespace ItemShopPlugin
{
  partial class ItemShopForm
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
      System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Accuracy",
            "+1",
            "50000"}, -1);
      System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "Strength",
            "+1",
            "50000"}, -1);
      System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "Stamina",
            "+1",
            "50000"}, -1);
      System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "Willpower",
            "+1",
            "50000"}, -1);
      System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] {
            "Shields",
            "+25",
            "100000"}, -1);
      System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] {
            "Armor",
            "+5",
            "50000"}, -1);
      System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem(new string[] {
            "Nanoshard",
            "+10",
            "50000"}, -1);
      System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem(new string[] {
            "Damage",
            "+10%",
            "50000"}, -1);
      System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem(new string[] {
            "Damage",
            "+15%",
            "100000"}, -1);
      System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem(new string[] {
            "Damage",
            "+20%",
            "200000"}, -1);
      System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem(new string[] {
            "Damage",
            "+25%",
            "300000"}, -1);
      System.Windows.Forms.ListViewItem listViewItem12 = new System.Windows.Forms.ListViewItem(new string[] {
            "Damage",
            "+30%",
            "500000"}, -1);
      System.Windows.Forms.ListViewItem listViewItem13 = new System.Windows.Forms.ListViewItem(new string[] {
            "Select Skill",
            "+1",
            "50000"}, -1);
      System.Windows.Forms.ListViewItem listViewItem14 = new System.Windows.Forms.ListViewItem(new string[] {
            "Select Skill",
            "+2",
            "150000"}, -1);
      System.Windows.Forms.ListViewItem listViewItem15 = new System.Windows.Forms.ListViewItem(new string[] {
            "Select Skill",
            "+3",
            "500000"}, -1);
      this.button1 = new System.Windows.Forms.Button();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.clb_char1 = new System.Windows.Forms.CheckedListBox();
      this.cb_char1 = new System.Windows.Forms.ComboBox();
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.tabPage1 = new System.Windows.Forms.TabPage();
      this.tabPage2 = new System.Windows.Forms.TabPage();
      this.listView1 = new System.Windows.Forms.ListView();
      this.ch_item = new System.Windows.Forms.ColumnHeader();
      this.ch_amount = new System.Windows.Forms.ColumnHeader();
      this.ch_price = new System.Windows.Forms.ColumnHeader();
      this.listView2 = new System.Windows.Forms.ListView();
      this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
      this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
      this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
      this.groupBox2.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.tabPage2.SuspendLayout();
      this.SuspendLayout();
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(218, 31);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(28, 21);
      this.button1.TabIndex = 6;
      this.button1.Text = "<<";
      this.button1.UseVisualStyleBackColor = true;
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.tabControl1);
      this.groupBox2.Location = new System.Drawing.Point(252, 12);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(220, 240);
      this.groupBox2.TabIndex = 5;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Shop";
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.clb_char1);
      this.groupBox1.Controls.Add(this.cb_char1);
      this.groupBox1.Location = new System.Drawing.Point(12, 12);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(200, 240);
      this.groupBox1.TabIndex = 4;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Character";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(67, 45);
      this.label2.MaximumSize = new System.Drawing.Size(127, 13);
      this.label2.MinimumSize = new System.Drawing.Size(127, 13);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(127, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "0";
      this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 45);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(55, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Palladium:";
      // 
      // clb_char1
      // 
      this.clb_char1.FormattingEnabled = true;
      this.clb_char1.Location = new System.Drawing.Point(6, 61);
      this.clb_char1.Name = "clb_char1";
      this.clb_char1.Size = new System.Drawing.Size(188, 169);
      this.clb_char1.TabIndex = 1;
      // 
      // cb_char1
      // 
      this.cb_char1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cb_char1.FormattingEnabled = true;
      this.cb_char1.Location = new System.Drawing.Point(6, 19);
      this.cb_char1.Name = "cb_char1";
      this.cb_char1.Size = new System.Drawing.Size(188, 21);
      this.cb_char1.TabIndex = 0;
      // 
      // tabControl1
      // 
      this.tabControl1.Controls.Add(this.tabPage1);
      this.tabControl1.Controls.Add(this.tabPage2);
      this.tabControl1.Location = new System.Drawing.Point(6, 19);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(208, 215);
      this.tabControl1.TabIndex = 0;
      // 
      // tabPage1
      // 
      this.tabPage1.Controls.Add(this.listView1);
      this.tabPage1.Location = new System.Drawing.Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage1.Size = new System.Drawing.Size(200, 189);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Character";
      this.tabPage1.UseVisualStyleBackColor = true;
      // 
      // tabPage2
      // 
      this.tabPage2.Controls.Add(this.listView2);
      this.tabPage2.Location = new System.Drawing.Point(4, 22);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage2.Size = new System.Drawing.Size(200, 189);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "Items";
      this.tabPage2.UseVisualStyleBackColor = true;
      // 
      // listView1
      // 
      this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ch_item,
            this.ch_amount,
            this.ch_price});
      this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5,
            listViewItem6,
            listViewItem7});
      this.listView1.Location = new System.Drawing.Point(3, 3);
      this.listView1.Name = "listView1";
      this.listView1.Size = new System.Drawing.Size(194, 183);
      this.listView1.TabIndex = 1;
      this.listView1.UseCompatibleStateImageBehavior = false;
      this.listView1.View = System.Windows.Forms.View.Details;
      // 
      // ch_item
      // 
      this.ch_item.Text = "Item";
      this.ch_item.Width = 70;
      // 
      // ch_amount
      // 
      this.ch_amount.Text = "Amount";
      this.ch_amount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.ch_amount.Width = 48;
      // 
      // ch_price
      // 
      this.ch_price.Text = "Price";
      this.ch_price.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.ch_price.Width = 48;
      // 
      // listView2
      // 
      this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
      this.listView2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listView2.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem8,
            listViewItem9,
            listViewItem10,
            listViewItem11,
            listViewItem12,
            listViewItem13,
            listViewItem14,
            listViewItem15});
      this.listView2.Location = new System.Drawing.Point(3, 3);
      this.listView2.Name = "listView2";
      this.listView2.Size = new System.Drawing.Size(194, 183);
      this.listView2.TabIndex = 2;
      this.listView2.UseCompatibleStateImageBehavior = false;
      this.listView2.View = System.Windows.Forms.View.Details;
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Item";
      this.columnHeader1.Width = 70;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Amount";
      this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.columnHeader2.Width = 48;
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "Price";
      this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.columnHeader3.Width = 48;
      // 
      // ItemShopForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(484, 264);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.groupBox1);
      this.Name = "ItemShopForm";
      this.Text = "ItemShopForm";
      this.groupBox2.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.tabPage2.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.CheckedListBox clb_char1;
    private System.Windows.Forms.ComboBox cb_char1;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.ListView listView1;
    private System.Windows.Forms.ColumnHeader ch_item;
    private System.Windows.Forms.ColumnHeader ch_amount;
    private System.Windows.Forms.ColumnHeader ch_price;
    private System.Windows.Forms.TabPage tabPage2;
    private System.Windows.Forms.ListView listView2;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.ColumnHeader columnHeader3;

  }
}