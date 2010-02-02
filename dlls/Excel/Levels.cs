using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class Levels : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class LevelsTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string level;

            // is always zeros - assuming it's reserved, or a buffer or something.
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            byte[] reserved;

            public Int32 id;
            public Int32 unknown1;
            public Int32 unknown2;
            public Int32 unknown3;
            public Int32 unknown4;
            public Int32 unknown5;
            public Int32 unknown6;
            public Int32 unknown7;
            public Int32 unknown8;
            public Int32 unknown9;
            public Int32 unknown10;
            public Int32 unknown11;
            public Int32 unknown12;

            // a lot more ints following but can't be bothered

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 260)]
            byte[] unknownData1;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string map1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string map2;

            public Int32 unknown13;
            public Int32 unknown14;
            public float unknownFloat1;
            public float unknownFloat2;
            public Int32 unknown15;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 84)]
            byte[] unknownData2;
        }

        public Levels(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<LevelsTable>(data, ref offset, Count);
        }
    }
}
