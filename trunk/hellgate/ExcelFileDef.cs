using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Hellgate.Excel;
using Revival.Common;

namespace Hellgate
{
    public partial class ExcelFile
    {
        public const String FolderPath = "excel\\";
        public const String FileExtention = ".txt.cooked";
        public static KeyValuePair<uint, TypeMap>[] DataTypes;
        public static KeyValuePair<string, uint>[] DataTables;

        static ExcelFile()
        {
            #region Data Types List
            DataTypes = new KeyValuePair<uint, TypeMap>[]
            {
                new KeyValuePair<uint,TypeMap>((uint)0x01082DBE, new TypeMap { DataType = typeof(UnitEvents) }),
                new KeyValuePair<uint,TypeMap>((uint)0x01A80106, new TypeMap { DataType = typeof(FactionStanding) }),
                new KeyValuePair<uint,TypeMap>((uint)0x01A8DA81, new TypeMap { DataType = typeof(Treasure) }),
                new KeyValuePair<uint,TypeMap>((uint)0x03407879, new TypeMap { DataType = typeof(AnimationGroups) }),
                new KeyValuePair<uint,TypeMap>((uint)0x043A420D, new TypeMap { DataType = typeof(Quest) }),
                new KeyValuePair<uint,TypeMap>((uint)0x08402828, new TypeMap { DataType = typeof(CharacterClass) }),
                new KeyValuePair<uint,TypeMap>((uint)0x0A36EF57, new TypeMap { DataType = typeof(PlayerRace) }),

                new KeyValuePair<uint,TypeMap>((uint)0x102ECE59, new TypeMap { DataType = typeof(AiStart) }),
                new KeyValuePair<uint,TypeMap>((uint)0x106C109C, new TypeMap { DataType = typeof(AffixTypes) }),
                new KeyValuePair<uint,TypeMap>((uint)0x11302F85, new TypeMap { DataType = typeof(Offer) }),
                new KeyValuePair<uint,TypeMap>((uint)0x1481FC18, new TypeMap { DataType = typeof(Movies) }),
                new KeyValuePair<uint,TypeMap>((uint)0x1A0A1C09, new TypeMap { DataType = typeof(QuestState) }),
                //new KeyValuePair<uint,TypeMap>((uint)0x1A4CAF8A, new TypeMap { DataType = typeof(Sounds) }),
                new KeyValuePair<uint,TypeMap>((uint)0x1ACBB8F7, new TypeMap { DataType = typeof(SubLevel) }),
                new KeyValuePair<uint,TypeMap>((uint)0x1B82B5B5, new TypeMap { DataType = typeof(MusicStingerSets) }),
                new KeyValuePair<uint,TypeMap>((uint)0x1CF9BDE9, new TypeMap { DataType = typeof(LevelsDrlgs) }),
                new KeyValuePair<uint,TypeMap>((uint)0x1E760FB1, new TypeMap { DataType = typeof(SoundBuses) }),
                new KeyValuePair<uint,TypeMap>((uint)0x1EE32EF6, new TypeMap { DataType = typeof(Bones) }),
                new KeyValuePair<uint,TypeMap>((uint)0x1F0513C5, new TypeMap { DataType = typeof(WardrobeModelGroupRow) }),
                new KeyValuePair<uint,TypeMap>((uint)0x1F9DDC98, new TypeMap { DataType = typeof(UnitTypes), IgnoresTable = true, HasIndexBitRelations = true }),

                new KeyValuePair<uint,TypeMap>((uint)0x22FCCFEB, new TypeMap { DataType = typeof(StringFiles) }),
                new KeyValuePair<uint,TypeMap>((uint)0x26BC8A8D, new TypeMap { DataType = typeof(DamageEffects) }),
                new KeyValuePair<uint,TypeMap>((uint)0x2C085508, new TypeMap { DataType = typeof(StatsFunc) }),
                new KeyValuePair<uint,TypeMap>((uint)0x2D83EDCC, new TypeMap { DataType = typeof(ItemQuality) }),
                new KeyValuePair<uint,TypeMap>((uint)0x2F942E56, new TypeMap { DataType = typeof(MusicRef) }),

                new KeyValuePair<uint,TypeMap>((uint)0x3230B6BC, new TypeMap { DataType = typeof(Recipes) }),
                new KeyValuePair<uint,TypeMap>((uint)0x3723584E, new TypeMap { DataType = typeof(ObjectTriggers) }),

                new KeyValuePair<uint,TypeMap>((uint)0x40382E46, new TypeMap { DataType = typeof(MaterialsGlobal) }),
                new KeyValuePair<uint,TypeMap>((uint)0x4319D23D, new TypeMap { DataType = typeof(Display) }),
                new KeyValuePair<uint,TypeMap>((uint)0x4506F984, new TypeMap { DataType = typeof(FootSteps) }),
                new KeyValuePair<uint,TypeMap>((uint)0x45B08A9E, new TypeMap { DataType = typeof(AnimationStance) }),
                new KeyValuePair<uint,TypeMap>((uint)0x472CEC2D, new TypeMap { DataType = typeof(FontColor) }),
                new KeyValuePair<uint,TypeMap>((uint)0x498E341C, new TypeMap { DataType = typeof(UnitModes) }),
                new KeyValuePair<uint,TypeMap>((uint)0x4C5392D7, new TypeMap { DataType = typeof(Font) }),
                new KeyValuePair<uint,TypeMap>((uint)0x4CC2F23D, new TypeMap { DataType = typeof(LevelsEnv) }),
                new KeyValuePair<uint,TypeMap>((uint)0x4D232409, new TypeMap { DataType = typeof(Global) }),
                new KeyValuePair<uint,TypeMap>((uint)0x4FC6AEA2, new TypeMap { DataType = typeof(ItemLooks) }),

                new KeyValuePair<uint,TypeMap>((uint)0x50F90D15, new TypeMap { DataType = typeof(SoundVideoCasets) }),
                new KeyValuePair<uint,TypeMap>((uint)0x51C1C606, new TypeMap { DataType = typeof(Levels) }),
                new KeyValuePair<uint,TypeMap>((uint)0x569C0513, new TypeMap { DataType = typeof(Stats) }),
                new KeyValuePair<uint,TypeMap>((uint)0x57D269AF, new TypeMap { DataType = typeof(GlobalThemes) }),
                new KeyValuePair<uint,TypeMap>((uint)0x5876D156, new TypeMap { DataType = typeof(Properties), HasMysh = true }),
                new KeyValuePair<uint,TypeMap>((uint)0x58D35B7C, new TypeMap { DataType = typeof(WardrobePart) }),
                new KeyValuePair<uint,TypeMap>((uint)0x5BCCB897, new TypeMap { DataType = typeof(MonsterQuality) }),

                new KeyValuePair<uint,TypeMap>((uint)0x6078CD93, new TypeMap { DataType = typeof(InteractMenu) }),
                new KeyValuePair<uint,TypeMap>((uint)0x60CA8F60, new TypeMap { DataType = typeof(MaterialsCollision) }),
                new KeyValuePair<uint,TypeMap>((uint)0x62ECA6E1, new TypeMap { DataType = typeof(Achievements) }),
                new KeyValuePair<uint,TypeMap>((uint)0x63F90CA5, new TypeMap { DataType = typeof(BackGroundSounds3D) }),
                new KeyValuePair<uint,TypeMap>((uint)0x64429E37, new TypeMap { DataType = typeof(MonLevel) }),
                new KeyValuePair<uint,TypeMap>((uint)0x6635D021, new TypeMap { DataType = typeof(MonScaling) }),
                new KeyValuePair<uint,TypeMap>((uint)0x686002E7, new TypeMap { DataType = typeof(WardrobeBody) }),
                new KeyValuePair<uint,TypeMap>((uint)0x6CA52FE7, new TypeMap { DataType = typeof(MeleeWeapons) }),

                new KeyValuePair<uint,TypeMap>((uint)0x71D76819, new TypeMap { DataType = typeof(UIComponent) }),
                new KeyValuePair<uint,TypeMap>((uint)0x72477C36, new TypeMap { DataType = typeof(StateEventTypes) }),
                new KeyValuePair<uint,TypeMap>((uint)0x7765138E, new TypeMap { DataType = typeof(QuestCast) }),
                new KeyValuePair<uint,TypeMap>((uint)0x7A7D891E, new TypeMap { DataType = typeof(BudgetsModel) }),
                new KeyValuePair<uint,TypeMap>((uint)0x7DF5E322, new TypeMap { DataType = typeof(MusicGrooveLevels) }),
                new KeyValuePair<uint,TypeMap>((uint)0x7F15F865, new TypeMap { DataType = typeof(AiInit) }),

                new KeyValuePair<uint,TypeMap>((uint)0x80DE708E, new TypeMap { DataType = typeof(MonsterNames) }),
                new KeyValuePair<uint,TypeMap>((uint)0x86DC367C, new TypeMap { DataType = typeof(BookMarks) }),
                new KeyValuePair<uint,TypeMap>((uint)0x887988C4, new TypeMap { DataType = typeof(Items), HasExtended = true }),
                new KeyValuePair<uint,TypeMap>((uint)0x8A5FF6B8, new TypeMap { DataType = typeof(AiBehaviour) }),
                new KeyValuePair<uint,TypeMap>((uint)0x8B84B802, new TypeMap { DataType = typeof(ConditionFunctions) }),
                new KeyValuePair<uint,TypeMap>((uint)0x8E1FFDA1, new TypeMap { DataType = typeof(WardrobeBlendOp) }),
                new KeyValuePair<uint,TypeMap>((uint)0x8EF00B17, new TypeMap { DataType = typeof(LevelsRoomIndex) }),
                new KeyValuePair<uint,TypeMap>((uint)0x8FEEC9AC, new TypeMap { DataType = typeof(Inventory) }),

                new KeyValuePair<uint,TypeMap>((uint)0x904D8906, new TypeMap { DataType = typeof(GameGlobals) }),
                new KeyValuePair<uint,TypeMap>((uint)0x95909737, new TypeMap { DataType = typeof(UnitModeGroups) }),
                new KeyValuePair<uint,TypeMap>((uint)0x9923217F, new TypeMap { DataType = typeof(InvLoc) }),
                new KeyValuePair<uint,TypeMap>((uint)0x99264BCB, new TypeMap { DataType = typeof(EffectsIndex) }),
                new KeyValuePair<uint,TypeMap>((uint)0x9DF76E6C, new TypeMap { DataType = typeof(Affixes) }),
                new KeyValuePair<uint,TypeMap>((uint)0x9FF616C5, new TypeMap { DataType = typeof(Weather) }),

                new KeyValuePair<uint,TypeMap>((uint)0xA30E10D3, new TypeMap { DataType = typeof(MusicScriptDebug) }),
                new KeyValuePair<uint,TypeMap>((uint)0xA42BCE8C, new TypeMap { DataType = typeof(SoundVidCas) }),
                new KeyValuePair<uint,TypeMap>((uint)0xA5887717, new TypeMap { DataType = typeof(PlayerLevels) }),
                new KeyValuePair<uint,TypeMap>((uint)0xA6D29E2E, new TypeMap { DataType = typeof(WardrobeModel) }),
                new KeyValuePair<uint,TypeMap>((uint)0xA84F422C, new TypeMap { DataType = typeof(SoundMixStateValues) }),
                new KeyValuePair<uint,TypeMap>((uint)0xAA0F158C, new TypeMap { DataType = typeof(BadgeRewards) }),
                new KeyValuePair<uint,TypeMap>((uint)0xACB1C3DD, new TypeMap { DataType = typeof(LevelScaling) }),
                new KeyValuePair<uint,TypeMap>((uint)0xAF168F4E, new TypeMap { DataType = typeof(Dialog) }),
                new KeyValuePair<uint,TypeMap>((uint)0xAFBF5906, new TypeMap { DataType = typeof(AiCommonState) }),

                new KeyValuePair<uint,TypeMap>((uint)0xB0593288, new TypeMap { DataType = typeof(QuestStatus) }),
                new KeyValuePair<uint,TypeMap>((uint)0xB0DA4BE1, new TypeMap { DataType = typeof(BackGroundSounds2D) }),
                new KeyValuePair<uint,TypeMap>((uint)0xB1B5294C, new TypeMap { DataType = typeof(StateLighting) }),
                new KeyValuePair<uint,TypeMap>((uint)0xB28448F5, new TypeMap { DataType = typeof(LevelsFilePath) }),
                new KeyValuePair<uint,TypeMap>((uint)0xB47A4EC5, new TypeMap { DataType = typeof(SkillEventTypes) }),
                new KeyValuePair<uint,TypeMap>((uint)0xB7A70C96, new TypeMap { DataType = typeof(LevelsRules) }),
                new KeyValuePair<uint,TypeMap>((uint)0xB7BB74D1, new TypeMap { DataType = typeof(Faction) }),
                new KeyValuePair<uint,TypeMap>((uint)0xBAF5E904, new TypeMap { DataType = typeof(Skills), HasMysh = true }),
                new KeyValuePair<uint,TypeMap>((uint)0xBB554372, new TypeMap { DataType = typeof(Act) }),
                new KeyValuePair<uint,TypeMap>((uint)0xBBC15A50, new TypeMap { DataType = typeof(ColorSets) }),
                new KeyValuePair<uint,TypeMap>((uint)0xBBEBD669, new TypeMap { DataType = typeof(SpawnClass) }),
                new KeyValuePair<uint,TypeMap>((uint)0xBCDCE6DE, new TypeMap { DataType = typeof(LevelsDrlgChoice) }),
                new KeyValuePair<uint,TypeMap>((uint)0xBEE975EA, new TypeMap { DataType = typeof(BudgetTextureMips) }),
                new KeyValuePair<uint,TypeMap>((uint)0xBF3BFFF5, new TypeMap { DataType = typeof(Tasks) }),

                new KeyValuePair<uint,TypeMap>((uint)0xC625A079, new TypeMap { DataType = typeof(Wardrobe) }),
                new KeyValuePair<uint,TypeMap>((uint)0xC67C9F70, new TypeMap { DataType = typeof(MovieLists) }),
                new KeyValuePair<uint,TypeMap>((uint)0xC8071451, new TypeMap { DataType = typeof(TextureTypes) }),
                new KeyValuePair<uint,TypeMap>((uint)0xC8471612, new TypeMap { DataType = typeof(EffectsShaders) }),
                new KeyValuePair<uint,TypeMap>((uint)0xCC2486A8, new TypeMap { DataType = typeof(MusicGrooveLevelTypes) }),
                new KeyValuePair<uint,TypeMap>((uint)0xCCE72560, new TypeMap { DataType = typeof(Music) }),
                new KeyValuePair<uint,TypeMap>((uint)0xCD2DFD19, new TypeMap { DataType = typeof(MovieSubTitles) }),
                new KeyValuePair<uint,TypeMap>((uint)0xCE724D43, new TypeMap { DataType = typeof(ItemLevels) }),
                new KeyValuePair<uint,TypeMap>((uint)0xCF23C241, new TypeMap { DataType = typeof(Sku) }),

                new KeyValuePair<uint,TypeMap>((uint)0xD1136038, new TypeMap { DataType = typeof(LevelsThemes) }),
                new KeyValuePair<uint,TypeMap>((uint)0xD2FE445A, new TypeMap { DataType = typeof(SkillGroups) }),
                new KeyValuePair<uint,TypeMap>((uint)0xD3FC2A56, new TypeMap { DataType = typeof(Filter) }),
                new KeyValuePair<uint,TypeMap>((uint)0xD406BEF5, new TypeMap { DataType = typeof(WardrobeTextureSet) }),
                new KeyValuePair<uint,TypeMap>((uint)0xD4AE7FA7, new TypeMap { DataType = typeof(QuestTemplate) }),
                new KeyValuePair<uint,TypeMap>((uint)0xD4CB8A6A, new TypeMap { DataType = typeof(Npc) }),
                new KeyValuePair<uint,TypeMap>((uint)0xD5910000, new TypeMap { DataType = typeof(MusicConditions) }),
                new KeyValuePair<uint,TypeMap>((uint)0xD59E46B8, new TypeMap { DataType = typeof(AnimationCondition) }),
                new KeyValuePair<uint,TypeMap>((uint)0xDB77DD54, new TypeMap { DataType = typeof(SoundMixStates) }),
                new KeyValuePair<uint,TypeMap>((uint)0xDC35E3D0, new TypeMap { DataType = typeof(ChatInstancedChannels) }),
                new KeyValuePair<uint,TypeMap>((uint)0xDDBAD110, new TypeMap { DataType = typeof(Procs) }),
                new KeyValuePair<uint,TypeMap>((uint)0xDF1BE0CD, new TypeMap { DataType = typeof(InitDb) }),

                new KeyValuePair<uint,TypeMap>((uint)0xE1A7A39A, new TypeMap { DataType = typeof(RecipeLists) }),
                new KeyValuePair<uint,TypeMap>((uint)0xE70A388C, new TypeMap { DataType = typeof(WeatherSets) }),
                new KeyValuePair<uint,TypeMap>((uint)0xEAC1CAA4, new TypeMap { DataType = typeof(DamageTypes) }),
                new KeyValuePair<uint,TypeMap>((uint)0xEBE7DF6B, new TypeMap { DataType = typeof(ItemLookGroups) }),

                new KeyValuePair<uint,TypeMap>((uint)0xF09E4089, new TypeMap { DataType = typeof(RareNames) }),
                new KeyValuePair<uint,TypeMap>((uint)0xF303187F, new TypeMap { DataType = typeof(EffectsFiles) }),
                new KeyValuePair<uint,TypeMap>((uint)0xF5E3E982, new TypeMap { DataType = typeof(MusicStingers) }),
                new KeyValuePair<uint,TypeMap>((uint)0xF8A155D8, new TypeMap { DataType = typeof(Tag) }),
                new KeyValuePair<uint,TypeMap>((uint)0xF934EB3B, new TypeMap { DataType = typeof(SkillTabs) }),
                new KeyValuePair<uint,TypeMap>((uint)0xF98A5E41, new TypeMap { DataType = typeof(LoadingTips) }),
                new KeyValuePair<uint,TypeMap>((uint)0xFA7B3939, new TypeMap { DataType = typeof(WardrobeAppearanceGroup) }),
                new KeyValuePair<uint,TypeMap>((uint)0xFC8E3B0C, new TypeMap { DataType = typeof(Interact) }),
                new KeyValuePair<uint,TypeMap>((uint)0xFD6839DE, new TypeMap { DataType = typeof(States), HasIndexBitRelations = true }),
                new KeyValuePair<uint,TypeMap>((uint)0xFE47EF60, new TypeMap { DataType = typeof(Palettes) })
            };

            DataTables = new KeyValuePair<string, uint>[]
            {
                new KeyValuePair<String, UInt32>("ACHIEVEMENTS", 0x62ECA6E1),
                new KeyValuePair<String, UInt32>("ACT", 0xBB554372),
                new KeyValuePair<String, UInt32>("AFFIXES", 0x9DF76E6C),
                new KeyValuePair<String, UInt32>("AFFIXTYPES", 0x106C109C),
                new KeyValuePair<String, UInt32>("AI_BEHAVIOR", 0x8A5FF6B8),
                new KeyValuePair<String, UInt32>("AI_INIT", 0x7F15F865),
                new KeyValuePair<String, UInt32>("AI_START", 0x102ECE59),
                new KeyValuePair<String, UInt32>("AICOMMON_STATE", 0xAFBF5906),
                new KeyValuePair<String, UInt32>("ANIMATION_CONDITION", 0xD59E46B8),
                new KeyValuePair<String, UInt32>("ANIMATION_GROUP", 0x03407879),
                new KeyValuePair<String, UInt32>("ANIMATION_STANCE", 0x45B08A9E),

                new KeyValuePair<String, UInt32>("BACKGROUNDSOUNDS", 0xD783DCDA),
                new KeyValuePair<String, UInt32>("BACKGROUNDSOUNDS2D", 0xB0DA4BE1),
                new KeyValuePair<String, UInt32>("BACKGROUNDSOUNDS3D", 0x63F90CA5),
                new KeyValuePair<String, UInt32>("BADGE_REWARDS", 0xAA0F158C),
                new KeyValuePair<String, UInt32>("BONES", 0x1EE32EF6),
                new KeyValuePair<String, UInt32>("BONEWEIGHTS", 0x1EE32EF6),
                new KeyValuePair<String, UInt32>("BOOKMARKS", 0x86DC367C),
                new KeyValuePair<String, UInt32>("BUDGETS_MODEL", 0x7A7D891E),
                new KeyValuePair<String, UInt32>("BUDGETS_TEXTURE_MIPS", 0xBEE975EA),

                new KeyValuePair<String, UInt32>("CHARACTER_CLASS", 0x08402828),
                new KeyValuePair<String, UInt32>("CHAT_INSTANCED_CHANNELS", 0xDC35E3D0),
                new KeyValuePair<String, UInt32>("CHATFILTER", 0xD3FC2A56),
                new KeyValuePair<String, UInt32>("COLORSETS", 0xBBC15A50),
                new KeyValuePair<String, UInt32>("CONDITION_FUNCTIONS", 0x8B84B802),

                new KeyValuePair<String, UInt32>("DAMAGEEFFECTS", 0x26BC8A8D),
                new KeyValuePair<String, UInt32>("DAMAGETYPES", 0xEAC1CAA4),
                new KeyValuePair<String, UInt32>("DIALOG", 0xAF168F4E),
                new KeyValuePair<String, UInt32>("DIFFICULTY", 0x5C719A50),
                new KeyValuePair<String, UInt32>("DISPLAY_CHAR", 0x4319D23D),
                new KeyValuePair<String, UInt32>("DISPLAY_ITEM", 0x4319D23D),

                new KeyValuePair<String, UInt32>("EXCELTABLES", 0x86DC367C),
                new KeyValuePair<String, UInt32>("EFFECT_FILES", 0xF303187F),
                new KeyValuePair<String, UInt32>("EFFECTS_INDEX", 0x99264BCB),
                new KeyValuePair<String, UInt32>("EFFECTS_SHADERS", 0xC8471612),

                new KeyValuePair<String, UInt32>("FACTION", 0xB7BB74D1),
                new KeyValuePair<String, UInt32>("FACTION_STANDING", 0x01A80106),
                new KeyValuePair<String, UInt32>("FONT", 0x4C5392D7),
                new KeyValuePair<String, UInt32>("FONTCOLOR", 0x472CEC2D),
                new KeyValuePair<String, UInt32>("FOOTSTEPS", 0x4506F984),

                new KeyValuePair<String, UInt32>("GAMEGLOBALS", 0x904D8906),
                new KeyValuePair<String, UInt32>("GLOBAL_THEMES", 0x57D269AF),
                new KeyValuePair<String, UInt32>("GLOBALINDEX", 0x4D232409),
                new KeyValuePair<String, UInt32>("GLOBALSTRING", 0x4D232409),
                new KeyValuePair<String, UInt32>("GOSSIP", 0x4F1EFCCD),

                new KeyValuePair<String, UInt32>("INITDB", 0xDF1BE0CD),
                new KeyValuePair<String, UInt32>("INTERACT", 0xFC8E3B0C),
                new KeyValuePair<String, UInt32>("INTERACT_MENU", 0x6078CD93),
                new KeyValuePair<String, UInt32>("INVENTORY", 0x8FEEC9AC),
                new KeyValuePair<String, UInt32>("INVENTORY_TYPES", 0x102ECE59),
                new KeyValuePair<String, UInt32>("INVLOC", 0x9923217F),
                new KeyValuePair<String, UInt32>("ITEM_LEVELS", 0xCE724D43),
                new KeyValuePair<String, UInt32>("ITEM_LOOK_GROUPS", 0xEBE7DF6B),
                new KeyValuePair<String, UInt32>("ITEM_LOOKS", 0x4FC6AEA2),
                new KeyValuePair<String, UInt32>("ITEMQUALITY", 0x2D83EDCC),
                new KeyValuePair<String, UInt32>("ITEMS", 0x887988C4),

                new KeyValuePair<String, UInt32>("LEVELS", 0x51C1C606),
                new KeyValuePair<String, UInt32>("LEVELS_DRLG_CHOICE", 0xBCDCE6DE),
                new KeyValuePair<String, UInt32>("LEVELS_DRLGS", 0x1CF9BDE9),
                new KeyValuePair<String, UInt32>("LEVELS_ENV", 0x4CC2F23D),
                new KeyValuePair<String, UInt32>("LEVELS_FILE_PATH", 0xB28448F5),
                new KeyValuePair<String, UInt32>("LEVELS_ROOM_INDEX", 0x8EF00B17),
                new KeyValuePair<String, UInt32>("LEVELS_RULES", 0xB7A70C96),
                new KeyValuePair<String, UInt32>("LEVELS_THEMES", 0xD1136038),
                new KeyValuePair<String, UInt32>("LEVELSCALING", 0xACB1C3DD),
                new KeyValuePair<String, UInt32>("LOADING_TIPS", 0xF98A5E41),

                new KeyValuePair<String, UInt32>("MATERIALS_COLLISION", 0x60CA8F60),
                new KeyValuePair<String, UInt32>("MATERIALS_GLOBAL", 0x40382E46),
                new KeyValuePair<String, UInt32>("MELEEWEAPONS", 0x6CA52FE7),
                new KeyValuePair<String, UInt32>("MISSILES", 0x887988C4),
                new KeyValuePair<String, UInt32>("MONLEVEL", 0x64429E37),
                new KeyValuePair<String, UInt32>("MONSCALING", 0x6635D021),
                new KeyValuePair<String, UInt32>("MONSTER_NAME_TYPES", 0x86DC367C),
                new KeyValuePair<String, UInt32>("MONSTER_NAMES", 0x80DE708E),
                new KeyValuePair<String, UInt32>("MONSTER_QUALITY", 0x5BCCB897),
                new KeyValuePair<String, UInt32>("MONSTERS", 0x887988C4),
                new KeyValuePair<String, UInt32>("MOVIE_SUBTITLES", 0xCD2DFD19),
                new KeyValuePair<String, UInt32>("MOVIELISTS", 0xC67C9F70),
                new KeyValuePair<String, UInt32>("MOVIES", 0x1481FC18),
                new KeyValuePair<String, UInt32>("MUSIC", 0xCCE72560),
                new KeyValuePair<String, UInt32>("MUSICCONDITIONS", 0xD5910000),
                new KeyValuePair<String, UInt32>("MUSICGROOVELEVELS", 0x7DF5E322),
                new KeyValuePair<String, UInt32>("MUSICGROOVELEVELTYPES", 0xCC2486A8),
                new KeyValuePair<String, UInt32>("MUSICREF", 0x2F942E56),
                new KeyValuePair<String, UInt32>("MUSICSCRIPTDEBUG", 0xA30E10D3),
                new KeyValuePair<String, UInt32>("MUSICSTINGERS", 0xF5E3E982),
                new KeyValuePair<String, UInt32>("MUSICSTINGERSETS", 0x1B82B5B5),

                new KeyValuePair<String, UInt32>("NAMEFILTER", 0xD3FC2A56),
                new KeyValuePair<String, UInt32>("NPC", 0xD4CB8A6A),

                new KeyValuePair<String, UInt32>("OBJECTS", 0x887988C4),
                new KeyValuePair<String, UInt32>("OBJECTTRIGGERS", 0x3723584E),
                new KeyValuePair<String, UInt32>("OFFER", 0x11302F85),

                new KeyValuePair<String, UInt32>("PALETTES", 0xFE47EF60),
                new KeyValuePair<String, UInt32>("PETLEVEL", 0x64429E37),
                new KeyValuePair<String, UInt32>("PLAYERSLEVELS", 0xA5887717),
                new KeyValuePair<String, UInt32>("PLAYERRACE", 0x0A36EF57),
                new KeyValuePair<String, UInt32>("PLAYERS", 0x887988C4),
                new KeyValuePair<String, UInt32>("PROCS", 0xDDBAD110),
                new KeyValuePair<String, UInt32>("PROPERTIES", 0x5876D156),
                new KeyValuePair<String, UInt32>("PROPS", 0x8EF00B17),

                new KeyValuePair<String, UInt32>("QUEST", 0x043A420D),
                new KeyValuePair<String, UInt32>("QUEST_CAST", 0x7765138E),
                new KeyValuePair<String, UInt32>("QUEST_STATE", 0x1A0A1C09),
                new KeyValuePair<String, UInt32>("QUEST_STATE_VALUE", 0x86DC367C),
                new KeyValuePair<String, UInt32>("QUEST_STATUS", 0xB0593288),
                new KeyValuePair<String, UInt32>("QUEST_TEMPALTE", 0xD4AE7FA7),

                new KeyValuePair<String, UInt32>("RARENAMES", 0xF09E4089),
                new KeyValuePair<String, UInt32>("RECIPELISTS", 0xE1A7A39A),
                new KeyValuePair<String, UInt32>("RECIPES", 0x3230B6BC),

                new KeyValuePair<String, UInt32>("SKILLEVENTTYPES", 0xB47A4EC5),
                new KeyValuePair<String, UInt32>("SKILLGROUPS", 0xD2FE445A),
                new KeyValuePair<String, UInt32>("SKILLS", 0xBAF5E904),
                new KeyValuePair<String, UInt32>("SKILLTABS", 0xF934EB3B),
                new KeyValuePair<String, UInt32>("SKU", 0xCF23C241),
                new KeyValuePair<String, UInt32>("SOUNDSBUSES", 0x1E760FB1),
                new KeyValuePair<String, UInt32>("SOUNDMIXSTATES", 0xDB77DD54),
                new KeyValuePair<String, UInt32>("SOUNDMIXSTATEVALUES", 0xA84F422C),
                new KeyValuePair<String, UInt32>("SOUNDOVERRIDES", 0x83228488),
                new KeyValuePair<String, UInt32>("SOUNDS", 0x1A4CAF8A),
                new KeyValuePair<String, UInt32>("SOUNDVCAS", 0xA42BCE8C),
                new KeyValuePair<String, UInt32>("SOUNDVCASETS", 0x50F90D15),
                new KeyValuePair<String, UInt32>("SPAWNCLASS", 0xBBEBD669),
                new KeyValuePair<String, UInt32>("STATES", 0xFD6839DE),
                new KeyValuePair<String, UInt32>("STATE_EVENT_TYPES", 0x72477C36),
                new KeyValuePair<String, UInt32>("STATE_LIGHTING", 0xB1B5294C),
                new KeyValuePair<String, UInt32>("STATS", 0x569C0513),
                new KeyValuePair<String, UInt32>("STATSFUNC", 0x2C085508),
                new KeyValuePair<String, UInt32>("STATSSELECTOR", 0x86DC367C),
                new KeyValuePair<String, UInt32>("STRING_FILES", 0x22FCCFEB),
                new KeyValuePair<String, UInt32>("SUBLEVEL", 0x1ACBB8F7),

                new KeyValuePair<String, UInt32>("TAG", 0xF8A155D8),
                new KeyValuePair<String, UInt32>("TASKS", 0xBF3BFFF5),
                new KeyValuePair<String, UInt32>("TASK_STATUS", 0x86DC367C),
                new KeyValuePair<String, UInt32>("TEXTURETYPES", 0xC8071451),
                new KeyValuePair<String, UInt32>("TREASURE", 0x01A8DA81),

                new KeyValuePair<String, UInt32>("UI_COMPONENT", 0x71D76819),
                new KeyValuePair<String, UInt32>("UNITEVENTS", 0x01082DBE),
                new KeyValuePair<String, UInt32>("UNITMODE_GROUPS", 0x95909737),
                new KeyValuePair<String, UInt32>("UNITMODES", 0x498E341C),
                new KeyValuePair<String, UInt32>("UNITTYPES", 0x1F9DDC98),

                new KeyValuePair<String, UInt32>("WARDROBE", 0xC625A079),
                new KeyValuePair<String, UInt32>("WARDROBE_APPEARANCE_GROUP", 0xFA7B3939),
                new KeyValuePair<String, UInt32>("WARDROBE_BLENDOP", 0x8E1FFDA1),
                new KeyValuePair<String, UInt32>("WARDROBE_BODY", 0x686002E7),
                new KeyValuePair<String, UInt32>("WARDROBE_LAYERSET", 0x8E1FFDA1),
                new KeyValuePair<String, UInt32>("WARDROBE_MODEL", 0xA6D29E2E),
                new KeyValuePair<String, UInt32>("WARDROBE_MODEL_GROUP", 0x1F0513C5),
                new KeyValuePair<String, UInt32>("WARDROBE_PART", 0x58D35B7C),
                new KeyValuePair<String, UInt32>("WARDROBE_TEXTURESET", 0xD406BEF5),
                new KeyValuePair<String, UInt32>("WARDROBE_TEXTURESET_GROUP", 0x1F0513C5),
                new KeyValuePair<String, UInt32>("WEATHER", 0x9FF616C5),
                new KeyValuePair<String, UInt32>("WEATHER_SETS", 0xE70A388C)
            };
            #endregion
        }

        #region Excel Types
        public class TypeMap
        {
            public Type DataType;
            public Boolean HasMysh;
            public Boolean HasExtended;
            public Boolean HasIndexBitRelations;
            public Boolean IgnoresTable;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct ExcelHeader
        {
            public UInt32 StructureID;
            public Int32 Unknown321;
            public Int32 Unknown322;
            public Int16 Unknown161;
            public Int16 Unknown162;
            public Int16 Unknown163;
            public Int16 Unknown164;
            public Int16 Unknown165;
            public Int16 Unknown166;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TableHeader
        {
            public Int32 Unknown1;
            public Int32 Unknown2;
            public Int16 VersionMajor;
            public Int16 Reserved1;
            public Int16 VersionMinor;
            public Int16 Reserved2;
        }

        [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
        public class OutputAttribute : Attribute
        {
            public bool IsBitmask { get; set; }
            public uint DefaultBitmask { get; set; }
            public bool IsBool { get; set; }
            public bool IsStringOffset { get; set; }
            public bool IsIntOffset { get; set; }
            public bool IsStringIndex { get; set; }
            public bool IsTableIndex { get; set; }
            public bool IsSecondaryString { get; set; }
            public string TableStringID { get; set; }
            public int SortAscendingID { get; set; }
            public int SortDistinctID { get; set; }
            public int SortPostOrderID { get; set; }
            public bool RequiresDefault { get; set; }
            public string SortColumnTwo { get; set; }
        }

        class Token
        {
            public const Int32 cxeh = 0x68657863;
            public const Int32 rcsh = 0x68736372;
            public const Int32 tysh = 0x68737974;
            public const Int32 mysh = 0x6873796D;
            public const Int32 dneh = 0x68656E64;
            public const Int32 RcshValue = 4;
            public const Int32 TyshValue = 2;
            public const Int32 DnehValue = 0;
        }

        public class ColumnTypeKeys
        {
            public const String IsFinalData = "IsFinalData";
            public const String IsExtendedProps = "IsExtendedProps";
            public const String IsStringOffset = "IsStringOffset";
            public const String IsStringIndex = "IsStringIndex";
            public const String IsRelationGenerated = "IsRelationGenerated";
            public const String IsTableIndex = "IsTableIndex";
            public const String IsBitmask = "IsBitmask";
            public const String IsBool = "IsBool";
            public const String IsIntOffset = "IsIntOffset";
            public const String IsSecondaryString = "IsSecondaryString";
            public const String RequiresDefault = "RequiresDefault";
            public const String SortAscendingID = "SortAscendingID";
            public const String SortColumnTwo = "SortColumnTwo";
            public const String SortDistinctID = "SortDistinctID";
            public const String SortPostOrderID = "SortPostOrderID";
        }

        static class IntTableDef
        {
            // t, x, 0
            public static int[] Case01 = new int[] { 2, 98, 707 };
            // t, x
            public static int[] Case02 = new int[] { 1, 3, 4, 5, 6, 14, 26, 50, 86, 516, 527, 700 };
            // t
            public static int[] Case03 = new int[] { 320, 333, 339, 347, 358, 369, 388, 399, 418, 426, 437, 448, 459, 470, 481, 538, 708, 709, 710, 711, 712 };
            // t, x
            public static int[] BitField = new int[] { 666, 667, 669, 673, 674, 680, 683, 687, 688 };
        }
        #endregion

        public static OutputAttribute GetExcelOutputAttribute(FieldInfo fieldInfo)
        {
            object[] query = fieldInfo.GetCustomAttributes(typeof(OutputAttribute), true);
            return (!(query.Length == 0)) ? (OutputAttribute)query[0] : null;
        }

        public string GetStringID()
        {
            if (!(String.IsNullOrEmpty(StringID))) return StringID;
            var query = DataTables.Where(dt => dt.Value == StructureID);
            if ((query.Count() == 1)) return StringID = query.First().Key;
            if ((String.IsNullOrEmpty(FilePath))) return String.Empty;
            return StringID = FileName.ToUpper();
        }

        public static uint GetStructureID(string stringID)
        {
            var query = DataTables.Where(dt => dt.Key == stringID);
            return (!(query.Count() == 0)) ? query.First().Value : 0;
        }

        public static TypeMap GetTypeMap(uint structureID)
        {
            var query = DataTypes.Where(dt => dt.Key == structureID);
            return (!(query.Count() == 0)) ? query.First().Value : null;
        }

        bool CheckFlag(byte[] buffer, ref int offset, int token)
        {
            return token == FileTools.ByteArrayToInt32(buffer, ref offset);
        }

        bool CheckFlag(byte[] buffer, int offset, int token)
        {
            return token == BitConverter.ToInt32(buffer, offset);
        }

        public int[] ReadIntegerTable(int offset)
        {
            int position = offset;
            int value = FileTools.ByteArrayToInt32(IntegerBuffer, position);

            while (!(value == 0))
            {
                if (IntTableDef.Case01.Contains(value))
                    position += (3 * sizeof(int));
                else if (IntTableDef.Case02.Contains(value))
                    position += (2 * sizeof(int));
                else if (IntTableDef.Case03.Contains(value))
                    position += (1 * sizeof(int));
                else if (IntTableDef.BitField.Contains(value))
                    position += (2 * sizeof(int));
                else return null;
                value = FileTools.ByteArrayToInt32(IntegerBuffer, position);
            }

            int length = (position + sizeof(int) - offset) / sizeof(int);
            return FileTools.ByteArrayToInt32Array(IntegerBuffer, ref offset, length);
        }

        public string ReadStringTable(int offset)
        {
            return offset == -1 ? String.Empty : FileTools.ByteArrayToStringASCII(StringBuffer, offset);
        }

        public byte[] ReadStringTableAsBytes(int offset)
        {
            return FileTools.GetDelimintedByteArray(StringBuffer, ref offset, 0);
        }

        public byte[] ReadExtendedProperties(int index)
        {
            return (!(ExtendedBuffer == null)) ? ExtendedBuffer[index] : null;
        }

        public string ReadSecondaryStringTable(int index)
        {
            return SecondaryStrings[index];
        }

        void ParseMyshTable(byte[] data, ref int offset)
        {
            byte[] endtoken = new byte[] { 0x64, 0x6E, 0x65, 0x68 };
            int check = 0;

            List<byte> buffer = new List<byte>();

            while (check < 4)
            {
                if (data[offset] == endtoken[check])
                {
                    check++;
                }
                else
                {
                    check = 0;
                }
                buffer.Add(data[offset++]);
            }

            offset = offset - 4;
            buffer.RemoveRange(buffer.Count - 4, 4);
            MyshBuffer = buffer.ToArray();

            //int totalAttributeCount = 0;
            //int attributeCount = 0;
            //int blockCount = 0;
            //int flagCount = 0;
            //while (offset < data.Length)
            //{
            //    ////////////// temp fix /////////////////
            //    //int f = BitConverter.ToInt32(data, offset);
            //    //if (CheckFlag(f, 0x68657863))
            //    //{
            //    //    //Debug.Write("mysh flagCount = " + flagCount + "\n");
            //    //    break;
            //    //}
            //    ////if (CheckFlag(f, 0x6873796D))
            //    ////{
            //    ////    flagCount++;
            //    ////}
            //    //offset++;
            //    //continue;
            //    ////////////// temp fix /////////////////


            //                    int flag = FileTools.ByteArrayToInt32(data, ref offset);
            //                    if (!CheckFlag(flag, 0x6873796D))
            //                    {
            //                        offset -= 4;
            //                        break;
            //                    }

            //                    int unknown1 = FileTools.ByteArrayToInt32(data, ref offset);
            //                    if (unknown1 == 0x00)
            //                    {
            //                        break;
            //                    }

            //                    int byteCount = FileTools.ByteArrayToInt32(data, ref offset);
            //                    // this is really lazy and I couldn't be bothered trying to figure out how to get a *non-zero terminated* string out of a byte array
            //                    byte[] temp = new byte[byteCount + 1];
            //                    Buffer.BlockCopy(data, offset, temp, 0, byteCount);
            //                    String str = FileTools.ByteArrayToStringASCII(temp, 0);
            //                    offset += byteCount;

            //                    int unknown2 = FileTools.ByteArrayToInt32(data, ref offset);
            //                    int type = FileTools.ByteArrayToInt32(data, ref offset); // 0x39 = single, 0x41 = has properties, 0x3C = property
            //                    int unknown3 = FileTools.ByteArrayToInt32(data, ref offset);  // usually 0x05
            //                    int unknown4 = FileTools.ByteArrayToInt32(data, ref offset);  // always 0x00?
            //                    int unknown5 = FileTools.ByteArrayToInt32(data, ref offset);  // usually 0x04
            //                    int unknown6 = FileTools.ByteArrayToInt32(data, ref offset);  // always 0x00?

            //                    if (type == 0x39)
            //                    {
            //                        continue;
            //                    }

            //                    int id = FileTools.ByteArrayToInt32(data, ref offset);
            //                    attributeCount = FileTools.ByteArrayToInt32(data, ref offset);
            //                    totalAttributeCount = attributeCount;

            //                    if (type == 0x41)
            //                    {
            //                        int attributeCountAgain = FileTools.ByteArrayToInt32(data, ref offset); // ??
            //                    }
            //                    else if (type == 0x3C)
            //                    {
            //                        if (str == "dam")
            //                        {
            //                            blockCount += 2;
            //                        }
            //                        else if (str == "dur")
            //                        {
            //                            blockCount++;
            //                        }

            //                        const int blockSize = 4 * sizeof(Int32);
            //                        if (attributeCount == 0)
            //                        {
            //                            offset += blockCount * blockSize;
            //                            continue;
            //                        }

            //                        attributeCount--;
            //                    }
            //                    else
            //                    {
            //                        throw new NotImplementedException("type not implemented!\ntype = " + type);
            //                    }

            //                    int endInt = FileTools.ByteArrayToInt32(data, ref offset);
            //                    if (endInt != 0x00)
            //                    {
            //                        int breakpoint = 1;
            //                    }

            //}
        }

        UInt32[,] CreateIndexBitRelations()
        {
            // get our index ranges
            int startIndex, endIndex;
            switch (StringID)
            {
                case "STATES":
                    // states has 10x columns to check from 3 (zero based index)
                    startIndex = 3;
                    endIndex = startIndex + 10;
                    break;

                case "UNITTYPES":
                    // unittypes has 16x columns to check from 2
                    startIndex = 2;
                    endIndex = startIndex + 15;
                    break;

                default:
                    // this shouldn't happen
                    return new uint[0, 0];
            }


            // need 1 bit for every row; 32 bits per int
            int intCount = (Count >> 5) + 1;
            UInt32[,] indexBitRelations = new UInt32[Count, intCount];
            bool[] isGenerated = new bool[Count];


            // generate binary relation table
            for (int i = 0; i < Count; i++)
            {
                if (isGenerated[i]) continue;

                _GenerateIndexBitRelation(ref indexBitRelations, ref isGenerated, i, startIndex, endIndex);
                isGenerated[i] = true;
            }

            return indexBitRelations;
        }

        void _GenerateIndexBitRelation(ref uint[,] indexBitRelations, ref bool[] isGenerated, int index, int startIndex, int endIndex)
        {
            int intCount = (Count >> 5) + 1;

            // need column fields
            Object row = Rows[index];
            FieldInfo[] fields = Rows[0].GetType().GetFields();

            // each row has its own bit high
            int intOffset = index >> 5;
            indexBitRelations[index, intOffset] |= (uint)1 << index;

            // check isAX columns
            for (int j = startIndex; j < endIndex; j++)
            {
                int value = (int)fields[j].GetValue(row);
                if (value == -1) continue; // at first -1, no other values have been found (tested)

                intOffset = value >> 5;
                indexBitRelations[index, intOffset] |= (uint)1 << value;

                if (!isGenerated[value])
                {
                    _GenerateIndexBitRelation(ref indexBitRelations, ref isGenerated, value, startIndex, endIndex);
                    isGenerated[value] = true;
                }

                // now we need to | the related row and its relations
                for (int relationIndex = 0; relationIndex < intCount; relationIndex++)
                {
                    indexBitRelations[index, relationIndex] |= indexBitRelations[value, relationIndex];
                }
            }
        }


        void DoPrecedenceHack(FieldInfo fieldInfo)
        {
            OutputAttribute attribute = GetExcelOutputAttribute(fieldInfo);

            if ((fieldInfo.FieldType == typeof(string)))
            {
                foreach (object row in Rows)
                {
                    fieldInfo.SetValue(row, ((string)fieldInfo.GetValue(row)).Replace("-", "8"));
                    fieldInfo.SetValue(row, ((string)fieldInfo.GetValue(row)).Replace("_", "9"));
                }
            }

            if ((attribute.IsStringOffset))
            {
                for (int i = 0; i < StringBuffer.Length; i++)
                {
                    switch (StringBuffer[i])
                    {
                        case (byte)'-':
                            StringBuffer[i] = (byte)'8';
                            break;
                        case (byte)'_':
                            StringBuffer[i] = (byte)'9';
                            break;
                    }
                }
            }

            if (!(String.IsNullOrEmpty(attribute.SortColumnTwo)))
            {
                FieldInfo fieldInfo2 = DataType.GetField(attribute.SortColumnTwo);
                if (fieldInfo2.FieldType == typeof(string))
                {
                    foreach (object row in Rows)
                    {
                        fieldInfo.SetValue(row, ((string)fieldInfo.GetValue(row)).Replace("-", "8"));
                        fieldInfo.SetValue(row, ((string)fieldInfo.GetValue(row)).Replace("_", "9"));
                    }
                }
            }
        }

        void UndoPrecedenceHack(FieldInfo fieldInfo)
        {

        }

        int[][] CreateSortIndices()
        {
            int[][] customSorts = new int[4][];

            foreach (FieldInfo fieldInfo in DataType.GetFields())
            {
                OutputAttribute attribute = GetExcelOutputAttribute(fieldInfo);
                if ((attribute == null)) continue;
                if ((attribute.SortAscendingID == 0) && (attribute.SortDistinctID == 0) && (attribute.SortPostOrderID == 0)) continue;

                // Precedence Hack
                // This Excel files order special characters differently to convention
                DoPrecedenceHack(fieldInfo);

                #region Ascending Sort
                // Standard Ascending Sort
                if (attribute.SortAscendingID != 0)
                {
                    // Gets the sort array position
                    int pos = attribute.SortAscendingID - 1;
                    // Checks for code field
                    FieldInfo codeFieldInfo = DataType.GetField("code");

                    // 1 Ascending Sort Column
                    if (String.IsNullOrEmpty(attribute.SortColumnTwo))
                    {
                        // Requires Default
                        if ((attribute.RequiresDefault))
                        {
                            int defaultRow = (from element in Rows
                                              orderby fieldInfo.GetValue(element)
                                              select Rows.IndexOf(element)).First();

                            // Contains a code column
                            if (codeFieldInfo != null)
                            {
                                if (codeFieldInfo.FieldType == typeof(short))
                                {
                                    var sortedList = from element in Rows
                                                     where (((short)codeFieldInfo.GetValue(element) != 0 &&
                                                             fieldInfo.GetValue(element).ToString() != "-1" &&
                                                             !(String.IsNullOrEmpty(fieldInfo.GetValue(element).ToString()))) ||
                                                             Rows.IndexOf(element) == defaultRow)
                                                     orderby fieldInfo.GetValue(element)
                                                     select Rows.IndexOf(element);
                                    customSorts[pos] = sortedList.ToArray();
                                }
                                else
                                {
                                    var sortedList = from element in Rows
                                                     where (((int)codeFieldInfo.GetValue(element) != 0 &&
                                                             fieldInfo.GetValue(element).ToString() != "-1" &&
                                                             !String.IsNullOrEmpty(fieldInfo.GetValue(element).ToString())) ||
                                                             Rows.IndexOf(element) == defaultRow)
                                                     orderby fieldInfo.GetValue(element)
                                                     select Rows.IndexOf(element);
                                    customSorts[pos] = sortedList.ToArray();
                                }
                            }
                            // Doesnt contain a code column
                            else
                            {
                                var sortedList = from element in Rows
                                                 where ((fieldInfo.GetValue(element).ToString() != "-1" &&
                                                         !String.IsNullOrEmpty(fieldInfo.GetValue(element).ToString())) ||
                                                         Rows.IndexOf(element) == defaultRow)
                                                 orderby fieldInfo.GetValue(element)
                                                 select Rows.IndexOf(element);
                                customSorts[pos] = sortedList.ToArray();
                            }
                        }
                        // Doesnt require a default row
                        else
                        {
                            // Contains a code column
                            if (!(codeFieldInfo == null))
                            {
                                if (codeFieldInfo.FieldType == typeof(short))
                                {
                                    var sortedList = from element in Rows
                                                     where (!((short)codeFieldInfo.GetValue(element) == 0) &&
                                                            !(fieldInfo.GetValue(element).ToString() == "-1") &&
                                                            !(String.IsNullOrEmpty(fieldInfo.GetValue(element).ToString())))
                                                     orderby fieldInfo.GetValue(element)
                                                     select Rows.IndexOf(element);
                                    customSorts[pos] = sortedList.ToArray();
                                }
                                else
                                {
                                    // Is a string offset
                                    if (attribute.IsStringOffset)
                                    {
                                        var sortedList = from element in Rows
                                                         where (int)codeFieldInfo.GetValue(element) != 0
                                                         orderby ReadStringTable((int)fieldInfo.GetValue(element))
                                                         select Rows.IndexOf(element);
                                        customSorts[pos] = sortedList.ToArray();
                                    }
                                    else
                                    {
                                        var sortedList = from element in Rows
                                                         where (!((int)codeFieldInfo.GetValue(element) == 0) &&
                                                                !(fieldInfo.GetValue(element).ToString() == "-1") &&
                                                                !(String.IsNullOrEmpty(fieldInfo.GetValue(element).ToString())))
                                                         orderby fieldInfo.GetValue(element)
                                                         select Rows.IndexOf(element);
                                        customSorts[pos] = sortedList.ToArray();
                                    }
                                }
                            }
                            // Doesn't contain a code column
                            else
                            {
                                var sortedList = from element in Rows
                                                 where (!(fieldInfo.GetValue(element).ToString() == "-1") &&
                                                        !(String.IsNullOrEmpty(fieldInfo.GetValue(element).ToString())))
                                                 orderby fieldInfo.GetValue(element)
                                                 select Rows.IndexOf(element);
                                customSorts[pos] = sortedList.ToArray();
                            }
                        }
                    }
                    // Sort by 2 columns - only used in wardrobe tables
                    else
                    {
                        FieldInfo fieldInfo2 = DataType.GetField(attribute.SortColumnTwo);
                        var sortedList = from element in Rows
                                         where (int)fieldInfo.GetValue(element) != 0 // wardrobe tables exclude index 0
                                         orderby fieldInfo.GetValue(element), fieldInfo2.GetValue(element)
                                         select Rows.IndexOf(element);
                        customSorts[pos] = sortedList.ToArray();
                    }
                }
                #endregion

                #region Distinct Sort
                // A distinct sort
                if (!(attribute.SortDistinctID == 0))
                {
                    int pos = attribute.SortDistinctID - 1;
                    var sortedList = from element in Rows
                                     where (!(fieldInfo.GetValue(element).ToString() == "-1"))
                                     orderby fieldInfo.GetValue(element)
                                     select element;
                    int lastValue = -1;
                    List<int> distinctList = new List<int>();
                    foreach (Object element in sortedList)
                    {
                        if (!(lastValue == (int)fieldInfo.GetValue(element)))
                        {
                            lastValue = (int)fieldInfo.GetValue(element);
                            distinctList.Add(Rows.IndexOf(element));
                        }
                    }
                    customSorts[pos] = distinctList.ToArray();
                }
                #endregion

                #region Post Order Sort
                // Post order tree sort. only used in the affix class
                if (!(attribute.SortPostOrderID == 0))
                {
                    int pos = attribute.SortPostOrderID - 1;
                    var sortedList = from element in Rows
                                     where (!(fieldInfo.GetValue(element).ToString() == "0"))
                                     orderby fieldInfo.GetValue(element)
                                     select Rows.IndexOf(element);
                    customSorts[pos] = sortedList.ToArray();
                }
                #endregion

                // Remove precedence hack
                // UndoPrecedenceHack(fieldInfo);
            }

            return customSorts;
        }
    }
}
