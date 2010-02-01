﻿using System;
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
        Unit heroUnit;
        ExcelTables excelTables;
        String filePath;
        string savedPath;

        public HeroEditor(Unit unit, ExcelTables tables, String file)
        {
            heroUnit = unit;
            excelTables = tables;
            filePath = file;

            InitializeComponent();
        }

        private void ListBox1SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            // this is just a quick nasty test - ignore me
            Unit[] items = heroUnit.Items;
            Unit item = items[this.items_ListBox.SelectedIndex];
            for (int i = 0; i < item.statBlock.statCount; i++)
            {
                Unit.Stat stat = item.statBlock.stats[i];
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
            }*/
        }

        private void PopulateItems(Unit unit)
        {
            Unit[] items = unit.Items;
            for (int i = 0; i < items.Length; i++)
            {
                Unit item = items[i];
                item.Name = "Item #" + item.unknownFlagValue;
                this.currentlyEditing_ComboBox.Items.Add(item);
            }
        }

        private void PopulateStats(Unit unit)
        {
            stats_ListBox.Items.Clear();

            for (int i = 0; i < unit.Stats.Length; i++)
            {
                Unit.StatBlock.Stat stat = unit.Stats[i];
                stat.Name = ((Stats)excelTables.GetTable("stats")).GetStringFromId(stat.Id);

                stats_ListBox.Items.Add(stat);
            }
        }

        private void charStats_ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.panel1.Controls.Clear();

            Unit.StatBlock.Stat stat = (Unit.StatBlock.Stat)this.stats_ListBox.SelectedItem;

            int heightOffset = 0;
            for (int i = 0; i < stat.Length; i++)
            {
                Unit.StatBlock.Stat.Values statValues = stat[i];

                bool hasExtraAttribute = false;
                for (int j = 0; j < 3; j++)
                {
                    if (stat.AttributeAt(j) == null)
                    {
                        break;
                    }

                    Label eaValueLabel = new Label();
                    eaValueLabel.Text = "Attr" + j + ": ";
                    eaValueLabel.Width = 40;
                    eaValueLabel.Top = 3 + heightOffset;
                    TextBox eaValueTextBox = new TextBox();
                    eaValueTextBox.Text = stat.AttributeAt(j).ToString();
                    eaValueTextBox.Left = eaValueLabel.Right;
                    eaValueTextBox.Top = heightOffset;
                    eaValueTextBox.DataBindings.Add("Text", statValues, "Attribute" + (j + 1));

                    this.panel1.Controls.Add(eaValueLabel);
                    this.panel1.Controls.Add(eaValueTextBox);

                    heightOffset += 25;
                    hasExtraAttribute = true;
                }

                int leftOffset = 0;
                if (hasExtraAttribute)
                {
                    leftOffset += 35;
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

                heightOffset += 45;
            }
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
            int startIndex = filePath.LastIndexOf("\\") + 1;
            string characterName = filePath.Substring(startIndex, filePath.Length - startIndex - 4);
            FileStream saveFile = new FileStream(characterName + ".hg1", FileMode.Create, FileAccess.ReadWrite);
            savedPath = saveFile.Name;

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
            string charString = characterName;
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
            byte[] saveData = heroUnit.GenerateSaveData(charStringBytes);
            saveFile.Write(saveData, 0, saveData.Length);

            saveFile.Close();
        }

        private void HeroEditor_Load(object sender, EventArgs e)
        {
            currentlyEditing_ComboBox.Items.Add(heroUnit);
            currentlyEditing_ComboBox.SelectedIndex = 0;

            PopulateGeneral(heroUnit);
            PopulateStats(heroUnit);
            PopulateItems(heroUnit);
        }

        private void PopulateGeneral(Unit heroUnit)
        {
          name_TextBox.Text = heroUnit.ToString();

          string job;
          switch (heroUnit.JobClass)
          {
            case (93):
              {
                job = "Summoner";
              }
              break;
            case (97):
              {
                job = "Guardian";
              }
              break;
            case (124):
              {
                job = "Marksman";
              }
              break;
            case (141):
              {
                job = "Evoker";
              }
              break;
            case (180):
              {
                job = "Blademaster";
              }
              break;
            case (191):
              {
                job = "Engineer";
              }
              break;
            default:
              {
                job = "Unknown";
              }
              break;
          }

            textBox1.Text = String.Format("{0:00000000}", Int32.Parse(Convert.ToString(heroUnit.JobClass, 2)));
            class_TextBox.Text = job;

            SetCheckBoxes();
            // sets the level label and corrects the offset to display the real level
            level_NumericUpDown.Value = heroUnit.Stats[0].values[0].Stat - 8;

            DisplayFlags();

            initialized = true;
        }

        // flag used to prevent overwriting of the character game mode when loading the check box states
        bool initialized = false;

        private void SetCheckBoxes()
        {
            if (heroUnit.Flags1 != null && heroUnit.Flags1.Contains(21062))
            {
                elite_CheckBox.Checked = true;
            }
            if (heroUnit.Flags2 != null && heroUnit.Flags2.Contains(18243))
            {
                hardcore_CheckBox.Checked = true;
            }
            if (heroUnit.Flags2 != null && heroUnit.Flags2.Contains(18499))
            {
                dead_CheckBox.Checked = true;
            }
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (initialized)
            {
                heroUnit.SetGameMode(elite_CheckBox.Checked, hardcore_CheckBox.Checked, dead_CheckBox.Checked);
            }
        }

        private void DisplayFlags()
        {
            richTextBox1.Text = string.Empty;

            richTextBox1.Text += "Flag1:\n";
            richTextBox1.Text += heroUnit.PlayerFlagCount1 + "\n";
            if (heroUnit.Flags1 != null)
            {
                richTextBox1.Text += "Array size: " + heroUnit.Flags1.Length + "\n";

                foreach (int flag in heroUnit.Flags1)
                {
                    richTextBox1.Text += flag.ToString() + "\n";
                }
            }
            else
            {
                richTextBox1.Text += "null";
            }

            richTextBox1.Text += "\n\nFlag2:\n";
            richTextBox1.Text += heroUnit.PlayerFlagCount2 + "\n";
            if (heroUnit.Flags2 != null)
            {
                richTextBox1.Text += "Array size: " + heroUnit.Flags2.Length + "\n";

                foreach (int flag in heroUnit.Flags2)
                {
                    richTextBox1.Text += flag.ToString() + "\n";
                }
            }
            else
            {
                richTextBox1.Text += "Array = null";
            }
        }

        private void hardcore_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            dead_CheckBox.Enabled = hardcore_CheckBox.Checked;
            dead_CheckBox.Checked = false;

            CheckBox_CheckedChanged(sender, e);
        }

        private void currentlyEditing_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Unit unit = (Unit)currentlyEditing_ComboBox.SelectedItem;

            PopulateStats(unit);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Config.gameClientPath, "-singleplayer -load\"" + savedPath + "\"");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to start game at:\n" + Config.gameClientPath + "\n\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DisplayFlags();
        }
    }
}