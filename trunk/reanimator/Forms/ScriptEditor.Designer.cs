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
            this._save_Button = new System.Windows.Forms.Button();
            this._testCompile_Button = new System.Windows.Forms.Button();
            this._scriptEditor_RichTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // _save_Button
            // 
            this._save_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._save_Button.Location = new System.Drawing.Point(372, 200);
            this._save_Button.Name = "_save_Button";
            this._save_Button.Size = new System.Drawing.Size(75, 23);
            this._save_Button.TabIndex = 0;
            this._save_Button.Text = "Save";
            this._save_Button.UseVisualStyleBackColor = true;
            this._save_Button.Click += new System.EventHandler(this._Save_Button_Click);
            // 
            // _testCompile_Button
            // 
            this._testCompile_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._testCompile_Button.Location = new System.Drawing.Point(279, 200);
            this._testCompile_Button.Name = "_testCompile_Button";
            this._testCompile_Button.Size = new System.Drawing.Size(87, 23);
            this._testCompile_Button.TabIndex = 2;
            this._testCompile_Button.Text = "Test Compile";
            this._testCompile_Button.UseVisualStyleBackColor = true;
            this._testCompile_Button.Click += new System.EventHandler(this._TestCompile_Button_Click);
            // 
            // _scriptEditor_RichTextBox
            // 
            this._scriptEditor_RichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._scriptEditor_RichTextBox.Location = new System.Drawing.Point(13, 13);
            this._scriptEditor_RichTextBox.Name = "_scriptEditor_RichTextBox";
            this._scriptEditor_RichTextBox.Size = new System.Drawing.Size(434, 181);
            this._scriptEditor_RichTextBox.TabIndex = 3;
            this._scriptEditor_RichTextBox.Text = "";
            this._scriptEditor_RichTextBox.TextChanged += new System.EventHandler(this._ScriptEditor_RichTextBox_TextChanged);
            // 
            // ScriptEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 231);
            this.Controls.Add(this._scriptEditor_RichTextBox);
            this.Controls.Add(this._testCompile_Button);
            this.Controls.Add(this._save_Button);
            this.Name = "ScriptEditor";
            this.Text = "ScriptEditor";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _save_Button;
        private System.Windows.Forms.Button _testCompile_Button;
        private System.Windows.Forms.RichTextBox _scriptEditor_RichTextBox;
    }
}