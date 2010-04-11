using System;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class MusicConditions : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class MusicConditionsTable
        {
            TableHeader header;

            public Int32 unknown1;
            public Int32 unknown2;
            public Int32 unknown3;
            public Int32 unknown4;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;
            public Int32 unknown1_1;
            public Int32 unknown2_1;
            public Int32 unknown3_1;
            public Int32 unknown4_1;
            public Int32 unknown5_1;
            public Int32 unknown6_1;
            public Int32 unknown7_1;
            public Int32 unknown8_1;
            public Int32 unknown9_1;
            public Int32 unknown10_1;
            public Int32 unknown11_1;
            public Int32 unknown12_1;
            public Int32 unknown13_1;
            public Int32 unknown14_1;
            public Int32 unknown15_1;
            public Int32 unknown16_1;
            public Int32 unknown1_2;
            public Int32 unknown2_2;
            public Int32 unknown3_2;
            public Int32 unknown4_2;
            public Int32 unknown5_2;
            public Int32 unknown6_2;
            public Int32 unknown7_2;
            public Int32 unknown8_2;
            public Int32 unknown9_2;
            public Int32 unknown10_2;
            public Int32 unknown11_2;
            public Int32 unknown12_2;
            public Int32 unknown13_2;
            public Int32 unknown14_2;
            public Int32 unknown15_2;
            public Int32 unknown16_2;
            public Int32 unknown1_3;
            public Int32 unknown2_3;
            public Int32 unknown3_3;
            public Int32 unknown4_3;
            public Int32 unknown5_3;
            public Int32 unknown6_3;
            public Int32 unknown7_3;
            public Int32 unknown8_3;
            public Int32 unknown9_3;
            public Int32 unknown10_3;
            public Int32 unknown11_3;
            public Int32 unknown12_3;
            public Int32 unknown13_3;
            public Int32 unknown14_3;
            public Int32 unknown15_3;
            public Int32 unknown16_3;
            public Int32 unknown1_4;
            public Int32 unknown2_4;
            public Int32 unknown3_4;
            public Int32 unknown4_4;
            public Int32 unknown5_4;
            public Int32 unknown6_4;
            public Int32 unknown7_4;
            public Int32 unknown8_4;
            public Int32 unknown9_4;
            public Int32 unknown10_4;
            public Int32 unknown11_4;
            public Int32 unknown12_4;
            public Int32 unknown13_4;
            public Int32 unknown14_4;
            public Int32 unknown15_4;
            public Int32 unknown16_4;
            public Int32 unknown1_5;
            public Int32 unknown2_5;
            public Int32 unknown3_5;
            public Int32 unknown4_5;
            public Int32 unknown5_5;
            public Int32 unknown6_5;
            public Int32 unknown7_5;
            public Int32 unknown8_5;
            public Int32 unknown9_5;
            public Int32 unknown10_5;
            public Int32 unknown11_5;
            public Int32 unknown12_5;
            public Int32 unknown13_5;
            public Int32 unknown14_5;
            public Int32 unknown15_5;
            public Int32 unknown16_5;
            public Int32 unknown1_6;
            public Int32 unknown2_6;
            public Int32 unknown3_6;
            public Int32 unknown4_6;
            public Int32 unknown5_6;
            public Int32 unknown6_6;
            public Int32 unknown7_6;
            public Int32 unknown8_6;
            public Int32 unknown9_6;
            public Int32 unknown10_6;
            public Int32 unknown11_6;
            public Int32 unknown12_6;
            public Int32 unknown13_6;
            public Int32 unknown14_6;
            public Int32 unknown15_6;
            public Int32 unknown16_6;
            public Int32 unknown1_7;
            public Int32 unknown2_7;
            public Int32 unknown3_7;
            public Int32 unknown4_7;
            public Int32 unknown5_7;
            public Int32 unknown6_7;
            public Int32 unknown7_7;
            public Int32 unknown8_7;
            public Int32 unknown9_7;
            public Int32 unknown10_7;
            public Int32 unknown11_7;
            public Int32 unknown12_7;
            public Int32 unknown13_7;
            public Int32 unknown14_7;
            public Int32 unknown15_7;
            public Int32 unknown16_7;
            public Int32 unknown1_8;
            public Int32 unknown2_8;
            public Int32 unknown3_8;
            public Int32 unknown4_8;
            public Int32 unknown5_8;
            public Int32 unknown6_8;
            public Int32 unknown7_8;
            public Int32 unknown8_8;
            public Int32 unknown9_8;
            public Int32 unknown10_8;
            public Int32 unknown11_8;
            public Int32 unknown12_8;
            public Int32 unknown13_8;
            public Int32 unknown14_8;
            public Int32 unknown15_8;
            public Int32 unknown16_8;
            public Int32 unknown1_9;
            public Int32 unknown2_9;
            public Int32 unknown3_9;
            public Int32 unknown4_9;
            public Int32 unknown5_9;
            public Int32 unknown6_9;
            public Int32 unknown7_9;
            public Int32 unknown8_9;
            public Int32 unknown9_9;
            public Int32 unknown10_9;
            public Int32 unknown11_9;
            public Int32 unknown12_9;
            public Int32 unknown13_9;
            public Int32 unknown14_9;
            public Int32 unknown15_9;
            public Int32 unknown16_9;
            public Int32 unknown1_10;
            public Int32 unknown2_10;
            public Int32 unknown3_10;
            public Int32 unknown4_10;
            public Int32 unknown5_10;
            public Int32 unknown6_10;
            public Int32 unknown7_10;
            public Int32 unknown8_10;
            public Int32 unknown9_10;
            public Int32 unknown10_10;
            public Int32 unknown11_10;
            public Int32 unknown12_10;
            public Int32 unknown13_10;
            public Int32 unknown14_10;
            public Int32 unknown15_10;
            public Int32 unknown16_10;
        }

        public MusicConditions(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<MusicConditionsTable>(data, ref offset, Count);
        }
    }
}
