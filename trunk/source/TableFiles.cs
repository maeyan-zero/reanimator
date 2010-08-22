using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Reanimator.Forms;
using Reanimator.ExcelDefinitions;

namespace Reanimator
{
    public class TableFiles
    {
        private readonly bool _debugAll;

        public Hashtable DataFiles { get; private set; }
        public Hashtable TableMap { get; private set; }

        public bool AllExcelFilesLoaded { get; set; }
        public bool AllStringsFilesLoaded { get; set; }
        public int LoadedFileCount
        {
            get { return DataFiles.Count; }
        }

        private class MapItem
        {
            public bool IsTCv4 { get; set; }
            public String NameReplace { get; set; }
            public Type RowType { get; set; }
            public bool IsEmpty { get; set; }
            public bool IsMythos { get; set; }
            public DataFile LoadedFile { get; set; }
            public bool ManualLoad { get; set; }
        }

        public TableFiles()
        {
            _debugAll = false;
            DataFiles = new Hashtable();

            TableMap = new Hashtable
            {
                // Excel files
                {"ACHIEVEMENTS", new MapItem {RowType = typeof (AchievementsRow)}},
                {"ACT", new MapItem {RowType = typeof (ActRow)}},
                {"AFFIXES", new MapItem {RowType = typeof (AffixesRow)}},
                {"AFFIXTYPES", new MapItem {RowType = typeof (AffixTypesRow)}},
                {"AI_BEHAVIOR", new MapItem {RowType = typeof (AiBehaviourRow)}},
                {"AICOMMON_STATE", new MapItem {RowType = typeof (AiCommonStateRow)}},
                {"AI_INIT", new MapItem {RowType = typeof (AiInitRow)}},
                {"AI_START", new MapItem {RowType = typeof (AiStartRow)}},
                {"ANIMATION_CONDITION", new MapItem {RowType = typeof (AnimationConditionRow)}},
                {"ANIMATION_GROUP", new MapItem {NameReplace = "ANIMATION_GROUPS", RowType = typeof (AnimationGroupsRow)}},
                {"ANIMATION_STANCE", new MapItem {RowType = typeof (AnimationStanceRow)}},
                {"BACKGROUNDSOUNDS", new MapItem {RowType = typeof (BackGroundSoundsRow)}},
                {"BACKGROUNDSOUNDS2D", new MapItem {RowType = typeof (BackGroundSounds2DRow)}},
                {"BACKGROUNDSOUNDS3D", new MapItem {RowType = typeof (BackGroundSounds3DRow)}},
                {"BADGE_REWARDS", new MapItem {RowType = typeof (BadgeRewardsRow)}},
                {"BONES", new MapItem {RowType = typeof (BonesRow)}},
                {"BONEWEIGHTS", new MapItem {RowType = typeof (BonesRow)}},
                {"BOOKMARKS", new MapItem {RowType = typeof (BookMarksRow)}},
                {"BUDGETS_MODEL", new MapItem {RowType = typeof (BudgetsModelRow)}},
                {"BUDGETS_TEXTURE_MIPS", new MapItem {RowType = typeof (BudgetTextureMipsRow)}},
                {"CHAT_INSTANCED_CHANNELS", new MapItem {RowType = typeof (ChatInstancedChannelsRow)}},
                {"CHARACTER_CLASS", new MapItem {RowType = typeof (CharacterClassRow)}},
                {"COLORSETS", new MapItem {RowType = typeof (ColorSetsRow)}},
                {"CONDITION_FUNCTIONS", new MapItem {RowType = typeof (ConditionFunctionsRow)}},
                {"DAMAGE_EFFECTS", new MapItem {NameReplace = "DAMAGEEFFECTS", RowType = typeof (DamageEffectsRow)}},
                {"DAMAGETYPES", new MapItem {RowType = typeof (DamageTypesRow)}},
                {"DIALOG", new MapItem {RowType = typeof (DialogRow)}},
                {"DIFFICULTY", new MapItem {RowType = typeof (DifficultyRow)}},
                {"CHARDISPLAY", new MapItem {NameReplace = "DISPLAY_CHAR", RowType = typeof (DisplayRow)}},
                {"ITEMDISPLAY", new MapItem {NameReplace = "DISPLAY_ITEM", RowType = typeof (DisplayRow)}},
                {"EFFECTS_FILES", new MapItem {RowType = typeof (EffectsFilesRow)}},
                {"EFFECTS", new MapItem {NameReplace = "EFFECTS_INDEX", RowType = typeof (EffectsIndexRow)}},
                {"EFFECTS_SHADERS", new MapItem {RowType = typeof (EffectsShadersRow)}},
                {"EXCELTABLES", new MapItem {RowType = typeof (ExcelTablesRow)}},
                {"FACTION", new MapItem {RowType = typeof (FactionRow)}},
                {"FACTION_STANDING", new MapItem {RowType = typeof (FactionStandingRow)}},
                {"FILTER_CHATFILTER", new MapItem {NameReplace = "CHATFILTER", RowType = typeof (FilterRow)}},
                {"FILTER_NAMEFILTER", new MapItem {NameReplace = "NAMEFILTER", RowType = typeof (FilterRow)}},
                {"FONT", new MapItem {RowType = typeof (FontRow)}},
                {"FONTCOLORS", new MapItem {NameReplace = "FONTCOLOR", RowType = typeof (FontColorRow)}},
                {"FOOTSTEPS", new MapItem {RowType = typeof (FootStepsRow)}},
                {"GAME_GLOBALS", new MapItem {NameReplace = "GAMEGLOBALS", RowType = typeof (GameGlobalsRow)}},
                {"GLOBAL_INDEX", new MapItem {NameReplace = "GLOBALINDEX", RowType = typeof (GlobalRow)}},
                {"GLOBAL_STRING", new MapItem {NameReplace = "GLOBALSTRING", RowType = typeof (GlobalRow)}},
                {"GLOBAL_THEMES", new MapItem {RowType = typeof (GlobalThemesRow)}},
                {"INITDB", new MapItem {RowType = typeof (InitDbRow)}},
                {"INTERACT", new MapItem {RowType = typeof (InteractRow)}},
                {"INTERACT_MENU", new MapItem {RowType = typeof (InteractMenuRow)}},
                {"INVENTORY", new MapItem {RowType = typeof (InventoryRow)}},
                {"INVENTORY_TYPES", new MapItem {RowType = typeof (InventoryTypesRow)}},
                {"INVLOC", new MapItem {RowType = typeof (InvLocRow)}},
                {"ITEM_LEVELS", new MapItem {RowType = typeof (ItemLevelsRow)}},
                {"ITEM_LOOK_GROUPS", new MapItem {RowType = typeof (ItemLookGroupsRow)}},
                {"ITEM_LOOKS", new MapItem {RowType = typeof (ItemLooksRow)}},
                {"ITEM_QUALITY", new MapItem {NameReplace = "ITEMQUALITY", RowType = typeof (ItemQualityRow)}},
                {"ITEMS", new MapItem {RowType = typeof (ItemsRow)}},
                {"LEVEL", new MapItem {NameReplace = "LEVELS", RowType = typeof (LevelsRow)}},
                {"LEVEL_DRLG_CHOICE", new MapItem {NameReplace = "LEVELS_DRLG_CHOICE", RowType = typeof (LevelsDrlgChoiceRow)}},
                {"LEVEL_DRLGS", new MapItem {NameReplace = "LEVELS_DRLGS", RowType = typeof (LevelsDrlgsRow)}},
                {"LEVEL_FILE_PATHS", new MapItem {NameReplace = "LEVELS_FILE_PATH", RowType = typeof (LevelsFilePathRow)}},
                {"LEVEL_ENVIRONMENTS", new MapItem {NameReplace = "LEVELS_ENV", RowType = typeof (LevelsEnvRow)}},
                {"ROOM_INDEX", new MapItem {NameReplace = "LEVELS_ROOM_INDEX", RowType = typeof (LevelsRoomIndexRow)}},
                {"LEVEL_RULES", new MapItem {NameReplace = "LEVELS_RULES", RowType = typeof (LevelsRulesRow)}},
                {"LEVEL_THEMES", new MapItem {NameReplace = "LEVELS_THEMES", RowType = typeof (LevelsThemesRow)}},
                {"LEVEL_SCALING", new MapItem {NameReplace = "LEVELSCALING", RowType = typeof (LevelScalingRow)}},
                {"LOADING_TIPS", new MapItem {RowType = typeof (LoadingTipsRow)}},
                {"MATERIALS_COLLISION", new MapItem {RowType = typeof (MaterialsCollisionRow)}},
                {"MATERIALS_GLOBAL", new MapItem {RowType = typeof (MaterialsGlobalRow)}},
                {"MELEEWEAPONS", new MapItem {RowType = typeof (MeleeWeaponsRow)}},
                {"MISSILES", new MapItem {RowType = typeof (ItemsRow)}},
                {"MONLEVEL", new MapItem {RowType = typeof (MonLevelRow)}},
                {"MONSCALING", new MapItem {RowType = typeof (MonScalingRow)}},
                {"MONSTER_NAME_TYPES", new MapItem {RowType = typeof (MonsterNameTypesRow)}},
                {"MONSTER_NAMES", new MapItem {RowType = typeof (MonsterNamesRow)}},
                {"MONSTER_QUALITY", new MapItem {RowType = typeof (MonsterQualityRow)}},
                {"MONSTERS", new MapItem {RowType = typeof (ItemsRow)}},
                {"MOVIE_SUBTITLES", new MapItem {RowType = typeof (MovieSubTitlesRow)}},
                {"MOVIELISTS", new MapItem {RowType = typeof (MovieListsRow)}},
                {"MOVIES", new MapItem {RowType = typeof (MoviesRow)}},
                {"MUSIC", new MapItem {RowType = typeof (MusicRow)}},
                {"MUSICCONDITIONS", new MapItem {RowType = typeof (MusicConditionsRow)}},
                {"MUSICGROOVELEVELS", new MapItem {RowType = typeof (MusicGrooveLevelsRow)}},
                {"MUSICGROOVELEVELTYPES", new MapItem {RowType = typeof (MusicGrooveLevelTypesRow)}},
                {"MUSIC_REF", new MapItem {NameReplace = "MUSICREF", RowType = typeof (MusicRefRow)}},
                {"MUSIC_SCRIPT_DEBUG", new MapItem {NameReplace = "MUSICSCRIPTDEBUG", RowType = typeof (MusicScriptDebugRow)}},
                {"MUSICSTINGERS", new MapItem {RowType = typeof (MusicStingersRow)}},
                {"MUSICSTINGERSETS", new MapItem {RowType = typeof (MusicStingerSetsRow)}},
                {"NPC", new MapItem {RowType = typeof (NpcRow)}},
                {"OBJECTS", new MapItem {RowType = typeof (ItemsRow)}},
                {"OBJECTTRIGGERS", new MapItem {RowType = typeof (ObjectTriggersRow)}},
                {"OFFER", new MapItem {RowType = typeof (OfferRow)}},
                {"PALETTES", new MapItem {RowType = typeof (PalettesRow)}},
                {"PETLEVEL", new MapItem {RowType = typeof (MonLevelRow)}},
                {"PLAYERLEVELS", new MapItem {RowType = typeof (PlayerLevelsRow)}},
                {"PLAYER_RACE", new MapItem {NameReplace = "PLAYERRACE", RowType = typeof (PlayerRaceRow)}},
                {"PLAYERS", new MapItem {RowType = typeof (ItemsRow)}},
                {"PROPERTIES", new MapItem {RowType = typeof (PropertiesRow)}},
                {"PROCS", new MapItem {RowType = typeof (ProcsRow)}},
                {"PROPS", new MapItem {RowType = typeof (LevelsRoomIndexRow)}},
                {"QUEST", new MapItem {RowType = typeof (QuestRow)}},
                {"QUEST_CAST", new MapItem {RowType = typeof (QuestCastRow)}},
                {"QUEST_STATE", new MapItem {RowType = typeof (QuestStateRow)}},
                {"QUEST_STATE_VALUE", new MapItem {RowType = typeof (BookMarksRow)}},
                {"QUEST_STATUS", new MapItem {RowType = typeof (QuestStatusRow)}},
                {"QUEST_TEMPLATE", new MapItem {RowType = typeof (QuestTemplateRow)}},
                {"RARENAMES", new MapItem {RowType = typeof (RareNamesRow)}},
                {"RECIPELISTS", new MapItem {RowType = typeof (RecipeListsRow)}},
                {"RECIPES", new MapItem {RowType = typeof (RecipesRow)}},
                {"SKILLGROUPS", new MapItem {RowType = typeof (SkillGroupsRow)}},
                {"SKILLS", new MapItem {RowType = typeof (SkillsRow)}},
                {"SKILLEVENTTYPES", new MapItem {RowType = typeof (SkillEventTypesRow)}},
                {"SKILLTABS", new MapItem {RowType = typeof (SkillTabsRow)}},
                {"SKU", new MapItem {RowType = typeof (SkuRow)}},
                {"SOUNDBUSES", new MapItem {RowType = typeof (SoundBusesRow)}},
                {"SOUND_MIXSTATES", new MapItem {NameReplace = "SOUNDMIXSTATES", RowType = typeof (SoundMixStatesRow)}},
                {"SOUND_MIXSTATE_VALUES", new MapItem {NameReplace = "SOUNDMIXSTATEVALUES", RowType = typeof (SoundMixStateValuesRow)}},
                {"SOUNDS", new MapItem {RowType = typeof (SoundsRow)}},
                {"SOUNDVCAS", new MapItem {RowType = typeof (SoundVidCasRow)}},
                {"SOUNDVCASETS", new MapItem {RowType = typeof (SoundVideoCasetsRow)}},
                {"SPAWN_CLASS", new MapItem {NameReplace = "SPAWNCLASS", RowType = typeof (SpawnClassRow)}},
                {"SUBLEVEL", new MapItem {RowType = typeof (SubLevelRow)}},
                {"STATE_EVENT_TYPES", new MapItem {RowType = typeof (StateEventTypesRow)}},
                {"STATE_LIGHTING", new MapItem {RowType = typeof (StateLightingRow)}},
                {"STATES", new MapItem {RowType = typeof (StatesRow)}},
                {"STATS", new MapItem {RowType = typeof (StatsRow)}},
                {"STATS_FUNC", new MapItem {NameReplace = "STATSFUNC", RowType = typeof (StatsFuncRow)}},
                {"STATS_SELECTOR", new MapItem {NameReplace = "STATSSELECTOR", RowType = typeof (BookMarksRow)}},
                {"STRING_FILES", new MapItem {RowType = typeof (StringFilesRow)}},
                {"TASK_STATUS", new MapItem {RowType = typeof (BookMarksRow)}},
                {"TAG", new MapItem {RowType = typeof (TagRow)}},
                {"TASKS", new MapItem {RowType = typeof (TasksRow)}},
                {"TEXTURE_TYPES", new MapItem {NameReplace = "TEXTURETYPES", RowType = typeof (TextureTypesRow)}},
                {"TREASURE", new MapItem {RowType = typeof (TreasureRow)}},
                {"UI_COMPONENT", new MapItem {RowType = typeof (UIComponentRow)}},
                {"UNIT_EVENT_TYPES", new MapItem {NameReplace = "UNITEVENTS", RowType = typeof (UnitEventsRow)}},
                {"UNITMODE_GROUPS", new MapItem {RowType = typeof (UnitModeGroupsRow)}},
                {"UNITMODES", new MapItem {RowType = typeof (UnitModesRow)}},
                {"UNITTYPES", new MapItem {RowType = typeof (UnitTypesRow)}},
                {"WARDROBE_LAYER", new MapItem {NameReplace = "WARDROBE", RowType = typeof (WardrobeRow)}},
                {"WARDROBE_APPEARANCE_GROUP", new MapItem {RowType = typeof (WardrobeAppearanceGroupRow)}},
                {"WARDROBE_BLENDOP", new MapItem {RowType = typeof (WardrobeBlendOpRow)}},
                {"WARDROBE_BODY", new MapItem {RowType = typeof (WardrobeBodyRow)}},
                {"WARDROBE_LAYERSET", new MapItem {RowType = typeof (WardrobeBlendOpRow)}},
                {"WARDROBE_MODEL", new MapItem {RowType = typeof (WardrobeModelRow)}},
                {"WARDROBE_MODEL_GROUP", new MapItem {RowType = typeof (WardrobeModelGroupRow)}},
                {"WARDROBE_PART", new MapItem {RowType = typeof (WardrobePartRow)}},
                {"WARDROBE_TEXTURESET", new MapItem {RowType = typeof (WardrobeTextureSetRow)}},
                {"WARDROBE_TEXTURESET_GROUP", new MapItem {RowType = typeof (WardrobeTextureSetGroupRow)}},
                {"WEATHER", new MapItem {RowType = typeof (WeatherRow)}},
                {"WEATHER_SETS", new MapItem {RowType = typeof (WeatherSetsRow)}},

                
                // TCv4
                {"ACHIEVEMENTS_TCv4", new MapItem {IsTCv4 = true, RowType = typeof (AchievementsTCv4Row)}},
                {"ACT_TCv4", new MapItem {IsTCv4 = true, RowType = typeof (ActTCv4Row)}},
                {"AFFIXES_TCv4", new MapItem {IsTCv4 = true, RowType = typeof (AffixesTCv4Row)}},
                {"BACKGROUNDSOUNDS2D_TCv4", new MapItem {IsTCv4 = true, RowType = typeof (BackGroundSounds2DTCv4Row)}},
                {"BADGE_REWARDS_TCv4", new MapItem {IsTCv4 = true, RowType = typeof (BadgeRewardsTCv4Row)}},
                {"CHARACTER_CLASS_TCv4", new MapItem {IsTCv4 = true, RowType = typeof (CharacterClassTCv4Row)}},
                {"DAMAGE_EFFECTS_TCv4", new MapItem {IsTCv4 = true, NameReplace = "DAMAGEEFFECTS", RowType = typeof (DamageEffectsTCv4Row)}},
                {"DAMAGETYPES_TCv4", new MapItem {IsTCv4 = true, RowType = typeof (DamageTypesTCv4Row)}},
                {"INVENTORY_TCv4", new MapItem {IsTCv4 = true, RowType = typeof (InventoryTCv4Row)}},
                {"ITEM_LOOKS_TCv4", new MapItem {IsTCv4 = true, RowType = typeof (ItemLooksTCv4Row)}},
                {"ITEM_QUALITY_TCv4", new MapItem {IsTCv4 = true, NameReplace = "ITEMQUALITY", RowType = typeof (ItemQualityTCv4Row)}},
                {"ITEMS_TCv4", new MapItem {IsTCv4 = true, RowType = typeof (ItemsTCv4Row)}},
                {"MATERIALS_COLLISION_TCv4", new MapItem {IsTCv4 = true, RowType = typeof (MaterialsCollisionTCv4Row)}},
                {"LEVEL_TCv4", new MapItem {IsTCv4 = true, NameReplace = "LEVELS", RowType = typeof (LevelsTCv4Row)}},
                {"LEVEL_SCALING_TCv4", new MapItem {IsTCv4 = true, NameReplace = "LEVELSCALING", RowType = typeof (LevelScalingTCv4Row)}},
                {"ROOM_INDEX_TCv4", new MapItem {IsTCv4 = true, NameReplace = "LEVELS_ROOM_INDEX", RowType = typeof (LevelsRoomIndexTCv4Row)}},
                {"PLAYERLEVELS_TCv4", new MapItem {IsTCv4 = true, RowType = typeof (PlayerLevelsTCv4Row)}},
                {"PROPS_TCv4", new MapItem {IsTCv4 = true, RowType = typeof (LevelsRoomIndexTCv4Row)}},
                {"RECIPES_TCv4", new MapItem {IsTCv4 = true, RowType = typeof (RecipesTCv4Row)}},
                {"SKILLS_TCv4", new MapItem {IsTCv4 = true, RowType = typeof (SkillsTCv4Row)}},
                {"SKILLEVENTTYPES_TCv4", new MapItem {IsTCv4 = true, RowType = typeof (SkillEventTypesTCv4Row)}},
                {"SKILLTABS_TCv4", new MapItem {IsTCv4 = true, RowType = typeof (SkillTabsTCv4Row)}},
                {"SOUND_MIXSTATES_TCv4", new MapItem {IsTCv4 = true, NameReplace = "SOUNDMIXSTATES", RowType = typeof (SoundMixStatesTCv4Row)}},
                {"SOUNDS_TCv4", new MapItem {IsTCv4 = true, RowType = typeof (SoundsTCv4Row)}},
                {"STATS_TCv4", new MapItem {IsTCv4 = true, RowType = typeof (StatsTCv4Row)}},
                {"TREASURE_TCv4", new MapItem {IsTCv4 = true, RowType = typeof (TreasureTCv4Row)}},
                {"UNITMODE_GROUPS_TCv4", new MapItem {IsTCv4 = true, RowType = typeof (UnitModeGroupsTCv4Row)}},
                {"UNITMODES_TCv4", new MapItem {IsTCv4 = true, RowType = typeof (UnitModesTCv4Row)}},
                {"UNITTYPES_TCv4", new MapItem {IsTCv4 = true, RowType = typeof (UnitTypesTCv4Row)}},
                {"WARDROBE_LAYER_TCv4", new MapItem {IsTCv4 = true, NameReplace = "WARDROBE", RowType = typeof (WardrobeTCv4Row)}},


                // Empty Excel files
                {"GOSSIP", new MapItem {IsEmpty = true}},
                {"INVLOCIDX", new MapItem {IsEmpty = true}},
                {"SOUNDOVERRIDES", new MapItem {IsEmpty = true}},


                // Non-Indexed Excel file
                {"LANGUAGE", new MapItem {ManualLoad = true, RowType = typeof (LanguageRow)}},
                {"REGION", new MapItem {ManualLoad = true, RowType = typeof (RegionRow)}},


                // Mythos Excel files
                {"LEVEL_AREAS", new MapItem {IsMythos = true}},
                {"LEVEL_AREAS_ADJ_BRIGHT", new MapItem {IsMythos = true}},
                {"LEVEL_AREAS_ADJECTIVES", new MapItem {IsMythos = true}},
                {"LEVEL_AREAS_AFFIXS", new MapItem {IsMythos = true}},
                {"LEVEL_AREAS_CANYON_NOUNS", new MapItem {IsMythos = true}},
                {"LEVEL_AREAS_CAVE_NOUNS", new MapItem {IsMythos = true}},
                {"LEVEL_AREAS_DESERTGOTHIC_NOUNS", new MapItem {IsMythos = true}},
                {"LEVEL_AREAS_GOBLIN_NAMES", new MapItem {IsMythos = true}},
                {"LEVEL_AREAS_GOBLIN_NOUNS", new MapItem {IsMythos = true}},
                {"LEVEL_AREAS_GOTHIC_NOUNS", new MapItem {IsMythos = true}},
                {"LEVEL_AREAS_FARMLAND_NOUNS", new MapItem {IsMythos = true}},
                {"LEVEL_AREAS_FOREST_NOUNS", new MapItem {IsMythos = true}},
                {"LEVEL_AREAS_HEATH_NOUNS", new MapItem {IsMythos = true}},
                {"LEVEL_AREAS_LINKER", new MapItem {IsMythos = true}},
                {"LEVEL_AREAS_MADLIB", new MapItem {IsMythos = true}},
                {"LEVEL_AREAS_PROPERNAMEZONE1", new MapItem {IsMythos = true}},
                {"LEVEL_AREAS_PROPERNAMEZONE2", new MapItem {IsMythos = true}},
                {"LEVEL_AREAS_TEMPLE_NOUNS", new MapItem {IsMythos = true}},
                {"LEVEL_AREAS_SUFFIXS", new MapItem {IsMythos = true}},
                {"LEVEL_ZONES", new MapItem {IsMythos = true}},
                {"QUEST_COUNT_TUGBOAT", new MapItem {IsMythos = true}},
                {"QUEST_DICTIONARY_FOR_TUGBOAT", new MapItem {IsMythos = true}},
                {"QUEST_RANDOM_FOR_TUGBOAT", new MapItem {IsMythos = true}},
                {"QUEST_RANDOM_TASKS_FOR_TUGBOAT", new MapItem {IsMythos = true}},
                {"QUEST_REWARD_BY_LEVEL_TUGBOAT", new MapItem {IsMythos = true}},
                {"QUEST_TITLES_FOR_TUGBOAT", new MapItem {IsMythos = true}},
                {"QUESTS_TASKS_FOR_TUGBOAT", new MapItem {IsMythos = true}},

                // I think these are Mythos
                {"ACHIEVEMENT_SLOTS", new MapItem {IsMythos = true}},
                {"CRAFTING_SLOTS", new MapItem {IsMythos = true}},
                {"DEBUG_BARS", new MapItem {IsMythos = true}},
                {"RENDER_FLAGS", new MapItem {IsMythos = true}},
                {"SKILL_LEVELS", new MapItem {IsMythos = true}},
                {"SKILL_STATS", new MapItem {IsMythos = true}},

                // Strings files
                {"Strings_Affix", new MapItem {RowType = typeof (StringsFile)}},
                {"Strings_Cinematic", new MapItem {RowType = typeof (StringsFile)}},
                {"Strings_Common", new MapItem {RowType = typeof (StringsFile)}},
                {"Strings_Credits", new MapItem {RowType = typeof (StringsFile)}},
                {"Strings_DisplayFormat", new MapItem {RowType = typeof (StringsFile)}},
                {"Strings_Install", new MapItem {RowType = typeof (StringsFile)}},
                {"Strings_Items", new MapItem {RowType = typeof (StringsFile)}},
                {"Strings_Level", new MapItem {RowType = typeof (StringsFile)}},
                {"Strings_LoadingTips", new MapItem {RowType = typeof (StringsFile)}},
                {"Strings_Monsters", new MapItem {RowType = typeof (StringsFile)}},
                {"Strings_Names", new MapItem {RowType = typeof (StringsFile)}},
                {"Strings_Quest", new MapItem {RowType = typeof (StringsFile)}},
                {"Strings_Skills", new MapItem {RowType = typeof (StringsFile)}},
                {"Strings_Strings", new MapItem {RowType = typeof (StringsFile)}},

                // Empty Strings files
                {"Strings_Install_MSI", new MapItem {IsEmpty = true}}
            };
        }

        public DataFile this[String stringId]
        {
            get
            {
                MapItem mapItem = TableMap[stringId] as MapItem;
                return mapItem == null ? null : mapItem.LoadedFile;
            }
        }

        public bool ParseSingleExcelData(String stringId, byte[] data)
        {
            if (String.IsNullOrEmpty(stringId) || data == null) return false;

            MapItem mapItem = TableMap[stringId] as MapItem;
            if (mapItem == null) return false;

            ExcelFile excelFile = new ExcelFile(stringId, mapItem.RowType);
            excelFile.ParseData(data);
            if (!excelFile.IsGood) return false;

            DataFiles.Add(excelFile.StringId, excelFile);
            mapItem.LoadedFile = excelFile;
            return true;
        }

        public bool LoadExcelFiles(ProgressForm progress, byte[] data)
        {
            if (data == null) return false;


            // load in initial exceltables.txt.cooked
            DataFile excelTables = new ExcelFile("EXCELTABLES", typeof(ExcelTablesRow));
            if (!excelTables.ParseData(data))
            {
                const string msg = "Failed to parse primary exceltables.txt.cooked file!\n\n";
                MessageBox.Show(msg, "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Debug.Write(msg);
                return false;
            }
            DataFiles.Add(excelTables.StringId, excelTables);


            // set status
            if (progress != null)
            {
                progress.ConfigBar(0, excelTables.Count, 1);
                progress.SetLoadingText("Loading in excel files (" + excelTables.Count + ")...");
            }


            // load in tables
            String folderPath = Config.DataDirsRoot + ExcelFile.FolderPath;
            AllExcelFilesLoaded = true;
            for (int i = 0; i < excelTables.Count; i++)
            {
                ExcelTablesRow excelTablesRow = excelTables.Rows[i] as ExcelTablesRow;
                if (excelTablesRow == null) continue;


                // do we have a definition for this stringId
                String stringId = excelTablesRow.StringId;
                MapItem mapItem = TableMap[stringId] as MapItem;
                if (mapItem == null)
                {
                    String msg = "Unknown excel table ID!\n\nstringId = " + stringId;
                    MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Debug.WriteLine(msg);
                    AllExcelFilesLoaded = false;
                    continue;
                }


                // do we want to even parse it?
                if (mapItem.IsMythos || mapItem.IsEmpty) continue;

                // get file name
                String fileName = mapItem.NameReplace ?? stringId;

                // don't do/add self again
                if (fileName == excelTables.StringId) continue;


                // get path to file
                String filePath = String.Format("{0}{1}.{2}", folderPath, fileName, ExcelFile.FileExtention);
                if (!File.Exists(filePath))
                {
                    filePath = filePath.Replace("_common", "");
                    if (!File.Exists(filePath))
                    {
                        String msg = "Excel file not found!\n\n" + filePath;
                        MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Debug.WriteLine(msg);
                        AllExcelFilesLoaded = false;
                        continue;
                    }
                }


                // update progress
                if (progress != null)
                {
                    progress.SetCurrentItemText(stringId);
                }


                // parse file
                try
                {
                    byte[] buffer = File.ReadAllBytes(filePath);

                    // note: this (stringId, or fileName?) is what the user will see in the loaded tables form
                    DataFile excelFile = new ExcelFile(stringId, mapItem.RowType);
                    excelFile.ParseData(buffer);

                    if (excelFile.IsGood)
                    {
                        DataFiles.Add(excelFile.StringId, excelFile);
                        mapItem.LoadedFile = excelFile;
                    }

                    continue;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to parse excel file!\n\n" + filePath + "\n\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                AllExcelFilesLoaded = false;
                Debug.WriteLine("Debug Output - File failed to parse: " + filePath);
            }

            AllExcelFilesLoaded = DoManualTables(progress);

            return true;
        }

        // this function is a bit of a dodgy copy-paste job
        // todo: fix up or something (consolidate with strings loading & excel loading etc
        private bool DoManualTables(ProgressForm progress)
        {
            String folderPath = Config.DataDirsRoot + ExcelFile.FolderPath;
            String folderPathTCv4 = Config.DataDirsRoot + @"\tcv4" + ExcelFile.FolderPath;

            foreach (DictionaryEntry de in TableMap)
            {
                MapItem mapItem = de.Value as MapItem;
                if (mapItem == null) continue;
                if (!mapItem.ManualLoad && !mapItem.IsTCv4) continue;


                // get path to file
                String stringId = de.Key as String;
                String fileName = mapItem.NameReplace ?? stringId;
                Debug.Assert(stringId != null);
                String filePath = mapItem.IsTCv4 ?
                                    String.Format("{0}{1}.{2}", folderPathTCv4, fileName.Replace("_TCv4", ""), ExcelFile.FileExtention) :
                                    String.Format("{0}{1}.{2}", folderPath,     fileName,                      ExcelFile.FileExtention);

                if (!File.Exists(filePath))
                {
                    filePath = filePath.Replace("_common", "");
                    if (!File.Exists(filePath))
                    {
                        String msg = "Excel file not found!\n\n" + filePath;
                        Debug.WriteLine(msg);

                        // doesn't matter if they don't load
                        // MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (!mapItem.IsTCv4)
                        {
                            // for release, uncomment me, and remove above - for testing, we want to know if the TC files aren't loading/found
                            //MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            AllExcelFilesLoaded = false;
                        }

                        continue;
                    }
                }


                // update progress
                if (progress != null)
                {
                    progress.SetCurrentItemText(stringId);
                }


                // parse file
                try
                {
                    byte[] buffer = File.ReadAllBytes(filePath);

                    // note: this (stringId, or fileName?) is what the user will see in the loaded tables form
                    DataFile excelFile = new ExcelFile(stringId, mapItem.RowType);
                    excelFile.ParseData(buffer);

                    if (excelFile.IsGood)
                    {
                        DataFiles.Add(excelFile.StringId, excelFile);
                        mapItem.LoadedFile = excelFile;
                    }

                    continue;
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Failed to parse excel file!\n\n" + filePath + "\n\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ExceptionLogger.LogException(ex, "DoManualTables", false);
                }

                AllExcelFilesLoaded = false;
                Debug.WriteLine("Debug Output - File failed to parse: " + filePath);
            }

            return true;
        }

        public bool LoadStringsFiles(ProgressForm progress, ExcelFile excelFile)
        {
            if (excelFile == null) return false;


            // set status
            if (progress != null)
            {
                progress.ConfigBar(0, excelFile.Count, 1);
                progress.SetLoadingText("Loading in strings files (" + excelFile.Count + ")...");
            }


            // loop through excel strings file table
            String baseDataDir = Config.DataDirsRoot + StringsFile.FolderPath;
            AllStringsFilesLoaded = true;
            foreach (StringFilesRow stringFilesRow in excelFile.Rows)
            {
                // do we have a definition for this stringId
                String stringId = stringFilesRow.name;
                MapItem mapItem = TableMap[stringId] as MapItem;
                if (mapItem == null)
                {
                    String msg = "Unknown strings table ID!\n\nstringId = " + stringId;
                    MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Debug.WriteLine(msg);
                    AllStringsFilesLoaded = false;
                    continue;
                }


                // do we want to even parse it?
                if (mapItem.IsMythos || mapItem.IsEmpty) continue;

                // get file name
                String fileName = mapItem.NameReplace ?? stringId;


                // ensure exists
                String filePath = String.Format("{0}{1}.{2}", baseDataDir, fileName, StringsFile.FileExtention);
                if (!File.Exists(filePath))
                {
                    filePath = filePath.Replace("data", "data_common");
                    if (!File.Exists(filePath))
                    {
                        String msg = "Strings file not found!\n\n" + filePath;
                        MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Debug.WriteLine(msg);
                        AllStringsFilesLoaded = false;
                        continue;
                    }
                }


                // update progress
                if (progress != null)
                {
                    progress.SetCurrentItemText(stringId);
                }


                // parse file
                try
                {
                    byte[] buffer = File.ReadAllBytes(filePath);

                    StringsFile stringsFile = new StringsFile(stringId, mapItem.RowType) { FilePath = filePath };
                    stringsFile.ParseData(buffer);

                    if (stringsFile.IsGood)
                    {
                        DataFiles.Add(stringsFile.StringId, stringsFile);
                        mapItem.LoadedFile = stringsFile;
                    }

                    continue;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to parse strings file!\n\n" + filePath + "\n\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                AllStringsFilesLoaded = false;
                Debug.WriteLine("Debug Output - File failed to parse: " + filePath);
            }


            return true;
        }

        public ExcelFile GetExcelTableFromId(int tableId)
        {
            ExcelFile excelTables = DataFiles["EXCELTABLES"] as ExcelFile;
            if (excelTables == null) return null;

            return (from ExcelTablesRow excelTable in excelTables.Rows
                    where excelTable.Code == tableId
                    select DataFiles[excelTable.StringId] as ExcelFile).FirstOrDefault();
        }

        public ExcelFile GetExcelTableFromId(string tableId)
        {
            ExcelFile excelTables = DataFiles["EXCELTABLES"] as ExcelFile;
            if (excelTables == null) return null;

            return (from ExcelTablesRow excelTable in excelTables.Rows
                    where excelTable.StringId == tableId
                    select DataFiles[excelTable.StringId] as ExcelFile).FirstOrDefault();
        }

        public String GetStringIdFromFileName(String fileName)
        {
            if (String.IsNullOrEmpty(fileName)) return null;

            return (from DictionaryEntry de in TableMap
                    let mapItem = de.Value as MapItem
                    let compareTo = mapItem.NameReplace ?? (string)de.Key
                    where String.Equals(fileName, compareTo, StringComparison.CurrentCultureIgnoreCase)
                    select de.Key as String).FirstOrDefault();
        }

        public String GetFileNameFromStringId(String stringId)
        {
            if (String.IsNullOrEmpty(stringId)) return null;

            MapItem mapItem = TableMap[stringId] as MapItem;
            if (mapItem == null) return null;

            return mapItem.NameReplace ?? stringId;
        }

        public DataFile GetTableFromFileName(String fileName)
        {
            if (String.IsNullOrEmpty(fileName)) return null;

            String stringId = GetStringIdFromFileName(fileName);
            return DataFiles[stringId] as DataFile;
        }
    }
}