using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Reanimator.Excel;
using System.IO;
using System.Xml.Serialization;
using System.Drawing;
using Reanimator.Forms.HeroEditorFunctions;

namespace Reanimator.Forms
{
    public partial class HeroEditor : Form
    {
        enum CharacterClass
        {
            Blademaster = 1,
            Guardian = 3,

            Evoker = 6,
            Summoner = 8,

            Marksman = 11,
            Engineer = 13
        }

        readonly Unit _heroUnit;
        readonly TableDataSet _dataSet;
        readonly ExcelTables _excelTables;
        readonly CompletePanelControl _panel;
        readonly String _filePath;
        readonly UnitHelpFunctions _itemFunctions;
        CharacterClass _characterClass;

        public HeroEditor(Unit heroUnit, TableDataSet tableDataSet, String filePath)
        {
            _heroUnit = heroUnit;
            _dataSet = tableDataSet;
            _excelTables = tableDataSet.ExcelTables;
            _panel = new CompletePanelControl();
            //_statsTable = _excelTables.GetTable("stats") as Stats;
            _filePath = filePath;

            _itemFunctions = new UnitHelpFunctions(ref _dataSet, ref _excelTables);
            _itemFunctions.LoadCharacterValues(_heroUnit);
            //_itemFunctions.GenerateUnitNameStrings();
            //_itemFunctions.PopulateItems(ref _heroUnit);

            InitializeComponent();
        }

        private void HeroEditor_Load(object sender, EventArgs e)
        {
            currentlyEditing_ComboBox.Items.Add(_heroUnit);
            currentlyEditing_ComboBox.SelectedIndex = 0;

            PopulateGeneral(_heroUnit);

            PopulateStats(_heroUnit);
            PopulateItemDropDown(_heroUnit);

            PopulateMinigame();
            PopulateWaypoints();

            InitUnknownStatList();

            int charClassId = (int)_characterClass;
            InitializeAttributeSkillPanel(charClassId);

            InitInventory();
        }

        private void PopulateItemDropDown(Unit unit)
        {
            foreach (Unit item in unit.Items)
            {
                currentlyEditing_ComboBox.Items.Add(item);
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
                panel1.Controls.Clear();

                Unit.StatBlock.Stat stat = (Unit.StatBlock.Stat)stats_ListBox.SelectedItem;
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
                    statAttribute1_tableId_TextBox.Text = stat.Attribute1.TableId > 0 ? _excelTables.GetTable(stat.Attribute1.TableId).StringId : "NA";
                }
                else
                {
                    statAttribute1_GroupBox.Enabled = false;
                    statAttribute1_bitCount_TextBox.Clear();
                    statAttribute1_unknown1_1_TextBox.Clear();
                    statAttribute1_unknown1_TextBox.Clear();
                    statAttribute1_tableId_TextBox.Clear();
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
                    statAttribute2_tableId_TextBox.Text = stat.Attribute2.TableId > 0 ? _excelTables.GetTable(stat.Attribute2.TableId).StringId : "NA";
                }
                else
                {
                    statAttribute2_GroupBox.Enabled = false;
                    statAttribute2_bitCount_TextBox.Clear();
                    statAttribute2_unknown1_1_TextBox.Clear();
                    statAttribute2_unknown1_TextBox.Clear();
                    statAttribute2_tableId_TextBox.Clear();
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
                    statAttribute3_tableId_TextBox.Text = stat.Attribute3.TableId > 0 ? _excelTables.GetTable(stat.Attribute3.TableId).StringId : "NA";
                }
                else
                {
                    statAttribute3_GroupBox.Enabled = false;
                    statAttribute3_bitCount_TextBox.Clear();
                    statAttribute3_unknown1_1_TextBox.Clear();
                    statAttribute3_unknown1_TextBox.Clear();
                    statAttribute3_tableId_TextBox.Clear();
                }

                SetStatValues(stat);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "charStats_ListBox_SelectedIndexChanged");
            }
        }

        /* 
         * This is how the table lookup works:
         * if Attribute1 set -> Attribute1 = tableID && values.Attribute1 set -> values.Attribute1 = codeID
         * if Attribute1 not set -> Resource = tableID && values.Attribute1 set -> values.Attribute1 = codeID
         * if Attribute1 not set -> Resource = tableID && values.Attribute1 not set -> values.stat = codeID
         */
        private void SetStatValues(Unit.StatBlock.Stat stat)
        {
            int heightOffset = 0;
            for (int i = 0; i < stat.Length; i++)
            {
                Unit.StatBlock.Stat.Values statValues = stat[i];

                bool hasExtraAttribute = false;
                string lookUpString;
                for (int j = 0; j < 3; j++)
                {
                    if (stat.AttributeAt(j) == null)
                    {
                        break;
                    }

                    Label eaValueLabel = new Label { Text = "Attr" + j + ": ", Width = 40, Top = 3 + heightOffset };
                    TextBox eaMappingTextBox = new TextBox { Left = eaValueLabel.Right, Top = heightOffset, Width = 80 };
                    TextBox eaValueTextBox = new TextBox { Left = eaValueLabel.Right + eaMappingTextBox.Width };
                    eaValueLabel.Top = heightOffset;
                    eaValueTextBox.Width = 80;

                    if (stat.Name.Equals(ItemValueNames.minigame_category_needed.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        // checks for minigame entries by using the values as the minigame doesn't define any tables
                        lookUpString = GetMinigameGoal(stat.values[i].Attribute1, stat.values[i].Attribute2);
                    }
                    else
                    {
                        lookUpString = _itemFunctions.MapIdToString(stat, stat.AttributeAt(j).TableId, stat.values[i].AttributeAt(j));
                    }

                    if (lookUpString != string.Empty)
                    {
                        eaMappingTextBox.Text = lookUpString;
                        eaValueTextBox.Text = stat.values[i].AttributeAt(j).ToString();
                    }
                    else
                    {
                        eaMappingTextBox.DataBindings.Add("Text", statValues, "Attribute" + (j + 1));
                        eaValueTextBox.Text = stat.values[i].AttributeAt(j).ToString();
                    }

                    panel1.Controls.Add(eaValueLabel);
                    panel1.Controls.Add(eaMappingTextBox);
                    panel1.Controls.Add(eaValueTextBox);

                    heightOffset += 25;
                    hasExtraAttribute = true;
                }

                int leftOffset = 0;
                if (hasExtraAttribute)
                {
                    leftOffset += 35;
                }

                Label valueLabel = new Label { Text = "Value: ", Left = leftOffset, Width = 40, Top = 3 + heightOffset };
                TextBox valueTextBox = new TextBox { Left = valueLabel.Right, Top = heightOffset };

                lookUpString = _itemFunctions.MapIdToString(stat, stat.resource, stat.values[i].Stat);
                if (!String.IsNullOrEmpty(lookUpString))
                {
                    valueTextBox.Text = lookUpString;
                }
                else
                {
                    valueTextBox.DataBindings.Add("Text", statValues, "Stat");
                }

                panel1.Controls.Add(valueLabel);
                panel1.Controls.Add(valueTextBox);

                heightOffset += 45;
            }
        }

        private static string GetMinigameGoal(int val1, int val2)
        {
            switch (val2)
            {
                case 1:
                    return "deal physical";
                case 2:
                    return "deal fire";
                case 3:
                    return "deal electric";
                case 4:
                    return "deal spectral";
                case 5:
                    return "deal poison";
                case 10:
                    return "kill necro";
                case 11:
                    return "kill beast";
                case 12:
                    return "kill spectral";
                case 13:
                    return "kill demon";
                case 15:
                    return "find mod";
                case 17:
                    return "find armor";
                case 43:
                    return "find sword";
                case 46:
                    return "find gun";
                default:
                    {
                        if (val2 == 0)
                        {
                            switch (val1)
                            {
                                case 2:
                                    return "deal critical";
                                case 5:
                                    return "find magical";
                            }
                        }

                        return String.Empty;
                    }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<String> references = new List<String>();
            CheckTableReferencesForItems(references, _heroUnit.Items.ToArray());

            listBox1.DataSource = references;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<String> references = new List<String>();
            CheckTableReferencesForItems(references, new[] { _heroUnit });

            listBox1.DataSource = references;
        }

        private void CheckTableReferencesForItems(List<string> references, Unit[] items)
        {
            foreach (Unit item in items)
            {
                foreach (Unit.StatBlock.Stat stats in item.Stats.stats)
                {
                    CheckTableReferencesForItems(references, item.Items.ToArray());

                    string id;
                    if (stats.skipResource == 0)
                    {
                        id = _excelTables.GetTable(stats.resource).StringId;
                        if (!references.Contains(id))
                        {
                            references.Add(id);
                        }
                    }
                    else
                    {
                        foreach (Unit.StatBlock.Stat.Attribute att in stats.attributes)
                        {
                            ExcelTable tab = _excelTables.GetTable(att.TableId);
                            if (tab == null) continue;

                            id = tab.StringId;

                            if (!references.Contains(id))
                            {
                                references.Add(id);
                            }
                        }
                    }
                }
            }
        }

        private void saveCharButton_Click(object sender, EventArgs e)
        {
            String filePath = FileTools.SaveFileDiag("hg1", "HGL Character", _heroUnit.Name, Config.SaveDir);
            if (String.IsNullOrEmpty(filePath)) return;

            _panel.GetSkillControls(_heroUnit);
            UnitHelpFunctions.SaveCharacterFile(_heroUnit, filePath);
        }

        private void InitUnknownStatList()
        {
            string text = string.Empty;
            text += "playerFlagCount1: " + _heroUnit.PlayerFlags1.Count + "\n";
            text += "playerFlagCount2: " + _heroUnit.PlayerFlags2.Count + "\n";
            if (_heroUnit.PlayerFlags1 != null)
            {
                foreach (int val in _heroUnit.PlayerFlags1)
                {
                    text += "playerFlags1: " + val + "\n";
                }
            }
            if (_heroUnit.PlayerFlags2 != null)
            {
                foreach (int val in _heroUnit.PlayerFlags2)
                {
                    text += "PlayerFlags2: " + val + "\n";
                }
            }
            text += "timeStamp1: " + _heroUnit.timeStamp1 + "\n";
            text += "timeStamp2: " + _heroUnit.timeStamp2 + "\n";
            text += "timeStamp3: " + _heroUnit.timeStamp3 + "\n";
            text += "unknown_01_03_1: " + _heroUnit.inventoryType + "\n";
            text += "unknown_01_03_2: " + _heroUnit.inventoryPositionX + "\n";
            text += "unknown_01_03_3: " + _heroUnit.inventoryPositionY + "\n";
            if (_heroUnit.unknown_01_03_4 != null)
            {
                foreach (byte val in _heroUnit.unknown_01_03_4)
                {
                    text += "unknown_01_03_4: " + (int)val;
                }
            }
            text += "unknown_02: " + _heroUnit.unknown_02 + "\n";
            text += "unknown_09: " + _heroUnit.unknown_09 + "\n";
            if (_heroUnit.unitUniqueId != null)
            {
                foreach (byte val in _heroUnit.unitUniqueId)
                {
                    text += "unitUniqueId: " + (int)val + "\n";
                }
            }
            text += "unknownBool_01_03: " + _heroUnit.unknownBool_01_03 + "\n";
            text += "unknownBool_06: " + _heroUnit.unknownBool_06 + "\n";
            text += "unknownBool1: " + _heroUnit.unknownBool1 + "\n";
            text += "unknownCount1F: " + _heroUnit.unknownCount1F + "\n";
            text += "unitType: " + _heroUnit.unitType + "\n";

            text += "\n\n\n\n";

            Unit.UnitAppearance appearance = _heroUnit.Appearance;
            text += "unknown1: " + appearance.unknown1 + "\n";

            if (appearance.unknown2 != null)
            {
                foreach (byte val in appearance.unknown2)
                {
                    text += "unknown2: " + (int)val + "\n";
                }
            }

            richTextBox2.Text = text;
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
            Unit.StatBlock.Stat minigame = UnitHelpFunctions.GetComplexValue(_heroUnit, ItemValueNames.minigame_category_needed.ToString());

            // As long as VS won't let me place the control in the form by hand I'll initialize it here
            MinigameControl control = new MinigameControl(minigame.values);
            p_miniGame.Controls.Add(control);
        }

        private void PopulateWaypoints()
        {
            Unit.StatBlock.Stat wayPoints = UnitHelpFunctions.GetComplexValue(_heroUnit, ItemValueNames.waypoint_flags.ToString());

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
                name_TextBox.Text = heroUnit.Name;

                string job;
                switch (heroUnit.unitCode)
                {
                    case (0x7679):
                        job = "Male Summoner";
                        _characterClass = CharacterClass.Summoner;
                        break;
                    case (0x7579):
                        job = "Female Summoner";
                        _characterClass = CharacterClass.Summoner;
                        break;

                    case (0x7A7A):
                        job = "Male Guardian";
                        _characterClass = CharacterClass.Guardian;
                        break;
                    case (0x797A):
                        job = "Female Guardian";
                        _characterClass = CharacterClass.Guardian;
                        break;

                    case (0x7678):
                        job = "Male Marksman";
                        _characterClass = CharacterClass.Marksman;
                        break;
                    case (0x7578):
                        job = "Female Marksman";
                        _characterClass = CharacterClass.Marksman;
                        break;

                    case (0x7879):
                        job = "Male Evoker";
                        _characterClass = CharacterClass.Evoker;
                        break;
                    case (0x7779):
                        job = "Female Evoker";
                        _characterClass = CharacterClass.Evoker;
                        break;

                    case (0x787A):
                        job = "Male Blademaster";
                        _characterClass = CharacterClass.Blademaster;
                        break;
                    case (0x777A):
                        job = "Female Blademaster";
                        _characterClass = CharacterClass.Blademaster;
                        break;

                    case (0x7878):
                        job = "Male Engineer";
                        _characterClass = CharacterClass.Engineer;
                        break;
                    case (0x7778):
                        job = "Female Engineer";
                        _characterClass = CharacterClass.Engineer;
                        break;

                    default:
                        job = "Unknown";
                        break;
                }
                class_TextBox.Text = job;

                SetStateCheckBoxes();
                SetCharacterValues();
                DisplayFlags();
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
                int level = UnitHelpFunctions.GetSimpleValue(_heroUnit, ItemValueNames.level.ToString());
                level_NumericUpDown.Value = level - 8;


                int palladium = UnitHelpFunctions.GetSimpleValue(_heroUnit, ItemValueNames.gold.ToString());
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

                int statPoints = UnitHelpFunctions.GetSimpleValue(_heroUnit, ItemValueNames.stat_points.ToString());
                if (statPoints < 0)
                {
                    statPoints = 0;
                }
                nud_statPoints.Value = statPoints;

                int skillPoints = UnitHelpFunctions.GetSimpleValue(_heroUnit, ItemValueNames.skill_points.ToString());
                if (skillPoints < 0)
                {
                    skillPoints = 0;
                }
                nud_skillPoints.Value = skillPoints;

                int accuracy = UnitHelpFunctions.GetSimpleValue(_heroUnit, ItemValueNames.accuracy.ToString());
                nud_accuracy.Value = accuracy;

                int strength = UnitHelpFunctions.GetSimpleValue(_heroUnit, ItemValueNames.strength.ToString());
                nud_strength.Value = strength;

                int stamina = UnitHelpFunctions.GetSimpleValue(_heroUnit, ItemValueNames.stamina.ToString());
                nud_stamina.Value = stamina;

                int willpower = UnitHelpFunctions.GetSimpleValue(_heroUnit, ItemValueNames.willpower.ToString());
                nud_willpower.Value = willpower;

                int health = UnitHelpFunctions.GetSimpleValue(_heroUnit, ItemValueNames.hp_cur.ToString());
                nud_health.Value = health;

                int power = UnitHelpFunctions.GetSimpleValue(_heroUnit, ItemValueNames.power_cur.ToString());
                nud_power.Value = power;

                int shields = UnitHelpFunctions.GetSimpleValue(_heroUnit, ItemValueNames.shield_buffer_cur.ToString());
                nud_shields.Value = shields;

                //int armor = UnitHelpFunctions.GetSimpleValue(_heroUnit, ItemValueNames.power_max.ToString());
                //nud_armor.Value = armor;

                int sfxDefence = UnitHelpFunctions.GetSimpleValue(_heroUnit, ItemValueNames.sfx_defense_bonus.ToString());
                nud_sfxDefence.Value = sfxDefence - 100;

                int currentAP = UnitHelpFunctions.GetSimpleValue(_heroUnit, ItemValueNames.achievement_points_total.ToString());
                nud_currentAP.Value = currentAP;

                int maxAP = UnitHelpFunctions.GetSimpleValue(_heroUnit, ItemValueNames.achievement_points_cur.ToString());
                nud_maxAP.Value = maxAP;

                int playTime = UnitHelpFunctions.GetSimpleValue(_heroUnit, ItemValueNames.played_time_in_seconds.ToString());
                TimeSpan t = TimeSpan.FromSeconds(playTime);

                string time = string.Format("{0:D2}d {0:D2}h {1:D2}m {2:D2}s", t.Days, t.Hours, t.Minutes, t.Seconds);
                tb_playedTime.Text = time;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SetCharacterValues");
            }
        }

        // TODO use enums or something - using 21062, 18243 and 18499 is just messy and asking for trouble
        private void SetStateCheckBoxes()
        {
            // elite
            if (_heroUnit.PlayerFlags1.Contains(21062) || _heroUnit.PlayerFlags2.Contains(21062))
            {
                elite_CheckBox.Checked = true;
            }

            // hc
            if (_heroUnit.PlayerFlags1.Contains(18243) || _heroUnit.PlayerFlags2.Contains(18243))
            {
                hardcore_CheckBox.Checked = true;
            }

            // dead
            if (_heroUnit.PlayerFlags1.Contains(18499) || _heroUnit.PlayerFlags2.Contains(18499))
            {
                dead_CheckBox.Checked = true;
            }
        }

        private void dead_CheckBox_Changed(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb == null) return;

            SetCharacterState(18499, cb.Checked);
        }

        private void elite_CheckBox_Changed(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb == null) return;

            SetCharacterState(21062, cb.Checked);
        }

        private void hardcore_CheckBox_Changed(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb == null) return;

            SetCharacterState(18243, cb.Checked);
        }

        private void SetCharacterState(int stateId, bool set)
        {
            if (set)
            {
                if (!_heroUnit.PlayerFlags1.Contains(stateId))
                {
                    _heroUnit.PlayerFlags1.Add(stateId);
                }
                if (!_heroUnit.PlayerFlags2.Contains(stateId))
                {
                    _heroUnit.PlayerFlags2.Add(stateId);
                }
            }
            else
            {
                _heroUnit.PlayerFlags1.Remove(stateId);
                _heroUnit.PlayerFlags2.Remove(stateId);
            }

        }

        private void DisplayFlags()
        {
            richTextBox1.Text = string.Empty;

            richTextBox1.Text += "Flag1:\n";
            richTextBox1.Text += _heroUnit.PlayerFlags1.Count + "\n";
            if (_heroUnit.PlayerFlags1 != null)
            {
                richTextBox1.Text += "Array size: " + _heroUnit.PlayerFlags1.Count + "\n";

                foreach (int flag in _heroUnit.PlayerFlags1)
                {
                    richTextBox1.Text += flag + "\n";
                }
            }
            else
            {
                richTextBox1.Text += "null";
            }

            richTextBox1.Text += "\n\nFlag2:\n";
            richTextBox1.Text += _heroUnit.PlayerFlags2.Count + "\n";
            if (_heroUnit.PlayerFlags2 != null)
            {
                richTextBox1.Text += "Array size: " + _heroUnit.PlayerFlags2.Count + "\n";

                foreach (int flag in _heroUnit.PlayerFlags2)
                {
                    richTextBox1.Text += flag + "\n";
                }
            }
            else
            {
                richTextBox1.Text += "Array = null";
            }

            richTextBox1.Text += "\n\n\n\n";
        }

        private void currentlyEditing_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Unit unit = (Unit)currentlyEditing_ComboBox.SelectedItem;

                ShowInvInfo(unit);

                PopulateStats(unit);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "currentlyEditing_ComboBox_SelectedIndexChanged");
            }
        }

        Unit _currentlySelectedItem;
        private void ShowInvInfo(Unit unit)
        {
            //save currently selected item
            _currentlySelectedItem = unit;

            DataTable items = _dataSet.GetExcelTable(27953);
            DataRow[] itemRow = items.Select("code1 = '" + unit.unitCode + "'");

            if (itemRow.Length > 0)
            {
                int value = (int)itemRow[0]["unitType"];

                DataTable unitTypes = _dataSet.GetExcelTable(21040);
                DataRow[] unitRow = unitTypes.Select("Index = '" + value + "'");

                if (unitRow.Length > 0)
                {
                    tb_itemType.Text = unitRow[0]["type"].ToString();
                }
            }
            else
            {
                tb_itemType.Text = "unknown";
            }

            tb_itemLevel.Text = (UnitHelpFunctions.GetSimpleValue(_currentlySelectedItem, ItemValueNames.level.ToString()) - 8).ToString();
            tb_itemName.Text = unit.Name;
            tb_invLoc.Text = unit.inventoryType.ToString();

            nud_invPosX.Value = unit.inventoryPositionX;
            nud_invPosY.Value = unit.inventoryPositionY;

            tb_itemWidth.Text = GetItemWidth(unit).ToString();
            tb_itemHeight.Text = GetItemHeight(unit).ToString();

            int quantity = UnitHelpFunctions.GetSimpleValue(unit, ItemValueNames.item_quantity.ToString());

            if (quantity <= 0)
            {
                quantity = 1;
            }

            nud_itemQuantity.Value = quantity;

            ShowItemMods(unit.Items.ToArray());
        }

        private void ShowItemMods(Unit[] items)
        {
            cb_availableMods.Items.Clear();

            if (items.Length > 0)
            {
                cb_availableMods.Enabled = true;
                cb_availableMods.Items.AddRange(items);
                cb_availableMods.SelectedIndex = 0;
            }
            else
            {
                cb_availableMods.Enabled = false;
            }
        }

        private void nud_itemCount_ValueChanged(object sender, EventArgs e)
        {
            UnitHelpFunctions.SetSimpleValue(_currentlySelectedItem, ItemValueNames.item_quantity.ToString(), (int)nud_itemQuantity.Value);
        }

        private void nud_invPosX_ValueChanged(object sender, EventArgs e)
        {
            _currentlySelectedItem.inventoryPositionX = (int)nud_invPosX.Value;
        }

        private void nud_invPosY_ValueChanged(object sender, EventArgs e)
        {
            _currentlySelectedItem.inventoryPositionY = (int)nud_invPosY.Value;
        }

        private static int GetItemWidth(Unit item)
        {
            int width = UnitHelpFunctions.GetSimpleValue(item, ItemValueNames.inventory_width.ToString());

            if (width <= 0)
            {
                width = 1;
            }

            return width;
        }

        private static int GetItemHeight(Unit item)
        {
            int height = UnitHelpFunctions.GetSimpleValue(item, ItemValueNames.inventory_height.ToString());

            if (height <= 0)
            {
                height = 1;
            }

            return height;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                String path = String.Format("{0}-singleplayer -load\"{1}\"", Config.GameClientPath, _filePath);
                System.Diagnostics.Process.Start(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to start game at:\n" + Config.GameClientPath + "\n\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DisplayFlags();
        }


        #region modify character values
        //private void SetSimpleValue(Unit unit, string valueName, int value)
        //{
        //    if (!initialized) return;

        //    for (int counter = 0; counter < unit.Stats.Length; counter++)
        //    {
        //        Unit.StatBlock.Stat unitStats = unit.Stats[counter];

        //        if (unitStats.Name != valueName) continue;

        //        unitStats.values[0].Stat = value;
        //        return;
        //    }
        //}

        //private void SetComplexValue(Unit unit, string valueName, Unit.StatBlock.Stat stat)
        //{
        //    if (!initialized) return;

        //    for (int counter = 0; counter < unit.Stats.Length; counter++)
        //    {
        //        Unit.StatBlock.Stat unitStats = unit.Stats[counter];

        //        if (unitStats.Name != valueName) continue;

        //        unitStats = stat;
        //        return;
        //    }
        //}

        //private int GetSimpleValue(Unit unit, string valueName)
        //{
        //    for (int counter = 0; counter < unit.Stats.Length; counter++)
        //    {
        //        Unit.StatBlock.Stat unitStats = unit.Stats[counter];

        //        if (unitStats.Name == valueName)
        //        {
        //            return unitStats.values[0].Stat;
        //        }
        //    }
        //    //MessageBox.Show("Field \"" + valueName + "\" not present in unit " + unit.Name + "!");
        //    return 0;
        //}

        //private Unit.StatBlock.Stat GetComplexValue(Unit unit, string valueName)
        //{
        //    for (int counter = 0; counter < unit.Stats.Length; counter++)
        //    {
        //        Unit.StatBlock.Stat unitStats = unit.Stats[counter];

        //        if (unitStats.Name.Equals(valueName, StringComparison.OrdinalIgnoreCase))
        //        {
        //            return unitStats;
        //        }
        //    }
        //    return null;
        //}

        private void level_NumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            UnitHelpFunctions.SetSimpleValue(_heroUnit, "level", (int)level_NumericUpDown.Value + 8);
        }

        private void palladium_numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            UnitHelpFunctions.SetSimpleValue(_heroUnit, "gold", (int)nud_palladium.Value);
        }

        private void skillPoints_numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            UnitHelpFunctions.SetSimpleValue(_heroUnit, "skill_points", (int)nud_skillPoints.Value);
        }

        private void statPoints_numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            UnitHelpFunctions.SetSimpleValue(_heroUnit, "stat_points", (int)nud_statPoints.Value);
        }

        private void nud_accuracy_ValueChanged(object sender, EventArgs e)
        {
            UnitHelpFunctions.SetSimpleValue(_heroUnit, "accuracy", (int)nud_accuracy.Value);
        }

        private void nud_strength_ValueChanged(object sender, EventArgs e)
        {
            UnitHelpFunctions.SetSimpleValue(_heroUnit, "strength", (int)nud_strength.Value);
        }

        private void nud_stamina_ValueChanged(object sender, EventArgs e)
        {
            UnitHelpFunctions.SetSimpleValue(_heroUnit, "stamina", (int)nud_stamina.Value);
        }

        private void nud_willpower_ValueChanged(object sender, EventArgs e)
        {
            UnitHelpFunctions.SetSimpleValue(_heroUnit, "willpower", (int)nud_willpower.Value);
        }


        private void nud_shields_ValueChanged(object sender, EventArgs e)
        {
            UnitHelpFunctions.SetSimpleValue(_heroUnit, "shield_buffer_cur", (int)nud_shields.Value);
        }

        private void nud_armor_ValueChanged(object sender, EventArgs e)
        {
            //UnitHelpFunctions.SetSimpleValue("power_max", (int)nud_armor.Value);
        }

        private void nud_currentAP_ValueChanged(object sender, EventArgs e)
        {
            UnitHelpFunctions.SetSimpleValue(_heroUnit, "achievement_points_cur", (int)nud_currentAP.Value);
        }

        private void nud_maxAP_ValueChanged(object sender, EventArgs e)
        {
            UnitHelpFunctions.SetSimpleValue(_heroUnit, "achievement_points_total", (int)nud_maxAP.Value);
        }

        private void nud_health_ValueChanged(object sender, EventArgs e)
        {
            UnitHelpFunctions.SetSimpleValue(_heroUnit, "hp_cur", (int)nud_health.Value);
        }

        private void nud_power_ValueChanged(object sender, EventArgs e)
        {
            UnitHelpFunctions.SetSimpleValue(_heroUnit, "power_cur", (int)nud_power.Value);
        }

        private void nud_sfxDefence_ValueChanged(object sender, EventArgs e)
        {
            UnitHelpFunctions.SetSimpleValue(_heroUnit, "sfx_defense_bonus", (int)nud_sfxDefence.Value + 100);
        }

        #endregion

        private void button5_Click(object sender, EventArgs e)
        {
            List<string> itemValues = new List<string>();
            CheckItemValues(itemValues, _heroUnit.Items.ToArray());
            listBox2.DataSource = itemValues;

            CheckItemValues(itemValues, new Unit[] { _heroUnit });

            itemValues.Sort();

            richTextBox3.Text = string.Empty;
            foreach(string text in itemValues)
            {
                richTextBox3.Text += text + "\n";
            }
        }

        private static void CheckItemValues(ICollection<String> values, IEnumerable<Unit> items)
        {
            foreach (Unit item in items)
            {
                foreach (Unit.StatBlock.Stat stat in item.Stats.stats)
                {
                    string val = stat.Name + " = " + stat.Id.ToString() + ",";
                    if (!values.Contains(val))
                    {
                        values.Add(val);
                    }
                }

                CheckItemValues(values, item.Items.ToArray());
            }
        }

        //private void CharValuesToSkillPanel()
        //{
        //    Unit.StatBlock.Stat skillList = null;

        //    foreach (Unit.StatBlock.Stat skills in _heroUnit.Stats.stats)
        //    {
        //        if (skills.Name.Equals("skill_level", StringComparison.CurrentCultureIgnoreCase))
        //        {
        //            skillList = skills;
        //            break;
        //        }
        //    }

        //    foreach (SkillControls skill in _skillControls)
        //    {
        //        foreach (Unit.StatBlock.Stat.Values value in skillList.values)
        //        {
        //            if (value.Attribute1 == skill._id)
        //            {
        //                skill.CurrentLevel = value.Stat;
        //                break;
        //            }
        //        }
        //    }
        //}

        //private void SkillPanelToCharValues()
        //{
        //    Unit.StatBlock.Stat skillList = null;

        //    foreach (Unit.StatBlock.Stat skills in _heroUnit.Stats.stats)
        //    {
        //        if (skills.Name.Equals("skill_level", StringComparison.CurrentCultureIgnoreCase))
        //        {
        //            skillList = skills;
        //            break;
        //        }
        //    }

        //    foreach (SkillControls skill in _skillControls)
        //    {
        //        bool found = false;

        //        foreach (Unit.StatBlock.Stat.Values value in skillList.values)
        //        {
        //            if (value.Attribute1 == skill._id)
        //            {
        //                value.Stat = skill.CurrentLevel;
        //                found = true;
        //                break;
        //            }
        //        }

        //        if (!found && skill.CurrentLevel > 0)
        //        {
        //            List<Unit.StatBlock.Stat.Values> values = new List<Unit.StatBlock.Stat.Values>();
        //            values.AddRange(skillList.values);

        //            Unit.StatBlock.Stat.Values newValue = new Unit.StatBlock.Stat.Values();
        //            newValue.Attribute1 = skill._id;
        //            newValue.Stat = skill.CurrentLevel;

        //            values.Add(newValue);

        //            skillList.values = values.ToArray();
        //            skillList.repeatCount = values.Count;
        //        }
        //    }
        //}

        bool _isMousePressed;
        private void HeroEditor_MouseDown(object sender, MouseEventArgs e)
        {
            if (_isMousePressed) return;

            _isMousePressed = true;
            SuspendLayout();
        }

        private void HeroEditor_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_isMousePressed) return;

            _isMousePressed = false;
            ResumeLayout();
        }

        #region SKILLPANEL
        private void InitializeAttributeSkillPanel(int characterClass)
        {
            DataTable table = _dataSet.GetExcelTable(27952);
            _panel.Initialize(ref table, characterClass, _heroUnit);

            tp_characterValues.Controls.Add(_panel);
            tp_characterValues.Size = _panel.Size;
        }
        #endregion

        private void saveAsData_Click(object sender, EventArgs e)
        {
            Unit unit = currentlyEditing_ComboBox.SelectedItem as Unit;
            if (unit == null) return;

            String savePath = FileTools.SaveFileDiag("dat", "Data", unit.Name, null);
            if (String.IsNullOrEmpty(savePath)) return;

            using (FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.ReadWrite))
            {
                BinaryFormatter bf = new BinaryFormatter { TypeFormat = FormatterTypeStyle.XsdString };
                bf.Serialize(fs, unit);
                fs.Close();
            }

            //XmlUtilities<Unit>.Serialize(unit, savePath + ".xml");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Unit unit = XmlUtilities<Unit>.Deserialize(RESOURCEFOLDER + @"\" + textBox4.Text + ".xml");
            //unit.inventoryPositionX++;
            //unit.inventoryPositionY++;

            String filePath = FileTools.OpenFileDiag("dat", "Data", null);
            if (String.IsNullOrEmpty(filePath)) return;

            Unit unit;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter bf = new BinaryFormatter { TypeFormat = FormatterTypeStyle.XsdString };
                unit = bf.Deserialize(fs) as Unit;
                fs.Close();
            }
            if (unit == null) return;

            _heroUnit.Items.Add(unit);

            MessageBox.Show("Item successfully loaded!");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            _heroUnit.Items.Remove((Unit)currentlyEditing_ComboBox.SelectedItem);
        }

        private void InitInventory()
        {
            try
            {
                const int ITEMSIZE = 56;

                foreach (Unit item in _heroUnit.Items)
                {
                    foreach (Control control in tp_characterInventory.Controls)
                    {
                        if (item.inventoryType.ToString() != (string)control.Tag) continue;

                        int quality = UnitHelpFunctions.GetSimpleValue(item, ItemValueNames.item_quality.ToString());
                        int quantity = UnitHelpFunctions.GetSimpleValue(item, ItemValueNames.item_quantity.ToString());
                        if (quantity <= 0)
                        {
                            quantity = 1;
                        }

                        if (item.inventoryType == 19760 || item.inventoryType == 28208 || item.inventoryType == 26928 || item.inventoryType == 22577)
                        {
                            ((ListBox)control).Items.Add(item);

                            Color color = Color.White;

                            switch (quality)
                            {
                                case (int)ItemQuality.MutantMod:
                                case (int)ItemQuality.Mutant:
                                    color = Color.Purple;
                                    break;
                                case (int)ItemQuality.NormalMod:
                                case (int)ItemQuality.Normal:
                                    color = Color.White;
                                    break;
                                case (int)ItemQuality.UniqueMod:
                                case (int)ItemQuality.Unique:
                                    color = Color.Gold;
                                    break;
                                case (int)ItemQuality.RareMod:
                                case (int)ItemQuality.Rare:
                                    color = Color.Blue;
                                    break;
                                case (int)ItemQuality.Uncommon:
                                    color = Color.Green;
                                    break;
                                case (int)ItemQuality.LegendaryMod:
                                case (int)ItemQuality.Legendary:
                                    color = Color.Orange;
                                    break;
                            }

                            Button b = new Button
                                           {
                                               FlatStyle = FlatStyle.Flat,
                                               BackColor = color,
                                               Width = GetItemWidth(item) * ITEMSIZE,
                                               Height = GetItemHeight(item) * ITEMSIZE,
                                               Location =
                                                   new Point(item.inventoryPositionX * ITEMSIZE,
                                                             item.inventoryPositionY * ITEMSIZE),
                                               Tag = item
                                           };
                            b.Click += b_Click;

                            if (quantity == 1)
                            {
                                b.Text = item.Name;
                            }
                            else
                            {
                                b.Text = quantity + "x " + item.Name;
                            }

                            switch (item.inventoryType)
                            {
                                case (int)InventoryTypes.Inventory:
                                    tp_inventory.Controls.Add(b);
                                    break;
                                case (int)InventoryTypes.Stash:
                                    tp_stash.Controls.Add(b);
                                    break;
                                case (int)InventoryTypes.QuestRewards:
                                    tp_extraStash.Controls.Add(b);
                                    break;
                                case (int)InventoryTypes.Cube:
                                    tp_cubeStash.Controls.Add(b);
                                    break;
                            }

                            break;
                        }
                        else if (item.inventoryType == (int)InventoryTypes.CurrentWeaponSet)
                        {
                            lb_equipped.Items.Add(item);

                            TextBox box = (TextBox)tp_characterInventory.Controls["tb_hand" + item.inventoryPositionX];

                            if (quantity == 1)
                            {
                                box.Text += item.Name;
                            }
                            else
                            {
                                box.Text += quantity + "x " + item.Name;
                            }
                            break;
                        }
                        else
                        {
                            lb_equipped.Items.Add(item);

                            if (quantity == 1)
                            {
                                control.Text += item.Name;
                            }
                            else
                            {
                                control.Text += quantity + "x " + item.Name;
                            }
                            break;
                        }
                    }
                }

                l_inventory.Text += " (" + lb_inventory.Items.Count + ")";
                l_stash.Text += " (" + lb_stash.Items.Count + ")";
                l_questRewards.Text += " (" + lb_questRewards.Items.Count + ")";
                l_cubeStash.Text += " (" + lb_cubeStash.Items.Count + ")";
                l_equipped.Text += " (" + lb_equipped.Items.Count + ")";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "InitInventory");
            }
        }

        void b_Click(object sender, EventArgs e)
        {
            Unit unit = (Unit)((Button)sender).Tag;

            currentlyEditing_ComboBox.SelectedItem = unit;

            ShowInvInfo(unit);
        }

        private void lv_itemSelected_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox view = (ListBox)sender;

            Unit unit = (Unit)view.SelectedItems[0];

            currentlyEditing_ComboBox.SelectedItem = unit;

            ShowInvInfo(unit);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Fixme");
            //Unit unit = XmlUtilities<Unit>.Deserialize(RESOURCEFOLDER + @"\" + textBox4.Text + ".xml");
            //unit.inventoryPositionX++;
            //unit.inventoryPositionY++;
            //_heroUnit = unit;
        }

        private void cb_availableMods_SelectedIndexChanged(object sender, EventArgs e)
        {
            Unit mod = (Unit)cb_availableMods.SelectedItem;
            Unit.StatBlock.Stat affix = mod.Stats.GetStatByName(ItemValueNames.applied_affix.ToString());

            if (affix != null)
            {
                tb_modAttribute.Text = affix.Id.ToString();
                tb_modValue.Text = affix.values[0].Stat.ToString();
            }
        }

        private void b_saveXML_Click(object sender, EventArgs e)
        {
            Unit unit = currentlyEditing_ComboBox.SelectedItem as Unit;
            if (unit == null) return;

            XmlUtilities<Unit>.Serialize(unit, @"F:\" + unit.Name + ".xml");
        }

        private void b_loadXML_Click(object sender, EventArgs e)
        {
            String filePath = FileTools.OpenFileDiag("xml", "XML", null);
            if (String.IsNullOrEmpty(filePath)) return;

            Unit unit = XmlUtilities<Unit>.Deserialize(filePath);

            if (unit != null)
            {
                _heroUnit.Items.Add(unit);
            }

            MessageBox.Show("Item successfully loaded!");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(richTextBox3.Text);
        }
    }
}