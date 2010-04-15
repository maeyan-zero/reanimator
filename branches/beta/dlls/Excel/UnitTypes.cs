using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class UnitTypes : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class UnitTypesTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public String type;
            public Int32 code;
            public Int32 isA0;
            public Int32 isA1;
            public Int32 isA2;
            public Int32 isA3;
            public Int32 isA4;
            public Int32 isA5;
            public Int32 isA6;
            public Int32 isA7;
            public Int32 isA8;
            public Int32 isA9;
            public Int32 isA10;
            public Int32 isA11;
            public Int32 isA12;
            public Int32 isA13;
            public Int32 isA14;
            public Int32 isA15;
            public Int32 String;//stridx
        }

        public UnitTypes(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<UnitTypesTable>(data, ref offset, Count);
        }
    }
}
