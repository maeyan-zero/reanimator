namespace Reanimator.Forms.HeroEditorFunctions
{
    partial class ParentStatusControl
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
            this.l_controlMenuName = new System.Windows.Forms.Label();
            this.b_minimizeMaximize = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // l_controlMenuName
            // 
            this.l_controlMenuName.AutoSize = true;
            this.l_controlMenuName.BackColor = System.Drawing.Color.Transparent;
            this.l_controlMenuName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_controlMenuName.ForeColor = System.Drawing.Color.White;
            this.l_controlMenuName.Location = new System.Drawing.Point(5, 4);
            this.l_controlMenuName.Name = "l_controlMenuName";
            this.l_controlMenuName.Size = new System.Drawing.Size(94, 17);
            this.l_controlMenuName.TabIndex = 24;
            this.l_controlMenuName.Tag = "header";
            this.l_controlMenuName.Text = "LABEL NAME";
            // 
            // b_minimizeMaximize
            // 
            this.b_minimizeMaximize.BackgroundImage = global::Reanimator.Properties.Resources.panelButton_maximize_small;
            this.b_minimizeMaximize.FlatAppearance.BorderSize = 0;
            this.b_minimizeMaximize.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.b_minimizeMaximize.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.b_minimizeMaximize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.b_minimizeMaximize.Location = new System.Drawing.Point(232, 3);
            this.b_minimizeMaximize.Name = "b_minimizeMaximize";
            this.b_minimizeMaximize.Size = new System.Drawing.Size(19, 18);
            this.b_minimizeMaximize.TabIndex = 25;
            this.b_minimizeMaximize.Tag = "header";
            this.b_minimizeMaximize.UseVisualStyleBackColor = true;
            this.b_minimizeMaximize.Click += new System.EventHandler(this.b_minimizeMaximize_Click);
            // 
            // ParentStatusControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.b_minimizeMaximize);
            this.Controls.Add(this.l_controlMenuName);
            this.Name = "ParentStatusControl";
            this.Size = new System.Drawing.Size(256, 27);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Label l_controlMenuName;
        protected System.Windows.Forms.Button b_minimizeMaximize;
    }
}
