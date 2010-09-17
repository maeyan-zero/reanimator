using System;
using System.Runtime.InteropServices;
using ExcelAttribute = Hellgate.ExcelFile.ExcelAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Achievements
    {
        TableHeader header;
        UInt32 null01;
        [ExcelAttribute(SortID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String name;
        [ExcelAttribute(SortID = 2)]
        public UInt32 code;
        [ExcelAttribute(IsStringIndex = true)]
        public Int32 nameString;
        [ExcelAttribute(IsStringIndex = true)]
        public Int32 descripFormatString;
        [ExcelAttribute(IsStringIndex = true)]
        public Int32 detailsString;
        [ExcelAttribute(IsStringIndex = true)]
        public Int32 rewardTypeString;
        [ExcelAttribute(IsValueList = true)]
        public Reveal revealCondition;
        public UInt32 revealValue;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "ACHIEVEMENTS")]
        public Int32 revealParentAchievement;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNIT_TYPES")]
        public Int32 playerClass0;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNIT_TYPES")]
        public Int32 playerClass1;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNIT_TYPES")]
        public Int32 playerClass2;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNIT_TYPES")]
        public Int32 playerClass3;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNIT_TYPES")]
        public Int32 playerClass4;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNIT_TYPES")]
        public Int32 playerClass5;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNIT_TYPES")]
        public Int32 playerClass6;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNIT_TYPES")]
        public Int32 playerClass7;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNIT_TYPES")]
        public Int32 playerClass8;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNIT_TYPES")]
        public Int32 playerClass9;
        [ExcelAttribute(IsValueList = true)]
        public Kind type;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "ACHIEVEMENTS")]
        public Int32 notActiveTillParentComplete;
        public UInt32 completeNumber;
        public UInt32 param1;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNIT_TYPES")]
        public Int32 unitType0;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNIT_TYPES")]
        public Int32 unitType1;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNIT_TYPES")]
        public Int32 unitType2;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNIT_TYPES")]
        public Int32 unitType3;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNIT_TYPES")]
        public Int32 unitType4;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNIT_TYPES")]
        public Int32 unitType5;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNIT_TYPES")]
        public Int32 unitType6;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNIT_TYPES")]
        public Int32 unitType7;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNIT_TYPES")]
        public Int32 unitType8;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNIT_TYPES")]
        public Int32 unitType9;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "QUEST")]
        public Int32 questTaskComplete;
        public UInt32 randomQuests;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "MONSTERS")]
        public Int32 monster;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "OBJECTS")]
        public Int32 Object;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "ITEMS")]
        public Int32 item;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "ITEM_QUALITY")]
        public Int32 quality;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "SKILLS")]
        public Int32 skill;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "LEVEL")]
        public Int32 level;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "STATS")]
        public Int32 stat;
        public UInt32 rewardAchievementPoints;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "TREASURE")]
        public Int32 rewardTreasureClass;
        public UInt32 rewardXP;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "SKILLS")]
        public Int32 rewardSkill;
        [ExcelAttribute(IsIntOffset = true)]
        public UInt32 rewardScript;

        public enum Kind : uint
        {
            killMonsters = 0,
            killWithWeapon = 1,
            equipItems = 2,
            useSkill = 3,
            speedKills = 4,
            collectStat = 5,
            _null01 = 6,
            completeQuests = 7,
            winMinigame = 8,
            useItems = 9,
            modItems = 10,
            craftItems = 11,
            upgradeItems = 12,
            identifyItems = 13,
            dismantleItems = 14,
            speedLevel = 15,
            pointsInSkill = 16,
            learnAllSkills = 17,
            findLevel = 18,
            _null02 = 19,
            _null03 = 20,
            fillInventory = 21
        }

        public enum Reveal : uint
        {
            always = 0,
            onValue = 1,
            onCompletion = 2
        }
    }
}