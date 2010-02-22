using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class Achievements : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class AchievementsTable
        {
            TableHeader header;

            Int32 undefined;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 code;
            [ExcelTable.ExcelOutput(IsStringId = true, StringTable = "Strings_Strings")]
            public Int32 nameString;
            [ExcelTable.ExcelOutput(IsStringId = true, StringTable = "Strings_Strings")]
            public Int32 descripFormatString;
            [ExcelTable.ExcelOutput(IsStringId = true, StringTable = "Strings_Strings")]
            public Int32 detailsString;
            [ExcelTable.ExcelOutput(IsStringId = true, StringTable = "Strings_Strings")]
            public Int32 rewardTypeString;
            public Int32 revealCondition;
            public Int32 revealValue;
            public Int32 revealParentAchievement;//idx
            public Int32 playerClass;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            Int32[] undefined2;
            public Int32 type;
            public Int32 notActiveTillParentComplete;//idx
            public Int32 completeNumber;
            public Int32 param1;
            public Int32 unitType0;
            public Int32 unitType1;
            public Int32 unitType2;
            public Int32 unitType3;
            public Int32 unitType4;
            public Int32 unitType5;
            public Int32 unitType6;
            public Int32 unitType7;
            public Int32 unitType8;
            public Int32 unitType9;
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
            public Int32 rewardTreasureClass;//idx
            public Int32 rewardXP;
            public Int32 rewardSkill;//idx
            public Int32 rewardScript;//intptr
        }

        public Achievements(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<AchievementsTable>(data, ref offset, Count);
        }
    }
}
