using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MusicConditions
    {
        RowHeader header;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSIC_REF")]
        public Int32 musicRef;//idx
        public Int32 unknownA;
        public Int32 unknownB;
        public Int32 unknownC;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;//seems to be by name and numerical value. CG_1-5_* is before CG_11-15_* in the index, but not in the table data.
        [ExcelOutput(IsScript = true)]
        public Int32 condition1;//intptr
        public Int32 percentChance1;
        [ExcelOutput(IsScript = true)]
        public Int32 characterClass1;//intptr
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICCONDITIONS")]
        public Int32 useConditionCode1;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICGROOVELEVELS")]
        public Int32 groove1;//idx
        public Int32 unknown1_1;
        public Int32 unknown1_2;
        public Int32 unknown1_3;
        public Int32 unknown1_4;
        public Int32 unknown1_5;
        public Int32 unknown1_6;
        public Int32 unknown1_7;
        public Int32 unknown1_8;
        public Int32 unknown1_9;
        public Int32 unknown1_10;
        public Int32 unknown1_11;
        [ExcelOutput(IsScript = true)]
        public Int32 condition2;//intptr
        public Int32 percentChance2;
        [ExcelOutput(IsScript = true)]
        public Int32 characterClass2;//intptr
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICCONDITIONS")]
        public Int32 useConditionCode2;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICGROOVELEVELS")]
        public Int32 groove2;//idx
        public Int32 unknown2_1;
        public Int32 unknown2_2;
        public Int32 unknown2_3;
        public Int32 unknown2_4;
        public Int32 unknown2_5;
        public Int32 unknown2_6;
        public Int32 unknown2_7;
        public Int32 unknown2_8;
        public Int32 unknown2_9;
        public Int32 unknown2_10;
        public Int32 unknown2_11;
        [ExcelOutput(IsScript = true)]
        public Int32 condition3;//intptr
        public Int32 percentChance3;
        [ExcelOutput(IsScript = true)]
        public Int32 characterClass3;//intptr
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICCONDITIONS")]
        public Int32 useConditionCode3;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICGROOVELEVELS")]
        public Int32 groove3;//idx
        public Int32 unknown3_1;
        public Int32 unknown3_2;
        public Int32 unknown3_3;
        public Int32 unknown3_4;
        public Int32 unknown3_5;
        public Int32 unknown3_6;
        public Int32 unknown3_7;
        public Int32 unknown3_8;
        public Int32 unknown3_9;
        public Int32 unknown3_10;
        public Int32 unknown3_11;
        [ExcelOutput(IsScript = true)]
        public Int32 condition4;//intptr
        public Int32 percentChance4;
        [ExcelOutput(IsScript = true)]
        public Int32 characterClass4;//intptr
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICCONDITIONS")]
        public Int32 useConditionCode4;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICGROOVELEVELS")]
        public Int32 groove4;//idx
        public Int32 unknown4_1;
        public Int32 unknown4_2;
        public Int32 unknown4_3;
        public Int32 unknown4_4;
        public Int32 unknown4_5;
        public Int32 unknown4_6;
        public Int32 unknown4_7;
        public Int32 unknown4_8;
        public Int32 unknown4_9;
        public Int32 unknown4_10;
        public Int32 unknown4_11;
        [ExcelOutput(IsScript = true)]
        public Int32 condition5;//intptr
        public Int32 percentChance5;
        [ExcelOutput(IsScript = true)]
        public Int32 characterClass5;//intptr
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICCONDITIONS")]
        public Int32 useConditionCode5;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICGROOVELEVELS")]
        public Int32 groove5;//idx
        public Int32 unknown5_1;
        public Int32 unknown5_2;
        public Int32 unknown5_3;
        public Int32 unknown5_4;
        public Int32 unknown5_5;
        public Int32 unknown5_6;
        public Int32 unknown5_7;
        public Int32 unknown5_8;
        public Int32 unknown5_9;
        public Int32 unknown5_10;
        public Int32 unknown5_11;
        [ExcelOutput(IsScript = true)]
        public Int32 condition6;//intptr
        public Int32 percentChance6;
        [ExcelOutput(IsScript = true)]
        public Int32 characterClass6;//intptr
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICGONDITIONS")]
        public Int32 useConditionCode6;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICGROOVELEVELS")]
        public Int32 groove6;//idx
        public Int32 unknown6_1;
        public Int32 unknown6_2;
        public Int32 unknown6_3;
        public Int32 unknown6_4;
        public Int32 unknown6_5;
        public Int32 unknown6_6;
        public Int32 unknown6_7;
        public Int32 unknown6_8;
        public Int32 unknown6_9;
        public Int32 unknown6_10;
        public Int32 unknown6_11;
        [ExcelOutput(IsScript = true)]
        public Int32 condition7;//intptr
        public Int32 percentChance7;
        [ExcelOutput(IsScript = true)]
        public Int32 characterClass7;//intptr
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICCONDITIONS")]
        public Int32 useConditionCode7;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICGROOVELEVELS")]
        public Int32 groove7;//idx
        public Int32 unknown7_1;
        public Int32 unknown7_2;
        public Int32 unknown7_3;
        public Int32 unknown7_4;
        public Int32 unknown7_5;
        public Int32 unknown7_6;
        public Int32 unknown7_7;
        public Int32 unknown7_8;
        public Int32 unknown7_9;
        public Int32 unknown7_10;
        public Int32 unknown7_11;
        [ExcelOutput(IsScript = true)]
        public Int32 condition8;//intptr
        public Int32 percentChance8;
        [ExcelOutput(IsScript = true)]
        public Int32 characterClass8;//intptr
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICCONDITIONS")]
        public Int32 useConditionCode8;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICGROOVELEVELS")]
        public Int32 groove8;//idx
        public Int32 unknown8_1;
        public Int32 unknown8_2;
        public Int32 unknown8_3;
        public Int32 unknown8_4;
        public Int32 unknown8_5;
        public Int32 unknown8_6;
        public Int32 unknown8_7;
        public Int32 unknown8_8;
        public Int32 unknown8_9;
        public Int32 unknown8_10;
        public Int32 unknown8_11;
        [ExcelOutput(IsScript = true)]
        public Int32 condition9;//intptr
        public Int32 percentChance9;
        [ExcelOutput(IsScript = true)]
        public Int32 characterClass9;//intptr
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICCONDITIONS")]
        public Int32 useConditionCode9;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICGROOVELEVELS")]
        public Int32 groove9;//idx
        public Int32 unknown9_1;
        public Int32 unknown9_2;
        public Int32 unknown9_3;
        public Int32 unknown9_4;
        public Int32 unknown9_5;
        public Int32 unknown9_6;
        public Int32 unknown9_7;
        public Int32 unknown9_8;
        public Int32 unknown9_9;
        public Int32 unknown9_10;
        public Int32 unknown9_11;
        [ExcelOutput(IsScript = true)]
        public Int32 condition10;//intptr
        public Int32 percentChance10;
        [ExcelOutput(IsScript = true)]
        public Int32 characterClass10;//intptr
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICCONDITIONS")]
        public Int32 useConditionCode10;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICGROOVELEVELS")]
        public Int32 groove10;//idx
        public Int32 unknown10_1;
        public Int32 unknown10_2;
        public Int32 unknown10_3;
        public Int32 unknown10_4;
        public Int32 unknown10_5;
        public Int32 unknown10_6;
        public Int32 unknown10_7;
        public Int32 unknown10_8;
        public Int32 unknown10_9;
        public Int32 unknown10_10;
        public Int32 unknown10_11;
    }
}