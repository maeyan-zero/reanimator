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
        //public abstract byte[] ExportSQL();

        public class DataFileAttributes
        {
            public bool IsTCv4;
            public String FileName;
            public Type RowType;
            public bool IsEmpty;
            public bool IsMythos;
            public bool HasIndexBitRelations;
            public bool HasScriptTable;
            public bool HasExtended;
            public UInt32 StructureId;
        }

        public static readonly Dictionary<String, DataFileAttributes> DataFileMap = new Dictionary<String, DataFileAttributes>
        {
            // Excel files
            {"ACHIEVEMENTS", new DataFileAttributes {RowType = typeof(Achievements), HasScriptTable = true, StructureId = 0x62ECA6E1}},
            {"ACT", new DataFileAttributes {RowType = typeof(Act), StructureId = 0xBB554372}},
            {"AFFIXES", new DataFileAttributes {RowType = typeof(Affixes), StructureId = 0x9DF76E6C}},
            {"AFFIXTYPES", new DataFileAttributes {RowType = typeof(AffixTypes), StructureId = 0x106C109C}},
            {"AI_BEHAVIOR", new DataFileAttributes {RowType = typeof(AiBehaviour), StructureId = 0x8A5FF6B8}},
            {"AI_INIT", new DataFileAttributes {RowType = typeof(AiInit), StructureId = 0x7F15F865}},
            {"AI_START", new DataFileAttributes {RowType = typeof(AiStart), StructureId = 0x102ECE59}},
            {"AICOMMON_STATE", new DataFileAttributes {RowType = typeof(AiCommonState), StructureId = 0xAFBF5906}},
            {"ANIMATION_CONDITION", new DataFileAttributes {RowType = typeof(AnimationCondition), StructureId = 0xD59E46B8}},
            {"ANIMATION_GROUP", new DataFileAttributes {FileName = "ANIMATION_GROUPS", RowType = typeof(AnimationGroups), StructureId = 0x03407879}},
            {"ANIMATION_STANCE", new DataFileAttributes {RowType = typeof(AnimationStance), StructureId = 0x45B08A9E}},
            {"BACKGROUNDSOUNDS", new DataFileAttributes {RowType = typeof(BackGroundSounds), StructureId = 0xD783DCDA}},
            {"BACKGROUNDSOUNDS2D", new DataFileAttributes {RowType = typeof(BackGroundSounds2D), StructureId = 0xB0DA4BE1}},
            {"BACKGROUNDSOUNDS3D", new DataFileAttributes {RowType = typeof(BackGroundSounds3D), StructureId = 0x63F90CA5}},
            {"BADGE_REWARDS", new DataFileAttributes {RowType = typeof(BadgeRewards), StructureId = 0xAA0F158C}},
            {"BONES", new DataFileAttributes {RowType = typeof(Bones), StructureId = 0x1EE32EF6}},
            {"BONEWEIGHTS", new DataFileAttributes {RowType = typeof(Bones), StructureId = 0x1EE32EF6}},
            {"BOOKMARKS", new DataFileAttributes {RowType = typeof(ExcelTables), StructureId = 0x86DC367C}},
            {"BUDGETS_MODEL", new DataFileAttributes {RowType = typeof(BudgetsModel), StructureId = 0x7A7D891E}},
            {"BUDGETS_TEXTURE_MIPS", new DataFileAttributes {RowType = typeof(BudgetTextureMips), StructureId = 0xBEE975EA}},
            {"CHARACTER_CLASS", new DataFileAttributes {RowType = typeof(CharacterClass), StructureId = 0x08402828}},
            {"CHAT_INSTANCED_CHANNELS", new DataFileAttributes {RowType = typeof(ChatInstancedChannels), StructureId = 0xDC35E3D0}},
            {"COLORSETS", new DataFileAttributes {RowType = typeof(ColorSets), StructureId = 0xBBC15A50}},
            {"CONDITION_FUNCTIONS", new DataFileAttributes {RowType = typeof(ConditionFunctions), StructureId = 0x8B84B802}},
            {"DAMAGE_EFFECTS", new DataFileAttributes {FileName = "DAMAGEEFFECTS", RowType = typeof(DamageEffects), StructureId = 0x26BC8A8D}},
            {"DAMAGETYPES", new DataFileAttributes {RowType = typeof(DamageTypes), StructureId = 0xEAC1CAA4}},
            {"DIALOG", new DataFileAttributes {RowType = typeof(Dialog), StructureId = 0xAF168F4E}},
            {"DIFFICULTY", new DataFileAttributes {RowType = typeof(Difficulty), StructureId = 0x5C719A50}},
            {"CHARDISPLAY", new DataFileAttributes {FileName = "DISPLAY_CHAR", RowType = typeof(Display), StructureId = 0x4319D23D}},
            {"ITEMDISPLAY", new DataFileAttributes {FileName = "DISPLAY_ITEM", RowType = typeof(Display), StructureId = 0x4319D23D}},
            {"EXCELTABLES", new DataFileAttributes {RowType = typeof(ExcelTables), StructureId = 0x86DC367C}},
            {"EFFECTS_FILES", new DataFileAttributes {RowType = typeof(EffectsFiles), StructureId = 0xF303187F}},
            {"EFFECTS", new DataFileAttributes {FileName = "EFFECTS_INDEX", RowType = typeof(EffectsIndex), StructureId = 0x99264BCB}},
            {"EFFECTS_SHADERS", new DataFileAttributes {RowType = typeof(EffectsShaders), StructureId = 0xC8471612}},
            {"FACTION", new DataFileAttributes {RowType = typeof(Faction), StructureId = 0xB7BB74D1}},
            {"FACTION_STANDING", new DataFileAttributes {RowType = typeof(FactionStanding), StructureId = 0x01A80106}},
            {"FILTER_CHATFILTER", new DataFileAttributes {FileName = "CHATFILTER", RowType = typeof(Filter), StructureId = 0xD3FC2A56}},
            {"FILTER_NAMEFILTER", new DataFileAttributes {FileName = "NAMEFILTER", RowType = typeof(Filter), StructureId = 0xD3FC2A56}},
            {"FONT", new DataFileAttributes {RowType = typeof(Font), StructureId = 0x4C5392D7}},
            {"FONTCOLORS", new DataFileAttributes {FileName = "FONTCOLOR", RowType = typeof(FontColor), StructureId = 0x472CEC2D}},
            {"FOOTSTEPS", new DataFileAttributes {RowType = typeof(FootSteps), StructureId = 0x4506F984}},
            {"GAME_GLOBALS", new DataFileAttributes {FileName = "GAMEGLOBALS", RowType = typeof(GameGlobals), StructureId = 0x904D8906}},
            {"GLOBAL_INDEX", new DataFileAttributes {FileName = "GLOBALINDEX", RowType = typeof(Global), StructureId = 0x4D232409}},
            {"GLOBAL_STRING", new DataFileAttributes {FileName = "GLOBALSTRING", RowType = typeof(Global), StructureId = 0x4D232409}},
            {"GLOBAL_THEMES", new DataFileAttributes {RowType = typeof(GlobalThemes), StructureId = 0x57D269AF}},
            {"INITDB", new DataFileAttributes {RowType = typeof(InitDb), StructureId = 0xDF1BE0CD}},
            {"INTERACT", new DataFileAttributes {RowType = typeof(Interact), StructureId = 0xFC8E3B0C}},
            {"INTERACT_MENU", new DataFileAttributes {RowType = typeof(InteractMenu), StructureId = 0x6078CD93}},
            {"INVENTORY", new DataFileAttributes {RowType = typeof(Inventory), StructureId = 0x8FEEC9AC}},
            {"INVENTORY_TYPES", new DataFileAttributes {RowType = typeof(InventoryTypes), StructureId = 0x102ECE59}},
            {"INVLOCIDX", new DataFileAttributes {FileName = "INVLOC", RowType = typeof(InvLoc), StructureId = 0x9923217F}},
            {"ITEM_LEVELS", new DataFileAttributes {RowType = typeof(ItemLevels), StructureId = 0xCE724D43}},
            {"ITEM_LOOK_GROUPS", new DataFileAttributes {RowType = typeof(ItemLookGroups), StructureId = 0xEBE7DF6B}},
            {"ITEM_LOOKS", new DataFileAttributes {RowType = typeof(ItemLooks), StructureId = 0x4FC6AEA2}},
            {"ITEM_QUALITY", new DataFileAttributes {FileName = "ITEMQUALITY", RowType = typeof(ItemQuality), StructureId = 0x2D83EDCC}},
            {"ITEMS", new DataFileAttributes {RowType = typeof(Items), HasExtended = true, StructureId = 0x887988C4}},
            {"LEVEL", new DataFileAttributes {FileName = "LEVELS", RowType = typeof(Levels), StructureId = 0x51C1C606}},
            {"LEVEL_DRLG_CHOICE", new DataFileAttributes {FileName = "LEVELS_DRLG_CHOICE", RowType = typeof(LevelsDrlgChoice), StructureId = 0xBCDCE6DE}},
            {"LEVEL_DRLGS", new DataFileAttributes {FileName = "LEVELS_DRLGS", RowType = typeof(LevelsDrlgs), StructureId = 0x1CF9BDE9}},
            {"LEVEL_ENVIRONMENTS", new DataFileAttributes {FileName = "LEVELS_ENV", RowType = typeof(LevelsEnv), StructureId = 0x4CC2F23D}},
            {"LEVEL_FILE_PATHS", new DataFileAttributes {FileName = "LEVELS_FILE_PATH", RowType = typeof(LevelsFilePath), StructureId = 0xB28448F5}},
            {"LEVEL_RULES", new DataFileAttributes {FileName = "LEVELS_RULES", RowType = typeof(LevelsRules), StructureId = 0xB7A70C96}},
            {"LEVEL_THEMES", new DataFileAttributes {FileName = "LEVELS_THEMES", RowType = typeof(LevelsThemes), StructureId = 0xD1136038}},
            {"LEVEL_SCALING", new DataFileAttributes {FileName = "LEVELSCALING", RowType = typeof(LevelScaling), StructureId = 0xACB1C3DD}},
            {"LOADING_TIPS", new DataFileAttributes {RowType = typeof(LoadingTips), StructureId = 0xF98A5E41}},
            {"MATERIALS_COLLISION", new DataFileAttributes {RowType = typeof(MaterialsCollision), StructureId = 0x60CA8F60}},
            {"MATERIALS_GLOBAL", new DataFileAttributes {RowType = typeof(MaterialsGlobal), StructureId = 0x40382E46}},
            {"MELEEWEAPONS", new DataFileAttributes {RowType = typeof(MeleeWeapons), StructureId = 0x6CA52FE7}},
            {"MISSILES", new DataFileAttributes {RowType = typeof(Items), HasExtended = true, StructureId = 0x887988C4}},
            {"MONLEVEL", new DataFileAttributes {RowType = typeof(MonLevel), HasScriptTable = true, StructureId = 0x64429E37}},
            {"MONSCALING", new DataFileAttributes {RowType = typeof(MonScaling), StructureId = 0x6635D021}},
            {"MONSTER_NAME_TYPES", new DataFileAttributes {RowType = typeof(MonsterNameTypes), StructureId = 0x86DC367C}},
            {"MONSTER_NAMES", new DataFileAttributes {RowType = typeof(MonsterNames), StructureId = 0x80DE708E}},
            {"MONSTER_QUALITY", new DataFileAttributes {RowType = typeof(MonsterQuality), StructureId = 0x5BCCB897}},
            {"MONSTERS", new DataFileAttributes {RowType = typeof(Items), HasExtended = true, StructureId = 0x887988C4}},
            {"MOVIE_SUBTITLES", new DataFileAttributes {RowType = typeof(MovieSubTitles), StructureId = 0xCD2DFD19}},
            {"MOVIELISTS", new DataFileAttributes {RowType = typeof(MovieLists), StructureId = 0xC67C9F70}},
            {"MOVIES", new DataFileAttributes {RowType = typeof(Movies), StructureId = 0x1481FC18}},
            {"MUSIC", new DataFileAttributes {RowType = typeof(Music), StructureId = 0xCCE72560}},
            {"MUSIC_REF", new DataFileAttributes {FileName = "MUSICREF", RowType = typeof(MusicRef), StructureId = 0x2F942E56}},
            {"MUSIC_SCRIPT_DEBUG", new DataFileAttributes {FileName = "MUSICSCRIPTDEBUG", RowType = typeof(MusicScriptDebug), StructureId = 0xA30E10D3}},
            {"MUSICCONDITIONS", new DataFileAttributes {RowType = typeof(MusicConditions), StructureId = 0xD5910000}},
            {"MUSICGROOVELEVELS", new DataFileAttributes {RowType = typeof(MusicGrooveLevels), StructureId = 0x7DF5E322}},
            {"MUSICGROOVELEVELTYPES", new DataFileAttributes {RowType = typeof(MusicGrooveLevelTypes), StructureId = 0xCC2486A8}},
            {"MUSICSTINGERS", new DataFileAttributes {RowType = typeof(MusicStingers), StructureId = 0xF5E3E982}},
            {"MUSICSTINGERSETS", new DataFileAttributes {RowType = typeof(MusicStingerSets), StructureId = 0x1B82B5B5}},
            {"NPC", new DataFileAttributes {RowType = typeof(Npc), StructureId = 0xD4CB8A6A}},
            {"OBJECTS", new DataFileAttributes {RowType = typeof(Items), HasExtended = true, HasScriptTable = true, StructureId = 0x887988C4}},
            {"OBJECTTRIGGERS", new DataFileAttributes {RowType = typeof(ObjectTriggers), StructureId = 0x3723584E}},
            {"OFFER", new DataFileAttributes {RowType = typeof(Offer), StructureId = 0x11302F85}},
            {"PALETTES", new DataFileAttributes {RowType = typeof(Palettes), StructureId = 0xFE47EF60}},
            {"PETLEVEL", new DataFileAttributes {RowType = typeof(MonLevel), HasScriptTable = true, StructureId = 0x64429E37}},
            {"PLAYER_RACE", new DataFileAttributes {FileName = "PLAYERRACE", RowType = typeof(PlayerRace), StructureId = 0x0A26EF57}},
            {"PLAYERLEVELS", new DataFileAttributes {RowType = typeof(PlayerLevels), HasScriptTable = true, StructureId = 0xA5887717}},
            {"PLAYERS", new DataFileAttributes {RowType = typeof(Items), HasExtended = true, StructureId = 0x887988C4}},
            {"PROCS", new DataFileAttributes {RowType = typeof(Procs), StructureId = 0xDDBAD110}},
            {"PROPERTIES", new DataFileAttributes {RowType = typeof(Properties), HasScriptTable = true, StructureId = 0x5876D156}},
            {"PROPS", new DataFileAttributes {RowType = typeof(LevelsRoomIndex), StructureId = 0x8EF00B17}},
            {"QUEST", new DataFileAttributes {RowType = typeof(Quest), StructureId = 0x043A420D}},
            {"QUEST_CAST", new DataFileAttributes {RowType = typeof(QuestCast), StructureId = 0x7765138E}},
            {"QUEST_STATE", new DataFileAttributes {RowType = typeof(QuestState), StructureId = 0x1A0A1C09}},
            {"QUEST_STATE_VALUE", new DataFileAttributes {RowType = typeof(ExcelTables), StructureId = 0x86DC367C}},
            {"QUEST_STATUS", new DataFileAttributes {RowType = typeof(QuestStatus), StructureId = 0xB0593288}},
            {"QUEST_TEMPLATE", new DataFileAttributes {RowType = typeof(QuestTemplate), StructureId = 0xD4AE7FA7}},
            {"RARENAMES", new DataFileAttributes {RowType = typeof(RareNames), HasScriptTable = true, StructureId = 0xF09E4089}},
            {"RECIPELISTS", new DataFileAttributes {RowType = typeof(RecipeLists), StructureId = 0xE1A7A39A}},
            {"RECIPES", new DataFileAttributes {RowType = typeof(Recipes), StructureId = 0x3230B6BC}},
            {"ROOM_INDEX", new DataFileAttributes {FileName = "LEVELS_ROOM_INDEX", RowType = typeof(LevelsRoomIndex), StructureId = 0x8EF00B17}},
            {"SKILLEVENTTYPES", new DataFileAttributes {RowType = typeof(SkillEventTypes), StructureId = 0xB47A4EC5}},
            {"SKILLGROUPS", new DataFileAttributes {RowType = typeof(SkillGroups), StructureId = 0xD2FE445A}},
            {"SKILLS", new DataFileAttributes {RowType = typeof(Skills), HasScriptTable = true, StructureId = 0xBAF5E904}},
            {"SKILLTABS", new DataFileAttributes {RowType = typeof(SkillTabs), StructureId = 0xF934EB3B}},
            {"SKU", new DataFileAttributes {RowType = typeof(Sku), StructureId = 0xCF23C241}},
            {"SOUNDBUSES", new DataFileAttributes {RowType = typeof(SoundBuses), StructureId = 0x1E760FB1}},
            {"SOUND_MIXSTATE_VALUES", new DataFileAttributes {FileName = "SOUNDMIXSTATEVALUES", RowType = typeof(SoundMixStateValues), StructureId = 0xA84F422C}},
            {"SOUND_MIXSTATES", new DataFileAttributes {FileName = "SOUNDMIXSTATES", RowType = typeof(SoundMixStates), StructureId = 0xDB77DD54}},
            {"SOUNDS", new DataFileAttributes {RowType = typeof(Sounds), StructureId = 0x1A4CAF8A}},
            {"SOUNDVCAS", new DataFileAttributes {RowType = typeof(SoundVidCas), StructureId = 0xA42BCE8C}},
            {"SOUNDVCASETS", new DataFileAttributes {RowType = typeof(SoundVideoCasets), StructureId = 0x50F90D15}},
            {"SPAWN_CLASS", new DataFileAttributes {FileName = "SPAWNCLASS", RowType = typeof(SpawnClass), StructureId = 0xBBEBD669}},
            {"STATE_EVENT_TYPES", new DataFileAttributes {RowType = typeof(StateEventTypes), StructureId = 0x72477C36}},
            {"STATE_LIGHTING", new DataFileAttributes {RowType = typeof(StateLighting), StructureId = 0xB1B5294C}},
            {"STATES", new DataFileAttributes {RowType = typeof(States), HasIndexBitRelations = true, StructureId = 0xFD6839DE}},
            {"STATS", new DataFileAttributes {RowType = typeof(Stats), StructureId = 0x569C0513}},
            {"STATS_FUNC", new DataFileAttributes {FileName = "STATSFUNC", RowType = typeof(StatsFunc), StructureId = 0x2C085508}},
            {"STATS_SELECTOR", new DataFileAttributes {FileName = "STATSSELECTOR", RowType = typeof(ExcelTables), StructureId = 0x86DC367C}},
            {"STRING_FILES", new DataFileAttributes {RowType = typeof(StringFiles), StructureId = 0x22FCCFEB}},
            {"SUBLEVEL", new DataFileAttributes {RowType = typeof(SubLevel), StructureId = 0x1ACBB8F7}},
            {"TAG", new DataFileAttributes {RowType = typeof(Tag), StructureId = 0xF8A155D8}},
            {"TASK_STATUS", new DataFileAttributes {RowType = typeof(ExcelTables), StructureId = 0x86DC367C}},
            {"TASKS", new DataFileAttributes {RowType = typeof(Tasks), StructureId = 0xBF3BFFF5}},
            {"TEXTURE_TYPES", new DataFileAttributes {FileName = "TEXTURETYPES", RowType = typeof(TextureTypes), StructureId = 0xC8071451}},
            {"TREASURE", new DataFileAttributes {RowType = typeof(Treasure), StructureId = 0x01A8DA81}},
            {"UI_COMPONENT", new DataFileAttributes {RowType = typeof(UIComponent), StructureId = 0x71D76819}},
            {"UNIT_EVENT_TYPES", new DataFileAttributes {FileName = "UNITEVENTS", RowType = typeof(UnitEvents), StructureId = 0x01082DBE}},
            {"UNITMODE_GROUPS", new DataFileAttributes {RowType = typeof(UnitModeGroups), StructureId = 0x95909737}},
            {"UNITMODES", new DataFileAttributes {RowType = typeof(UnitModes), StructureId = 0x498E341C}},
            {"UNITTYPES", new DataFileAttributes {RowType = typeof(UnitTypes), HasIndexBitRelations = true, StructureId = 0x1F9DDC98}},
            {"WARDROBE_APPEARANCE_GROUP", new DataFileAttributes {RowType = typeof(WardrobeAppearanceGroup), StructureId = 0xFA7B3939}},
            {"WARDROBE_BLENDOP", new DataFileAttributes {RowType = typeof(WardrobeBlendOp), StructureId = 0x8E1FFDA1}},
            {"WARDROBE_BODY", new DataFileAttributes {RowType = typeof(WardrobeBody), StructureId = 0x686002E7}},
            {"WARDROBE_LAYER", new DataFileAttributes {FileName = "WARDROBE", RowType = typeof(Wardrobe), StructureId = 0xC625A079}},
            {"WARDROBE_LAYERSET", new DataFileAttributes {RowType = typeof(WardrobeBlendOp), StructureId = 0x8E1FFDA1}},
            {"WARDROBE_MODEL", new DataFileAttributes {RowType = typeof(WardrobeModel), StructureId = 0xA6D29E2E}},
            {"WARDROBE_MODEL_GROUP", new DataFileAttributes {RowType = typeof(WardrobeModelGroup), StructureId = 0x1F0513C5}},
            {"WARDROBE_PART", new DataFileAttributes {RowType = typeof(WardrobePart), StructureId = 0x58D35B7C}},
            {"WARDROBE_TEXTURESET", new DataFileAttributes {RowType = typeof(WardrobeTextureSet), StructureId = 0xD406BEF5}},
            {"WARDROBE_TEXTURESET_GROUP", new DataFileAttributes {RowType = typeof(WardrobeTextureSetGroup), StructureId = 0x1F0513C5}},
            {"WEATHER", new DataFileAttributes {RowType = typeof(Weather), StructureId = 0x9FF616C5}},
            {"WEATHER_SETS", new DataFileAttributes {RowType = typeof(WeatherSets), StructureId = 0xE70A388C}},

                
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
            {"_TCv4_ITEMS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(ItemsTCv4), HasExtended = true}},
            {"_TCv4_OBJECTS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(ItemsTCv4), HasExtended = true}},
            {"_TCv4_MONSTERS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(ItemsTCv4), HasExtended = true}},
            {"_TCv4_PLAYERS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(ItemsTCv4), HasExtended = true}},
            {"_TCv4_MISSILES", new DataFileAttributes {IsTCv4 = true, RowType = typeof(ItemsTCv4), HasExtended = true}},
            {"_TCv4_MATERIALS_COLLISION", new DataFileAttributes {IsTCv4 = true, RowType = typeof(MaterialsCollisionTCv4)}},
            {"_TCv4_LEVEL", new DataFileAttributes {IsTCv4 = true, FileName = "LEVELS", RowType = typeof(LevelsTCv4)}},
            {"_TCv4_LEVEL_SCALING", new DataFileAttributes {IsTCv4 = true, FileName = "LEVELSCALING", RowType = typeof(LevelScalingTCv4)}},
            {"_TCv4_ROOM_INDEX", new DataFileAttributes {IsTCv4 = true, FileName = "LEVELS_ROOM_INDEX", RowType = typeof(LevelsRoomIndexTCv4)}},
            {"_TCv4_PLAYERLEVELS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(PlayerLevelsTCv4)}},
            {"_TCv4_PROPS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(LevelsRoomIndexTCv4)}},
            {"_TCv4_RECIPES", new DataFileAttributes {IsTCv4 = true, RowType = typeof(RecipesTCv4)}},
            {"_TCv4_SKILLS", new DataFileAttributes {IsTCv4 = true, RowType = typeof(SkillsTCv4), HasScriptTable = true}},
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
            {"INVLOC", new DataFileAttributes {IsEmpty = true}},
            {"SOUNDOVERRIDES", new DataFileAttributes {IsEmpty = true}},


            // TCv4 - Empty Excel files
            {"_TCv4_GOSSIP", new DataFileAttributes {IsEmpty = true}},
            {"_TCv4_SOUNDOVERRIDES", new DataFileAttributes {IsEmpty = true}},


            // Non-Indexed Excel file
            {"LANGUAGE", new DataFileAttributes {RowType = typeof(Language), StructureId = 0x61E37775}},
            {"REGION", new DataFileAttributes {RowType = typeof(Region), StructureId = 0x21C02711}},


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