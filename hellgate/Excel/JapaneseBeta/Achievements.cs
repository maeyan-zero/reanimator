using System;
using System.Runtime.InteropServices;

namespace Hellgate.Excel.JapaneseBeta
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class AchievementsBeta
    {
        ExcelFile.RowHeader header;
        Int32 undefined;
        [ExcelFile.OutputAttribute(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String name;
        [ExcelFile.OutputAttribute(SortColumnOrder = 2)]
        public Int32 code;
        [ExcelFile.OutputAttribute(IsStringIndex = true)]
        public Int32 nameString;
        [ExcelFile.OutputAttribute(IsStringIndex = true)]
        public Int32 descripFormatString;
        [ExcelFile.OutputAttribute(IsStringIndex = true)]
        public Int32 detailsString;
        [ExcelFile.OutputAttribute(IsStringIndex = true)]
        public Int32 rewardTypeString;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String icon;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "ACHIEVEMENTS")]
        public Int32 linkedAchievement;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "ACHIEVEMENTS")]
        public Int32 progressionAchievement;
        public RevealCondition revealCondition;
        public Int32 revealValue;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "ACHIEVEMENTS")]
        public Int32 revealParentAchievement;
        public HideCondition hideCondition;
        public Int32 hideValue;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "ACHIEVEMENTS")]
        public Int32 hideParentAchievement;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 playerClass0;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 playerClass1;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 playerClass2;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 playerClass3;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 playerClass4;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 playerClass5;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 playerClass6;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 playerClass7;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 playerClass8;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 playerClass9;
        public Type type;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "ACHIEVEMENTS")]
        public Int32 notActiveTillParentComplete;
        public Int32 completeNumber;
        public Int32 param1;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 monsterUnitType0;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 monsterUnitType1;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 monsterUnitType2;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 monsterUnitType3;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 monsterUnitType4;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 monsterUnitType5;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 monsterUnitType6;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 monsterUnitType7;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 monsterUnitType8;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 monsterUnitType9;		
        //[ExcelOutput(IsTableIndex = true, TableStringId = "")]//table C9
        public Int32 pvpGameType0;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "")]
        public Int32 pvpGameType1;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "")]
        public Int32 pvpGameType2;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "")]
        public Int32 pvpGameType3;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "")]
        public Int32 pvpGameType4;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "QUEST")]
        public Int32 questTaskComplete;
        public Int32 randomQuests;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 monster;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "OBJECTS")]
        public Int32 Object;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 itemUnitType0;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 itemUnitType1;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 itemUnitType2;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 itemUnitType3;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 itemUnitType4;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 itemUnitType5;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 itemUnitType6;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 itemUnitType7;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 itemUnitType8;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 itemUnitType9;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "ITEM_QUALITY")]
        public Int32 quality;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 item;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 skill;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 level;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "")]//table C8
        public Int32 fieldLevel;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 stat;
        public Int32 rewardAchievementPoints;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "TREASURE")]
        public Int32 rewardTreasureClass;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "EMOTES")]
        public Int32 rewardEmote;
        public Int32 rewardXP;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 rewardSkill;
        [ExcelFile.OutputAttribute(IsStringIndex = true)]
        public Int32 rewardTitle;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 rewardScript;
		public bool cheatComplete;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "QUEST")]
        public Int32 quest;

        public enum RevealCondition
        {
            Always = 0,
            AmtComplete = 1,
            Completion = 2,
            ParentComplete = 3
        }

        public enum HideCondition
        {
            never = 0,
            AmtComplete = 1,
            ParentComplete = 2
        }
        public enum Type
        {
            Null = -1,
            Kill = 0,
            KilledBy = 1,
            WeaponKill = 2,
            Equip = 3,
            SkillKill = 4,
            TimedKills = 5,
            StatValue = 6,
            Collect = 7,
            QuestComplete = 8,
            TutorialComplete = 9,
            MainQuestComplete = 10,
            TwoInventoryComplete = 11,
            AbyssQuestComplete = 12,
            TokyoAct1QuestComplete = 13,
            EachQuestComplete = 14,
            MinigameWin = 15,
            ItemUse = 16,
            ItemMod = 17,
            ItemCraft = 18,
            ItemUpgrade = 19,
            ItemIdentify = 20,
            ItemDismantle = 21,
            LevelTime = 22,
            SkillToLevel = 23,
            FinishSkillTree = 24,
            LevelFind = 25,
            PartyInvite = 26,
            PartyAccept = 27,
            InventoryFill = 28,
            PvPGameWon = 29,
            PvPTopKills = 30
        }
    }
}
