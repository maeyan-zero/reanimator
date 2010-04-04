using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using Reanimator.Excel;
using System.Data;
using System.IO;

namespace Reanimator.Forms
{
    public class UnitHelpFunctions
    {
        TableDataSet _dataSet;
        ExcelTables _excelTables;
        Stats _statsTable;

        public UnitHelpFunctions(ref TableDataSet dataSet, ref ExcelTables excelTables)
        {
            _dataSet = dataSet;
            _excelTables = excelTables;
            _statsTable = _excelTables.GetTable("stats") as Stats;
        }

        public void LoadCharacterValues(Unit unit)
        {
            GenerateUnitNameStrings(new Unit[] { unit }, null);

            PopulateItems(unit);
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

                    GenerateUnitNameStrings(unit.Items.ToArray(), hash);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public string MapIdToString(Unit.StatBlock.Stat stat, int tableId, int lookupId)
        {
            string value = string.Empty;

            if (stat.values.Length != 0)
            {
                String select = String.Format("code = '{0}'", lookupId);
                DataTable table = _dataSet.GetExcelTable(tableId);
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

        public static Unit OpenCharacterFile(ref ExcelTables excelTables, string fileName)
        {
            Unit unit = null;

            String excelError = "You must have all excel tables loaded to use the Hero Editor!";
            if (excelTables == null)
            {
                MessageBox.Show(excelError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            if (!excelTables.AllTablesLoaded)
            {
                MessageBox.Show(excelError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            FileStream heroFile;
            try
            {
                heroFile = new FileStream(fileName, FileMode.Open);
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to open file: " + fileName + "\n\n" + e, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            BitBuffer bitBuffer = new BitBuffer(FileTools.StreamToByteArray(heroFile));
            bitBuffer.DataByteOffset = 0x2028;

            unit = new Unit(bitBuffer);
            unit.ReadUnit(ref unit);

            return unit;
        }

        private void PopulateItems(Unit unit)
        {
            bool canGetItemNames = true;
            DataTable itemsTable = _dataSet.GetExcelTable(27953);
            DataTable affixTable = _dataSet.GetExcelTable(30512);
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


            List<Unit> items = unit.Items;
            for (int i = 0; i < items.Count; i++)
            {
                Unit item = items[i];
                if (item == null) continue;


                // assign default name
                item.Name = "Item Id: " + item.itemCode;
                if (!canGetItemNames)
                {
                    continue;
                }


                // get item name
                DataRow[] itemsRows = itemsTable.Select(String.Format("code1 = '{0}'", item.itemCode));
                if (itemsRows.Length == 0)
                {
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
            }
        }
    }

    enum ItemQuality
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
        UniqueMod = 17456
    };

    enum ItemValueNames
    {
        level,
        gold,
        stat_points,
        skill_points,

        skill_right,
        skill_left,
        skill_level,
        skill_shift_enabled,

        holy_radius,
        level_def_start,
        level_def_return,

        save_quest_state_1,
        save_quest_state_2,
        save_quest_state_3,
        save_quest_state_4,
        save_quest_state_5,

        save_quest_status,
        save_quest_version,
        save_quest_data_version,

        save_task_version,
        save_task_count,

        faction_score,
        last_trigger,
        player_visited_level_bitfield,

        badge_reward_received,
        no_tutorial_tips,

        experience,
        experience_prev,
        experience_next,
        achievement_progress,

        quest_global_fix_flags,

        hp_cur,
        power_cur,

        accuracy,
        stamina,
        strength,
        willpower,

        accuracy_feed,
        stamina_feed,
        strength_feed,
        willpower_feed,

        applied_affix,
        attached_affix_hidden,

        armor,
        armor_buffer_max,
        armor_buffer_regen,

        critical_chance,
        critical_mult,

        base_dmg_max,
        base_dmg_min,
        damage_increment,
        damage_increment_field,
        damage_increment_radial,
        damage_max,
        damage_min,

        energy_decrease_source,
        energy_increase_source,
        energy_max,

        firing_error_max,
        firing_error_decrease_source_weapon,
        firing_error_increase_source_weapon,
        firing_error_max_weapon,

        hp_regen,

        identified,

        interrupt_attack,
        item_augmented_count,

        item_difficulty_spawned,
        item_level_limit,
        item_level_req,
        item_lookup_group,
        item_pickedup,

        item_quality,
        item_quantity,
        item_quantity_max,

        item_slots,
        item_spawner_level,
        item_upgraded_count,

        level_req,
        no_trade,

        offer_id,
        offweapon_melee_speed,
        pet_damage_bonus,

        power_cost_pct_skillgroup,
        quest_reward,
        reward_original_location,

        sfx_attack,
        sfx_attack_focus,
        sfx_defense_bonus,
        sfx_duration_in_ticks,
        sfx_strength_pct,

        shield_buffer_cur,
        shield_buffer_max,
        shield_buffer_regen,
        shield_overload_pct,
        shield_penetration_dir,
        shields,

        splash_increment,
        unlimited_in_merchant_inventory,

        inventory_width,
        inventory_height,

        power_max,

        achievement_points_total,
        achievement_points_cur,
        played_time_in_seconds,
        waypoint_flags,
        minigame_category_needed
    };

    enum InventoryTypes
    {
        Inventory = 19760,
        Stash = 28208,
        QuestRewards = 26928,
        Cube = 22577
    };
}
