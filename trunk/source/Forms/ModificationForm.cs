using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reanimator.Excel;

namespace Reanimator.Forms
{
    public partial class ModificationForm : Form
    {
        Mod revivalMod;

        public ModificationForm()
        {
            InitializeComponent();
        }

        public ModificationForm(Mod revivalMod)
        {
            InitializeComponent();

            this.revivalMod = revivalMod;

            for (int i = 0; i < revivalMod.Length; i++)
            {
                checkedListBox.Items.Add(revivalMod.Title(i), revivalMod.Enabled(i));
            }
        }

        private void checkedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxDescription.Text = revivalMod.Description(checkedListBox.SelectedIndex);
        }

        private void ModificationForm_Load(object sender, EventArgs e)
        {
            checkedListBox.SelectedIndex = 0;

            for (int i = 0; i < revivalMod.Length; i++ )
            {
                if (revivalMod.Enabled(i))
                {
                    revivalMod.Apply(i, true);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.CurrentValue == CheckState.Checked)
            {
                revivalMod.Apply(checkedListBox.Items.Count - 1, true);
            }
            else
            {
                revivalMod.Apply(checkedListBox.Items.Count - 1, false);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            revivalMod.Apply();
        }
    }
}
