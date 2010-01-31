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
            private string stringId;
            public string StringId
            {
                get { return stringId; }
                set { stringId = value; }
            }

            private Int16 id;
            public Int16 Id
            {
                get { return id; }
                set { id = value; }
            }

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 430)]
            public byte[] unknownData;
        }

        List<StatsTable> stats;

        public Stats(byte[] data) : base(data) { }

        public override object GetTableArray()
        {
            return stats.ToArray();
        }

        protected override void ParseTables(byte[] data)
        {
            stats = ExcelTables.ReadTables<StatsTable>(data, ref offset, Count);
        }

        public string GetStringFromId(int id)
        {
            foreach (StatsTable statsTable in stats)
            {
                if (statsTable.Id == id)
                {
                    return statsTable.StringId;
                }
            }

            return "NOT FOUND";
        }
    }
}
