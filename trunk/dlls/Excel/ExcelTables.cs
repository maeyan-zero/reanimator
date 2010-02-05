using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace Reanimator.Excel
{
    public class ExcelTables : ExcelTable
    {
        [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
        public class ExcelOutputAttribute : System.Attribute
        {
            public bool IsStringOffset { get; set; }
            public bool IsIntOffset { get; set; }
            public String[] FieldNames { get; set; }
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class ExcelTableTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string stringId;
            public Int16 id;
        }

        class ExcelTableManagerManager
        {
            class TableIndexHelper
            {
                public string StringId { get; set; }
                public string FileName { get; set; }
                public ExcelTable excelTable;
                public Type type;

                public TableIndexHelper(string id, string name, ExcelTable table, Type t)
                {
                    StringId = id;
                    FileName = name;
                    excelTable = table;
                    type = t;
                }
            }

            List<TableIndexHelper> tables;

            public ExcelTableManagerManager()
            {
                tables = new List<TableIndexHelper>();
            }

            public void AddTable(string stringId, string fileName, Type type)
            {
                if (GetTableIndex(stringId) == null)
                {
                    tables.Add(new TableIndexHelper(stringId, fileName, null, type));
                }
            }

            public string GetReplacement(string id)
            {
                TableIndexHelper tableIndex = GetTableIndex(id);

                if (tableIndex == null)
                {
                    return id;
                }
                if (tableIndex.FileName == null)
                {
                    return id;
                }

                return tableIndex.FileName;
            }

            public ExcelTable CreateTable(string id, byte[] buffer)
            {
                TableIndexHelper tableIndex = GetTableIndex(id);
                if (tableIndex != null)
                {
                    tableIndex.excelTable = (ExcelTable)Activator.CreateInstance(tableIndex.type, buffer);
                    tableIndex.excelTable.StringId = tableIndex.StringId;
                    return tableIndex.excelTable;
                }

                return null;
            }

            public ExcelTable GetTable(string stringId)
            {
                TableIndexHelper tableIndex = GetTableIndex(stringId);
                if (tableIndex != null)
                {
                    return tableIndex.excelTable;
                }

                return null;
            }

            private TableIndexHelper GetTableIndex(string id)
            {
                foreach (TableIndexHelper tableIndex in tables)
                {
                    if (tableIndex.StringId.Equals(id, StringComparison.OrdinalIgnoreCase))
                    {
                        return tableIndex;
                    }
                    else if (tableIndex.FileName != null)
                    {
                        if (tableIndex.FileName.Equals(id, StringComparison.OrdinalIgnoreCase))
                        {
                            return tableIndex;
                        }
                    }
                }

                return null;
            }
        }

        ExcelTableManagerManager excelTables;

        public ExcelTables(byte[] data)
            : base(data)
        {
            excelTables = new ExcelTableManagerManager();
            excelTables.AddTable("ACHIEVEMENTS", null, typeof(Excel.Achievements));
            excelTables.AddTable("ACT", null, typeof(Excel.Act));
            excelTables.AddTable("AFFIXES", null, typeof(Excel.Affixes));
            excelTables.AddTable("AFFIXTYPES", null, typeof(Excel.AffixTypes));
            excelTables.AddTable("AI_BEHAVIOUR", null, typeof(Excel.AiBehaviour));
            excelTables.AddTable("AICOMMON_STATE", null, typeof(Excel.AiCommonState));
            excelTables.AddTable("AI_INIT", null, typeof(Excel.AiInit));
            excelTables.AddTable("AI_START", null, typeof(Excel.AiStart));
            excelTables.AddTable("ANIMATION_CONDITION", null, typeof(Excel.AnimationCondition));
            excelTables.AddTable("ANIMATION_GROUP", "ANIMATIONGROUPS", typeof(Excel.AnimationGroups));
            excelTables.AddTable("ANIMATION_STANCE", null, typeof(Excel.AnimationStance));
            excelTables.AddTable("BACKGROUNDSOUNDS", null, typeof(Excel.BackGroundSounds));
            excelTables.AddTable("BACKGROUNDSOUNDS2D", null, typeof(Excel.BackGroundSounds2D));
            excelTables.AddTable("BACKGROUNDSOUNDS3D", null, typeof(Excel.BackGroundSounds3D));
            excelTables.AddTable("BADGE_REWARDS", null, typeof(Excel.BadgeRewards));
            excelTables.AddTable("BONES", null, typeof(Excel.Bones));
            excelTables.AddTable("BONEWEIGHTS", null, typeof(Excel.Bones));
            excelTables.AddTable("BOOKMARKS", null, typeof(Excel.BookMarks));
            excelTables.AddTable("BUDGETSMODEL", null, typeof(Excel.BudgetsModel));
            excelTables.AddTable("BUDGETTEXTUREMIPS", null, typeof(Excel.BudgetTextureMips));
            excelTables.AddTable("CHARACTER_CLASS", null, typeof(Excel.CharacterClass));
            excelTables.AddTable("COLORSETS", null, typeof(Excel.ColorSets));
            excelTables.AddTable("CONDITION_FUNCTIONS", null, typeof(Excel.ConditionFunctions));
            excelTables.AddTable("DAMAGE_EFFECTS", "DAMAGEEFFECTS", typeof(Excel.DamageEffects));
            excelTables.AddTable("DAMAGETYPES", null, typeof(Excel.DamageTypes));
            excelTables.AddTable("DIALOG", null, typeof(Excel.Dialog));
            excelTables.AddTable("DIFFICULTY", null, typeof(Excel.Difficulty));
            excelTables.AddTable("DISPLAY_CHAR", "DISPLAYCHAR", typeof(Excel.Display));
            excelTables.AddTable("DISPLAY_ITEM", "DISPLAYITEM", typeof(Excel.Display));
            excelTables.AddTable("EFFECTSFILES", "EFFECTS_FILES", typeof(Excel.EffectsFiles));
            excelTables.AddTable("EFFECTS_INDEX", "EFFECTSINDEX", typeof(Excel.EffectsIndex));
            excelTables.AddTable("EFFECTSSHADERS", "EFFECTS_SHADERS", typeof(Excel.EffectsShaders));
            excelTables.AddTable("FILTER_CHATFILTER", "CHATFILTER", typeof(Excel.Filter));
            excelTables.AddTable("FILTER_NAMEFILTER", "NAMEFILTER", typeof(Excel.Filter));
            excelTables.AddTable("FACTION", null, typeof(Excel.Faction));
            excelTables.AddTable("FACTIONSTANDING", "FACTION_STANDING", typeof(Excel.FactionStanding));
            excelTables.AddTable("FONT", null, typeof(Excel.Font));
            excelTables.AddTable("FONTCOLORS", "FONTCOLOR", typeof(Excel.FontColor));
            excelTables.AddTable("FOOTSTEPS", null, typeof(Excel.FootSteps));
            excelTables.AddTable("GAME_GLOBALS", "GAMEGLOBALS", typeof(Excel.GameGlobals));
            excelTables.AddTable("GLOBAL_INDEX", "GLOBALINDEX", typeof(Excel.GlobalIndex));
            excelTables.AddTable("GLOBAL_STRING", "GLOBALSTRING", typeof(Excel.GlobalIndex));
            excelTables.AddTable("GLOBALTHEMES", "GLOBAL_THEMES", typeof(Excel.GlobalThemes));
            excelTables.AddTable("INITDB", null, typeof(Excel.InitDb));
            excelTables.AddTable("INTERACT", null, typeof(Excel.Interact));
            excelTables.AddTable("INTERACT_MENU", null, typeof(Excel.InteractMenu));
            excelTables.AddTable("INVENTORY", null, typeof(Excel.Inventory));
            excelTables.AddTable("INVENTORY_TYPES", null, typeof(Excel.InventoryTypes));
            excelTables.AddTable("INVLOC", null, typeof(Excel.InvLoc));
            excelTables.AddTable("ITEM_LEVELS", null, typeof(Excel.ItemLevels));
            excelTables.AddTable("ITEMLOOKGROUPS", "ITEM_LOOK_GROUPS", typeof(Excel.ItemLookGroups));
            excelTables.AddTable("ITEM_QUALITY", "ITEMQUALITY", typeof(Excel.ItemQuality));
            excelTables.AddTable("ITEMS", null, typeof(Excel.Items));
            excelTables.AddTable("LEVEL", "LEVELS", typeof(Excel.Levels));
            excelTables.AddTable("LEVEL_DRLG_CHOICE", "LEVELS_DRLG_CHOICE", typeof(Excel.LevelsDrlgChoice));
            excelTables.AddTable("LEVEL_DRLGS", "LEVELS_DRLGS", typeof(Excel.LevelsDrlgs));
            excelTables.AddTable("LEVELS_ENV", "LEVELSENV", typeof(Excel.LevelsEnv));
            excelTables.AddTable("LEVELS_FILE_PATHS", "LEVELS_FILE_PATH", typeof(Excel.LevelsFilePath));
            excelTables.AddTable("LEVELS_ROOM_INDEX", "LEVELSROOMINDEX", typeof(Excel.LevelsRoomIndex));
            excelTables.AddTable("LEVEL_RULES", "LEVELS_RULES", typeof(Excel.LevelsRules));
            excelTables.AddTable("LEVEL_THEMES", "LEVELS_THEMES", typeof(Excel.LevelsThemes));
            excelTables.AddTable("LEVEL_SCALING", "LEVELSCALING", typeof(Excel.LevelScaling));
            excelTables.AddTable("LOADING_TIPS", null, typeof(Excel.LoadingTips));
            excelTables.AddTable("MATERIALSCOLLISION", "MATERIALS_COLLISION", typeof(Excel.MaterialsCollision));
            excelTables.AddTable("MATERIALSGLOBAL", "MATERIALS_GLOBAL", typeof(Excel.MaterialsGlobal));
            excelTables.AddTable("MELEE_WEAPONS", "MELEEWEAPONS", typeof(Excel.MeleeWeapons));
            excelTables.AddTable("MISSILES", null, typeof(Excel.Items));
            excelTables.AddTable("MONLEVEL", null, typeof(Excel.MonLevel));
            excelTables.AddTable("MONSCALING", null, typeof(Excel.MonScaling));
            excelTables.AddTable("MONSTER_NAME_TYPES", "MONSTERNAMETYPES", typeof(Excel.MonsterNameTypes));
            excelTables.AddTable("MONSTERS", null, typeof(Excel.Items));
            excelTables.AddTable("OBJECTS", null, typeof(Excel.Items));
            excelTables.AddTable("PLAYERS", null, typeof(Excel.Items));
            excelTables.AddTable("PROPERTIES", null, typeof(Excel.Properties));
            excelTables.AddTable("PROCS", null, typeof(Excel.Procs));
            excelTables.AddTable("QUEST_STATE_VALUE", null, typeof(Excel.BookMarks));
            excelTables.AddTable("QUEST_STATUS", null, typeof(Excel.QuestStatus));
            excelTables.AddTable("SKILLS", null, typeof(Excel.Skills));
            excelTables.AddTable("SKILLEVENTTYPES", null, typeof(Excel.SkillEventTypes));
            excelTables.AddTable("STATE_EVENT_TYPES", null, typeof(Excel.StateEventTypes));
            excelTables.AddTable("STATE_LIGHTING", null, typeof(Excel.StateLighting));
            excelTables.AddTable("STATES", null, typeof(Excel.States));
            excelTables.AddTable("STATS", null, typeof(Excel.Stats));
            excelTables.AddTable("STATS_FUNC", "STATSFUNC", typeof(Excel.StatsFunc));
            excelTables.AddTable("STATS_SELECTOR", "STATSSELECTOR", typeof(Excel.BookMarks));
            excelTables.AddTable("TASK_STATUS", null, typeof(Excel.BookMarks));
            excelTables.AddTable("TAG", null, typeof(Excel.Tag));
            excelTables.AddTable("TREASURE", null, typeof(Excel.Treasure));
            excelTables.AddTable("UI_COMPONENT", null, typeof(Excel.UIComponent));
            excelTables.AddTable("UNIT_EVENT_TYPES", "UNITEVENTS", typeof(Excel.UnitEvents));
            excelTables.AddTable("UNITMODE_GROUPS", null, typeof(Excel.UnitModeGroups));
            excelTables.AddTable("UNITMODES", null, typeof(Excel.UnitModes));
            excelTables.AddTable("UNITTYPES", null, typeof(Excel.UnitTypes));
        }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<ExcelTableTable>(data, ref offset, Count);
        }

        public string GetTableStringId(int index)
        {
            return ((ExcelTableTable)tables[index]).stringId;
        }

        public ExcelTable GetTable(string stringId)
        {
            return excelTables.GetTable(stringId);
        }

        public bool LoadTables(string folder, Label label, ListBox excelTablesLoaded)
        {
            excelTablesLoaded.Sorted = true;

            for (int i = 0; i < Count; i++)
            {
                string stringId = GetTableStringId(i);
                string fileName = excelTables.GetReplacement(stringId);
                if (fileName == "EXCELTABLES" || fileName == "QUEST_COUNT_TUGBOAT")
                {
                    continue;
                }

                string filePath = folder + "\\" + fileName + ".txt.cooked";
                FileStream cookedFile;

                string currentItem = fileName.ToLower() + ".txt.cooked";
                label.Text = currentItem;

                try
                {
                    if (File.Exists(filePath))
                    {
                        cookedFile = new FileStream(filePath, FileMode.Open);
                    }
                    else
                    {
                        filePath = filePath.Replace("_common", "");
                        if (File.Exists(filePath))
                        {
                            cookedFile = new FileStream(filePath, FileMode.Open);
                        }
                        else
                        {
                            Debug.WriteLine("Debug Output - File not found: " + fileName);
                            continue;
                        }
                    }
                }
                catch (Exception)
                {

                    Debug.WriteLine("Debug Output - File failed to open: " + filePath);
                    continue;
                }

                byte[] buffer = FileTools.StreamToByteArray(cookedFile);
                try
                {
                    ExcelTable excelTable = excelTables.CreateTable(stringId, buffer);
                    if (excelTable != null)
                    {
                        excelTablesLoaded.Items.Add(excelTable);
                    }
                    else
                    {
                        Debug.WriteLine("Debug Output - File does not have table definition: " + fileName);
                    }

                }
                catch (Exception e)
                {
                    MessageBox.Show("Failed to parse cooked file " + currentItem + "\n\n" + e.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


                if (cookedFile != null)
                {
                    cookedFile.Dispose();
                }
            }

            return true;
        }
    }
}
