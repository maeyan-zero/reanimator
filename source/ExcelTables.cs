using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
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
            public Int16 code;
        }

        public class ExcelTableManagerManager
        {
            class TableIndexHelper
            {
                public string StringId { get; private set; }
                public string FileName { get; private set; }
                public ExcelTable ExcelTable;
                public readonly Type Type;

                public TableIndexHelper(string stringId, String fileName, ExcelTable excelTable, Type type)
                {
                    StringId = stringId;
                    FileName = fileName;
                    ExcelTable = excelTable;
                    Type = type;
                }
            }

            readonly List<TableIndexHelper> _tables;

            public ExcelTableManagerManager()
            {
                _tables = new List<TableIndexHelper>();
            }

            public void AddTable(string stringId, string fileName, Type type)
            {
                if (GetTableIndex(stringId) == null)
                {
                    _tables.Add(new TableIndexHelper(stringId, fileName, null, type));
                }
            }

            public string GetReplacement(String stringId)
            {
                TableIndexHelper tableIndex = GetTableIndex(stringId);
                if (tableIndex == null) return stringId;

                return tableIndex.FileName ?? stringId;
            }

            public string ResolveTableId(String stringId)
            {
                TableIndexHelper tableIndex = GetTableIndex(stringId);

                if (tableIndex == null)
                {
                    return stringId;
                }
                if (tableIndex.FileName == null)
                {
                    return stringId;
                }
                return tableIndex.StringId ?? tableIndex.FileName;
            }

            public ExcelTable CreateTable(string id, byte[] buffer)
            {
                TableIndexHelper tableIndex = GetTableIndex(id);
                if (tableIndex != null)
                {
                    if (tableIndex.Type == null)
                    {
                        return new ExcelTables(null);
                    }

                    tableIndex.ExcelTable = (ExcelTable)Activator.CreateInstance(tableIndex.Type, buffer);
                    tableIndex.ExcelTable.StringId = tableIndex.StringId;
                    return tableIndex.ExcelTable;
                }

                return null;
            }

            public ExcelTable GetTable(string stringId)
            {
                TableIndexHelper tableIndex = GetTableIndex(stringId);
                return tableIndex != null ? tableIndex.ExcelTable : null;
            }

            public bool IsMythosTable(String stringId)
            {
                TableIndexHelper tableIndex = GetTableIndex(stringId);
                if (tableIndex != null)
                {
                    return tableIndex.Type == null ? true : false;
                }

                return false;
            }

            private TableIndexHelper GetTableIndex(String id)
            {
                foreach (TableIndexHelper tableIndex in _tables)
                {
                    if (tableIndex.StringId.Equals(id, StringComparison.OrdinalIgnoreCase))
                    {
                        return tableIndex;
                    }
                    if (tableIndex.FileName == null) continue;

                    if (tableIndex.FileName.Equals(id, StringComparison.OrdinalIgnoreCase))
                    {
                        return tableIndex;
                    }
                }

                return null;
            }


        }

        public bool AllTablesLoaded { get; private set; }
        readonly ExcelTableManagerManager _excelTables;
        readonly List<ExcelTable> _loadedTables;

        public ExcelTableManagerManager TableManager
        {
            get
            {
                return _excelTables;
            }
        }

        public ExcelTables(byte[] data)
            : base(data)
        {
            if (data == null)
            {
                return;
            }

            StringId = "EXCELTABLES";
            _loadedTables = new List<ExcelTable>();

            _excelTables = new ExcelTableManagerManager();
            _excelTables.AddTable("ACHIEVEMENTS", null, typeof(Achievements));
            _excelTables.AddTable("ACT", null, typeof(Act));
            _excelTables.AddTable("AFFIXES", null, typeof(Affixes));
            _excelTables.AddTable("AFFIXTYPES", null, typeof(AffixTypes));
            _excelTables.AddTable("AI_BEHAVIOR", null, typeof(AiBehaviour));
            _excelTables.AddTable("AICOMMON_STATE", null, typeof(AiCommonState));
            _excelTables.AddTable("AI_INIT", null, typeof(AiInit));
            _excelTables.AddTable("AI_START", null, typeof(AiStart));
            _excelTables.AddTable("ANIMATION_CONDITION", null, typeof(AnimationCondition));
            _excelTables.AddTable("ANIMATION_GROUP", "ANIMATION_GROUPS", typeof(AnimationGroups));
            _excelTables.AddTable("ANIMATION_STANCE", null, typeof(AnimationStance));
            _excelTables.AddTable("BACKGROUNDSOUNDS", null, typeof(BackGroundSounds));
            _excelTables.AddTable("BACKGROUNDSOUNDS2D", null, typeof(BackGroundSounds2D));
            _excelTables.AddTable("BACKGROUNDSOUNDS3D", null, typeof(BackGroundSounds3D));
            _excelTables.AddTable("BADGE_REWARDS", null, typeof(BadgeRewards));
            _excelTables.AddTable("BONES", null, typeof(Bones));
            _excelTables.AddTable("BONEWEIGHTS", null, typeof(Bones));
            _excelTables.AddTable("BOOKMARKS", null, typeof(BookMarks));
            _excelTables.AddTable("BUDGETS_MODEL", null, typeof(BudgetsModel));
            _excelTables.AddTable("BUDGETS_TEXTURE_MIPS", null, typeof(BudgetTextureMips));
            _excelTables.AddTable("CHAT_INSTANCED_CHANNELS", null, typeof(ChatInstancedChannels));
            _excelTables.AddTable("CHARACTER_CLASS", null, typeof(CharacterClass));
            _excelTables.AddTable("COLORSETS", null, typeof(ColorSets));
            _excelTables.AddTable("CONDITION_FUNCTIONS", null, typeof(ConditionFunctions));
            _excelTables.AddTable("DAMAGE_EFFECTS", "DAMAGEEFFECTS", typeof(DamageEffects));
            _excelTables.AddTable("DAMAGETYPES", null, typeof(DamageTypes));
            _excelTables.AddTable("DIALOG", null, typeof(Dialog));
            _excelTables.AddTable("DIFFICULTY", null, typeof(Difficulty));
            _excelTables.AddTable("CHARDISPLAY", "DISPLAY_CHAR", typeof(Display));
            _excelTables.AddTable("ITEMDISPLAY", "DISPLAY_ITEM", typeof(Display));
            _excelTables.AddTable("EFFECTS_FILES", null, typeof(EffectsFiles));
            _excelTables.AddTable("EFFECTS", "EFFECTS_INDEX", typeof(EffectsIndex));
            _excelTables.AddTable("EFFECTS_SHADERS", null, typeof(EffectsShaders));
            _excelTables.AddTable("FILTER_CHATFILTER", "CHATFILTER", typeof(Filter));
            _excelTables.AddTable("FILTER_NAMEFILTER", "NAMEFILTER", typeof(Filter));
            _excelTables.AddTable("FACTION", null, typeof(Faction));
            _excelTables.AddTable("FACTIONSTANDING", "FACTION_STANDING", typeof(FactionStanding));
            _excelTables.AddTable("FONT", null, typeof(Font));
            _excelTables.AddTable("FONTCOLORS", "FONTCOLOR", typeof(FontColor));
            _excelTables.AddTable("FOOTSTEPS", null, typeof(FootSteps));
            _excelTables.AddTable("GAME_GLOBALS", "GAMEGLOBALS", typeof(GameGlobals));
            _excelTables.AddTable("GLOBAL_INDEX", "GLOBALINDEX", typeof(GlobalIndex));
            _excelTables.AddTable("GLOBAL_STRING", "GLOBALSTRING", typeof(GlobalIndex));
            _excelTables.AddTable("GLOBALTHEMES", "GLOBAL_THEMES", typeof(GlobalThemes));
            _excelTables.AddTable("INITDB", null, typeof(InitDb));
            _excelTables.AddTable("INTERACT", null, typeof(Interact));
            _excelTables.AddTable("INTERACT_MENU", null, typeof(InteractMenu));
            _excelTables.AddTable("INVENTORY", null, typeof(Inventory));
            _excelTables.AddTable("INVENTORY_TYPES", null, typeof(InventoryTypes));
            _excelTables.AddTable("INVLOCIDX", "INVLOC", typeof(InvLoc));
            _excelTables.AddTable("ITEM_LEVELS", null, typeof(ItemLevels));
            _excelTables.AddTable("ITEMLOOKGROUPS", "ITEM_LOOK_GROUPS", typeof(ItemLookGroups));
            _excelTables.AddTable("ITEM_LOOKS", null, typeof(ItemLooks));
            _excelTables.AddTable("ITEM_QUALITY", "ITEMQUALITY", typeof(ItemQuality));
            _excelTables.AddTable("ITEMS", null, typeof(Items));
            _excelTables.AddTable("LEVEL", "LEVELS", typeof(Levels));
            _excelTables.AddTable("LEVEL_DRLG_CHOICE", "LEVELS_DRLG_CHOICE", typeof(LevelsDrlgChoice));
            _excelTables.AddTable("LEVEL_DRLGS", "LEVELS_DRLGS", typeof(LevelsDrlgs));
            _excelTables.AddTable("LEVELS_ENV", null, typeof(LevelsEnv));
            _excelTables.AddTable("LEVEL_FILE_PATHS", "LEVELS_FILE_PATH", typeof(LevelsFilePath));
            _excelTables.AddTable("ROOM_INDEX", "LEVELS_ROOM_INDEX", typeof(LevelsRoomIndex));
            _excelTables.AddTable("LEVEL_RULES", "LEVELS_RULES", typeof(LevelsRules));
            _excelTables.AddTable("LEVEL_THEMES", "LEVELS_THEMES", typeof(LevelsThemes));
            _excelTables.AddTable("LEVEL_SCALING", "LEVELSCALING", typeof(LevelScaling));
            _excelTables.AddTable("LOADING_TIPS", null, typeof(LoadingTips));
            _excelTables.AddTable("MATERIALSCOLLISION", "MATERIALS_COLLISION", typeof(MaterialsCollision));
            _excelTables.AddTable("MATERIALSGLOBAL", "MATERIALS_GLOBAL", typeof(MaterialsGlobal));
            _excelTables.AddTable("MELEE_WEAPONS", "MELEEWEAPONS", typeof(MeleeWeapons));
            _excelTables.AddTable("MISSILES", null, typeof(Items));
            _excelTables.AddTable("MONLEVEL", null, typeof(MonLevel));
            _excelTables.AddTable("MONSCALING", null, typeof(MonScaling));
            _excelTables.AddTable("MONSTERNAMETYPES", "MONSTER_NAME_TYPES", typeof(MonsterNameTypes));
            _excelTables.AddTable("MONSTERNAMES", "MONSTER_NAMES", typeof(MonsterNames));
            _excelTables.AddTable("MONSTERS", null, typeof(Items));
            _excelTables.AddTable("MONSTER_QUALITY", null, typeof(MonsterQuality));
            _excelTables.AddTable("MOVIESUBTITLES", "MOVIE_SUBTITLES", typeof(MovieSubTitles));
            _excelTables.AddTable("MOVIELISTS", null, typeof(MovieLists));
            _excelTables.AddTable("MOVIES", null, typeof(Movies));
            _excelTables.AddTable("MUSIC", null, typeof(Music));
            _excelTables.AddTable("MUSICCONDITIONS", null, typeof(MusicConditions));
            _excelTables.AddTable("MUSICGROOVELEVELS", null, typeof(MusicGrooveLevels));
            _excelTables.AddTable("MUSICGROOVELEVELTYPES", null, typeof(MusicGrooveLevelTypes));
            _excelTables.AddTable("MUSIC_REF", "MUSICREF", typeof(MusicRef));
            _excelTables.AddTable("MUSIC_SCRIPT_DEBUG", "MUSICSCRIPTDEBUG", typeof(MusicScriptDebug));
            _excelTables.AddTable("MUSICSTINGERS", null, typeof(MusicStingers));
            _excelTables.AddTable("MUSICSTINGERSETS", null, typeof(MusicStingerSets));
            _excelTables.AddTable("NPC", null, typeof(Npc));
            _excelTables.AddTable("OBJECTS", null, typeof(Items));
            _excelTables.AddTable("OBJECTTRIGGERS", null, typeof(ObjectTriggers));
            _excelTables.AddTable("OFFER", null, typeof(Offer));
            _excelTables.AddTable("PALETTES", null, typeof(Palettes));
            _excelTables.AddTable("PETLEVEL", null, typeof(MonLevel));
            _excelTables.AddTable("PLAYERLEVELS", null, typeof(PlayerLevels));
            _excelTables.AddTable("PLAYER_RACE", "PLAYERRACE", typeof(PlayerRace));
            _excelTables.AddTable("PLAYERS", null, typeof(Items));
            _excelTables.AddTable("PROPERTIES", null, typeof(Properties));
            _excelTables.AddTable("PROCS", null, typeof(Procs));
            _excelTables.AddTable("PROPS", null, typeof(LevelsRoomIndex));
            _excelTables.AddTable("QUEST", null, typeof(Quest));
            _excelTables.AddTable("QUEST_CAST", null, typeof(QuestCast));
            _excelTables.AddTable("QUEST_STATE", null, typeof(QuestState));
            _excelTables.AddTable("QUEST_STATE_VALUE", null, typeof(BookMarks));
            _excelTables.AddTable("QUEST_STATUS", null, typeof(QuestStatus));
            _excelTables.AddTable("QUEST_TEMPLATE", null, typeof(QuestTemplate));
            _excelTables.AddTable("RARENAMES", null, typeof(RareNames));
            _excelTables.AddTable("RECIPELISTS", null, typeof(RecipeLists));
            _excelTables.AddTable("RECIPES", null, typeof(Recipes));
            _excelTables.AddTable("SKILLGROUPS", null, typeof(SkillGroups));
            _excelTables.AddTable("SKILLS", null, typeof(Skills));
            _excelTables.AddTable("SKILLEVENTTYPES", null, typeof(SkillEventTypes));
            _excelTables.AddTable("SKILLTABS", null, typeof(SkillTabs));
            _excelTables.AddTable("SKU", null, typeof(Sku));
            _excelTables.AddTable("SOUNDBUSES", null, typeof(SoundBuses));
            _excelTables.AddTable("SOUND_MIXSTATES", "SOUNDMIXSTATES", typeof(SoundMixStates));
            _excelTables.AddTable("SOUND_MIXSTATE_VALUES", "SOUNDMIXSTATEVALUES", typeof(SoundMixStateValues));
            _excelTables.AddTable("SOUNDS", null, typeof(Sounds));
            _excelTables.AddTable("SOUNDVCAS", null, typeof(SoundVidCas));
            _excelTables.AddTable("SOUNDVCASETS", null, typeof(SoundVideoCasets));
            _excelTables.AddTable("SPAWN_CLASS", "SPAWNCLASS", typeof(SpawnClass));
            _excelTables.AddTable("SUBLEVEL", null, typeof(SubLevel));
            _excelTables.AddTable("STATE_EVENT_TYPES", null, typeof(StateEventTypes));
            _excelTables.AddTable("STATE_LIGHTING", null, typeof(StateLighting));
            _excelTables.AddTable("STATES", null, typeof(States));
            _excelTables.AddTable("STATS", null, typeof(Stats));
            _excelTables.AddTable("STATS_FUNC", "STATSFUNC", typeof(StatsFunc));
            _excelTables.AddTable("STATS_SELECTOR", "STATSSELECTOR", typeof(BookMarks));
            _excelTables.AddTable("STRING_FILES", null, typeof(StringsFiles));
            _excelTables.AddTable("TASK_STATUS", null, typeof(BookMarks));
            _excelTables.AddTable("TAG", null, typeof(Tag));
            _excelTables.AddTable("TASKS", null, typeof(Tasks));
            _excelTables.AddTable("TEXTURE_TYPES", "TEXTURETYPES", typeof(TextureTypes));
            _excelTables.AddTable("TREASURE", null, typeof(Treasure));
            _excelTables.AddTable("UI_COMPONENT", null, typeof(UIComponent));
            _excelTables.AddTable("UNIT_EVENT_TYPES", "UNITEVENTS", typeof(UnitEvents));
            _excelTables.AddTable("UNITMODE_GROUPS", null, typeof(UnitModeGroups));
            _excelTables.AddTable("UNITMODES", null, typeof(UnitModes));
            _excelTables.AddTable("UNITTYPES", null, typeof(UnitTypes));
            _excelTables.AddTable("WARDROBE_LAYER", "WARDROBE", typeof(Wardrobe));
            _excelTables.AddTable("WARDROBE_APPEARANCE_GROUP", null, typeof(WardrobeAppearanceGroup));
            _excelTables.AddTable("WARDROBE_BLENDOP", null, typeof(WardrobeBlendOp));
            _excelTables.AddTable("WARDROBE_BODY", null, typeof(WardrobeBody));
            _excelTables.AddTable("WARDROBE_LAYERSET", null, typeof(WardrobeBlendOp));
            _excelTables.AddTable("WARDROBE_MODEL", null, typeof(WardrobeModel));
            _excelTables.AddTable("WARDROBE_MODEL_GROUP", null, typeof(WardrobeModelGroup));
            _excelTables.AddTable("WARDROBE_PART", null, typeof(WardrobePart));
            _excelTables.AddTable("WARDROBE_TEXTURESET", null, typeof(WardrobeTextureSet));
            _excelTables.AddTable("WARDROBE_TEXTURESET_GROUP", null, typeof(WardrobeTextureSetGroup));
            _excelTables.AddTable("WEATHER", null, typeof(Weather));
            _excelTables.AddTable("WEATHER_SETS", null, typeof(WeatherSets));

            // Empty tables
            _excelTables.AddTable("GOSSIP", null, null);
            _excelTables.AddTable("SOUNDOVERRIDES", null, null);

            // Mythos tables
            _excelTables.AddTable("LEVEL_AREAS_MADLIB", null, null);
            _excelTables.AddTable("LEVEL_AREAS_CAVE_NOUNS", null, null);
            _excelTables.AddTable("LEVEL_AREAS_TEMPLE_NOUNS", null, null);
            _excelTables.AddTable("LEVEL_AREAS_GOTHIC_NOUNS", null, null);
            _excelTables.AddTable("LEVEL_AREAS_DESERTGOTHIC_NOUNS", null, null);
            _excelTables.AddTable("LEVEL_AREAS_GOBLIN_NOUNS", null, null);
            _excelTables.AddTable("LEVEL_AREAS_HEATH_NOUNS", null, null);
            _excelTables.AddTable("LEVEL_AREAS_CANYON_NOUNS", null, null);
            _excelTables.AddTable("LEVEL_AREAS_FOREST_NOUNS", null, null);
            _excelTables.AddTable("LEVEL_AREAS_FARMLAND_NOUNS", null, null);
            _excelTables.AddTable("LEVEL_AREAS_ADJECTIVES", null, null);
            _excelTables.AddTable("LEVEL_AREAS_ADJ_BRIGHT", null, null);
            _excelTables.AddTable("LEVEL_AREAS_AFFIXS", null, null);
            _excelTables.AddTable("LEVEL_AREAS_SUFFIXS", null, null);
            _excelTables.AddTable("LEVEL_AREAS_PROPERNAMEZONE1", null, null);
            _excelTables.AddTable("LEVEL_AREAS_PROPERNAMEZONE2", null, null);
            _excelTables.AddTable("LEVEL_AREAS_GOBLIN_NAMES", null, null);
            _excelTables.AddTable("LEVEL_ZONES", null, null);
            _excelTables.AddTable("LEVEL_AREAS", null, null);
            _excelTables.AddTable("LEVEL_AREAS_LINKER", null, null);
            _excelTables.AddTable("LEVEL_AREAS_SUFFIXS", null, null);
            _excelTables.AddTable("LEVEL_ENVIRONMENTS", null, null);
            _excelTables.AddTable("QUEST_COUNT_TUGBOAT", null, null);
            _excelTables.AddTable("QUEST_RANDOM_FOR_TUGBOAT", null, null);
            _excelTables.AddTable("QUEST_TITLES_FOR_TUGBOAT", null, null);
            _excelTables.AddTable("QUEST_REWARD_BY_LEVEL_TUGBOAT", null, null);
            _excelTables.AddTable("QUEST_RANDOM_TASKS_FOR_TUGBOAT", null, null);
            _excelTables.AddTable("QUESTS_TASKS_FOR_TUGBOAT", null, null);
            _excelTables.AddTable("QUEST_DICTIONARY_FOR_TUGBOAT", null, null);

            // I think these are Mythos
            _excelTables.AddTable("SKILL_LEVELS", null, null);
            _excelTables.AddTable("SKILL_STATS", null, null);
            _excelTables.AddTable("RENDER_FLAGS", null, null);
            _excelTables.AddTable("DEBUG_BARS", null, null);
            _excelTables.AddTable("ACHIEVEMENT_SLOTS", null, null);
            _excelTables.AddTable("CRAFTING_SLOTS", null, null);
        }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<ExcelTableTable>(data, ref offset, Count);
        }

        private string GetTableStringId(int index)
        {
            return ((ExcelTableTable)tables[index]).stringId;
        }

        public ExcelTable GetTable(string stringId)
        {
            return _excelTables.GetTable(stringId);
        }

        public ExcelTable GetTable(int tableId)
        {
            foreach (ExcelTableTable excelTable in tables)
            {
                if (excelTable.code == tableId)
                {
                    return GetTable(excelTable.stringId);
                }
            }

            return null;
        }

        public List<ExcelTable> GetLoadedTables()
        {
            return _loadedTables;
        }

        public int LoadedTableCount
        {
            get { return _loadedTables.Count; }
        }

        public void LoadTables(String folder, ProgressForm progress)
        {
            if (String.IsNullOrEmpty(folder)) return;

            AllTablesLoaded = true;

            for (int i = 0; i < Count; i++)
            {
                String stringId = GetTableStringId(i);
                String fileName = _excelTables.GetReplacement(stringId);
                if (String.IsNullOrEmpty(fileName)) continue;


                // add the base excel tables (this), but ovbviously, don't parse it again
                if (fileName == "EXCELTABLES")
                {
                    _loadedTables.Add(this);
                    continue;
                }


                // stop double loading issues with INVLOC/INVLOCIDX
                if (stringId == "INVLOC") continue;


                // get path to file
                String filePath = String.Format("{0}\\{1}.txt.cooked", folder, fileName);
                if (!File.Exists(filePath))
                {
                    filePath = filePath.Replace("_common", "");
                    if (!File.Exists(filePath))
                    {
                        if (!_excelTables.IsMythosTable(stringId))
                        {
                            MessageBox.Show("File not found!\n\n" + filePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Debug.WriteLine("Debug Output - File not found: " + fileName);
                            AllTablesLoaded = false;
                        }
                        continue;
                    }
                }


                // update progress
                if (progress != null)
                {
                    String currentItem = fileName.ToLower() + ".txt.cooked";
                    progress.SetCurrentItemText(currentItem);
                }


                // parse file
                try
                {
                    using (FileStream cookedFile = new FileStream(filePath, FileMode.Open))
                    {
                        byte[] buffer = FileTools.StreamToByteArray(cookedFile);

                        ExcelTable excelTable = _excelTables.CreateTable(stringId, buffer);
                        if (excelTable != null)
                        {
                            if (!excelTable.IsNull)
                            {
                                _loadedTables.Add(excelTable);
                                Debug.WriteLine("Excel Table Parsed: " + stringId);
                            }
                        }
                        else
                        {
                            Debug.WriteLine("File does not have table definition: " + fileName);
                        }
                    }

                    continue;
                }
                catch (ExcelTableException e)
                {
                    MessageBox.Show("Unexpected parsing error!\n\n" + e, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (BadHeaderFlag e)
                {
                    MessageBox.Show("File data tokens not aligned!\n\n" + e, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Failed to open file for reading!\n\n" + filePath + "\n\n" + e, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                AllTablesLoaded = false;
                Debug.WriteLine("Debug Output - File failed to parse: " + filePath);
            }
        }
    }
}
