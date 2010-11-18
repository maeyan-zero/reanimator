namespace Reanimator.Forms.HeroEditorFunctions
{
    partial class SingleSkillControl
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
            this.components = new System.ComponentModel.Container();
            this.p_icon = new System.Windows.Forms.Panel();
            this.b_up = new System.Windows.Forms.Button();
            this.b_down = new System.Windows.Forms.Button();
            this.l_values = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // p_icon
            // 
            this.p_icon.BackColor = System.Drawing.Color.Black;
            this.p_icon.Location = new System.Drawing.Point(3, 3);
            this.p_icon.Name = "p_icon";
            this.p_icon.Size = new System.Drawing.Size(64, 64);
            this.p_icon.TabIndex = 0;
            this.toolTip1.SetToolTip(this.p_icon, "tttt");
            // 
            // b_up
            // 
            this.b_up.BackColor = System.Drawing.Color.Gold;
            this.b_up.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.b_up.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.b_up.Location = new System.Drawing.Point(70, 8);
            this.b_up.Name = "b_up";
            this.b_up.Size = new System.Drawing.Size(26, 24);
            this.b_up.TabIndex = 1;
            this.b_up.Text = "+";
            this.toolTip1.SetToolTip(this.b_up, "Add skill point");
            this.b_up.UseVisualStyleBackColor = false;
            this.b_up.Click += new System.EventHandler(this.b_up_Click);
            // 
            // b_down
            // 
            this.b_down.BackColor = System.Drawing.Color.Gold;
            this.b_down.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.b_down.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.b_down.Location = new System.Drawing.Point(70, 41);
            this.b_down.Name = "b_down";
            this.b_down.Size = new System.Drawing.Size(26, 24);
            this.b_down.TabIndex = 2;
            this.b_down.Text = "-";
            this.toolTip1.SetToolTip(this.b_down, "Remove skill point");
            this.b_down.UseVisualStyleBackColor = false;
            this.b_down.Click += new System.EventHandler(this.b_down_Click);
            // 
            // l_values
            // 
            this.l_values.AutoSize = true;
            this.l_values.BackColor = System.Drawing.Color.Black;
            this.l_values.ForeColor = System.Drawing.Color.White;
            this.l_values.Location = new System.Drawing.Point(8, 70);
            this.l_values.MaximumSize = new System.Drawing.Size(54, 20);
            this.l_values.MinimumSize = new System.Drawing.Size(54, 20);
            this.l_values.Name = "l_values";
            this.l_values.Size = new System.Drawing.Size(54, 20);
            this.l_values.TabIndex = 3;
            this.l_values.Text = "10/10";
            this.l_values.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SingleSkillControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.l_values);
            this.Controls.Add(this.b_down);
            this.Controls.Add(this.b_up);
            this.Controls.Add(this.p_icon);
            this.Name = "SingleSkillControl";
            this.Size = new System.Drawing.Size(102, 96);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel p_icon;
        private System.Windows.Forms.Button b_up;
        private System.Windows.Forms.Button b_down;
        private System.Windows.Forms.Label l_values;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
