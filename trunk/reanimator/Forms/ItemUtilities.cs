using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using Hellgate;
using Revival.Common;

namespace Reanimator.Forms
{
    public enum SpecialItems
    {
        Drone = 29797
    }

    public class UnitHelpFunctions
    {
        readonly FileManager _fileManager;
        private readonly DataTable _statsTable;

       public UnitHelpFunctions(FileManager fileManager)
        {
            _fileManager = fileManager;
           _statsTable = fileManager.GetDataTable("STATS");
        }

        public void LoadCharacterValues(UnitObject unit)
        {
            //GenerateUnitNameStrings(new[] { unit }, null);

            PopulateItems(unit);
        }

        // not needed anymore (name is obtained automatically)
        //private void GenerateUnitNameStrings(UnitObject[] units, Hashtable hash)
        //{
        //    if (hash == null) hash = new Hashtable();

        //    try
        //    {
        //        UnitObjectStats.Stat stat;
        //        foreach (UnitObject unit in units)
        //        {
        //            for (int counter = 0; counter < unit.Stats.Stats.Count; counter++)
        //            {
        //                stat = unit.Stats.Stats[counter];

        //                String name;
        //                if (hash.Contains(stat.Code))
        //                {
        //                    name = (string)hash[stat.Code];
        //                }
        //                else
        //                {
        //                    DataRow[] statRows = statsTable.Select("code = " + stat.Code);
        //                    name = (string)statRows[0]["stat"];

        //                    if (name != null)
        //                    {
        //                        hash.Add(stat.Code, name);
        //                    }
        //                }

        //                unit.Stats[counter].Name = name;
        //            }

        //            GenerateUnitNameStrings(unit.Items.ToArray(), hash);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "GenerateUnitNameStrings");
        //    }
        //}

        public string MapIdToString(UnitObjectStats.Stat stat, Xls.TableCodes tableId, int lookupId)
        {
            string value = string.Empty;

            if (stat.Values.Count != 0)
            {
                String select = String.Format("code = '{0}'", lookupId);
                DataTable table = null;// todo: rewrite  _dataSet.GetExcelTableFromCode((uint)tableId);
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

        public static CharacterFile OpenCharacterFile(string fileName)
        {
            CharacterFile characterFile = new CharacterFile(fileName);

            try
            {
                byte[] fileBytes = File.ReadAllBytes(fileName);
                characterFile.ParseFileBytes(fileBytes);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open file: " + fileName + "\n\n" + ex, "OpenCharacterFile", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return characterFile;
        }

        //public static Unit OpenCharacterFile(TableFiles tableFiles, string fileName)
        //// todo: rewrite public static Unit OpenCharacterFile(TableDataSet tableDataSet, string fileName)
        //{
        //    Unit unit = null;

        //    const string excelError = "You must have all excel tables loaded to load characters!";
        //    if (tableDataSet == null)
        //    {
        //        MessageBox.Show(excelError, "OpenCharacterFile", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return null;
        //    }
        //    //if (!tableFiles.AllExcelFilesLoaded || !tableFiles.AllStringsFilesLoaded)
        //    //{
        //    //    MessageBox.Show(excelError, "OpenCharacterFile", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    //    return null;
        //    //}

        //    FileStream heroFile;
        //    try
        //    {
        //        heroFile = new FileStream(fileName, FileMode.Open);
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show("Failed to open file: " + fileName + "\n\n" + e, "OpenCharacterFile", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return null;
        //    }

        //    BitBuffer bitBuffer = new BitBuffer(FileTools.StreamToByteArray(heroFile)) {DataByteOffset = 0x2028};

        //    unit = new Unit(bitBuffer);
        //    unit.ParseUnit();

        //    heroFile.Close();

        //    return unit;
        //}

        private void PopulateItems(UnitObject unitObject)
        {
            try
            {
                bool canGetItemNames = true;
                DataTable itemsTable = _fileManager.GetDataTable("ITEMS");
                DataTable affixTable = _fileManager.GetDataTable("AFFIXES");
                DataTable affixNames = _fileManager.GetDataTable("Strings_Affix");
                if (itemsTable != null && affixTable != null)
                {
                    if (!itemsTable.Columns.Contains("code") || !itemsTable.Columns.Contains("String_string"))
                        canGetItemNames = false;
                    if (!affixTable.Columns.Contains("code") || !affixTable.Columns.Contains("setNameString_string") ||
                        !affixTable.Columns.Contains("magicNameString_string"))
                        canGetItemNames = false;
                }
                else
                {
                    canGetItemNames = false;
                }


                List<UnitObject> items = unitObject.Items;
                for (int i = 0; i < items.Count; i++)
                {
                    UnitObject item = items[i];
                    if (item == null) continue;

                    //It's an engineer and his drone
                    if (item.UnitCode == (int)SpecialItems.Drone)
                    {
                        item.Name = "Drone";
                        continue;
                    }
                    // assign default name
                    item.Name = "Item Id: " + item.UnitCode;
                    if (!canGetItemNames)
                    {
                        continue;
                    }
                    if (item.UnitCode == 25393)
                    {
                        //string a;
                    }

                    // get item name
                    DataRow[] itemsRows = itemsTable.Select(String.Format("code = '{0}'", item.UnitCode));
                    if (itemsRows.Length == 0)
                    {
                        continue;
                    }
                    item.Name = itemsRows[0]["String_string"] as String;


                    // does it have an affix/prefix
                    String affixString = String.Empty;
                    foreach (UnitObjectStats.Stat stat in item.Stats.Stats.Values)
                    {
                        
                    //for (int s = 0; s < item.Stats.NameCount; s++)
                    //{
                        // "applied_affix"
                        if (stat.Code == (int)ItemValueNames.applied_affix)
                        {
                            int affixCode = stat.Values[0].Value;
                            DataRow[] affixRows = affixTable.Select(String.Format("code = '{0}'", affixCode));
                            if (affixRows.Length > 0)
                            {
                                //String replaceString = affixRows[0]["setNameString_String"] as String;
                                int index = (int)affixRows[0]["setNameString"];
                                if (index < 0)
                                {
                                    index = (int)affixRows[0]["magicNameString"];

                                    //replaceString = affixRows[0]["magicNameString"] as String;
                                    if (index < 0)
                                    {
                                        break;
                                    }
                                }

                                DataRow[] stringRows = affixNames.Select(String.Format("ReferenceId = '{0}'", index));
                                String replaceString = stringRows[0]["String"] as String;

                                affixString = replaceString;
                            }
                        }

                        // "item_quality"
                        if (stat.Code == (int)ItemValueNames.item_quality)
                        {
                            // is unique || is mutant then no affix
                            int itemQualityCode = stat.Values[0].Value;
                            if (itemQualityCode == (int)ItemQuality.Unique || itemQualityCode == (int)ItemQuality.Mutant)
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

                    if (item.Items.Count > 0)
                    {
                        PopulateItems(item);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, true);
            }
        }

        public static bool SetSimpleValue(UnitObject unit, string valueName, int value)
        {
            //if (!initialized) return;

            foreach (UnitObjectStats.Stat unitStats in unit.Stats.Stats.Values)
            {
                if (unitStats.Name != valueName) continue;

                unitStats.Values[0].Value = value;
                return true;
            }

            //for (int counter = 0; counter < unit.Stats.Stats.Count; counter++)
            //{
            //    UnitObjectStats.Stat unitStats = unit.Stats[counter];

            //    if (unitStats.Name != valueName) continue;

            //    unitStats.Values[0].Value = value;
            //    return true;
            //}

            return false;
        }

        public static bool SetSimpleValue(UnitObject unit, int valueId, int value)
        {
            //if (!initialized) return;

            foreach (UnitObjectStats.Stat unitStats in unit.Stats.Stats.Values)
            {
                if (unitStats.Code != valueId) continue;

                unitStats.Values[0].Value = value;
                return true;
            }

            //for (int counter = 0; counter < unit.Stats.Stats.Count; counter++)
            //{
            //    UnitObjectStats.Stat unitStats = unit.Stats[counter];

            //    if (unitStats.Code != valueId) continue;

            //    unitStats.Values[0].Value = value;
            //    return true;
            //}

            return false;
        }

        public static bool SetComplexValue(UnitObject unit, string valueName, UnitObjectStats.Stat stat)
        {
            //if (!initialized) return;

            foreach (UnitObjectStats.Stat unitStats in unit.Stats.Stats.Values)
            {
                if (unitStats.Name != valueName) continue;

                return true;
            }

            //for (int counter = 0; counter < unit.Stats.Stats.Count; counter++)
            //{
            //    UnitObjectStats.Stat unitStats = unit.Stats[counter];

            //    if (unitStats.Name != valueName) continue;

            //    unitStats = stat;
            //    return true;
            //}

            return false;
        }

        public static int GetSimpleValue(UnitObject unit, string valueName)
        {
            foreach (UnitObjectStats.Stat unitStats in unit.Stats.Stats.Values)
            {
                if (unitStats.Name == valueName)
                {
                    return unitStats.Values[0].Value;
                }
            }

            //for (int counter = 0; counter < unit.Stats.Stats.Count; counter++)
            //{
            //    UnitObjectStats.Stat unitStats = unit.Stats[counter];

            //    if (unitStats.Name == valueName)
            //    {
            //        return unitStats.Values[0].Value;
            //    }
            //}
            //MessageBox.Show("Field \"" + valueName + "\" not present in unit " + unit.Name + "!");

            return 0;
        }

        public static int GetSimpleValue(UnitObject unit, int valueId)
        {
            foreach (UnitObjectStats.Stat unitStats in unit.Stats.Stats.Values)
            {
                if (unitStats.Code != valueId) continue;

                UnitObjectStats.Stat.StatValue entry = unitStats.Values[0];

                // if all atributes are 0 the value is most likely a simple value
                if (entry.Param1 == 0 && entry.Param2 == 0 && entry.Param3 == 0)
                {
                    return entry.Value;
                }

                ExceptionLogger.LogException(new Exception("IsComplexAttributeException"), false, unitStats.Code + " is of type ComplexValue");
            }

            //for (int counter = 0; counter < unit.Stats.Stats.Count; counter++)
            //{
            //    UnitObjectStats.Stat unitStats = unit.Stats[counter];

            //    if (unitStats.Code == valueId)
            //    {
            //        UnitObjectStats.Stat.StatValue entry = unitStats.Values[0];

            //        // if all atributes are 0 the value is most likely a simple value
            //        if (entry.Param1 == 0 && entry.Param2 == 0 && entry.Param3 == 0)
            //        {
            //            return entry.Value;
            //        }

            //        ExceptionLogger.LogException(new Exception("IsComplexAttributeException"), "GetSimpleValue", unitStats.Code + " is of type ComplexValue", false);
            //    }
            //}
            //MessageBox.Show("Field \"" + valueName + "\" not present in unit " + unit.Name + "!");

            return 0;
        }

        public static UnitObjectStats.Stat GetComplexValue(UnitObject unit, string valueName)
        {
            foreach (UnitObjectStats.Stat unitStats in unit.Stats.Stats.Values)
            {
                if (unitStats.Name.Equals(valueName, StringComparison.OrdinalIgnoreCase))
                {
                    return unitStats;
                }
            }

            //for (int counter = 0; counter < unit.Stats.Stats.Count; counter++)
            //{
            //    UnitObjectStats.Stat unitStats = unit.Stats[counter];

            //    if (unitStats.Name.Equals(valueName, StringComparison.OrdinalIgnoreCase))
            //    {
            //        return unitStats;
            //    }
            //}

            return null;
        }

        public static UnitObjectStats.Stat GetComplexValue(UnitObject unit, ItemValueNames valueName)
        {
            foreach (UnitObjectStats.Stat unitStats in unit.Stats.Stats.Values)
            {
                if (unitStats.Code == (int)valueName)
                {
                    return unitStats;
                }
            }
            //for (int counter = 0; counter < unit.Stats.Stats.Count; counter++)
            //{
            //    UnitObjectStats.Stat unitStats = unit.Stats[counter];

            //    if (unitStats.Code == (int)valueName)
            //    {
            //        return unitStats;
            //    }
            //}
            return null;
        }

        /// <summary>
        /// Adds a new simple value (entry that holds only one value) to the stats table
        /// </summary>
        /// <param name="unit">The unit to add this stat to</param>
        /// <param name="valueName">The name/id of the value to add</param>
        /// <param name="value">The actual value to add</param>
        /// <param name="bitCount">The bitCount of this value (possibly defines the maximum value of the "value" entry)</param>
        public static void AddSimpleValue(UnitObject unit, ItemValueNames valueName, int value, int bitCount)
        {
            List<UnitObjectStats.Stat> newStats = new List<UnitObjectStats.Stat>();
            //copies the existing values to a new array
            newStats.AddRange(unit.Stats.Stats.Values);

            //check if the value already exists
            if (newStats.Find(tmp => tmp.Code == (int)valueName) != null)
            {
                return;
            }

            //generates a new stat
            UnitObjectStats.Stat newStat = new UnitObjectStats.Stat();
            //generates the entry that holds the stat value
            UnitObjectStats.Stat.StatValue newValue = new UnitObjectStats.Stat.StatValue();
            //newStat.Values = new List<UnitObjectStats.Stat.StatValue>();
            //adds the entry to the new stat
            newStat.Values.Add(newValue);
            //sets the bitCOunt value (maximum stat value defined by the number of bits?)
            //newStat.BitCount = bitCount;
            //sets the length of the stat array (may be unnecessary)
            //newStat.Length = 1;
            //sets the Id of the new stat
            //newStat.Code = (short)valueName;
            //newStat.SkipResource = 1;
            //newStat.repeatCount = 1;

            //adds the new value to the array
            newStats.Add(newStat);

            //assigns the new array to the unit
            unit.Stats.Stats.TryAdd(newStat.Code, newStat);// = newStats.ToArray();
            //unit.Stats.statCount = newStats.Count;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct MainHeader
        {
            public Int32 Flag;
            public Int32 Version;
            public Int32 DataOffset1;
            public Int32 DataOffset2;
        };

        public static void SaveCharacterFile(UnitObject unit, string filePath)
        {
            DialogResult dr = DialogResult.Retry;
            while (dr == DialogResult.Retry)
            {
                try
                {
                    using (FileStream saveFile = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                    {
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
                        string charString = unit.Name;
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
                        byte[] saveData = null;// todo: update me unit.GenerateSaveData(charStringBytes);
                        saveFile.Write(saveData, 0, saveData.Length);
                    }

                    MessageBox.Show("Character saved successfully!", "Saved", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    break;
                }
                catch (Exception e)
                {
                    dr = MessageBox.Show("Failed to save character file!Try again?\n\n" + e, "Error",
                                         MessageBoxButtons.RetryCancel);
                }
            }
        }
    }



    public enum ItemQuality
    {
        Normal = 12336,
        NormalMod = 14384,

        Uncommon = 12592,

        Rare = 18480,
        RareMod = 18736,

        Legendary = 13104,
        LegendaryMod = 16944,

        Mutant = 13360,
        MutantMod = 17200,

        Unique = 13616,
        UniqueMod = 17456,

        DoubleEdged = 00000,
        DoubleEdgedMod = 00001,

        Mythic = 00002,
        MythicMod = 00003
    };

    public enum ItemValueNames
    {
        accuracy = 18224,
        accuracy_feed = 16690,
        achievement_points_cur = 24900,
        achievement_points_total = 23108,
        achievement_progress = 22852,
        ai_change_attack = 20793,
        ai_change_duration = 24889,
        ai_last_attacker_id = 27186,
        applied_affix = 29752,
        armor = 18737,
        armor_buffer_cur = 19761,
        armor_buffer_max = 19505,
        armor_buffer_regen = 20273,
        attached_affix_hidden = 19512,
        attack_rating = 25392,
        badge_reward_received = 23107,
        base_dmg_max = 25904,
        base_dmg_min = 25648,
        critical_chance = 12849,
        critical_mult = 14385,
        cube_known_recipes = 12869,
        damage_increment = 27696,
        damage_increment_field = 17473,
        damage_increment_radial = 17729,
        damage_max = 27440,
        damage_min = 27184,
        difficulty_max = 25667,
        energy_decrease_source = 27970,
        energy_increase_source = 29250,
        energy_max = 28226,
        experience = 13874,
        experience_next = 14386,
        experience_prev = 14130,
        faction_score = 13622,
        firing_error_decrease_source = 12610,
        firing_error_decrease_source_weapon = 12866,
        firing_error_increase_source = 12354,
        firing_error_increase_source_weapon = 30529,
        firing_error_max = 31297,
        firing_error_max_weapon = 31041,
        gold = 13618,
        hp_cur = 13360,
        hp_max = 13616,
        hp_regen = 14384,
        identified = 16951,
        interrupt_attack = 17713,
        inventory_height = 12595,
        inventory_width = 12339,
        item_augmented_count = 21315,
        item_difficulty_spawned = 17989,
        item_level_limit = 13123,
        item_level_req = 28739,
        item_look_group = 25401,
        item_pickedup = 12357,
        item_quality = 30770,
        item_quantity = 21300,
        item_quantity_max = 21044,
        item_slots = 26163,
        item_spawner_level = 28726,
        item_upgraded_count = 21059,
        last_trigger = 28982,
        level = 12336,
        level_def_return = 27700,
        level_def_start = 27444,
        level_req = 12848,
        minigame_category_needed = 26180,
        newest_headstone_unit_id = 27457,
        no_trade = 24898,
        no_tutorial_tips = 20548,
        offerid = 27703,
        offweapon_melee_speed = 29236,
        pet_damage_bonus = 22081,
        played_time_in_seconds = 26690,
        player_visited_level_bitfield = 21061,
        power_cost_pct_skillgroup = 17716,
        power_cur = 16944,
        power_max = 17200,
        power_regen = 17456,
        previous_weaponconfig = 17718,
        quest_global_fix_flags = 31044,
        quest_player_tracking = 24899,
        quest_reward = 14657,
        reward_original_location = 31286,
        save_quest_data_version = 22596,
        save_quest_hunt_version = 29497,
        save_quest_state_1 = 30260,
        save_quest_state_2 = 30516,
        save_quest_state_3 = 30772,
        save_quest_state_4 = 31028,
        save_quest_state_5 = 31284,
        save_quest_state_6 = 12341,
        save_quest_state_7 = 12597,
        save_quest_state_8 = 12853,
        save_quest_state_9 = 13109,
        save_quest_status = 17973,
        save_quest_version = 18229,
        save_task_count = 18741,
        save_task_version = 18485,
        sfx_attack = 28977,
        sfx_attack_focus = 29251,
        sfx_defense_bonus = 28721,
        sfx_duration_in_ticks = 30769,
        sfx_strength_pct = 12338,
        shield_buffer_cur = 25137,
        shield_buffer_max = 24881,
        shield_buffer_regen = 25649,
        shield_overload_pct = 26417,
        shields = 23089,
        skill_is_selected = 17204,
        skill_level = 21043,
        skill_points = 30004,
        skill_right = 20787,
        skill_shift_enabled = 28225,
        splash_increment = 30515,
        stamina = 19248,
        stamina_feed = 17202,
        stat_points = 14642,
        strength = 21808,
        strength_feed = 20274,
        unlimited_in_merchant_inventory = 20536,
        waypoint_flags = 20532,
        willpower = 19760,
        willpower_feed = 17458
    };

    public enum InventoryTypes
    {
        Inventory = 19760,
        Stash = 28208,
        //SharedStash = 26160,
        QuestRewards = 26928,
        Cube = 22577,
        CurrentWeaponSet = 25904,
        Turret = 17457
    };

    public enum InventoryTypesComplete
    {
        Inventory = 19760,
        Stash = 28208,
        SharedStash = 26160,
        QuestRewards = 26928,
        Cube = 22577,
        CurrentWeaponSet = 25904,
        Turret = 17457,

        Belt = 17200,
        Helmet = 12592,
        Shoulders = 13360,
        Gloves = 16944,
        Armor = 13616,
        Pants = 17712,
        Boots = 17456,
        HandLeft = 14128,
        HandRight = 14384,
        DyeKit = 19249,

        Ring,
        Amulet,
        Goggles
    };
}
