using System;
using System.Runtime.InteropServices;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MonScaling
    {
        TableHeader header;
        public Int32 playerCount;
        public float playerVsMonsterIncrementPct0;
        public float playerVsMonsterIncrementPct1;
        public Int32 monsterVsPlayerIncrementPct;
        public Int32 experienceTotal;
        public Int32 treasurePerPlayer;
    }
}
