namespace Reanimator.Forms
{
    sealed partial class LevelRulesEditor
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
            this.flipWidthHeight_checkBox = new System.Windows.Forms.CheckBox();
            this.reverseRotation_checkBox = new System.Windows.Forms.CheckBox();
            this.flipXYOffsets_checkBox = new System.Windows.Forms.CheckBox();
            this.disableWidthOffset_checkBox = new System.Windows.Forms.CheckBox();
            this.flipWidthHeightOffset_checkBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // flipWidthHeight_checkBox
            // 
            this.flipWidthHeight_checkBox.AutoSize = true;
            this.flipWidthHeight_checkBox.Location = new System.Drawing.Point(13, 13);
            this.flipWidthHeight_checkBox.Name = "flipWidthHeight_checkBox";
            this.flipWidthHeight_checkBox.Size = new System.Drawing.Size(109, 17);
            this.flipWidthHeight_checkBox.TabIndex = 0;
            this.flipWidthHeight_checkBox.Text = "Flip Width/Height";
            this.flipWidthHeight_checkBox.UseVisualStyleBackColor = true;
            this.flipWidthHeight_checkBox.CheckedChanged += new System.EventHandler(this.flipWidthHeight_checkBox_CheckedChanged);
            // 
            // reverseRotation_checkBox
            // 
            this.reverseRotation_checkBox.AutoSize = true;
            this.reverseRotation_checkBox.Location = new System.Drawing.Point(13, 37);
            this.reverseRotation_checkBox.Name = "reverseRotation_checkBox";
            this.reverseRotation_checkBox.Size = new System.Drawing.Size(109, 17);
            this.reverseRotation_checkBox.TabIndex = 1;
            this.reverseRotation_checkBox.Text = "Reverse Rotation";
            this.reverseRotation_checkBox.UseVisualStyleBackColor = true;
            this.reverseRotation_checkBox.CheckedChanged += new System.EventHandler(this.reverseRotation_checkBox_CheckedChanged);
            // 
            // flipXYOffsets_checkBox
            // 
            this.flipXYOffsets_checkBox.AutoSize = true;
            this.flipXYOffsets_checkBox.Location = new System.Drawing.Point(13, 61);
            this.flipXYOffsets_checkBox.Name = "flipXYOffsets_checkBox";
            this.flipXYOffsets_checkBox.Size = new System.Drawing.Size(100, 17);
            this.flipXYOffsets_checkBox.TabIndex = 2;
            this.flipXYOffsets_checkBox.Text = "Flip X/Y Offsets";
            this.flipXYOffsets_checkBox.UseVisualStyleBackColor = true;
            this.flipXYOffsets_checkBox.CheckedChanged += new System.EventHandler(this.flipXYOffsets_checkBox_CheckedChanged);
            // 
            // disableWidthOffset_checkBox
            // 
            this.disableWidthOffset_checkBox.AutoSize = true;
            this.disableWidthOffset_checkBox.Location = new System.Drawing.Point(13, 85);
            this.disableWidthOffset_checkBox.Name = "disableWidthOffset_checkBox";
            this.disableWidthOffset_checkBox.Size = new System.Drawing.Size(123, 17);
            this.disableWidthOffset_checkBox.TabIndex = 3;
            this.disableWidthOffset_checkBox.Text = "Disable Width Offset";
            this.disableWidthOffset_checkBox.UseVisualStyleBackColor = true;
            this.disableWidthOffset_checkBox.CheckedChanged += new System.EventHandler(this.disableWidthOffset_checkBox_CheckedChanged);
            // 
            // flipWidthHeightOffset_checkBox
            // 
            this.flipWidthHeightOffset_checkBox.AutoSize = true;
            this.flipWidthHeightOffset_checkBox.Location = new System.Drawing.Point(13, 109);
            this.flipWidthHeightOffset_checkBox.Name = "flipWidthHeightOffset_checkBox";
            this.flipWidthHeightOffset_checkBox.Size = new System.Drawing.Size(140, 17);
            this.flipWidthHeightOffset_checkBox.TabIndex = 4;
            this.flipWidthHeightOffset_checkBox.Text = "Flip Width/Height Offset";
            this.flipWidthHeightOffset_checkBox.UseVisualStyleBackColor = true;
            this.flipWidthHeightOffset_checkBox.CheckedChanged += new System.EventHandler(this.flipWidthHeightOffset_checkBox_CheckedChanged);
            // 
            // LevelRulesEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(556, 457);
            this.Controls.Add(this.flipWidthHeightOffset_checkBox);
            this.Controls.Add(this.disableWidthOffset_checkBox);
            this.Controls.Add(this.flipXYOffsets_checkBox);
            this.Controls.Add(this.reverseRotation_checkBox);
            this.Controls.Add(this.flipWidthHeight_checkBox);
            this.Name = "LevelRulesEditor";
            this.Text = "LevelRulesEditor";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this._LevelRulesEditor_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this._LevelRulesEditor_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this._LevelRulesEditor_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this._LevelRulesEditor_MouseUp);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this._LevelRulesEditor_MouseWheel);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox flipWidthHeight_checkBox;
        private System.Windows.Forms.CheckBox reverseRotation_checkBox;
        private System.Windows.Forms.CheckBox flipXYOffsets_checkBox;
        private System.Windows.Forms.CheckBox disableWidthOffset_checkBox;
        private System.Windows.Forms.CheckBox flipWidthHeightOffset_checkBox;
    }
}