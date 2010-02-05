using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class MonScaling : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class MonScalingTable
        {
            TableHeader header;

            public Int32 playerCount;
            public float playerVsMonsterIncrementPct0;
            public float playerVsMonsterIncrementPct1;
            public Int32 monsterVsPlayerIncrementPct;
            public Int32 experienceTotal;
            public Int32 treasurePerPlayer;
        }

        public MonScaling(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<MonScalingTable>(data, ref offset, Count);
        }
    }
}
