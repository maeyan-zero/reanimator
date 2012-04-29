using System;
using System.Runtime.InteropServices;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class LevelScaling
    {
        RowHeader header;
        public Int32 levelDiff;
        public Int32 PlayerAttackMonsterDmg;
        public Int32 PlayerAttackMonsterExp;
        public Int32 MonsterAttackPlayerDmg;
        public Int32 PlayerAttackPlayerDmg;
    }
}
