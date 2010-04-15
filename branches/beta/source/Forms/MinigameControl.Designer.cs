namespace Reanimator.Forms
{
    partial class MinigameControl
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
            this.p_icons = new System.Windows.Forms.Panel();
            this.b_icon3 = new System.Windows.Forms.Button();
            this.b_icon2 = new System.Windows.Forms.Button();
            this.b_icon1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rb_easy = new System.Windows.Forms.RadioButton();
            this.rb_hard = new System.Windows.Forms.RadioButton();
            this.rb_normal = new System.Windows.Forms.RadioButton();
            this.b_reset = new System.Windows.Forms.Button();
            this.p_icons.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // p_icons
            // 
            this.p_icons.Controls.Add(this.b_icon1);
            this.p_icons.Controls.Add(this.b_icon2);
            this.p_icons.Controls.Add(this.b_icon3);
            this.p_icons.Location = new System.Drawing.Point(3, 51);
            this.p_icons.Name = "p_icons";
            this.p_icons.Size = new System.Drawing.Size(280, 70);
            this.p_icons.TabIndex = 0;
            // 
            // b_icon1
            // 
            this.b_icon1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.b_icon1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.b_icon1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_icon1.ForeColor = System.Drawing.Color.Red;
            this.b_icon1.Location = new System.Drawing.Point(3, 3);
            this.b_icon1.Name = "b_icon1";
            this.b_icon1.Size = new System.Drawing.Size(64, 64);
            this.b_icon1.TabIndex = 3;
            this.b_icon1.Text = "-1";
            this.b_icon1.UseVisualStyleBackColor = true;
            this.b_icon1.Click += new System.EventHandler(this.b_icon_Click);
            // 
            // b_icon2
            // 
            this.b_icon2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.b_icon2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.b_icon2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_icon2.ForeColor = System.Drawing.Color.Red;
            this.b_icon2.Location = new System.Drawing.Point(108, 3);
            this.b_icon2.Name = "b_icon2";
            this.b_icon2.Size = new System.Drawing.Size(64, 64);
            this.b_icon2.TabIndex = 4;
            this.b_icon2.Text = "-1";
            this.b_icon2.UseVisualStyleBackColor = true;
            this.b_icon2.Click += new System.EventHandler(this.b_icon_Click);
            // 
            // b_icon3
            // 
            this.b_icon3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.b_icon3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.b_icon3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_icon3.ForeColor = System.Drawing.Color.Red;
            this.b_icon3.Location = new System.Drawing.Point(213, 3);
            this.b_icon3.Name = "b_icon3";
            this.b_icon3.Size = new System.Drawing.Size(64, 64);
            this.b_icon3.TabIndex = 5;
            this.b_icon3.Text = "-1";
            this.b_icon3.UseVisualStyleBackColor = true;
            this.b_icon3.Click += new System.EventHandler(this.b_icon_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rb_easy);
            this.groupBox1.Controls.Add(this.rb_hard);
            this.groupBox1.Controls.Add(this.rb_normal);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 42);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Difficulty";
            // 
            // rb_easy
            // 
            this.rb_easy.AutoSize = true;
            this.rb_easy.Location = new System.Drawing.Point(28, 19);
            this.rb_easy.Name = "rb_easy";
            this.rb_easy.Size = new System.Drawing.Size(47, 17);
            this.rb_easy.TabIndex = 2;
            this.rb_easy.Text = "easy";
            this.rb_easy.UseVisualStyleBackColor = true;
            this.rb_easy.CheckedChanged += new System.EventHandler(this.rb_difficulty_CheckedChanged);
            // 
            // rb_hard
            // 
            this.rb_hard.AutoSize = true;
            this.rb_hard.Location = new System.Drawing.Point(207, 19);
            this.rb_hard.Name = "rb_hard";
            this.rb_hard.Size = new System.Drawing.Size(46, 17);
            this.rb_hard.TabIndex = 1;
            this.rb_hard.Text = "hard";
            this.rb_hard.UseVisualStyleBackColor = true;
            this.rb_hard.CheckedChanged += new System.EventHandler(this.rb_difficulty_CheckedChanged);
            // 
            // rb_normal
            // 
            this.rb_normal.AutoSize = true;
            this.rb_normal.Checked = true;
            this.rb_normal.Location = new System.Drawing.Point(113, 19);
            this.rb_normal.Name = "rb_normal";
            this.rb_normal.Size = new System.Drawing.Size(56, 17);
            this.rb_normal.TabIndex = 0;
            this.rb_normal.TabStop = true;
            this.rb_normal.Text = "normal";
            this.rb_normal.UseVisualStyleBackColor = true;
            this.rb_normal.CheckedChanged += new System.EventHandler(this.rb_difficulty_CheckedChanged);
            // 
            // b_reset
            // 
            this.b_reset.Location = new System.Drawing.Point(3, 127);
            this.b_reset.Name = "b_reset";
            this.b_reset.Size = new System.Drawing.Size(280, 46);
            this.b_reset.TabIndex = 6;
            this.b_reset.Text = "Reset";
            this.b_reset.UseVisualStyleBackColor = true;
            this.b_reset.Click += new System.EventHandler(this.b_reset_Click);
            // 
            // MinigameControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.b_reset);
            this.Controls.Add(this.p_icons);
            this.Name = "MinigameControl";
            this.Size = new System.Drawing.Size(286, 176);
            this.p_icons.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel p_icons;
        private System.Windows.Forms.Button b_icon1;
        private System.Windows.Forms.Button b_icon2;
        private System.Windows.Forms.Button b_icon3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rb_easy;
        private System.Windows.Forms.RadioButton rb_hard;
        private System.Windows.Forms.RadioButton rb_normal;
        private System.Windows.Forms.Button b_reset;
    }
}
