namespace Reanimator.Forms.CustomControls
{
    partial class HGLForm
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
            this.comboBoxWindowStyle = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // comboBoxWindowStyle
            // 
            this.comboBoxWindowStyle.FormattingEnabled = true;
            this.comboBoxWindowStyle.Location = new System.Drawing.Point(167, 12);
            this.comboBoxWindowStyle.Name = "comboBoxWindowStyle";
            this.comboBoxWindowStyle.Size = new System.Drawing.Size(121, 21);
            this.comboBoxWindowStyle.TabIndex = 0;
            this.comboBoxWindowStyle.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // HGLForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(300, 300);
            this.Controls.Add(this.comboBoxWindowStyle);
            this.DoubleBuffered = true;
            this.Name = "HGLForm";
            this.ShowIcon = false;
            this.Text = "HGLForm";
            this.Resize += new System.EventHandler(this.HGLForm_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxWindowStyle;

    }
}