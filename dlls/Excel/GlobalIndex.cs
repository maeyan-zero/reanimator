using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class GlobalIndex : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class GlobalIndexTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public String name;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String String;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String string_tugboat;

            public Int32 dataTable;//tbl
            Int32 unknown2;             // always 0
            Int32 unknown3;             // always 0
            Int32 unknown4;             // always 0
        }

        public GlobalIndex(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<GlobalIndexTable>(data, ref offset, Count);
        }
    }
}