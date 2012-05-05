using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Achievements
    {
        RowHeader header;
        Int32 undefined;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String name;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 nameString;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 descripFormatString;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 detailsString;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 rewardTypeString;
        public RevealCondition revealCondition;
        public Int32 revealValue;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ACHIEVEMENTS")]
        public Int32 revealParentAchievement;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 playerClass0;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 playerClass1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 playerClass2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 playerClass3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 playerClass4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 playerClass5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 playerClass6;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 playerClass7;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 playerClass8;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 playerClass9;
        public Int32 type;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ACHIEVEMENTS")]
        public Int32 notActiveTillParentComplete;
        public Int32 completeNumber;
        public Int32 param1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 unitType0;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 unitType1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 unitType2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 unitType3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 unitType4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 unitType5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 unitType6;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 unitType7;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 unitType8;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 unitType9;
        [ExcelOutput(IsTableIndex = true, TableStringId = "QUEST")]
        public Int32 questTaskComplete;
        public Int32 randomQuests;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 monster;
        [ExcelOutput(IsTableIndex = true, TableStringId = "OBJECTS")]
        public Int32 Object;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 item;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEM_QUALITY")]
        public Int32 quality;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 skill;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 level;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 stat;
        public Int32 rewardAchievementPoints;
        [ExcelOutput(IsTableIndex = true, TableStringId = "TREASURE")]
        public Int32 rewardTreasureClass;
        public Int32 rewardXP;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 rewardSkill;
        [ExcelOutput(IsScript = true)]
        public Int32 rewardScript;

        public enum RevealCondition
        {
            Always = 0,
            AmtComplete = 1,
            Completion = 2,
            ParentComplete = 3
        }

        public enum AchievementType
        {
            Null = -1,
            Kill = 0,
            WeaponKill = 1,
            Equip = 2,
            SkillKill = 3,
            TimedKills = 4,
            StatValue = 5,
            Collect = 6,
            QuestComplete = 7,
            WinMinigame = 8,
            ItemUse = 9,
            ItemMod = 10,
            ItemCraft = 11,
            ItemUpgrade = 12,
            ItemIdentify = 13,
            ItemDismantle = 14,
            LevelTime = 15,
            SkillToLevel = 16,
            FinishSkillTree = 17,
            LevelFind = 18,
            PartyInvite = 19,
            PartyAccept = 20,
            InventoryFill = 21
        }
    }
}
