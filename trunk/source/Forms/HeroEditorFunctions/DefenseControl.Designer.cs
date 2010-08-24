namespace Reanimator.Forms.HeroEditorFunctions
{
    partial class DefenseControl
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
            this.l_armor = new System.Windows.Forms.Label();
            this.l_shields = new System.Windows.Forms.Label();
            this.nud_shieldRecharge = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nud_poison = new System.Windows.Forms.NumericUpDown();
            this.nud_phase = new System.Windows.Forms.NumericUpDown();
            this.nud_shock = new System.Windows.Forms.NumericUpDown();
            this.nud_ignite = new System.Windows.Forms.NumericUpDown();
            this.nud_stun = new System.Windows.Forms.NumericUpDown();
            this.nud_armor = new System.Windows.Forms.NumericUpDown();
            this.nud_shields = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nud_shieldRecharge)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_poison)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_phase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_shock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_ignite)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_stun)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_armor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_shields)).BeginInit();
            this.SuspendLayout();
            // 
            // l_armor
            // 
            this.l_armor.AutoSize = true;
            this.l_armor.BackColor = System.Drawing.Color.Black;
            this.l_armor.ForeColor = System.Drawing.Color.SteelBlue;
            this.l_armor.Location = new System.Drawing.Point(9, 112);
            this.l_armor.MaximumSize = new System.Drawing.Size(42, 13);
            this.l_armor.MinimumSize = new System.Drawing.Size(42, 13);
            this.l_armor.Name = "l_armor";
            this.l_armor.Size = new System.Drawing.Size(42, 13);
            this.l_armor.TabIndex = 61;
            this.l_armor.Text = "999";
            this.l_armor.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // l_shields
            // 
            this.l_shields.AutoSize = true;
            this.l_shields.BackColor = System.Drawing.Color.Black;
            this.l_shields.ForeColor = System.Drawing.Color.SteelBlue;
            this.l_shields.Location = new System.Drawing.Point(9, 58);
            this.l_shields.MaximumSize = new System.Drawing.Size(42, 13);
            this.l_shields.MinimumSize = new System.Drawing.Size(42, 13);
            this.l_shields.Name = "l_shields";
            this.l_shields.Size = new System.Drawing.Size(42, 13);
            this.l_shields.TabIndex = 60;
            this.l_shields.Text = "999";
            this.l_shields.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // nud_shieldRecharge
            // 
            this.nud_shieldRecharge.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(60)))), ((int)(((byte)(80)))));
            this.nud_shieldRecharge.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nud_shieldRecharge.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nud_shieldRecharge.ForeColor = System.Drawing.Color.White;
            this.nud_shieldRecharge.Location = new System.Drawing.Point(193, 60);
            this.nud_shieldRecharge.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nud_shieldRecharge.Name = "nud_shieldRecharge";
            this.nud_shieldRecharge.Size = new System.Drawing.Size(42, 16);
            this.nud_shieldRecharge.TabIndex = 59;
            this.nud_shieldRecharge.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_shieldRecharge.Value = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nud_shieldRecharge.ValueChanged += new System.EventHandler(this.nud_shieldRecharge_ValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Black;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.SteelBlue;
            this.label10.Location = new System.Drawing.Point(57, 59);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(130, 13);
            this.label10.TabIndex = 58;
            this.label10.Text = "TOTAL RECHARGE RATE:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Black;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(57, 42);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(66, 17);
            this.label9.TabIndex = 57;
            this.label9.Text = "SHIELDS";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Black;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(57, 95);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 17);
            this.label7.TabIndex = 56;
            this.label7.Text = "ARMOR";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Black;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(80, 128);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 17);
            this.label6.TabIndex = 55;
            this.label6.Text = "DEFENSES";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Black;
            this.label5.ForeColor = System.Drawing.Color.Lime;
            this.label5.Location = new System.Drawing.Point(80, 261);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 54;
            this.label5.Text = "POISON";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Black;
            this.label4.ForeColor = System.Drawing.Color.Violet;
            this.label4.Location = new System.Drawing.Point(80, 235);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 53;
            this.label4.Text = "PHASE";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Black;
            this.label3.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.label3.Location = new System.Drawing.Point(80, 209);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 52;
            this.label3.Text = "SHOCK";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Black;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(80, 183);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 51;
            this.label2.Text = "IGNITE";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.ForeColor = System.Drawing.Color.DarkGray;
            this.label1.Location = new System.Drawing.Point(80, 156);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 50;
            this.label1.Text = "STUN";
            // 
            // nud_poison
            // 
            this.nud_poison.BackColor = System.Drawing.Color.Black;
            this.nud_poison.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nud_poison.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nud_poison.ForeColor = System.Drawing.Color.Lime;
            this.nud_poison.Location = new System.Drawing.Point(30, 259);
            this.nud_poison.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nud_poison.Name = "nud_poison";
            this.nud_poison.Size = new System.Drawing.Size(44, 16);
            this.nud_poison.TabIndex = 49;
            this.nud_poison.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_poison.ThousandsSeparator = true;
            this.nud_poison.Value = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nud_poison.ValueChanged += new System.EventHandler(this.nud_poison_ValueChanged);
            // 
            // nud_phase
            // 
            this.nud_phase.BackColor = System.Drawing.Color.Black;
            this.nud_phase.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nud_phase.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nud_phase.ForeColor = System.Drawing.Color.Violet;
            this.nud_phase.Location = new System.Drawing.Point(30, 233);
            this.nud_phase.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nud_phase.Name = "nud_phase";
            this.nud_phase.Size = new System.Drawing.Size(44, 16);
            this.nud_phase.TabIndex = 48;
            this.nud_phase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_phase.ThousandsSeparator = true;
            this.nud_phase.Value = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nud_phase.ValueChanged += new System.EventHandler(this.nud_phase_ValueChanged);
            // 
            // nud_shock
            // 
            this.nud_shock.BackColor = System.Drawing.Color.Black;
            this.nud_shock.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nud_shock.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nud_shock.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.nud_shock.Location = new System.Drawing.Point(30, 207);
            this.nud_shock.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nud_shock.Name = "nud_shock";
            this.nud_shock.Size = new System.Drawing.Size(44, 16);
            this.nud_shock.TabIndex = 47;
            this.nud_shock.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_shock.ThousandsSeparator = true;
            this.nud_shock.Value = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nud_shock.ValueChanged += new System.EventHandler(this.nud_shock_ValueChanged);
            // 
            // nud_ignite
            // 
            this.nud_ignite.BackColor = System.Drawing.Color.Black;
            this.nud_ignite.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nud_ignite.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nud_ignite.ForeColor = System.Drawing.Color.Red;
            this.nud_ignite.Location = new System.Drawing.Point(30, 181);
            this.nud_ignite.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nud_ignite.Name = "nud_ignite";
            this.nud_ignite.Size = new System.Drawing.Size(44, 16);
            this.nud_ignite.TabIndex = 46;
            this.nud_ignite.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_ignite.ThousandsSeparator = true;
            this.nud_ignite.Value = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nud_ignite.ValueChanged += new System.EventHandler(this.nud_ignite_ValueChanged);
            // 
            // nud_stun
            // 
            this.nud_stun.BackColor = System.Drawing.Color.Black;
            this.nud_stun.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nud_stun.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nud_stun.ForeColor = System.Drawing.Color.DarkGray;
            this.nud_stun.Location = new System.Drawing.Point(30, 154);
            this.nud_stun.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nud_stun.Name = "nud_stun";
            this.nud_stun.Size = new System.Drawing.Size(44, 16);
            this.nud_stun.TabIndex = 45;
            this.nud_stun.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_stun.ThousandsSeparator = true;
            this.nud_stun.Value = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nud_stun.ValueChanged += new System.EventHandler(this.nud_stun_ValueChanged);
            // 
            // nud_armor
            // 
            this.nud_armor.BackColor = System.Drawing.Color.Black;
            this.nud_armor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nud_armor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nud_armor.ForeColor = System.Drawing.Color.White;
            this.nud_armor.Location = new System.Drawing.Point(9, 95);
            this.nud_armor.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nud_armor.Name = "nud_armor";
            this.nud_armor.Size = new System.Drawing.Size(42, 16);
            this.nud_armor.TabIndex = 44;
            this.nud_armor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_armor.Value = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nud_armor.ValueChanged += new System.EventHandler(this.nud_armor_ValueChanged);
            // 
            // nud_shields
            // 
            this.nud_shields.BackColor = System.Drawing.Color.Black;
            this.nud_shields.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nud_shields.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nud_shields.ForeColor = System.Drawing.Color.White;
            this.nud_shields.Location = new System.Drawing.Point(9, 42);
            this.nud_shields.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nud_shields.Name = "nud_shields";
            this.nud_shields.Size = new System.Drawing.Size(42, 16);
            this.nud_shields.TabIndex = 43;
            this.nud_shields.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_shields.Value = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nud_shields.ValueChanged += new System.EventHandler(this.nud_shields_ValueChanged);
            // 
            // DefenseControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            //this.BackgroundImage = global::Reanimator.Properties.Resources.defense_small;
            this.Controls.Add(this.l_armor);
            this.Controls.Add(this.l_shields);
            this.Controls.Add(this.nud_shieldRecharge);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nud_poison);
            this.Controls.Add(this.nud_phase);
            this.Controls.Add(this.nud_shock);
            this.Controls.Add(this.nud_ignite);
            this.Controls.Add(this.nud_stun);
            this.Controls.Add(this.nud_armor);
            this.Controls.Add(this.nud_shields);
            this.Name = "DefenseControl";
            this.Size = new System.Drawing.Size(256, 284);
            ((System.ComponentModel.ISupportInitialize)(this.nud_shieldRecharge)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_poison)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_phase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_shock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_ignite)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_stun)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_armor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_shields)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label l_armor;
        private System.Windows.Forms.Label l_shields;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.NumericUpDown nud_shieldRecharge;
        public System.Windows.Forms.NumericUpDown nud_poison;
        public System.Windows.Forms.NumericUpDown nud_phase;
        public System.Windows.Forms.NumericUpDown nud_shock;
        public System.Windows.Forms.NumericUpDown nud_ignite;
        public System.Windows.Forms.NumericUpDown nud_stun;
        public System.Windows.Forms.NumericUpDown nud_armor;
        public System.Windows.Forms.NumericUpDown nud_shields;
    }
}
