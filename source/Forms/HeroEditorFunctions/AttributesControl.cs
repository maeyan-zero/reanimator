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
    public partial class AttributesControl : ParentStatusControl
    {
        public AttributesControl()
        {
            InitializeComponent();

            this.SetLabelText("ATTRIBUTES");
            if (this.BackgroundImage != null)
            {
                this.MaximizedSize = this.BackgroundImage.Size;
            }
        }

        private void b_accuracyAdd_Click(object sender, EventArgs e)
        {
            if (nud_accuracy.Value < nud_accuracy.Maximum)
            {
                nud_accuracy.Value++;
            }
        }

        private void b_strengthAdd_Click(object sender, EventArgs e)
        {
            if (nud_strength.Value < nud_strength.Maximum)
            {
                nud_strength.Value++;
            }
        }

        private void b_staminaAdd_Click(object sender, EventArgs e)
        {
            if (nud_stamina.Value < nud_stamina.Maximum)
            {
                nud_stamina.Value++;
            }
        }

        private void b_willpowerAdd_Click(object sender, EventArgs e)
        {
            if (nud_willpower.Value < nud_willpower.Maximum)
            {
                nud_willpower.Value++;
            }
        }

        private void b_accuracySubstract_Click(object sender, EventArgs e)
        {
            if (nud_accuracy.Value > nud_accuracy.Minimum)
            {
                nud_accuracy.Value--;
            }
        }

        private void b_strengthSubstract_Click(object sender, EventArgs e)
        {
            if (nud_strength.Value > nud_strength.Minimum)
            {
                nud_strength.Value--;
            }
        }

        private void b_staminaSubstract_Click(object sender, EventArgs e)
        {
            if (nud_stamina.Value > nud_stamina.Minimum)
            {
                nud_stamina.Value--;
            }
        }

        private void b_willpowerSubstract_Click(object sender, EventArgs e)
        {
            if (nud_willpower.Value > nud_willpower.Minimum)
            {
                nud_willpower.Value--;
            }
        }

        private void nud_accuracy_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nud_strength_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nud_stamina_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nud_willpower_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nud_attributePoints_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
