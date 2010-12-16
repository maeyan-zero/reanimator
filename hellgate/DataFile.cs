using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Hellgate.Excel;
using Hellgate.Excel.TCv4;
using SkillEventTypes = Hellgate.Excel.SkillEventTypes;
using Stats = Hellgate.Excel.Stats;
using UnitModes = Hellgate.Excel.UnitModes;

namespace Hellgate
{
    public abstract class DataFile
    {
        public String StringId { get; protected set; }
        public DataFileAttributes Attributes { get; protected set; }
        public Type DataType { get { return Attributes.RowType; } }
        public bool HasIntegrity { get; protected set; }
        public bool IsExcelFile { get; protected set; }
        public bool IsStringsFile { get; protected set; }
        public int Count { get { return (Rows != null) ? Rows.Count : 0; } }
        public string FilePath { get; protected set; }
        public string FileExtension { get; set; }
        public string FileName { get { return Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(FilePath)); } }
        public List<Object> Rows { get; protected set; }

        public override string ToString()
        {
            return StringId;
        }
        public abstract bool ParseData(byte[] buffer);
        public abstract bool ParseCSV(byte[] buffer);
        public abstract bool ParseDataTable(DataTable dataTable);
        public abstract byte[] ToByteArray();
        public abstract byte[] ExportCSV();

        public class DataFileAttributes
        {
            public bool IsTCv4 { get; set; }
            public String FileName { get; set; }
            public Type RowType { get; set; }
            public bool IsEmpty { get; set; }
            public bool IsMythos { get; set; }
            public bool HasIndexBitRelations { get; set; }
            public bool HasScriptTable { get; set; }
            public bool HasExtended { get; set; }
        }

        protected static readonly Dictionary<String, DataFileAttributes> DataFileMap = new Dictionary<String, DataFileAttributes>
        {
            // Excel files
            {"ACHIEVEMENTS", new DataFileAttributes {RowType = typeof(Achievements), HasScriptTable = true}},
            {"ACT", new DataFileAttributes {RowType = typeof(Act)}},
            {"AFFIXES", new DataFileAttributes {RowType = typeof(Affixes)}},
            {"AFFIXTYPES", new DataFileAttributes {RowType = typeof(AffixTypes)}},
            {"AI_BEHAVIOR", new DataFileAttributes {RowType = typeof(AiBehaviour)}},
            {"AICOMMON_STATE", new DataFileAttributes {RowType = typeof(AiCommonState)}},
            {"AI_INIT", new DataFileAttributes {RowType = typeof(AiInit)}},
            {"AI_START", new DataFileAttributes {RowType = typeof(AiStart)}},
            {"ANIMATION_CONDITION", new DataFileAttributes {RowType = typeof(AnimationCondition)}},
            {"ANIMATION_GROUP", new DataFileAttributes {FileName = "ANIMATION_GROUPS", RowType = typeof(AnimationGroups)}},
            {"ANIMATION_STANCE", new DataFileAttributes {RowType = typeof(AnimationStance)}},
            {"BACKGROUNDSOUNDS", new DataFileAttributes {RowType = typeof(BackGroundSounds)}},
            {"BACKGROUNDSOUNDS2D", new DataFileAttributes {RowType = typeof(BackGroundSounds2D)}},
            {"BACKGROUNDSOUNDS3D", new DataFileAttributes {RowType = typeof(BackGroundSounds3D)}},
            {"BADGE_REWARDS", new DataFileAttributes {RowType = typeof(BadgeRewards)}},
            {"BONES", new DataFileAttributes {RowType = typeof(Bones)}},
            {"BONEWEIGHTS", new DataFileAttributes {RowType = typeof(Bones)}},
            {"BOOKMARKS", new DataFileAttributes {RowType = typeof(ExcelTables)}},
            {"BUDGETS_MODEL", new DataFileAttributes {RowType = typeof(BudgetsModel)}},
            {"BUDGETS_TEXTURE_MIPS", new DataFileAttributes {RowType = typeof(BudgetTextureMips)}},
            {"CHAT_INSTANCED_CHANNELS", new DataFileAttributes {RowType = typeof(ChatInstancedChannels)}},
            {"CHARACTER_CLASS", new DataFileAttributes {RowType = typeof(CharacterClass)}},
            {"COLORSETS", new DataFileAttributes {RowType = typeof(ColorSets)}},
            {"CONDITION_FUNCTIONS", new DataFileAttributes {RowType = typeof(ConditionFunctions)}},
            {"DAMAGE_EFFECTS", new DataFileAttributes {FileName = "DAMAGEEFFECTS", RowType = typeof(DamageEffects)}},
            {"DAMAGETYPES", new DataFileAttributes {RowType = typeof(DamageTypes)}},
            {"DIALOG", new DataFileAttributes {RowType = typeof(Dialog)}},
            {"DIFFICULTY", new DataFileAttributes {RowType = typeof(Difficulty)}},
            {"CHARDISPLAY", new DataFileAttributes {FileName = "DISPLAY_CHAR", RowType = typeof(Display)}},
            {"ITEMDISPLAY", new DataFileAttributes {FileName = "DISPLAY_ITEM", RowType = typeof(Display)}},
            {"EFFECTS_FILES", new DataFileAttributes {RowType = typeof(EffectsFiles)}},
            {"EFFECTS", new DataFileAttributes {FileName = "EFFECTS_INDEX", RowType = typeof(EffectsIndex)}},
            {"EFFECTS_SHADERS", new DataFileAttributes {RowType = typeof(EffectsShaders)}},
            {"EXCELTABLES", new DataFileAttributes {RowType = typeof(ExcelTables)}},
            {"FACTION", new DataFileAttributes {RowType = typeof(Faction)}},
            {"FACTION_STANDING", new DataFileAttributes {RowType = typeof(FactionStanding)}},
            {"FILTER_CHATFILTER", new DataFileAttributes {FileName = "CHATFILTER", RowType = typeof(Filter)}},
            {"FILTER_NAMEFILTER", new DataFileAttributes {FileName = "NAMEFILTER", RowType = typeof(Filter)}},
            {"FONT", new DataFileAttributes {RowType = typeof(Font)}},
            {"FONTCOLORS", new DataFileAttributes {FileName = "FONTCOLOR", RowType = typeof(FontColor)}},
            {"FOOTSTEPS", new DataFileAttributes {RowType = typeof(FootSteps)}},
            {"GAME_GLOBALS", new DataFileAttributes {FileName = "GAMEGLOBALS", RowType = typeof(GameGlobals)}},
            {"GLOBAL_INDEX", new DataFileAttributes {FileName = "GLOBALINDEX", RowType = typeof(Global)}},
            {"GLOBAL_STRING", new DataFileAttributes {FileName = "GLOBALSTRING", RowType = typeof(Global)}},
            {"GLOBAL_THEMES", new DataFileAttributes {RowType = typeof(GlobalThemes)}},
            {"INITDB", new DataFileAttributes {RowType = typeof(InitDb)}},
            {"INTERACT", new DataFileAttributes {RowType = typeof(Interact)}},
            {"INTERACT_MENU", new DataFileAttributes {RowType = typeof(InteractMenu)}},
            {"INVENTORY", new DataFileAttributes {RowType = typeof(Inventory)}},
            {"INVENTORY_TYPES", new DataFileAttributes {RowType = typeof(InventoryTypes)}},
            {"INVLOC", new DataFileAttributes {RowType = typeof(InvLoc)}},
            {"ITEM_LEVELS", new DataFileAttributes {RowType = typeof(ItemLevels)}},
            {"ITEM_LOOK_GROUPS", new DataFileAttributes {RowType = typeof(ItemLookGroups)}},
            {"ITEM_LOOKS", new DataFileAttributes {RowType = typeof(ItemLooks)}},
            {"ITEM_QUALITY", new DataFileAttributes {FileName = "ITEMQUALITY", RowType = typeof(ItemQuality)}},
            {"ITEMS", new DataFileAttributes {RowType = typeof(Items), HasExtended = true}},
            {"LEVEL", new DataFileAttributes {FileName = "LEVELS", RowType = typeof(Levels)}},
            {"LEVEL_DRLG_CHOICE", new DataFileAttributes {FileName = "LEVELS_DRLG_CHOICE", RowType = typeof(LevelsDrlgChoice)}},
            {"LEVEL_DRLGS", new DataFileAttributes {FileName = "LEVELS_DRLGS", RowType = typeof(LevelsDrlgs)}},
            {"LEVEL_FILE_PATHS", new DataFileAttributes {FileName = "LEVELS_FILE_PATH", RowType = typeof(LevelsFilePath)}},
            {"LEVEL_ENVIRONMENTS", new DataFileAttributes {FileName = "LEVELS_ENV", RowType = typeof(LevelsEnv)}},
            {"ROOM_INDEX", new DataFileAttributes {FileName = "LEVELS_ROOM_INDEX", RowType = typeof(LevelsRoomIndex)}},
            {"LEVEL_RULES", new DataFileAttributes {FileName = "LEVELS_RULES", RowType = typeof(LevelsRules)}},
            {"LEVEL_THEMES", new DataFileAttributes {FileName = "LEVELS_THEMES", RowType = typeof(LevelsThemes)}},
            {"LEVEL_SCALING", new DataFileAttributes {FileName = "LEVELSCALING", RowType = typeof(LevelScaling)}},
            {"LOADING_TIPS", new DataFileAttributes {RowType = typeof(LoadingTips)}},
            {"MATERIALS_COLLISION", new DataFileAttributes {RowType = typeof(MaterialsCollision)}},
            {"MATERIALS_GLOBAL", new DataFileAttributes {RowType = typeof(MaterialsGlobal)}},
            {"MELEEWEAPONS", new DataFileAttributes {RowType = typeof(MeleeWeapons)}},
            {"MISSILES", new DataFileAttributes {RowType = typeof(Items), HasExtended = true}},
            {"MONLEVEL", new DataFileAttributes {RowType = typeof(MonLevel), HasScriptTable = true}},
            {"MONSCALING", new DataFileAttributes {RowType = typeof(MonScaling)}},
            {"MONSTER_NAME_TYPES", new DataFileAttributes {RowType = typeof(MonsterNameTypes)}},
            {"MONSTER_NAMES", new DataFileAttributes {RowType = typeof(MonsterNames)}},
            {"MONSTER_QUALITY", new DataFileAttributes {RowType = typeof(MonsterQuality)}},
            {"MONSTERS", new DataFileAttributes {RowType = typeof(Items), HasExtended = true}},
            {"MOVIE_SUBTITLES", new DataFileAttributes {RowType = typeof(MovieSubTitles)}},
            {"MOVIELISTS", new DataFileAttributes {RowType = typeof(MovieLists)}},
            {"MOVIES", new DataFileAttributes {RowType = typeof(Movies)}},
            {"MUSIC", new DataFileAttributes {RowType = typeof(Music)}},
            {"MUSICCONDITIONS", new DataFileAttributes {RowType = typeof(MusicConditions)}},
            {"MUSICGROOVELEVELS", new DataFileAttributes {RowType = typeof(MusicGrooveLevels)}},
            {"MUSICGROOVELEVELTYPES", new DataFileAttributes {RowType = typeof(MusicGrooveLevelTypes)}},
            {"MUSIC_REF", new DataFileAttributes {FileName = "MUSICREF", RowType = typeof(MusicRef)}},
            {"MUSIC_SCRIPT_DEBUG", new DataFileAttributes {FileName = "MUSICSCRIPTDEBUG", RowType = typeof(MusicScriptDebug)}},
            {"MUSICSTINGERS", new DataFileAttributes {RowType = typeof(MusicStingers)}},
            {"MUSICSTINGERSETS", new DataFileAttributes {RowType = typeof(MusicStingerSets)}},
            {"NPC", new DataFileAttributes {RowType = typeof(Npc)}},
            {"OBJECTS", new DataFileAttributes {RowType = typeof(Items), HasExtended = true, HasScriptTable = true}},
            {"OBJECTTRIGGERS", new DataFileAttributes {RowType = typeof(ObjectTriggers)}},
            {"OFFER", new DataFileAttributes {RowType = typeof(Offer)}},
            {"PALETTES", new DataFileAttributes {RowType = typeof(Palettes)}},
            {"PETLEVEL", new DataFileAttributes {RowType = typeof(MonLevel), HasScriptTable = true}},
            {"PLAYERLEVELS", new DataFileAttributes {RowType = typeof(PlayerLevels), HasScriptTable = true}},
            {"PLAYER_RACE", new DataFileAttributes {FileName = "PLAYERRACE", RowType = typeof(PlayerRace)}},
            {"PLAYERS", new DataFileAttributes {RowType = typeof(Items), HasExtended = true}},
            {"PROPERTIES", new DataFileAttributes {RowType = typeof(Properties), HasScriptTable = true}},
            {"PROCS", new DataFileAttributes {RowType = typeof(Procs)}},
            {"PROPS", new DataFileAttributes {RowType = typeof(LevelsRoomIndex)}},
            {"QUEST", new DataFileAttributes {RowType = typeof(Quest)}},
            {"QUEST_CAST", new DataFileAttributes {RowType = typeof(QuestCast)}},
            {"QUEST_STATE", new DataFileAttributes {RowType = typeof(QuestState)}},
            {"QUEST_STATE_VALUE", new DataFileAttributes {RowType = typeof(ExcelTables)}},
            {"QUEST_STATUS", new DataFileAttributes {RowType = typeof(QuestStatus)}},
            {"QUEST_TEMPLATE", new DataFileAttributes {RowType = typeof(QuestTemplate)}},
            {"RARENAMES", new DataFileAttributes {RowType = typeof(RareNames), HasScriptTable = true}},
            {"RECIPELISTS", new DataFileAttributes {RowType = typeof(RecipeLists)}},
            {"RECIPES", new DataFileAttributes {RowType = typeof(Recipes)}},
            {"SKILLGROUPS", new DataFileAttributes {RowType = typeof(SkillGroups)}},
            {"SKILLS", new DataFileAttributes {RowType = typeof(Skills), HasScriptTable = true}},
            {"SKILLEVENTTYPES", new DataFileAttributes {RowType = typeof(SkillEventTypes)}},
            {"SKILLTABS", new DataFileAttributes {RowType = typeof(SkillTabs)}},
            {"SKU", new DataFileAttributes {RowType = typeof(Sku)}},
            {"SOUNDBUSES", new DataFileAttributes {RowType = typeof(SoundBuses)}},
            {"SOUND_MIXSTATES", new DataFileAttributes {FileName = "SOUNDMIXSTATES", RowType = typeof(SoundMixStates)}},
            {"SOUND_MIXSTATE_VALUES", new DataFileAttributes {FileName = "SOUNDMIXSTATEVALUES", RowType = typeof(SoundMixStateValues)}},
            {"SOUNDS", new DataFileAttributes {RowType = typeof(Sounds)}},
            {"SOUNDVCAS", new DataFileAttributes {RowType = typeof(SoundVidCas)}},
            {"SOUNDVCASETS", new DataFileAttributes {RowType = typeof(SoundVideoCasets)}},
            {"SPAWN_CLASS", new DataFileAttributes {FileName = "SPAWNCLASS", RowType = typeof(SpawnClass)}},
            {"SUBLEVEL", new DataFileAttributes {RowType = typeof(SubLevel)}},
            {"STATE_EVENT_TYPES", new DataFileAttributes {RowType = typeof(StateEventTypes)}},
            {"STATE_LIGHTING", new DataFileAttributes {RowType = typeof(StateLighting)}},
            {"STATES", new DataFileAttributes {RowType = typeof(States), HasIndexBitRelations = true}},
            {"STATS", new DataFileAttributes {RowType = typeof(Stats)}},
            {"STATS_FUNC", new DataFileAttributes {FileName = "STATSFUNC", RowType = typeof(StatsFunc)}},
            {"STATS_SELECTOR", new DataFileAttributes {FileName = "STATSSELECTOR", RowType = typeof(ExcelTables)}},
            {"STRING_FILES", new DataFileAttributes {RowType = typeof(StringFiles)}},
            {"TASK_STATUS", new DataFileAttributes {RowType = typeof(ExcelTables)}},
            {"TAG", new DataFileAttributes {RowType = typeof(Tag)}},
            {"TASKS", new DataFileAttributes {RowType = typeof(Tasks)}},
            {"TEXTURE_TYPES", new DataFileAttributes {FileName = "TEXTURETYPES", RowType = typeof(TextureTypes)}},
            {"TREASURE", new DataFileAttributes {RowType = typeof(Treasure)}},
            {"UI_COMPONENT", new DataFileAttributes {RowType = typeof(UIComponent)}},
            {"UNIT_EVENT_TYPES", new DataFileAttributes {FileName = "UNITEVENTS", RowType = typeof(UnitEvents)}},
            {"UNITMODE_GROUPS", new DataFileAttributes {RowType = typeof(UnitModeGroups)}},
            {"UNITMODES", new DataFileAttributes {RowType = typeof(UnitModes)}},
            {"UNITTYPES", new DataFileAttributes {RowType = typeof(UnitTypes), HasIndexBitRelations = true}},
            {"WARDROBE_LAYER", new DataFileAttributes {FileName = "WARDROBE", RowType = typeof(Wardrobe)}},
            {"WARDROBE_APPEARANCE_GROUP", new DataFileAttributes {RowType = typeof(WardrobeAppearanceGroup)}},
            {"WARDROBE_BLENDOP", new DataFileAttributes {RowType = typeof(WardrobeBlendOp)}},
            {"WARDROBE_BODY", new DataFileAttributes {RowType = typeof(WardrobeBody)}},
            {"WARDROBE_LAYERSET", new DataFileAttributes {RowType = typeof(WardrobeBlendOp)}},
            {"WARDROBE_MODEL", new DataFileAttributes {RowType = typeof(WardrobeModel)}},
            {"WARDROBE_MODEL_GROUP", new DataFileAttributes {RowType = typeof(WardrobeModelGroup)}},
            {"WARDROBE_PART", new DataFileAttributes {RowType = typeof(WardrobePart)}},
            {"WARDROBE_TEXTURESET", new DataFileAttributes {RowType = typeof(WardrobeTextureSet)}},
            {"WARDROBE_TEXTURESET_GROUP", new DataFileAttributes {RowType = typeof(WardrobeTextureSetGroup)}},
            {"WEATHER", new DataFileAttributes {RowType = typeof(Weather)}},
            {"WEATHER_SETS", new DataFileAttributes {RowType = typeof(WeatherSets)}},

                
            // TCv4 - different
            {"_TCv4_ACHIEVEMENTS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(AchievementsTCv4)}},
            {"_TCv4_ACT", new DataFileAttributes {IsTCv4 = true, RowType = typeof(ActTCv4)}},
            {"_TCv4_AFFIXES", new DataFileAttributes {IsTCv4 = true, RowType = typeof(AffixesTCv4)}},
            {"_TCv4_BACKGROUNDSOUNDS2D", new DataFileAttributes {IsTCv4 = true, RowType = typeof(BackGroundSounds2DTCv4)}},
            {"_TCv4_BADGE_REWARDS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(BadgeRewardsTCv4)}},
            {"_TCv4_CHARACTER_CLASS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(CharacterClassTCv4)}},
            {"_TCv4_DAMAGE_EFFECTS", new DataFileAttributes {IsTCv4 = true, FileName = "DAMAGEEFFECTS", RowType = typeof(DamageEffectsTCv4)}},
            {"_TCv4_DAMAGETYPES", new DataFileAttributes {IsTCv4 = true, RowType = typeof(DamageTypesTCv4)}},
            {"_TCv4_INVENTORY", new DataFileAttributes {IsTCv4 = true, RowType = typeof(InventoryTCv4)}},
            {"_TCv4_ITEM_LOOKS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(ItemLooksTCv4)}},
            {"_TCv4_ITEM_QUALITY", new DataFileAttributes {IsTCv4 = true, FileName = "ITEMQUALITY", RowType = typeof(ItemQualityTCv4)}},
            {"_TCv4_ITEMS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(ItemsTCv4)}},
            {"_TCv4_OBJECTS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(ItemsTCv4)}},
            {"_TCv4_MONSTERS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(ItemsTCv4)}},
            {"_TCv4_PLAYERS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(ItemsTCv4)}},
            {"_TCv4_MISSILES", new DataFileAttributes {IsTCv4 = true, RowType = typeof(ItemsTCv4)}},
            {"_TCv4_MATERIALS_COLLISION", new DataFileAttributes {IsTCv4 = true, RowType = typeof(MaterialsCollisionTCv4)}},
            {"_TCv4_LEVEL", new DataFileAttributes {IsTCv4 = true, FileName = "LEVELS", RowType = typeof(LevelsTCv4)}},
            {"_TCv4_LEVEL_SCALING", new DataFileAttributes {IsTCv4 = true, FileName = "LEVELSCALING", RowType = typeof(LevelScalingTCv4)}},
            {"_TCv4_ROOM_INDEX", new DataFileAttributes {IsTCv4 = true, FileName = "LEVELS_ROOM_INDEX", RowType = typeof(LevelsRoomIndexTCv4)}},
            {"_TCv4_PLAYERLEVELS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(PlayerLevelsTCv4)}},
            {"_TCv4_PROPS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(LevelsRoomIndexTCv4)}},
            {"_TCv4_RECIPES", new DataFileAttributes {IsTCv4 = true, RowType = typeof(RecipesTCv4)}},
            {"_TCv4_SKILLS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(SkillsTCv4)}},
            {"_TCv4_SKILLEVENTTYPES", new DataFileAttributes {IsTCv4 = true, RowType = typeof(SkillEventTypesTCv4)}},
            {"_TCv4_SKILLTABS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(SkillTabsTCv4)}},
            {"_TCv4_SOUND_MIXSTATES", new DataFileAttributes {IsTCv4 = true, FileName = "SOUNDMIXSTATES", RowType = typeof(SoundMixStatesTCv4)}},
            {"_TCv4_SOUNDS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(SoundsTCv4)}},
            {"_TCv4_STATS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(StatsTCv4)}},
            {"_TCv4_TREASURE", new DataFileAttributes {IsTCv4 = true, RowType = typeof(TreasureTCv4)}},
            {"_TCv4_UNITMODE_GROUPS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(UnitModeGroupsTCv4)}},
            {"_TCv4_UNITMODES", new DataFileAttributes {IsTCv4 = true, RowType = typeof(UnitModesTCv4)}},
            {"_TCv4_UNITTYPES", new DataFileAttributes {IsTCv4 = true, RowType = typeof(UnitTypesTCv4), HasIndexBitRelations = true}},
            {"_TCv4_WARDROBE_LAYER", new DataFileAttributes {IsTCv4 = true, FileName = "WARDROBE", RowType = typeof(WardrobeTCv4)}},


            // TCv4 - No Definition
            {"_TCv4_ACHIEVEMENTSLOTS", new DataFileAttributes {IsTCv4 = true, IsEmpty = true}}, // todo: no definition
            {"_TCv4_AFFIX_GROUPS", new DataFileAttributes {IsTCv4 = true, IsEmpty = true}}, // todo: no definition
            {"_TCv4_DONATION_REWARDS", new DataFileAttributes {IsTCv4 = true, IsEmpty = true}}, // todo: no definition
            {"_TCv4_LOADING_SCREEN", new DataFileAttributes {IsTCv4 = true, IsEmpty = true}}, // todo: no definition
            {"_TCv4_PLAYERRANKS", new DataFileAttributes {IsTCv4 = true, IsEmpty = true}}, // todo: no definition
            {"_TCv4_SOUNDMIXSTATESETS", new DataFileAttributes {IsTCv4 = true, IsEmpty = true}}, // todo: no definition
            {"_TCv4_VERSIONINGAFFIXES", new DataFileAttributes {IsTCv4 = true, IsEmpty = true}}, // todo: no definition
            {"_TCv4_VERSIONINGUNITS", new DataFileAttributes {IsTCv4 = true, IsEmpty = true}}, // todo: no definition
            {"_TCv4_CMD_MENUS", new DataFileAttributes {IsTCv4 = true, IsEmpty = true}}, // todo: no definition
            {"_TCv4_EMOTES", new DataFileAttributes {IsTCv4 = true, IsEmpty = true}}, // todo: no definition
            {"_TCv4_RECIPE_PROPERTIES", new DataFileAttributes {IsTCv4 = true, IsEmpty = true}}, // todo: no definition



            // TCv4 - Same
            {"_TCv4_AFFIXTYPES", new DataFileAttributes {IsTCv4 = true, RowType = typeof(AffixTypes)}},
            {"_TCv4_AI_BEHAVIOR", new DataFileAttributes {IsTCv4 = true, RowType = typeof(AiBehaviour)}},
            {"_TCv4_AICOMMON_STATE", new DataFileAttributes {IsTCv4 = true, RowType = typeof(AiCommonState)}},
            {"_TCv4_AI_INIT", new DataFileAttributes {IsTCv4 = true, RowType = typeof(AiInit)}},
            {"_TCv4_AI_START", new DataFileAttributes {IsTCv4 = true, RowType = typeof(AiStart)}},
            {"_TCv4_ANIMATION_CONDITION", new DataFileAttributes {IsTCv4 = true, RowType = typeof(AnimationCondition)}},
            {"_TCv4_ANIMATION_GROUP", new DataFileAttributes {IsTCv4 = true, FileName = "ANIMATION_GROUPS", RowType = typeof(AnimationGroups)}},
            {"_TCv4_ANIMATION_STANCE", new DataFileAttributes {IsTCv4 = true, RowType = typeof(AnimationStance)}},
            {"_TCv4_BACKGROUNDSOUNDS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(BackGroundSounds)}},
            {"_TCv4_BACKGROUNDSOUNDS3D", new DataFileAttributes {IsTCv4 = true, RowType = typeof(BackGroundSounds3D)}},
            {"_TCv4_BONES", new DataFileAttributes {IsTCv4 = true, RowType = typeof(Bones)}},
            {"_TCv4_BONEWEIGHTS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(Bones)}},
            {"_TCv4_BOOKMARKS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(ExcelTables)}},
            {"_TCv4_BUDGETS_MODEL", new DataFileAttributes {IsTCv4 = true, RowType = typeof(BudgetsModel)}},
            {"_TCv4_BUDGETS_TEXTURE_MIPS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(BudgetTextureMips)}},
            {"_TCv4_CHAT_INSTANCED_CHANNELS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(ChatInstancedChannels)}},
            {"_TCv4_COLORSETS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(ColorSets)}},
            {"_TCv4_CONDITION_FUNCTIONS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(ConditionFunctions)}},
            {"_TCv4_DIALOG", new DataFileAttributes {IsTCv4 = true, RowType = typeof(Dialog)}},
            {"_TCv4_DIFFICULTY", new DataFileAttributes {IsTCv4 = true, RowType = typeof(Difficulty)}},
            {"_TCv4_CHARDISPLAY", new DataFileAttributes {IsTCv4 = true, FileName = "DISPLAY_CHAR", RowType = typeof(Display)}},
            {"_TCv4_ITEMDISPLAY", new DataFileAttributes {IsTCv4 = true, FileName = "DISPLAY_ITEM", RowType = typeof(Display)}},
            {"_TCv4_EFFECTS_FILES", new DataFileAttributes {IsTCv4 = true, RowType = typeof(EffectsFiles)}},
            {"_TCv4_EFFECTS", new DataFileAttributes {IsTCv4 = true, FileName = "EFFECTS_INDEX", RowType = typeof(EffectsIndex)}},
            {"_TCv4_EFFECTS_SHADERS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(EffectsShaders)}},
            {"_TCv4_EXCELTABLES", new DataFileAttributes {IsTCv4 = true, RowType = typeof(ExcelTables)}},
            {"_TCv4_FACTION", new DataFileAttributes {IsTCv4 = true, RowType = typeof(Faction)}},
            {"_TCv4_FACTION_STANDING", new DataFileAttributes {IsTCv4 = true, RowType = typeof(FactionStanding)}},
            {"_TCv4_FILTER_CHATFILTER", new DataFileAttributes {IsTCv4 = true, FileName = "CHATFILTER", RowType = typeof(Filter)}},
            {"_TCv4_FILTER_NAMEFILTER", new DataFileAttributes {IsTCv4 = true, FileName = "NAMEFILTER", RowType = typeof(Filter)}},
            {"_TCv4_FONT", new DataFileAttributes {IsTCv4 = true, RowType = typeof(Font)}},
            {"_TCv4_FONTCOLORS", new DataFileAttributes {IsTCv4 = true, FileName = "FONTCOLOR", RowType = typeof(FontColor)}},
            {"_TCv4_FOOTSTEPS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(FootSteps)}},
            {"_TCv4_GAME_GLOBALS", new DataFileAttributes {IsTCv4 = true, FileName = "GAMEGLOBALS", RowType = typeof(GameGlobals)}},
            {"_TCv4_GLOBAL_INDEX", new DataFileAttributes {IsTCv4 = true, FileName = "GLOBALINDEX", RowType = typeof(Global)}},
            {"_TCv4_GLOBAL_STRING", new DataFileAttributes {IsTCv4 = true, FileName = "GLOBALSTRING", RowType = typeof(Global)}},
            {"_TCv4_GLOBAL_THEMES", new DataFileAttributes {IsTCv4 = true, RowType = typeof(GlobalThemes)}},
            {"_TCv4_INITDB", new DataFileAttributes {IsTCv4 = true, RowType = typeof(InitDb)}},
            {"_TCv4_INTERACT", new DataFileAttributes {IsTCv4 = true, RowType = typeof(Interact)}},
            {"_TCv4_INTERACT_MENU", new DataFileAttributes {IsTCv4 = true, RowType = typeof(InteractMenu)}},
            {"_TCv4_INVENTORY_TYPES", new DataFileAttributes {IsTCv4 = true, RowType = typeof(InventoryTypes)}},
            {"_TCv4_INVLOC", new DataFileAttributes {IsTCv4 = true, RowType = typeof(InvLoc)}},
            {"_TCv4_ITEM_LEVELS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(ItemLevels)}},
            {"_TCv4_ITEM_LOOK_GROUPS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(ItemLookGroups)}},
            {"_TCv4_LEVEL_DRLG_CHOICE", new DataFileAttributes {IsTCv4 = true, FileName = "LEVELS_DRLG_CHOICE", RowType = typeof(LevelsDrlgChoice)}},
            {"_TCv4_LEVEL_DRLGS", new DataFileAttributes {IsTCv4 = true, FileName = "LEVELS_DRLGS", RowType = typeof(LevelsDrlgs)}},
            {"_TCv4_LEVEL_FILE_PATHS", new DataFileAttributes {IsTCv4 = true, FileName = "LEVELS_FILE_PATH", RowType = typeof(LevelsFilePath)}},
            {"_TCv4_LEVEL_ENVIRONMENTS", new DataFileAttributes {IsTCv4 = true, FileName = "LEVELS_ENV", RowType = typeof(LevelsEnv)}},
            {"_TCv4_LEVEL_RULES", new DataFileAttributes {IsTCv4 = true, FileName = "LEVELS_RULES", RowType = typeof(LevelsRules)}},
            {"_TCv4_LEVEL_THEMES", new DataFileAttributes {IsTCv4 = true, FileName = "LEVELS_THEMES", RowType = typeof(LevelsThemes)}},
            {"_TCv4_LOADING_TIPS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(LoadingTips)}},
            {"_TCv4_MATERIALS_GLOBAL", new DataFileAttributes {IsTCv4 = true, RowType = typeof(MaterialsGlobal)}},
            {"_TCv4_MELEEWEAPONS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(MeleeWeapons)}},
            {"_TCv4_MONLEVEL", new DataFileAttributes {IsTCv4 = true, RowType = typeof(MonLevel)}},
            {"_TCv4_MONSCALING", new DataFileAttributes {IsTCv4 = true, RowType = typeof(MonScaling)}},
            {"_TCv4_MONSTER_NAME_TYPES", new DataFileAttributes {IsTCv4 = true, RowType = typeof(MonsterNameTypes)}},
            {"_TCv4_MONSTER_NAMES", new DataFileAttributes {IsTCv4 = true, RowType = typeof(MonsterNames)}},
            {"_TCv4_MONSTER_QUALITY", new DataFileAttributes {IsTCv4 = true, RowType = typeof(MonsterQuality)}},
            {"_TCv4_MOVIE_SUBTITLES", new DataFileAttributes {IsTCv4 = true, RowType = typeof(MovieSubTitles)}},
            {"_TCv4_MOVIELISTS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(MovieLists)}},
            {"_TCv4_MOVIES", new DataFileAttributes {IsTCv4 = true, RowType = typeof(Movies)}},
            {"_TCv4_MUSIC", new DataFileAttributes {IsTCv4 = true, RowType = typeof(Music)}},
            {"_TCv4_MUSICCONDITIONS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(MusicConditions)}},
            {"_TCv4_MUSICGROOVELEVELS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(MusicGrooveLevels)}},
            {"_TCv4_MUSICGROOVELEVELTYPES", new DataFileAttributes {IsTCv4 = true, RowType = typeof(MusicGrooveLevelTypes)}},
            {"_TCv4_MUSIC_REF", new DataFileAttributes {IsTCv4 = true, FileName = "MUSICREF", RowType = typeof(MusicRef)}},
            {"_TCv4_MUSIC_SCRIPT_DEBUG", new DataFileAttributes {IsTCv4 = true, FileName = "MUSICSCRIPTDEBUG", RowType = typeof(MusicScriptDebug)}},
            {"_TCv4_MUSICSTINGERS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(MusicStingers)}},
            {"_TCv4_MUSICSTINGERSETS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(MusicStingerSets)}},
            {"_TCv4_NPC", new DataFileAttributes {IsTCv4 = true, RowType = typeof(Npc)}},
            {"_TCv4_OBJECTTRIGGERS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(ObjectTriggers)}},
            {"_TCv4_OFFER", new DataFileAttributes {IsTCv4 = true, RowType = typeof(Offer)}},
            {"_TCv4_PALETTES", new DataFileAttributes {IsTCv4 = true, RowType = typeof(Palettes)}},
            {"_TCv4_PETLEVEL", new DataFileAttributes {IsTCv4 = true, RowType = typeof(MonLevel)}},
            {"_TCv4_PLAYER_RACE", new DataFileAttributes {IsTCv4 = true, FileName = "PLAYERRACE", RowType = typeof(PlayerRace)}},
            {"_TCv4_PROPERTIES", new DataFileAttributes {IsTCv4 = true, RowType = typeof(Properties), HasScriptTable = true}},
            {"_TCv4_PROCS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(Procs)}},
            {"_TCv4_QUEST", new DataFileAttributes {IsTCv4 = true, RowType = typeof(Quest)}},
            {"_TCv4_QUEST_CAST", new DataFileAttributes {IsTCv4 = true, RowType = typeof(QuestCast)}},
            {"_TCv4_QUEST_STATE", new DataFileAttributes {IsTCv4 = true, RowType = typeof(QuestState)}},
            {"_TCv4_QUEST_STATE_VALUE", new DataFileAttributes {IsTCv4 = true, RowType = typeof(ExcelTables)}},
            {"_TCv4_QUEST_STATUS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(QuestStatus)}},
            {"_TCv4_QUEST_TEMPLATE", new DataFileAttributes {IsTCv4 = true, RowType = typeof(QuestTemplate)}},
            {"_TCv4_RARENAMES", new DataFileAttributes {IsTCv4 = true, RowType = typeof(RareNames)}},
            {"_TCv4_RECIPELISTS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(RecipeLists)}},
            {"_TCv4_SKILLGROUPS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(SkillGroups)}},
            {"_TCv4_SKU", new DataFileAttributes {IsTCv4 = true, RowType = typeof(Sku)}},
            {"_TCv4_SOUNDBUSES", new DataFileAttributes {IsTCv4 = true, RowType = typeof(SoundBuses)}},
            {"_TCv4_SOUND_MIXSTATE_VALUES", new DataFileAttributes {IsTCv4 = true, FileName = "SOUNDMIXSTATEVALUES", RowType = typeof(SoundMixStateValues)}},
            {"_TCv4_SOUNDVCAS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(SoundVidCas)}},
            {"_TCv4_SOUNDVCASETS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(SoundVideoCasets)}},
            {"_TCv4_SPAWN_CLASS", new DataFileAttributes {IsTCv4 = true, FileName = "SPAWNCLASS", RowType = typeof(SpawnClass)}},
            {"_TCv4_SUBLEVEL", new DataFileAttributes {IsTCv4 = true, RowType = typeof(SubLevel)}},
            {"_TCv4_STATE_EVENT_TYPES", new DataFileAttributes {IsTCv4 = true, RowType = typeof(StateEventTypes)}},
            {"_TCv4_STATE_LIGHTING", new DataFileAttributes {IsTCv4 = true, RowType = typeof(StateLighting)}},
            {"_TCv4_STATES", new DataFileAttributes {IsTCv4 = true, RowType = typeof(States), HasIndexBitRelations = true}},
            {"_TCv4_STATS_FUNC", new DataFileAttributes {IsTCv4 = true, FileName = "STATSFUNC", RowType = typeof(StatsFunc)}},
            {"_TCv4_STATS_SELECTOR", new DataFileAttributes {IsTCv4 = true, FileName = "STATSSELECTOR", RowType = typeof(ExcelTables)}},
            {"_TCv4_STRING_FILES", new DataFileAttributes {IsTCv4 = true, RowType = typeof(StringFiles)}},
            {"_TCv4_TASK_STATUS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(ExcelTables)}},
            {"_TCv4_TAG", new DataFileAttributes {IsTCv4 = true, RowType = typeof(Tag)}},
            {"_TCv4_TASKS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(Tasks)}},
            {"_TCv4_TEXTURE_TYPES", new DataFileAttributes {IsTCv4 = true, FileName = "TEXTURETYPES", RowType = typeof(TextureTypes)}},
            {"_TCv4_UI_COMPONENT", new DataFileAttributes {IsTCv4 = true, RowType = typeof(UIComponent)}},
            {"_TCv4_UNIT_EVENT_TYPES", new DataFileAttributes {IsTCv4 = true, FileName = "UNITEVENTS", RowType = typeof(UnitEvents)}},
            {"_TCv4_WARDROBE_APPEARANCE_GROUP", new DataFileAttributes {IsTCv4 = true, RowType = typeof(WardrobeAppearanceGroup)}},
            {"_TCv4_WARDROBE_BLENDOP", new DataFileAttributes {IsTCv4 = true, RowType = typeof(WardrobeBlendOp)}},
            {"_TCv4_WARDROBE_BODY", new DataFileAttributes {IsTCv4 = true, RowType = typeof(WardrobeBody)}},
            {"_TCv4_WARDROBE_LAYERSET", new DataFileAttributes {IsTCv4 = true, RowType = typeof(WardrobeBlendOp)}},
            {"_TCv4_WARDROBE_MODEL", new DataFileAttributes {IsTCv4 = true, RowType = typeof(WardrobeModel)}},
            {"_TCv4_WARDROBE_MODEL_GROUP", new DataFileAttributes {IsTCv4 = true, RowType = typeof(WardrobeModelGroup)}},
            {"_TCv4_WARDROBE_PART", new DataFileAttributes {IsTCv4 = true, RowType = typeof(WardrobePart)}},
            {"_TCv4_WARDROBE_TEXTURESET", new DataFileAttributes {IsTCv4 = true, RowType = typeof(WardrobeTextureSet)}},
            {"_TCv4_WARDROBE_TEXTURESET_GROUP", new DataFileAttributes {IsTCv4 = true, RowType = typeof(WardrobeTextureSetGroup)}},
            {"_TCv4_WEATHER", new DataFileAttributes {IsTCv4 = true, RowType = typeof(Weather)}},
            {"_TCv4_WEATHER_SETS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(WeatherSets)}},


            // Empty Excel files
            {"GOSSIP", new DataFileAttributes {IsEmpty = true}},
            {"INVLOCIDX", new DataFileAttributes {IsEmpty = true}},
            {"SOUNDOVERRIDES", new DataFileAttributes {IsEmpty = true}},


            // TCv4 - Empty Excel files
            {"_TCv4_GOSSIP", new DataFileAttributes {IsEmpty = true}},
            {"_TCv4_SOUNDOVERRIDES", new DataFileAttributes {IsEmpty = true}},


            // Non-Indexed Excel file
            {"LANGUAGE", new DataFileAttributes {RowType = typeof(Language)}},
            {"REGION", new DataFileAttributes {RowType = typeof(Region)}},


            // Mythos Excel files
            {"LEVEL_AREAS", new DataFileAttributes {IsMythos = true}},
            {"LEVEL_AREAS_ADJ_BRIGHT", new DataFileAttributes {IsMythos = true}},
            {"LEVEL_AREAS_ADJECTIVES", new DataFileAttributes {IsMythos = true}},
            {"LEVEL_AREAS_AFFIXS", new DataFileAttributes {IsMythos = true}},
            {"LEVEL_AREAS_CANYON_NOUNS", new DataFileAttributes {IsMythos = true}},
            {"LEVEL_AREAS_CAVE_NOUNS", new DataFileAttributes {IsMythos = true}},
            {"LEVEL_AREAS_DESERTGOTHIC_NOUNS", new DataFileAttributes {IsMythos = true}},
            {"LEVEL_AREAS_GOBLIN_NAMES", new DataFileAttributes {IsMythos = true}},
            {"LEVEL_AREAS_GOBLIN_NOUNS", new DataFileAttributes {IsMythos = true}},
            {"LEVEL_AREAS_GOTHIC_NOUNS", new DataFileAttributes {IsMythos = true}},
            {"LEVEL_AREAS_FARMLAND_NOUNS", new DataFileAttributes {IsMythos = true}},
            {"LEVEL_AREAS_FOREST_NOUNS", new DataFileAttributes {IsMythos = true}},
            {"LEVEL_AREAS_HEATH_NOUNS", new DataFileAttributes {IsMythos = true}},
            {"LEVEL_AREAS_LINKER", new DataFileAttributes {IsMythos = true}},
            {"LEVEL_AREAS_MADLIB", new DataFileAttributes {IsMythos = true}},
            {"LEVEL_AREAS_PROPERNAMEZONE1", new DataFileAttributes {IsMythos = true}},
            {"LEVEL_AREAS_PROPERNAMEZONE2", new DataFileAttributes {IsMythos = true}},
            {"LEVEL_AREAS_TEMPLE_NOUNS", new DataFileAttributes {IsMythos = true}},
            {"LEVEL_AREAS_SUFFIXS", new DataFileAttributes {IsMythos = true}},
            {"LEVEL_ZONES", new DataFileAttributes {IsMythos = true}},
            {"QUEST_COUNT_TUGBOAT", new DataFileAttributes {IsMythos = true}},
            {"QUEST_DICTIONARY_FOR_TUGBOAT", new DataFileAttributes {IsMythos = true}},
            {"QUEST_RANDOM_FOR_TUGBOAT", new DataFileAttributes {IsMythos = true}},
            {"QUEST_RANDOM_TASKS_FOR_TUGBOAT", new DataFileAttributes {IsMythos = true}},
            {"QUEST_REWARD_BY_LEVEL_TUGBOAT", new DataFileAttributes {IsMythos = true}},
            {"QUEST_TITLES_FOR_TUGBOAT", new DataFileAttributes {IsMythos = true}},
            {"QUESTS_TASKS_FOR_TUGBOAT", new DataFileAttributes {IsMythos = true}},


            // I think these are Mythos
            {"ACHIEVEMENT_SLOTS", new DataFileAttributes {IsMythos = true}},
            {"CRAFTING_SLOTS", new DataFileAttributes {IsMythos = true}},
            {"DEBUG_BARS", new DataFileAttributes {IsMythos = true}},
            {"RENDER_FLAGS", new DataFileAttributes {IsMythos = true}},
            {"SKILL_LEVELS", new DataFileAttributes {IsMythos = true}},
            {"SKILL_STATS", new DataFileAttributes {IsMythos = true}},


            // Strings files
            {"Strings_Affix", new DataFileAttributes {RowType = typeof(StringsFile)}},
            {"Strings_Cinematic", new DataFileAttributes {RowType = typeof(StringsFile)}},
            {"Strings_Common", new DataFileAttributes {RowType = typeof(StringsFile)}},
            {"Strings_Credits", new DataFileAttributes {RowType = typeof(StringsFile)}},
            {"Strings_DisplayFormat", new DataFileAttributes {RowType = typeof(StringsFile)}},
            {"Strings_Install", new DataFileAttributes {RowType = typeof(StringsFile)}},
            {"Strings_Items", new DataFileAttributes {RowType = typeof(StringsFile)}},
            {"Strings_Level", new DataFileAttributes {RowType = typeof(StringsFile)}},
            {"Strings_LoadingTips", new DataFileAttributes {RowType = typeof(StringsFile)}},
            {"Strings_Monsters", new DataFileAttributes {RowType = typeof(StringsFile)}},
            {"Strings_Names", new DataFileAttributes {RowType = typeof(StringsFile)}},
            {"Strings_Quest", new DataFileAttributes {RowType = typeof(StringsFile)}},
            {"Strings_Skills", new DataFileAttributes {RowType = typeof(StringsFile)}},
            {"Strings_Strings", new DataFileAttributes {RowType = typeof(StringsFile)}},
            {"Strings_Revival", new DataFileAttributes {RowType = typeof(StringsFile)}},


            // Empty Strings files
            {"Strings_Install_MSI", new DataFileAttributes {IsEmpty = true}}
        };
    }
}