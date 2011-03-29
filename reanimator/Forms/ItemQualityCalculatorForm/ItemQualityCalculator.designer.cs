namespace Reanimator.Forms.ItemQualityCalculatorForm
{
    partial class ItemQualityCalculator
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
            this.clb_qualities = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.l_quality = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // clb_qualities
            // 
            this.clb_qualities.FormattingEnabled = true;
            this.clb_qualities.Location = new System.Drawing.Point(12, 12);
            this.clb_qualities.Name = "clb_qualities";
            this.clb_qualities.Size = new System.Drawing.Size(200, 244);
            this.clb_qualities.TabIndex = 0;
            this.clb_qualities.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clb_qualities_ItemCheck);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 262);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Resulting quality value:";
            // 
            // l_quality
            // 
            this.l_quality.AutoSize = true;
            this.l_quality.Location = new System.Drawing.Point(122, 262);
            this.l_quality.MaximumSize = new System.Drawing.Size(90, 13);
            this.l_quality.MinimumSize = new System.Drawing.Size(90, 13);
            this.l_quality.Name = "l_quality";
            this.l_quality.Size = new System.Drawing.Size(90, 13);
            this.l_quality.TabIndex = 2;
            this.l_quality.Text = "0";
            this.l_quality.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ItemQualityCalculator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(224, 284);
            this.Controls.Add(this.l_quality);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.clb_qualities);
            this.Name = "ItemQualityCalculator";
            this.ShowIcon = false;
            this.Text = "ItemQualityCalculator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox clb_qualities;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label l_quality;
    }
}