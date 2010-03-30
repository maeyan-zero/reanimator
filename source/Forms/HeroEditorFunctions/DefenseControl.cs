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
    public partial class DefenseControl : ParentStatusControl
    {
        public DefenseControl()
        {
            InitializeComponent();

            this.SetLabelText("DEFENSE");
            this.MaximizedSize = this.BackgroundImage.Size;
        }

        private void nud_shields_ValueChanged(object sender, EventArgs e)
        {
            l_shields.Text = nud_shields.Value.ToString();
        }

        private void nud_armor_ValueChanged(object sender, EventArgs e)
        {
            l_armor.Text = nud_armor.Value.ToString();
        }

        private void nud_shieldRecharge_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nud_stun_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nud_ignite_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nud_shock_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nud_phase_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nud_poison_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
