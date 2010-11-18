namespace Reanimator.Forms
{
    partial class WayPointControl
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
            this.clb_wayPoints = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // clb_wayPoints
            // 
            this.clb_wayPoints.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clb_wayPoints.FormattingEnabled = true;
            this.clb_wayPoints.Location = new System.Drawing.Point(0, 0);
            this.clb_wayPoints.Name = "clb_wayPoints";
            this.clb_wayPoints.Size = new System.Drawing.Size(170, 169);
            this.clb_wayPoints.TabIndex = 28;
            this.clb_wayPoints.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clb_wayPoints_ItemCheck);
            // 
            // WayPointControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.clb_wayPoints);
            this.Name = "WayPointControl";
            this.Size = new System.Drawing.Size(170, 170);
            this.Load += new System.EventHandler(this.WayPointControl_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox clb_wayPoints;
    }
}
