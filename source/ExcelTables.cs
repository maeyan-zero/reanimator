using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data;
using Reanimator.Forms;

namespace Reanimator.Excel
{
    public class ExcelTables : ExcelTable
    {
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
                    if (tableIndex.type == null)
                    {
                        return new ExcelTables(null);
                    }

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

            public bool IsMythosTable(String stringId)
            {
                TableIndexHelper tableIndex = GetTableIndex(stringId);
                if (tableIndex != null)
                {
                    return tableIndex.type == null ? true : false;
                }

                return false;
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

        public bool AllTablesLoaded { get; set; }
        ExcelTableManagerManager excelTables;
        List<ExcelTable> loadedTables;

        public ExcelTables(byte[] data)
            : base(data)
        {
            if (data == null)
            {
                return;
            }

            this.StringId = "EXCELTABLES";
            loadedTables = new List<ExcelTable>();

            excelTables = new ExcelTableManagerManager();
            excelTables.AddTable("ACHIEVEMENTS", null, typeof(Excel.Achievements));
            excelTables.AddTable("ACT", null, typeof(Excel.Act));
            excelTables.AddTable("AFFIXES", null, typeof(Excel.Affixes));
            excelTables.AddTable("AFFIXTYPES", null, typeof(Excel.AffixTypes));
            excelTables.AddTable("AI_BEHAVIOR", null, typeof(Excel.AiBehaviour));
            excelTables.AddTable("AICOMMON_STATE", null, typeof(Excel.AiCommonState));
            excelTables.AddTable("AI_INIT", null, typeof(Excel.AiInit));
            excelTables.AddTable("AI_START", null, typeof(Excel.AiStart));
            excelTables.AddTable("ANIMATION_CONDITION", null, typeof(Excel.AnimationCondition));
            excelTables.AddTable("ANIMATION_GROUP", "ANIMATION_GROUPS", typeof(Excel.AnimationGroups));
            excelTables.AddTable("ANIMATION_STANCE", null, typeof(Excel.AnimationStance));
            excelTables.AddTable("BACKGROUNDSOUNDS", null, typeof(Excel.BackGroundSounds));
            excelTables.AddTable("BACKGROUNDSOUNDS2D", null, typeof(Excel.BackGroundSounds2D));
            excelTables.AddTable("BACKGROUNDSOUNDS3D", null, typeof(Excel.BackGroundSounds3D));
            excelTables.AddTable("BADGE_REWARDS", null, typeof(Excel.BadgeRewards));
            excelTables.AddTable("BONES", null, typeof(Excel.Bones));
            excelTables.AddTable("BONEWEIGHTS", null, typeof(Excel.Bones));
            excelTables.AddTable("BOOKMARKS", null, typeof(Excel.BookMarks));
            excelTables.AddTable("BUDGETS_MODEL", null, typeof(Excel.BudgetsModel));
            excelTables.AddTable("BUDGETS_TEXTURE_MIPS", null, typeof(Excel.BudgetTextureMips));
            excelTables.AddTable("CHARACTER_CLASS", null, typeof(Excel.CharacterClass));
            excelTables.AddTable("COLORSETS", null, typeof(Excel.ColorSets));
            excelTables.AddTable("CONDITION_FUNCTIONS", null, typeof(Excel.ConditionFunctions));
            excelTables.AddTable("DAMAGE_EFFECTS", "DAMAGEEFFECTS", typeof(Excel.DamageEffects));
            excelTables.AddTable("DAMAGETYPES", null, typeof(Excel.DamageTypes));
            excelTables.AddTable("DIALOG", null, typeof(Excel.Dialog));
            excelTables.AddTable("DIFFICULTY", null, typeof(Excel.Difficulty));
            excelTables.AddTable("DISPLAY_CHAR", "DISPLAYCHAR", typeof(Excel.Display));                     //// THESE AREN'T BEING LOADED - TODO FIXME
            excelTables.AddTable("DISPLAY_ITEM", "DISPLAYITEM", typeof(Excel.Display));                     //// THESE AREN'T BEING LOADED - TODO FIXME
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
            excelTables.AddTable("ITEM_LOOKS", null, typeof(Excel.ItemLooks));
            excelTables.AddTable("ITEM_QUALITY", "ITEMQUALITY", typeof(Excel.ItemQuality));
            excelTables.AddTable("ITEMS", null, typeof(Excel.Items));
            excelTables.AddTable("LEVEL", "LEVELS", typeof(Excel.Levels));
            excelTables.AddTable("LEVEL_DRLG_CHOICE", "LEVELS_DRLG_CHOICE", typeof(Excel.LevelsDrlgChoice));
            excelTables.AddTable("LEVEL_DRLGS", "LEVELS_DRLGS", typeof(Excel.LevelsDrlgs));
            excelTables.AddTable("LEVELS_ENV", null, typeof(Excel.LevelsEnv));
            excelTables.AddTable("LEVEL_FILE_PATHS", "LEVELS_FILE_PATH", typeof(Excel.LevelsFilePath));
            excelTables.AddTable("ROOM_INDEX", "LEVELS_ROOM_INDEX", typeof(Excel.LevelsRoomIndex));
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
            excelTables.AddTable("MONSTERNAMETYPES", "MONSTER_NAME_TYPES", typeof(Excel.MonsterNameTypes));
            excelTables.AddTable("MONSTERNAMES", "MONSTER_NAMES", typeof(Excel.MonsterNames));
            excelTables.AddTable("MONSTERS", null, typeof(Excel.Items));
            excelTables.AddTable("MONSTER_QUALITY", null, typeof(Excel.MonsterQuality));
            excelTables.AddTable("MOVIESUBTITLES", "MOVIE_SUBTITLES", typeof(Excel.MovieSubTitles));
            excelTables.AddTable("MOVIELISTS", null, typeof(Excel.MovieLists));
            excelTables.AddTable("MOVIES", null, typeof(Excel.Movies));
            excelTables.AddTable("MUSIC", null, typeof(Excel.Music));
            excelTables.AddTable("MUSICGROOVELEVELS", null, typeof(Excel.MusicGrooveLevels));
            excelTables.AddTable("MUSICGROOVELEVELTYPES", null, typeof(Excel.MusicGrooveLevelTypes));
            excelTables.AddTable("MUSIC_REF", "MUSICREF", typeof(Excel.MusicRef));
            excelTables.AddTable("MUSIC_SCRIPT_DEBUG", "MUSICSCRIPTDEBUG", typeof(Excel.MusicScriptDebug));
            excelTables.AddTable("MUSICSTINGERS", null, typeof(Excel.MusicStingers));
            excelTables.AddTable("MUSICSTINGERSETS", null, typeof(Excel.MusicStingerSets));
            excelTables.AddTable("NPC", null, typeof(Excel.Npc));
            excelTables.AddTable("OBJECTS", null, typeof(Excel.Items));
            excelTables.AddTable("OBJECTTRIGGERS", null, typeof(Excel.ObjectTriggers));
            excelTables.AddTable("OFFER", null, typeof(Excel.Offer));
            //excelTables.AddTable("PALETTES", null, typeof(Excel.FontColor));
            excelTables.AddTable("PETLEVEL", null, typeof(Excel.MonLevel));
            excelTables.AddTable("PLAYERLEVELS", null, typeof(Excel.PlayerLevels));
            excelTables.AddTable("PLAYER_RACE", "PLAYERRACE", typeof(Excel.PlayerRace));
            excelTables.AddTable("PLAYERS", null, typeof(Excel.Items));
            excelTables.AddTable("PROPERTIES", null, typeof(Excel.Properties));
            excelTables.AddTable("PROCS", null, typeof(Excel.Procs));
            excelTables.AddTable("PROPS", null, typeof(Excel.LevelsRoomIndex));
            excelTables.AddTable("QUEST", null, typeof(Excel.Quest));
            excelTables.AddTable("QUEST_CAST", null, typeof(Excel.QuestCast));
            excelTables.AddTable("QUEST_STATE", null, typeof(Excel.QuestState));
            excelTables.AddTable("QUEST_STATE_VALUE", null, typeof(Excel.BookMarks));
            excelTables.AddTable("QUEST_STATUS", null, typeof(Excel.QuestStatus));
            excelTables.AddTable("QUEST_TEMPLATE", null, typeof(Excel.QuestTemplate));
            excelTables.AddTable("RARENAMES", null, typeof(Excel.RareNames));
            excelTables.AddTable("RECIPELISTS", null, typeof(Excel.RecipeLists));
            excelTables.AddTable("RECIPES", null, typeof(Excel.Recipes));
            excelTables.AddTable("SKILLGROUPS", null, typeof(Excel.SkillGroups));
            excelTables.AddTable("SKILLS", null, typeof(Excel.Skills));
            excelTables.AddTable("SKILLEVENTTYPES", null, typeof(Excel.SkillEventTypes));
            excelTables.AddTable("SKILLTABS", null, typeof(Excel.SkillTabs));
            excelTables.AddTable("SKU", null, typeof(Excel.Sku));
            excelTables.AddTable("SOUNDBUSES", null, typeof(Excel.SoundBuses));
            excelTables.AddTable("SOUND_MIXSTATES", "SOUNDMIXSTATES", typeof(Excel.SoundMixStates));
            excelTables.AddTable("SOUND_MIXSTATE_VALUES", "SOUNDMIXSTATEVALUES", typeof(Excel.SoundMixStateValues));
            excelTables.AddTable("SOUNDS", null, typeof(Excel.Sounds));
            excelTables.AddTable("SOUNDVCASETS", null, typeof(Excel.SoundVideoCasets));
            excelTables.AddTable("SPAWN_CLASS", "SPAWNCLASS", typeof(Excel.SpawnClass));
            excelTables.AddTable("SUBLEVEL", null, typeof(Excel.SubLevel));
            excelTables.AddTable("STATE_EVENT_TYPES", null, typeof(Excel.StateEventTypes));
            excelTables.AddTable("STATE_LIGHTING", null, typeof(Excel.StateLighting));
            excelTables.AddTable("STATES", null, typeof(Excel.States));
            excelTables.AddTable("STATS", null, typeof(Excel.Stats));
            excelTables.AddTable("STATS_FUNC", "STATSFUNC", typeof(Excel.StatsFunc));
            excelTables.AddTable("STATS_SELECTOR", "STATSSELECTOR", typeof(Excel.BookMarks));
            excelTables.AddTable("STRING_FILES", null, typeof(Excel.StringsFiles));
            excelTables.AddTable("TASK_STATUS", null, typeof(Excel.BookMarks));
            excelTables.AddTable("TAG", null, typeof(Excel.Tag));
            excelTables.AddTable("TASKS", null, typeof(Excel.Tasks));
            excelTables.AddTable("TEXTURE_TYPES", "TEXTURETYPES", typeof(Excel.TextureTypes));
            excelTables.AddTable("TREASURE", null, typeof(Excel.Treasure));
            excelTables.AddTable("UI_COMPONENT", null, typeof(Excel.UIComponent));
            excelTables.AddTable("UNIT_EVENT_TYPES", "UNITEVENTS", typeof(Excel.UnitEvents));
            excelTables.AddTable("UNITMODE_GROUPS", null, typeof(Excel.UnitModeGroups));
            excelTables.AddTable("UNITMODES", null, typeof(Excel.UnitModes));
            excelTables.AddTable("UNITTYPES", null, typeof(Excel.UnitTypes));
            excelTables.AddTable("WARDROBE_LAYER", "WARDROBE", typeof(Excel.Wardrobe));
            excelTables.AddTable("WARDROBE_APPEARANCE_GROUP", null, typeof(Excel.WardrobeAppearanceGroup));
            excelTables.AddTable("WARDROBE_BLENDOP", null, typeof(Excel.WardrobeBlendOp));
            excelTables.AddTable("WARDROBE_BODY", null, typeof(Excel.WardrobeBody));
            excelTables.AddTable("WARDROBE_LAYERSET", null, typeof(Excel.WardrobeBlendOp));
            excelTables.AddTable("WARDROBE_MODEL", null, typeof(Excel.WardrobeModel));
            excelTables.AddTable("WARDROBE_MODEL_GROUP", null, typeof(Excel.WardrobeModelGroup));
            excelTables.AddTable("WARDROBE_PART", null, typeof(Excel.WardrobePart));
            excelTables.AddTable("WARDROBE_TEXTURESET", null, typeof(Excel.WardrobeTextureSet));
            excelTables.AddTable("WARDROBE_TEXTURESET_GROUP", null, typeof(Excel.WardrobeTextureSetGroup));
            excelTables.AddTable("WEATHER", null, typeof(Excel.Weather));
            excelTables.AddTable("WEATHER_SETS", null, typeof(Excel.WeatherSets));

            // Empty tables
            excelTables.AddTable("GOSSIP", null, null);
            excelTables.AddTable("SOUNDOVERRIDES", null, null);

            // Mythos tables
            excelTables.AddTable("LEVEL_AREAS_MADLIB", null, null);
            excelTables.AddTable("LEVEL_AREAS_CAVE_NOUNS", null, null);
            excelTables.AddTable("LEVEL_AREAS_TEMPLE_NOUNS", null, null);
            excelTables.AddTable("LEVEL_AREAS_GOTHIC_NOUNS", null, null);
            excelTables.AddTable("LEVEL_AREAS_DESERTGOTHIC_NOUNS", null, null);
            excelTables.AddTable("LEVEL_AREAS_GOBLIN_NOUNS", null, null);
            excelTables.AddTable("LEVEL_AREAS_HEATH_NOUNS", null, null);
            excelTables.AddTable("LEVEL_AREAS_CANYON_NOUNS", null, null);
            excelTables.AddTable("LEVEL_AREAS_FOREST_NOUNS", null, null);
            excelTables.AddTable("LEVEL_AREAS_FARMLAND_NOUNS", null, null);
            excelTables.AddTable("LEVEL_AREAS_ADJECTIVES", null, null);
            excelTables.AddTable("LEVEL_AREAS_ADJ_BRIGHT", null, null);
            excelTables.AddTable("LEVEL_AREAS_AFFIXS", null, null);
            excelTables.AddTable("LEVEL_AREAS_SUFFIXS", null, null);
            excelTables.AddTable("LEVEL_AREAS_PROPERNAMEZONE1", null, null);
            excelTables.AddTable("LEVEL_AREAS_PROPERNAMEZONE2", null, null);
            excelTables.AddTable("LEVEL_AREAS_GOBLIN_NAMES", null, null);
            excelTables.AddTable("LEVEL_ZONES", null, null);
            excelTables.AddTable("LEVEL_AREAS", null, null);
            excelTables.AddTable("LEVEL_AREAS_LINKER", null, null);
            excelTables.AddTable("LEVEL_AREAS_SUFFIXS", null, null);
            excelTables.AddTable("LEVEL_ENVIRONMENTS", null, null);
            excelTables.AddTable("QUEST_COUNT_TUGBOAT", null, null);
            excelTables.AddTable("QUEST_RANDOM_FOR_TUGBOAT", null, null);
            excelTables.AddTable("QUEST_TITLES_FOR_TUGBOAT", null, null);
            excelTables.AddTable("QUEST_REWARD_BY_LEVEL_TUGBOAT", null, null);
            excelTables.AddTable("QUEST_RANDOM_TASKS_FOR_TUGBOAT", null, null);
            excelTables.AddTable("QUESTS_TASKS_FOR_TUGBOAT", null, null);
            excelTables.AddTable("QUEST_DICTIONARY_FOR_TUGBOAT", null, null);

            // I think these are Mythos
            //excelTables.AddTable("INVLOCIDX", null, null);          //// TODO this table is used in hero editor
            excelTables.AddTable("SKILL_LEVELS", null, null);
            excelTables.AddTable("SKILL_STATS", null, null);
            excelTables.AddTable("CHARDISPLAY", null, null);
            excelTables.AddTable("ITEMDISPLAY", null, null);
            excelTables.AddTable("EFFECTS", null, null);
            excelTables.AddTable("RENDER_FLAGS", null, null);
            excelTables.AddTable("DEBUG_BARS", null, null);
            excelTables.AddTable("ACHIEVEMENT_SLOTS", null, null);
            excelTables.AddTable("CRAFTING_SLOTS", null, null);
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

        public ExcelTable GetTable(int tableId)
        {
            foreach (ExcelTableTable excelTable in tables)
            {
                if (excelTable.id == tableId)
                {
                    return this.GetTable(excelTable.stringId);
                }
            }

            return null;
        }

        public List<ExcelTable> GetLoadedTables()
        {
            return loadedTables;
        }

        public int LoadedTableCount
        {
            get { return loadedTables.Count; }
        }

        public bool LoadTables(string folder, ProgressForm progress)
        {
            this.AllTablesLoaded = true;

            for (int i = 0; i < Count; i++)
            {
                string stringId = GetTableStringId(i);
                string fileName = excelTables.GetReplacement(stringId);
                if (fileName == "EXCELTABLES")
                {
                    loadedTables.Add(this);
                    continue;
                }

                string filePath = folder + "\\" + fileName + ".txt.cooked";
                FileStream cookedFile;

                string currentItem = fileName.ToLower() + ".txt.cooked";
                progress.SetCurrentItemText(currentItem);

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
                            if (!excelTables.IsMythosTable(stringId))
                            {
                                MessageBox.Show("File not found!\n\n" + filePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Debug.WriteLine("Debug Output - File not found: " + fileName);
                                this.AllTablesLoaded = false;
                            }
                            continue;
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Failed to open file for reading!\n\n" + filePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Debug.WriteLine("Debug Output - File failed to open: " + filePath);
                    this.AllTablesLoaded = false;
                    continue;
                }

                byte[] buffer = FileTools.StreamToByteArray(cookedFile);
                try
                {
                    Debug.Write(stringId + "\n");
                    ExcelTable excelTable = excelTables.CreateTable(stringId, buffer);
                    if (excelTable != null)
                    {
                        if (!excelTable.IsNull)
                        {
                            loadedTables.Add(excelTable);
                        }
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

            loadedTables.Sort();
            return true;
        }
    }
}
