using System;
using System.Runtime.InteropServices;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MonScaling
    {
        RowHeader header;
        public Int32 playerCount;
        public float playerVsMonsterIncrementPct0;
        public float playerVsMonsterIncrementPct1;
        public Int32 monsterVsPlayerIncrementPct;
        public Int32 experienceTotal;
        public Int32 treasurePerPlayer;
    }
}
