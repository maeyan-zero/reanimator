using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class Faction : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class FactionTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 code;
            public Int32 displayString;//stridx
            public Int32 unitTypeStartStanding1;
            public Int32 levelDefStartStanding1;
            public Int32 startStanding1;
            public Int32 unitTypeStartStanding2;
            public Int32 levelDefStartStanding2;
            public Int32 startStanding2;
            public Int32 unitTypeStartStanding3;
            public Int32 levelDefStartStanding3;
            public Int32 startStanding3;
            public Int32 unitTypeStartStanding4;
            public Int32 levelDefStartStanding4;
            public Int32 startStanding4;
        }

        public Faction(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<FactionTable>(data, ref offset, Count);
        }
    }
}
