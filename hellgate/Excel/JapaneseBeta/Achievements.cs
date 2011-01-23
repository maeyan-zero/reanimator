using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class AchievementsBeta
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
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String icon;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ACHIEVEMENTS")]
        public Int32 linkedAchievement;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ACHIEVEMENTS")]
        public Int32 progressionAchievement;
        public Int32 revealCondition;
        public Int32 revealValue;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ACHIEVEMENTS")]
        public Int32 revealParentAchievement;
        public Int32 hideCondition;
        public Int32 hideValue;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ACHIEVEMENTS")]
        public Int32 hideParentAchievement;
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
        public Int32 monsterUnitType0;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 monsterUnitType1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 monsterUnitType2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 monsterUnitType3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 monsterUnitType4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 monsterUnitType5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 monsterUnitType6;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 monsterUnitType7;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 monsterUnitType8;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
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
        [ExcelOutput(IsTableIndex = true, TableStringId = "QUEST")]
        public Int32 questTaskComplete;
        public Int32 randomQuests;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 monster;
        [ExcelOutput(IsTableIndex = true, TableStringId = "OBJECTS")]
        public Int32 Object;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 itemUnitType0;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 itemUnitType1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 itemUnitType2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 itemUnitType3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 itemUnitType4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 itemUnitType5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 itemUnitType6;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 itemUnitType7;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 itemUnitType8;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 itemUnitType9;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEM_QUALITY")]
        public Int32 quality;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 item;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 skill;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 level;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "")]//table C8
        public Int32 fieldLevel;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 stat;
        public Int32 rewardAchievementPoints;
        [ExcelOutput(IsTableIndex = true, TableStringId = "TREASURE")]
        public Int32 rewardTreasureClass;
        [ExcelOutput(IsTableIndex = true, TableStringId = "EMOTES")]
        public Int32 rewardEmote;
        public Int32 rewardXP;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 rewardSkill;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 rewardTitle;
        [ExcelOutput(IsScript = true)]
        public Int32 rewardScript;
		public bool cheatComplete;
        [ExcelOutput(IsTableIndex = true, TableStringId = "QUEST")]
        public Int32 quest;
		
    }
}
