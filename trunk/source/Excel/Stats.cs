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
        struct StatsTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] unknown;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szStringId;
            public Int16 id;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 430)]
            public byte[] unknownData;
        }

        List<StatsTable> stats;

        public Stats(byte[] data) : base(data)
        {
            stats = ExcelTables.ReadTables<StatsTable>(data, ref offset, Count);
        }

        public string GetStringFromId(int id)
        {
            foreach(StatsTable statsTable in stats)
            {
                if (statsTable.id == id)
                {
                    return statsTable.szStringId;
                }
            }

            return "NOT FOUND";
        }
    }
}
