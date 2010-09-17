using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MonScalingRow
    {
        ExcelFile.TableHeader header;

        public Int32 playerCount;
        public float playerVsMonsterIncrementPct0;
        public float playerVsMonsterIncrementPct1;
        public Int32 monsterVsPlayerIncrementPct;
        public Int32 experienceTotal;
        public Int32 treasurePerPlayer;
    }
}