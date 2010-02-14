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
using System.Xml.Serialization;

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
            try
            {
                heroUnit = unit;
                excelTables = tables;
                filePath = file;

                InitializeComponent();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Constructor");
            }
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
            try
            {
                Unit[] items = unit.Items;
                for (int i = 0; i < items.Length; i++)
                {
                    Unit item = items[i];
                    item.Name = "Item #" + item.unknownFlagValue;
                    this.currentlyEditing_ComboBox.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source);
            }
        }

        private void PopulateStats(Unit unit)
        {
            try
            {
                stats_ListBox.Items.Clear();

                for (int i = 0; i < unit.Stats.Length; i++)
                {
                    Unit.StatBlock.Stat stat = unit.Stats[i];

                    //string txt = string.Empty;
                    //for (int counter = 0; counter < excelTables.Count; counter++)
                    //{
                    //    txt += excelTables.GetTableStringId(counter) + "\n";
                    //}
                    //MessageBox.Show(txt);

                    ExcelTable table = excelTables.GetTable("stats");
                    Stats stats = (Stats)table;

                    stat.Name = stats.GetStringFromId(stat.Id);
                    //stat.Name = ((Stats)excelTables.GetTable("stats")).GetStringFromId(stat.Id);

                    stats_ListBox.Items.Add(stat);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "PopulateStats");
            }
        }

        private void charStats_ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.panel1.Controls.Clear();

                Unit.StatBlock.Stat stat = (Unit.StatBlock.Stat)this.stats_ListBox.SelectedItem;

                // yea, copy/paste nastiness ftw
                if (stat.Attribute1 != null)
                {
                    statAttribute1_GroupBox.Enabled = true;
                    statAttribute1_bitCount_TextBox.DataBindings.Clear();
                    statAttribute1_unknown1_TextBox.DataBindings.Clear();
                    statAttribute1_unknown1_1_TextBox.DataBindings.Clear();
                    statAttribute1_tableId_TextBox.DataBindings.Clear();
                    statAttribute1_bitCount_TextBox.DataBindings.Add("Text", stat.Attribute1, "BitCount");
                    statAttribute1_unknown1_TextBox.DataBindings.Add("Text", stat.Attribute1, "Unknown1");
                    statAttribute1_unknown1_1_TextBox.DataBindings.Add("Text", stat.Attribute1, "Unknown1_1");
                    if (stat.Attribute1.TableId > 0)
                    {
                        statAttribute1_tableId_TextBox.Text = excelTables.GetTable(stat.Attribute1.TableId).StringId;
                    }
                }
                else
                {
                    statAttribute1_GroupBox.Enabled = false;
                }
                if (stat.Attribute2 != null)
                {
                    statAttribute2_GroupBox.Enabled = true;
                    statAttribute2_bitCount_TextBox.DataBindings.Clear();
                    statAttribute2_unknown1_TextBox.DataBindings.Clear();
                    statAttribute2_unknown1_1_TextBox.DataBindings.Clear();
                    statAttribute2_tableId_TextBox.DataBindings.Clear();
                    statAttribute2_bitCount_TextBox.DataBindings.Add("Text", stat.Attribute2, "BitCount");
                    statAttribute2_unknown1_TextBox.DataBindings.Add("Text", stat.Attribute2, "Unknown1");
                    statAttribute2_unknown1_1_TextBox.DataBindings.Add("Text", stat.Attribute2, "Unknown1_1");
                    statAttribute2_tableId_TextBox.DataBindings.Add("Text", stat.Attribute2, "TableId");
                    if (stat.Attribute2.TableId > 0)
                    {
                        statAttribute2_tableId_TextBox.Text = excelTables.GetTable(stat.Attribute2.TableId).StringId;
                    }
                }
                else
                {
                    statAttribute2_GroupBox.Enabled = false;
                }
                if (stat.Attribute3 != null)
                {
                    statAttribute3_GroupBox.Enabled = true;
                    statAttribute3_bitCount_TextBox.DataBindings.Clear();
                    statAttribute3_unknown1_TextBox.DataBindings.Clear();
                    statAttribute3_unknown1_1_TextBox.DataBindings.Clear();
                    statAttribute3_tableId_TextBox.DataBindings.Clear();
                    statAttribute3_bitCount_TextBox.DataBindings.Add("Text", stat.Attribute3, "BitCount");
                    statAttribute3_unknown1_TextBox.DataBindings.Add("Text", stat.Attribute3, "Unknown1");
                    statAttribute3_unknown1_1_TextBox.DataBindings.Add("Text", stat.Attribute3, "Unknown1_1");
                    statAttribute3_tableId_TextBox.DataBindings.Add("Text", stat.Attribute3, "TableId");
                    if (stat.Attribute3.TableId > 0)
                    {
                        statAttribute3_tableId_TextBox.Text = excelTables.GetTable(stat.Attribute3.TableId).StringId;
                    }
                }
                else
                {
                    statAttribute3_GroupBox.Enabled = false;
                }

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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "charStats_ListBox_SelectedIndexChanged");
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
            try
            {
                currentlyEditing_ComboBox.Items.Add(heroUnit);
                currentlyEditing_ComboBox.SelectedIndex = 0;

                PopulateGeneral(heroUnit);
                PopulateStats(heroUnit);
                PopulateItems(heroUnit);


                PopulateMinigame();
                PopulateWaypoints();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "HeroEditor_Load");
            }
        }

        public static void Serialize(Unit.StatBlock.Stat stats, string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Unit.StatBlock.Stat));
            TextWriter tw = new StreamWriter(path);
            serializer.Serialize(tw, stats);
            tw.Close();
        }

        public static Unit.StatBlock.Stat Deserialize(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Unit.StatBlock.Stat));
            TextReader tr = new StreamReader(path);
            Unit.StatBlock.Stat stats = (Unit.StatBlock.Stat)serializer.Deserialize(tr);
            tr.Close();

            return stats;
        }

        private void PopulateMinigame()
        {
            Unit.StatBlock.Stat minigame = GetComplexValue("minigame_category_needed");

            // As long as VS won't let me place the control in the form by hand I'll initialize it here
            MinigameControl control = new MinigameControl(minigame.values);
            p_miniGame.Controls.Add(control);
        }

        private void PopulateWaypoints()
        {
            Unit.StatBlock.Stat wayPoints = GetComplexValue("waypoint_flags");

            if (wayPoints != null)
            {
                if (wayPoints.values.Length >= 1)
                {
                    WayPointControl wpcNormal = new WayPointControl(wayPoints.values[0]);
                    p_wpNormal.Controls.Add(wpcNormal);
                }
                if (wayPoints.values.Length >= 2)
                {
                    WayPointControl wpcNightmare = new WayPointControl(wayPoints.values[1]);
                    p_wpNightmare.Controls.Add(wpcNightmare);
                }

                //Serialize(wayPoints, @"F:\test.xml");
            }
            else
            {
                //wayPoints = Deserialize(@"F:\test.xml");
            }
        }

        private void PopulateGeneral(Unit heroUnit)
        {
            try
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

                SetCharacterValues();

                DisplayFlags();

                initialized = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "PopulateGeneral");
            }
        }

        private void SetCharacterValues()
        {
            try
            {
                int level = GetSimpleValue("level");
                level_NumericUpDown.Value = level - 8;


                int palladium = GetSimpleValue("gold");
                // when palladium reaches 9999999 the ingame value is set to a max value ao something like 16000000
                if (palladium > 9999999)
                {
                    palladium = 9999999;
                }
                //should not occur, but better be save than sorry
                else if (palladium < 0)
                {
                    palladium = 0;
                }
                nud_palladium.Value = palladium;

                int statPoints = GetSimpleValue("stat_points");
                if (statPoints < 0)
                {
                    statPoints = 0;
                }
                nud_statPoints.Value = statPoints;

                int skillPoints = GetSimpleValue("skill_points");
                if (skillPoints < 0)
                {
                    skillPoints = 0;
                }
                nud_skillPoints.Value = skillPoints;

                int accuracy = GetSimpleValue("accuracy");
                nud_accuracy.Value = accuracy;

                int strength = GetSimpleValue("strength");
                nud_strength.Value = strength;

                int stamina = GetSimpleValue("stamina");
                nud_stamina.Value = stamina;

                int willpower = GetSimpleValue("willpower");
                nud_willpower.Value = willpower;

                int health = GetSimpleValue("hp_cur");
                nud_health.Value = health;

                int power = GetSimpleValue("power_cur");
                nud_power.Value = power;

                int shields = GetSimpleValue("shield_buffer_cur");
                nud_shields.Value = shields;

                int armor = GetSimpleValue("power_max");
                //nud_armor.Value = armor;

                int sfxDefence = GetSimpleValue("sfx_defense_bonus");
                nud_sfxDefence.Value = sfxDefence - 100;

                int currentAP = GetSimpleValue("achievement_points_total");
                nud_currentAP.Value = currentAP;

                int maxAP = GetSimpleValue("achievement_points_cur");
                nud_maxAP.Value = maxAP;

                int playTime = GetSimpleValue("played_time_in_seconds");
                TimeSpan t = TimeSpan.FromSeconds(playTime);

                string time = string.Format("{0:D2}d {0:D2}h {1:D2}m {2:D2}s", t.Days, t.Hours, t.Minutes, t.Seconds);
                tb_playedTime.Text = time;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SetCharacterValues");
            }
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


        #region modify character values
        private void SetSimpleValue(string valueName, int value)
        {
            if (initialized)
            {
                for (int counter = 0; counter < heroUnit.Stats.Length; counter++)
                {
                    Unit.StatBlock.Stat unit = heroUnit.Stats[counter];

                    if (unit.Name == valueName)
                    {
                        unit.values[0].Stat = value;
                        return;
                    }
                }
            }
        }

        private void SetComplexValue(string valueName, Unit.StatBlock.Stat stat)
        {
            if (initialized)
            {
                for (int counter = 0; counter < heroUnit.Stats.Length; counter++)
                {
                    Unit.StatBlock.Stat unit = heroUnit.Stats[counter];

                    if (unit.Name == valueName)
                    {
                        unit = stat;
                        return;
                    }
                }
            }
        }

        private int GetSimpleValue(string valueName)
        {
            for (int counter = 0; counter < heroUnit.Stats.Length; counter++)
            {
                Unit.StatBlock.Stat unit = heroUnit.Stats[counter];

                if (unit.Name == valueName)
                {
                    return unit.values[0].Stat;
                }
            }
            MessageBox.Show("Field \"" + valueName + "\" not present in current save file!");
            return 0;
        }

        private Unit.StatBlock.Stat GetComplexValue(string valueName)
        {
            for (int counter = 0; counter < heroUnit.Stats.Length; counter++)
            {
                Unit.StatBlock.Stat unit = heroUnit.Stats[counter];

                if (unit.Name.Equals(valueName, StringComparison.OrdinalIgnoreCase))
                {
                    return unit;
                }
            }
            return null;
        }

        private void level_NumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            SetSimpleValue("level", (int)level_NumericUpDown.Value + 8);
        }

        private void palladium_numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            SetSimpleValue("gold", (int)nud_palladium.Value);
        }

        private void skillPoints_numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            SetSimpleValue("skill_points", (int)nud_skillPoints.Value);
        }

        private void statPoints_numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            SetSimpleValue("stat_points", (int)nud_statPoints.Value);
        }

        private void nud_accuracy_ValueChanged(object sender, EventArgs e)
        {
            SetSimpleValue("accuracy", (int)nud_accuracy.Value);
        }

        private void nud_strength_ValueChanged(object sender, EventArgs e)
        {
            SetSimpleValue("strength", (int)nud_strength.Value);
        }

        private void nud_stamina_ValueChanged(object sender, EventArgs e)
        {
            SetSimpleValue("stamina", (int)nud_stamina.Value);
        }

        private void nud_willpower_ValueChanged(object sender, EventArgs e)
        {
            SetSimpleValue("willpower", (int)nud_willpower.Value);
        }


        private void nud_shields_ValueChanged(object sender, EventArgs e)
        {
            SetSimpleValue("shield_buffer_cur", (int)nud_shields.Value);
        }

        private void nud_armor_ValueChanged(object sender, EventArgs e)
        {
            //SetSimpleValue("power_max", (int)nud_armor.Value);
        }

        private void nud_currentAP_ValueChanged(object sender, EventArgs e)
        {
            SetSimpleValue("achievement_points_cur", (int)nud_currentAP.Value);
        }

        private void nud_maxAP_ValueChanged(object sender, EventArgs e)
        {
            SetSimpleValue("achievement_points_total", (int)nud_maxAP.Value);
        }

        private void nud_health_ValueChanged(object sender, EventArgs e)
        {
            SetSimpleValue("hp_cur", (int)nud_health.Value);
        }

        private void nud_power_ValueChanged(object sender, EventArgs e)
        {
            SetSimpleValue("power_cur", (int)nud_power.Value);
        }

        private void nud_sfxDefence_ValueChanged(object sender, EventArgs e)
        {
            SetSimpleValue("sfx_defense_bonus", (int)nud_sfxDefence.Value + 100);
        }

        #endregion
    }
}