namespace Reanimator.Forms
{
    partial class ScriptEditor
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
            this._save_button = new System.Windows.Forms.Button();
            this._testCompile_button = new System.Windows.Forms.Button();
            this._scriptEditor_richTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // _save_button
            // 
            this._save_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._save_button.Location = new System.Drawing.Point(267, 200);
            this._save_button.Name = "_save_button";
            this._save_button.Size = new System.Drawing.Size(75, 23);
            this._save_button.TabIndex = 0;
            this._save_button.Text = "Save";
            this._save_button.UseVisualStyleBackColor = true;
            // 
            // _testCompile_button
            // 
            this._testCompile_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._testCompile_button.Location = new System.Drawing.Point(174, 200);
            this._testCompile_button.Name = "_testCompile_button";
            this._testCompile_button.Size = new System.Drawing.Size(87, 23);
            this._testCompile_button.TabIndex = 2;
            this._testCompile_button.Text = "Test Compile";
            this._testCompile_button.UseVisualStyleBackColor = true;
            // 
            // _scriptEditor_richTextBox
            // 
            this._scriptEditor_richTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._scriptEditor_richTextBox.Location = new System.Drawing.Point(13, 13);
            this._scriptEditor_richTextBox.Name = "_scriptEditor_richTextBox";
            this._scriptEditor_richTextBox.Size = new System.Drawing.Size(329, 181);
            this._scriptEditor_richTextBox.TabIndex = 3;
            this._scriptEditor_richTextBox.Text = "";
            // 
            // ScriptEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 231);
            this.Controls.Add(this._scriptEditor_richTextBox);
            this.Controls.Add(this._testCompile_button);
            this.Controls.Add(this._save_button);
            this.Name = "ScriptEditor";
            this.Text = "ScriptEditor";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _save_button;
        private System.Windows.Forms.Button _testCompile_button;
        private System.Windows.Forms.RichTextBox _scriptEditor_richTextBox;
    }
}