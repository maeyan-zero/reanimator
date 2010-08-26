using System.Drawing;
namespace Reanimator.Forms.HeroEditorFunctions
{
    partial class CompletePanelControl
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
            this.nud_nextExp = new System.Windows.Forms.NumericUpDown();
            this.nud_currentExp = new System.Windows.Forms.NumericUpDown();
            this.l_characterClass = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.nud_level = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_charakterName = new System.Windows.Forms.TextBox();
            this.p_attributePanel = new System.Windows.Forms.Panel();
            this.p_skillPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.nud_nextExp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_currentExp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_level)).BeginInit();
            this.SuspendLayout();
            // 
            // nud_nextExp
            // 
            this.nud_nextExp.BackColor = System.Drawing.Color.Black;
            this.nud_nextExp.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nud_nextExp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nud_nextExp.ForeColor = System.Drawing.Color.White;
            this.nud_nextExp.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nud_nextExp.Location = new System.Drawing.Point(318, 74);
            this.nud_nextExp.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nud_nextExp.Name = "nud_nextExp";
            this.nud_nextExp.Size = new System.Drawing.Size(80, 17);
            this.nud_nextExp.TabIndex = 46;
            this.nud_nextExp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_nextExp.ThousandsSeparator = true;
            this.nud_nextExp.Value = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            // 
            // nud_currentExp
            // 
            this.nud_currentExp.BackColor = System.Drawing.Color.Black;
            this.nud_currentExp.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nud_currentExp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nud_currentExp.ForeColor = System.Drawing.Color.White;
            this.nud_currentExp.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nud_currentExp.Location = new System.Drawing.Point(224, 74);
            this.nud_currentExp.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nud_currentExp.Name = "nud_currentExp";
            this.nud_currentExp.Size = new System.Drawing.Size(80, 17);
            this.nud_currentExp.TabIndex = 38;
            this.nud_currentExp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_currentExp.ThousandsSeparator = true;
            this.nud_currentExp.Value = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            // 
            // l_characterClass
            // 
            this.l_characterClass.AutoSize = true;
            this.l_characterClass.BackColor = System.Drawing.Color.Black;
            this.l_characterClass.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_characterClass.ForeColor = System.Drawing.Color.White;
            this.l_characterClass.Location = new System.Drawing.Point(186, 38);
            this.l_characterClass.Name = "l_characterClass";
            this.l_characterClass.Size = new System.Drawing.Size(108, 17);
            this.l_characterClass.TabIndex = 45;
            this.l_characterClass.Text = "Character Class";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Black;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(306, 74);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(12, 17);
            this.label9.TabIndex = 44;
            this.label9.Text = "/";
            // 
            // nud_level
            // 
            this.nud_level.AutoSize = true;
            this.nud_level.BackColor = System.Drawing.Color.Black;
            this.nud_level.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nud_level.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nud_level.ForeColor = System.Drawing.Color.White;
            this.nud_level.Location = new System.Drawing.Point(189, 56);
            this.nud_level.Maximum = new decimal(new int[] {
            247,
            0,
            0,
            0});
            this.nud_level.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_level.Name = "nud_level";
            this.nud_level.Size = new System.Drawing.Size(52, 17);
            this.nud_level.TabIndex = 43;
            this.nud_level.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Black;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.label8.Location = new System.Drawing.Point(128, 74);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 17);
            this.label8.TabIndex = 42;
            this.label8.Text = "EXPERIENCE";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Black;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.label7.Location = new System.Drawing.Point(128, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 17);
            this.label7.TabIndex = 41;
            this.label7.Text = "LEVEL";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Black;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.label6.Location = new System.Drawing.Point(128, 38);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 17);
            this.label6.TabIndex = 40;
            this.label6.Text = "CLASS";
            // 
            // tb_charakterName
            // 
            this.tb_charakterName.BackColor = System.Drawing.Color.Black;
            this.tb_charakterName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tb_charakterName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_charakterName.ForeColor = System.Drawing.Color.White;
            this.tb_charakterName.Location = new System.Drawing.Point(128, 8);
            this.tb_charakterName.Name = "tb_charakterName";
            this.tb_charakterName.Size = new System.Drawing.Size(251, 22);
            this.tb_charakterName.TabIndex = 39;
            this.tb_charakterName.Text = "CHARACTER NAME";
            // 
            // p_attributePanel
            // 
            this.p_attributePanel.AutoScroll = true;
            this.p_attributePanel.BackColor = System.Drawing.SystemColors.Control;
            this.p_attributePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.p_attributePanel.Location = new System.Drawing.Point(123, 115);
            this.p_attributePanel.Name = "p_attributePanel";
            this.p_attributePanel.Size = new System.Drawing.Size(276, 560);
            this.p_attributePanel.TabIndex = 47;
            // 
            // p_skillPanel
            // 
            this.p_skillPanel.BackColor = System.Drawing.SystemColors.Control;
            this.p_skillPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.p_skillPanel.Location = new System.Drawing.Point(418, 0);
            this.p_skillPanel.Name = "p_skillPanel";
            this.p_skillPanel.Size = new System.Drawing.Size(802, 746);
            this.p_skillPanel.TabIndex = 48;
            // 
            // CompletePanelControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            //this.BackgroundImage = global::Reanimator.Properties.Resources.attributePanel;
            this.BackgroundImage = Bitmap.FromFile(@"HeroEditor\attributePanel.png");
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Controls.Add(this.p_skillPanel);
            this.Controls.Add(this.p_attributePanel);
            this.Controls.Add(this.nud_nextExp);
            this.Controls.Add(this.nud_currentExp);
            this.Controls.Add(this.l_characterClass);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.nud_level);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tb_charakterName);
            this.MaximumSize = new System.Drawing.Size(1220, 746);
            this.MinimumSize = new System.Drawing.Size(1220, 746);
            this.Name = "CompletePanelControl";
            this.Size = new System.Drawing.Size(1220, 746);
            ((System.ComponentModel.ISupportInitialize)(this.nud_nextExp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_currentExp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_level)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel p_attributePanel;
        public System.Windows.Forms.NumericUpDown nud_nextExp;
        public System.Windows.Forms.NumericUpDown nud_currentExp;
        public System.Windows.Forms.Label l_characterClass;
        public System.Windows.Forms.NumericUpDown nud_level;
        public System.Windows.Forms.TextBox tb_charakterName;
        private System.Windows.Forms.Panel p_skillPanel;
    }
}
