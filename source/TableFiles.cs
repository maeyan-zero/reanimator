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
        private readonly FileExplorer _fileExplorer;

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

        public DataFile this[String stringId]
        {
            get
            {
                MapItem mapItem = TableMap[stringId] as MapItem;
                return mapItem == null ? null : mapItem.LoadedFile;
            }
        }

        public TableFiles(ref FileExplorer fileExplorer)
        {
            _debugAll = false;
            _fileExplorer = fileExplorer;

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

                
                // TCv4 - different
                {"_TCv4_ACHIEVEMENTS", new MapItem {IsTCv4 = true, RowType = typeof (AchievementsTCv4Row)}},
                {"_TCv4_ACT", new MapItem {IsTCv4 = true, RowType = typeof (ActTCv4Row)}},
                {"_TCv4_AFFIXES", new MapItem {IsTCv4 = true, RowType = typeof (AffixesTCv4Row)}},
                {"_TCv4_BACKGROUNDSOUNDS2D", new MapItem {IsTCv4 = true, RowType = typeof (BackGroundSounds2DTCv4Row)}},
                {"_TCv4_BADGE_REWARDS", new MapItem {IsTCv4 = true, RowType = typeof (BadgeRewardsTCv4Row)}},
                {"_TCv4_CHARACTER_CLASS", new MapItem {IsTCv4 = true, RowType = typeof (CharacterClassTCv4Row)}},
                {"_TCv4_DAMAGE_EFFECTS", new MapItem {IsTCv4 = true, NameReplace = "DAMAGEEFFECTS", RowType = typeof (DamageEffectsTCv4Row)}},
                {"_TCv4_DAMAGETYPES", new MapItem {IsTCv4 = true, RowType = typeof (DamageTypesTCv4Row)}},
                {"_TCv4_INVENTORY", new MapItem {IsTCv4 = true, RowType = typeof (InventoryTCv4Row)}},
                {"_TCv4_ITEM_LOOKS", new MapItem {IsTCv4 = true, RowType = typeof (ItemLooksTCv4Row)}},
                {"_TCv4_ITEM_QUALITY", new MapItem {IsTCv4 = true, NameReplace = "ITEMQUALITY", RowType = typeof (ItemQualityTCv4Row)}},
                {"_TCv4_ITEMS", new MapItem {IsTCv4 = true, RowType = typeof (ItemsTCv4Row)}},
                {"_TCv4_OBJECTS", new MapItem {IsTCv4 = true, RowType = typeof (ItemsTCv4Row)}},
                {"_TCv4_MONSTERS", new MapItem {IsTCv4 = true, RowType = typeof (ItemsTCv4Row)}},
                {"_TCv4_PLAYERS", new MapItem {IsTCv4 = true, RowType = typeof (ItemsTCv4Row)}},
                {"_TCv4_MISSILES", new MapItem {IsTCv4 = true, RowType = typeof (ItemsTCv4Row)}},
                {"_TCv4_MATERIALS_COLLISION", new MapItem {IsTCv4 = true, RowType = typeof (MaterialsCollisionTCv4Row)}},
                {"_TCv4_LEVEL", new MapItem {IsTCv4 = true, NameReplace = "LEVELS", RowType = typeof (LevelsTCv4Row)}},
                {"_TCv4_LEVEL_SCALING", new MapItem {IsTCv4 = true, NameReplace = "LEVELSCALING", RowType = typeof (LevelScalingTCv4Row)}},
                {"_TCv4_ROOM_INDEX", new MapItem {IsTCv4 = true, NameReplace = "LEVELS_ROOM_INDEX", RowType = typeof (LevelsRoomIndexTCv4Row)}},
                {"_TCv4_PLAYERLEVELS", new MapItem {IsTCv4 = true, RowType = typeof (PlayerLevelsTCv4Row)}},
                {"_TCv4_PROPS", new MapItem {IsTCv4 = true, RowType = typeof (LevelsRoomIndexTCv4Row)}},
                {"_TCv4_RECIPES", new MapItem {IsTCv4 = true, RowType = typeof (RecipesTCv4Row)}},
                {"_TCv4_SKILLS", new MapItem {IsTCv4 = true, RowType = typeof (SkillsTCv4Row)}},
                {"_TCv4_SKILLEVENTTYPES", new MapItem {IsTCv4 = true, RowType = typeof (SkillEventTypesTCv4Row)}},
                {"_TCv4_SKILLTABS", new MapItem {IsTCv4 = true, RowType = typeof (SkillTabsTCv4Row)}},
                {"_TCv4_SOUND_MIXSTATES", new MapItem {IsTCv4 = true, NameReplace = "SOUNDMIXSTATES", RowType = typeof (SoundMixStatesTCv4Row)}},
                {"_TCv4_SOUNDS", new MapItem {IsTCv4 = true, RowType = typeof (SoundsTCv4Row)}},
                {"_TCv4_STATS", new MapItem {IsTCv4 = true, RowType = typeof (StatsTCv4Row)}},
                {"_TCv4_TREASURE", new MapItem {IsTCv4 = true, RowType = typeof (TreasureTCv4Row)}},
                {"_TCv4_UNITMODE_GROUPS", new MapItem {IsTCv4 = true, RowType = typeof (UnitModeGroupsTCv4Row)}},
                {"_TCv4_UNITMODES", new MapItem {IsTCv4 = true, RowType = typeof (UnitModesTCv4Row)}},
                {"_TCv4_UNITTYPES", new MapItem {IsTCv4 = true, RowType = typeof (UnitTypesTCv4Row)}},
                {"_TCv4_WARDROBE_LAYER", new MapItem {IsTCv4 = true, NameReplace = "WARDROBE", RowType = typeof (WardrobeTCv4Row)}},


                //TCv4 - Same
                {"_TCv4_AFFIXTYPES", new MapItem {IsTCv4 = true, RowType = typeof (AffixTypesRow)}},
                {"_TCv4_AI_BEHAVIOR", new MapItem {IsTCv4 = true, RowType = typeof (AiBehaviourRow)}},
                {"_TCv4_AICOMMON_STATE", new MapItem {IsTCv4 = true, RowType = typeof (AiCommonStateRow)}},
                {"_TCv4_AI_INIT", new MapItem {IsTCv4 = true, RowType = typeof (AiInitRow)}},
                {"_TCv4_AI_START", new MapItem {IsTCv4 = true, RowType = typeof (AiStartRow)}},
                {"_TCv4_ANIMATION_CONDITION", new MapItem {IsTCv4 = true, RowType = typeof (AnimationConditionRow)}},
                {"_TCv4_ANIMATION_GROUP", new MapItem {IsTCv4 = true, NameReplace = "ANIMATION_GROUPS", RowType = typeof (AnimationGroupsRow)}},
                {"_TCv4_ANIMATION_STANCE", new MapItem {IsTCv4 = true, RowType = typeof (AnimationStanceRow)}},
                {"_TCv4_BACKGROUNDSOUNDS", new MapItem {IsTCv4 = true, RowType = typeof (BackGroundSoundsRow)}},
                {"_TCv4_BACKGROUNDSOUNDS3D", new MapItem {IsTCv4 = true, RowType = typeof (BackGroundSounds3DRow)}},
                {"_TCv4_BONES", new MapItem {IsTCv4 = true, RowType = typeof (BonesRow)}},
                {"_TCv4_BONEWEIGHTS", new MapItem {IsTCv4 = true, RowType = typeof (BonesRow)}},
                {"_TCv4_BOOKMARKS", new MapItem {IsTCv4 = true, RowType = typeof (BookMarksRow)}},
                {"_TCv4_BUDGETS_MODEL", new MapItem {IsTCv4 = true, RowType = typeof (BudgetsModelRow)}},
                {"_TCv4_BUDGETS_TEXTURE_MIPS", new MapItem {IsTCv4 = true, RowType = typeof (BudgetTextureMipsRow)}},
                {"_TCv4_CHAT_INSTANCED_CHANNELS", new MapItem {IsTCv4 = true, RowType = typeof (ChatInstancedChannelsRow)}},
                {"_TCv4_COLORSETS", new MapItem {IsTCv4 = true, RowType = typeof (ColorSetsRow)}},
                {"_TCv4_CONDITION_FUNCTIONS", new MapItem {IsTCv4 = true, RowType = typeof (ConditionFunctionsRow)}},
                {"_TCv4_DIALOG", new MapItem {IsTCv4 = true, RowType = typeof (DialogRow)}},
                {"_TCv4_DIFFICULTY", new MapItem {IsTCv4 = true, RowType = typeof (DifficultyRow)}},
                {"_TCv4_CHARDISPLAY", new MapItem {IsTCv4 = true, NameReplace = "DISPLAY_CHAR", RowType = typeof (DisplayRow)}},
                {"_TCv4_ITEMDISPLAY", new MapItem {IsTCv4 = true, NameReplace = "DISPLAY_ITEM", RowType = typeof (DisplayRow)}},
                {"_TCv4_EFFECTS_FILES", new MapItem {IsTCv4 = true, RowType = typeof (EffectsFilesRow)}},
                {"_TCv4_EFFECTS", new MapItem {IsTCv4 = true, NameReplace = "EFFECTS_INDEX", RowType = typeof (EffectsIndexRow)}},
                {"_TCv4_EFFECTS_SHADERS", new MapItem {IsTCv4 = true, RowType = typeof (EffectsShadersRow)}},
                {"_TCv4_EXCELTABLES", new MapItem {IsTCv4 = true, RowType = typeof (ExcelTablesRow)}},
                {"_TCv4_FACTION", new MapItem {IsTCv4 = true, RowType = typeof (FactionRow)}},
                {"_TCv4_FACTION_STANDING", new MapItem {IsTCv4 = true, RowType = typeof (FactionStandingRow)}},
                {"_TCv4_FILTER_CHATFILTER", new MapItem {IsTCv4 = true, NameReplace = "CHATFILTER", RowType = typeof (FilterRow)}},
                {"_TCv4_FILTER_NAMEFILTER", new MapItem {IsTCv4 = true, NameReplace = "NAMEFILTER", RowType = typeof (FilterRow)}},
                {"_TCv4_FONT", new MapItem {IsTCv4 = true, RowType = typeof (FontRow)}},
                {"_TCv4_FONTCOLORS", new MapItem {IsTCv4 = true, NameReplace = "FONTCOLOR", RowType = typeof (FontColorRow)}},
                {"_TCv4_FOOTSTEPS", new MapItem {IsTCv4 = true, RowType = typeof (FootStepsRow)}},
                {"_TCv4_GAME_GLOBALS", new MapItem {IsTCv4 = true, NameReplace = "GAMEGLOBALS", RowType = typeof (GameGlobalsRow)}},
                {"_TCv4_GLOBAL_INDEX", new MapItem {IsTCv4 = true, NameReplace = "GLOBALINDEX", RowType = typeof (GlobalRow)}},
                {"_TCv4_GLOBAL_STRING", new MapItem {IsTCv4 = true, NameReplace = "GLOBALSTRING", RowType = typeof (GlobalRow)}},
                {"_TCv4_GLOBAL_THEMES", new MapItem {IsTCv4 = true, RowType = typeof (GlobalThemesRow)}},
                {"_TCv4_INITDB", new MapItem {IsTCv4 = true, RowType = typeof (InitDbRow)}},
                {"_TCv4_INTERACT", new MapItem {IsTCv4 = true, RowType = typeof (InteractRow)}},
                {"_TCv4_INTERACT_MENU", new MapItem {IsTCv4 = true, RowType = typeof (InteractMenuRow)}},
                {"_TCv4_INVENTORY_TYPES", new MapItem {IsTCv4 = true, RowType = typeof (InventoryTypesRow)}},
                {"_TCv4_INVLOC", new MapItem {IsTCv4 = true, RowType = typeof (InvLocRow)}},
                {"_TCv4_ITEM_LEVELS", new MapItem {IsTCv4 = true, RowType = typeof (ItemLevelsRow)}},
                {"_TCv4_ITEM_LOOK_GROUPS", new MapItem {IsTCv4 = true, RowType = typeof (ItemLookGroupsRow)}},
                {"_TCv4_LEVEL_DRLG_CHOICE", new MapItem {IsTCv4 = true, NameReplace = "LEVELS_DRLG_CHOICE", RowType = typeof (LevelsDrlgChoiceRow)}},
                {"_TCv4_LEVEL_DRLGS", new MapItem {IsTCv4 = true, NameReplace = "LEVELS_DRLGS", RowType = typeof (LevelsDrlgsRow)}},
                {"_TCv4_LEVEL_FILE_PATHS", new MapItem {IsTCv4 = true, NameReplace = "LEVELS_FILE_PATH", RowType = typeof (LevelsFilePathRow)}},
                {"_TCv4_LEVEL_ENVIRONMENTS", new MapItem {IsTCv4 = true, NameReplace = "LEVELS_ENV", RowType = typeof (LevelsEnvRow)}},
                {"_TCv4_LEVEL_RULES", new MapItem {IsTCv4 = true, NameReplace = "LEVELS_RULES", RowType = typeof (LevelsRulesRow)}},
                {"_TCv4_LEVEL_THEMES", new MapItem {IsTCv4 = true, NameReplace = "LEVELS_THEMES", RowType = typeof (LevelsThemesRow)}},
                {"_TCv4_LOADING_TIPS", new MapItem {IsTCv4 = true, RowType = typeof (LoadingTipsRow)}},
                {"_TCv4_MATERIALS_GLOBAL", new MapItem {IsTCv4 = true, RowType = typeof (MaterialsGlobalRow)}},
                {"_TCv4_MELEEWEAPONS", new MapItem {IsTCv4 = true, RowType = typeof (MeleeWeaponsRow)}},
                {"_TCv4_MONLEVEL", new MapItem {IsTCv4 = true, RowType = typeof (MonLevelRow)}},
                {"_TCv4_MONSCALING", new MapItem {IsTCv4 = true, RowType = typeof (MonScalingRow)}},
                {"_TCv4_MONSTER_NAME_TYPES", new MapItem {IsTCv4 = true, RowType = typeof (MonsterNameTypesRow)}},
                {"_TCv4_MONSTER_NAMES", new MapItem {IsTCv4 = true, RowType = typeof (MonsterNamesRow)}},
                {"_TCv4_MONSTER_QUALITY", new MapItem {IsTCv4 = true, RowType = typeof (MonsterQualityRow)}},
                {"_TCv4_MOVIE_SUBTITLES", new MapItem {IsTCv4 = true, RowType = typeof (MovieSubTitlesRow)}},
                {"_TCv4_MOVIELISTS", new MapItem {IsTCv4 = true, RowType = typeof (MovieListsRow)}},
                {"_TCv4_MOVIES", new MapItem {IsTCv4 = true, RowType = typeof (MoviesRow)}},
                {"_TCv4_MUSIC", new MapItem {IsTCv4 = true, RowType = typeof (MusicRow)}},
                {"_TCv4_MUSICCONDITIONS", new MapItem {IsTCv4 = true, RowType = typeof (MusicConditionsRow)}},
                {"_TCv4_MUSICGROOVELEVELS", new MapItem {IsTCv4 = true, RowType = typeof (MusicGrooveLevelsRow)}},
                {"_TCv4_MUSICGROOVELEVELTYPES", new MapItem {IsTCv4 = true, RowType = typeof (MusicGrooveLevelTypesRow)}},
                {"_TCv4_MUSIC_REF", new MapItem {IsTCv4 = true, NameReplace = "MUSICREF", RowType = typeof (MusicRefRow)}},
                {"_TCv4_MUSIC_SCRIPT_DEBUG", new MapItem {IsTCv4 = true, NameReplace = "MUSICSCRIPTDEBUG", RowType = typeof (MusicScriptDebugRow)}},
                {"_TCv4_MUSICSTINGERS", new MapItem {IsTCv4 = true, RowType = typeof (MusicStingersRow)}},
                {"_TCv4_MUSICSTINGERSETS", new MapItem {IsTCv4 = true, RowType = typeof (MusicStingerSetsRow)}},
                {"_TCv4_NPC", new MapItem {IsTCv4 = true, RowType = typeof (NpcRow)}},
                {"_TCv4_OBJECTTRIGGERS", new MapItem {IsTCv4 = true, RowType = typeof (ObjectTriggersRow)}},
                {"_TCv4_OFFER", new MapItem {IsTCv4 = true, RowType = typeof (OfferRow)}},
                {"_TCv4_PALETTES", new MapItem {IsTCv4 = true, RowType = typeof (PalettesRow)}},
                {"_TCv4_PETLEVEL", new MapItem {IsTCv4 = true, RowType = typeof (MonLevelRow)}},
                {"_TCv4_PLAYER_RACE", new MapItem {IsTCv4 = true, NameReplace = "PLAYERRACE", RowType = typeof (PlayerRaceRow)}},
                {"_TCv4_PROPERTIES", new MapItem {IsTCv4 = true, RowType = typeof (PropertiesRow)}},
                {"_TCv4_PROCS", new MapItem {IsTCv4 = true, RowType = typeof (ProcsRow)}},
                {"_TCv4_QUEST", new MapItem {IsTCv4 = true, RowType = typeof (QuestRow)}},
                {"_TCv4_QUEST_CAST", new MapItem {IsTCv4 = true, RowType = typeof (QuestCastRow)}},
                {"_TCv4_QUEST_STATE", new MapItem {IsTCv4 = true, RowType = typeof (QuestStateRow)}},
                {"_TCv4_QUEST_STATE_VALUE", new MapItem {IsTCv4 = true, RowType = typeof (BookMarksRow)}},
                {"_TCv4_QUEST_STATUS", new MapItem {IsTCv4 = true, RowType = typeof (QuestStatusRow)}},
                {"_TCv4_QUEST_TEMPLATE", new MapItem {IsTCv4 = true, RowType = typeof (QuestTemplateRow)}},
                {"_TCv4_RARENAMES", new MapItem {IsTCv4 = true, RowType = typeof (RareNamesRow)}},
                {"_TCv4_RECIPELISTS", new MapItem {IsTCv4 = true, RowType = typeof (RecipeListsRow)}},
                {"_TCv4_SKILLGROUPS", new MapItem {IsTCv4 = true, RowType = typeof (SkillGroupsRow)}},
                {"_TCv4_SKU", new MapItem {IsTCv4 = true, RowType = typeof (SkuRow)}},
                {"_TCv4_SOUNDBUSES", new MapItem {IsTCv4 = true, RowType = typeof (SoundBusesRow)}},
                {"_TCv4_SOUND_MIXSTATE_VALUES", new MapItem {IsTCv4 = true, NameReplace = "SOUNDMIXSTATEVALUES", RowType = typeof (SoundMixStateValuesRow)}},
                {"_TCv4_SOUNDVCAS", new MapItem {IsTCv4 = true, RowType = typeof (SoundVidCasRow)}},
                {"_TCv4_SOUNDVCASETS", new MapItem {IsTCv4 = true, RowType = typeof (SoundVideoCasetsRow)}},
                {"_TCv4_SPAWN_CLASS", new MapItem {IsTCv4 = true, NameReplace = "SPAWNCLASS", RowType = typeof (SpawnClassRow)}},
                {"_TCv4_SUBLEVEL", new MapItem {IsTCv4 = true, RowType = typeof (SubLevelRow)}},
                {"_TCv4_STATE_EVENT_TYPES", new MapItem {IsTCv4 = true, RowType = typeof (StateEventTypesRow)}},
                {"_TCv4_STATE_LIGHTING", new MapItem {IsTCv4 = true, RowType = typeof (StateLightingRow)}},
                {"_TCv4_STATES", new MapItem {IsTCv4 = true, RowType = typeof (StatesRow)}},
                {"_TCv4_STATS_FUNC", new MapItem {IsTCv4 = true, NameReplace = "STATSFUNC", RowType = typeof (StatsFuncRow)}},
                {"_TCv4_STATS_SELECTOR", new MapItem {IsTCv4 = true, NameReplace = "STATSSELECTOR", RowType = typeof (BookMarksRow)}},
                {"_TCv4_STRING_FILES", new MapItem {IsTCv4 = true, RowType = typeof (StringFilesRow)}},
                {"_TCv4_TASK_STATUS", new MapItem {IsTCv4 = true, RowType = typeof (BookMarksRow)}},
                {"_TCv4_TAG", new MapItem {IsTCv4 = true, RowType = typeof (TagRow)}},
                {"_TCv4_TASKS", new MapItem {IsTCv4 = true, RowType = typeof (TasksRow)}},
                {"_TCv4_TEXTURE_TYPES", new MapItem {IsTCv4 = true, NameReplace = "TEXTURETYPES", RowType = typeof (TextureTypesRow)}},
                {"_TCv4_UI_COMPONENT", new MapItem {IsTCv4 = true, RowType = typeof (UIComponentRow)}},
                {"_TCv4_UNIT_EVENT_TYPES", new MapItem {IsTCv4 = true, NameReplace = "UNITEVENTS", RowType = typeof (UnitEventsRow)}},
                {"_TCv4_WARDROBE_APPEARANCE_GROUP", new MapItem {IsTCv4 = true, RowType = typeof (WardrobeAppearanceGroupRow)}},
                {"_TCv4_WARDROBE_BLENDOP", new MapItem {IsTCv4 = true, RowType = typeof (WardrobeBlendOpRow)}},
                {"_TCv4_WARDROBE_BODY", new MapItem {IsTCv4 = true, RowType = typeof (WardrobeBodyRow)}},
                {"_TCv4_WARDROBE_LAYERSET", new MapItem {IsTCv4 = true, RowType = typeof (WardrobeBlendOpRow)}},
                {"_TCv4_WARDROBE_MODEL", new MapItem {IsTCv4 = true, RowType = typeof (WardrobeModelRow)}},
                {"_TCv4_WARDROBE_MODEL_GROUP", new MapItem {IsTCv4 = true, RowType = typeof (WardrobeModelGroupRow)}},
                {"_TCv4_WARDROBE_PART", new MapItem {IsTCv4 = true, RowType = typeof (WardrobePartRow)}},
                {"_TCv4_WARDROBE_TEXTURESET", new MapItem {IsTCv4 = true, RowType = typeof (WardrobeTextureSetRow)}},
                {"_TCv4_WARDROBE_TEXTURESET_GROUP", new MapItem {IsTCv4 = true, RowType = typeof (WardrobeTextureSetGroupRow)}},
                {"_TCv4_WEATHER", new MapItem {IsTCv4 = true, RowType = typeof (WeatherRow)}},
                {"_TCv4_WEATHER_SETS", new MapItem {IsTCv4 = true, RowType = typeof (WeatherSetsRow)}},


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

        public bool LoadTableFiles(ProgressForm progress)
        {
            AllExcelFilesLoaded = true;
            AllStringsFilesLoaded = true;


            // since we're parsing them so quickly now, the progress form is actually causing a slowdow
            const int progressStepRate = 10;
            if (progress != null)
            {
                progress.ConfigBar(0, TableMap.Count, progressStepRate);
                progress.SetLoadingText("Loading Hellgate Excel Files (" + TableMap.Count + ")...");
            }


            // loop entries
            int i = 0;
            foreach (DictionaryEntry de in TableMap)
            {
                MapItem mapItem = de.Value as MapItem;
                if (mapItem == null) continue;

                // do we want to even parse it?
                if (mapItem.IsMythos || mapItem.IsEmpty || mapItem.IsTCv4) continue;


                String stringId = de.Key as String;
                String fileName = mapItem.NameReplace ?? stringId;
                String fileExtention = mapItem.RowType == typeof(StringsFile) ? StringsFile.FileExtention : ExcelFile.FileExtention;
                String folderPath = mapItem.RowType == typeof(StringsFile) ? StringsFile.FolderPath : ExcelFile.FolderPath;
                String filePath = String.Format("{0}{1}.{2}", folderPath, fileName, fileExtention).ToLower();

                Debug.Assert(stringId != null);


                // fix path.. some files belong in 'data', others in 'data_common'
                if (!_fileExplorer.GetFileExists(filePath))
                {
                    filePath = filePath.Replace("_common", "");
                    if (!_fileExplorer.GetFileExists(filePath))
                    {
                        String msg = "Excel file not found!\n\n" + filePath;
                        Debug.WriteLine(msg);

                        if (!mapItem.IsTCv4)
                        {
                            AllExcelFilesLoaded = false;
                        }

                        continue;
                    }
                }


                // update progress
                if (progress != null & i % progressStepRate == 0)
                {
                    progress.SetCurrentItemText(stringId);
                }
                i++;


                // parse file
                try
                {
                    byte[] buffer = _fileExplorer.GetFileBytes(filePath);

                    DataFile excelFile = mapItem.RowType == typeof(StringsFile) ?
                        (DataFile)new StringsFile(stringId, mapItem.RowType) :
                        (DataFile)new ExcelFile(stringId, mapItem.RowType);

                    if (excelFile.ParseData(buffer))
                    {
                        DataFiles.Add(excelFile.StringId, excelFile);
                        mapItem.LoadedFile = excelFile;
                    }

                    continue;
                }
                catch (Exception ex)
                {
                    AllExcelFilesLoaded = false;
                    AllStringsFilesLoaded = false;
                    ExceptionLogger.LogException(ex, "LoadTableFiles", true);
                }
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
            if (stringId == null) return null;

            return DataFiles[stringId] as DataFile;
        }
    }
}