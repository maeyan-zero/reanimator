using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reanimator.Forms.HeroEditorFunctions
{
    public partial class BasicStatisticsControl : ParentStatusControl
    {
        public BasicStatisticsControl()
        {
            InitializeComponent();

            this.SetLabelText("BASE STATISTICS");
            this.MaximizedSize = this.BackgroundImage.Size;
        }

        private void nud_health_ValueChanged(object sender, EventArgs e)
        {
            l_health.Text = nud_health.Value.ToString();
        }

        private void nud_power_ValueChanged(object sender, EventArgs e)
        {
            l_power.Text = nud_power.Value.ToString();
        }

        private void nud_healthRegen_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nud_powerRecharge_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nud_movementSpeed_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nud_criticalChanceLeft_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nud_criticalChanceRight_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nud_criticalDamageLeft_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nud_criticalDamageRight_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
