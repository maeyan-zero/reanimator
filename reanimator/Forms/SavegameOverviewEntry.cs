using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reanimator.Forms
{
    public delegate void EntrySelected(object sender, EventArgs e); 
    public partial class SavegameOverviewEntry : UserControl
    {
        public event EntrySelected EntrySelectedEvent = delegate { };

        public string CharacterName
        {
            get { return labelName.Text; }
            set { labelName.Text = value; }
        }

        public string Level
        {
            get { return labelLevel.Text; }
            set { labelLevel.Text = value; }
        }

        public string Class
        {
            get { return labelClass.Text; }
            set { labelClass.Text = value; }
        }

        public string Gender
        {
            get { return labelGender.Text; }
            set { labelGender.Text = value; }
        }

        public string Playtime
        {
            get { return labelPlaytime.Text; }
            set { labelPlaytime.Text = value; }
        }

        public string Palladium
        {
            get { return labelPalladium.Text; }
            set { labelPalladium.Text = value; }
        }

        public SavegameOverviewEntry()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ABC");
        }

        private void buttonSelectEntry_Click(object sender, EventArgs e)
        {
            EntrySelectedEvent(this, e);
        }
    }
}
