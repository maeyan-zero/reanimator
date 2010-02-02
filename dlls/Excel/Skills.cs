using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class Skills : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class SkillsTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public String stringId;
            public Int32 id;
            Int32 buffer;
            public Int32 unknown1;
            public Int32 unknown2;
            public Int32 unknown3;
            public Int32 unknown4;
            public Int32 unknown5;
            public Int32 unknown6;
            public Int32 unknown7;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public String usage1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public String usage2;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 596)]
            byte[] unknownData1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public String unknownString1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public String unknownString2;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1208)]
            byte[] unknownData2;
        }

        public Skills(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<SkillsTable>(data, ref offset, Count);
        }
    }
}
