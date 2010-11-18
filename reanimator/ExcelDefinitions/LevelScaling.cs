using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class LevelScalingRow
    {
        ExcelFile.TableHeader header;

        public Int32 levelDiff;
        public Int32 PlayerAttackMonsterDmg;
        public Int32 PlayerAttackMonsterExp;
        public Int32 MonsterAttackPlayerDmg;
        public Int32 PlayerAttackPlayerDmg;
    }
}