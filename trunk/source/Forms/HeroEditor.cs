using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Reanimator.Excel;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using System.Collections;
using Reanimator.HeroEditorFunctions;
using System.Drawing;

namespace Reanimator.Forms
{
    public partial class HeroEditor : Form
    {
        readonly Unit _heroUnit;
        readonly TableDataSet dataSet;
        readonly ExcelTables excelTables;
        readonly Stats _statsTable;
        readonly String _filePath;
        string savedPath;

        public HeroEditor(Unit heroUnit, TableDataSet tableDataSet, String filePath)
        {
            _heroUnit = heroUnit;
            dataSet = tableDataSet;
            excelTables = tableDataSet.ExcelTables;
            _statsTable = excelTables.GetTable("stats") as Stats;
            _filePath = filePath;

            _skillPanel = new SkillPanel();

            GenerateUnitNameStrings(new[] { _heroUnit }, null);

            InitializeComponent();
        }

        private void HeroEditor_Load(object sender, EventArgs e)
        {
            currentlyEditing_ComboBox.Items.Add(_heroUnit);
            currentlyEditing_ComboBox.SelectedIndex = 0;

            PopulateGeneral(_heroUnit);

            initialized = true;

            PopulateStats(_heroUnit);
            PopulateItems(_heroUnit);

            PopulateMinigame();
            PopulateWaypoints();

            InitUnknownStatList();

            int charClassId = 1;
            InitSkillPanel(charClassId);
            InitStatPanel();
        }

        private void PopulateItems(Unit unit)
        {
            bool canGetItemNames = true;
            DataTable itemsTable = dataSet.GetExcelTable(27953);
            DataTable affixTable = dataSet.GetExcelTable(30512);
            if (itemsTable != null && affixTable != null)
            {
                if (!itemsTable.Columns.Contains("code1") || !itemsTable.Columns.Contains("String_string"))
                    canGetItemNames = false;
                if (!affixTable.Columns.Contains("code") || !affixTable.Columns.Contains("setNameString_string") ||
                    !affixTable.Columns.Contains("magicNameString_string"))
                    canGetItemNames = false;
            }
            else
            {
                canGetItemNames = false;
            }


            Unit[] items = unit.Items;
            for (int i = 0; i < items.Length; i++)
            {
                Unit item = items[i];
                if (item == null) continue;


                // assign default name
                item.Name = "Item Id: " + item.itemCode;
                if (!canGetItemNames)
                {
                    currentlyEditing_ComboBox.Items.Add(item);
                    continue;
                }


                // get item name
                DataRow[] itemsRows = itemsTable.Select(String.Format("code1 = '{0}'", item.itemCode));
                if (itemsRows.Length == 0)
                {
                    currentlyEditing_ComboBox.Items.Add(item);
                    continue;
                }
                item.Name = itemsRows[0]["String_string"] as String;


                // does it have an affix/prefix
                String affixString = String.Empty;
                for (int s = 0; s < item.Stats.Length; s++)
                {
                    // "applied_affix"
                    if (item.Stats[s].Id == 0x7438)
                    {
                        int affixCode = item.Stats[s].values[0].Stat;
                        DataRow[] affixRows = affixTable.Select(String.Format("code = '{0}'", affixCode));
                        if (affixRows.Length > 0)
                        {
                            String replaceString = affixRows[0]["setNameString_string"] as String;
                            if (String.IsNullOrEmpty(replaceString))
                            {
                                replaceString = affixRows[0]["magicNameString_string"] as String;
                                if (String.IsNullOrEmpty(replaceString))
                                {
                                    break;
                                }
                            }

                            affixString = replaceString;
                        }
                    }

                    // "item_quality"
                    if (item.Stats[s].Id == 0x7832)
                    {
                        // is unique || is mutant then no affix
                        int itemQualityCode = item.Stats[s].values[0].Stat;
                        if (itemQualityCode == 13616 || itemQualityCode == 13360)
                        {
                            affixString = String.Empty;
                            break;
                        }
                    }
                }

                if (affixString.Length > 0)
                {
                    item.Name = affixString.Replace("[item]", item.Name);
                }
                currentlyEditing_ComboBox.Items.Add(item);
            }
        }

        private void PopulateStats(Unit unit)
        {
            try
            {
                stats_ListBox.Items.Clear();
                //GenerateStatNameStrings(unit.Items);

                for (int i = 0; i < unit.Stats.Length; i++)
                {
                    Unit.StatBlock.Stat stat = unit.Stats[i];

                    ////string txt = string.Empty;
                    ////for (int counter = 0; counter < excelTables.Count; counter++)
                    ////{
                    ////    txt += excelTables.GetTableStringId(counter) + "\n";
                    ////}
                    ////MessageBox.Show(txt);

                    //stat.Name = _statsTable.GetStringFromId(stat.Id);
                    ////stat.Name = ((Stats)excelTables.GetTable("stats")).GetStringFromId(stat.Id);

                    stats_ListBox.Items.Add(stat);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "PopulateStats");
            }
        }

        private void GenerateUnitNameStrings(Unit[] units, Hashtable hash)
        {
            if (hash == null)
            {
                hash = new Hashtable();
            }

            try
            {
                Unit.StatBlock.Stat stat;
                foreach (Unit unit in units)
                {
                    for (int counter = 0; counter < unit.Stats.Length; counter++)
                    {
                        stat = unit.Stats[counter];

                        String name;
                        if (hash.Contains(stat.id))
                        {
                            name = (string)hash[stat.Id];
                        }
                        else
                        {
                            name = _statsTable.GetStringFromId(stat.id);

                            if (name != null)
                            {
                                hash.Add(stat.id, name);
                            }
                        }

                        unit.Stats[counter].Name = name;
                    }

                    GenerateUnitNameStrings(unit.Items, hash);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //private string DoStringLookup(Unit.StatBlock.Stat stat, int lookupId)
        //{
        //    string lookUpString = string.Empty;

        //    lookUpString = MapIdToString(stat, lookupId);

        //    return lookUpString;
        //}

        private string MapIdToString(Unit.StatBlock.Stat stat, int tableId, int lookupId)
        {
            string value = string.Empty;

            if (stat.values.Length != 0)
            {
                String select = String.Format("code = '{0}'", lookupId);
                DataTable table = dataSet.GetExcelTable(tableId);
                DataRow[] row;

                if (table != null)
                {
                    row = table.Select(select);

                    if (row != null && row.Length != 0)
                    {
                        value = (string)row[0][1];
                    }
                }
            }

            return value;
        }

        private void charStats_ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.panel1.Controls.Clear();

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
                    if (stat.Attribute1.TableId > 0)
                    {
                        statAttribute1_tableId_TextBox.Text = excelTables.GetTable(stat.Attribute1.TableId).StringId;
                    }
                    else
                    {
                        statAttribute1_tableId_TextBox.Text = "NA";
                    }
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
                    if (stat.Attribute2.TableId > 0)
                    {
                        statAttribute2_tableId_TextBox.Text = excelTables.GetTable(stat.Attribute2.TableId).StringId;
                    }
                    else
                    {
                        statAttribute2_tableId_TextBox.Text = "NA";
                    }
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
                    if (stat.Attribute3.TableId > 0)
                    {
                        statAttribute3_tableId_TextBox.Text = excelTables.GetTable(stat.Attribute3.TableId).StringId;
                    }
                    else
                    {
                        statAttribute3_tableId_TextBox.Text = "NA";
                    }
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
            string lookUpString;
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
                    TextBox eaMappingTextBox = new TextBox();
                    eaMappingTextBox.Left = eaValueLabel.Right;
                    eaMappingTextBox.Top = heightOffset;
                    eaMappingTextBox.Width = 80;

                    TextBox eaValueTextBox = new TextBox();
                    eaValueTextBox.Left = eaValueLabel.Right + eaMappingTextBox.Width;
                    eaValueLabel.Top = heightOffset;
                    eaValueTextBox.Width = 80;

                    if (stat.Name.Equals("minigame_category_needed", StringComparison.InvariantCultureIgnoreCase))
                    {
                        // checks for minigame entries by using the values as the minigame doesn't define any tables
                        lookUpString = GetMinigameGoal(stat.values[i].Attribute1, stat.values[i].Attribute2);
                    }
                    else
                    {
                        lookUpString = MapIdToString(stat, stat.AttributeAt(j).TableId, stat.values[i].AttributeAt(j));
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

                    this.panel1.Controls.Add(eaValueLabel);
                    this.panel1.Controls.Add(eaMappingTextBox);
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

                valueTextBox.Left = valueLabel.Right;
                valueTextBox.Top = heightOffset;

                lookUpString = MapIdToString(stat, stat.resource, stat.values[i].Stat);

                if (lookUpString != string.Empty)
                {
                    valueTextBox.Text = lookUpString;
                }
                else
                {
                    valueTextBox.DataBindings.Add("Text", statValues, "Stat");
                }

                this.panel1.Controls.Add(valueLabel);
                this.panel1.Controls.Add(valueTextBox);

                heightOffset += 45;
            }
        }

        private string GetMinigameGoal(int val1, int val2)
        {
            switch (val2)
            {
                case 1:
                    {
                        return "deal physical";
                    }
                case 2:
                    {
                        return "deal fire";
                    }
                case 3:
                    {
                        return "deal electric";
                    }
                case 4:
                    {
                        return "deal spectral";
                    }
                case 5:
                    {
                        return "deal poison";
                    }

                case 10:
                    {
                        return "kill necro";
                    }
                case 11:
                    {
                        return "kill beast";
                    }
                case 12:
                    {
                        return "kill spectral";
                    }
                case 13:
                    {
                        return "kill demon";
                    }

                case 15:
                    {
                        return "find mod";
                    }
                case 17:
                    {
                        return "find armor";
                    }
                case 43:
                    {
                        return "find sword";
                    }
                case 46:
                    {
                        return "find gun";
                    }
                default:
                    {
                        if (val2 == 0)
                        {
                            if (val1 == 2)
                            {
                                return "deal critical";
                            }
                            else if (val1 == 5)
                            {
                                return "find magical";
                            }
                        }

                        return string.Empty;
                    }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<string> references = new List<string>();
            CheckTableReferencesForItems(references, _heroUnit.Items);

            listBox1.DataSource = references;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<string> references = new List<string>();
            CheckTableReferencesForItems(references, new Unit[] { _heroUnit });

            listBox1.DataSource = references;
        }

        private void CheckTableReferencesForItems(List<string> references, Unit[] items)
        {
            string id;

            foreach (Unit item in items)
            {
                foreach (Unit.StatBlock.Stat stats in item.Stats.stats)
                {
                    CheckTableReferencesForItems(references, item.Items);

                    if (stats.skipResource == 0)
                    {
                        id = excelTables.GetTable(stats.resource).StringId;
                        if (!references.Contains(id))
                        {
                            references.Add(id);
                        }
                    }
                    else
                    {
                        foreach (Unit.StatBlock.Stat.Attribute att in stats.attributes)
                        {
                            ExcelTable tab = excelTables.GetTable(att.TableId);
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

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct MainHeader
        {
            public Int32 Flag;
            public Int32 Version;
            public Int32 DataOffset1;
            public Int32 DataOffset2;
        };

        private void saveCharButton_Click(object sender, EventArgs e)
        {
            SkillPanelToCharValues();

            int startIndex = _filePath.LastIndexOf("\\") + 1;
            string characterName = _filePath.Substring(startIndex, _filePath.Length - startIndex - 4);
            FileStream saveFile = new FileStream(characterName + ".hg1", FileMode.Create, FileAccess.ReadWrite);
            savedPath = saveFile.Name;

            // main header
            MainHeader mainHeader;
            mainHeader.Flag = 0x484D4752; // "RGMH"
            mainHeader.Version = 1;
            mainHeader.DataOffset1 = 0x2028;
            mainHeader.DataOffset2 = 0x2028;
            byte[] data = FileTools.StructureToByteArray(mainHeader);
            saveFile.Write(data, 0, data.Length);

            // hellgate string (is this needed?)
            const string hellgateString = "Hellgate: London";
            byte[] hellgateStringBytes = FileTools.StringToUnicodeByteArray(hellgateString);
            saveFile.Seek(0x28, SeekOrigin.Begin);
            saveFile.Write(hellgateStringBytes, 0, hellgateStringBytes.Length);

            // char name (not actually used in game though I don't think)  (is this needed?)
            string charString = characterName;
            byte[] charStringBytes = FileTools.StringToUnicodeByteArray(charString);
            saveFile.Seek(0x828, SeekOrigin.Begin);
            saveFile.Write(charStringBytes, 0, charStringBytes.Length);

            // no detail string (is this needed?)
            const string noDetailString = "No detail";
            byte[] noDetailStringBytes = FileTools.StringToUnicodeByteArray(noDetailString);
            saveFile.Seek(0x1028, SeekOrigin.Begin);
            saveFile.Write(noDetailStringBytes, 0, noDetailStringBytes.Length);

            // load char string (is this needed?)
            const string loadCharacterString = "Load this Character";
            byte[] loadCharacterStringBytes = FileTools.StringToUnicodeByteArray(loadCharacterString);
            saveFile.Seek(0x1828, SeekOrigin.Begin);
            saveFile.Write(loadCharacterStringBytes, 0, loadCharacterStringBytes.Length);

            // main character data
            saveFile.Seek(0x2028, SeekOrigin.Begin);
            byte[] saveData = _heroUnit.GenerateSaveData(charStringBytes);
            saveFile.Write(saveData, 0, saveData.Length);

            saveFile.Close();
        }

        private void InitUnknownStatList()
        {
            string text = string.Empty;
            text += "jobClass: " + _heroUnit.jobClass + "\n";
            text += "majorVersion: " + _heroUnit.majorVersion + "\n";
            text += "minorVersion: " + _heroUnit.minorVersion + "\n";
            text += "playerFlagCount1: " + _heroUnit.playerFlagCount1 + "\n";
            text += "playerFlagCount2: " + _heroUnit.playerFlagCount2 + "\n";
            if (_heroUnit.playerFlags1 != null)
            {
                foreach (int val in _heroUnit.playerFlags1)
                {
                    text += "playerFlags1: " + val + "\n";
                }
            }
            if (_heroUnit.playerFlags2 != null)
            {
                foreach (int val in _heroUnit.playerFlags2)
                {
                    text += "playerFlags2: " + val + "\n";
                }
            }
            text += "timeStamp1: " + _heroUnit.timeStamp1 + "\n";
            text += "timeStamp2: " + _heroUnit.timeStamp2 + "\n";
            text += "timeStamp3: " + _heroUnit.timeStamp3 + "\n";
            text += "unknown_01_03_1: " + _heroUnit.unknown_01_03_1 + "\n";
            text += "unknown_01_03_2: " + _heroUnit.unknown_01_03_2 + "\n";
            text += "unknown_01_03_3: " + _heroUnit.unknown_01_03_3 + "\n";
            if (_heroUnit.unknown_01_03_4 != null)
            {
                foreach (byte val in _heroUnit.unknown_01_03_4)
                {
                    text += "unknown_01_03_4: " + (int)val;
                }
            }
            text += "unknown_02: " + _heroUnit.unknown_02 + "\n";
            text += "unknown_07: " + _heroUnit.unknown_07 + "\n";
            text += "unknown_09: " + _heroUnit.unknown_09 + "\n";
            if (_heroUnit.unknown17 != null)
            {
                foreach (byte val in _heroUnit.unknown17)
                {
                    text += "unknown17: " + (int)val + "\n";
                }
            }
            text += "unknownBool_01_03: " + _heroUnit.unknownBool_01_03 + "\n";
            text += "unknownBool_06: " + _heroUnit.unknownBool_06 + "\n";
            text += "unknownBool1: " + _heroUnit.unknownBool1 + "\n";
            text += "unknownCount1B: " + _heroUnit.unknownCount1B + "\n";
            text += "unknownCount1F: " + _heroUnit.unknownCount1F + "\n";
            text += "unknownFlag: " + _heroUnit.unknownFlag + "\n";
            text += "weaponConfigCount: " + _heroUnit.weaponConfigCount + "\n";
            text += "weaponConfigFlag: " + _heroUnit.weaponConfigFlag + "\n";

            text += "\n\n\n\n";

            UnitAppearance appearance = _heroUnit.unitAppearance;
            text += "unknown1: " + appearance.unknown1 + "\n";

            if (appearance.unknown2 != null)
            {
                foreach (byte val in appearance.unknown2)
                {
                    text += "unknown2: " + (int)val + "\n";
                }
            }

            if (appearance.unknownCount1s != null)
            {
                foreach (UnitAppearance.UnknownCount1_S count in appearance.unknownCount1s)
                {
                    text += "unknown1: " + count.unknown1 + "\n";
                    text += "unknown2: " + count.unknown2 + "\n";

                    if (count.unknownCount1s != null)
                    {
                        foreach (int val in count.unknownCount1s)
                        {
                            text += "unknownCount1s: " + val + "\n";
                        }
                    }
                }
            }

            if (appearance.unknownCount2s != null)
            {
                foreach (int val in appearance.unknownCount2s)
                {
                    text += "unknownCount2s: " + val + "\n";
                }
            }

            if (appearance.unknownCount3s != null)
            {
                foreach (int val in appearance.unknownCount3s)
                {
                    text += "unknownCount3s: " + val + "\n";
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
            if (_heroUnit.Flags1 != null && _heroUnit.Flags1.Contains(21062))
            {
                elite_CheckBox.Checked = true;
            }
            if (_heroUnit.Flags2 != null && _heroUnit.Flags2.Contains(18243))
            {
                hardcore_CheckBox.Checked = true;
            }
            if (_heroUnit.Flags2 != null && _heroUnit.Flags2.Contains(18499))
            {
                dead_CheckBox.Checked = true;
            }
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (initialized)
            {
                _heroUnit.SetGameMode(elite_CheckBox.Checked, hardcore_CheckBox.Checked, dead_CheckBox.Checked);
            }
        }

        private void DisplayFlags()
        {
            richTextBox1.Text = string.Empty;

            richTextBox1.Text += "Flag1:\n";
            richTextBox1.Text += _heroUnit.PlayerFlagCount1 + "\n";
            if (_heroUnit.Flags1 != null)
            {
                richTextBox1.Text += "Array size: " + _heroUnit.Flags1.Length + "\n";

                foreach (int flag in _heroUnit.Flags1)
                {
                    richTextBox1.Text += flag.ToString() + "\n";
                }
            }
            else
            {
                richTextBox1.Text += "null";
            }

            richTextBox1.Text += "\n\nFlag2:\n";
            richTextBox1.Text += _heroUnit.PlayerFlagCount2 + "\n";
            if (_heroUnit.Flags2 != null)
            {
                richTextBox1.Text += "Array size: " + _heroUnit.Flags2.Length + "\n";

                foreach (int flag in _heroUnit.Flags2)
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
                System.Diagnostics.Process.Start(Config.GameClientPath, "-singleplayer -load\"" + savedPath + "\"");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to start game at:\n" + Config.GameClientPath + "\n\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DisplayFlags();
        }


        #region modify character values
        private void SetSimpleValue(string valueName, int value)
        {
            if (!initialized) return;

            for (int counter = 0; counter < _heroUnit.Stats.Length; counter++)
            {
                Unit.StatBlock.Stat unit = _heroUnit.Stats[counter];

                if (unit.Name != valueName) continue;

                unit.values[0].Stat = value;
                return;
            }
        }

        private void SetComplexValue(string valueName, Unit.StatBlock.Stat stat)
        {
            if (!initialized) return;

            for (int counter = 0; counter < _heroUnit.Stats.Length; counter++)
            {
                Unit.StatBlock.Stat unit = _heroUnit.Stats[counter];

                if (unit.Name != valueName) continue;

                unit = stat;
                return;
            }
        }

        private int GetSimpleValue(string valueName)
        {
            for (int counter = 0; counter < _heroUnit.Stats.Length; counter++)
            {
                Unit.StatBlock.Stat unit = _heroUnit.Stats[counter];

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
            for (int counter = 0; counter < _heroUnit.Stats.Length; counter++)
            {
                Unit.StatBlock.Stat unit = _heroUnit.Stats[counter];

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

        private void button5_Click(object sender, EventArgs e)
        {
            List<string> itemValues = new List<string>();
            CheckItemValues(itemValues, _heroUnit.Items);
            listBox2.DataSource = itemValues;
        }

        private void CheckItemValues(List<string> values, Unit[] items)
        {
            foreach (Unit item in items)
            {
                foreach (Unit.StatBlock.Stat stat in item.Stats.stats)
                {
                    string val = stat.ToString();
                    if (!values.Contains(val))
                    {
                        values.Add(val);
                    }
                }

                CheckItemValues(values, item.Items);
            }
        }

        SkillPanel _skillPanel;
        private void InitStatPanel()
        {
            tp_stats.SuspendLayout();

            DataTable table = dataSet.GetExcelTable(23088);
            StatComponents comp = new StatComponents(_skillPanel);

            Panel panel = comp.CreatePanel(_heroUnit, table);

            panel.Scale(new SizeF(.7f, .7f));

            tp_stats.Controls.Add(panel);

            tp_stats.ResumeLayout();
        }

        List<SkillControls> _skillControls;
        public void InitSkillPanel(int characterClass)
        {
            tp_skills.SuspendLayout();

            DataTable table = dataSet.GetExcelTable(27952);
            SkillComponents comp = new SkillComponents(_skillPanel);

            tp_skills.Controls.Clear();
            Panel panel = comp.CreatePanel(ref table, characterClass);

            panel.Scale(new SizeF(.7f, .7f));

            tp_skills.Controls.Add(panel);

            _skillControls = comp.SkillControls;

            CharValuesToSkillPanel();

            tp_skills.ResumeLayout();
        }

        private void CharValuesToSkillPanel()
        {
            Unit.StatBlock.Stat skillList = null;

            foreach (Unit.StatBlock.Stat skills in _heroUnit.Stats.stats)
            {
                if (skills.Name.Equals("skill_level", StringComparison.CurrentCultureIgnoreCase))
                {
                    skillList = skills;
                    break;
                }
            }
            
            foreach (SkillControls skill in _skillControls)
            {
                foreach (Unit.StatBlock.Stat.Values value in skillList.values)
                {
                    if (value.Attribute1 == skill._id)
                    {
                        skill.CurrentLevel = value.Stat;
                        break;
                    }
                }
            }
        }

        private void SkillPanelToCharValues()
        {
            Unit.StatBlock.Stat skillList = null;

            foreach (Unit.StatBlock.Stat skills in _heroUnit.Stats.stats)
            {
                if (skills.Name.Equals("skill_level", StringComparison.CurrentCultureIgnoreCase))
                {
                    skillList = skills;
                    break;
                }
            }

            foreach (SkillControls skill in _skillControls)
            {
                bool found = false;

                foreach (Unit.StatBlock.Stat.Values value in skillList.values)
                {
                    if (value.Attribute1 == skill._id)
                    {
                        value.Stat = skill.CurrentLevel;
                        found = true;
                        break;
                    }
                }

                if (!found && skill.CurrentLevel > 0)
                {
                    List<Unit.StatBlock.Stat.Values> values = new List<Unit.StatBlock.Stat.Values>();
                    values.AddRange(skillList.values);

                    Unit.StatBlock.Stat.Values newValue = new Unit.StatBlock.Stat.Values();
                    newValue.Attribute1 = skill._id;
                    newValue.Stat = skill.CurrentLevel;

                    values.Add(newValue);

                    skillList.values = values.ToArray();
                    skillList.repeatCount = values.Count;
                }
            }
        }

        bool isMousePressed;
        private void HeroEditor_MouseDown(object sender, MouseEventArgs e)
        {
            if(!isMousePressed)
            {
                isMousePressed = true;
                this.SuspendLayout();
            }
        }

        private void HeroEditor_MouseUp(object sender, MouseEventArgs e)
        {
            if (isMousePressed)
            {
                isMousePressed = false;
                this.ResumeLayout();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitSkillPanel(int.Parse(comboBox1.Text));
        }
    }
}