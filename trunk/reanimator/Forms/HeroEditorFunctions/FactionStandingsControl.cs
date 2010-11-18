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
    public partial class FactionStandingsControl : ParentStatusControl
    {
        public FactionStandingsControl()
        {
            const int MAXIMUMSTANDING = 1601;

            InitializeComponent();

            this.SetLabelText("FACTION STANDINGS");
            this.MaximizedSize = this.BackgroundImage.Size;

            richTextBox1.SelectAll();
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox2.SelectAll();
            richTextBox2.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox3.SelectAll();
            richTextBox3.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox4.SelectAll();
            richTextBox4.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox5.SelectAll();
            richTextBox5.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox6.SelectAll();
            richTextBox6.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox7.SelectAll();
            richTextBox7.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox8.SelectAll();
            richTextBox8.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox9.SelectAll();
            richTextBox9.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox10.SelectAll();
            richTextBox10.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox11.SelectAll();
            richTextBox11.SelectionAlignment = HorizontalAlignment.Center;

            nud_Broker.Maximum = MAXIMUMSTANDING;
            nud_factionWithTheBrothers.Maximum = MAXIMUMSTANDING;
            nud_templarFaction.Maximum = MAXIMUMSTANDING;
            nud_thePeopleOfCharingCrossStation.Maximum = MAXIMUMSTANDING;
            nud_thePeopleOfCoventGardenStation.Maximum = MAXIMUMSTANDING;
            nud_thePeopleOfHolbornStation.Maximum = MAXIMUMSTANDING;
            nud_thePeopleOfLiverpoolStation.Maximum = MAXIMUMSTANDING;
            nud_thePeopleOfMonumentStation.Maximum = MAXIMUMSTANDING;
            nud_thePeopleOfStPaulsStation.Maximum = MAXIMUMSTANDING;
            nud_thePeopleOfTemplarBase.Maximum = MAXIMUMSTANDING;
            nud_thePeopleOfTempleStation.Maximum = MAXIMUMSTANDING;

            string labelText = "/ " + MAXIMUMSTANDING.ToString();
            label1.Text = labelText;
            label2.Text = labelText;
            label3.Text = labelText;
            label4.Text = labelText;
            label5.Text = labelText;
            label6.Text = labelText;
            label7.Text = labelText;
            label9.Text = labelText;
            label10.Text = labelText;
            label11.Text = labelText;
            label12.Text = labelText;
        }

        private void nud_templarFaction_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nud_thePeopleOfHolbornStation_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nud_thePeopleOfCoventGardenStation_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nud_thePeopleOfCharingCrossStation_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nud_thePeopleOfTempleStation_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nud_thePeopleOfTemplarBase_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nud_thePeopleOfMonumentStation_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nud_thePeopleOfLiverpoolStation_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nud_Broker_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nud_thePeopleOfStPaulsStation_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nud_factionWithTheBrothers_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
