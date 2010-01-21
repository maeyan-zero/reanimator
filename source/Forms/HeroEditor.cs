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
    public partial class HeroEditor : Form
    {
        HeroUnit heroUnit;
        ExcelTables excelTables;

        public HeroEditor(HeroUnit unit, ExcelTables tables)
        {
            heroUnit = unit;
            excelTables = tables;

            InitializeComponent();

            PopulateStats();
            PopulateItems();
        }

        private void ListBox1SelectedIndexChanged(object sender, EventArgs e)
        {
            // this is just a quick nasty test - ignore me
            Unit[] items = heroUnit.Items;
            Unit item = items[this.itemListBox.SelectedIndex];
            for (int i = 0; i < item.statBlock.statCount; i++)
            {
                UnitStat stat = item.statBlock.stats[i];
                if (i == 0)
                    this.textBox1.Text = "0x" + stat.statId.ToString("X4") + " : " + stat.values[0];
                if (i == 1)
                    this.textBox2.Text = "0x" + stat.statId.ToString("X4") + " : " + stat.values[0];
                if (i == 2)
                    this.textBox3.Text = "0x" + stat.statId.ToString("X4") + " : " + stat.values[0];
                if (i == 3)
                    this.textBox4.Text = "0x" + stat.statId.ToString("X4") + " : " + stat.values[0];
                if (i == 4)
                    this.textBox5.Text = "0x" + stat.statId.ToString("X4") + " : " + stat.values[0];
            }
        }

        private void PopulateItems()
        {
            Unit[] items = heroUnit.Items;
            for (int i = 0; i < items.Length; i++)
            {
                Unit item = items[i];
                this.itemListBox.Items.Add("Item #" + i);
            }
        }

        private void PopulateStats()
        {
            HeroUnitStats stats = heroUnit.Stats;
            for (int i = 0; i < stats.Count; i++)
            {
                int statId = stats.StatIdAt(i);
                this.charListBox.Items.Add(excelTables.Stats.GetStringFromId(statId));
            }
        }
    }
}