using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reanimator.Forms
{
    public partial class ModificationForm : Form
    {
        Mod.Revival revivalMod;

        public ModificationForm()
        {
            InitializeComponent();
        }

        public ModificationForm(Mod.Revival revivalMod)
        {
            InitializeComponent();

            this.revivalMod = revivalMod;

            foreach (Mod.Modification mod in revivalMod)
            {
                checkedListBox.Items.Add(mod.title, mod.GetListEnable());
            }
        }

        private void checkedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxDescription.Text = revivalMod.modification[this.checkedListBox.SelectedIndex].GetListDescription();
        }

        private void ModificationForm_Load(object sender, EventArgs e)
        {
            checkedListBox.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
