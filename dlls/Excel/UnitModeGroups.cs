using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class UnitModeGroups : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class UnitModeGroupsTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public String name;

            public Int32 code;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public Int32 undefined;
        }

        public UnitModeGroups(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<UnitModeGroupsTable>(data, ref offset, Count);
        }
    }
}
