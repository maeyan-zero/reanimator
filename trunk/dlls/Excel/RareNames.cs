using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class RareNames : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class RareNamesTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 suffix;//bool
            public Int32 level;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string code;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public Int32[] types;
            public Int32 cond;//intptr
        }

        public RareNames(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<RareNamesTable>(data, ref offset, Count);
        }
    }
}
