using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Achievements
    {
        TableHeader header;
        Int32 undefined;
        [ExcelOutput(SortAscendingID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String name;
        [ExcelOutput(SortAscendingID = 2)]
        public Int32 code;
        [ExcelOutput(IsStringID = true)]
        public Int32 nameString;
        [ExcelOutput(IsStringID = true)]
        public Int32 descripFormatString;
        [ExcelOutput(IsStringID = true)]
        public Int32 detailsString;
        [ExcelOutput(IsStringID = true)]
        public Int32 rewardTypeString;
        public Int32 revealCondition;
        public Int32 revealValue;
        [ExcelOutput(IsTableIndex = true, TableStringID = "ACHIEVEMENTS")]
        public Int32 revealParentAchievement;
        [ExcelOutput(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 playerClass0;
        [ExcelOutput(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 playerClass1;
        [ExcelOutput(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 playerClass2;
        [ExcelOutput(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 playerClass3;
        [ExcelOutput(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 playerClass4;
        [ExcelOutput(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 playerClass5;
        [ExcelOutput(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 playerClass6;
        [ExcelOutput(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 playerClass7;
        [ExcelOutput(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 playerClass8;
        [ExcelOutput(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 playerClass9;
        public Int32 type;
        [ExcelOutput(IsTableIndex = true, TableStringID = "ACHIEVEMENTS")]
        public Int32 notActiveTillParentComplete;
        public Int32 completeNumber;
        public Int32 param1;
        [ExcelOutput(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 unitType0;
        [ExcelOutput(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 unitType1;
        [ExcelOutput(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 unitType2;
        [ExcelOutput(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 unitType3;
        [ExcelOutput(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 unitType4;
        [ExcelOutput(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 unitType5;
        [ExcelOutput(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 unitType6;
        [ExcelOutput(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 unitType7;
        [ExcelOutput(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 unitType8;
        [ExcelOutput(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 unitType9;
        [ExcelOutput(IsTableIndex = true, TableStringID = "QUEST")]
        public Int32 questTaskComplete;
        public Int32 randomQuests;
        [ExcelOutput(IsTableIndex = true, TableStringID = "MONSTERS")]
        public Int32 monster;
        [ExcelOutput(IsTableIndex = true, TableStringID = "OBJECTS")]
        public Int32 Object;
        [ExcelOutput(IsTableIndex = true, TableStringID = "ITEMS")]
        public Int32 item;
        [ExcelOutput(IsTableIndex = true, TableStringID = "ITEM_QUALITY")]
        public Int32 quality;
        [ExcelOutput(IsTableIndex = true, TableStringID = "SKILLS")]
        public Int32 skill;
        [ExcelOutput(IsTableIndex = true, TableStringID = "LEVEL")]
        public Int32 level;
        [ExcelOutput(IsTableIndex = true, TableStringID = "STATS")]
        public Int32 stat;
        public Int32 rewardAchievementPoints;
        [ExcelOutput(IsTableIndex = true, TableStringID = "TREASURE")]
        public Int32 rewardTreasureClass;
        public Int32 rewardXP;
        [ExcelOutput(IsTableIndex = true, TableStringID = "SKILLS")]
        public Int32 rewardSkill;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 rewardScript;
    }
}
