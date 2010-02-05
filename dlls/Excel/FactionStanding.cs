using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class FactionStanding : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class FactionStandingTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 code;
            public Int32 displayString;//stridx
            public Int32 displayStringNumbers;//stridx
            public Int32 minScore;
            public Int32 maxScore;
            public Int32 mood;
        }

        public FactionStanding(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<FactionStandingTable>(data, ref offset, Count);
        }
    }
}
