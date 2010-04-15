using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class UnitEvents : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class UnitEventsTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public String name;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            byte[] unknown2;
            public Int32 recordPlayer;
            public Int32 recordMonster;
            public Int32 recordMissile;
            public Int32 recordItem;
            public Int32 recordObject;
        }

        public UnitEvents(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<UnitEventsTable>(data, ref offset, Count);
        }
    }
}
