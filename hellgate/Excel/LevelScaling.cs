using System;
using System.Runtime.InteropServices;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class LevelScaling
    {
        TableHeader header;
        public Int32 levelDiff;
        public Int32 PlayerAttackMonsterDmg;
        public Int32 PlayerAttackMonsterExp;
        public Int32 MonsterAttackPlayerDmg;
        public Int32 PlayerAttackPlayerDmg;
    }
}
