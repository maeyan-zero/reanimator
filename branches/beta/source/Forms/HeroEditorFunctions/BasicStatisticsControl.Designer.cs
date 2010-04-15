namespace Reanimator.Forms.HeroEditorFunctions
{
    partial class BasicStatisticsControl
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
            this.nud_powerRecharge = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.l_power = new System.Windows.Forms.Label();
            this.l_health = new System.Windows.Forms.Label();
            this.nud_healthRegen = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nud_criticalDamageRight = new System.Windows.Forms.NumericUpDown();
            this.nud_criticalDamageLeft = new System.Windows.Forms.NumericUpDown();
            this.nud_criticalChanceRight = new System.Windows.Forms.NumericUpDown();
            this.nud_criticalChanceLeft = new System.Windows.Forms.NumericUpDown();
            this.nud_movementSpeed = new System.Windows.Forms.NumericUpDown();
            this.nud_power = new System.Windows.Forms.NumericUpDown();
            this.nud_health = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nud_powerRecharge)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_healthRegen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_criticalDamageRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_criticalDamageLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_criticalChanceRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_criticalChanceLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_movementSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_power)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_health)).BeginInit();
            this.SuspendLayout();
            // 
            // nud_powerRecharge
            // 
            this.nud_powerRecharge.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.nud_powerRecharge.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nud_powerRecharge.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nud_powerRecharge.ForeColor = System.Drawing.Color.White;
            this.nud_powerRecharge.Location = new System.Drawing.Point(193, 113);
            this.nud_powerRecharge.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nud_powerRecharge.Name = "nud_powerRecharge";
            this.nud_powerRecharge.Size = new System.Drawing.Size(42, 16);
            this.nud_powerRecharge.TabIndex = 68;
            this.nud_powerRecharge.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_powerRecharge.Value = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nud_powerRecharge.ValueChanged += new System.EventHandler(this.nud_powerRecharge_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.SteelBlue;
            this.label9.Location = new System.Drawing.Point(57, 113);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(130, 13);
            this.label9.TabIndex = 67;
            this.label9.Text = "TOTAL RECHARGE RATE:";
            // 
            // l_power
            // 
            this.l_power.AutoSize = true;
            this.l_power.BackColor = System.Drawing.Color.Transparent;
            this.l_power.ForeColor = System.Drawing.Color.SteelBlue;
            this.l_power.Location = new System.Drawing.Point(9, 112);
            this.l_power.MaximumSize = new System.Drawing.Size(42, 13);
            this.l_power.MinimumSize = new System.Drawing.Size(42, 13);
            this.l_power.Name = "l_power";
            this.l_power.Size = new System.Drawing.Size(42, 13);
            this.l_power.TabIndex = 66;
            this.l_power.Text = "999";
            this.l_power.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // l_health
            // 
            this.l_health.AutoSize = true;
            this.l_health.BackColor = System.Drawing.Color.Transparent;
            this.l_health.ForeColor = System.Drawing.Color.SteelBlue;
            this.l_health.Location = new System.Drawing.Point(9, 58);
            this.l_health.MaximumSize = new System.Drawing.Size(42, 13);
            this.l_health.MinimumSize = new System.Drawing.Size(42, 13);
            this.l_health.Name = "l_health";
            this.l_health.Size = new System.Drawing.Size(42, 13);
            this.l_health.TabIndex = 65;
            this.l_health.Text = "999";
            this.l_health.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // nud_healthRegen
            // 
            this.nud_healthRegen.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.nud_healthRegen.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nud_healthRegen.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nud_healthRegen.ForeColor = System.Drawing.Color.White;
            this.nud_healthRegen.Location = new System.Drawing.Point(172, 60);
            this.nud_healthRegen.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nud_healthRegen.Name = "nud_healthRegen";
            this.nud_healthRegen.Size = new System.Drawing.Size(42, 16);
            this.nud_healthRegen.TabIndex = 64;
            this.nud_healthRegen.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_healthRegen.Value = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nud_healthRegen.ValueChanged += new System.EventHandler(this.nud_healthRegen_ValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.SteelBlue;
            this.label10.Location = new System.Drawing.Point(57, 59);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(109, 13);
            this.label10.TabIndex = 63;
            this.label10.Text = "TOTAL REGEN RATE:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.SteelBlue;
            this.label7.Location = new System.Drawing.Point(58, 173);
            this.label7.MaximumSize = new System.Drawing.Size(42, 16);
            this.label7.MinimumSize = new System.Drawing.Size(42, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 16);
            this.label7.TabIndex = 62;
            this.label7.Text = "RIGHT";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.SteelBlue;
            this.label6.Location = new System.Drawing.Point(9, 173);
            this.label6.MaximumSize = new System.Drawing.Size(42, 16);
            this.label6.MinimumSize = new System.Drawing.Size(42, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 16);
            this.label6.TabIndex = 61;
            this.label6.Text = "LEFT";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(57, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 17);
            this.label2.TabIndex = 58;
            this.label2.Text = "MOVEMENT SPEED";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(57, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 17);
            this.label1.TabIndex = 57;
            this.label1.Text = "POWER";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(106, 222);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 13);
            this.label5.TabIndex = 60;
            this.label5.Text = "CRITICAL DAMAGE";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(106, 195);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 13);
            this.label4.TabIndex = 59;
            this.label4.Text = "CRITICAL CHANCE";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(57, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 17);
            this.label3.TabIndex = 56;
            this.label3.Text = "HEALTH";
            // 
            // nud_criticalDamageRight
            // 
            this.nud_criticalDamageRight.BackColor = System.Drawing.Color.Black;
            this.nud_criticalDamageRight.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nud_criticalDamageRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nud_criticalDamageRight.ForeColor = System.Drawing.Color.White;
            this.nud_criticalDamageRight.Location = new System.Drawing.Point(58, 222);
            this.nud_criticalDamageRight.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nud_criticalDamageRight.Name = "nud_criticalDamageRight";
            this.nud_criticalDamageRight.Size = new System.Drawing.Size(42, 16);
            this.nud_criticalDamageRight.TabIndex = 55;
            this.nud_criticalDamageRight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_criticalDamageRight.Value = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nud_criticalDamageRight.ValueChanged += new System.EventHandler(this.nud_criticalDamageRight_ValueChanged);
            // 
            // nud_criticalDamageLeft
            // 
            this.nud_criticalDamageLeft.BackColor = System.Drawing.Color.Black;
            this.nud_criticalDamageLeft.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nud_criticalDamageLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nud_criticalDamageLeft.ForeColor = System.Drawing.Color.White;
            this.nud_criticalDamageLeft.Location = new System.Drawing.Point(9, 222);
            this.nud_criticalDamageLeft.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nud_criticalDamageLeft.Name = "nud_criticalDamageLeft";
            this.nud_criticalDamageLeft.Size = new System.Drawing.Size(42, 16);
            this.nud_criticalDamageLeft.TabIndex = 54;
            this.nud_criticalDamageLeft.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_criticalDamageLeft.Value = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nud_criticalDamageLeft.ValueChanged += new System.EventHandler(this.nud_criticalDamageLeft_ValueChanged);
            // 
            // nud_criticalChanceRight
            // 
            this.nud_criticalChanceRight.BackColor = System.Drawing.Color.Black;
            this.nud_criticalChanceRight.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nud_criticalChanceRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nud_criticalChanceRight.ForeColor = System.Drawing.Color.White;
            this.nud_criticalChanceRight.Location = new System.Drawing.Point(58, 195);
            this.nud_criticalChanceRight.Maximum = new decimal(new int[] {
            95,
            0,
            0,
            0});
            this.nud_criticalChanceRight.Name = "nud_criticalChanceRight";
            this.nud_criticalChanceRight.Size = new System.Drawing.Size(42, 16);
            this.nud_criticalChanceRight.TabIndex = 53;
            this.nud_criticalChanceRight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_criticalChanceRight.Value = new decimal(new int[] {
            95,
            0,
            0,
            0});
            this.nud_criticalChanceRight.ValueChanged += new System.EventHandler(this.nud_criticalChanceRight_ValueChanged);
            // 
            // nud_criticalChanceLeft
            // 
            this.nud_criticalChanceLeft.BackColor = System.Drawing.Color.Black;
            this.nud_criticalChanceLeft.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nud_criticalChanceLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nud_criticalChanceLeft.ForeColor = System.Drawing.Color.White;
            this.nud_criticalChanceLeft.Location = new System.Drawing.Point(9, 195);
            this.nud_criticalChanceLeft.Maximum = new decimal(new int[] {
            95,
            0,
            0,
            0});
            this.nud_criticalChanceLeft.Name = "nud_criticalChanceLeft";
            this.nud_criticalChanceLeft.Size = new System.Drawing.Size(42, 16);
            this.nud_criticalChanceLeft.TabIndex = 52;
            this.nud_criticalChanceLeft.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_criticalChanceLeft.Value = new decimal(new int[] {
            95,
            0,
            0,
            0});
            this.nud_criticalChanceLeft.ValueChanged += new System.EventHandler(this.nud_criticalChanceLeft_ValueChanged);
            // 
            // nud_movementSpeed
            // 
            this.nud_movementSpeed.BackColor = System.Drawing.Color.Black;
            this.nud_movementSpeed.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nud_movementSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nud_movementSpeed.ForeColor = System.Drawing.Color.White;
            this.nud_movementSpeed.Location = new System.Drawing.Point(9, 151);
            this.nud_movementSpeed.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nud_movementSpeed.Name = "nud_movementSpeed";
            this.nud_movementSpeed.Size = new System.Drawing.Size(42, 16);
            this.nud_movementSpeed.TabIndex = 51;
            this.nud_movementSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_movementSpeed.Value = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nud_movementSpeed.ValueChanged += new System.EventHandler(this.nud_movementSpeed_ValueChanged);
            // 
            // nud_power
            // 
            this.nud_power.BackColor = System.Drawing.Color.Black;
            this.nud_power.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nud_power.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nud_power.ForeColor = System.Drawing.Color.White;
            this.nud_power.Location = new System.Drawing.Point(9, 95);
            this.nud_power.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nud_power.Name = "nud_power";
            this.nud_power.Size = new System.Drawing.Size(42, 16);
            this.nud_power.TabIndex = 50;
            this.nud_power.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_power.Value = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nud_power.ValueChanged += new System.EventHandler(this.nud_power_ValueChanged);
            // 
            // nud_health
            // 
            this.nud_health.BackColor = System.Drawing.Color.Black;
            this.nud_health.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nud_health.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nud_health.ForeColor = System.Drawing.Color.White;
            this.nud_health.Location = new System.Drawing.Point(9, 42);
            this.nud_health.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nud_health.Name = "nud_health";
            this.nud_health.Size = new System.Drawing.Size(42, 16);
            this.nud_health.TabIndex = 49;
            this.nud_health.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_health.Value = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nud_health.ValueChanged += new System.EventHandler(this.nud_health_ValueChanged);
            // 
            // BasicStatisticsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Reanimator.Properties.Resources.statistics_small;
            this.Controls.Add(this.nud_powerRecharge);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.l_power);
            this.Controls.Add(this.l_health);
            this.Controls.Add(this.nud_healthRegen);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nud_criticalDamageRight);
            this.Controls.Add(this.nud_criticalDamageLeft);
            this.Controls.Add(this.nud_criticalChanceRight);
            this.Controls.Add(this.nud_criticalChanceLeft);
            this.Controls.Add(this.nud_movementSpeed);
            this.Controls.Add(this.nud_power);
            this.Controls.Add(this.nud_health);
            this.Name = "BasicStatisticsControl";
            this.Size = new System.Drawing.Size(256, 250);
            ((System.ComponentModel.ISupportInitialize)(this.nud_powerRecharge)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_healthRegen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_criticalDamageRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_criticalDamageLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_criticalChanceRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_criticalChanceLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_movementSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_power)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_health)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label l_power;
        private System.Windows.Forms.Label l_health;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.NumericUpDown nud_powerRecharge;
        public System.Windows.Forms.NumericUpDown nud_healthRegen;
        public System.Windows.Forms.NumericUpDown nud_criticalDamageRight;
        public System.Windows.Forms.NumericUpDown nud_criticalDamageLeft;
        public System.Windows.Forms.NumericUpDown nud_criticalChanceRight;
        public System.Windows.Forms.NumericUpDown nud_criticalChanceLeft;
        public System.Windows.Forms.NumericUpDown nud_movementSpeed;
        public System.Windows.Forms.NumericUpDown nud_power;
        public System.Windows.Forms.NumericUpDown nud_health;
    }
}
