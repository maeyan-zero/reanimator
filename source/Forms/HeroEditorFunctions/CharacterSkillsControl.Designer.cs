using System.Drawing;
namespace Reanimator.Forms.HeroEditorFunctions
{
    partial class CharacterSkillsControl
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
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nud_availableSkillPoints = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nud_availableSkillPoints)).BeginInit();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Black;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(36, 12);
            this.label5.MaximumSize = new System.Drawing.Size(686, 22);
            this.label5.MinimumSize = new System.Drawing.Size(686, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(686, 22);
            this.label5.TabIndex = 58;
            this.label5.Text = "SKILLS";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Black;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Yellow;
            this.label4.Location = new System.Drawing.Point(36, 46);
            this.label4.MaximumSize = new System.Drawing.Size(686, 22);
            this.label4.MinimumSize = new System.Drawing.Size(686, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(686, 22);
            this.label4.TabIndex = 57;
            this.label4.Text = "ACQUIRE A NEW SKILL!";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Black;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Yellow;
            this.label2.Location = new System.Drawing.Point(318, 658);
            this.label2.MaximumSize = new System.Drawing.Size(390, 26);
            this.label2.MinimumSize = new System.Drawing.Size(390, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(390, 26);
            this.label2.TabIndex = 56;
            this.label2.Text = "SKILL SELECTIONS REMAINING:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.label1.Location = new System.Drawing.Point(14, 658);
            this.label1.MaximumSize = new System.Drawing.Size(296, 20);
            this.label1.MinimumSize = new System.Drawing.Size(296, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(296, 20);
            this.label1.TabIndex = 55;
            this.label1.Text = "UNIVERSAL SKILLS";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nud_availableSkillPoints
            // 
            this.nud_availableSkillPoints.AutoSize = true;
            this.nud_availableSkillPoints.BackColor = System.Drawing.Color.Black;
            this.nud_availableSkillPoints.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nud_availableSkillPoints.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nud_availableSkillPoints.ForeColor = System.Drawing.Color.White;
            this.nud_availableSkillPoints.Location = new System.Drawing.Point(714, 660);
            this.nud_availableSkillPoints.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nud_availableSkillPoints.MaximumSize = new System.Drawing.Size(78, 0);
            this.nud_availableSkillPoints.MinimumSize = new System.Drawing.Size(78, 0);
            this.nud_availableSkillPoints.Name = "nud_availableSkillPoints";
            this.nud_availableSkillPoints.Size = new System.Drawing.Size(78, 26);
            this.nud_availableSkillPoints.TabIndex = 54;
            this.nud_availableSkillPoints.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nud_availableSkillPoints.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_availableSkillPoints.ValueChanged += new System.EventHandler(this.numericUpDown4_ValueChanged);
            // 
            // CharacterSkillsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            //this.BackgroundImage = global::Reanimator.Properties.Resources.skillPanelBg;
            this.BackgroundImage = Bitmap.FromFile(@"HeroEditor\skillPanelBg.png");
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nud_availableSkillPoints);
            this.DoubleBuffered = true;
            this.Name = "CharacterSkillsControl";
            this.Size = new System.Drawing.Size(802, 746);
            ((System.ComponentModel.ISupportInitialize)(this.nud_availableSkillPoints)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.NumericUpDown nud_availableSkillPoints;
    }
}
