namespace ItemTransferPlugin
{
  partial class ItemTransferForm
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
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.clb_char1 = new System.Windows.Forms.CheckedListBox();
      this.cb_char1 = new System.Windows.Forms.ComboBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.clb_char2 = new System.Windows.Forms.CheckedListBox();
      this.cb_char2 = new System.Windows.Forms.ComboBox();
      this.button1 = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.clb_char1);
      this.groupBox1.Controls.Add(this.cb_char1);
      this.groupBox1.Location = new System.Drawing.Point(12, 12);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(200, 240);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Character 1";
      // 
      // clb_char1
      // 
      this.clb_char1.FormattingEnabled = true;
      this.clb_char1.Location = new System.Drawing.Point(6, 46);
      this.clb_char1.Name = "clb_char1";
      this.clb_char1.Size = new System.Drawing.Size(188, 184);
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
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.clb_char2);
      this.groupBox2.Controls.Add(this.cb_char2);
      this.groupBox2.Location = new System.Drawing.Point(252, 12);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(200, 240);
      this.groupBox2.TabIndex = 1;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Character 2";
      // 
      // clb_char2
      // 
      this.clb_char2.FormattingEnabled = true;
      this.clb_char2.Location = new System.Drawing.Point(6, 46);
      this.clb_char2.Name = "clb_char2";
      this.clb_char2.Size = new System.Drawing.Size(188, 184);
      this.clb_char2.TabIndex = 2;
      // 
      // cb_char2
      // 
      this.cb_char2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cb_char2.FormattingEnabled = true;
      this.cb_char2.Location = new System.Drawing.Point(6, 19);
      this.cb_char2.Name = "cb_char2";
      this.cb_char2.Size = new System.Drawing.Size(188, 21);
      this.cb_char2.TabIndex = 1;
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(218, 31);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(28, 21);
      this.button1.TabIndex = 2;
      this.button1.Text = "<<";
      this.button1.UseVisualStyleBackColor = true;
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(218, 58);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(28, 21);
      this.button2.TabIndex = 3;
      this.button2.Text = ">>";
      this.button2.UseVisualStyleBackColor = true;
      // 
      // ItemTransferForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(464, 264);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.groupBox1);
      this.Name = "ItemTransferForm";
      this.Text = "ItemTransferForm";
      this.groupBox1.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.ComboBox cb_char1;
    private System.Windows.Forms.ComboBox cb_char2;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.CheckedListBox clb_char1;
    private System.Windows.Forms.CheckedListBox clb_char2;
  }
}