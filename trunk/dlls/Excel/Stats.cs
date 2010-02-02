using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class Stats : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class StatsTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] unknown;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string stringId;

            public Int16 id;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 430)]
            byte[] unknownData;
        }

        public Stats(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<StatsTable>(data, ref offset, Count);
        }

        public string GetStringFromId(int id)
        {
            foreach (StatsTable statsTable in tables)
            {
                if (statsTable.id == id)
                {
                    return statsTable.stringId;
                }
            }

            return "NOT FOUND";
        }
    }
}
