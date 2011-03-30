using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reanimator.Forms.HeroEditorFunctions;

namespace Reanimator.Forms
{
    public partial class SavegameOverviewControl : UserControl
    {
        public event EntrySelected EntrySelectedEvent = delegate { };

        public SavegameOverviewControl()
        {
            InitializeComponent();
        }

        public void AddCharacter(UnitWrapper2 wrapper)
        {
            SavegameOverviewEntry entry = new SavegameOverviewEntry();
            entry.EntrySelectedEvent += new EntrySelected(entry_EntrySelectedEvent);
            entry.Tag = wrapper;
            entry.CharacterName = wrapper.Name;
            entry.Gender = wrapper.CharacterWrapper.Gender.ToString();
            entry.Class = wrapper.CharacterWrapper.ClassName;
            entry.Level = wrapper.CharacterWrapper.CharacterValues.Level.ToString();
            entry.Palladium = string.Format("{0:N0}", wrapper.CharacterWrapper.CharacterValues.Palladium);
            entry.Playtime = wrapper.CharacterWrapper.CharacterValues.PlayTimeString;

            entry.Location = new Point(0, this.Controls.Count * entry.Height);
            this.Controls.Add(entry);
        }

        void entry_EntrySelectedEvent(object sender, EventArgs e)
        {
            EntrySelectedEvent(sender, e);
        }
    }
}
