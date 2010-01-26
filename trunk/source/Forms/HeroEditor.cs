using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reanimator.Excel;
using System.IO;
using System.Runtime.InteropServices;

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
            Unit item = items[this.items_ListBox.SelectedIndex];
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
                this.items_ListBox.Items.Add("Item #" + i);
            }
        }

        private void PopulateStats()
        {
            foreach (HeroUnitStat stat in heroUnit.Stats)
            {
                this.charStats_ListBox.Items.Add(stat);
            }
        }


        private void charStats_ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.panel1.Controls.Clear();

            HeroUnitStat stat = (HeroUnitStat)this.charStats_ListBox.SelectedItem;
            charStatValues_ListBox.Items.Clear();

            int heightOffset = 0;
            for (int i = 0; i < stat.Length; i++)
            {
                Unit.StatValues statValues = stat.Values(i);

                bool blag = false;
                for (int j = 0; j < 3; j++)
                {
                    int extraAttribute = 0;
                    if (j == 0)
                        extraAttribute = statValues.ExtraAttribute1;
                    if (j == 1)
                        extraAttribute = statValues.ExtraAttribute2;
                    if (j == 2)
                        extraAttribute = statValues.ExtraAttribute3;

                    if (extraAttribute == 0)
                        continue;

                    Label eaValueLabel = new Label();
                    eaValueLabel.Text = "Attr" + j + ": ";
                    eaValueLabel.Width = 40;
                    eaValueLabel.Top = 3 + heightOffset;
                    TextBox eaValueTextBox = new TextBox();
                    eaValueTextBox.Text = extraAttribute.ToString();
                    eaValueTextBox.Left = eaValueLabel.Right;
                    eaValueTextBox.Top = heightOffset;
                    eaValueTextBox.DataBindings.Add("Text", statValues, "ExtraAttribute" + (j+1));

                    this.panel1.Controls.Add(eaValueLabel);
                    this.panel1.Controls.Add(eaValueTextBox);

                    charStatValues_ListBox.Items.Add("  *" + extraAttribute);

                    heightOffset += 25;
                    blag = true;
                }

                int leftOffset = 0;
                if (blag)
                {
                    leftOffset += 40;
                }

                Label valueLabel = new Label();
                valueLabel.Text = "Value: ";
                valueLabel.Left = leftOffset;
                valueLabel.Width = 40;
                valueLabel.Top = 3 + heightOffset;
                TextBox valueTextBox = new TextBox();
                valueTextBox.Text = statValues.Stat.ToString();
                valueTextBox.Left = valueLabel.Right;
                valueTextBox.Top = heightOffset;
                valueTextBox.DataBindings.Add("Text", statValues, "Stat");

                this.panel1.Controls.Add(valueLabel);
                this.panel1.Controls.Add(valueTextBox);

                charStatValues_ListBox.Items.Add(statValues.Stat);
                heightOffset += 45;
            }
        }

        private void charStats_ListBox_Resize(object sender, EventArgs e)
        {
            charStatValues_ListBox.Height = charStats_ListBox.Height;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        protected struct MainHeader
        {
            public Int32 flag;
            public Int32 version;
            public Int32 dataOffset1;
            public Int32 dataOffset2;
        };

        private void saveCharButton_Click(object sender, EventArgs e)
        {
            string character = "Alex";
            FileStream saveFile = new FileStream(character + ".hg1", FileMode.Create, FileAccess.ReadWrite);

            // main header
            MainHeader mainHeader;
            mainHeader.flag = 0x484D4752; // "RGMH"
            mainHeader.version = 1;
            mainHeader.dataOffset1 = 0x2028;
            mainHeader.dataOffset2 = 0x2028;
            byte[] data = FileTools.StructureToByteArray(mainHeader);
            saveFile.Write(data, 0, data.Length);

            // hellgate string (is this needed?)
            string hellgateString = "Hellgate: London";
            byte[] hellgateStringBytes = FileTools.StringToUnicodeByteArray(hellgateString);
            saveFile.Seek(0x28, SeekOrigin.Begin);
            saveFile.Write(hellgateStringBytes, 0, hellgateStringBytes.Length);

            // char name (not actually used in game though I don't think)  (is this needed?)
            string charString = character;
            byte[] charStringBytes = FileTools.StringToUnicodeByteArray(charString);
            saveFile.Seek(0x828, SeekOrigin.Begin);
            saveFile.Write(charStringBytes, 0, charStringBytes.Length);

            // no detail string (is this needed?)
            string noDetailString = "No detail";
            byte[] noDetailStringBytes = FileTools.StringToUnicodeByteArray(noDetailString);
            saveFile.Seek(0x1028, SeekOrigin.Begin);
            saveFile.Write(noDetailStringBytes, 0, noDetailStringBytes.Length);

            // load char string (is this needed?)
            string loadCharacterString = "Load this Character";
            byte[] loadCharacterStringBytes = FileTools.StringToUnicodeByteArray(loadCharacterString);
            saveFile.Seek(0x1828, SeekOrigin.Begin);
            saveFile.Write(loadCharacterStringBytes, 0, loadCharacterStringBytes.Length);

            // main character data
            saveFile.Seek(0x2028, SeekOrigin.Begin);
            byte[] saveData = heroUnit.GenerateSaveData();
            saveFile.Write(saveData, 0, saveData.Length);

            saveFile.Close();
        }
    }
}