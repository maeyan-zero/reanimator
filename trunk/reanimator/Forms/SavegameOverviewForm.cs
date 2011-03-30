using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reanimator.Forms.HeroEditorFunctions;

namespace Reanimator.Forms
{
    public partial class SavegameOverviewForm : Form
    {
        public SavegameOverviewForm()
        {
            InitializeComponent();
            savegameOverviewControl1.EntrySelectedEvent += new EntrySelected(savegameOverviewControl1_EntrySelectedEvent);
        }

        void savegameOverviewControl1_EntrySelectedEvent(object sender, EventArgs e)
        {
            SavegameOverviewEntry entry = (SavegameOverviewEntry)sender;
            UnitWrapper2 unit = (UnitWrapper2)entry.Tag;
            textBox1.Text = unit.Name;
        }

        public void AddCharacter(UnitWrapper2 wrapper)
        {
            savegameOverviewControl1.AddCharacter(wrapper);
        }
    }
}
