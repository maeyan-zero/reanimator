using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class AchievementsRow
    {
        ExcelFile.TableHeader header;

        Int32 undefined;
        [ExcelOutput(SortId = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(SortId = 2)]
        public Int32 code;
        [ExcelFile.ExcelOutput(IsStringId = true, Table = "Strings_Strings")]
        public Int32 nameString;
        [ExcelFile.ExcelOutput(IsStringId = true, Table = "Strings_Strings")]
        public Int32 descripFormatString;
        [ExcelFile.ExcelOutput(IsStringId = true, Table = "Strings_Strings")]
        public Int32 detailsString;
        [ExcelFile.ExcelOutput(IsStringId = true, Table = "Strings_Strings")]
        Int32 rewardTypeString;
        public Int32 revealCondition;
        public Int32 revealValue;
        Int32 revealParentAchievement;
        public Int32 playerClass;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined2;
        public Int32 type;
        Int32 notActiveTillParentComplete;
        public Int32 completeNumber;
        public Int32 param1;
        public Int32 unitType0;
        Int32 unitType1;
        Int32 unitType2;
        Int32 unitType3;
        Int32 unitType4;
        Int32 unitType5;
        Int32 unitType6;
        Int32 unitType7;
        Int32 unitType8;
        Int32 unitType9;
        public Int32 questTaskComplete;//idx
        public Int32 randomQuests;
        public Int32 monster;//idx
        public Int32 Object;//idx
        public Int32 item;//idx
        public Int32 quality;//idx
        public Int32 skill;//idx
        public Int32 level;//idx
        public Int32 stat;//idx
        public Int32 rewardAchievementPoints;
        Int32 rewardTreasureClass;//idx
        public Int32 rewardXP;
        Int32 rewardSkill;//idx
        [ExcelOutput(IsIntOffset = true)]
        Int32 rewardScript;
    }
}